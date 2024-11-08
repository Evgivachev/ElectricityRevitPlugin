using Nuke.Common.ProjectModel;

/// <inheritdoc />
public class RevitInstallerBuilder2022 : RevitInstallerBuilder2021
{
    /// <inheritdoc />
    protected override string GetDebugInstallDir(Project project) => "%AppDataFolder%/Autodesk/Revit/Addins/2022";
}
