namespace CommonUtils.Sample;

using Autodesk.Revit.Attributes;
using Microsoft.Extensions.DependencyInjection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class CmdSample : WpfCmd<SampleWindow>
{

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<SampleWindow>();
        serviceCollection.AddSingleton<ViewModel>();
        serviceCollection.AddSingleton<IRevitService, RevitService>();
        
    }
}
