namespace CommonUtils.Sample;

using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

public class RevitService : IRevitService, IDisposable
{
    private readonly UIApplication _application;
    public RevitService(UIApplication application)
    {
        _application = application;
    }

    public void DoSomething()
    {


    }

    public void Dispose()
    {
        _application.Idling += ApplicationOnIdling;

    }
    
    private void ApplicationOnIdling(object sender, IdlingEventArgs e)
    {
        _application.Idling -= ApplicationOnIdling;
    }
}
