using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities;
using RxBim.Nuke;
using RxBim.Nuke.Builders;
using RxBim.Nuke.Builds;
using RxBim.Nuke.Helpers;
using RxBim.Nuke.Models;

public partial class Build
{
    RevitInstallerBuilder2021 _builder = new();
    string? _project;
    List<AssemblyType>? _types;
    string? _outputTmpDir;
    bool _timestampRevisionVersion;
    string? _outputTmpDirBin;

    /// <summary>Solution.</summary>
    [Solution]
    public Solution Solution { get; set; }

    /// <summary>Environment variable.</summary>
    [Parameter("Environment variable")]
    public string RxBimEnvironment { get; set; } = "Testing";

    /// <summary>
    /// Configuration to build - Default is 'Debug' (local) or 'Release' (server).
    /// </summary>
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Select project")]
    public virtual string? Project
    {
        get
        {
            if (_project != null)
                return _project;
            var result = ConsoleUtility.PromptForChoice("Select project:", Solution.AllProjects
                .Select<Project, (string, string)>(x => (x.Name, x.Name))
                .Append(("Solution", "All"))
                .ToArray());
            _project = result == "Solution" ? Solution.Name : Solution.AllProjects.FirstOrDefault<Project>(x => x.Name == result)?.Name;
            return _project;
        }
        set => _project = value;
    }

    /// <summary>Builds an EXE package.</summary>
    public virtual Target BuildInnoExe => _ => _
        .Description("Build installation EXE from selected project (if Release - sign assemblies)")
        .DependsOn(GenerateAdditionalFiles)
        .DependsOn(GeneratePackageContentsFile)
        .Executes((Action)(() =>
        {
            CreateOutDirectory();
            BuildInnoInstaller(ProjectForInstallBuild, Configuration);
        }));

    private void CreateOutDirectory()
    {
        var path = Solution.Directory / "out";
        if (Directory.Exists((string)path))
            return;
        Directory.CreateDirectory((string)path);
    }

    private void BuildInnoInstaller(Project project, string configuration)
    {
        var buildOptions = GetBuildOptions(project, configuration);
        var str = buildOptions.OutFileName + "_" + buildOptions.Version;
        _builder.BuildInno(TemporaryDirectory, OutputTmpDir, OutputTmpDirBin, buildOptions);
        FileSystemTasks.DeleteDirectory(OutputTmpDir);
    }

    /// <summary>Generates additional files.</summary>
    public Target GenerateAdditionalFiles => _ => _
        .Requires((Expression<Func<string>>)(() => Project))
        .Requires((Expression<Func<Configuration>>)(() => Configuration))
        .DependsOn(CompileToTemp)
        .Executes((Action)(() => _builder.GenerateAdditionalFiles(ProjectForInstallBuild.Name, Solution.AllProjects,
            GetAssemblyTypes(), OutputTmpDir)));

    /// <summary>Generates a package contents file.</summary>
    public Target GeneratePackageContentsFile => _ => _
        .Requires((Expression<Func<string>>)(() => Project))
        .Requires((Expression<Func<Configuration>>)(() => Configuration))
        .DependsOn(CompileToTemp)
        .Executes((Action)(() =>
            _builder.GeneratePackageContentsFile(ProjectForInstallBuild, Configuration, GetAssemblyTypes(), OutputTmpDir, SeriesMaxAny)));

    private List<AssemblyType> GetAssemblyTypes()
    {
        return _types ??= GetAssemblyTypes(ProjectForInstallBuild, OutputTmpDirBin, GetBuildOptions(ProjectForInstallBuild, Configuration));
    }

    public static List<AssemblyType> GetAssemblyTypes(
        Project project,
        string output,
        Options options)
    {
        var file1 = Path.Combine(output, project.Name + ".dll");
        var assemblyTypes = GetAssemblyTypes(file1, [
            "RxBimApplication"
        ]);
        var source = new List<string>();
        if (options.AddAllAppToManifest)
        {
            source = Directory.GetFiles(output, "*.dll").ToList();
            source.Remove(file1);
        }
        else if (options.ProjectsAddingToManifest != null && options.ProjectsAddingToManifest.Any())
        {
            source = options.ProjectsAddingToManifest.Select((Func<string, string>)(p => Path.Combine(output, p.Trim() + ".dll"))).ToList();
            if (source.Any((Func<string, bool>)(f => !File.Exists(f))))
                throw new FileNotFoundException("Assembly not found from property ProjectsAddingToManifest");
        }
        foreach (var file2 in source)
            assemblyTypes.AddRange(GetAssemblyTypes(file2, new string[1]
            {
                "RxBimApplication"
            }));
        return assemblyTypes;
    }

    private static List<AssemblyType> GetAssemblyTypes(string file, string[] typeNames)
    {
        Console.WriteLine("[GetAssemblyTypes]" + string.Join("\n", AssemblyScanner.Scan(file)));
        return AssemblyScanner.Scan(file).Where(x => typeNames.Intersect(x.BaseTypeNames).Any()).ToList();
    }

    /// <summary>Selected project.</summary>
    protected virtual Project ProjectForInstallBuild => Solution.AllProjects.First<Project>(x => x.Name == Project);

    /// <summary>Add timestamp revision version.</summary>
    [Parameter("Adds timestamp revision version")]
    public bool TimestampRevisionVersion
    {
        get => Configuration == Configuration.Debug || _timestampRevisionVersion;
        set => _timestampRevisionVersion = value;
    }

    /// <summary>
    /// Returns <see cref="T:RxBim.Nuke.Options" />.
    /// </summary>
    /// <param name="project">Selected project.</param>
    /// <param name="configuration">Configuration.</param>
    protected virtual Options GetBuildOptions(Project project, string configuration)
    {
        var optsBuilder = new OptionsBuilder();
        optsBuilder.SetDefaultSettings(project)
            .SetDirectorySettings(_builder.GetInstallDir(project, configuration), OutputTmpDir)
            .SetProductVersion(project, configuration)
            .SetEnvironment(RxBimEnvironment)
            .SetVersion(project);
        if (TimestampRevisionVersion)
            optsBuilder.SetTimestampRevisionVersion();
        return optsBuilder.Build();
    }

    /// <summary>Output temp directory path.</summary>
    protected virtual string OutputTmpDir
    {
        get
        {
            var outputTmpDir = _outputTmpDir;
            if (outputTmpDir != null)
                return outputTmpDir;
            var tempPath = Path.GetTempPath();
            var interpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
            interpolatedStringHandler.AppendLiteral("RxBim_build_");
            interpolatedStringHandler.AppendFormatted(Guid.NewGuid());
            var stringAndClear = interpolatedStringHandler.ToStringAndClear();
            return _outputTmpDir = Path.Combine(tempPath, stringAndClear);
        }
    }

    /// <summary>Output "bin" temp directory path.</summary>
    protected virtual string OutputTmpDirBin => _outputTmpDirBin ??= Path.Combine(OutputTmpDir, "bin");

    /// <summary>Supports any maximum version of CAD.</summary>
    [Parameter("Supports any maximum version of CAD")]
    public bool SeriesMaxAny { get; set; }

}
