using System.Linq;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

class MyLogicalAndFilter : MyLogicalFilter
{
    public override bool PassesFilter(Element element)
    {
            return _filters.All(x => x.PassesFilter(element));
        }
    public override string Name => "\"И\"";
    public override ElementFilter ConvertToElementFilter()
    {
            return new LogicalAndFilter(_filters
                .Select(x=>x.ConvertToElementFilter()).ToList());
        }
}