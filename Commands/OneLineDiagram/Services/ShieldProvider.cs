namespace Diagrams.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Models;

    public class ShieldProvider : IShieldsProvider
    {
        private readonly UIApplication _uiApplication;

        public ShieldProvider(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
        }

        public IEnumerable<Shield> GetShields()
        {
            return new FilteredElementCollector(_uiApplication.ActiveUIDocument.Document)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(x =>
                {
                    var uString = x.LookupParameter("Напряжение в щите")?.AsValueString()?.Split(' ')[0];
                    if (uString is null) return false;
                    var uFlag = double.TryParse(uString, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture,
                        out var u);
                    return uFlag && !(u < 200);
                })
                .OrderBy(x => x.Name)
                .Select(x => new Shield()
                {
                    Name = x.Name,
                    Id = x.Id.IntegerValue,
                    UniqueId = x.UniqueId
                })
                .ToArray();
        }
    }
}
