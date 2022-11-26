namespace ShieldManager.ViewOfDevicesOfShield
{
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Electrical;
    using Extensions;

    public class ShieldProcessing
    {
        private readonly ElectricalSystem[] _assignedElectricalSystems;
        private readonly double _countOfModulsOfShield;
        private readonly double _heightOfShield;

        private readonly FamilyInstance _shield;
        private readonly double _widthOfShield;
        private readonly double _lengthOfInputDevice = 85;

        private readonly ParametersOfShield _parameters;
        private readonly double _widthOfModule = 18;
        private const double LengthFromWall = 35;

        public ShieldProcessing(FamilyInstance sh)
        {
            _shield = sh;
            _assignedElectricalSystems = sh.MEPModel
                .GetAssignedElectricalSystems()
                .OrderBy(es => es.StartSlot)
                .ToArray();
            _widthOfShield = _shield.LookupParameter("Ширина щита по каталогу").AsDouble().FootToMillimeters();
            _heightOfShield = _shield.LookupParameter("Высота щита по каталогу").AsDouble().FootToMillimeters();
            _countOfModulsOfShield = _shield.LookupParameter("Всего модулей на щит для спецификации").AsDouble();
            _parameters = new ParametersOfShield(sh);
        }

        public void SetParametersOfThis()
        {
            var widthOfCurrentRow = 0.0;
            var numberOfLine = 1;
            var numberOfDeviceInShield = 1;
            SetParametersOfInputDevice();
            for (var i = 1; i <= _assignedElectricalSystems.Length; i++)
            {
                var currentElS = _assignedElectricalSystems[i - 1];
                var device1Cl = currentElS.LookupParameter("Классификатор ОУ1").AsDouble();
                var device1Nm = currentElS.LookupParameter("Количество модулей ОУ1").AsDouble();
                var device2Cl = currentElS.LookupParameter("Классификатор ОУ2").AsDouble();
                var device2Nm = currentElS.LookupParameter("Количество модулей ОУ2").AsDouble();
                var deltaOfWidthRow = _widthOfModule * (device1Nm + device2Nm);
                var newWidthRow = widthOfCurrentRow + deltaOfWidthRow;
                if (i == 35)
                {
                }

                while (!TrySetParametersToCurrentDevice(ref numberOfDeviceInShield, ref numberOfLine, ref widthOfCurrentRow,
                           deltaOfWidthRow, new[] { device1Cl, device1Nm, device2Cl, device2Nm }))
                {
                    _parameters.ToZero(numberOfDeviceInShield);
                    numberOfDeviceInShield++;
                    if (numberOfDeviceInShield > _countOfModulsOfShield)
                        break;
                    CheckRowLength(ref numberOfLine, ref widthOfCurrentRow, numberOfDeviceInShield);
                    if (numberOfLine > 10)
                        break;
                }
            }

            _parameters.ToZeroToEnd(numberOfDeviceInShield);
        }

        private bool TrySetParametersToCurrentDevice(
            ref int deviceNumber,
            ref int rowNumber,
            ref double currentWidth,
            double width,
            double[] parameters)
        {
            var newWidthRow = currentWidth + width;
            if (InsideOfShield(GetRow(deviceNumber), newWidthRow))
            {
                _parameters.SetParameter(deviceNumber, parameters[0], parameters[1], parameters[2], parameters[3]);
                deviceNumber++;
                currentWidth += width;
                CheckRowLength(ref rowNumber, ref currentWidth, deviceNumber);
                return true;
            }

            return false;
        }

        private void CheckRowLength(ref int numberOfRow, ref double currentWidth, int deviceNumber)
        {
            if (numberOfRow != GetRow(deviceNumber))
            {
                numberOfRow++;
                currentWidth = 0;
            }
        }

        private bool InsideOfShield(int row, double width)
        {
            if (row == 1)
            {
                var result = _widthOfShield > (LengthFromWall * 2 + _lengthOfInputDevice + width);
                return result;
            }

            return _widthOfShield > LengthFromWall * 2 + width;
        }

        /// <summary>
        /// Возвращает ряд автоматов в щите в зависимости от номера автомата,
        /// зависит от геометрии семейства
        /// </summary>
        /// <param name="numberOfDevice"></param>
        /// <returns></returns>
        private int GetRow(int numberOfDevice)
        {
            if (numberOfDevice < 10)
                return 1;
            return 2 + (numberOfDevice - 10) / 14;
        }

        protected void SetParametersOfInputDevice()
        {
            //var doc = _shield.Document;
            // //вводной аппарат в щите
            // var inputDeviceId = _shield.LookupParameter("Тип вводного автомата").AsElementId();
            //var inputDevice = doc.GetElement(inputDeviceId);
            // Установка параметра К ВУ из другого параметра, у них разные типы!!!!!
            var typeOfInputDeviceParameter = _shield.LookupParameter("К ВУ");
            //    .SetValueString(
            var typeOfInputDeviceParameter2 = _shield.LookupParameter("Классификация вводных устройств").AsValueString();
            typeOfInputDeviceParameter.SetValueString(typeOfInputDeviceParameter2);

            //Параметр Номинальный ток ВУ //double
            var currentOfInputDeviceParameter = _shield.LookupParameter("Номинальный ток ВУ");
            //double параметр из вводного аппарата из ключевой спецификации
            var currentOfInputDevice = _shield.LookupParameter("Номинальный ток вводного устройства").AsDouble();
            currentOfInputDeviceParameter.Set(currentOfInputDevice);
        }
    }
}
