﻿using Microsoft.Extensions.Logging;
using ManiaAPI.NadeoAPI;
using Microsoft.EntityFrameworkCore;
using UOTDBot.Models;

namespace UOTDBot;

internal sealed class TotdChecker
{
    private readonly NadeoLiveServices _nls;
    private readonly HttpClient _http;
    private readonly CarChecker _carChecker;
    private readonly AppDbContext _db;
    private readonly ILogger<TotdChecker> _logger;

    public TotdChecker(
        NadeoLiveServices nls,
        HttpClient http,
        CarChecker carChecker,
        AppDbContext db,
        ILogger<TotdChecker> logger)
    {
        _nls = nls;
        _http = http;
        _carChecker = carChecker;
        _db = db;
        _logger = logger;
    }

    public async Task<Map?> CheckAsync(int day, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for TOTD...");

        var totds = await _nls.GetTrackOfTheDaysAsync(length: 1, cancellationToken: cancellationToken);

        if (totds.MonthList.Length == 0)
        {
            _logger.LogError("No TOTD found (empty month list).");
            return null;
        }

        var month = totds.MonthList[0];

        if (month.Days.Length == 0)
        {
            _logger.LogCritical("No TOTD found (empty day list).");
            return null;
        }

        if (day > month.Days.Length)
        {
            _logger.LogCritical("No TOTD found (day out of range).");
            return null;
        }

        var dayInfo = month.Days[day - 1];

        if (string.IsNullOrEmpty(dayInfo.MapUid))
        {
            _logger.LogError("No TOTD found (empty map UID). Is Scheduler:StartTime too early?");
            return null;
        }

        _logger.LogInformation("Found TOTD day {MonthDay} (MapUid: {MapUid})", dayInfo.MonthDay, dayInfo.MapUid);
        _logger.LogDebug("TOTD details: {DayInfo}", day);

        var mapModel = await _db.Maps.FirstOrDefaultAsync(x => x.MapUid == dayInfo.MapUid, cancellationToken);

        if (mapModel is not null)
        {
            _logger.LogInformation("Map already exists in database (MapUid: {MapUid}). Nothing to report.", dayInfo.MapUid);
            return null;
        }

        _logger.LogInformation("Checking map details (MapUid: {MapUid})...", dayInfo.MapUid);

        var mapInfo = await _nls.GetMapInfoAsync(dayInfo.MapUid, cancellationToken);
        
        _logger.LogDebug("Map details: {MapInfo}", mapInfo);

        using var mapResponse = await _http.GetAsync(mapInfo.DownloadUrl, cancellationToken);

        mapResponse.EnsureSuccessStatusCode();

        using var mapStream = await mapResponse.Content.ReadAsStreamAsync(cancellationToken);

        _carChecker.CheckMap(mapStream);

        mapModel = new Map
        {
            MapId = mapInfo.MapId,
            MapUid = mapInfo.Uid,
            Name = mapInfo.Name,
            ThumbnailUrl = mapInfo.ThumbnailUrl,
            DownloadUrl = mapInfo.DownloadUrl,
            AuthorTime = mapInfo.AuthorTime,
            FileSize = (int)mapResponse.Content.Headers.ContentLength.GetValueOrDefault(),
            UploadedAt = mapInfo.UploadTimestamp,
            UpdatedAt = mapInfo.UpdateTimestamp,
        };

        await _db.Maps.AddAsync(mapModel, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return mapModel;
    }
}
