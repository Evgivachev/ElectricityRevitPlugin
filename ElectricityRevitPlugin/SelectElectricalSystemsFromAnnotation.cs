﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SelectElectricalSystemsFromAnnotation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;
            var systems = new Dictionary<string,ElectricalSystem>();
            var systems1 = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .Cast<ElectricalSystem>();
            foreach (var system in systems1)
            {
                systems[system.Name] = system;
            }

            var selection = uiDoc.Selection.GetElementIds()
                .Select(x => doc.GetElement(x))
                .Select(x => x.LookupParameter("Номер цепи").AsString())
                .Where(x => x != null)
                .Select(x =>
                {
                    if (systems.ContainsKey(x))
                        return systems[x];
                    return null;
                })
                .Where(x => x != null);

            uiDoc.Selection.SetElementIds(selection.Select(x=>x.Id).ToArray());
            //using (var tr = new Transaction(doc))
            //{
            //    tr.Start("sds");
            //    foreach (var system in systems)
            //    {
            //        var isReserveParameter = system.LookupParameter("Резервная группа");
            //        var isControlParameter = system.LookupParameter("Контрольные цепи");

            //        var d1Id = system.LookupParameter("Отключающее устройство 1").AsElementId();
            //        var d1 = doc.GetElement(d1Id)?.Name;

            //        var d2Id = system.LookupParameter("Отключающее устройство 2").AsElementId();
            //        var d2 = doc.GetElement(d2Id)?.Name;


            //        var isReserve = d1?.Contains("Резерв") ?? false;
            //        var isControl = d2 == "Цепи управления";

            //        isReserveParameter.Set(isReserve ? 1 : 0);
            //        isControlParameter.Set(isControl ? 1 : 0);

            //    }
            //    tr.Commit();
            //}

            return result;

        }

    }

}
