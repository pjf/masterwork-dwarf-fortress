Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Uses a find and replace pattern to change RAW values.")> _
Public Class optionComboPatternToken
    Inherits mwComboBox
    Implements iToken
    Implements iTest
    Implements iExportInfo

    Private m_value As String

    Private m_opt As New optionList
    Private m_optPattern As New optionBasePattern("", "")

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub loadOption(Optional value As Object = Nothing) Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If Me.DataSource Is Nothing Then
                If LicenseManager.UsageMode <> LicenseUsageMode.Designtime Then
                    Me.DataSource = m_opt.itemList
                    Me.DisplayMember = "display"
                    Me.ValueMember = "value"
                End If
            End If
            If value IsNot Nothing Then
                m_opt.valueUpdatingPaused = False : Me.SelectedValue = CStr(value) : optionComboPatternToken_SelectionChangeCommitted(Me, New EventArgs) : m_opt.valueUpdatingPaused = True
            Else
                Me.SelectedValue = m_opt.optionManager.loadPatternValue(m_optPattern.find, m_opt.fileManager.loadFiles(m_opt.optionManager, m_optPattern.find))
            End If
            m_value = Me.SelectedValue

        Catch ex As Exception
            Me.SelectedIndex = -1
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub

        If Not m_optPattern.replace.Contains("${value}") Then
            MsgBox("Unable to save, replacement pattern is in an invalid format!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Else
            Dim r As String = m_optPattern.replace.Replace("${value}", m_value)
            If Not m_opt.optionManager.replacePatternsInFiles(m_optPattern.find, r, m_opt.fileManager) Then
                MsgBox("Failed to save changes for " & Me.Name & "!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        End If
        m_opt.valueUpdatingPaused = False
    End Sub

    Public Property options As optionList
        Get
            Return m_opt
        End Get
        Set(value As optionList)
            m_opt = value
        End Set
    End Property

    Public Property optPattern As optionBasePattern
        Get
            Return m_optPattern
        End Get
        Set(value As optionBasePattern)
            m_optPattern = value
        End Set
    End Property

    Private Sub optionComboPatternToken_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Me.SelectionChangeCommitted
        m_value = Me.SelectedValue
        saveOption()
    End Sub

    Public Sub runtTest() Implements iTest.runTest
        For i As Integer = 0 To Me.Items.Count
            If Not Me.Items(i).Equals(Me.SelectedItem) Then
                Me.SelectedItem = Me.Items(i)
                m_value = Me.SelectedValue
                Exit For
            End If
        Next
        optionComboPatternToken_SelectionChangeCommitted(Me, New EventArgs)
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return m_opt.itemList
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return m_opt.optionTags
    End Function

    Public Function hasFileOverrides() As Boolean Implements iExportInfo.hasFileOverrides
        Return m_opt.fileManager.isOverriden
    End Function

    Public Function patternInfo() As optionBasePattern Implements iExportInfo.patternInfo
        Return m_optPattern
    End Function

    Public Function affectsGraphics() As Boolean Implements iExportInfo.affectsGraphics
        Return m_opt.fileManager.affectsGraphics
    End Function

    Public Function currentValue() As Object Implements iToken.currentValue
        If Me.SelectedValue Is Nothing Then
            Return ""
        Else
            Return Me.SelectedValue.ToString
        End If
    End Function

End Class
