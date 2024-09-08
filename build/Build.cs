using Bimlab.Nuke.Components;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.DotNet;
using RxBim.Nuke.Builders;
using RxBim.Nuke.Builds;
using RxBim.Nuke.Revit.Generators;

[ShutdownDotNetAfterServerBuild]
[GitHubActions(name: nameof(BuildApp),
    image: GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(BuildApp) }
)]
[PublicAPI]
public class Build : RxBimBuild<RevitInstallerBuilder2021, RevitPackageContentsGenerator, RevitProjectPropertiesGenerator, OptionsBuilder>,
    IRestore,
    IHazConfiguration
{
    [Parameter("Revit Version.")]
    public string RevitVersion { get; set; } = "2022";

    public static int Main() => Execute<Build>(x => x.Compile);

    Target BuildApp => d => d
        .DependsOn(Test2)
        .Produces(RootDirectory / "out/*.exe")
        .Inherit(t =>
        {
            Project = "ElectricityRevitPluginApp";
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
