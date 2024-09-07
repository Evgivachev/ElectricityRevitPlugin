using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using MoreLinq;
using ShieldPanel.ViewOfDevicesOfShield;
using Grid = System.Windows.Controls.Grid;

namespace ShieldPanel.SelectModelOfShield;

/// <summary>
/// Логика взаимодействия для SelectModelOfShieldWPF.xaml
/// </summary>
public partial class SelectModelOfShieldWPF : Window
{
    private Document _doc;
    private Element[] _catalog;
    private bool _isAllSelected = false;
    private bool _isVisibleOfLineOfGrid = false;
    private bool _visibleOfDoors = false;
    public SelectModelOfShieldWPF(FamilyInstance[] shields, Element[] catalog, HashSet<int> selection)
    {
        _catalog = catalog;
        _doc = shields.First().Document;
        InitializeComponent();
        var groups = shields.GroupBy(x => x.Name.Split("- .".ToCharArray()).First());
        //добавление в treeView
        foreach (var group in groups)
        {
            var parentGrid = GetGrid(null, group.Key);
            var parentItem = new TreeViewItem();
            parentItem.Header = parentGrid;
            TreeView.Items.Add(parentItem);
            foreach (var familyInstance in group)
            {
                var isChecked = selection != null && selection.Contains(familyInstance.Id.IntegerValue);
                var grid = GetGrid(familyInstance, null, isChecked);
                var item = new TreeViewItem();
                item.Header = grid;
                parentItem.Items.Add(item);
            }
            //    var text = new TextBlock();
            //    text.Text = shield.Name;
            //    text.Tag = shield;
            //    var grid = new Grid();
            //    var cd = new ColumnDefinition();
            //    cd.Width = new GridLength(1, GridUnitType.Star);
            //    grid.ColumnDefinitions.Add(cd);
            //    grid.Children.Add(text);
            //    var comboBox = new ComboBox();
            //    comboBox.ItemsSource = catalog;
            //    comboBox.DisplayMemberPath = "Name";
            //    var valueId = shield.LookupParameter("Оболочка щита").AsElementId();
            //    comboBox.SelectedItem = catalog.FirstOrDefault(sh => sh.Id.IntegerValue == valueId.IntegerValue);

            //    //grid.ShowGridLines = true;
            //    grid.Children.Add(comboBox);

            //    comboBox.Parent.SetValue(Grid.ColumnSpanProperty, 1);
            //    //StackPanel.Children.Add(grid);
        }

        var manufactures = catalog.Select(m => m
                .LookupParameter("Изготовитель щита для спецификации")
                .AsString())
            .Distinct();
        ManufactureGroupBox.ItemsSource = manufactures;

    }

    private Grid GetGrid(FamilyInstance familyInstance, string name, bool isChecked = false)
    {
        var grid = new Grid();
        grid.HorizontalAlignment = HorizontalAlignment.Center;
        grid.VerticalAlignment = VerticalAlignment.Center;
        grid.Width = 250;
        grid.Height = 20;
        grid.ShowGridLines = _isVisibleOfLineOfGrid;
        var cd1 = new ColumnDefinition();
        cd1.Width = new GridLength(20, GridUnitType.Pixel);
        var cd2 = new ColumnDefinition();
        cd2.Width = new GridLength(1, GridUnitType.Star);
        grid.ColumnDefinitions.Add(cd1);
        grid.ColumnDefinitions.Add(cd2);
        var cb = new CheckBox();
        cb.IsChecked = isChecked;
        cb.Checked += CbOnClick;
        cb.Unchecked += CbOnClick;
        //cb.Click += CbOnClick;
        var text = new TextBlock();
        text.Text = familyInstance?.Name ?? name;
        text.Tag = familyInstance;
        Grid.SetColumn(cb, 0);
        Grid.SetRow(cb, 0);
        Grid.SetColumn(text, 1);
        Grid.SetRow(text, 0);

        if (familyInstance != null)
        {
            var cd3 = new ColumnDefinition();
            cd3.Width = new GridLength(1.5, GridUnitType.Star);
            grid.ColumnDefinitions.Add(cd3);

            var comboBox = new ComboBox();
            comboBox.ItemsSource = _catalog;
            comboBox.DisplayMemberPath = "Name";
            var valueId = familyInstance.LookupParameter("Оболочка щита").AsElementId();
            comboBox.SelectedItem = _catalog.FirstOrDefault(sh => sh.Id.IntegerValue == valueId.IntegerValue);
            Grid.SetColumn(comboBox, 2);
            Grid.SetRow(comboBox, 0);
            grid.Children.Add(comboBox);

        }




        grid.Children.Add(cb);
        grid.Children.Add(text);
        return grid;
    }

