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

    Private m_opt As New optionList

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub loadOption() Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If Me.DataSource Is Nothing Then
                If LicenseManager.UsageMode <> LicenseUsageMode.Designtime Then
                    Me.DataSource = m_opt.itemList
                    Me.DisplayMember = "display"
                    Me.ValueMember = "value"
                End If
            End If

            Me.SelectedValue = m_opt.optionManager.loadPatternValue(m_pattern, m_opt.fileManager.getFilePaths)
            m_value = Me.SelectedValue

        Catch ex As Exception
            Me.SelectedIndex = -1
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub

        If Not m_replace.Contains("${value}") Then
            MsgBox("Unable to save, replacement pattern is in an invalid format!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Else
            Dim r As String = m_replace.Replace("${value}", m_value)
            If Not m_opt.optionManager.replacePatternsInFiles(m_pattern, r, m_opt.fileManager, m_opt.updateTileSets) Then
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
End Class
