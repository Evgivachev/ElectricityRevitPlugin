using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

[ShutdownDotNetAfterServerBuild]
[GitHubActions(name: nameof(BuildApp),
    image: GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(BuildApp) }
)]
[PublicAPI]
public partial class Build : NukeBuild
{
    [Parameter("Revit Version.")]
    public string RevitVersion { get; set; } = "2021";

    public static int Main() => Execute<Build>(x => x.Compile);

    /// <summary>Compiles a solution.</summary>
    public Target Compile => _ => _.Description("Compile solution").DependsOn(Restore).Executes((Action) (() => DotNetTasks.DotNetBuild((Configure<DotNetBuildSettings>) (settings => settings.SetProjectFile((string) Solution.Path).SetConfiguration((string) Configuration)))));

    /// <summary>Restores packages from a solution.</summary>
    public Target Restore => _ => _.Description("Restore packages")
        .Executes((Action) (() => DotNetTasks.DotNetRestore((Configure<DotNetRestoreSettings>) (s => s.SetProjectFile((string) Solution.Path)))));


    /// <summary>
    /// Compiles the project defined in <see cref="P:RxBim.Nuke.Builds.RxBimBuild`4.Project" /> to temporary path.
    /// </summary>
    public Target CompileToTemp => (Target) (_ => _.Description("Build project to temp output")
        .Requires((Expression<Func<string>>) (() => Project))
        .DependsOn(Restore).Executes((Action) (() => DotNetTasks.DotNetBuild((Configure<DotNetBuildSettings>) (settings => settings.SetProjectFile((string) this.GetProjectPath(Project))
            .SetOutputDirectory(OutputTmpDirBin).SetConfiguration((string) Configuration))))));

    private AbsolutePath? GetProjectPath(string? name)
    {
        var path = this.Solution.AllProjects.FirstOrDefault<Nuke.Common.ProjectModel.Project>(x => x.Name == name)?.Path;
        return (object) path != null ? path : this.Solution.Path;
    }
    
    Target BuildApp => d => d
        .DependsOn(Test2)
        .Produces(RootDirectory / "out/*.exe")
        .Inherit(t =>
        {
            Project = "ElectricityRevitPlugin.Application";
            return t;
        }, BuildInnoExe);

    Target Test2 => _ =>
        _.Executes(() =>
        {
            DotNetTasks.DotNetTest(s => s.SetConfiguration("Release"));
        });


    T From<T>()
        where T : INukeBuild =>
        (T)(object)this;
}
