namespace MarkingElectricalSystems.Views;

using System.Globalization;
using System.Windows;

/// <summary>
/// Логика взаимодействия для GetShiftWpf.xaml
/// </summary>
public partial class GetShiftView : Window
{
    public GetShiftView()
    {
        InitializeComponent();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var text = TextBox.Text;
        if (double.TryParse(text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture,
                out var shift))
        {
            TextBox.Tag = shift;
            Close();
        }
    }
}
