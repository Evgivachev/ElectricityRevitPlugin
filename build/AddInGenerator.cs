using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Nuke.Common.ProjectModel;
using RxBim.Nuke.Extensions;
using RxBim.Nuke.Models;
using RxBim.Nuke.Revit.Generators.Extensions;
using RxBim.Nuke.Revit.Generators.Models;

public class AddInGenerator
{
    /// <summary>Generate a new addin file.</summary>
    /// <param name="rootProjectName">The root project name.</param>
    /// <param name="addInTypesPerProjects">Addin types for registration in Revit.</param>
    /// <param name="outputDirectory">The output directory path.</param>
    public void GenerateAddInFile(
        string? rootProjectName,
        IReadOnlyList<ProjectWithAssemblyType> addInTypesPerProjects,
        string outputDirectory)
    {
        Console.WriteLine("Generating addin file...");
        Console.WriteLine($"count is {addInTypesPerProjects.Count} ");
        Console.WriteLine(string.Join("\t",addInTypesPerProjects
            .Select(x => x.AssemblyType)));
        var list = addInTypesPerProjects
            .ToList();
        
        if (!addInTypesPerProjects.Any())
        {
            var interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
            interpolatedStringHandler.AppendLiteral("Project ");
            interpolatedStringHandler.AppendFormatted(rootProjectName);
            interpolatedStringHandler.AppendLiteral(" should contain any ");
            interpolatedStringHandler.AppendFormatted("IExternalApplication");
            throw new ArgumentException(interpolatedStringHandler.ToStringAndClear());
        }

        this.GenerateAddIn(rootProjectName, list, outputDirectory);
    }

    private void GenerateAddIn(
        string? rootProjectName,
        IEnumerable<ProjectWithAssemblyType> addinTypesPerProjects,
        string output)
    {
        var addInList = new List<AddIn>();
        foreach (var addinTypesPerProject in addinTypesPerProjects)
        {
            var project = addinTypesPerProject.Project;
            var assemblyType = addinTypesPerProject.AssemblyType;
            var addInGuid = this.GetAddInGuid(project, assemblyType);
            addInList.Add(new AddIn(project.Name, rootProjectName + "/" + project.Name + ".dll", addInGuid.ToString(),
                assemblyType.FullName, assemblyType.ToPluginType()));
        }

        var revitAddIns = new RevitAddIns();
        revitAddIns.AddIn = addInList;
        var fileName = Path.Combine(output, rootProjectName + ".addin");
        revitAddIns.ToXDocument().Save(fileName);
    }

    private Guid GetAddInGuid(Project project, AssemblyType assemblyType)
    {
        var propertyName = assemblyType.ToPropertyName();
        Guid result;
        if (!Guid.TryParse(project.GetProperty(propertyName), out result))
            throw new ArgumentException("Property '" + propertyName + "' should contain valid guid value!");
        return result;
    }
}

