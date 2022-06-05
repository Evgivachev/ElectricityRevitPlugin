namespace ElectricityRevitPlugin
{
    using System.Windows;
    using System.Windows.Controls;
    using Autodesk.Revit.DB;
    using Microsoft.Win32;

    /// <summary>
    /// Логика взаимодействия для CopyElementsInViewScheduleWPF.xaml
    /// </summary>
    public partial class CopyElementsInViewScheduleWPF : Window
    {
        private CopyElementsViewScheduleExternalCommand _command;

        public CopyElementsInViewScheduleWPF(CopyElementsViewScheduleExternalCommand externalCommand)
        {
            _command = externalCommand;
            InitializeComponent();
            SchedulesComboBox.DataContext = _command.SchedulesInOpenFile;
            SchedulesComboBox.DisplayMemberPath = "Name";
        }

        public void CopyElements()
        {
            if (ShowDialog() == true)
            {
                _command.SimilarViewSchedule = (ViewSchedule)SchedulesComboBox.SelectedItem;
                _command.Work();
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var opf = new OpenFileDialog();
            opf.Filter = "Файлы проектов (*rvt)|*.rvt";
            FileNameTextBlock.Text = "";
            if (opf.ShowDialog() == true)
            {
                var fileName = opf.FileName;
                FileNameTextBlock.Text = fileName;
                _command.OpenedDocument = _command.Application.OpenDocumentFile(fileName);
                _command.SchedulesInOpenFile =
                    _command.GetSimilarKeySchedules(_command.OpenedDocument, _command.ActiveViewSchedule);
                SetDataContextToComboBox();
            }
        }

        private void SetDataContextToComboBox()
        {
            SchedulesComboBox.ItemsSource = _command.SchedulesInOpenFile;
            SchedulesComboBox.DisplayMemberPath = "Name";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void SchedulesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
