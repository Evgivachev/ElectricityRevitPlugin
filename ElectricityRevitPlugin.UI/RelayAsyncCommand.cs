namespace ElectricityRevitPlugin.UI;

using System.Windows.Input;

/// <summary>
/// Асинхронная команда с параметром и с обработкой ошибок
/// </summary>
/// <typeparam name="T">Тип параметра</typeparam>
public class RelayAsyncCommand<T> : ICommand
{
    private readonly Func<T, Task> _execute;
    private readonly Action<Exception?>? _onError;
    private readonly Func<T, bool>? _canExecute;
    private bool _isExecuting;

    /// <summary>Конструктор</summary>
    /// <param name="execute">Метод выполнения комнады</param>
    /// <param name="canExecute">Метод проверки возможности выполнения команды</param>
    public RelayAsyncCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary>Конструктор</summary>
    /// <param name="execute">Метод выполнения комнады</param>
    /// <param name="onError">Обработчик ошибки</param>
    /// <param name="canExecute">Метод проверки возможности выполнения команды</param>
    public RelayAsyncCommand(
        Func<T, Task> execute,
        Action<Exception?> onError,
        Func<T, bool>? canExecute = null)
    {
        _execute = execute;
        _onError = onError;
        _canExecute = canExecute;
    }

    /// <summary>Событие на проверку возможности выполнения команды</summary>
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>Возможность выполнить команду</summary>
    /// <param name="parameter">Параметр</param>
    /// <returns>true - возможно, иначе - false</returns>
    public bool CanExecute(T parameter)
    {
        if (_isExecuting)
            return false;
        Func<T, bool>? canExecute = _canExecute;
        return canExecute == null || canExecute(parameter);
    }

    /// <summary>Выполнение асинхронной команды</summary>
    /// <param name="parameter">Параметр</param>
    public async Task ExecuteAsync(T parameter)
    {
        var relayAsyncCommand = this;
        if (!relayAsyncCommand.CanExecute(parameter))
            return;
        relayAsyncCommand._isExecuting = true;
        try
        {
            await _execute(parameter);
        }
        catch (Exception e)
        {
            _onError?.Invoke(e);
        }
        relayAsyncCommand._isExecuting = false;
    }

    /// <inheritdoc />
    bool ICommand.CanExecute(object parameter)
    {
        return parameter is T parameter1 && CanExecute(parameter1);
    }

    /// <inheritdoc />
    async void ICommand.Execute(object parameter) => await ExecuteAsync((T)parameter);
}
