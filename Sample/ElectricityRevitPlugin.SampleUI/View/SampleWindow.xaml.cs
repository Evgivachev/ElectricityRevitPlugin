namespace ElectricityRevitPlugin.SampleUI.View;

using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

public partial class SampleWindow : Window
{
    public SampleWindow()
    {
        var theme = new BundledTheme()
            { BaseTheme = BaseTheme.Light, PrimaryColor = PrimaryColor.DeepPurple, SecondaryColor = SecondaryColor.Lime };

        Resources.MergedDictionaries.Add(theme);
        Resources.MergedDictionaries.Add(new ResourceDictionary()
            { Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml") });
        InitializeComponent();
    }
}
