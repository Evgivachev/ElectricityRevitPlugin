namespace PrintCmd.Infrastructure;

using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Print.Application.Application;
using Print.Application.Domain;

public class SettingsService(UIApplication uiApplication) : ISettingsService
{
    private Document Document => uiApplication.ActiveUIDocument.Document;

    public async Task<ExportSettings> GetSettings()
    {
        await Task.Delay(1);
        return new ExportSettings()
        {
            DwgSettings = ExportDWGSettings.ListNames(Document).ToArray()
        };
    }
}
