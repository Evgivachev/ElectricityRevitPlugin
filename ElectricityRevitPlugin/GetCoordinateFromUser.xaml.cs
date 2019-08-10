using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private CoordinateModelMvc _model;

        public GetCoordinateFromUserWpf(CoordinateModelMvc model)
        {
            _model = model;
            InitializeComponent();
            MeterRadioButton.Checked += MeterRadioButtonOnChecked;
            var array = new[]
            {
                XTextBlock,
                YTextBlock,
                ZTextBlock
            };
            for(var i=0;i<array.Length;i++)
            {
                array[i].TextChanged += TextBlockOnTextChanged;
                array[i].PreviewTextInput += XTextBlockOnTextInput;
            }
            FtRadioButton.Checked += FtRadioButtonOnChecked;
            UseShiftCheckBox.Click += UseShiftCheckBoxClick;
            _model.ModelChanged += _model_ModelChanged;
            SetValues();
        }

        private void _model_ModelChanged(CoordinateModelMvc obj)
        {
            _model = obj;
            SetValues();
        }

        private void XTextBlockOnTextInput(object sender, TextCompositionEventArgs e)
        {
            var inputSymbol = e.Text;

            var tb = sender as TextBox;
            var text = tb.Text+inputSymbol;
            if (text == "-")
                return;
            e.Handled = !double.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out _);
        }

        private void TextBlockOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            string inputSymbol = tb.Text;
           
            if (!double.TryParse(inputSymbol, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
            {
                e.Handled = true;
            }
        }

        private void UseShiftCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb)
                _model.UseShift = cb.IsChecked.HasValue && cb.IsChecked.Value;
            

        }

        private void TextBlockOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var flag = double.TryParse(((TextBox) e.Source).Text, NumberStyles.Any, CultureInfo.InvariantCulture,
                out var value);
            if (!flag)
            {
                tb.Text = ((TextBox) e.OriginalSource).Text;
                _model.XField =XTextBlock.Text;
                _model.YField = YTextBlock.Text;
                _model.ZField = ZTextBlock.Text;

            }
            else
            {
                e.Handled = false;
            }
        }

        private void FtRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            _model.IsMeterUnits = false;
        }

        private void MeterRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            _model.IsMeterUnits = true;
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