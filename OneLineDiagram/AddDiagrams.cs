/*
namespace Diagrams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Form = System.Windows.Forms.Form;
    using View = Autodesk.Revit.DB.View;

    public partial class AddDiagrams : Form
    {
        private readonly Dictionary<string, FamilyInstance> _shieldsDictionary;

        public AddDiagrams(FamilyInstance[] shields)
        {
            InitializeComponent();
            shieldsTreeView.AfterCheck += Node_AfterCheck;
            var shieldsDictionary = shields.ToDictionary(x => x.UniqueId);
            _shieldsDictionary = shieldsDictionary;
            var q = shields.GroupBy(x =>
            {
                var index = x.Name.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                var subName = x.Name.Substring(0, index > 0 ? index : x.Name.Length - 1);
                return subName;
            });
            foreach (var pair in q)
            {
                var newNode = shieldsTreeView.Nodes.Add(pair.Key);
                foreach (var instance in pair)
                {
                    newNode.Nodes.Add(instance.UniqueId, instance.Name);
                }
            }
        }

        private void Node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. #1#
                    CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAll Children Nodes method recursively.
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void BuiltButton_Click(object sender, EventArgs e)
        {
            //var shieldsList = new List<FamilyInstance>();
            Close();
            foreach (TreeNode node in shieldsTreeView.Nodes)
            {
                foreach (TreeNode sheetNode in node.Nodes)
                {
                    var name = sheetNode.Name;
                    if (sheetNode.Checked)
                    {
                        var flag = OneLineDiagram.OneLineDiagramBuiltDiagram.DrawDiagram(_shieldsDictionary[sheetNode.Name]);
                        if (!flag)
                        {
                        }
                    }
                }
            }
            //TaskDialog.Show("Info", "Успешно!");
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Close();
            var commandData = OneLineDiagram.OneLineDiagramBuiltDiagram.CommandData;
            var uiApp = commandData?.Application;
            var uiDoc = uiApp?.ActiveUIDocument;
            var app = uiApp?.Application;
            var doc = uiDoc?.Document;

            //var viewsDict = new FilteredElementCollector(doc)
            //    .OfClass(typeof(ViewDrafting))
            //    .OfType<ViewDrafting>()
            //    .Where(x => x.LookupParameter("Назначение вида")?.AsString() == "Однолинейные схемы")
            //    .ToDictionary(x => x.ViewName);
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
                .WherePasses(filter)
                .OfType<FamilyInstance>();
            foreach (TreeNode node in shieldsTreeView.Nodes)
            {
                foreach (TreeNode sheetNode in node.Nodes)
                {
                    var uniqueName = sheetNode.Name;
                    if (sheetNode.Checked)
                    {
                        //виды где есть аннотация со ссылкой на этот щит
                        var views = heads.Where(an =>
                                {
                                    var uniqueId = an.LookupParameter("ID электрического щита")?.AsString();
                                    return string.CompareOrdinal(uniqueId, uniqueName) == 0;
                                    //return uniqueId == uniqueName;
                                    //var shield = doc.GetElement(uniqueId) as FamilyInstance;
                                    //var viewID = an.OwnerViewId;
                                    //var view = doc.GetElement(viewID) as Autodesk.Revit.DB.View;
                                }
                            )
                            .Select(an => an.OwnerViewId)
                            .Select(x => doc.GetElement(x) as View);
                        foreach (var view in views)
                        {
                            var flag = OneLineDiagramUpdateDiagram.UpdateScheme(commandData, view);
                            if (flag is Result.Failed)
                            {
                            }
                        }
                    }
                }
            }
            //TaskDialog.Show("Info", "Успешно!");
        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            var flag = shieldsTreeView
                .Nodes
                .Cast<TreeNode>()
                .Any(x => x.Checked == true);
            foreach (TreeNode node in shieldsTreeView.Nodes)
            {
                node.Checked = !flag;
                foreach (TreeNode sheetNode in node.Nodes)
                {
                    sheetNode.Checked = !flag;
                }
            }
        }
    }
}
*/
