namespace GeneralSubjectDiagram.Views
{
    using PikTools.Ui.Abstractions;
    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для GeneralSubjectWpf.xaml
    /// </summary>
    public partial class GeneralSubjectView : IHidable
    {
        /// <inheritdoc />
        public GeneralSubjectView(GeneralSubjectViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
