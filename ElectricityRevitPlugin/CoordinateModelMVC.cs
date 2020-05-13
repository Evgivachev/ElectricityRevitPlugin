using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityRevitPlugin
{
    public class CoordinateModelMvc
    {
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
        private readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private int tolerance = 3;
        private Element[] _elements;
        private readonly int _elementsCount;
        public string IsNotSimilar => "*РАЗЛИЧНЫЕ*";
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

        public string XField
        {
            get { return GetValueToField(_xField); }
            set
            {
                if (value == IsNotSimilar)
                    return;
                var doubleValue = double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
                if (IsMeterUnits)
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, DisplayUnitType.DUT_METERS);
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
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, DisplayUnitType.DUT_METERS);
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
                    doubleValue = UnitUtils.ConvertToInternalUnits(doubleValue, DisplayUnitType.DUT_METERS);
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

        private string GetValueToRField(double[] rField)
        {
            var isSimilar = IsSimilar(rField, tolerance);
            if (!isSimilar)
                return IsNotSimilar;
            var result = rField.First();
            return Math.Round(result, tolerance).ToString(_cultureInfo);
        }

        private double[] _xField;
        private double[] _yField;
        private double[] _zField;
        private double[] _rField;
        private bool _isMeterUnits = true;
        public bool IsMeterUnits
        {
            get => _isMeterUnits;
            set
            {
                _isMeterUnits = value;
                ModelChanged.Invoke(this);
            }
        }
        private bool _useShift = false;
        public bool UseShift
        {
            get => _useShift;
            set
            {
                _useShift = value;
                ModelChanged.Invoke(this);

            }
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
                result = UnitUtils.ConvertFromInternalUnits(result, DisplayUnitType.DUT_METERS);
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