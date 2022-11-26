using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialValues
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
            shieldsTreeView.AfterCheck += node_AfterCheck;
        }
        internal bool[] Flags => new[]
        {
            checkBox1.Checked,
            checkBox2.Checked,
            checkBox3.Checked,
            checkBox4.Checked
        };

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    var flag = ShieldsCheckedListBox.CheckedItems.Count > 0;
        //    for (int i = 0; i < ShieldsCheckedListBox.Items.Count; i++)
        //    {
        //        ShieldsCheckedListBox.SetItemChecked(i, !flag);
        //    }
        //}
        private void node_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
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
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {

        }

        
    }
}
