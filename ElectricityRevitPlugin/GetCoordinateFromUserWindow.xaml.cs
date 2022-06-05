namespace ElectricityRevitPlugin
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Логика взаимодействия для GetCoordinateFromUser.xaml
    /// </summary>
    public partial class GetCoordinateFromUserWindow : Window
    {
        private CoordinateModelMvc _model;

        public GetCoordinateFromUserWindow(CoordinateModelMvc model)
        {
            _model = model;
            InitializeComponent();
            var array = new[]
            {
                XTextBlock,
                YTextBlock,
                ZTextBlock,
                RTextBlock
            };
            for (var i = 0; i < array.Length; i++)
            {
                array[i].PreviewTextInput += XTextBlockOnTextInput;
                array[i].GotFocus += GetCoordinateFromUserWpf_GotFocus;
            }

            MeterRadioButton.Checked += MeterRadioButtonOnChecked;
            FtRadioButton.Checked += FtRadioButtonOnChecked;
            UseShiftCheckBox.Click += UseShiftCheckBoxClick;
            _model.ModelChanged += _model_ModelChanged;
            SetValues();
        }

        private void GetCoordinateFromUserWpf_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.SelectAll();
        }

        private void GetCoordinateFromUserWpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            _model.XField = XTextBlock.Text;
            _model.YField = YTextBlock.Text;
            _model.ZField = ZTextBlock.Text;
        }

        private void GetCoordinateFromUserWpf_TextInput(object sender, TextCompositionEventArgs e)
        {
            _model.XField = XTextBlock.Text;
            _model.YField = YTextBlock.Text;
            _model.ZField = ZTextBlock.Text;
        }

        private void GetCoordinateFromUserWpf_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _model.XField = XTextBlock.Text;
            _model.YField = YTextBlock.Text;
            _model.ZField = ZTextBlock.Text;
        }

        private void GetCoordinateFromUserWpf_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            _model.XField = XTextBlock.Text;
            _model.YField = YTextBlock.Text;
            _model.ZField = ZTextBlock.Text;
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
            if (tb.Text == _model.IsNotSimilar)
                tb.Text = "";
            var text = tb.Text + inputSymbol;
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
                return;
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
            var flag = double.TryParse(((TextBox)e.Source).Text, NumberStyles.Any, CultureInfo.InvariantCulture,
                out var value);
            if (tb.Text == _model.IsNotSimilar)
                return;
            if (flag)
            {
                tb.Text = ((TextBox)e.OriginalSource).Text;
                _model.XField = XTextBlock.Text;
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
            RTextBlock.Text = _model.RField;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var _x = (string)XTextBlock.Text.Clone();
            var _y = (string)YTextBlock.Text.Clone();
            var _z = (string)ZTextBlock.Text.Clone();
            var _r = (string)RTextBlock.Text.Clone();
            _model.XField = _x;
            _model.YField = _y;
            _model.ZField = _z;
            _model.RField = _r;
            _model.SetCoordinate();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
