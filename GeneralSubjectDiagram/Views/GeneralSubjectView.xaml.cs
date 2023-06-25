namespace GeneralSubjectDiagram.Views
{
    using PikTools.Ui.Abstractions;
    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для GeneralSubjectWpf.xaml
    /// </summary>
    public partial class GeneralSubjectView : IClosable
    {
        private readonly GeneralSubjectViewModel _viewModel;

        /// <inheritdoc />
        public GeneralSubjectView(GeneralSubjectViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
