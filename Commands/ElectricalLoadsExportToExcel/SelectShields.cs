namespace ElectricalLoadsExportToExcel
{
    using System.Linq;
    using System.Windows.Forms;
    using Autodesk.Revit.DB;
    using Form = System.Windows.Forms.Form;

    public partial class SelectShields : Form
    {
        public SelectShields(FamilyInstance[] shields)
        {
            InitializeComponent();
            ShieldsTreeView.AfterCheck += Node_AfterCheck;
            var q = shields.GroupBy(x =>
                {
                    var index = x.Name.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    var subName = x.Name.Substring(0, index > 0 ? index : x.Name.Length - 1);
                    return subName;
                })
                .OrderBy(x => x.Key);
            foreach (var pair in q)
            {
                var newNode = ShieldsTreeView.Nodes.Add(pair.Key);
                foreach (var instance in pair)
                {
                    var node = newNode.Nodes.Add(instance.UniqueId, instance.Name);
                    node.Tag = instance;
                }
            }
        }

        private void Node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
                if (e.Node.Nodes.Count > 0)
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    CheckAllChildNodes(e.Node, e.Node.Checked);
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    CheckAllChildNodes(node, nodeChecked);
            }
        }
    }
}
