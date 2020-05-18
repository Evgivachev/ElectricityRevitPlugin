using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetLenghtForTubeOfElectricalSystemsExternalCommand : IExternalCommand, IUpdaterParameters<ElectricalSystem>
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка режима траектории электрической цепи на все устройства");
                    var electricalSystems = new FilteredElementCollector(doc)
                        .OfClass(typeof(ElectricalSystem))
                        .WhereElementIsNotElementType()
                        .OfType<ElectricalSystem>();
                    foreach (var el in electricalSystems)
                        SetLenghtForTubeOfElectricalSystems(el);

                    tr.Commit();

                }
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }
            finally
            {

            }
            return result;
        }

        private string SetLenghtForTubeOfElectricalSystems(ElectricalSystem el)
        {
            var name = el.Name;
            var numberOfCabeles = el.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
            //Длина кабелей для ОС
            //число в метрах
            var lengthForDiagrams = el.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
            //длина в миллиметрах
            var storeLengthForTube = UnitUtils.ConvertFromInternalUnits(el.get_Parameter(new Guid("25122ee0-d761-4a5f-af49-b507b64188e3")).AsDouble(), DisplayUnitType.DUT_METERS);
            //длина в миллиметрах
            var tubeLengthParam = el.LookupParameter("Длина труб для спецификации");
            var value = Math.Max(0, numberOfCabeles * (lengthForDiagrams + storeLengthForTube));
            tubeLengthParam.Set(UnitUtils.ConvertToInternalUnits(value, DisplayUnitType.DUT_METERS));
            return null;
        }

        public string UpdateParameters(ElectricalSystem el)
        {
            return SetLenghtForTubeOfElectricalSystems(el);
        }
    }
}
