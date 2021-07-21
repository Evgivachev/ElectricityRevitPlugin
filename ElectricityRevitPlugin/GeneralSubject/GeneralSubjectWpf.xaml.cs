﻿using System;
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
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems =(_viewModel.TreeCollectionOfCheckableItems)
                .SelectMany(x=>x.GetSelectedCheckableItems())
                .Where(x=>x.Item is Element)
                .Select(x=>(Element)x.Item);
            this.Close();
            _viewModel.InsertInstances(selectedItems);
        }
    }
}