using Nuke.Common;
using Nuke.Common.CI;
using RxBim.Nuke.Revit;

[ShutdownDotNetAfterServerBuild]
class Build : RevitRxBimBuild
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
}
