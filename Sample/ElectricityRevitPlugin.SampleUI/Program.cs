namespace ElectricityRevitPlugin.SampleUI;

using System.Windows;
using View;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new Application();
        var window = new SampleWindow();
        app.Run(window);
    }

}
