Public Class mwCheckBox
    Inherits CheckBox
    Implements iTheme
    Implements iEnabled

    Private m_enabled As Boolean = True

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Appearance = Windows.Forms.Appearance.Button
        Me.FlatStyle = FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 0
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
            Me.FlatAppearance.MouseOverBackColor = Theme.ColorTable.TabSelectedGlow
            Me.FlatAppearance.MouseDownBackColor = Theme.ColorTable.TabSelectedGlow
        Else
            Me.FlatAppearance.MouseDownBackColor = Theme.ColorTable.RibbonBackground_2013
            Me.FlatAppearance.MouseOverBackColor = Theme.ColorTable.ButtonSelected_2013
        End If

        Me.BackColor = Theme.ColorTable.ButtonBgCenter
        Me.FlatAppearance.CheckedBackColor = Theme.ColorTable.ButtonBgCenter
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

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

    Public Property isEnabled As Boolean Implements iEnabled.isEnabled
        Get
            Return m_enabled
        End Get
        Set(value As Boolean)
            m_enabled = value
            MyBase.Enabled = value
        End Set
    End Property
End Class
