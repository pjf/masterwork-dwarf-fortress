Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals
Imports System.Text.RegularExpressions

<DescriptionAttribute("Changes a single RAW token with a specifically formatted value.")> _
Public Class optionFormatted
    Inherits TextBox
    Implements iToken
    Implements iTooltip
    Implements iTest

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Me.TextAlign = HorizontalAlignment.Center
    End Sub

    Private m_opt As New optionSingle
    Private m_pattern As String
    Private m_niceFormat As String
    Private m_ep As New ErrorProvider

    Private Sub optionFormatted_Validating(sender As Object, e As CancelEventArgs) Handles Me.Validating
        Dim rx As New Regex(m_pattern)
        If Not rx.IsMatch(Me.Text) Then
            m_ep.SetError(Me, "The input must be in the format of: " & m_niceFormat)
            m_ep.SetIconAlignment(Me, ErrorIconAlignment.MiddleLeft)
            e.Cancel = True
        Else
            m_ep.SetError(Me, "")
        End If
    End Sub

    Private Sub optionFormatted_Validated(sender As Object, e As EventArgs) Handles Me.Validated
        m_opt.valueChanged(Me.Text)
        saveOption()
    End Sub

    Public Sub loadOption() Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            Me.Text = CStr(m_opt.loadOption).Trim
        Catch ex As Exception
            Me.Text = ""
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub
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

    <DisplayNameAttribute("Token Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the regular expression pattern the input must match to be validated.")> _
    Public Property pattern As String
        Get
            Return m_pattern
        End Get
        Set(value As String)
            m_pattern = value
        End Set
    End Property

    <DisplayNameAttribute("Display Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the pattern to display to the user.")> _
    Public Property niceFormat As String
        Get
            Return m_niceFormat
        End Get
        Set(value As String)
            m_niceFormat = value
        End Set
    End Property

    Public Function getToolTip() As String Implements iTooltip.getToolTip
        Return m_niceFormat
    End Function


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

    Public Sub runtTest() Implements iTest.runtTest
        Dim valid As Boolean = False
        Dim rx As New Regex(m_pattern)
        Dim newVal As String = Me.Text
        While Not valid
            newval = InputBox("Enter test value for " & Me.Name.ToString, "", Me.Text)
            If rx.IsMatch(newVal) Then valid = True
        End While
        Me.Text = newVal
        optionFormatted_Validated(Me, New System.EventArgs)
    End Sub
End Class
