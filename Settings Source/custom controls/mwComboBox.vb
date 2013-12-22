Public Class mwComboBox
    Inherits ComboBox
    Implements iTheme

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.DoubleBuffered = True
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.RibbonBackground_2013 'ButtonPressed_2013
        Me.ForeColor = Theme.ColorTable.Text
    End Sub

End Class
