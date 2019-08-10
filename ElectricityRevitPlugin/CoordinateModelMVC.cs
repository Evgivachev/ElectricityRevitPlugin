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

            for (var i = 0; i < _elementsCount; i++)
            {
                var element = elements[i];
                var p = element.Location as LocationPoint;
                if (p is null)
                    throw new ArgumentException($"Location point of element {element.Id.IntegerValue} is null ");
                _xField[i] = Math.Round(p.Point.X, tolerance);
                _yField[i] = Math.Round(p.Point.Y, tolerance);
                _zField[i] = Math.Round(p.Point.Z, tolerance);
            }
        }

        private readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private int tolerance = 3;
        private Element[] _elements;
        private readonly int _elementsCount;
        private string IsNotSimilar => "*РАЗЛИЧНЫЕ*";


        public string XField
        {
            get { return GetValueToField(_xField); }
            set
            {
                var doubleValue = double.Parse(value);
                _xField = Enumerable.Repeat(doubleValue,_xField.Length).ToArray();

            }
        }

        public string YField
        {
            get { return GetValueToField(_yField); }
            set
            {
                var doubleValue = double.Parse(value);
                _yField = Enumerable.Repeat(doubleValue, _yField.Length).ToArray();

            }
        }

        public string ZField
        {
            get { return GetValueToField(_zField,UseShift); }
            set
            {
                var doubleValue = double.Parse(value);
                _zField = Enumerable.Repeat(doubleValue, _zField.Length).ToArray();

            }
        }

        private double[] _xField;
        private double[] _yField;
        private double[] _zField;
        public bool IsMeterUnits = true;
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

        private string GetValueToField(double[] array,bool useShift = false)
        {
            var isSimilar = IsSimilar(array, tolerance);
            if (!isSimilar)
                return IsNotSimilar;
            var result = array.First();
            if(useShift)
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
                if (Math.Abs(coord - first) > tolerance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}