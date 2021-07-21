using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits.LossVoltageCalculator
{
    class RateCalculator :StandardCalculator
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
