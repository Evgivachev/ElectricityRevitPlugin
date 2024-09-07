namespace ElectricityRevitPlugin.UI;

using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

public class ThemesResources : ResourceDictionary
{
    public ThemesResources()
    {
        var theme = new BundledTheme()
            { BaseTheme = BaseTheme.Light, PrimaryColor = PrimaryColor.DeepPurple, SecondaryColor = SecondaryColor.Lime };

        MergedDictionaries.Add(theme);
        MergedDictionaries.Add(new ResourceDictionary()
            { Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml") });
    }
}
