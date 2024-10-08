﻿namespace ElectricityRevitPlugin;

using System;
using System.Linq;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommonUtils.Helpers;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
class Tepm9 : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var selection = UiDoc.Selection;
        var panelFromReference = selection.PickObject(ObjectType.Element, "First");
        var panelToReference = selection.PickObject(ObjectType.Element, "Second");
        var panelFrom = Doc.GetElement(panelFromReference) as FamilyInstance;
        var panelTo = Doc.GetElement(panelToReference) as FamilyInstance;
        var sortedSystem = panelFrom?.MEPModel.GetAssignedElectricalSystems()
            .OrderBy(s =>
            {
                var gg = s.get_Parameter(SharedParametersFile.Nomer_Gruppy_Po_GOST).AsString();
                var n = gg.Split(".-".ToCharArray()).Last();
                return int.Parse(n);
            });
        using (var tr = new Transaction(Doc, "Переподключение цепей"))
        {
            tr.Start();
            foreach (var elS in sortedSystem)
            {
                try
                {
                    elS.SelectPanel(panelTo);
                }
                catch (Exception e)
                {
                    MessageBox.Show(elS.Name + "\n" + e.Message + '\n' + e.StackTrace);
                    throw;
                }
            }

            tr.Commit();
        }

        return Result.Succeeded;
    }
}
