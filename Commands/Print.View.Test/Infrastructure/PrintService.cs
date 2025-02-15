namespace Print.View.Test.Infrastructure;

using Application.Application;
using Application.Domain;

public class PrintService : IPrintService
{

    public Task Print(PrintSettingsOut printSettingsOut)
    {
        return Task.CompletedTask;
    }
    public async Task Export(ExportSettingsOut exportSettingsOut)
    {
        await Task.Delay(1);
        Console.WriteLine(exportSettingsOut);
    }
}
