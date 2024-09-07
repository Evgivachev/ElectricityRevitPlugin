namespace ElectricityRevitPlugin.UpdateModels;

using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ModelFromServerListGetter
{
    /// <summary>
    /// Sample: "\\10.1.1.2\revit\PROJECTS\тест_19ПД77-МСИ"
    /// </summary>
    public string Root { get; set; }

    /// <summary>
    /// Sample: "RNS://192.168.10.205/"
    /// </summary>
    public string CentralServerLocation { get; set; }

    public IEnumerable<DirectoryInfo> GetModels()
    {
        var rootDirectory = new DirectoryInfo(Root);
        var revitModels = rootDirectory.EnumerateDirectories("*.rvt", SearchOption.AllDirectories);
        return revitModels;
    }

    public void SaveModelsList(string path)
    {
        var models = GetModels()
            .ToArray();
        string.Join<DirectoryInfo>("\n", models);
        File.WriteAllLines(path, models
            .Select(di => Path
                .Combine(di.FullName
                    .Split(Path.DirectorySeparatorChar)
                    .Skip(5).ToArray())));
    }

    public IEnumerable<string> GetModels(string path)
    {
        return File.ReadLines(path);
    }
}
