﻿namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using MoreLinq;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class UpdateCableManagementMethodExternalCommand : IExternalCommand, IUpdaterParameters<ElectricalSystem>
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;
        var uiDoc = uiApp.ActiveUIDocument;
        var doc = uiDoc.Document;
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
                electricalSystems.ForEach(el => UpdateParameters(el));
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
        //Способ прокладки кабелей для ОС
        var markParam = els.get_Parameter(new Guid("914fd7c8-80ed-4e93-9461-13e8c8fec57d"));
        var fromParam = els.LookupParameter("Способ прокладки для схем").AsString();
        markParam.Set(fromParam);
        return fromParam;
    }
}
