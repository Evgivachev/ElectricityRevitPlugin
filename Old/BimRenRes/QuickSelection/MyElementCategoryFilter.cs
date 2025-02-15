using System.Linq;
using Autodesk.Revit.DB;

namespace BimRenRes.QuickSelection;

class MyElementCategoryFilter : MyFilter
{
    public BuiltInCategory Category { get; set; }
    public override bool PassesFilter(Element element)
    {
            var revitFilter = new ElementCategoryFilter(Category);
            return revitFilter.PassesFilter(element);
        }
    public override string Name => $"Категория {Category}";
    public override ElementFilter ConvertToElementFilter()
    {
             return new ElementCategoryFilter(Category);

        }
}