using Kook.Commands;
using PKHeX.Core;
using System.Threading.Tasks;

namespace SysBot.Pokemon.Kook;

public class LegalizerModule<T> : ModuleBase<SocketCommandContext> where T : PKM, new()
{
    [Command("legalize"), Alias("alm")]
    [Summary("Tries to legalize the attached pkm data.")]
    public async Task LegalizeAsync()
    {
        var attachments = Context.Message.Attachments;
        foreach (var att in attachments)
            await Context.Channel.ReplyWithLegalizedSetAsync(att).ConfigureAwait(false);
    }

    [Command("convert"), Alias("showdown")]
    [Summary("Tries to convert the Showdown Set to pkm data.")]
    [Priority(1)]
    public Task ConvertShowdown([Summary("Generation/Format")] byte gen, [Remainder][Summary("Showdown Set")] string content)
    {
        return Context.Channel.ReplyWithLegalizedSetAsync(content, gen);
    }

    [Command("convert"), Alias("showdown")]
    [Summary("Tries to convert the Showdown Set to pkm data.")]
    [Priority(0)]
    public Task ConvertShowdown([Remainder][Summary("Showdown Set")] string content)
    {
        return Context.Channel.ReplyWithLegalizedSetAsync<T>(content);
    }
}
