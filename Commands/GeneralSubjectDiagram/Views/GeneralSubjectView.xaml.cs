namespace GeneralSubjectDiagram.Views;

using ViewModels;

/// <summary>
/// Логика взаимодействия для GeneralSubjectWpf.xaml
/// </summary>
public partial class GeneralSubjectView
{
    /// <inheritdoc />
    public GeneralSubjectView(GeneralSubjectViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
