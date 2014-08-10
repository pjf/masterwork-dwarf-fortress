<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class colorPreviewer
    Inherits System.Windows.Forms.Panel

    'UserControl overrides dispose to clean up the component list.
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
        Me.SuspendLayout()
        '
        'colorViewer
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(207, 37)
        Me.Name = "colorViewer"
        Me.ResumeLayout(False)

    End Sub

End Class
