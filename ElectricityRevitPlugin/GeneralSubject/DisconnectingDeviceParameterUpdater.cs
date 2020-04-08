using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using ElectricityRevitPlugin.Extensions;

namespace ElectricityRevitPlugin.GeneralSubject
{
    public class DisconnectingDeviceParameterUpdater : CableParameterUpdater
    {
        public DisconnectingDeviceParameterUpdater() :base()
        { }
        public DisconnectingDeviceParameterUpdater(Element fromElement) : base(fromElement)
        {
            ParametersDictionary = new Dictionary<dynamic, dynamic>();
            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                {
                    "Тип ОУ1", (system) =>
                    {
                        var es = (ElectricalSystem) system;
                        var p = es.LookupParameter("ОУ1").GetValueDynamic();
                        return p;
                    }
                },
                {
                    "Тип ОУ2", (system) =>
                    {
                        var es = (ElectricalSystem) system;
                        var p = es.LookupParameter("ОУ2").GetValueDynamic();
                        return p;
                    }
                }
            };
        }
        public override FamilyInstance InsertInstance(FamilySymbol familySymbol, XYZ xyz)
        {
            var instance = Doc.Create.NewFamilyInstance(xyz, familySymbol, Doc.ActiveView);
            return instance;
        }
    }
}
