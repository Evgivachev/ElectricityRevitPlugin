namespace Print.View.View
{
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <inheritdoc />
    public class RoutedCommandBindingBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Команда
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(RoutedCommandBindingBehavior),
            new PropertyMetadata(default(ICommand)));

        /// <summary> The command that should be executed when the RoutedCommand fires. </summary>
        public ICommand? Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary> The command that triggers <see cref="ICommand"/>. </summary>
        public ICommand? RoutedCommand { get; set; }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            var binding = new CommandBinding(RoutedCommand!, HandleExecuted, HandleCanExecute);
            AssociatedObject.CommandBindings.Add(binding);
        }

        /// <summary> Proxy to the current Command.CanExecute(object). </summary>
        private void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Command?.CanExecute(e.Parameter) != null;
            e.Handled = true;
        }

        /// <summary> Proxy to the current Command.Execute(object). </summary>
        private void HandleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Command?.Execute(e.Parameter);
            e.Handled = true;
        }
    }
}
