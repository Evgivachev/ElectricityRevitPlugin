using Bimlab.Nuke.Components;
using Nuke.Common;
using Nuke.Common.CI;
using RxBim.Nuke.Builders;
using RxBim.Nuke.Builds;
using RxBim.Nuke.Revit.Generators;

[ShutdownDotNetAfterServerBuild]
class Build : RxBimBuild<RevitInstallerBuilder2021, RevitPackageContentsGenerator, RevitProjectPropertiesGenerator, OptionsBuilder>, IRestore, IHazConfiguration
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    
    
    /// <summary>Revit Version.</summary>
    [Parameter(null)]
    public string RevitVersion { get; set; } = "2022";
    public static int Main() => Execute<Build>(x => x.Compile);


    T From<T>()
        where T : INukeBuild =>
        (T)(object)this;
}
