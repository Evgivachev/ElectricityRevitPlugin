using System.Collections.Generic;
using System.Linq;
using Nuke.Common.ProjectModel;
using RxBim.Nuke.Models;
using RxBim.Nuke.Revit;
using RxBim.Nuke.Revit.Generators;

public class RevitInstallerBuilder2021 : RevitInstallerBuilder
{
    /// <inheritdoc />
    protected override string GetDebugInstallDir(Project project) => "%AppDataFolder%/Autodesk/Revit/Addins/2021";

    /// <inheritdoc />
    public override void GenerateAdditionalFiles(
        string? rootProjectName,
        IEnumerable<Project> allProject,
        IEnumerable<AssemblyType> allAssembliesTypes,
        string outputDir)
    {
        var addInGenerator = new AddInGenerator();
        var list = allAssembliesTypes
            .Select(x => new ProjectWithAssemblyType(allProject.First(proj => proj.Name == x.AssemblyName), x))
            .ToList();
        var rootProjectName1 = rootProjectName;
        var addInTypesPerProjects = list;
        var outputDirectory = outputDir;
        addInGenerator.GenerateAddInFile(rootProjectName1, addInTypesPerProjects, outputDirectory);
    }
}
