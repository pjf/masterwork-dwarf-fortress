Public Class frmWait

    Private WithEvents t As New Timer

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hide()
    End Sub

    Private Sub frmThemeChange_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        t.Stop()
    End Sub

    Private Sub frmThemeChange_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        t.Interval = 250
        t.Enabled = True
        t.Start()
    End Sub

    Public Sub updateTheme()
        lblMsg.TextAlign = ContentAlignment.MiddleCenter
        lblMsg.ForeColor = Theme.ColorTable.Text
        lblMsg.BackColor = Theme.ColorTable.RibbonBackground_2013
        Me.BackColor = Theme.ColorTable.RibbonBackground_2013
    End Sub

    Private Sub t_Tick(sender As Object, e As EventArgs) Handles t.Tick
        pBoxProgress.Invalidate()
    End Sub
End Class
