'
' * Created by SharpDevelop.
' * User: mjackson
' * Date: 28/06/2010
' * Time: 13:38
' * 
' * To change this template use Tools | Options | Coding | Edit Standard Headers.
' 

	Partial Class MainForm
		''' <summary>
		''' Designer variable used to keep track of non-visual components.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Disposes resources used by the form.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		''' <summary>
		''' This method is required for Windows Forms designer support.
		''' Do not change the method contents inside the source code editor. The Forms designer might
		''' not be able to load this method if it was changed manually.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
			Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
			Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
			Me.customTabControl1 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage1 = New System.Windows.Forms.TabPage()
			Me.tabPage2 = New System.Windows.Forms.TabPage()
			Me.tabPage3 = New System.Windows.Forms.TabPage()
			Me.tabPage4 = New System.Windows.Forms.TabPage()
			Me.tabPage5 = New System.Windows.Forms.TabPage()
			Me.customTabControl2 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage6 = New System.Windows.Forms.TabPage()
			Me.tabPage7 = New System.Windows.Forms.TabPage()
			Me.tabPage8 = New System.Windows.Forms.TabPage()
			Me.tabPage9 = New System.Windows.Forms.TabPage()
			Me.tabPage10 = New System.Windows.Forms.TabPage()
			Me.customTabControl3 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage11 = New System.Windows.Forms.TabPage()
			Me.tabPage12 = New System.Windows.Forms.TabPage()
			Me.tabPage13 = New System.Windows.Forms.TabPage()
			Me.tabPage14 = New System.Windows.Forms.TabPage()
			Me.tabPage15 = New System.Windows.Forms.TabPage()
			Me.customTabControl4 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage16 = New System.Windows.Forms.TabPage()
			Me.tabPage17 = New System.Windows.Forms.TabPage()
			Me.tabPage18 = New System.Windows.Forms.TabPage()
			Me.tabPage19 = New System.Windows.Forms.TabPage()
			Me.tabPage20 = New System.Windows.Forms.TabPage()
			Me.customTabControl5 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage21 = New System.Windows.Forms.TabPage()
			Me.tabPage22 = New System.Windows.Forms.TabPage()
			Me.tabPage23 = New System.Windows.Forms.TabPage()
			Me.tabPage24 = New System.Windows.Forms.TabPage()
			Me.tabPage25 = New System.Windows.Forms.TabPage()
			Me.customTabControl6 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage26 = New System.Windows.Forms.TabPage()
			Me.tabPage27 = New System.Windows.Forms.TabPage()
			Me.tabPage28 = New System.Windows.Forms.TabPage()
			Me.tabPage29 = New System.Windows.Forms.TabPage()
			Me.tabPage30 = New System.Windows.Forms.TabPage()
			Me.customTabControl7 = New System.Windows.Forms.CustomTabControl()
			Me.tabPage31 = New System.Windows.Forms.TabPage()
			Me.tabPage32 = New System.Windows.Forms.TabPage()
			Me.tabPage33 = New System.Windows.Forms.TabPage()
			Me.tabPage34 = New System.Windows.Forms.TabPage()
			Me.tabPage35 = New System.Windows.Forms.TabPage()
			Me.panel1 = New System.Windows.Forms.Panel()
			Me.customTabControl1.SuspendLayout()
			Me.customTabControl2.SuspendLayout()
			Me.customTabControl3.SuspendLayout()
			Me.customTabControl4.SuspendLayout()
			Me.customTabControl5.SuspendLayout()
			Me.customTabControl6.SuspendLayout()
			Me.customTabControl7.SuspendLayout()
			Me.panel1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = DirectCast(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
			Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
			Me.imageList1.Images.SetKeyName(0, "battery.png")
			Me.imageList1.Images.SetKeyName(1, "book_open.png")
			Me.imageList1.Images.SetKeyName(2, "brush3.png")
			Me.imageList1.Images.SetKeyName(3, "calculator.png")
			Me.imageList1.Images.SetKeyName(4, "cd_music.png")
			Me.imageList1.Images.SetKeyName(5, "Close")
			Me.imageList1.Images.SetKeyName(6, "google_favicon.png")
			' 
			' contextMenuStrip1
			' 
			Me.contextMenuStrip1.Name = "contextMenuStrip1"
			Me.contextMenuStrip1.Size = New System.Drawing.Size(61, 4)
			' 
			' customTabControl1
			' 
			Me.customTabControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl1.Controls.Add(Me.tabPage1)
			Me.customTabControl1.Controls.Add(Me.tabPage2)
			Me.customTabControl1.Controls.Add(Me.tabPage3)
			Me.customTabControl1.Controls.Add(Me.tabPage4)
			Me.customTabControl1.Controls.Add(Me.tabPage5)
			' 
			' 
			' 
			Me.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl1.DisplayStyleProvider.FocusTrack = True
			Me.customTabControl1.DisplayStyleProvider.HotTrack = True
			Me.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.customTabControl1.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl1.DisplayStyleProvider.Overlap = 0
			Me.customTabControl1.DisplayStyleProvider.Padding = New System.Drawing.Point(6, 3)
			Me.customTabControl1.DisplayStyleProvider.Radius = 2
			Me.customTabControl1.DisplayStyleProvider.ShowTabCloser = False
			Me.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl1.HotTrack = True
			Me.customTabControl1.ImageList = Me.imageList1
			Me.customTabControl1.Location = New System.Drawing.Point(12, 12)
			Me.customTabControl1.Name = "customTabControl1"
			Me.customTabControl1.SelectedIndex = 0
			Me.customTabControl1.Size = New System.Drawing.Size(486, 72)
			Me.customTabControl1.TabIndex = 1
			' 
			' tabPage1
			' 
			Me.tabPage1.ImageKey = "(none)"
			Me.tabPage1.Location = New System.Drawing.Point(4, 24)
			Me.tabPage1.Name = "tabPage1"
			Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage1.Size = New System.Drawing.Size(478, 44)
			Me.tabPage1.TabIndex = 0
			Me.tabPage1.Text = "Allgemein"
			Me.tabPage1.UseVisualStyleBackColor = True
			' 
			' tabPage2
			' 
			Me.tabPage2.ImageKey = "(none)"
			Me.tabPage2.Location = New System.Drawing.Point(4, 24)
			Me.tabPage2.Name = "tabPage2"
			Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage2.Size = New System.Drawing.Size(478, 44)
			Me.tabPage2.TabIndex = 1
			Me.tabPage2.Text = "tabPage2"
			Me.tabPage2.UseVisualStyleBackColor = True
			' 
			' tabPage3
			' 
			Me.tabPage3.ImageKey = "brush3.png"
			Me.tabPage3.Location = New System.Drawing.Point(4, 24)
			Me.tabPage3.Name = "tabPage3"
			Me.tabPage3.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage3.Size = New System.Drawing.Size(478, 44)
			Me.tabPage3.TabIndex = 2
			Me.tabPage3.Text = "tabPage3"
			Me.tabPage3.UseVisualStyleBackColor = True
			' 
			' tabPage4
			' 
			Me.tabPage4.ImageKey = "(none)"
			Me.tabPage4.Location = New System.Drawing.Point(4, 24)
			Me.tabPage4.Name = "tabPage4"
			Me.tabPage4.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage4.Size = New System.Drawing.Size(478, 44)
			Me.tabPage4.TabIndex = 3
			Me.tabPage4.Text = "tabPage4"
			Me.tabPage4.UseVisualStyleBackColor = True
			' 
			' tabPage5
			' 
			Me.tabPage5.ImageKey = "cd_music.png"
			Me.tabPage5.Location = New System.Drawing.Point(4, 24)
			Me.tabPage5.Name = "tabPage5"
			Me.tabPage5.Size = New System.Drawing.Size(478, 44)
			Me.tabPage5.TabIndex = 4
			Me.tabPage5.Text = "tabPage5"
			Me.tabPage5.UseVisualStyleBackColor = True
			' 
			' customTabControl2
			' 
			Me.customTabControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl2.Controls.Add(Me.tabPage6)
			Me.customTabControl2.Controls.Add(Me.tabPage7)
			Me.customTabControl2.Controls.Add(Me.tabPage8)
			Me.customTabControl2.Controls.Add(Me.tabPage9)
			Me.customTabControl2.Controls.Add(Me.tabPage10)
			Me.customTabControl2.DisplayStyle = System.Windows.Forms.TabStyle.VisualStudio
			' 
			' 
			' 
			Me.customTabControl2.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl2.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl2.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl2.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl2.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl2.DisplayStyleProvider.HotTrack = True
			Me.customTabControl2.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.customTabControl2.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl2.DisplayStyleProvider.Overlap = 7
			Me.customTabControl2.DisplayStyleProvider.Padding = New System.Drawing.Point(14, 1)
			Me.customTabControl2.DisplayStyleProvider.ShowTabCloser = False
			Me.customTabControl2.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl2.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl2.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl2.HotTrack = True
			Me.customTabControl2.ImageList = Me.imageList1
			Me.customTabControl2.Location = New System.Drawing.Point(12, 94)
			Me.customTabControl2.Name = "customTabControl2"
			Me.customTabControl2.SelectedIndex = 0
			Me.customTabControl2.Size = New System.Drawing.Size(486, 72)
			Me.customTabControl2.TabIndex = 2
			' 
			' tabPage6
			' 
			Me.tabPage6.ImageKey = "(none)"
			Me.tabPage6.Location = New System.Drawing.Point(4, 22)
			Me.tabPage6.Name = "tabPage6"
			Me.tabPage6.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage6.Size = New System.Drawing.Size(478, 46)
			Me.tabPage6.TabIndex = 0
			Me.tabPage6.Text = "Allgemein"
			Me.tabPage6.UseVisualStyleBackColor = True
			' 
			' tabPage7
			' 
			Me.tabPage7.ImageKey = "book_open.png"
			Me.tabPage7.Location = New System.Drawing.Point(4, 22)
			Me.tabPage7.Name = "tabPage7"
			Me.tabPage7.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage7.Size = New System.Drawing.Size(478, 46)
			Me.tabPage7.TabIndex = 1
			Me.tabPage7.Text = "tabPage7"
			Me.tabPage7.UseVisualStyleBackColor = True
			' 
			' tabPage8
			' 
			Me.tabPage8.ImageKey = "(none)"
			Me.tabPage8.Location = New System.Drawing.Point(4, 22)
			Me.tabPage8.Name = "tabPage8"
			Me.tabPage8.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage8.Size = New System.Drawing.Size(478, 46)
			Me.tabPage8.TabIndex = 2
			Me.tabPage8.Text = "tabPage8"
			Me.tabPage8.UseVisualStyleBackColor = True
			' 
			' tabPage9
			' 
			Me.tabPage9.ImageKey = "(none)"
			Me.tabPage9.Location = New System.Drawing.Point(4, 22)
			Me.tabPage9.Name = "tabPage9"
			Me.tabPage9.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage9.Size = New System.Drawing.Size(478, 46)
			Me.tabPage9.TabIndex = 3
			Me.tabPage9.Text = "tabPage9"
			Me.tabPage9.UseVisualStyleBackColor = True
			' 
			' tabPage10
			' 
			Me.tabPage10.ImageKey = "cd_music.png"
			Me.tabPage10.Location = New System.Drawing.Point(4, 22)
			Me.tabPage10.Name = "tabPage10"
			Me.tabPage10.Size = New System.Drawing.Size(478, 46)
			Me.tabPage10.TabIndex = 4
			Me.tabPage10.Text = "tabPage10"
			Me.tabPage10.UseVisualStyleBackColor = True
			' 
			' customTabControl3
			' 
			Me.customTabControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl3.Controls.Add(Me.tabPage11)
			Me.customTabControl3.Controls.Add(Me.tabPage12)
			Me.customTabControl3.Controls.Add(Me.tabPage13)
			Me.customTabControl3.Controls.Add(Me.tabPage14)
			Me.customTabControl3.Controls.Add(Me.tabPage15)
			Me.customTabControl3.DisplayStyle = System.Windows.Forms.TabStyle.Rounded
			' 
			' 
			' 
			Me.customTabControl3.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl3.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl3.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl3.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl3.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl3.DisplayStyleProvider.HotTrack = True
			Me.customTabControl3.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.customTabControl3.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl3.DisplayStyleProvider.Overlap = 5
			Me.customTabControl3.DisplayStyleProvider.Padding = New System.Drawing.Point(6, 3)
			Me.customTabControl3.DisplayStyleProvider.Radius = 10
			Me.customTabControl3.DisplayStyleProvider.ShowTabCloser = False
			Me.customTabControl3.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl3.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl3.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl3.HotTrack = True
			Me.customTabControl3.ImageList = Me.imageList1
			Me.customTabControl3.Location = New System.Drawing.Point(12, 328)
			Me.customTabControl3.Name = "customTabControl3"
			Me.customTabControl3.SelectedIndex = 0
			Me.customTabControl3.Size = New System.Drawing.Size(486, 72)
			Me.customTabControl3.TabIndex = 3
			' 
			' tabPage11
			' 
			Me.tabPage11.ImageKey = "(none)"
			Me.tabPage11.Location = New System.Drawing.Point(4, 24)
			Me.tabPage11.Name = "tabPage11"
			Me.tabPage11.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage11.Size = New System.Drawing.Size(478, 44)
			Me.tabPage11.TabIndex = 0
			Me.tabPage11.Text = "Allgemein"
			Me.tabPage11.UseVisualStyleBackColor = True
			' 
			' tabPage12
			' 
			Me.tabPage12.ImageKey = "(none)"
			Me.tabPage12.Location = New System.Drawing.Point(4, 24)
			Me.tabPage12.Name = "tabPage12"
			Me.tabPage12.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage12.Size = New System.Drawing.Size(478, 44)
			Me.tabPage12.TabIndex = 1
			Me.tabPage12.Text = "tabPage12"
			Me.tabPage12.UseVisualStyleBackColor = True
			' 
			' tabPage13
			' 
			Me.tabPage13.ImageKey = "brush3.png"
			Me.tabPage13.Location = New System.Drawing.Point(4, 24)
			Me.tabPage13.Name = "tabPage13"
			Me.tabPage13.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage13.Size = New System.Drawing.Size(478, 44)
			Me.tabPage13.TabIndex = 2
			Me.tabPage13.Text = "tabPage13"
			Me.tabPage13.UseVisualStyleBackColor = True
			' 
			' tabPage14
			' 
			Me.tabPage14.ImageKey = "calculator.png"
			Me.tabPage14.Location = New System.Drawing.Point(4, 24)
			Me.tabPage14.Name = "tabPage14"
			Me.tabPage14.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage14.Size = New System.Drawing.Size(478, 44)
			Me.tabPage14.TabIndex = 3
			Me.tabPage14.Text = "tabPage14"
			Me.tabPage14.UseVisualStyleBackColor = True
			' 
			' tabPage15
			' 
			Me.tabPage15.ImageKey = "cd_music.png"
			Me.tabPage15.Location = New System.Drawing.Point(4, 24)
			Me.tabPage15.Name = "tabPage15"
			Me.tabPage15.Size = New System.Drawing.Size(478, 44)
			Me.tabPage15.TabIndex = 4
			Me.tabPage15.Text = "tabPage15"
			Me.tabPage15.UseVisualStyleBackColor = True
			' 
			' customTabControl4
			' 
			Me.customTabControl4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl4.Controls.Add(Me.tabPage16)
			Me.customTabControl4.Controls.Add(Me.tabPage17)
			Me.customTabControl4.Controls.Add(Me.tabPage18)
			Me.customTabControl4.Controls.Add(Me.tabPage19)
			Me.customTabControl4.Controls.Add(Me.tabPage20)
			Me.customTabControl4.DisplayStyle = System.Windows.Forms.TabStyle.Angled
			' 
			' 
			' 
			Me.customTabControl4.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl4.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl4.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl4.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl4.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl4.DisplayStyleProvider.HotTrack = True
			Me.customTabControl4.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.customTabControl4.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl4.DisplayStyleProvider.Overlap = 7
			Me.customTabControl4.DisplayStyleProvider.Padding = New System.Drawing.Point(7, 3)
			Me.customTabControl4.DisplayStyleProvider.Radius = 10
			Me.customTabControl4.DisplayStyleProvider.ShowTabCloser = False
			Me.customTabControl4.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl4.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl4.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl4.HotTrack = True
			Me.customTabControl4.ImageList = Me.imageList1
			Me.customTabControl4.Location = New System.Drawing.Point(12, 406)
			Me.customTabControl4.Name = "customTabControl4"
			Me.customTabControl4.SelectedIndex = 0
			Me.customTabControl4.Size = New System.Drawing.Size(486, 74)
			Me.customTabControl4.TabIndex = 4
			' 
			' tabPage16
			' 
			Me.tabPage16.ImageKey = "battery.png"
			Me.tabPage16.Location = New System.Drawing.Point(4, 24)
			Me.tabPage16.Name = "tabPage16"
			Me.tabPage16.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage16.Size = New System.Drawing.Size(478, 46)
			Me.tabPage16.TabIndex = 0
			Me.tabPage16.Text = "Allgemein"
			Me.tabPage16.UseVisualStyleBackColor = True
			' 
			' tabPage17
			' 
			Me.tabPage17.ImageKey = "book_open.png"
			Me.tabPage17.Location = New System.Drawing.Point(4, 24)
			Me.tabPage17.Name = "tabPage17"
			Me.tabPage17.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage17.Size = New System.Drawing.Size(478, 46)
			Me.tabPage17.TabIndex = 1
			Me.tabPage17.Text = "tabPage17"
			Me.tabPage17.UseVisualStyleBackColor = True
			' 
			' tabPage18
			' 
			Me.tabPage18.ImageKey = "brush3.png"
			Me.tabPage18.Location = New System.Drawing.Point(4, 24)
			Me.tabPage18.Name = "tabPage18"
			Me.tabPage18.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage18.Size = New System.Drawing.Size(478, 46)
			Me.tabPage18.TabIndex = 2
			Me.tabPage18.Text = "tabPage18"
			Me.tabPage18.UseVisualStyleBackColor = True
			' 
			' tabPage19
			' 
			Me.tabPage19.ImageKey = "(none)"
			Me.tabPage19.Location = New System.Drawing.Point(4, 24)
			Me.tabPage19.Name = "tabPage19"
			Me.tabPage19.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage19.Size = New System.Drawing.Size(478, 46)
			Me.tabPage19.TabIndex = 3
			Me.tabPage19.Text = "tabPage19"
			Me.tabPage19.UseVisualStyleBackColor = True
			' 
			' tabPage20
			' 
			Me.tabPage20.ImageKey = "(none)"
			Me.tabPage20.Location = New System.Drawing.Point(4, 24)
			Me.tabPage20.Name = "tabPage20"
			Me.tabPage20.Size = New System.Drawing.Size(478, 46)
			Me.tabPage20.TabIndex = 4
			Me.tabPage20.Text = "tabPage20"
			Me.tabPage20.UseVisualStyleBackColor = True
			' 
			' customTabControl5
			' 
			Me.customTabControl5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl5.Controls.Add(Me.tabPage21)
			Me.customTabControl5.Controls.Add(Me.tabPage22)
			Me.customTabControl5.Controls.Add(Me.tabPage23)
			Me.customTabControl5.Controls.Add(Me.tabPage24)
			Me.customTabControl5.Controls.Add(Me.tabPage25)
			Me.customTabControl5.DisplayStyle = System.Windows.Forms.TabStyle.Chrome
			' 
			' 
			' 
			Me.customTabControl5.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl5.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl5.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl5.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl5.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.White
			Me.customTabControl5.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl5.DisplayStyleProvider.HotTrack = True
			Me.customTabControl5.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.customTabControl5.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl5.DisplayStyleProvider.Overlap = 16
			Me.customTabControl5.DisplayStyleProvider.Padding = New System.Drawing.Point(7, 5)
			Me.customTabControl5.DisplayStyleProvider.Radius = 16
			Me.customTabControl5.DisplayStyleProvider.ShowTabCloser = True
			Me.customTabControl5.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl5.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl5.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl5.HotTrack = True
			Me.customTabControl5.ImageList = Me.imageList1
			Me.customTabControl5.Location = New System.Drawing.Point(12, 172)
			Me.customTabControl5.Name = "customTabControl5"
			Me.customTabControl5.SelectedIndex = 0
			Me.customTabControl5.Size = New System.Drawing.Size(486, 72)
			Me.customTabControl5.TabIndex = 5
			' 
			' tabPage21
			' 
			Me.tabPage21.ImageKey = "(none)"
			Me.tabPage21.Location = New System.Drawing.Point(4, 28)
			Me.tabPage21.Name = "tabPage21"
			Me.tabPage21.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage21.Size = New System.Drawing.Size(478, 40)
			Me.tabPage21.TabIndex = 0
			Me.tabPage21.Text = "Allgemein"
			Me.tabPage21.UseVisualStyleBackColor = True
			' 
			' tabPage22
			' 
			Me.tabPage22.ImageKey = "google_favicon.png"
			Me.tabPage22.Location = New System.Drawing.Point(4, 28)
			Me.tabPage22.Name = "tabPage22"
			Me.tabPage22.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage22.Size = New System.Drawing.Size(478, 40)
			Me.tabPage22.TabIndex = 1
			Me.tabPage22.Text = "tabPage22"
			Me.tabPage22.UseVisualStyleBackColor = True
			' 
			' tabPage23
			' 
			Me.tabPage23.ImageKey = "(none)"
			Me.tabPage23.Location = New System.Drawing.Point(4, 28)
			Me.tabPage23.Name = "tabPage23"
			Me.tabPage23.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage23.Size = New System.Drawing.Size(478, 40)
			Me.tabPage23.TabIndex = 2
			Me.tabPage23.Text = "tabPage23"
			Me.tabPage23.UseVisualStyleBackColor = True
			' 
			' tabPage24
			' 
			Me.tabPage24.ImageKey = "(none)"
			Me.tabPage24.Location = New System.Drawing.Point(4, 28)
			Me.tabPage24.Name = "tabPage24"
			Me.tabPage24.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage24.Size = New System.Drawing.Size(478, 40)
			Me.tabPage24.TabIndex = 3
			Me.tabPage24.Text = "tabPage24"
			Me.tabPage24.UseVisualStyleBackColor = True
			' 
			' tabPage25
			' 
			Me.tabPage25.ImageKey = "google_favicon.png"
			Me.tabPage25.Location = New System.Drawing.Point(4, 28)
			Me.tabPage25.Name = "tabPage25"
			Me.tabPage25.Size = New System.Drawing.Size(478, 40)
			Me.tabPage25.TabIndex = 4
			Me.tabPage25.Text = "tabPage25"
			Me.tabPage25.UseVisualStyleBackColor = True
			' 
			' customTabControl6
			' 
			Me.customTabControl6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl6.Controls.Add(Me.tabPage26)
			Me.customTabControl6.Controls.Add(Me.tabPage27)
			Me.customTabControl6.Controls.Add(Me.tabPage28)
			Me.customTabControl6.Controls.Add(Me.tabPage29)
			Me.customTabControl6.Controls.Add(Me.tabPage30)
			Me.customTabControl6.DisplayStyle = System.Windows.Forms.TabStyle.IE8
			' 
			' 
			' 
			Me.customTabControl6.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark
			Me.customTabControl6.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark
			Me.customTabControl6.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl6.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray
			Me.customTabControl6.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.Red
			Me.customTabControl6.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl6.DisplayStyleProvider.HotTrack = True
			Me.customTabControl6.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.customTabControl6.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl6.DisplayStyleProvider.Overlap = 0
			Me.customTabControl6.DisplayStyleProvider.Padding = New System.Drawing.Point(6, 5)
			Me.customTabControl6.DisplayStyleProvider.Radius = 3
			Me.customTabControl6.DisplayStyleProvider.ShowTabCloser = True
			Me.customTabControl6.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText
			Me.customTabControl6.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark
			Me.customTabControl6.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl6.HotTrack = True
			Me.customTabControl6.ImageList = Me.imageList1
			Me.customTabControl6.Location = New System.Drawing.Point(12, 250)
			Me.customTabControl6.Name = "customTabControl6"
			Me.customTabControl6.SelectedIndex = 0
			Me.customTabControl6.Size = New System.Drawing.Size(486, 72)
			Me.customTabControl6.TabIndex = 6
			' 
			' tabPage26
			' 
			Me.tabPage26.ImageKey = "google_favicon.png"
			Me.tabPage26.Location = New System.Drawing.Point(4, 28)
			Me.tabPage26.Name = "tabPage26"
			Me.tabPage26.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage26.Size = New System.Drawing.Size(478, 40)
			Me.tabPage26.TabIndex = 0
			Me.tabPage26.Text = "Allgemein"
			Me.tabPage26.UseVisualStyleBackColor = True
			' 
			' tabPage27
			' 
			Me.tabPage27.ImageKey = "google_favicon.png"
			Me.tabPage27.Location = New System.Drawing.Point(4, 28)
			Me.tabPage27.Name = "tabPage27"
			Me.tabPage27.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage27.Size = New System.Drawing.Size(478, 40)
			Me.tabPage27.TabIndex = 1
			Me.tabPage27.Text = "tabPage27"
			Me.tabPage27.UseVisualStyleBackColor = True
			' 
			' tabPage28
			' 
			Me.tabPage28.ImageKey = "(none)"
			Me.tabPage28.Location = New System.Drawing.Point(4, 28)
			Me.tabPage28.Name = "tabPage28"
			Me.tabPage28.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage28.Size = New System.Drawing.Size(478, 40)
			Me.tabPage28.TabIndex = 2
			Me.tabPage28.Text = "tabPage28"
			Me.tabPage28.UseVisualStyleBackColor = True
			' 
			' tabPage29
			' 
			Me.tabPage29.ImageKey = "(none)"
			Me.tabPage29.Location = New System.Drawing.Point(4, 28)
			Me.tabPage29.Name = "tabPage29"
			Me.tabPage29.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage29.Size = New System.Drawing.Size(478, 40)
			Me.tabPage29.TabIndex = 3
			Me.tabPage29.Text = "tabPage29"
			Me.tabPage29.UseVisualStyleBackColor = True
			' 
			' tabPage30
			' 
			Me.tabPage30.ImageKey = "google_favicon.png"
			Me.tabPage30.Location = New System.Drawing.Point(4, 28)
			Me.tabPage30.Name = "tabPage30"
			Me.tabPage30.Size = New System.Drawing.Size(478, 40)
			Me.tabPage30.TabIndex = 4
			Me.tabPage30.Text = "tabPage30"
			Me.tabPage30.UseVisualStyleBackColor = True
			' 
			' customTabControl7
			' 
			Me.customTabControl7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.customTabControl7.Controls.Add(Me.tabPage31)
			Me.customTabControl7.Controls.Add(Me.tabPage32)
			Me.customTabControl7.Controls.Add(Me.tabPage33)
			Me.customTabControl7.Controls.Add(Me.tabPage34)
			Me.customTabControl7.Controls.Add(Me.tabPage35)
			Me.customTabControl7.DisplayStyle = System.Windows.Forms.TabStyle.VS2010
			' 
			' 
			' 
			Me.customTabControl7.DisplayStyleProvider.BorderColor = System.Drawing.Color.Transparent
			Me.customTabControl7.DisplayStyleProvider.BorderColorHot = System.Drawing.Color.FromArgb(CInt(CByte(155)), CInt(CByte(167)), CInt(CByte(183)))
			Me.customTabControl7.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(CInt(CByte(127)), CInt(CByte(157)), CInt(CByte(185)))
			Me.customTabControl7.DisplayStyleProvider.CloserColor = System.Drawing.Color.FromArgb(CInt(CByte(117)), CInt(CByte(99)), CInt(CByte(61)))
			Me.customTabControl7.DisplayStyleProvider.FocusTrack = False
			Me.customTabControl7.DisplayStyleProvider.HotTrack = True
			Me.customTabControl7.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
			Me.customTabControl7.DisplayStyleProvider.Opacity = 1F
			Me.customTabControl7.DisplayStyleProvider.Overlap = 0
			Me.customTabControl7.DisplayStyleProvider.Padding = New System.Drawing.Point(6, 5)
			Me.customTabControl7.DisplayStyleProvider.Radius = 3
			Me.customTabControl7.DisplayStyleProvider.ShowTabCloser = True
			Me.customTabControl7.DisplayStyleProvider.TextColor = System.Drawing.Color.White
			Me.customTabControl7.DisplayStyleProvider.TextColorDisabled = System.Drawing.Color.WhiteSmoke
			Me.customTabControl7.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText
			Me.customTabControl7.HotTrack = True
			Me.customTabControl7.ImageList = Me.imageList1
			Me.customTabControl7.Location = New System.Drawing.Point(12, 4)
			Me.customTabControl7.Name = "customTabControl7"
			Me.customTabControl7.SelectedIndex = 0
			Me.customTabControl7.Size = New System.Drawing.Size(482, 74)
			Me.customTabControl7.TabIndex = 7
			' 
			' tabPage31
			' 
			Me.tabPage31.ImageKey = "battery.png"
			Me.tabPage31.Location = New System.Drawing.Point(4, 28)
			Me.tabPage31.Name = "tabPage31"
			Me.tabPage31.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage31.Size = New System.Drawing.Size(474, 42)
			Me.tabPage31.TabIndex = 0
			Me.tabPage31.Text = "Allgemein"
			Me.tabPage31.UseVisualStyleBackColor = True
			' 
			' tabPage32
			' 
			Me.tabPage32.ImageKey = "book_open.png"
			Me.tabPage32.Location = New System.Drawing.Point(4, 28)
			Me.tabPage32.Name = "tabPage32"
			Me.tabPage32.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage32.Size = New System.Drawing.Size(484, 42)
			Me.tabPage32.TabIndex = 1
			Me.tabPage32.Text = "tabPage32"
			Me.tabPage32.UseVisualStyleBackColor = True
			' 
			' tabPage33
			' 
			Me.tabPage33.ImageKey = "brush3.png"
			Me.tabPage33.Location = New System.Drawing.Point(4, 28)
			Me.tabPage33.Name = "tabPage33"
			Me.tabPage33.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage33.Size = New System.Drawing.Size(484, 42)
			Me.tabPage33.TabIndex = 2
			Me.tabPage33.Text = "tabPage33"
			Me.tabPage33.UseVisualStyleBackColor = True
			' 
			' tabPage34
			' 
			Me.tabPage34.ImageKey = "(none)"
			Me.tabPage34.Location = New System.Drawing.Point(4, 28)
			Me.tabPage34.Name = "tabPage34"
			Me.tabPage34.Padding = New System.Windows.Forms.Padding(3)
			Me.tabPage34.Size = New System.Drawing.Size(484, 42)
			Me.tabPage34.TabIndex = 3
			Me.tabPage34.Text = "tabPage34"
			Me.tabPage34.UseVisualStyleBackColor = True
			' 
			' tabPage35
			' 
			Me.tabPage35.ImageKey = "(none)"
			Me.tabPage35.Location = New System.Drawing.Point(4, 28)
			Me.tabPage35.Name = "tabPage35"
			Me.tabPage35.Size = New System.Drawing.Size(484, 42)
			Me.tabPage35.TabIndex = 4
			Me.tabPage35.Text = "tabPage35"
			Me.tabPage35.UseVisualStyleBackColor = True
			' 
			' panel1
			' 
			Me.panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.panel1.BackColor = System.Drawing.Color.FromArgb(CInt(CByte(43)), CInt(CByte(60)), CInt(CByte(89)))
			Me.panel1.Controls.Add(Me.customTabControl7)
			Me.panel1.Location = New System.Drawing.Point(0, 482)
			Me.panel1.Name = "panel1"
			Me.panel1.Size = New System.Drawing.Size(512, 88)
			Me.panel1.TabIndex = 8
			' 
			' MainForm
			' 
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
			Me.BackgroundImage = DirectCast(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
			Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
			Me.ClientSize = New System.Drawing.Size(510, 570)
			Me.Controls.Add(Me.panel1)
			Me.Controls.Add(Me.customTabControl1)
			Me.Controls.Add(Me.customTabControl2)
			Me.Controls.Add(Me.customTabControl6)
			Me.Controls.Add(Me.customTabControl5)
			Me.Controls.Add(Me.customTabControl3)
			Me.Controls.Add(Me.customTabControl4)
			Me.DoubleBuffered = True
			Me.MinimumSize = New System.Drawing.Size(526, 521)
			Me.Name = "MainForm"
			Me.Text = "TabControl Demo"
			Me.customTabControl1.ResumeLayout(False)
			Me.customTabControl2.ResumeLayout(False)
			Me.customTabControl3.ResumeLayout(False)
			Me.customTabControl4.ResumeLayout(False)
			Me.customTabControl5.ResumeLayout(False)
			Me.customTabControl6.ResumeLayout(False)
			Me.customTabControl7.ResumeLayout(False)
			Me.panel1.ResumeLayout(False)
			Me.ResumeLayout(False)
		End Sub
		Private panel1 As System.Windows.Forms.Panel
		Private tabPage35 As System.Windows.Forms.TabPage
		Private tabPage34 As System.Windows.Forms.TabPage
		Private tabPage33 As System.Windows.Forms.TabPage
		Private tabPage32 As System.Windows.Forms.TabPage
		Private tabPage31 As System.Windows.Forms.TabPage
		Private customTabControl7 As System.Windows.Forms.CustomTabControl
		Private tabPage30 As System.Windows.Forms.TabPage
		Private tabPage29 As System.Windows.Forms.TabPage
		Private tabPage28 As System.Windows.Forms.TabPage
		Private tabPage27 As System.Windows.Forms.TabPage
		Private tabPage26 As System.Windows.Forms.TabPage
		Private customTabControl6 As System.Windows.Forms.CustomTabControl
		Private tabPage25 As System.Windows.Forms.TabPage
		Private tabPage24 As System.Windows.Forms.TabPage
		Private tabPage23 As System.Windows.Forms.TabPage
		Private tabPage22 As System.Windows.Forms.TabPage
		Private tabPage21 As System.Windows.Forms.TabPage
		Private customTabControl5 As System.Windows.Forms.CustomTabControl
		Private tabPage20 As System.Windows.Forms.TabPage
		Private tabPage19 As System.Windows.Forms.TabPage
		Private tabPage18 As System.Windows.Forms.TabPage
		Private tabPage17 As System.Windows.Forms.TabPage
		Private tabPage16 As System.Windows.Forms.TabPage
		Private customTabControl4 As System.Windows.Forms.CustomTabControl
		Private tabPage15 As System.Windows.Forms.TabPage
		Private tabPage14 As System.Windows.Forms.TabPage
		Private tabPage13 As System.Windows.Forms.TabPage
		Private tabPage12 As System.Windows.Forms.TabPage
		Private tabPage11 As System.Windows.Forms.TabPage
		Private customTabControl3 As System.Windows.Forms.CustomTabControl
		Private tabPage10 As System.Windows.Forms.TabPage
		Private tabPage9 As System.Windows.Forms.TabPage
		Private tabPage8 As System.Windows.Forms.TabPage
		Private tabPage7 As System.Windows.Forms.TabPage
		Private tabPage6 As System.Windows.Forms.TabPage
		Private customTabControl2 As System.Windows.Forms.CustomTabControl
		Private tabPage3 As System.Windows.Forms.TabPage
		Private tabPage5 As System.Windows.Forms.TabPage
		Private tabPage4 As System.Windows.Forms.TabPage
		Private tabPage2 As System.Windows.Forms.TabPage
		Private tabPage1 As System.Windows.Forms.TabPage
		Private customTabControl1 As System.Windows.Forms.CustomTabControl
		Private contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
		Private imageList1 As System.Windows.Forms.ImageList
	End Class
