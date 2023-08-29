using Nuke.Common.ProjectModel;
using RxBim.Nuke.Revit;

/// <inheritdoc />
public class RevitInstallerBuilder2022 : RevitInstallerBuilder
{
    /// <inheritdoc />
    protected override string GetDebugInstallDir(Project project) => "%AppDataFolder%/Autodesk/Revit/Addins/2022";
}
