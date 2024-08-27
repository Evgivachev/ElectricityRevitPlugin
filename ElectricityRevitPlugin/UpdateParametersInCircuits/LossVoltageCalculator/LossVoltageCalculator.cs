namespace ElectricityRevitPlugin.UpdateParametersInCircuits.LossVoltageCalculator;

using Autodesk.Revit.DB.Electrical;

abstract class LossVoltageCalculator
{
    internal abstract double CalculateLossVoltage(ElectricalSystem el);
}
