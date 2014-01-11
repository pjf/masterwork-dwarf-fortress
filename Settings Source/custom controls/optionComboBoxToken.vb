Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Sets the value of a specific token to a value chosen from a dropdown list.")> _
Public Class optionComboBoxToken
    Inherits mwComboBox
    Implements iToken
    Implements iTest
    Implements iExportInfo

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_opt = New optionList

    End Sub

    Private m_opt As optionList

    Public Property options As optionList
        Get
            Return m_opt
        End Get
        Set(value As optionList)
            m_opt = value
        End Set
    End Property

    Private Sub optionComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Me.SelectionChangeCommitted
        m_opt.valueChanged(Me.SelectedValue)
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
        optionComboBox_SelectionChangeCommitted(Me, New EventArgs)
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    Public Function optionInfo() As List(Of String) Implements iExportInfo.optionInfo
        Dim s As List(Of String) = m_opt.optionInfo
        s.Add("{VALUES} " & String.Join(", ", m_opt.itemList.valueList))
        Return s
    End Function
End Class

