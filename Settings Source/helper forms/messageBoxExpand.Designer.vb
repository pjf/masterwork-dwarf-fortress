<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class messageBoxExpand
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
        Me.tableLayoutButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.Ignore_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblExpand = New System.Windows.Forms.Label()
        Me.txtDetails = New System.Windows.Forms.TextBox()
        Me.messageIcon = New System.Windows.Forms.PictureBox()
        Me.tableLayoutButtons.SuspendLayout()
        CType(Me.messageIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tableLayoutButtons
        '
        Me.tableLayoutButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tableLayoutButtons.ColumnCount = 3
        Me.tableLayoutButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.tableLayoutButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.tableLayoutButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.tableLayoutButtons.Controls.Add(Me.Ignore_Button, 0, 0)
        Me.tableLayoutButtons.Controls.Add(Me.OK_Button, 2, 0)
        Me.tableLayoutButtons.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.tableLayoutButtons.Location = New System.Drawing.Point(278, 130)
        Me.tableLayoutButtons.Name = "tableLayoutButtons"
        Me.tableLayoutButtons.RowCount = 1
        Me.tableLayoutButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutButtons.Size = New System.Drawing.Size(192, 29)
        Me.tableLayoutButtons.TabIndex = 0
        '
        'Ignore_Button
        '
        Me.Ignore_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Ignore_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Ignore_Button.Location = New System.Drawing.Point(3, 3)
        Me.Ignore_Button.Name = "Ignore_Button"
        Me.Ignore_Button.Size = New System.Drawing.Size(57, 23)
        Me.Ignore_Button.TabIndex = 5
        Me.Ignore_Button.Text = "Ignore"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(131, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(58, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(67, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(57, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'lblMessage
        '
        Me.lblMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMessage.AutoEllipsis = True
        Me.lblMessage.Location = New System.Drawing.Point(63, 34)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(404, 92)
        Me.lblMessage.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.AutoEllipsis = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(60, 6)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(410, 16)
        Me.lblTitle.TabIndex = 2
        '
        'lblExpand
        '
        Me.lblExpand.AutoSize = True
        Me.lblExpand.Location = New System.Drawing.Point(8, 143)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(69, 13)
        Me.lblExpand.TabIndex = 1
        Me.lblExpand.Text = "Show Details"
        '
        'txtDetails
        '
        Me.txtDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDetails.Location = New System.Drawing.Point(9, 166)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDetails.Size = New System.Drawing.Size(462, 0)
        Me.txtDetails.TabIndex = 0
        '
        'messageIcon
        '
        Me.messageIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.messageIcon.Location = New System.Drawing.Point(18, 12)
        Me.messageIcon.Name = "messageIcon"
        Me.messageIcon.Size = New System.Drawing.Size(32, 32)
        Me.messageIcon.TabIndex = 4
        Me.messageIcon.TabStop = False
        '
        'messageBoxExpand
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(477, 162)
        Me.Controls.Add(Me.txtDetails)
        Me.Controls.Add(Me.messageIcon)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.tableLayoutButtons)
        Me.Controls.Add(Me.lblExpand)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "messageBoxExpand"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.tableLayoutButtons.ResumeLayout(False)
        CType(Me.messageIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tableLayoutButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblExpand As System.Windows.Forms.Label
    Friend WithEvents messageIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Ignore_Button As System.Windows.Forms.Button
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox

End Class
