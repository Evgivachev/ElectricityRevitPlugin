﻿namespace Diagrams.ViewContext
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using ElectricityRevitPlugin.UI;
    using GalaSoft.MvvmLight.CommandWpf;
    using Models;
    using MoreLinq;

    public class BuildDiagramsContext : MainViewModelBase
    {
        private readonly IDiagramsDrawer _diagramsDrawer;
        private readonly IDiagramsUpdater _diagramsUpdater;
        private readonly IShieldsProvider _shieldsProvider;
        private readonly UIApplication _uiApplication;
        private ObservableCollection<SelectableGroupModel<string, Shield>> _shields;

        public BuildDiagramsContext(
            IShieldsProvider shieldsProvider,
            UIApplication uiApplication,
            IDiagramsDrawer diagramsDrawer,
            IDiagramsUpdater diagramsUpdater)
            : base("Построение схем")
        {
            _shieldsProvider = shieldsProvider;
            _uiApplication = uiApplication;
            _diagramsDrawer = diagramsDrawer;
            _diagramsUpdater = diagramsUpdater;
            InitializeCommand = new RelayCommand(Initialize);
        }

        public ICommand SelectAllCommand => new RelayCommand(SelectAll);
        public ICommand BuildCommand => new RelayCommand(BuildMethod);
        public ICommand UpdateCommand => new RelayCommand(UpdateMethod);

        public ObservableCollection<SelectableGroupModel<string, Shield>> Shields
        {
            get => _shields;
            private set
            {
                _shields = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CheckCommand => new RelayCommand<object>(CheckMethod);

        private void Initialize()
        {
            var allShields = _shieldsProvider.GetShields();
            Shields = new ObservableCollection<SelectableGroupModel<string, Shield>>(allShields
                .GroupBy(s =>
                {
                    var index = s.Name.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    var subName = s.Name.Substring(0, index > 0 ? index : s.Name.Length - 1);
                    return subName;
                })
                .Select(g => new SelectableGroupModel<string, Shield>(g.Key, false)
                {
                    InnerItems = g.Select(s => new SelectableViewModel<Shield>(s, false)).ToList()
                }));
        }

        private void CheckMethod(object item)
        {
            switch (item)
            {
                case SelectableViewModel<Shield>:
                {
                    var parent = Shields.First(g => g.InnerItems.Contains(item));
                    UpdateGroupChecking(parent);
                    break;
                }

                case SelectableGroupModel<string, Shield> groupModel:
                {
                    if (groupModel.IsChecked is null)
                        break;
                    foreach (var innerItem in groupModel.InnerItems)
                        innerItem.IsChecked = groupModel.IsChecked ?? false;

                    break;
                }
            }
        }

        private void UpdateGroupChecking(SelectableGroupModel<string, Shield> groupModel)
        {
            bool? result = groupModel.InnerItems[0].IsChecked;
            if (groupModel.InnerItems.Any(s => s.IsChecked != result))
                result = null;

            groupModel.IsChecked = result;
        }

        private void UpdateMethod()
        {
            var doc = _uiApplication.ActiveUIDocument.Document!;
            var nameOfFamilyOfHead = "ЭОМ-Схемы однолинейные-Шапка (ГОСТ 2.708-81)";
            var familyHead = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .FirstOrDefault(x => x.Name == nameOfFamilyOfHead)
                as Family;
            if (familyHead == null)
                throw new NullReferenceException($"Не удалось найти семейство \"{nameOfFamilyOfHead}\"");
            var familySymbolHead = (FamilySymbol)doc?.GetElement(familyHead?.GetFamilySymbolIds().First());
            var filter = new FamilyInstanceFilter(doc, familySymbolHead.Id);
            //todo
            var heads = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .OfType<FamilyInstance>()
                .ToLookup(x => x.LookupParameter("ID электрического щита")?.AsString(), x => x);
            foreach (var shield in Shields.SelectMany(x => x.InnerItems))
            {
                if (!shield.IsChecked)
                    continue;
                //виды где есть аннотация со ссылкой на этот щит
                var uniqueName = shield.Value.UniqueId;
                if (!heads.Contains(uniqueName))
                {
                    continue;
                }
                
                heads[uniqueName]
                    .Select(an => an.OwnerViewId)
                    .Select(x => doc.GetElement(x) as ViewDrafting)
                    .Where(x => x is not null)
                    .ForEach(_diagramsUpdater.UpdateDiagram);
            }
        }

        private void BuildMethod()
        {
            foreach (var sheetNode in Shields.SelectMany(x => x.InnerItems))
            {
                if (!sheetNode.IsChecked)
                    continue;
                _diagramsDrawer.DrawDiagram(sheetNode.Value);
            }
        }


        private void SelectAll()
        {
            var flag = Shields
                .SelectMany(x => x.InnerItems)
                .Any(x => x.IsChecked);
            foreach (var s in Shields)
            {
                s.IsChecked = !flag;
                foreach (var innerItem in s.InnerItems)
                    innerItem.IsChecked = !flag;
            }
        }
    }
}
