using Autodesk.Revit.DB;

namespace BimRenRes.AddParametersToFamilyInstance;

public class GroupOfParameters
{
    public GroupOfParameters(int id,string name)
    {
            Id = id;
            Name = name;
        }
    public int Id;
    public string Name { get; set; }
}