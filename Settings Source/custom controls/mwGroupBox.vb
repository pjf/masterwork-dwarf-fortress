Public Class mwGroupBox
    Inherits GroupBox

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Me.DoubleBuffered = True
        'BackColor = System.Drawing.Color.FromArgb(130, 0, 0, 0) 'Color.Transparent
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

End Class
