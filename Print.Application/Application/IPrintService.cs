namespace Print.Application.Application;

using Domain;

public interface IPrintService
{
    Task Print(PrintSettingsOut exportSettingsOut);
    Task Export(ExportSettingsOut exportSettingsOut);
}