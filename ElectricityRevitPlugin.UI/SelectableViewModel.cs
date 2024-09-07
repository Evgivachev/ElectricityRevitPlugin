namespace ElectricityRevitPlugin.UI;

using GalaSoft.MvvmLight;

public class SelectableViewModel<T> : ObservableObject
{
    private bool _isChecked;
    public SelectableViewModel(T value, bool isChecked)
    {
        Value = value;
        _isChecked = isChecked;
    }
    public T Value { get; }

    public bool IsChecked
    {
        get => _isChecked;
        set => Set(ref _isChecked, value);
    }
}
