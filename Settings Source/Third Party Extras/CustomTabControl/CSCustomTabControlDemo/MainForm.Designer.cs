/*
 * Created by SharpDevelop.
 * User: mjackson
 * Date: 28/06/2010
 * Time: 13:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace CSCustomTabControlDemo
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.customTabControl1 = new System.Windows.Forms.CustomTabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.customTabControl2 = new System.Windows.Forms.CustomTabControl();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.tabPage8 = new System.Windows.Forms.TabPage();
			this.tabPage9 = new System.Windows.Forms.TabPage();
			this.tabPage10 = new System.Windows.Forms.TabPage();
			this.customTabControl3 = new System.Windows.Forms.CustomTabControl();
			this.tabPage11 = new System.Windows.Forms.TabPage();
			this.tabPage12 = new System.Windows.Forms.TabPage();
			this.tabPage13 = new System.Windows.Forms.TabPage();
			this.tabPage14 = new System.Windows.Forms.TabPage();
			this.tabPage15 = new System.Windows.Forms.TabPage();
			this.customTabControl4 = new System.Windows.Forms.CustomTabControl();
			this.tabPage16 = new System.Windows.Forms.TabPage();
			this.tabPage17 = new System.Windows.Forms.TabPage();
			this.tabPage18 = new System.Windows.Forms.TabPage();
			this.tabPage19 = new System.Windows.Forms.TabPage();
			this.tabPage20 = new System.Windows.Forms.TabPage();
			this.customTabControl5 = new System.Windows.Forms.CustomTabControl();
			this.tabPage21 = new System.Windows.Forms.TabPage();
			this.tabPage22 = new System.Windows.Forms.TabPage();
			this.tabPage23 = new System.Windows.Forms.TabPage();
			this.tabPage24 = new System.Windows.Forms.TabPage();
			this.tabPage25 = new System.Windows.Forms.TabPage();
			this.customTabControl6 = new System.Windows.Forms.CustomTabControl();
			this.tabPage26 = new System.Windows.Forms.TabPage();
			this.tabPage27 = new System.Windows.Forms.TabPage();
			this.tabPage28 = new System.Windows.Forms.TabPage();
			this.tabPage29 = new System.Windows.Forms.TabPage();
			this.tabPage30 = new System.Windows.Forms.TabPage();
			this.customTabControl7 = new System.Windows.Forms.CustomTabControl();
			this.tabPage31 = new System.Windows.Forms.TabPage();
			this.tabPage32 = new System.Windows.Forms.TabPage();
			this.tabPage33 = new System.Windows.Forms.TabPage();
			this.tabPage34 = new System.Windows.Forms.TabPage();
			this.tabPage35 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.customTabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.customTabControl2.SuspendLayout();
			this.customTabControl3.SuspendLayout();
			this.customTabControl4.SuspendLayout();
			this.customTabControl5.SuspendLayout();
			this.customTabControl6.SuspendLayout();
			this.customTabControl7.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "battery.png");
			this.imageList1.Images.SetKeyName(1, "book_open.png");
			this.imageList1.Images.SetKeyName(2, "brush3.png");
			this.imageList1.Images.SetKeyName(3, "calculator.png");
			this.imageList1.Images.SetKeyName(4, "cd_music.png");
			this.imageList1.Images.SetKeyName(5, "Close");
			this.imageList1.Images.SetKeyName(6, "google_favicon.png");
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// customTabControl1
			// 
			this.customTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl1.Controls.Add(this.tabPage1);
			this.customTabControl1.Controls.Add(this.tabPage2);
			this.customTabControl1.Controls.Add(this.tabPage3);
			this.customTabControl1.Controls.Add(this.tabPage4);
			this.customTabControl1.Controls.Add(this.tabPage5);
			// 
			// 
			// 
			this.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl1.DisplayStyleProvider.FocusTrack = true;
			this.customTabControl1.DisplayStyleProvider.HotTrack = true;
			this.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.customTabControl1.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl1.DisplayStyleProvider.Overlap = 0;
			this.customTabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
			this.customTabControl1.DisplayStyleProvider.Radius = 2;
			this.customTabControl1.DisplayStyleProvider.ShowTabCloser = false;
			this.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl1.HotTrack = true;
			this.customTabControl1.ImageList = this.imageList1;
			this.customTabControl1.Location = new System.Drawing.Point(12, 12);
			this.customTabControl1.Name = "customTabControl1";
			this.customTabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.customTabControl1.SelectedIndex = 0;
			this.customTabControl1.Size = new System.Drawing.Size(486, 72);
			this.customTabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.textBox1);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.ImageKey = "(none)";
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(478, 44);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Allge&mein";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(108, 10);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(27, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.ImageKey = "(none)";
			this.tabPage2.Location = new System.Drawing.Point(4, 26);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(478, 42);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage&2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.ImageKey = "brush3.png";
			this.tabPage3.Location = new System.Drawing.Point(4, 26);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(478, 42);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.ImageKey = "(none)";
			this.tabPage4.Location = new System.Drawing.Point(4, 26);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(478, 42);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tabPage5
			// 
			this.tabPage5.ImageKey = "cd_music.png";
			this.tabPage5.Location = new System.Drawing.Point(4, 26);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(478, 42);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// customTabControl2
			// 
			this.customTabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl2.Controls.Add(this.tabPage6);
			this.customTabControl2.Controls.Add(this.tabPage7);
			this.customTabControl2.Controls.Add(this.tabPage8);
			this.customTabControl2.Controls.Add(this.tabPage9);
			this.customTabControl2.Controls.Add(this.tabPage10);
			this.customTabControl2.DisplayStyle = System.Windows.Forms.TabStyle.VisualStudio;
			// 
			// 
			// 
			this.customTabControl2.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl2.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl2.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl2.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl2.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl2.DisplayStyleProvider.HotTrack = true;
			this.customTabControl2.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.customTabControl2.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl2.DisplayStyleProvider.Overlap = 7;
			this.customTabControl2.DisplayStyleProvider.Padding = new System.Drawing.Point(14, 1);
			this.customTabControl2.DisplayStyleProvider.ShowTabCloser = false;
			this.customTabControl2.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl2.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl2.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl2.HotTrack = true;
			this.customTabControl2.ImageList = this.imageList1;
			this.customTabControl2.Location = new System.Drawing.Point(12, 94);
			this.customTabControl2.Name = "customTabControl2";
			this.customTabControl2.SelectedIndex = 0;
			this.customTabControl2.Size = new System.Drawing.Size(486, 72);
			this.customTabControl2.TabIndex = 2;
			// 
			// tabPage6
			// 
			this.tabPage6.ImageKey = "(none)";
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(478, 46);
			this.tabPage6.TabIndex = 0;
			this.tabPage6.Text = "Allgemein";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// tabPage7
			// 
			this.tabPage7.ImageKey = "book_open.png";
			this.tabPage7.Location = new System.Drawing.Point(4, 24);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage7.Size = new System.Drawing.Size(478, 44);
			this.tabPage7.TabIndex = 1;
			this.tabPage7.Text = "tabPage7";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// tabPage8
			// 
			this.tabPage8.ImageKey = "(none)";
			this.tabPage8.Location = new System.Drawing.Point(4, 24);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage8.Size = new System.Drawing.Size(478, 44);
			this.tabPage8.TabIndex = 2;
			this.tabPage8.Text = "tabPage8";
			this.tabPage8.UseVisualStyleBackColor = true;
			// 
			// tabPage9
			// 
			this.tabPage9.ImageKey = "(none)";
			this.tabPage9.Location = new System.Drawing.Point(4, 24);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage9.Size = new System.Drawing.Size(478, 44);
			this.tabPage9.TabIndex = 3;
			this.tabPage9.Text = "tabPage9";
			this.tabPage9.UseVisualStyleBackColor = true;
			// 
			// tabPage10
			// 
			this.tabPage10.ImageKey = "cd_music.png";
			this.tabPage10.Location = new System.Drawing.Point(4, 24);
			this.tabPage10.Name = "tabPage10";
			this.tabPage10.Size = new System.Drawing.Size(478, 44);
			this.tabPage10.TabIndex = 4;
			this.tabPage10.Text = "tabPage10";
			this.tabPage10.UseVisualStyleBackColor = true;
			// 
			// customTabControl3
			// 
			this.customTabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl3.Controls.Add(this.tabPage11);
			this.customTabControl3.Controls.Add(this.tabPage12);
			this.customTabControl3.Controls.Add(this.tabPage13);
			this.customTabControl3.Controls.Add(this.tabPage14);
			this.customTabControl3.Controls.Add(this.tabPage15);
			this.customTabControl3.DisplayStyle = System.Windows.Forms.TabStyle.Rounded;
			// 
			// 
			// 
			this.customTabControl3.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl3.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl3.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl3.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl3.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl3.DisplayStyleProvider.HotTrack = true;
			this.customTabControl3.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.customTabControl3.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl3.DisplayStyleProvider.Overlap = 5;
			this.customTabControl3.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
			this.customTabControl3.DisplayStyleProvider.Radius = 10;
			this.customTabControl3.DisplayStyleProvider.ShowTabCloser = false;
			this.customTabControl3.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl3.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl3.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl3.HotTrack = true;
			this.customTabControl3.ImageList = this.imageList1;
			this.customTabControl3.Location = new System.Drawing.Point(12, 328);
			this.customTabControl3.Name = "customTabControl3";
			this.customTabControl3.RightToLeftLayout = true;
			this.customTabControl3.SelectedIndex = 0;
			this.customTabControl3.Size = new System.Drawing.Size(486, 72);
			this.customTabControl3.TabIndex = 3;
			// 
			// tabPage11
			// 
			this.tabPage11.ImageKey = "(none)";
			this.tabPage11.Location = new System.Drawing.Point(4, 24);
			this.tabPage11.Name = "tabPage11";
			this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage11.Size = new System.Drawing.Size(478, 44);
			this.tabPage11.TabIndex = 0;
			this.tabPage11.Text = "Allgemein";
			this.tabPage11.UseVisualStyleBackColor = true;
			// 
			// tabPage12
			// 
			this.tabPage12.ImageKey = "(none)";
			this.tabPage12.Location = new System.Drawing.Point(4, 26);
			this.tabPage12.Name = "tabPage12";
			this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage12.Size = new System.Drawing.Size(478, 42);
			this.tabPage12.TabIndex = 1;
			this.tabPage12.Text = "tabPage12";
			this.tabPage12.UseVisualStyleBackColor = true;
			// 
			// tabPage13
			// 
			this.tabPage13.ImageKey = "brush3.png";
			this.tabPage13.Location = new System.Drawing.Point(4, 26);
			this.tabPage13.Name = "tabPage13";
			this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage13.Size = new System.Drawing.Size(478, 42);
			this.tabPage13.TabIndex = 2;
			this.tabPage13.Text = "tabPage13";
			this.tabPage13.UseVisualStyleBackColor = true;
			// 
			// tabPage14
			// 
			this.tabPage14.ImageKey = "calculator.png";
			this.tabPage14.Location = new System.Drawing.Point(4, 26);
			this.tabPage14.Name = "tabPage14";
			this.tabPage14.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage14.Size = new System.Drawing.Size(478, 42);
			this.tabPage14.TabIndex = 3;
			this.tabPage14.Text = "tabPage14";
			this.tabPage14.UseVisualStyleBackColor = true;
			// 
			// tabPage15
			// 
			this.tabPage15.ImageKey = "cd_music.png";
			this.tabPage15.Location = new System.Drawing.Point(4, 26);
			this.tabPage15.Name = "tabPage15";
			this.tabPage15.Size = new System.Drawing.Size(478, 42);
			this.tabPage15.TabIndex = 4;
			this.tabPage15.Text = "tabPage15";
			this.tabPage15.UseVisualStyleBackColor = true;
			// 
			// customTabControl4
			// 
			this.customTabControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl4.Controls.Add(this.tabPage16);
			this.customTabControl4.Controls.Add(this.tabPage17);
			this.customTabControl4.Controls.Add(this.tabPage18);
			this.customTabControl4.Controls.Add(this.tabPage19);
			this.customTabControl4.Controls.Add(this.tabPage20);
			this.customTabControl4.DisplayStyle = System.Windows.Forms.TabStyle.Angled;
			// 
			// 
			// 
			this.customTabControl4.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl4.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl4.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl4.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl4.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl4.DisplayStyleProvider.HotTrack = true;
			this.customTabControl4.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.customTabControl4.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl4.DisplayStyleProvider.Overlap = 7;
			this.customTabControl4.DisplayStyleProvider.Padding = new System.Drawing.Point(10, 3);
			this.customTabControl4.DisplayStyleProvider.Radius = 10;
			this.customTabControl4.DisplayStyleProvider.ShowTabCloser = false;
			this.customTabControl4.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl4.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl4.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl4.HotTrack = true;
			this.customTabControl4.ImageList = this.imageList1;
			this.customTabControl4.Location = new System.Drawing.Point(12, 406);
			this.customTabControl4.Name = "customTabControl4";
			this.customTabControl4.SelectedIndex = 0;
			this.customTabControl4.Size = new System.Drawing.Size(486, 74);
			this.customTabControl4.TabIndex = 4;
			// 
			// tabPage16
			// 
			this.tabPage16.ImageKey = "battery.png";
			this.tabPage16.Location = new System.Drawing.Point(4, 24);
			this.tabPage16.Name = "tabPage16";
			this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage16.Size = new System.Drawing.Size(478, 46);
			this.tabPage16.TabIndex = 0;
			this.tabPage16.Text = "Allgemein";
			this.tabPage16.UseVisualStyleBackColor = true;
			// 
			// tabPage17
			// 
			this.tabPage17.ImageKey = "book_open.png";
			this.tabPage17.Location = new System.Drawing.Point(4, 26);
			this.tabPage17.Name = "tabPage17";
			this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage17.Size = new System.Drawing.Size(478, 44);
			this.tabPage17.TabIndex = 1;
			this.tabPage17.Text = "tabPage17";
			this.tabPage17.UseVisualStyleBackColor = true;
			// 
			// tabPage18
			// 
			this.tabPage18.ImageKey = "brush3.png";
			this.tabPage18.Location = new System.Drawing.Point(4, 26);
			this.tabPage18.Name = "tabPage18";
			this.tabPage18.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage18.Size = new System.Drawing.Size(478, 44);
			this.tabPage18.TabIndex = 2;
			this.tabPage18.Text = "tabPage18";
			this.tabPage18.UseVisualStyleBackColor = true;
			// 
			// tabPage19
			// 
			this.tabPage19.ImageKey = "(none)";
			this.tabPage19.Location = new System.Drawing.Point(4, 26);
			this.tabPage19.Name = "tabPage19";
			this.tabPage19.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage19.Size = new System.Drawing.Size(478, 44);
			this.tabPage19.TabIndex = 3;
			this.tabPage19.Text = "tabPage19";
			this.tabPage19.UseVisualStyleBackColor = true;
			// 
			// tabPage20
			// 
			this.tabPage20.ImageKey = "(none)";
			this.tabPage20.Location = new System.Drawing.Point(4, 26);
			this.tabPage20.Name = "tabPage20";
			this.tabPage20.Size = new System.Drawing.Size(478, 44);
			this.tabPage20.TabIndex = 4;
			this.tabPage20.Text = "tabPage20";
			this.tabPage20.UseVisualStyleBackColor = true;
			// 
			// customTabControl5
			// 
			this.customTabControl5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl5.Controls.Add(this.tabPage21);
			this.customTabControl5.Controls.Add(this.tabPage22);
			this.customTabControl5.Controls.Add(this.tabPage23);
			this.customTabControl5.Controls.Add(this.tabPage24);
			this.customTabControl5.Controls.Add(this.tabPage25);
			this.customTabControl5.DisplayStyle = System.Windows.Forms.TabStyle.Chrome;
			// 
			// 
			// 
			this.customTabControl5.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl5.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl5.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl5.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl5.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.White;
			this.customTabControl5.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl5.DisplayStyleProvider.HotTrack = true;
			this.customTabControl5.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.customTabControl5.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl5.DisplayStyleProvider.Overlap = 16;
			this.customTabControl5.DisplayStyleProvider.Padding = new System.Drawing.Point(7, 5);
			this.customTabControl5.DisplayStyleProvider.Radius = 16;
			this.customTabControl5.DisplayStyleProvider.ShowTabCloser = true;
			this.customTabControl5.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl5.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl5.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl5.HotTrack = true;
			this.customTabControl5.ImageList = this.imageList1;
			this.customTabControl5.Location = new System.Drawing.Point(12, 172);
			this.customTabControl5.Name = "customTabControl5";
			this.customTabControl5.RightToLeftLayout = true;
			this.customTabControl5.SelectedIndex = 0;
			this.customTabControl5.Size = new System.Drawing.Size(486, 72);
			this.customTabControl5.TabIndex = 5;
			// 
			// tabPage21
			// 
			this.tabPage21.ImageKey = "(none)";
			this.tabPage21.Location = new System.Drawing.Point(4, 28);
			this.tabPage21.Name = "tabPage21";
			this.tabPage21.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage21.Size = new System.Drawing.Size(478, 40);
			this.tabPage21.TabIndex = 0;
			this.tabPage21.Text = "Allgemein";
			this.tabPage21.UseVisualStyleBackColor = true;
			// 
			// tabPage22
			// 
			this.tabPage22.ImageKey = "google_favicon.png";
			this.tabPage22.Location = new System.Drawing.Point(4, 30);
			this.tabPage22.Name = "tabPage22";
			this.tabPage22.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage22.Size = new System.Drawing.Size(478, 38);
			this.tabPage22.TabIndex = 1;
			this.tabPage22.Text = "tabPage22";
			this.tabPage22.UseVisualStyleBackColor = true;
			// 
			// tabPage23
			// 
			this.tabPage23.ImageKey = "(none)";
			this.tabPage23.Location = new System.Drawing.Point(4, 30);
			this.tabPage23.Name = "tabPage23";
			this.tabPage23.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage23.Size = new System.Drawing.Size(478, 38);
			this.tabPage23.TabIndex = 2;
			this.tabPage23.Text = "tabPage23";
			this.tabPage23.UseVisualStyleBackColor = true;
			// 
			// tabPage24
			// 
			this.tabPage24.ImageKey = "(none)";
			this.tabPage24.Location = new System.Drawing.Point(4, 30);
			this.tabPage24.Name = "tabPage24";
			this.tabPage24.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage24.Size = new System.Drawing.Size(478, 38);
			this.tabPage24.TabIndex = 3;
			this.tabPage24.Text = "tabPage24";
			this.tabPage24.UseVisualStyleBackColor = true;
			// 
			// tabPage25
			// 
			this.tabPage25.ImageKey = "google_favicon.png";
			this.tabPage25.Location = new System.Drawing.Point(4, 30);
			this.tabPage25.Name = "tabPage25";
			this.tabPage25.Size = new System.Drawing.Size(478, 38);
			this.tabPage25.TabIndex = 4;
			this.tabPage25.Text = "tabPage25";
			this.tabPage25.UseVisualStyleBackColor = true;
			// 
			// customTabControl6
			// 
			this.customTabControl6.AllowDrop = true;
			this.customTabControl6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl6.Controls.Add(this.tabPage26);
			this.customTabControl6.Controls.Add(this.tabPage27);
			this.customTabControl6.Controls.Add(this.tabPage28);
			this.customTabControl6.Controls.Add(this.tabPage29);
			this.customTabControl6.Controls.Add(this.tabPage30);
			this.customTabControl6.DisplayStyle = System.Windows.Forms.TabStyle.IE8;
			// 
			// 
			// 
			this.customTabControl6.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.customTabControl6.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
			this.customTabControl6.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl6.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
			this.customTabControl6.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.Red;
			this.customTabControl6.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl6.DisplayStyleProvider.HotTrack = true;
			this.customTabControl6.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.customTabControl6.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl6.DisplayStyleProvider.Overlap = 0;
			this.customTabControl6.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 5);
			this.customTabControl6.DisplayStyleProvider.Radius = 3;
			this.customTabControl6.DisplayStyleProvider.ShowTabCloser = true;
			this.customTabControl6.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
			this.customTabControl6.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
			this.customTabControl6.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl6.HotTrack = true;
			this.customTabControl6.ImageList = this.imageList1;
			this.customTabControl6.Location = new System.Drawing.Point(12, 250);
			this.customTabControl6.Name = "customTabControl6";
			this.customTabControl6.RightToLeftLayout = true;
			this.customTabControl6.SelectedIndex = 0;
			this.customTabControl6.Size = new System.Drawing.Size(486, 72);
			this.customTabControl6.TabIndex = 6;
			// 
			// tabPage26
			// 
			this.tabPage26.ImageKey = "google_favicon.png";
			this.tabPage26.Location = new System.Drawing.Point(4, 28);
			this.tabPage26.Name = "tabPage26";
			this.tabPage26.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage26.Size = new System.Drawing.Size(478, 40);
			this.tabPage26.TabIndex = 0;
			this.tabPage26.Text = "Allgemein";
			this.tabPage26.UseVisualStyleBackColor = true;
			// 
			// tabPage27
			// 
			this.tabPage27.ImageKey = "google_favicon.png";
			this.tabPage27.Location = new System.Drawing.Point(4, 30);
			this.tabPage27.Name = "tabPage27";
			this.tabPage27.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage27.Size = new System.Drawing.Size(478, 38);
			this.tabPage27.TabIndex = 1;
			this.tabPage27.Text = "tabPage27";
			this.tabPage27.UseVisualStyleBackColor = true;
			// 
			// tabPage28
			// 
			this.tabPage28.ImageKey = "(none)";
			this.tabPage28.Location = new System.Drawing.Point(4, 30);
			this.tabPage28.Name = "tabPage28";
			this.tabPage28.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage28.Size = new System.Drawing.Size(478, 38);
			this.tabPage28.TabIndex = 2;
			this.tabPage28.Text = "tabPage28";
			this.tabPage28.UseVisualStyleBackColor = true;
			// 
			// tabPage29
			// 
			this.tabPage29.ImageKey = "(none)";
			this.tabPage29.Location = new System.Drawing.Point(4, 30);
			this.tabPage29.Name = "tabPage29";
			this.tabPage29.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage29.Size = new System.Drawing.Size(478, 38);
			this.tabPage29.TabIndex = 3;
			this.tabPage29.Text = "tabPage29";
			this.tabPage29.UseVisualStyleBackColor = true;
			// 
			// tabPage30
			// 
			this.tabPage30.ImageKey = "google_favicon.png";
			this.tabPage30.Location = new System.Drawing.Point(4, 30);
			this.tabPage30.Name = "tabPage30";
			this.tabPage30.Size = new System.Drawing.Size(478, 38);
			this.tabPage30.TabIndex = 4;
			this.tabPage30.Text = "tabPage30";
			this.tabPage30.UseVisualStyleBackColor = true;
			// 
			// customTabControl7
			// 
			this.customTabControl7.AllowDrop = true;
			this.customTabControl7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.customTabControl7.Controls.Add(this.tabPage31);
			this.customTabControl7.Controls.Add(this.tabPage32);
			this.customTabControl7.Controls.Add(this.tabPage33);
			this.customTabControl7.Controls.Add(this.tabPage34);
			this.customTabControl7.Controls.Add(this.tabPage35);
			this.customTabControl7.DisplayStyle = System.Windows.Forms.TabStyle.VS2010;
			// 
			// 
			// 
			this.customTabControl7.DisplayStyleProvider.BorderColor = System.Drawing.Color.Transparent;
			this.customTabControl7.DisplayStyleProvider.BorderColorHot = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(167)))), ((int)(((byte)(183)))));
			this.customTabControl7.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
			this.customTabControl7.DisplayStyleProvider.CloserColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(99)))), ((int)(((byte)(61)))));
			this.customTabControl7.DisplayStyleProvider.FocusTrack = false;
			this.customTabControl7.DisplayStyleProvider.HotTrack = true;
			this.customTabControl7.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.customTabControl7.DisplayStyleProvider.Opacity = 1F;
			this.customTabControl7.DisplayStyleProvider.Overlap = 0;
			this.customTabControl7.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 5);
			this.customTabControl7.DisplayStyleProvider.Radius = 3;
			this.customTabControl7.DisplayStyleProvider.ShowTabCloser = true;
			this.customTabControl7.DisplayStyleProvider.TextColor = System.Drawing.Color.White;
			this.customTabControl7.DisplayStyleProvider.TextColorDisabled = System.Drawing.Color.WhiteSmoke;
			this.customTabControl7.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
			this.customTabControl7.HotTrack = true;
			this.customTabControl7.ImageList = this.imageList1;
			this.customTabControl7.Location = new System.Drawing.Point(12, 4);
			this.customTabControl7.Name = "customTabControl7";
			this.customTabControl7.RightToLeftLayout = true;
			this.customTabControl7.SelectedIndex = 0;
			this.customTabControl7.Size = new System.Drawing.Size(482, 74);
			this.customTabControl7.TabIndex = 7;
			// 
			// tabPage31
			// 
			this.tabPage31.ImageKey = "battery.png";
			this.tabPage31.Location = new System.Drawing.Point(4, 28);
			this.tabPage31.Name = "tabPage31";
			this.tabPage31.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage31.Size = new System.Drawing.Size(474, 42);
			this.tabPage31.TabIndex = 0;
			this.tabPage31.Text = "Allgemein";
			this.tabPage31.UseVisualStyleBackColor = true;
			// 
			// tabPage32
			// 
			this.tabPage32.ImageKey = "book_open.png";
			this.tabPage32.Location = new System.Drawing.Point(4, 30);
			this.tabPage32.Name = "tabPage32";
			this.tabPage32.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage32.Size = new System.Drawing.Size(474, 40);
			this.tabPage32.TabIndex = 1;
			this.tabPage32.Text = "tabPage32";
			this.tabPage32.UseVisualStyleBackColor = true;
			// 
			// tabPage33
			// 
			this.tabPage33.ImageKey = "brush3.png";
			this.tabPage33.Location = new System.Drawing.Point(4, 30);
			this.tabPage33.Name = "tabPage33";
			this.tabPage33.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage33.Size = new System.Drawing.Size(474, 40);
			this.tabPage33.TabIndex = 2;
			this.tabPage33.Text = "tabPage33";
			this.tabPage33.UseVisualStyleBackColor = true;
			// 
			// tabPage34
			// 
			this.tabPage34.ImageKey = "(none)";
			this.tabPage34.Location = new System.Drawing.Point(4, 30);
			this.tabPage34.Name = "tabPage34";
			this.tabPage34.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage34.Size = new System.Drawing.Size(474, 40);
			this.tabPage34.TabIndex = 3;
			this.tabPage34.Text = "tabPage34";
			this.tabPage34.UseVisualStyleBackColor = true;
			// 
			// tabPage35
			// 
			this.tabPage35.ImageKey = "(none)";
			this.tabPage35.Location = new System.Drawing.Point(4, 30);
			this.tabPage35.Name = "tabPage35";
			this.tabPage35.Size = new System.Drawing.Size(474, 40);
			this.tabPage35.TabIndex = 4;
			this.tabPage35.Text = "tabPage35";
			this.tabPage35.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(60)))), ((int)(((byte)(89)))));
			this.panel1.Controls.Add(this.customTabControl7);
			this.panel1.Location = new System.Drawing.Point(0, 482);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(512, 88);
			this.panel1.TabIndex = 8;
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(510, 570);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.customTabControl1);
			this.Controls.Add(this.customTabControl2);
			this.Controls.Add(this.customTabControl6);
			this.Controls.Add(this.customTabControl5);
			this.Controls.Add(this.customTabControl3);
			this.Controls.Add(this.customTabControl4);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(526, 521);
			this.Name = "MainForm";
			this.Text = "TabControl Demo";
			this.customTabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.customTabControl2.ResumeLayout(false);
			this.customTabControl3.ResumeLayout(false);
			this.customTabControl4.ResumeLayout(false);
			this.customTabControl5.ResumeLayout(false);
			this.customTabControl6.ResumeLayout(false);
			this.customTabControl7.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabPage tabPage35;
		private System.Windows.Forms.TabPage tabPage34;
		private System.Windows.Forms.TabPage tabPage33;
		private System.Windows.Forms.TabPage tabPage32;
		private System.Windows.Forms.TabPage tabPage31;
		private System.Windows.Forms.CustomTabControl customTabControl7;
		private System.Windows.Forms.TabPage tabPage30;
		private System.Windows.Forms.TabPage tabPage29;
		private System.Windows.Forms.TabPage tabPage28;
		private System.Windows.Forms.TabPage tabPage27;
		private System.Windows.Forms.TabPage tabPage26;
		private System.Windows.Forms.CustomTabControl customTabControl6;
		private System.Windows.Forms.TabPage tabPage25;
		private System.Windows.Forms.TabPage tabPage24;
		private System.Windows.Forms.TabPage tabPage23;
		private System.Windows.Forms.TabPage tabPage22;
		private System.Windows.Forms.TabPage tabPage21;
		private System.Windows.Forms.CustomTabControl customTabControl5;
		private System.Windows.Forms.TabPage tabPage20;
		private System.Windows.Forms.TabPage tabPage19;
		private System.Windows.Forms.TabPage tabPage18;
		private System.Windows.Forms.TabPage tabPage17;
		private System.Windows.Forms.TabPage tabPage16;
		private System.Windows.Forms.CustomTabControl customTabControl4;
		private System.Windows.Forms.TabPage tabPage15;
		private System.Windows.Forms.TabPage tabPage14;
		private System.Windows.Forms.TabPage tabPage13;
		private System.Windows.Forms.TabPage tabPage12;
		private System.Windows.Forms.TabPage tabPage11;
		private System.Windows.Forms.CustomTabControl customTabControl3;
		private System.Windows.Forms.TabPage tabPage10;
		private System.Windows.Forms.TabPage tabPage9;
		private System.Windows.Forms.TabPage tabPage8;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.CustomTabControl customTabControl2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.CustomTabControl customTabControl1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ImageList imageList1;
	}
}
