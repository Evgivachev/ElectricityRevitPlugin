namespace CommonUtils.Sample;

using Autodesk.Revit.Attributes;
using Microsoft.Extensions.DependencyInjection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class CmdSample : WpfCmd<SampleWindow>
{

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<SampleWindow>();
        services.AddSingleton<ViewModel>();
        services.AddSingleton<IRevitService, RevitService>();
        
    }
}
