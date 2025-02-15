namespace Print.View.View;

using ElectricityRevitPlugin.UI;
using ViewModel;

public partial class PrintAndExportView : IHideable
{
    public PrintAndExportView(BaseViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void PrintAndExportView_OnContentRendered(object sender, EventArgs e)
    {
        (DataContext as BaseViewModel)?.InitializeCommand?.Execute(this);
    }
}
