using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DuctSystemRevitPlugin
{

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateViewsExternalCommand : IExternalCommand
    {
        private Document Document;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            this.Document = doc;
            var result = Result.Succeeded;
            try
            {
                using (var trGr = new TransactionGroup(doc))
                {
                    trGr.Start("Duct");

                    CreateDuctSystemViews();
                    trGr.Assimilate();


                }
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }

            return result;
        }

        #region CreateDuctSystemView
        private void CreateFilterForDuctSystem(Document _doc, ParameterElement _sysNameParam, string _systemName)
        {

            using (Transaction tr = new Transaction(_doc, "Создание фильтра для: " + _systemName))
            {
                tr.Start();
                View view = Document.ActiveView;
                IList<ElementId> categories = new List<ElementId>();
                categories.Add(new ElementId(BuiltInCategory.OST_DuctAccessory));
                categories.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
                categories.Add(new ElementId(BuiltInCategory.OST_DuctFitting));
                categories.Add(new ElementId(BuiltInCategory.OST_DuctInsulations));
                categories.Add(new ElementId(BuiltInCategory.OST_DuctTerminal));
                categories.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
                categories.Add(new ElementId(BuiltInCategory.OST_PlaceHolderDucts));
                //categories.Add(new ElementId(BuiltInCategory.OST_GenericModel));
                categories.Add(new ElementId(BuiltInCategory.OST_MechanicalEquipment));
                var sysNameParamId = new ElementId(BuiltInParameter.RBS_SYSTEM_NAME_PARAM);
                FilterRule rule = ParameterFilterRuleFactory.CreateNotContainsRule(sysNameParamId, _systemName, true);
                ElementParameterFilter epf = new ElementParameterFilter(rule);
                //ElementFilter ef = epf as ElementFilter;
                ParameterFilterElement filter = null;
                try
                {
                    filter = ParameterFilterElement
                        .Create(_doc, "MACROS_Возд_" + _systemName, categories, epf);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException e)
                {
                    Element filter1 = new FilteredElementCollector(_doc)
                        .OfClass(typeof(ParameterFilterElement))
                        .First(f => f.Name == "MACROS_Возд_" + _systemName);
                    filter = filter1 as ParameterFilterElement;
                    filter.SetCategories(categories);
                    filter.SetElementFilter(epf);
                }

                Element eView = new FilteredElementCollector(_doc)
                    .OfClass(typeof(View))
                    .WhereElementIsNotElementType()
                    .FirstOrDefault(v => v.Name == "Схема_Возд_" + _systemName);
                if (null == eView)
                {
                    ElementId copyViewId = view.Duplicate(ViewDuplicateOption.Duplicate);
                    View copiedView = _doc.GetElement(copyViewId) as View;
                    copiedView.Name = "Схема_Возд__" + _systemName;
                    copiedView.AddFilter(filter.Id);
                    copiedView.SetFilterVisibility(filter.Id, false);
                }

                tr.Commit();
            }

        }



        private IList<string> GetDuctSystemNames(Document _doc)
        {
            return new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_DuctSystem)
                .WhereElementIsNotElementType()
                .Select(s => s.Name)
                .ToList();
        }
        public void CreateDuctSystemViews()
        {
            Document doc = this.Document;
            if (doc.ActiveView.ViewType != ViewType.ProjectBrowser)
            {
                using (TransactionGroup trg = new TransactionGroup(doc, "Копирование значений имя системы"))
                {
                    trg.Start();
                    foreach (BuiltInCategory cat in GetDuctCategories())
                    {
                        IList<Element> elementsByCat = new FilteredElementCollector(doc)
                            .OfCategory(cat)
                            .WhereElementIsNotElementType()
                            .ToList();
                        if (elementsByCat.Count > 0)
                        {
                           // CopySystemNameValue(doc, elementsByCat);
                        }

                    }
                    trg.Assimilate();
                }
                TaskDialog td = new TaskDialog("Copy views");

                td.Id = "ID_TaskDialog_Copy_Views";
                td.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
                td.Title = "Создание копий видов с применением фильтра";
                td.TitleAutoPrefix = false;
                td.AllowCancellation = true;
                td.MainInstruction = "Данные из параметра Имя системы для всех элементов систем воздуховодов скопированы";
                td.MainContent = "Хотите создать копии текущего вида с применением фильтров по системам?";
                td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Да, создать фильтры и виды");
                td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Нет");
                TaskDialogResult tdRes = td.Show();
                if (tdRes == TaskDialogResult.CommandLink1)
                {
                    Element sysNameParamElement = new FilteredElementCollector(doc)
                        .OfClass(typeof(ParameterElement))
                        .Where(p => p.Name == "ИмяСистемы")
                        .FirstOrDefault();
                    ParameterElement sysNameParam = sysNameParamElement as ParameterElement;
                    //var 
                    foreach (string systemName in GetDuctSystemNames(doc))
                    {
                        CreateFilterForDuctSystem(doc, sysNameParam, systemName);
                    }
                }
            }
            else
            {
                TaskDialog.Show("Предупреждение", "Не активирован вид для создания копий с применением фильтра");
            }

        }
        private void CopySystemNameValue(Document _doc, IList<Element> _elements)
        {
            using (Transaction tr = new Transaction(_doc, "CopyNames"))
            {
                tr.Start();
                foreach (Element curElement in _elements)
                {
                    string rbs_name = curElement.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM).AsString();
                    FamilyInstance fInstance = curElement as FamilyInstance;
                    if (null != fInstance)
                    {
                        if (null != fInstance.SuperComponent)
                        {
                            rbs_name = fInstance.SuperComponent.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM).AsString();
                            fInstance.LookupParameter("ИмяСистемы").Set(rbs_name);
                        }
                        else
                        {
                            fInstance.LookupParameter("ИмяСистемы").Set(rbs_name);
                        }
                    }
                    else
                    {
                        curElement.LookupParameter("ИмяСистемы").Set(rbs_name);
                    }

                }
                tr.Commit();
            }
        }
        private IList<BuiltInCategory> GetDuctCategories()
        {
            IList<BuiltInCategory> cats = new List<BuiltInCategory>();
            cats.Add(BuiltInCategory.OST_DuctAccessory);
            cats.Add(BuiltInCategory.OST_DuctCurves);
            cats.Add(BuiltInCategory.OST_DuctFitting);
            cats.Add(BuiltInCategory.OST_DuctInsulations);
            cats.Add(BuiltInCategory.OST_DuctTerminal);
            cats.Add(BuiltInCategory.OST_FlexDuctCurves);
            cats.Add(BuiltInCategory.OST_PlaceHolderDucts);
            cats.Add(BuiltInCategory.OST_MechanicalEquipment);
            return cats;
        }

        #endregion
    }
}



