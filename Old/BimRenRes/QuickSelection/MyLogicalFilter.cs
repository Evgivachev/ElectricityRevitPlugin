using System.Collections.Generic;

namespace BimRenRes.QuickSelection;

public abstract class MyLogicalFilter :MyFilter
{
    protected List<MyFilter> _filters = new List<MyFilter>();

    public void AddFilter(MyFilter filter)
    {
            _filters.Add(filter);
        }
}