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
        private bool _shift;

        public static (double, double, bool, double) GetCoordinate(Element element)
        {
            var wpf = new GetCoordinateFromUserWpf(element);
            var q = wpf.ShowDialog();
            if(q == false)
            {
                throw new ArgumentException("Не удалось прочитать значения");
            }
            var shift = wpf.UseShiftCheckBox.IsChecked == true;
            return (wpf._x, wpf._y, wpf._shift, wpf._z);
        }
        public GetCoordinateFromUserWpf(Element element)
        {
            var ci = CultureInfo.InvariantCulture;
            InitializeComponent();
            var point = element.Location as LocationPoint;
            if (point != null)
            {
                XTextBlock.Text = point.Point.X.ToString(ci);
                YTextBlock.Text = point.Point.Y.ToString(ci);
                ZTextBlock.Text = point.Point.Z.ToString(ci);


            }
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
            DialogResult = !flags.Any(a=>a==false);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
