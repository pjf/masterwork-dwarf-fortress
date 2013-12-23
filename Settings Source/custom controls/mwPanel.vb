Public Class mwPanel
    Inherits Panel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property
End Class
