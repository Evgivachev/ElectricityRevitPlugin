namespace GeneralSubjectDiagram.Views
{
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.DB;
    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для GeneralSubjectWpf.xaml
    /// </summary>
    public partial class GeneralSubjectView : Window
    {
        private readonly GeneralSubjectViewModel _viewModel;

        public GeneralSubjectView(GeneralSubjectViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
