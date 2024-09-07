namespace ElectricityRevitPlugin.UI;

using System.Windows.Input;
using GalaSoft.MvvmLight;

public class MainViewModelBase : ViewModelBase
{
    protected MainViewModelBase(string title)
    {
        Title = title;
    }
    public ICommand? InitializeCommand { get; set; }

    public string Title { get; set; }
}
