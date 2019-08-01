using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectricityRevitPlugin
{
    /// <summary>
    /// Логика взаимодействия для GetCoordinateFromUser.xaml
    /// </summary>
    public partial class GetCoordinateFromUserWpf : Window
    {
        private double _x;
        private double _y;
        private double _z;
        private bool _shiftRelativelyLevel;
        private DisplayUnitType _currentUnit = DisplayUnitType.DUT_METERS;
        private bool _isDefaultUnit = true;

        public static (double, double, bool, double) GetCoordinate(Element element)
        {
            var wpf = new GetCoordinateFromUserWpf(element);
            var q = wpf.ShowDialog();
            if (q == false)
            {
                throw new ArgumentException("Не удалось прочитать значения");
            }

            var shift = wpf.UseShiftCheckBox.IsChecked == true;
            return (wpf._x, wpf._y, wpf._shiftRelativelyLevel, wpf._z);
        }

        public GetCoordinateFromUserWpf(Element element)
        {
            var ci = CultureInfo.InvariantCulture;
            InitializeComponent();
            MeterRadioButton.Tag = DisplayUnitType.DUT_METERS;
            MeterRadioButton.Checked += MeterRadioButtonOnChecked;
            FtRadioButton.Checked += FtRadioButtonOnChecked;
            XTextBlock.TextChanged += TextBlockOnTextChanged;
            YTextBlock.TextChanged += TextBlockOnTextChanged;
            ZTextBlock.TextChanged += TextBlockOnTextChanged;
            if (element.Location is LocationPoint point)
            {
                _x = point.Point.X;
                _y = point.Point.Y;
                _z = point.Point.Z;
            }
            SetValuesToTextBox();

            MeterRadioButton.IsChecked = true;
        }

        private void TextBlockOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var flag = double.TryParse(((TextBox) e.Source).Text,NumberStyles.Any,CultureInfo.InvariantCulture, out var value);
            if (!flag)
            {
                tb.Text = ((TextBox) e.OriginalSource).Text;
            }
        }

        private void FtRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            _isDefaultUnit = true;
            _x = UnitUtils.ConvertToInternalUnits(_x, DisplayUnitType.DUT_METERS);
            _y = UnitUtils.ConvertToInternalUnits(_y, DisplayUnitType.DUT_METERS);
            _z = UnitUtils.ConvertToInternalUnits(_z, DisplayUnitType.DUT_METERS);
            SetValuesToTextBox();
            _isDefaultUnit = false;
        }

        private void MeterRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            _currentUnit = DisplayUnitType.DUT_METERS;
            _isDefaultUnit = false;
            _x = UnitUtils.ConvertFromInternalUnits(_x, DisplayUnitType.DUT_METERS);
            _y = UnitUtils.ConvertFromInternalUnits(_y, DisplayUnitType.DUT_METERS);
            _z = UnitUtils.ConvertFromInternalUnits(_z, DisplayUnitType.DUT_METERS);
            SetValuesToTextBox();
            _isDefaultUnit = false;
        }

        private void SetValuesToTextBox()
        {
            var ci = CultureInfo.InvariantCulture;
            XTextBlock.Text = _x.ToString(ci);
            YTextBlock.Text = _y.ToString(ci);
            ZTextBlock.Text = _z.ToString(ci);
        }

        public GetCoordinateFromUserWpf(IEnumerable<Element> elements)
        {
            InitializeComponent();
            //var ci = CultureInfo.InvariantCulture;
            //var point = element.Location as LocationPoint;
            //if (point != null)
            //{
            //    XTextBlock.Text = point.Point.X.ToString(ci);
            //    YTextBlock.Text = point.Point.Y.ToString(ci);
            //    ZTextBlock.Text = point.Point.Z.ToString(ci);


            //}
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var flags = new[]
            {
                double.TryParse(XTextBlock.Text, out _x),
                double.TryParse(YTextBlock.Text, out _y),
                double.TryParse(ZTextBlock.Text, out _z),
            };
            DialogResult = !flags.Any(a => a == false);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}