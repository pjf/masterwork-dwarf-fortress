Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals
Public Class optionComboBoxMulti
    Inherits mwComboBox
    Implements iToken
    Implements iTest
    Implements iExportInfo

    Private m_opt As optionListMulti

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_opt = New optionListMulti
    End Sub

    Public Property options As optionListMulti
        Get
            Return m_opt
        End Get
        Set(value As optionListMulti)
            m_opt = value
        End Set
    End Property

    Private Sub optionComboBoxMulti_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Me.SelectionChangeCommitted
        Dim val As String = CStr(Me.SelectedValue)
        For Each t As rawToken In m_opt.tokenList
            t.optionOnValue = val
            t.optionOffValue = ""
        Next
        saveOption()
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

            Me.SelectedValue = CStr(m_opt.loadOption)

        Catch ex As Exception
            Me.SelectedIndex = -1
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub
        m_opt.saveOption()
        m_opt.valueUpdatingPaused = False
    End Sub

    Public Sub runtTest() Implements iTest.runTest
        For i As Integer = 0 To Me.Items.Count
            If Not Me.Items(i).Equals(Me.SelectedItem) Then
                Me.SelectedItem = Me.Items(i)
                Exit For
            End If
        Next
        optionComboBoxMulti_SelectionChangeCommitted(Me, New EventArgs)
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return m_opt.itemList
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return m_opt.tokenList
    End Function
End Class
