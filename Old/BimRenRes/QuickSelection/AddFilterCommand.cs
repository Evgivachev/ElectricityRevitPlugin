using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BimRenRes.QuickSelection;

public class AddFilterCommand :ICommand
{
    private IFilterHolder _filterHolder;
    private IFilterCreator _filterCreator;

    private Func<object, bool> _canExecute;

    public AddFilterCommand(IFilterHolder filterHolder, IFilterCreator filterCreator)
    {
            _filterHolder = filterHolder;
            _filterCreator = filterCreator;
        }

    public bool CanExecute(object parameter)
    {
            return true;
        }

    public void Execute(object parameter)
    {
            var filter = _filterCreator.ShowDialogAndCreateFilter();
            if (filter != null)
                _filterHolder.AddFilter(filter);
        }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}