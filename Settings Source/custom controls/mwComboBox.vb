Public Class mwComboBox
    Inherits ComboBox
    Implements iTheme

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.ButtonPressed_2013
        Me.ForeColor = Theme.ColorTable.Text
    End Sub

End Class
