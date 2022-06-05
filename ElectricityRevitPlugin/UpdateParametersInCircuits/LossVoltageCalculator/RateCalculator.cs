namespace ElectricityRevitPlugin.UpdateParametersInCircuits.LossVoltageCalculator
{
    using Autodesk.Revit.DB.Electrical;

    class RateCalculator : StandardCalculator
    {
        private double _k;

        internal RateCalculator(double k)
        {
            _k = k;
        }

        internal override double CalculateLossVoltage(ElectricalSystem el)
        {
            var standard = base.CalculateLossVoltage(el);
            return _k * standard;
        }
    }
}
