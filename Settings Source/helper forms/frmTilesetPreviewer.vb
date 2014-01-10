Public Class frmTilesetPreviewer

    Private Sub frmTilesetPreviewer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        e.Cancel = True
    End Sub

    Private Sub frmTilesetPreviewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.pBoxPreview.BackgroundImage = My.Resources.preview        
        Me.ClientSize = New Size(My.Resources.preview.Width, My.Resources.preview.Height)
    End Sub


End Class