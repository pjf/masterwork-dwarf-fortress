Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Updates a single RAW token's with a numeric value.")> _
Public Class optionNumeric
    Inherits NumericUpDown
    Implements iToken
    Implements iTooltip
    Implements iTest
    Implements iTheme

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TextAlign = HorizontalAlignment.Center
        Me.BorderStyle = Windows.Forms.BorderStyle.None
        Me.DoubleBuffered = True
    End Sub

    Dim m_opt As New optionSingle

    Public Sub loadOption() Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            Me.Value = CInt(m_opt.loadOption)
        Catch ex As Exception
            Me.Value = 0
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused OrElse Me.DesignMode Then Exit Sub
        m_opt.valueUpdatingPaused = True
        m_opt.saveOption()
        m_opt.valueUpdatingPaused = False
    End Sub

    Public Property options As optionSingle
        Get
            Return m_opt
        End Get
        Set(value As optionSingle)
            m_opt = value
        End Set
    End Property

    Private Sub optionNumeric_Validated(sender As Object, e As EventArgs) Handles Me.Validated
        saveOption()
    End Sub

    Private Sub optionNumeric_ValueChanged(sender As Object, e As EventArgs) Handles Me.ValueChanged
        m_opt.valueChanged(CStr(Me.Value))
    End Sub

    Public Function getToolTip() As String Implements iTooltip.getToolTip
        Return String.Format("{0} - {1}", Me.Minimum.ToString, Me.Maximum.ToString)
    End Function

    Public Sub runtTest() Implements iTest.runTest
        Dim newVal As Integer = Me.Value - Me.Increment
        If newVal >= Me.Minimum And newVal <= Me.Maximum Then
            Me.Value = newVal
        Else
            Me.Value = Me.Value + Me.Increment
        End If
        optionNumeric_Validated(Me, New EventArgs)
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.DropDownBg
        Me.ForeColor = Theme.ColorTable.Text
    End Sub
End Class
