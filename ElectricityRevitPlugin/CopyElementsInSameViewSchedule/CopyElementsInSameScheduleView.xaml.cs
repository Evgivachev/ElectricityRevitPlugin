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

namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule
{
    /// <summary>
    /// Логика взаимодействия для CopyElementsInSameScheduleView.xaml
    /// </summary>
    public partial class CopyElementsInSameScheduleView : Window
    {
        public CopyElementsInSameScheduleView(CopyElementsInSameScheduleViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
