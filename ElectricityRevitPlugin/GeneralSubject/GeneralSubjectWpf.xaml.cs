namespace ElectricityRevitPlugin.GeneralSubject
{
    using System.Linq;
    using System.Windows;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Логика взаимодействия для GeneralSubjectWpf.xaml
    /// </summary>
    public partial class GeneralSubjectWpf : Window
    {
        private GeneralSubjectViewModel _viewModel;

        public GeneralSubjectWpf(GeneralSubjectViewModel viewModel)
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
            var selectedItems = (_viewModel.TreeCollectionOfCheckableItems)
                .SelectMany(x => x.GetSelectedCheckableItems())
                .Where(x => x.Item is Element)
                .Select(x => (Element)x.Item);
            Close();
            _viewModel.InsertInstances(selectedItems);
        }
    }
}
