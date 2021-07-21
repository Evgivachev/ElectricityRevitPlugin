using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    class SetLoadNameForElectricalSystemsExternalCommand : DefaultExternalCommand, IUpdaterParameters<ElectricalSystem>
    {
        private Guid _loadNameGuid = new Guid("2e466686-a5bd-4329-9427-d0fa03e8742d");
        private Guid _isReserveGroupGuid = new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348");
        private Guid _isControlCircuit = new Guid("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee");
        protected override Result DoWork(ref string message, ElementSet elements)
        {

            var electricalSystems = UiDoc.Selection.GetElementIds()
                .Select(x => Doc.GetElement(x))
                .OfType<ElectricalSystem>();
            using (var tr = new Transaction(Doc))
            {
                tr.Start("Установка Наименование нагрузки для электрической цепи");

                foreach (var el in electricalSystems)
                {
                    UpdateParameters(el);
                }
                tr.Commit();
            }
            return Result.Succeeded;
        }

        public string UpdateParameters(ElectricalSystem el)
        {
             var isReserveGroup = el.get_Parameter(_isReserveGroupGuid).AsInteger() == 1;
            var isControlCircuit = el.get_Parameter(_isControlCircuit).AsInteger() == 1;
            string loadName;
            var loadNameParameter = el.get_Parameter(_loadNameGuid);
            if (isReserveGroup)
                loadName = "Резерв";
            else if (isControlCircuit)
                loadName = "Цепи управления";
            else
                loadName = el.GetLoadName();
            loadNameParameter.Set(loadName);
            return null;
        }
    }
}
