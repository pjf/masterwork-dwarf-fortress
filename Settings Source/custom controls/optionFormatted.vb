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
    Implements iTheme
    Implements iExportInfo

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
        Dim rx As New Regex(m_pattern, RegexOptions.IgnoreCase)
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

    Public Sub loadOption(Optional ByVal value As Object = Nothing) Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If value IsNot Nothing Then
                Me.Text = CStr(value)
                m_opt.valueUpdatingPaused = False : optionFormatted_Validated(Me, Nothing) : m_opt.valueUpdatingPaused = True
            Else
                Me.Text = CStr(m_opt.loadOption).Trim
            End If
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

    Public Sub runtTest() Implements iTest.runTest
        Dim valid As Boolean = False
        Dim rx As New Regex(m_pattern, RegexOptions.IgnoreCase)
        Dim newVal As String = Me.Text
        While Not valid
            newVal = InputBox("Enter test value for " & Me.Name.ToString, "", Me.Text)
            If rx.IsMatch(newVal) Then valid = True
        End While
        Me.Text = newVal
        optionFormatted_Validated(Me, New System.EventArgs)
    End Sub

    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.DropDownBg
        Me.ForeColor = Theme.ColorTable.Text
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return Nothing
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return m_opt.optionTags
    End Function

    Public Function hasFileOverrides() As Boolean Implements iExportInfo.hasFileOverrides
        Return m_opt.fileManager.isOverriden
    End Function

    Public Function patternInfo() As optionBasePattern Implements iExportInfo.patternInfo
        Return Nothing
    End Function

    Public Function affectsGraphics() As Boolean Implements iExportInfo.affectsGraphics
        Return m_opt.fileManager.affectsGraphics
    End Function

    Public Function currentValue() As Object Implements iToken.currentValue
        Return Me.Text.ToString
    End Function
End Class
