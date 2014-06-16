Imports PresentationControls
Imports System.ComponentModel

Public Class optionComboCheckbox
    Inherits CheckBoxComboBox
    Implements iToken
    Implements iTest
    Implements iExportInfo
    Implements iTheme

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_opt = New optionMulti

        Me.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.DoubleBuffered = True        
    End Sub

    Private m_opt As optionMulti
    Private m_wrapper As ListSelectionWrapper(Of rawToken)
    Private m_tooltip As New ToolTip
    Private m_allDisplay As String = "All"
    Private m_noneDisplay As String = "None"

    Public Property options As optionMulti
        Get
            Return m_opt
        End Get
        Set(value As optionMulti)
            m_opt = value
        End Set
    End Property

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("This will be shown when all items are selected."), _
    DisplayName("All Display"), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property allDisplay As String
        Get
            Return m_allDisplay
        End Get
        Set(value As String)
            m_allDisplay = value
        End Set
    End Property

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("This will be shown when all items are selected."), _
    DisplayName("None Display"), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property noneDisplay As String
        Get
            Return m_noneDisplay
        End Get
        Set(value As String)
            m_noneDisplay = value
        End Set
    End Property

    Public Sub loadOption(Optional ByVal value As Object = Nothing) Implements iToken.loadOption
        m_opt.valueUpdatingPaused = True
        Try
            If Me.DataSource Is Nothing Then
                If LicenseManager.UsageMode <> LicenseUsageMode.Designtime Then
                    m_wrapper = New ListSelectionWrapper(Of rawToken)(m_opt.tokenList, "tokenName")
                    m_wrapper.AllDisplay = m_allDisplay
                    m_wrapper.NoneDisplay = m_noneDisplay

                    Me.DataSource = m_wrapper
                    Me.DisplayMemberSingleItem = "Name"
                    Me.DisplayMember = "NameConcatenated"
                    Me.ValueMember = "Selected"
                End If
            End If
            If value IsNot Nothing Then
                'uncheck everything
                For Each cItem As CheckBoxComboBoxItem In Me.CheckBoxItems
                    cItem.Checked = False
                Next

                'only enable what we're loading from the profile
                Dim tokenNames As List(Of String) = CStr(value).Split(New [Char]() {","c}, StringSplitOptions.RemoveEmptyEntries).ToList
                For Each tName As String In tokenNames
                    For Each t As rawToken In m_opt.tokenList
                        If t.tokenName.ToLower.Trim = tName.ToLower.Trim Then
                            m_wrapper.FindObjectWithItem(t).Selected = True
                        End If
                    Next
                Next
                m_opt.valueUpdatingPaused = False : saveOption() : m_opt.valueUpdatingPaused = True
            Else
                Dim currTokens As New rawTokenCollection
                For Each t As rawToken In m_opt.tokenList
                    currTokens.Clear()
                    currTokens.Add(t)
                    If m_opt.loadOption(currTokens) Then
                        m_wrapper.FindObjectWithItem(t).Selected = True
                    End If
                Next
            End If

            m_tooltip.SetToolTip(Me, m_wrapper.SelectedNames(True))
        Catch ex As Exception
            Me.SelectedIndex = -1
        Finally
            m_opt.valueUpdatingPaused = False
        End Try
    End Sub

    Public Sub saveOption() Implements iToken.saveOption
        If m_opt.valueUpdatingPaused Or Me.DesignMode Then Exit Sub

        Dim currentTokens As New rawTokenCollection
        'save all the enabled options
        For Each cItem As CheckBoxComboBoxItem In Me.CheckBoxItems.Where(Function(item As CheckBoxComboBoxItem) item.Checked = True).ToList
            currentTokens.Add(DirectCast(cItem.ComboBoxItem, ObjectSelectionWrapper(Of rawToken)).Item)
        Next
        m_opt.saveOption(currentTokens, True)

        'save all the disabled options
        currentTokens.Clear()
        For Each cItem As CheckBoxComboBoxItem In Me.CheckBoxItems.Where(Function(item As CheckBoxComboBoxItem) item.Checked = False).ToList
            currentTokens.Add(DirectCast(cItem.ComboBoxItem, ObjectSelectionWrapper(Of rawToken)).Item)
        Next
        m_opt.saveOption(currentTokens, False)

        m_tooltip.SetToolTip(Me, m_wrapper.SelectedNames(True))

        m_opt.valueUpdatingPaused = False
    End Sub


    Public Sub runtTest() Implements iTest.runTest
        For Each t As rawToken In m_opt.tokenList
            m_wrapper.FindObjectWithItem(t).Selected = (Not m_wrapper.FindObjectWithItem(t).Selected)
        Next
        saveOption()
    End Sub

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return m_opt.fullFileList
    End Function

    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return Nothing
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return m_opt.tokenList
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
        Return m_wrapper.SelectedNames(True) 'dont export 'all' or 'none'
    End Function


    Public Sub applyTheme() Implements iTheme.applyTheme
        Me.BackColor = Theme.ColorTable.DropDownBg
        Me.ForeColor = Theme.ColorTable.Text

        Me.CheckBoxProperties.FlatStyle = Windows.Forms.FlatStyle.Flat
        Me.CheckBoxProperties.FlatAppearanceCheckedBackColor = Theme.ColorTable.DropDownBg
        Me.CheckBoxProperties.FlatAppearanceMouseDownBackColor = Theme.ColorTable.DropDownBg
        Me.CheckBoxProperties.FlatAppearanceMouseOverBackColor = Theme.ColorTable.DropDownBg
        Me.ComboBoxListBackColor = Theme.ColorTable.DropDownBg

        Me.CheckBoxProperties.ForeColor = Theme.ColorTable.Text
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H20
            Return cp
        End Get
    End Property

    Private Sub optionComboCheckbox_DropDownClosed(sender As Object, e As EventArgs) Handles Me.DropDownClosed
        saveOption()        
    End Sub
End Class
