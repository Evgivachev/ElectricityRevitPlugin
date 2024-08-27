using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialValues;

public partial class InitialValues : Form
{
    internal bool[] flags => new[]
    {
        checkBox2.Checked,
        checkBox3.Checked,
        checkBox4.Checked,
        checkBox5.Checked,
        checkBox6.Checked,
        checkBox7.Checked
    };

    public InitialValues()
    {
        InitializeComponent();
    }


    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {

        var q1 = new[]
        {
            checkBox2,
            checkBox3,
            checkBox4,
            checkBox5,
            checkBox6,
            checkBox7
        };

        for (var i = 0; i < q1.Length; i++)
        {
            q1[i].Checked = checkBox1.Checked;
            q1[i].Enabled = !checkBox1.Checked;
        }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
        Device1comboBox.Enabled = !checkBox2.Checked;
    }

    private void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
        Device2comboBox.Enabled = !checkBox3.Checked;
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
        CablesComboBox.Enabled = !checkBox4.Checked;
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e)
    {
        TubeComboBox.Enabled = !checkBox5.Checked;
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {
        InstallationСomboBox.Enabled = !checkBox6.Checked;
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e)
    {
        TemperatureComboBox.Enabled = !checkBox7.Checked;
    }
}
