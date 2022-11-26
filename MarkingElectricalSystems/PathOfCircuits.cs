namespace MarkingElectricalSystems;

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

public class PathOfCircuits
{
    //с точностью 1е-9 не работает
    private const double Tolerance = 1e-3;
    public List<XYZ> Points { get; } = new List<XYZ>();
    private XYZ _lastPoint = null;
    public int Count => Points.Count;

    private void Add(XYZ p)
    {
        if (_lastPoint is null)
        {
            Points.Add(p);
            _lastPoint = p;
            return;
        }

        if (OnSameLevel(_lastPoint, p))
        {
            if (LengthLessTolerance(p, _lastPoint))
            {
                return;
            }
            else
            {
                var dx = Math.Abs(_lastPoint.X - p.X);
                var dy = Math.Abs(_lastPoint.Y - p.Y);
                if (dx > Tolerance && dy > Tolerance)
                {
                    Add(dx > dy ? new XYZ(p.X, _lastPoint.Y, p.Z) : new XYZ(_lastPoint.X, p.Y, p.Z));
                }

                Points.Add(p);
                _lastPoint = p;
                return;
            }
        }
        else if (IsVertical(_lastPoint, p))
        {
            Points.Add(p);
            _lastPoint = p;
            return;
        }
        else
        {
            var p1 = new XYZ(_lastPoint.X, _lastPoint.Y, p.Z);
            Add(p1);
            Add(p);
        }


    }

    private bool LengthLessTolerance(XYZ p1, XYZ p2)
    {
        return p1.Subtract(p2).GetLength() < Tolerance;
    }
    private bool LengthLessTolerance(double p1, double p2)
    {
        return Math.Abs(p1 - p2) < Tolerance;
    }

    private bool OnSameLevel(XYZ p1, XYZ p2)
    {
        return Math.Abs(p1.Z - p2.Z) < Tolerance;
    }

    private bool IsVertical(XYZ p1, XYZ p2)
    {
        var b1 = Math.Abs(p1.X - p2.X) < Tolerance;
        var b2 = Math.Abs(p1.Y - p2.Y) < Tolerance;
        return b1 && b2;
    }

    public void Add(FamilyInstance f)
    {
        if (f is null)
            throw new NullReferenceException();
        var point = (f.Location as LocationPoint)?.Point;
        Add(point);
    }

    public void Add(FamilyInstance f, double shift)
    {
        if (f is null)
            throw new NullReferenceException();
        var point = (f.Location as LocationPoint)?.Point;
        Add(point, shift);
    }

    public void Add(XYZ p, double z)
    {
        if (_lastPoint is null)
        {
            Add(p);
            return;
        }

        if (_lastPoint.Subtract(p).IsZeroLength())
            return;
        if (Math.Abs(p.Z - z) < Tolerance)
        {
            Add(p);
            return;
        }
        else
        {
            Add(new XYZ(_lastPoint.X, _lastPoint.Y, z));
            Add(new XYZ(p.X, p.Y, z));
            Add(p);
        }
    }
}