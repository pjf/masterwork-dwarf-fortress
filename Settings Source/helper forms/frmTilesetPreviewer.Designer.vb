<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTilesetPreviewer
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
        Me.pBoxPreview = New System.Windows.Forms.PictureBox()
        CType(Me.pBoxPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pBoxPreview
        '
        Me.pBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pBoxPreview.Location = New System.Drawing.Point(0, 0)
        Me.pBoxPreview.Name = "pBoxPreview"
        Me.pBoxPreview.Size = New System.Drawing.Size(447, 244)
        Me.pBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pBoxPreview.TabIndex = 0
        Me.pBoxPreview.TabStop = False
        '
        'frmTilesetPreviewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(447, 244)
        Me.Controls.Add(Me.pBoxPreview)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmTilesetPreviewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        CType(Me.pBoxPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pBoxPreview As System.Windows.Forms.PictureBox
End Class
