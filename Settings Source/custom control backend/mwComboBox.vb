Public Class mwComboBox
    Inherits ComboBox

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.FlatStyle = Windows.Forms.FlatStyle.Standard
    End Sub


    Public Overrides Property ForeColor As Color
        Get
            Return Theme.ColorTable.Text
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
        End Set
    End Property
    Public Overrides Property BackColor As Color
        Get
            Return Theme.ColorTable.TabSelectedGlow
        End Get
        Set(value As Color)
            MyBase.BackColor = value
        End Set
    End Property
End Class
