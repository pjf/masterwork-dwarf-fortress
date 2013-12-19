using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RibbonDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btBlackForm_Click(object sender, EventArgs e)
        {
            BlackForm f = new BlackForm();
            f.Show();
        }

        private void btHostForm_Click(object sender, EventArgs e)
        {
            HostForm f = new HostForm();
            f.Show();
        }

        private void btTestForm_Click(object sender, EventArgs e)
        {
            TestForm f = new TestForm();
            f.Show();
        }

        private void btMainForm_Click(object sender, EventArgs e)
        {
            MainForm f = new MainForm();
            f.Show();
        }

        private void btnMDIForm_Click(object sender, EventArgs e)
        {
           Form f = new MDIForm();
           f.Show();
        }

        private void btRightToLeft_Click(object sender, EventArgs e)
        {
            RightToLeftForm f = new RightToLeftForm();
            f.Show();
        }

        private void btThemeBuilderForm_Click(object sender, EventArgs e)
        {
            ThemeBuilderForm f = new ThemeBuilderForm();
            f.Show();
        }

    }
}
