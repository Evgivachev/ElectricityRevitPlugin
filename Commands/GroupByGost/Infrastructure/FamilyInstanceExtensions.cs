namespace GroupByGost.Infrastructure;

using Autodesk.Revit.DB;
using Domain;

public static class FamilyInstanceExtensions
{
    public static Shield ToShield(this FamilyInstance shield, ElectricalCircuit[] circuits)
    {
        var prefix = shield.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX).AsString();
        var separator = shield.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX_SEPARATOR).AsString();
        return  new Shield(prefix, separator, circuits);
    }
}
