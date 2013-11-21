Public Class mwCheckBox
    Inherits CheckBox

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Appearance = Windows.Forms.Appearance.Button
        Me.TextAlign = ContentAlignment.MiddleCenter
        Me.ImageAlign = ContentAlignment.MiddleLeft
        Me.DoubleBuffered = True

        Me.BackgroundImage = My.Resources.transp_1
        Me.BackgroundImageLayout = ImageLayout.Tile

        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.FlatAppearance.CheckedBackColor = Color.Transparent
        Me.FlatAppearance.BorderSize = 0

        Me.ForeColor = Color.WhiteSmoke

        Me.Image = My.Resources.cross_small
    End Sub


    Private Sub optionSingle_CheckedChanged(sender As Object, e As EventArgs) Handles Me.CheckedChanged
        If Me.Checked Then
            Me.Image = My.Resources.tick_small
        Else
            Me.Image = My.Resources.cross_small
        End If
    End Sub

    Private Sub optionSingleReplaceButton_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If MyBase.FlatAppearance.MouseOverBackColor <> Theme.ColorTable.TabSelectedGlow Then
            MyBase.FlatAppearance.MouseOverBackColor = Theme.ColorTable.TabSelectedGlow
            MyBase.FlatAppearance.MouseDownBackColor = Theme.ColorTable.TabSelectedGlow
        End If
        'If MyBase.ForeColor <> Theme.ColorTable.Text Then
        '    MyBase.ForeColor = Theme.ColorTable.Text
        'End If
    End Sub

    Protected Function yesNoToBoolean(ByVal value As String) As Boolean
        If String.Compare(value, "YES", True) = 0 Or String.Compare(value, "1", True) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Protected Overrides ReadOnly Property CreateParams() As CreateParams
    '    Get
    '        Dim parms = MyBase.CreateParams
    '        parms.Style = parms.Style And Not &H2000000 ' Turn off WS_CLIPCHILDREN
    '        Return parms
    '    End Get
    'End Property
End Class
