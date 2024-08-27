namespace ElectricityRevitPlugin.UpdateParametersInCircuits;

using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using LossVoltageCalculator;

[Regeneration(RegenerationOption.Manual)]
[Transaction(TransactionMode.Manual)]
public class LossVoltageOfElectricalSystemExternalCommand : DefaultExternalCommand,
    IUpdaterParameters<ElectricalSystem>
{
    private readonly Guid _disableChangeGuid = new("be64f474-c030-40cf-9975-6eaebe087a84");
    private readonly Guid _lossVoltageParameterGuid = new("b4954a6d-3d42-44ff-b700-e308cf0fcc46");

    public string UpdateParameters(ElectricalSystem el)
    {
        var methodParameter = el.LookupParameter("Способ расчета потерь напряжения в цепи");
        if (methodParameter is null)
            throw new NullReferenceException("Не найден параметер \"Способ расчета потерь напряжения в цепи\"");
        var method = methodParameter.AsValueString();
        LossVoltageCalculator.LossVoltageCalculator lossVoltageCalculator = new StandardCalculator();
        if (method.StartsWith("*"))
        {
            var kParameter = el.LookupParameter("Коэффициент для потерь напряжения цепи");
            if (kParameter is null)
                throw new NullReferenceException("Не найден параметер \"Коэффициент для потерь напряжения цепи\"");
            var k = kParameter.AsDouble();
            lossVoltageCalculator = new RateCalculator(k);
        }

        var du0 = lossVoltageCalculator.CalculateLossVoltage(el);
        var lossVoltageParameter = el.get_Parameter(_lossVoltageParameterGuid);
        if (!lossVoltageParameter.IsReadOnly)
        {
            var flag = lossVoltageParameter.Set(du0);
        }

        return null;
    }

    protected override Result DoWork(ref string message, ElementSet elements)
    {
        var selectedElectricalSystems = UiDoc.Selection
            .GetElementIds()
            .Select(id => Doc.GetElement(id))
            .OfType<ElectricalSystem>();
        using (var tr = new Transaction(Doc, "Расчет потерь напряжения в цепях"))
        {
            tr.Start();
            foreach (var electricalSystem in selectedElectricalSystems)
            {
                try
                {
                    var isDisableChange = electricalSystem.get_Parameter(_disableChangeGuid)?.AsInteger() == 1;
                    if (isDisableChange)
                        continue;
                    var resultMessage = UpdateParameters(electricalSystem);
                }
                catch (Exception e)
                {
                    message += '\n';
                    message += e.Message;
                    message += electricalSystem.Id.ToString();
                    return Result.Failed;
                }
            }

            tr.Commit();
        }

        return Result.Succeeded;
    }
}
