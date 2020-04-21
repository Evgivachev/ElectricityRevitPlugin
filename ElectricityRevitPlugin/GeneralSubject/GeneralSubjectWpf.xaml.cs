using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ElectricityRevitPlugin.GeneralSubject
{
    /// <summary>
    /// Логика взаимодействия для GeneralSubjectWpf.xaml
    /// </summary>
    public partial class GeneralSubjectWpf : Window
    {
        public GeneralSubjectViewModel ViewModel { get; set; }

        public GeneralSubjectWpf(GeneralSubjectViewModel generalSubjectViewModel)
        {
            ViewModel = generalSubjectViewModel;
            InitializeComponent();
            SelectedFamilySymbolComboBox.ItemsSource = generalSubjectViewModel.GetAvailableFamilySymbols();
            SelectedFamilySymbolComboBox.DisplayMemberPath = "Name";
            SelectedFamilySymbolComboBox.SelectionChanged += SelectedFamilySymbolComboBoxOnSelectionChanged;
            IsHideExistingElementsCheckBox.Click += IsHideExistingElementsCheckBoxOnClick;
        }

        private void SelectedFamilySymbolComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cb = (ComboBox)sender;
                var selectedItem = e.AddedItems[0];
                ViewModel.SelectedFamilySymbol = (FamilySymbol)selectedItem;

                //TreeView.DataContext  = ViewModel.GetTreeView(ViewModel.SelectedFamilySymbol);
                TreeView.ItemsSource = ViewModel.GetTreeView(ViewModel.SelectedFamilySymbol);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
           
        }

        private void IsHideExistingElementsCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            Debug.Assert(cb != null, nameof(cb) + " != null");
            Debug.Assert(cb.IsChecked != null, "cb.IsChecked != null");
            ViewModel.IsHideExistingElements = cb.IsChecked.Value;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems =((MyCollectionOfCheckableItems)TreeView.ItemsSource)
                .SelectMany(x=>x.GetSelectedCheckableItems())
                .Where(x=>x.Item is Element)
                .Select(x=>(Element)x.Item);
            this.Close();
            ViewModel.InsertInstances(selectedItems);

           

        }
    }
}
