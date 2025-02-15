using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimRenRes.QuickSelection;

public interface IFilterHolder
{
    MyFilter AddFilter(MyFilter myFilter);
}

public interface IFilterCreator
{
    MyFilter ShowDialogAndCreateFilter();
}