﻿namespace UOTDBot.Models;

public sealed class MapFeatures
{
    public required string DefaultCar { get; set; }
    public List<string> Gates { get; } = [];
    public Dictionary<string, int>? CarDistribution { get; set; }
    public float? NonStadiumDistribution { get; set; }
}
