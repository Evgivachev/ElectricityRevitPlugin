namespace ElectricityRevitPlugin.GeneralSubject
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Extensions;

    public class DisconnectingDeviceParameterUpdater : CableParameterUpdater
    {
        public DisconnectingDeviceParameterUpdater()
            : base()
        {
        }

        public DisconnectingDeviceParameterUpdater(Element fromElement)
            : base(fromElement)
        {
            ParametersDictionary = new Dictionary<dynamic, dynamic>
            {
                // Количество фаз
                { BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES, new Guid("20497dfd-f758-48d3-9652-9d4b22880dfd") }
            };
            FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>
            {
                {
                    "Тип ОУ1", (system) =>
                    {
                        var es = (ElectricalSystem)system;
                        var p = es.LookupParameter("ОУ1").GetValueDynamic();
                        return p;
                    }
                },
                {
                    "Тип ОУ2", (system) =>
                    {
                        var es = (ElectricalSystem)system;
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
