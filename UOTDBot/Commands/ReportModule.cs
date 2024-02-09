﻿using Discord;
using Discord.Interactions;

namespace UOTDBot.Commands;

[Group("report", "UOTD report management.")]
public sealed class ReportModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly AppDbContext _db;

    public ReportModule(AppDbContext db)
    {
        _db = db;
    }

    [SlashCommand("subscribe", "Subscribe channel to UOTD reports.")]
    public async Task Subscribe(IChannel? other = default)
    {
        var channel = other ?? Context.Channel;

        await RespondAsync(embed: new EmbedBuilder()
            .WithDescription($"TOTDs that fit the UOTD rules will be reported in {MentionUtils.MentionChannel(channel.Id)}.").Build(),
            ephemeral: true);

        if (channel is IMessageChannel msgChannel)
        {
            await msgChannel.SendMessageAsync("I spam reports here now, just notifying");
        }

        //_db.ReportChannels.Add(new ReportChannel { ChannelId = channel.Id });
    }

    [SlashCommand("unsubscribe", "Unsubscribe channel from UOTD reports.")]
    public async Task Unsubscribe()
    {
        await RespondAsync(embed: new EmbedBuilder()
            .WithDescription($"TOTDs that fit the UOTD rules will no longer be reported.").Build(),
            ephemeral: true);
    }

    [SlashCommand("dm", "Subscribe to UOTD reports in DMs.")]
    public async Task Dm()
    {
        await Context.User.SendMessageAsync("I spam DMs now");
    }

    [SlashCommand("list", "Gets the list of UOTD reports, including their map name.")]
    public async Task List(IChannel? other = default)
    {
        var channel = other ?? Context.Channel;
    }
}
