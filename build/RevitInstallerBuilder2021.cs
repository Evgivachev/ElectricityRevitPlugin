using Nuke.Common.ProjectModel;
using RxBim.Nuke.Revit;

public class RevitInstallerBuilder2021 : RevitInstallerBuilder
{
    /// <inheritdoc />
    protected override string GetDebugInstallDir(Project project) => "%AppDataFolder%/Autodesk/Revit/Addins/2021";
}
