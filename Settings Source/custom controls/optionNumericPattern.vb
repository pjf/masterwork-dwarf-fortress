Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals
Imports System.Text.RegularExpressions

<DescriptionAttribute("Performs a single find/replace based on regex patterns.")> _
Public Class optionNumericPattern
    Inherits NumericUpDown
    Implements iToken
    Implements iTooltip
    Implements iTest
    Implements iTheme
    Implements iExportInfo

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TextAlign = HorizontalAlignment.Center
        Me.BorderStyle = Windows.Forms.BorderStyle.None
        Me.DoubleBuffered = True
    End Sub

    Private m_opt As New optionPattern    

    Private Sub optionFormatted_Validated(sender As Object, e As EventArgs) Handles Me.Validated
        saveOption()
    End Sub

    Public Sub loadOption(Optional ByVal value As Object = Nothing) Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If value IsNot Nothing Then
                Me.Text = CStr(value)
                m_opt.valueUpdatingPaused = False : optionFormatted_Validated(Me, Nothing) : m_opt.valueUpdatingPaused = True
            Else
                Me.Text = m_opt.optionManager.loadPatternValue(m_opt.Pattern.find, m_opt.fileManager.loadFiles(m_opt.optionManager, m_opt.Pattern.find))
            End If
        Catch ex As Exception
            Me.Text = ""
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub

        If Not m_opt.Pattern.replace.Contains("${value}") Then
            MsgBox("Unable to save, replacement pattern is in an invalid format!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Else
            Dim r As String = m_opt.Pattern.replace.Replace("${value}", Me.Value)
            If Not m_opt.optionManager.replacePatternsInFiles(m_opt.Pattern.find, r, m_opt.fileManager) Then
                MsgBox("Failed to save changes for " & Me.Name & "!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        End If
        m_opt.valueUpdatingPaused = False
    End Sub

    Public Property options As optionPattern
        Get
            Return m_opt
        End Get
        Set(value As optionPattern)
            m_opt = value
        End Set
    End Property

    Private Sub optionNumeric_Validated(sender As Object, e As EventArgs) Handles Me.Validated
        saveOption()
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
        Return m_opt.Pattern
    End Function

    Public Function affectsGraphics() As Boolean Implements iExportInfo.affectsGraphics
        Return m_opt.fileManager.affectsGraphics
    End Function

    Public Function currentValue() As Object Implements iToken.currentValue
        Return CInt(Me.Value)
    End Function
End Class


