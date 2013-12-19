Public Class mwCheckBox
    Inherits CheckBox
    Implements iTheme

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Appearance = Windows.Forms.Appearance.Button
        Me.FlatStyle = FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
        'Me.TextAlign = ContentAlignment.MiddleCenter
        'Me.ImageAlign = ContentAlignment.MiddleLeft
        Me.DoubleBuffered = True


        Me.ForeColor = Theme.ColorTable.Text
        Me.Image = My.Resources.cross_small
    End Sub

    Private Sub optionSingle_CheckedChanged(sender As Object, e As EventArgs) Handles Me.CheckedChanged
        If Me.Checked Then
            Me.Image = My.Resources.tick_small
        Else
            Me.Image = My.Resources.cross_small
        End If
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        If Theme.ThemeColor = RibbonTheme.Normal Then
            Me.BackgroundImage = My.Resources.transp_1
            Me.BackgroundImageLayout = ImageLayout.Tile

            Me.BackColor = Color.Transparent
            Me.FlatAppearance.CheckedBackColor = Color.Transparent

            Me.FlatAppearance.MouseOverBackColor = Theme.ColorTable.TabSelectedGlow
            Me.FlatAppearance.MouseDownBackColor = Theme.ColorTable.TabSelectedGlow
        Else
            'Me.FlatStyle = Windows.Forms.FlatStyle.Standard
            Me.BackgroundImageLayout = ImageLayout.None
            Me.BackgroundImage = Nothing

            Me.BackColor = Theme.ColorTable.RibbonBackground_2013
            Me.FlatAppearance.CheckedBackColor = Theme.ColorTable.RibbonBackground_2013
            Me.FlatAppearance.MouseDownBackColor = Theme.ColorTable.RibbonBackground_2013

            Me.FlatAppearance.MouseOverBackColor = Theme.ColorTable.ButtonSelected_2013
        End If

        Me.FlatAppearance.BorderSize = 0
        Me.ForeColor = Theme.ColorTable.Text
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
