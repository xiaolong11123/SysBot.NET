using Kook;
using Kook.Commands;
using PKHeX.Core;
using System.Threading.Tasks;

namespace SysBot.Pokemon.Kook;

public class LegalityCheckModule : ModuleBase<SocketCommandContext>
{
    [Command("lc"), Alias("check", "validate", "verify")]
    [Summary("Verifies the attachment for legality.")]
    public async Task LegalityCheck()
    {
        var attachments = Context.Message.Attachments;
        foreach (var att in attachments)
            await LegalityCheck(att, false).ConfigureAwait(false);
    }

    [Command("lcv"), Alias("verbose")]
    [Summary("Verifies the attachment for legality with a verbose output.")]
    public async Task LegalityCheckVerbose()
    {
        var attachments = Context.Message.Attachments;
        foreach (var att in attachments)
            await LegalityCheck(att, true).ConfigureAwait(false);
    }

    private async Task LegalityCheck(IAttachment att, bool verbose)
    {
        var download = await NetUtil.DownloadPKMAsync(att).ConfigureAwait(false);
        if (!download.Success)
        {
            await ReplyTextAsync(download.ErrorMessage?? "No error message").ConfigureAwait(false);
            return;
        }

        var pkm = download.Data!;
        var la = new LegalityAnalysis(pkm);

        var card = new CardBuilder()
            .AddModule(new SectionModuleBuilder().WithText("Here's the legality report!"))
            .AddModule(new SectionModuleBuilder().WithText($"Legality Report for {download.SanitizedFileName}:"))
            .AddModule(new SectionModuleBuilder().WithText(la.Valid ? "Valid" : "Invalid"))
            .AddModule(new SectionModuleBuilder().WithText(la.Report(verbose)))
            .Build();

        await ReplyCardAsync(card).ConfigureAwait(false);
    }
}
