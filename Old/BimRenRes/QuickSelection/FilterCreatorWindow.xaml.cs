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

namespace BimRenRes.QuickSelection;

/// <summary>
/// Логика взаимодействия для FilterCreatorWindow.xaml
/// </summary>
public partial class FilterCreatorWindow : Window
{
    private FilterCreatorViewModel _filterCreatorViewModel;
    public FilterCreatorWindow(FilterCreatorViewModel filterCreatorViewModel)
    {
            this.DataContext = filterCreatorViewModel;
            _filterCreatorViewModel = filterCreatorViewModel;
            InitializeComponent();
        }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
            DialogResult = false;
            this.Close();
        }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
            this.DialogResult = true;
            this.Close();
        }
}