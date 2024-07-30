using System.Diagnostics;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace CommonUtils.Services;

public class DisService : IDisposable
{
    private readonly UIApplication _application;

    public DisService(UIApplication application)
    {
        _application = application;
        _application.Idling += ApplicationOnIdling;
    }

    ~DisService()
    {
        Debug.Print("finalize");
        Dispose();
    }

    private void ApplicationOnIdling(object sender, IdlingEventArgs e)
    {
        Debug.Print("ApplicationOnIdling");
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Debug.Print("dispose");
        _application.Idling -= ApplicationOnIdling;
        GC.SuppressFinalize(this);
    }
}