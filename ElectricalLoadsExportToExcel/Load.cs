using static System.Math;

namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.Collections.Generic;

    public class Load
    {
        public Load(Load load)
            : this(load.Classification, load.P, load.CosPhi)
        {
            Ks = load.Ks;
            Count = load.Count;
        }

        public Load(string cl, double p, double cosPhi)
        {
            Classification = cl;
            P = p;
            CosPhi = cosPhi;
        }

        public Load(string name, double ks)
        {
            Classification = name;
            Ks = ks;
        }

        public string Classification { get; set; }
        public double P { get; set; }
        public double CosPhi { get; set; }
        public double Ks { get; set; } = 1;
        public int Count { get; set; } = 1;

        public static Load operator +(Load load, Load otherLoad)
        {
            var phi0 = Acos(load.CosPhi);
            var q0 = load.P * Tan(phi0);
            var phi1 = Acos(otherLoad.CosPhi);
            var q1 = otherLoad.P * Tan(phi1);
            var q = q0 + q1;
            var p = load.P + otherLoad.P;
            var s = Sqrt(p * p + q * q);
            var cosPhi = p / s;
            load.P = p;
            load.CosPhi = cosPhi;
            load.Count += otherLoad.Count;
            if (p is double.NaN || cosPhi is double.NaN)
            {
            }

            return load;
        }

        public override string ToString()
        {
            return $"{Classification} {P} {CosPhi} {Ks} {Count}";
        }
    }

    public class LoadComparerByName : IComparer<Load>
    {
        public int Compare(Load x, Load y)
        {
            if (x is null || y is null)
            {
                throw new NullReferenceException();
            }

            return String.Compare(x.Classification, y.Classification, StringComparison.Ordinal);
        }
    }
}
