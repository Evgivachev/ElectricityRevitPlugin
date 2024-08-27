namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class UpdateCablesMarkExternalCommand : IExternalCommand, IUpdaterParameters<ElectricalSystem>
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
        var app = uiApp.Application;
        var result = Result.Succeeded;
        var electricalSystems = new FilteredElementCollector(doc)
            .OfClass(typeof(ElectricalSystem))
            .WhereElementIsNotElementType()
            .OfType<ElectricalSystem>();
        try
        {
            using (var tr = new Transaction(doc))
            {
                tr.Start("UpdateParametersOfElectricalSystem");
                foreach (var el in electricalSystems)
                    UpdateParameters(el);
                tr.Commit();
            }
        }
        catch (Exception e)
        {
            message += e.Message + '\n' + e.StackTrace;
            result = Result.Failed;
        }

        return result;
    }

    public string UpdateParameters(ElectricalSystem els)
    {
        var number = els.CircuitNumber;
        //Марка кабелей для выносок
        var markParam = els.get_Parameter(new Guid("78e8268c-f1d6-46c2-8bd9-8c0c0590868a"));
        var nCables = els.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        var cableMark = els.LookupParameter("Тип изоляции").AsString();
        var nConduits = els.LookupParameter("Кол-во жил").AsDouble();
        var crossSection = els.LookupParameter("Сечение кабеля").AsDouble();
        var tube = els.LookupParameter("Способ прокладки для схем").AsString();
        var result = new StringBuilder();
        if (nCables > 1)
            result.Append((int)nCables + "x");
        result.Append(cableMark + " ");
        result.Append($"{nConduits}x{crossSection} мм\u00B2");
        var resultStr = result.ToString();
        markParam.Set(resultStr);
        return resultStr;
    }
}
