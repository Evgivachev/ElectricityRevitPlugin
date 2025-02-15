namespace Print.View.Test.Infrastructure;

using Application.Application;
using Application.Domain;

public class SettingsService : ISettingsService
{

    public async Task<ExportSettings> GetSettings()
    {
        await Task.Delay(1000);
        return new ExportSettings()
        {
            DwgSettings = new[]
            {
                "One",
                "Two",
            }
        };
    }
}
