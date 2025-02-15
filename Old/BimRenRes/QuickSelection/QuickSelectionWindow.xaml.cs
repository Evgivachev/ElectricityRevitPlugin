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
using Autodesk.Revit.UI;

namespace BimRenRes.QuickSelection;

/// <summary>
/// Логика взаимодействия для QuickSelectionWindow.xaml
/// </summary>
public partial class QuickSelectionWindow : Window, IDisposable
{
    private readonly QuickSelectionViewModel _viewModel;
    public QuickSelectionWindow(QuickSelectionViewModel  viewModel)
    {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
            DialogResult = true;
            this.Close();
        }

    private void RemovePropertyButton_Click(object sender, RoutedEventArgs e)
    {
            _viewModel.RemoveFilter(_viewModel.SelectedFilter);
        }

    public void Dispose()
    {
            this.Close();
        }

    private void EditPropertyButton_OnClickPropertyButton_Click(object sender, RoutedEventArgs e)
    {
            _viewModel.EditFilter();
        }
}