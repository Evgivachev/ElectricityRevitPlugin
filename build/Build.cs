using Bimlab.Nuke.Components;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
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
        .Produces(RootDirectory / "out/*")
        .Inherit(t =>
        {
            Project = "ElectricityRevitPluginApp";
            return t;
        }, BuildInnoExe);


    T From<T>()
        where T : INukeBuild =>
        (T)(object)this;
}
