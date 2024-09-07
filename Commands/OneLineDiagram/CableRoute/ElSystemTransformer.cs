namespace Diagrams.CableRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;

    public class ElSystemTransformer
    {
        private readonly List<ICableTray> _elements;
        private List<XYZ> _points = new();

        public ElSystemTransformer(List<ICableTray> elements)
        {
            _elements = elements;
        }

        private void CalculatePoints()
        {
            _points = new List<XYZ>();
            var start = _elements.FirstOrDefault()?.GetPoints().First();
            _points.Add(start);
            for (var i = 0; i < _elements.Count - 1; i++)
            {
                var currentEl = _elements[i];
                var nextEl = _elements[i + 1];
                _points.AddRange(currentEl.GetPointsToICableTray(nextEl));
            }
        }

        private void TransformPoints()
        {
            var tolerance = 1e-9;
            if (_points.Count < 2)
                return;
            var result = new List<XYZ> { _points.First() };
            for (var i = 0; i < _points.Count - 1; i++)
            {
                if (i == _points.Count - 2)
                {
                }

                var current = _points[i];
                var next = _points[i + 1];
                if (Math.Abs(current.Y - 50.524934383) < 0.0001)
                {
                }

                if (Math.Abs(current.Z - next.Z) < tolerance)
                {
                    if ((current - next).GetLength() < tolerance)
                        continue;
                    result.Add(next);
                }
                else if (current.Z > next.Z)
                {
                    if (Math.Abs(next.X - current.X) > tolerance || Math.Abs(next.Y - current.Y) > tolerance)
                        result.Add(new XYZ(next.X, next.Y, current.Z));
                    result.Add(next);
                }
                else
                {
                    if (Math.Abs(next.X - current.X) > tolerance || Math.Abs(next.Y - current.Y) > tolerance)
                        result.Add(new XYZ(current.X, current.Y, next.Z));
                    result.Add(next);
                }
            }

            _points = result;
        }

        public List<XYZ> GetPoints()
        {
            CalculatePoints();
            TransformPoints();
            return _points;
        }
    }
}
