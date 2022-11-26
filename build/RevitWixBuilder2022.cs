namespace DefaultNamespace;

using Nuke.Common.ProjectModel;
using RxBim.Nuke.Revit;

public class RevitWixBuilder2022 : RevitWixBuilder
{
    protected override string GetDebugInstallDir(Project project) => "%AppDataFolder%/Autodesk/Revit/Addins/2022";
}
