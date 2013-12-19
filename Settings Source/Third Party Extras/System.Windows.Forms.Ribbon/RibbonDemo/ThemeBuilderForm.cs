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
    public partial class ThemeBuilderForm : Form
    {
        Dictionary<RibbonColorPart, Panel> dicPanel = new Dictionary<RibbonColorPart, Panel>();
        Dictionary<RibbonColorPart, TextBox> dicTxt = new Dictionary<RibbonColorPart, TextBox>();

        public ThemeBuilderForm()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            LoadTheme();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        void LoadTheme()
        {           
            RibbonProfesionalRendererColorTable r = Theme.ColorTable;

            txtAuthor.Text = r.ThemeAuthor;
            txtAuthorEmail.Text = r.ThemeAuthorEmail;
            txtAuthorWebsite.Text = r.ThemeAuthorWebsite;
            txtDateCreated.Text = r.ThemeDateCreated;
            txtThemeName.Text = r.ThemeName;
            flowLayoutPanel1.Controls.Remove(tableLayoutPanel1);
            tableLayoutPanel1 = null;

            foreach (KeyValuePair<RibbonColorPart, Panel> kv in dicPanel)
            {

                Panel p = kv.Value;
                p = null;
            }

            foreach (KeyValuePair<RibbonColorPart, TextBox> kv in dicTxt)
            {
                TextBox t = kv.Value;
                t = null;
            }

            dicPanel.Clear();
            dicTxt.Clear();

            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(c1);
            tableLayoutPanel1.Controls.Add(c2);
            tableLayoutPanel1.Controls.Add(c3);
            tableLayoutPanel1.Controls.Add(c4);

            int count = Enum.GetNames(typeof(RibbonColorPart)).Length;
            for (int i = 0; i < count; i++)
            {
                Label l = new Label();
                l.Width = 180;
                l.Text = ((RibbonColorPart)i).ToString();

                Panel p = new Panel();
                p.Height = 16;
                p.Width = 100;
                p.BorderStyle = BorderStyle.FixedSingle;
                p.BackColor = r.GetColor((RibbonColorPart)i);
                dicPanel[(RibbonColorPart)i] = p;

                TextBox t = new TextBox();
                t.Tag = (RibbonColorPart)i;
                t.Text = r.GetColorHexStr((RibbonColorPart)i);
                t.LostFocus += new EventHandler(t_LostFocus);
                t.KeyPress += new KeyPressEventHandler(t_KeyPress);
                t.TextChanged += new EventHandler(t_LostFocus);
                dicTxt[(RibbonColorPart)i] = t;

                Button b = new Button();
                b.Text = "Get Color";
                b.Tag = (RibbonColorPart)i;
                b.Click += new EventHandler(b_Click);

                tableLayoutPanel1.Controls.Add(l);
                tableLayoutPanel1.Controls.Add(p);
                tableLayoutPanel1.Controls.Add(t);
                tableLayoutPanel1.Controls.Add(b);
            }
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(tableLayoutPanel1);
            ribbon1.Refresh();
            this.Refresh();
        }

        void b_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            d.FullOpen = true;
            d.AllowFullOpen = true;
            if (d.ShowDialog() == DialogResult.OK)
            {
                RibbonColorPart rcp = (RibbonColorPart)((Button)sender).Tag;
                RefreshColor(rcp, d.Color);
            }
        }

        void t_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text.Length == 7)
                {
                    Color color = Theme.ColorTable.FromHex(((TextBox)sender).Text);
                    RibbonColorPart rcp = (RibbonColorPart)((TextBox)sender).Tag;
                    RefreshColor(rcp, color);
                }
                else if (((TextBox)sender).Text.Length > 7)
                    MessageBox.Show("Value is to long.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void t_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text.Length == 7)
                {
                    Color color = Theme.ColorTable.FromHex(((TextBox)sender).Text);
                    RibbonColorPart rcp = (RibbonColorPart)((TextBox)sender).Tag;
                    RefreshColor(rcp, color);
                }
                else if (((TextBox)sender).Text.Length > 7)
                    MessageBox.Show("Value is to long.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void RefreshColor(RibbonColorPart rcp, Color color)
        {
            Theme.ColorTable.SetColor(rcp, color);
            dicPanel[rcp].BackColor = color;
            dicTxt[rcp].Text = Theme.ColorTable.GetColorHexStr(rcp);
            ribbon1.Refresh();
            this.Refresh();
        }

        private void btGenerateThemeClass_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Class Name is empty.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Drawing;");
            sb.AppendLine();
            sb.AppendLine("namespace System.Windows.Forms");
            sb.AppendLine("{");
            sb.AppendLine("    public class " + textBox1.Text + " : RibbonProfesionalRendererColorTable");
            sb.AppendLine("    {");
            sb.AppendLine("        public " + textBox1.Text + "()");
            sb.AppendLine("        {");
            sb.AppendLine("            // Rebuild the solution for the first time");
            sb.AppendLine("            // for this ColorTable to take effect.");
            sb.AppendLine("            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically");
            sb.AppendLine();
            sb.AppendLine("            ThemeName = \"" + txtThemeName.Text + "\";");
            sb.AppendLine("            ThemeAuthor = \"" + txtAuthor.Text + "\";");
            sb.AppendLine("            ThemeAuthorWebsite = \"" + txtAuthorWebsite.Text + "\";");
            sb.AppendLine("            ThemeAuthorEmail = \"" + txtAuthorEmail.Text + "\";");
            sb.AppendLine("            ThemeDateCreated = \"" + txtDateCreated.Text + "\";");
            sb.AppendLine();

            int count = Enum.GetNames(typeof(RibbonColorPart)).Length;
            for (int i = 0; i < count; i++)
            {
                sb.AppendLine("            " + ((RibbonColorPart)i).ToString() + " = FromHex(\"" + Theme.ColorTable.GetColorHexStr((RibbonColorPart)i) + "\");");
            }

            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Color FromHex(string hex)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (hex.StartsWith(\"#\"))");
            sb.AppendLine("                hex = hex.Substring(1);");
            sb.AppendLine();
            sb.AppendLine("            if (hex.Length != 6) throw new Exception(\"Color not valid\");");
            sb.AppendLine();
            sb.AppendLine("            return Color.FromArgb(");
            sb.AppendLine("                int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),");
            sb.AppendLine("                int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),");
            sb.AppendLine("                int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            FormClassResult f = new FormClassResult(sb.ToString());
            f.ShowDialog();
        }

        private void btLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.Filter = "ini or xml|*.ini;*.xml";
            if (of.ShowDialog() == DialogResult.OK)
            {
                txtThemeFile.Text = of.FileName;
                string a = System.IO.File.ReadAllText(of.FileName);
                string ext = System.IO.Path.GetExtension(of.FileName);
                if (ext.ToLower() == ".ini")
                    Theme.ColorTable.ReadThemeIniFile(a);
                else if (ext.ToLower() == ".xml")
                    Theme.ColorTable.ReadThemeXmlFile(a);
                LoadTheme();
            }
        }

        private void btThemeSaveIni_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "ini|*.ini";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Theme.ColorTable.ThemeName = txtThemeName.Text;
                    Theme.ColorTable.ThemeAuthor = txtAuthor.Text;
                    Theme.ColorTable.ThemeAuthorEmail = txtAuthorEmail.Text;
                    Theme.ColorTable.ThemeAuthorWebsite = txtAuthorWebsite.Text;
                    Theme.ColorTable.ThemeDateCreated = txtDateCreated.Text;
                    string a = Theme.ColorTable.WriteThemeIniFile();
                    System.IO.File.WriteAllText(f.FileName, a, Encoding.UTF8);
                    MessageBox.Show("Saved at:\r\n" + f.FileName, "Save");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btThemeSaveXML_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "xml|*.xml";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Theme.ColorTable.ThemeName = txtThemeName.Text;
                    Theme.ColorTable.ThemeAuthor = txtAuthor.Text;
                    Theme.ColorTable.ThemeAuthorEmail = txtAuthorEmail.Text;
                    Theme.ColorTable.ThemeAuthorWebsite = txtAuthorWebsite.Text;
                    Theme.ColorTable.ThemeDateCreated = txtDateCreated.Text;
                    string a = Theme.ColorTable.WriteThemeXmlFile();
                    System.IO.File.WriteAllText(f.FileName, a, Encoding.UTF8);
                    MessageBox.Show("Saved at:\r\n" + f.FileName, "Save");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboChooseTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChooseTheme.Enabled = false;

            if (cboChooseTheme.Text == "Black")
                Theme.ThemeColor = RibbonTheme.Black;
            else if (cboChooseTheme.Text == "Green")
                Theme.ThemeColor = RibbonTheme.Green;
            else if (cboChooseTheme.Text == "Purple")
                Theme.ThemeColor = RibbonTheme.Purple;
            else if (cboChooseTheme.Text == "JellyBelly")
                Theme.ThemeColor = RibbonTheme.JellyBelly;
            else if (cboChooseTheme.Text == "Halloween")
                Theme.ThemeColor = RibbonTheme.Halloween;
            else
                Theme.ThemeColor = RibbonTheme.Normal;
            
            LoadTheme();

            this.Refresh();

            cboChooseTheme.Enabled = true;
            cboChooseTheme.Focus();
        }

        private void cboOfficeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOfficeStyle.Text == "Office 2007")
                ribbon1.OrbStyle = RibbonOrbStyle.Office_2007;
            if (cboOfficeStyle.Text == "Office 2010")
                ribbon1.OrbStyle = RibbonOrbStyle.Office_2010;
            if (cboOfficeStyle.Text == "Office 2013")
                ribbon1.OrbStyle = RibbonOrbStyle.Office_2013;
        }
    }
}
