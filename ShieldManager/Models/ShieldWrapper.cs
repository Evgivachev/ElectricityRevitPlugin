namespace ShieldManager.Models;

using Autodesk.Revit.DB;
using CommonUtils;

public class ShieldWrapper : ElementWrapperBase<FamilyInstance>
{
    /// <inheritdoc />
    public ShieldWrapper(FamilyInstance initialInstance)
        : base(initialInstance)
    {
        Name = initialInstance.Name;
    }

    /// <summary>
    /// Имя щита
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Количество модулей в щите
    /// </summary>
    public int ModulesCount
    {
        get
        {
            var result = 0;
            var es = InitialInstance.MEPModel?.GetAssignedElectricalSystems();
            if (es is null)
            {
                return result;
            }

            foreach (var e in es)
            {
                var n1 = e.LookupParameter("Количество модулей ОУ1")?.AsDouble() ?? 0;
                var n2 = e.LookupParameter("Количество модулей ОУ2")?.AsDouble() ?? 0;
                result += (int)(n1 + n2);
            }

            return result;
        }
    }
}
