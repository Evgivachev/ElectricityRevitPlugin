namespace ShieldManager;

using System;
using System.Linq;
using Abstractions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommonUtils.Extensions;
using RxBim.Di;
using ViewModels;
using Views;

/// <inheritdoc cref="Autodesk.Revit.UI.IExternalCommand" />
public class Cmd : IExternalCommand, IExternalCommandAvailability
{
    /// <inheritdoc />
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            var container = new SimpleInjectorContainer();
            container.AddBaseRevitDependences(commandData);
            new Config().Configure(container);
            var doc = container.GetService<Document>();
            var shieldsProvider = container.GetService<IShieldsProvider>();
            using var transactionGroup = new TransactionGroup(doc, "trGrName");
            if (TransactionStatus.Started == transactionGroup.Start())
            {
                var shields = shieldsProvider
                    .GetShields()
                    .OrderBy(x => x.Name)
                    .ToArray();
                if (new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_Schedules)
                        .First(x => x.Name == "* Оболочки щитов") is not ViewSchedule schedule)
                {
                    throw new Exception("Не удалось найти спецификацию \"*Оболочки щитов\"");
                }

                var catalog = new FilteredElementCollector(doc, schedule.Id)
                    .ToArray();
                var selection = container.GetService<Selection>().GetElementIds();
                var selectionElements = selection.Select(x => x.IntegerValue).ToHashSet();
                var viewModel = new ShieldsManagerViewModel();
                var view = new SelectModelOfShieldWPF(viewModel);
                view.ShowDialog();
            }

            transactionGroup.Assimilate();
            return Result.Succeeded;
        }
        catch (Exception e)
        {
            message += e.ToString();
            return Result.Failed;
        }
    }

    /// <inheritdoc />
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return applicationData.ActiveUIDocument?.Document is not null;
    }
}
