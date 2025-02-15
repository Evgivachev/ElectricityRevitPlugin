using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

public abstract class MyFilter
{
    public abstract bool PassesFilter(Element element);

    public abstract string Name { get; }

    public abstract ElementFilter ConvertToElementFilter();

}