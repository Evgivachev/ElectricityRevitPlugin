namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule;

using System.Windows;

/// <summary>
/// Логика взаимодействия для CopyElementsInSameScheduleView.xaml
/// </summary>
public partial class CopyElementsInSameScheduleView
{
    public CopyElementsInSameScheduleView(CopyElementsInSameScheduleViewModel model)
    {
        DataContext = model;
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
