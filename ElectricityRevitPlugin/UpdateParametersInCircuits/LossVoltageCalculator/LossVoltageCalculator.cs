using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.UpdateParametersInCircuits.LossVoltageCalculator
{
    abstract class LossVoltageCalculator
    {
        internal abstract double CalculateLossVoltage(ElectricalSystem el);
    }
}
