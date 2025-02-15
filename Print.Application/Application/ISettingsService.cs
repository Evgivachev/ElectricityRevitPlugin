namespace Print.Application.Application;

using Domain;

public interface ISettingsService
{
    Task<ExportSettings> GetSettings();
}
