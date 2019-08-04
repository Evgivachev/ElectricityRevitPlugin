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
        private readonly CoordinateModelMvc _model;
       
        public GetCoordinateFromUserWpf(CoordinateModelMvc model)
        {
            _model = model;
            InitializeComponent();
            MeterRadioButton.Checked += MeterRadioButtonOnChecked;
            FtRadioButton.Checked += FtRadioButtonOnChecked;
            XTextBlock.TextChanged += TextBlockOnTextChanged;
            YTextBlock.TextChanged += TextBlockOnTextChanged;
            ZTextBlock.TextChanged += TextBlockOnTextChanged;
            UseShiftCheckBox.Checked += UseShiftCheckBoxOnChecked;

            SetValues();
        }

        private void UseShiftCheckBoxOnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb) 
                _model.UseShift = cb.IsChecked.HasValue && cb.IsChecked.Value;
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
            _model.IsMeterUnits = false;
            SetValues();

        }

        private void MeterRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            _model.IsMeterUnits = true;
            SetValues();

        }

        private void SetValues()
        {
            MeterRadioButton.IsChecked = _model.IsMeterUnits;
            UseShiftCheckBox.IsChecked = _model.UseShift;
            XTextBlock.Text = _model.XField;
            YTextBlock.Text = _model.YField;
            ZTextBlock.Text = _model.ZField;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}