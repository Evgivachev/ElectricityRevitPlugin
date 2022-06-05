namespace Diagrams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PhaseDistribution
    {
        private string[] _namesOfPhase = new[]
        {
            "L1",
            "L2",
            "L3"
        };

        public string ThreePhases { get; } = "L1,L2,L3";

        public double[] LoadOnPhases { get; } = new double[3];

        public string AddCurrent(double current, int phaseCount)
        {
            if (!(phaseCount == 1 || phaseCount == 3))
                throw new Exception();
            if (phaseCount == 3)
            {
                for (var i = 0; i < LoadOnPhases.Length; i++)
                {
                    LoadOnPhases[i] += current;
                }

                return ThreePhases;
            }

            var minCurrentPhaseIndex = GetIndexMinValue(LoadOnPhases);
            LoadOnPhases[minCurrentPhaseIndex] += current;
            return _namesOfPhase[minCurrentPhaseIndex];
        }

        public double GetPhacesImbalance()
        {
            var max = LoadOnPhases.Max();
            var min = LoadOnPhases.Min();
            if (min != 0 && max != 0)
            {
                var dI = Math.Round(1 - min / max, 2);
                return dI;
            }
            else
                return 0.0;
        }

        private static int GetIndexMinValue(IReadOnlyList<double> array)
        {
            var min = double.MaxValue;
            var index = 0;
            for (var i = 0; i < array.Count; i++)
            {
                var value = array[i];
                if (value >= min) continue;
                min = value;
                index = i;
            }

            return index;
        }
    }
}
