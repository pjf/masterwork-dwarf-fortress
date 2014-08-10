Public Class mwComboBox
    Inherits hoverComboBox
    Implements iTheme

    Private m_enabled As Boolean = True

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.DoubleBuffered = True
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.DropDownBg
        Me.ForeColor = Theme.ColorTable.Text
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

End Class
