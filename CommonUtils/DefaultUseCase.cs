using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Hosting;

namespace CommonUtils;

public abstract class DefaultUseCase : ICmdUseCase
{
    private readonly IApplicationLifetime _applicationLifetime;

    /// <summary>
    /// ctr
    /// </summary>
    /// <param name="applicationLifetime"></param>
    protected DefaultUseCase(IApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
    }

    /// <inheritdoc />
    public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
}
