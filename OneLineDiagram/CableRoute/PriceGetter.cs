namespace Diagrams.CableRoute
{
    using System.Linq;

    public class PriceGetter
    {
        public double GetPrice(ICableTray ct1, ICableTray ct2)
        {
            if (ct1.Id == 20404656 && ct2.Id == 19969580)
            {
            }

            var s1 = ct1.GetSpace();
            var s2 = ct2.GetSpace();
            var d = ct1.DistanceTo(ct2);
            if (ct1 is MyCableTray && ct2 is MyCableTray)
            {
                var cp1 = ct1.GetPoints().Aggregate((x, y) => (x + y) / 2);
                var cp2 = ct2.GetPoints().Aggregate((x, y) => (x + y) / 2);
                d = cp1.DistanceTo(cp2);
                return d / 2;
            }
            else if (ct1 is MyCableTray || ct2 is MyCableTray)
            {
                return d;
            }

            if (s1 == null && s2 == null)
            {
                return d;
            }
            else if (s1 == null || s2 == null)
            {
                d += 5;
            }
            else if (s1?.Id.IntegerValue != s2?.Id.IntegerValue)
                d += 5;

            return d;
        }
    }
}
