<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblIcons = New System.Windows.Forms.LinkLabel()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblBackground = New System.Windows.Forms.LinkLabel()
        Me.lblRibbon = New System.Windows.Forms.LinkLabel()
        Me.lblTabs = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'lblIcons
        '
        Me.lblIcons.AutoSize = True
        Me.lblIcons.Location = New System.Drawing.Point(89, 46)
        Me.lblIcons.Name = "lblIcons"
        Me.lblIcons.Size = New System.Drawing.Size(200, 15)
        Me.lblIcons.TabIndex = 2
        Me.lblIcons.TabStop = True
        Me.lblIcons.Tag = "http://p.yusukekamiyamane.com/"
        Me.lblIcons.Text = "Fugue Icons By Yusuke Kamiyamane"
        Me.lblIcons.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblVersion
        '
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.Window
        Me.lblVersion.Location = New System.Drawing.Point(37, 10)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(304, 23)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBackground
        '
        Me.lblBackground.AutoSize = True
        Me.lblBackground.Location = New System.Drawing.Point(102, 74)
        Me.lblBackground.Name = "lblBackground"
        Me.lblBackground.Size = New System.Drawing.Size(174, 15)
        Me.lblBackground.TabIndex = 2
        Me.lblBackground.TabStop = True
        Me.lblBackground.Tag = "http://borkurart.com/"
        Me.lblBackground.Text = "Background by Borkur Eiriksson"
        Me.lblBackground.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRibbon
        '
        Me.lblRibbon.AutoSize = True
        Me.lblRibbon.Location = New System.Drawing.Point(128, 102)
        Me.lblRibbon.Name = "lblRibbon"
        Me.lblRibbon.Size = New System.Drawing.Size(122, 15)
        Me.lblRibbon.TabIndex = 2
        Me.lblRibbon.TabStop = True
        Me.lblRibbon.Tag = "https://officeribbon.codeplex.com/"
        Me.lblRibbon.Text = "Ribbon Menu Control"
        Me.lblRibbon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTabs
        '
        Me.lblTabs.AutoSize = True
        Me.lblTabs.Location = New System.Drawing.Point(132, 130)
        Me.lblTabs.Name = "lblTabs"
        Me.lblTabs.Size = New System.Drawing.Size(115, 15)
        Me.lblTabs.TabIndex = 2
        Me.lblTabs.TabStop = True
        Me.lblTabs.Tag = "http://www.codeproject.com/Articles/38014/KRBTabControl"
        Me.lblTabs.Text = "Custom Tab Control"
        Me.lblTabs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(378, 163)
        Me.Controls.Add(Me.lblTabs)
        Me.Controls.Add(Me.lblRibbon)
        Me.Controls.Add(Me.lblBackground)
        Me.Controls.Add(Me.lblIcons)
        Me.Controls.Add(Me.lblVersion)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblIcons As System.Windows.Forms.LinkLabel
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblBackground As System.Windows.Forms.LinkLabel
    Friend WithEvents lblRibbon As System.Windows.Forms.LinkLabel
    Friend WithEvents lblTabs As System.Windows.Forms.LinkLabel
End Class
