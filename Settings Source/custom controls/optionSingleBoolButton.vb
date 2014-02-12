Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Toggles the value of a single RAW token with 'YES' or 'NO' values.")> _
Public Class optionSingleBoolButton
    Inherits mwCheckBox
    Implements iToken
    Implements iTest
    Implements iExportInfo

    Public Sub New()
        MyBase.New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
        m_opt = New optionSingle
    End Sub

    Private m_opt As optionSingle

    Public Sub loadOption() Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            Me.Checked = yesNoToBoolean(m_opt.loadOption)
        Catch ex As Exception
            Me.Checked = False
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub

        If Not m_opt.saveOption(Me.Checked) Then
            m_opt.valueUpdatingPaused = True
            Me.Checked = Not Me.Checked
            m_opt.valueUpdatingPaused = False
        End If
    End Sub

    Public Property options As optionSingle
        Get
            Return m_opt
        End Get
        Set(value As optionSingle)
            m_opt = value
        End Set
    End Property

    Private Sub optionSingle_CheckedChanged(sender As Object, e As EventArgs) Handles Me.CheckedChanged
        m_opt.valueChanged(booleanToYesNo(Me.Checked))
        saveOption()
    End Sub

    Public Function booleanToYesNo(ByVal value As Boolean) As String
        Dim strValue
        If (value) Then strValue = "YES" Else strValue = "NO"
        Return strValue
    End Function

    Public Sub runtTest() Implements iTest.runTest
        Me.Checked = Not Me.Checked
        Me.Checked = Not Me.Checked
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    'Public Function optionInfo() As List(Of String) Implements iExportInfo.optionInfo
    '    Return m_opt.optionInfo
    'End Function
    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return Nothing
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return m_opt.optionTags
    End Function
End Class
