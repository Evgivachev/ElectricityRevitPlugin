using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldPanel.ViewOfDevicesOfShield
{
    public static class DoubleExtension
    {
        public static double FromFootToMillimeters(this double n)
        {
            return n * 0.3048 * 1000;
        }
        public static double ConvertVolts(this double n)
        {
            return n * 0.3048 * 0.3048;
        }
    }
}
