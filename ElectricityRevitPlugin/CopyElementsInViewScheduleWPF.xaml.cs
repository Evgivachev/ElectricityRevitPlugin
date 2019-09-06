using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Microsoft.Win32;

namespace ElectricityRevitPlugin
{
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
                _command.SimilarViewSchedule =(ViewSchedule) SchedulesComboBox.SelectedItem;
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
    }
}
