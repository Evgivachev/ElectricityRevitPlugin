namespace CommonUtils.Sample;

using System.Windows;

public partial class SampleWindow : Window
{
    public SampleWindow(ViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
