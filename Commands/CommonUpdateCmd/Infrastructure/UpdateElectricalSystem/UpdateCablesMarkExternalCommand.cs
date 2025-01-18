namespace CommonUpdateCmd.Infrastructure.UpdateElectricalSystem;

using System;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using CommonUtils.Extensions;
using CommonUtils.Helpers;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class UpdateCablesMarkExternalCommand : IUpdaterParameters<ElectricalSystem>, IUpdaterParameters<FamilyInstance>
{
    public string UpdateParameters(ElectricalSystem els)
    {
        var markParam = els.get_Parameter(SharedParametersFile.Marka_Kabeley_Dlya_Vynosok);
        var nCables = els.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        var cableMark = els.LookupParameter("Тип изоляции").AsString();
        var nConduits = els.LookupParameter("Кол-во жил").AsDouble();
        var crossSection = els.LookupParameter("Сечение кабеля").AsDouble();
        els.LookupParameter("Способ прокладки для схем").AsString();
        var result = new StringBuilder();
        if (nCables > 1)
            result.Append((int)nCables + "x");
        result.Append(cableMark + " ");
        result.Append($"{nConduits}x{crossSection} мм\u00B2");
        var resultStr = result.ToString();
        markParam.Set(resultStr);
        return resultStr;
    }

    public string UpdateParameters(FamilyInstance familyInstance)
    {
        var powerSystem = familyInstance.GetPowerElectricalSystem();
        if (powerSystem == null)
            return string.Empty;

        var markParam = familyInstance.get_Parameter(SharedParametersFile.Marka_Kabeley_Dlya_Vynosok);
        var mark = GetMark(powerSystem);
        markParam?.Set(mark);
        return mark;
    }

    private string GetMark(ElectricalSystem els)
    {
        var nCables = els.LookupParameter("Кол-во кабелей (провод) в одной группе").AsDouble();
        var cableMark = els.LookupParameter("Тип изоляции").AsString();
        var nConduits = els.LookupParameter("Кол-во жил").AsDouble();
        var crossSection = els.LookupParameter("Сечение кабеля").AsDouble();
        els.LookupParameter("Способ прокладки для схем").AsString();
        var result = new StringBuilder();
        if (nCables > 1)
            result.Append((int)nCables + "x");
        result.Append(cableMark + " ");
        result.Append($"{nConduits}x{crossSection} мм\u00B2");
        var resultStr = result.ToString();
        return resultStr;
    }
}
