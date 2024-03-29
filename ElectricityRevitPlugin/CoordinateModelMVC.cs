﻿namespace ElectricityRevitPlugin
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Extensions;

    public class CoordinateModelMvc
    {
        private readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private readonly int _elementsCount;
        private Element[] _elements;
        private bool _isMeterUnits = true;
        private double[] _rField;
        private bool _useShift = false;

        private double[] _xField;
        private double[] _yField;
        private double[] _zField;
        private int tolerance = 3;

        public CoordinateModelMvc(Element[] elements)
        {
            _elements = elements;
            _elementsCount = elements.Length;
            _xField = new double[_elementsCount];
            _yField = new double[_elementsCount];
            _zField = new double[_elementsCount];
            _rField = new double[_elementsCount];
            for (var i = 0; i < _elementsCount; i++)
            {
                var element = elements[i];
                if (!(element.Location is LocationPoint p))
                    throw new ArgumentException($"Location point of element {element.Id.IntegerValue} is null ");
                _xField[i] = Math.Round(p.Point.X, tolerance);
                _yField[i] = Math.Round(p.Point.Y, tolerance);
                _zField[i] = Math.Round(p.Point.Z, tolerance);
                var r = p.Rotation / Math.PI * 180;
                _rField[i] = Math.Round(r, tolerance);
            }
        }

        public string IsNotSimilar => "*РАЗЛИЧНЫЕ*";

        public string XField
        {
            get { return GetValueToField(_xField); }
            set
            {
                if (value == IsNotSimilar)
                    return;
                var doubleValue = double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
                if (IsMeterUnits)
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, UnitTypeId.Meters);
                _xField = Enumerable.Repeat(doubleValue, _xField.Length).ToArray();
                ModelChanged.Invoke(this);
            }
        }

        public string YField
        {
            get { return GetValueToField(_yField); }
            set
            {
                if (value == IsNotSimilar)
                    return;
                var doubleValue = double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
                if (IsMeterUnits)
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, UnitTypeId.Meters);
                _yField = Enumerable.Repeat(doubleValue, _yField.Length).ToArray();
                ModelChanged.Invoke(this);
            }
        }

        public string ZField
        {
            get { return GetValueToField(_zField, UseShift); }
            set
            {
                if (value == IsNotSimilar)
                    return;
                var doubleValue = double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
                if (IsMeterUnits)
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, UnitTypeId.Meters);
                _zField = Enumerable.Repeat(doubleValue, _zField.Length).ToArray();
                ModelChanged.Invoke(this);
            }
        }

        /// <summary>
        /// в градусах
        /// </summary>
        public string RField
        {
            get { return GetValueToRField(_rField); }
            set
            {
                if (value == IsNotSimilar)
                    return;
                var doubleValue = double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
                _rField = Enumerable.Repeat(doubleValue, _zField.Length).ToArray();
                ModelChanged.Invoke(this);
            }
        }

        public bool IsMeterUnits
        {
            get => _isMeterUnits;
            set
            {
                _isMeterUnits = value;
                ModelChanged.Invoke(this);
            }
        }

        public bool UseShift
        {
            get => _useShift;
            set
            {
                _useShift = value;
                ModelChanged.Invoke(this);
            }
        }

        public void SetCoordinate()
        {
            var points = new XYZ[_elementsCount];
            for (var i = 0; i < _elementsCount; i++)
            {
                XYZ point = new XYZ(_xField[i], _yField[i], _zField[i]);
                points[i] = point;
                _elements[i].SetElementCoordinate(point, true);
                if (UseShift)
                    _elements[i].SetInstallationHeightRelativeToLevel(point.Z, null, true);
                _elements[i].SetElementRotation(_rField[i], null, true);
            }
        }

        private string GetValueToRField(double[] rField)
        {
            var isSimilar = IsSimilar(rField, tolerance);
            if (!isSimilar)
                return IsNotSimilar;
            var result = rField.First();
            return Math.Round(result, tolerance).ToString(_cultureInfo);
        }

        public event Action<CoordinateModelMvc> ModelChanged;

        private string GetValueToField(double[] array, bool useShift = false)
        {
            var isSimilar = IsSimilar(array, tolerance);
            if (!isSimilar)
                return IsNotSimilar;
            var result = array.First();
            if (useShift)
            {
                result = _elements.First().GetInstallationHeightRelativeToLevel();
            }

            if (IsMeterUnits)
                result = UnitUtils.ConvertFromInternalUnits(result, UnitTypeId.Meters);
            return Math.Round(result, tolerance).ToString(_cultureInfo);
        }

        private bool IsSimilar(double[] coords, int tolerance)
        {
            if (coords.Length == 1)
                return true;
            var first = coords[0];
            foreach (var coord in coords)
            {
                if (Math.Abs(coord - first) > 1 / (10 * tolerance))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
