Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Uses a find and replace pattern to change RAW values.")> _
Public Class optionComboPatternToken
    Inherits mwComboBox
    Implements iToken
    Implements iTest

    Private m_pattern As String
    Private m_replace As String
    Private m_value As String

    Private m_opt As New optionBase

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub loadOption() Implements iToken.loadOption
        Try
            Me.SelectedItem = m_opt.optionManager.loadPatternValue(m_pattern, m_opt.fileManager.getFilePaths)
            m_value = Me.SelectedItem.ToString
        Catch ex As Exception
            Me.SelectedIndex = -1
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If Not m_replace.Contains("${value}") Then
            MsgBox("Unable to save, replacement pattern is in an invalid format!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Else
            Dim r As String = m_replace.Replace("${value}", m_value)
            If Not m_opt.optionManager.replacePatternsInFiles(m_pattern, r, m_opt.fileManager, m_opt.updateTileSets) Then
                MsgBox("Failed to save changes!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
        End If
    End Sub

    Public Property options As optionBase
        Get
            Return m_opt
        End Get
        Set(value As optionBase)
            m_opt = value
        End Set
    End Property


    <DisplayNameAttribute("Replace Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This will be used as the replacement pattern. Must contain ${value} to indicate where the new value should be placed.")> _
    Public Overridable Property replace As String
        Get
            Return m_replace
        End Get
        Set(value As String)
            m_replace = value
        End Set
    End Property

    <DisplayNameAttribute("Find Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This pattern is used to update a specific value. Must contain ?<value> to indicate where the value should be replaced in the pattern.")> _
    Public Overridable Property pattern As String
        Get
            Return m_pattern
        End Get
        Set(value As String)
            m_pattern = value
        End Set
    End Property

    Private Sub optionComboPatternToken_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Me.SelectionChangeCommitted
        m_value = Me.SelectedItem
        saveOption()
    End Sub

    Public Sub runtTest() Implements iTest.runtTest
        For i As Integer = 0 To Me.Items.Count
            If Me.Items(i) <> Me.SelectedItem Then
                Me.SelectedItem = Me.Items(i)
                m_value = Me.SelectedItem
                Exit For
            End If
        Next
        optionComboPatternToken_SelectionChangeCommitted(Me, New EventArgs)
    End Sub
End Class
