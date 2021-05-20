using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;
using RevitParametersCodeGenerator;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;

            var selection = uiDoc.Selection;
            var selectedEl = selection.GetElementIds().Select(x => doc.GetElement(x));


            var systems = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .Cast<ElectricalSystem>();
            var heads = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_GenericAnnotation)
                .WhereElementIsNotElementType()
                .Where(x => x.GetTypeId() == new ElementId(17172689));

            var shieldDict = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .WhereElementIsNotElementType()
                .ToDictionary(s => s.UniqueId);
            using (var tr = new Transaction(doc))
            {
                tr.Start("sds");
                foreach (var head in heads)
                {
                    var lParameter = head.LookupParameter("Длина линии в щитах");
                    var shieldGuid= head.get_Parameter(SharedParametersFile.ID_Elektricheskogo_SHCHita)?.AsString();
                    if(shieldGuid is null || !shieldDict.ContainsKey(shieldGuid))
                        continue;
                    var shield = shieldDict[shieldGuid] as FamilyInstance;
                    MessageBox.Show(shield.Name + "\n"+shield.Id + "\n" + head.Id);
                    var powerSystem = shield.GetPowerElectricalSystem();
                    var l = powerSystem.get_Parameter(SharedParametersFile.Dlina_Kabeley_Dlya_OS).AsDouble();
                    l = UnitUtils.ConvertFromInternalUnits(l, DisplayUnitType.DUT_METERS);
                    lParameter.Set(l.ToString("F2"));
                }
                


                tr.Commit();
            }

            return result;

        }
    }
}
