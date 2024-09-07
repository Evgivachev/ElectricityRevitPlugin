namespace CommonUtils.Sample;

public partial class SampleWindow
{
    public SampleWindow(ViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
