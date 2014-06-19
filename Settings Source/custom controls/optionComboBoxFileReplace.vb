Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DescriptionAttribute("Sets the value of a specific token to a value chosen from a dropdown list.")> _
Public Class optionComboBoxFileReplace
    Inherits mwComboBox
    Implements iToken
    Implements iTest

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_opt = New optionSingleFileReplace
    End Sub

    Private m_opt As optionSingleFileReplace

    Public Property options As optionSingleFileReplace
        Get
            Return m_opt
        End Get
        Set(value As optionSingleFileReplace)
            m_opt = value
        End Set
    End Property

    Private Sub optionComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles Me.SelectionChangeCommitted
        saveOption()
    End Sub

    Public Sub loadOption(Optional ByVal value As Object = Nothing) Implements iToken.loadOption
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
                Me.SelectedValue = CStr(value)
                m_opt.valueUpdatingPaused = False : saveOption() : m_opt.valueUpdatingPaused = True
            Else
                Me.SelectedValue = CStr(m_opt.settingManager.getSettingValue)
            End If
        Catch ex As Exception
            Me.SelectedIndex = -1
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Or Me.SelectedItem Is Nothing Then Exit Sub
        m_opt.saveOption(CType(Me.SelectedItem, comboFileItem).fileName, CType(Me.SelectedItem, comboFileItem).value)
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

    Public Function currentValue() As Object Implements iToken.currentValue
        Return Me.SelectedValue.ToString
    End Function

End Class

