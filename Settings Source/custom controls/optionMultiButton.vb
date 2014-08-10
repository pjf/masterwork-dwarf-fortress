Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Toggles multiple RAW tokens in one or more specified files.")> _
Public Class optionMultiButton
    Inherits mwCheckBox
    Implements iToken
    Implements iTest
    Implements iExportInfo

    Public Sub New()
        MyBase.New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private m_opt As New optionMulti

    Public Sub loadOption(Optional ByVal value As Object = Nothing) Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If value IsNot Nothing Then
                Me.Checked = CBool(value)
                m_opt.valueUpdatingPaused = False : saveOption() : m_opt.valueUpdatingPaused = True
            Else
                Me.Checked = yesNoToBoolean(m_opt.loadOption())
            End If
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

        m_opt.valueUpdatingPaused = False
    End Sub

    Private Sub optionButton_CheckedChanged(sender As Object, e As EventArgs) Handles Me.CheckedChanged
        saveOption()
    End Sub

    Public Property options As optionMulti
        Get
            Return m_opt
        End Get
        Set(value As optionMulti)
            m_opt = value
        End Set
    End Property

    Public Sub runtTest() Implements iTest.runTest
        Me.Checked = Not Me.Checked
        Me.Checked = Not Me.Checked
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
        Return CBool(Me.Checked)
    End Function
End Class