    private void CbOnClick(object sender, RoutedEventArgs e)
    {
        var cb = sender as CheckBox;
        if (cb is null)
            return;
        var grid = cb?.Parent as Grid;
        var item = grid?.Parent as TreeViewItem;
        var childCb = item?.Items
            .OfType<TreeViewItem>()
            .SelectMany(x => (x.Header as Grid)?.Children.OfType<CheckBox>());
        foreach (var checkBox in childCb)
            checkBox.IsChecked = cb.IsChecked;
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        using (var tr = new Transaction(_doc))
        {
            tr.Start("Set values");
            foreach (TreeViewItem parentItem in TreeView.Items)
            {
                foreach (TreeViewItem item in parentItem.Items)
                {
                    var grid = item.Header as Grid;
                    var children = grid.Children;
                    var cb = children.OfType<CheckBox>().First();
                    var isChecked = cb?.IsChecked;
                    if (!isChecked.HasValue || isChecked.Value == false)
                        continue;
                    var shield = children.OfType<TextBlock>().First().Tag as FamilyInstance;
                    var value = children.OfType<ComboBox>().First().SelectedItem as Element;
                    var parameter = shield.LookupParameter("Оболочка щита");
                    if (value != null)
                        parameter.Set(value.Id);
                }
            }
            tr.Commit();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void SelectionButton_Click(object sender, RoutedEventArgs e)
    {
        var manuf = ManufactureGroupBox.Text;
        if (manuf is null)
            return;
        foreach (TreeViewItem parentItem in TreeView.Items)
        {
            foreach (TreeViewItem item in parentItem.Items)
            {
                var grid = item.Header as Grid;
                var children = grid.Children;
                var c1 = children[0];
                var type = c1.GetType();

                var cb = children.OfType<CheckBox>().First();
                var isChecked = cb?.IsChecked;
                if (!isChecked.HasValue || isChecked.Value == false)
                    continue;
                var shield = children.OfType<TextBlock>().First().Tag as FamilyInstance;
                //if (value is null)
                //{
                var nmOfShield = ShieldGetter.GetNumberOfModules(shield);
                var q = _catalog.Where(el =>
                    {
                        var pr = el.LookupParameter("Изготовитель щита для спецификации")
                            .AsString();
                        var nm = el.LookupParameter("Всего модулей на щит для спецификации").AsDouble();
                        return pr == manuf && nm > nmOfShield;
                    })
                    .MinBy(el => el.LookupParameter("Всего модулей на щит для спецификации").AsDouble())
                    .FirstOrDefault();
                if (q != null)
                    children.OfType<ComboBox>().First().SelectedItem = q;
                //else
                //{
                //    cb.IsChecked = false;

                //}
                //}
            }
        }
    }

    private void SelectionTreeButton_Click(object sender, RoutedEventArgs e)
    {
        _isAllSelected = !_isAllSelected;
        foreach (TreeViewItem parentItem in TreeView.Items)
        {

            var grid = parentItem.Header as Grid;
            var cb = grid.Children.OfType<CheckBox>().First();
            cb.IsChecked = _isAllSelected;
        }

    }

    private void ViewOfShieldButton_Click(object sender, RoutedEventArgs e)
    {
        // var shields = new List<FamilyInstance>();

        using (var tr = new Transaction(_doc))
        {
            tr.Start("ViewOfShield");
            foreach (TreeViewItem parentItem in TreeView.Items)
            {
                foreach (TreeViewItem item in parentItem.Items)
                {
                    var grid = item.Header as Grid;
                    var children = grid.Children;
                    var c1 = children[0];
                    var type = c1.GetType();

                    var cb = children.OfType<CheckBox>().First();
                    var isChecked = cb?.IsChecked;
                    if (!isChecked.HasValue || isChecked.Value == false)
                        continue;
                    var shield = children.OfType<TextBlock>().First().Tag as FamilyInstance;
                    //shields.Add(shield);
                    var processing = new ShieldProcessing(shield);
                    processing.SetParametersOfThis();
                }
            }
            tr.Commit();
        }

    }

    private void ShowAllButton_Click(object sender, RoutedEventArgs e)
    {
        foreach (TreeViewItem parentItem in TreeView.Items)
            parentItem.IsExpanded = !parentItem.IsExpanded;

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var visible = 1;
        var flag = false;
        using (var tr = new Transaction(_doc))
        {
            tr.Start("ViewOfShieldDoor");
            foreach (TreeViewItem parentItem in TreeView.Items)
            {
                foreach (TreeViewItem item in parentItem.Items)
                {
                    var grid = item.Header as Grid;
                    var children = grid.Children;
                    var c1 = children[0];
                    var type = c1.GetType();

                    var cb = children.OfType<CheckBox>().First();
                    var isChecked = cb?.IsChecked;
                    if (!isChecked.HasValue || isChecked.Value == false)
                        continue;
                    var shield = children.OfType<TextBlock>().First().Tag as FamilyInstance;
                    var param = shield.LookupParameter("Дверь щита");
                    if (!flag)
                    {
                        visible = param.AsInteger();
                        flag = true;
                    }
                    param.Set(Math.Abs(visible - 1));
                }
            }
            tr.Commit();
        }

    }
}
