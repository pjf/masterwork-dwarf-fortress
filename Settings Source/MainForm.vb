Imports MasterworkDwarfFortress.globals
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions
Imports System.ComponentModel

<Microsoft.VisualBasic.ComClass()> Public Class MainForm

#Region "declarations"
    Private m_frmPreview As New frmTilesetPreviewer
    'Private m_refreshingWorldGen As Boolean = False

    Private m_currTheme As RibbonProfesionalRendererColorTable
#End Region

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        setTheme()
    End Sub

    Private Sub setTheme()
        Select Case My.Settings.THEME
            Case Is = "DEFAULT"
                Theme.ThemeColor = RibbonTheme.Normal
                m_currTheme = New RibbonProfesionalRendererColorTableNormal
            Case Is = "BLUE"
                Theme.ThemeColor = RibbonTheme.Blue
                m_currTheme = New RibbonProfesionalRendererColorTableBlue
        End Select

        Me.BackColor = Theme.ColorTable.RibbonBackground_2013
        ribbonMain.Refresh()
    End Sub


    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        refreshFileAndDirPaths()
        If m_dwarfFortressRootDir <> "" Then
            initialLoad()
            graphicsSets.findGraphicPacks(m_graphicsDir) 'and graphics
        End If

        setupRibbonHandlers(rPanelGeneral.Items)
        setupRibbonHandlers(rPanelUtilities.Items)

        Me.Text = "Masterwork Settings"

        randomCreaturesExistCheck()

        If fileWorking.savedGameDirs.Count > 1 Then
            btnUpdateSaves.Enabled = True
        End If
    End Sub


    'this override prevents flickering when drawing transparent controls over background images with a tabcontrol, since it doesn't support double buffering
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            Return cp
        End Get
    End Property

    Private Sub refreshFileAndDirPaths()
        Try
            If setDwarfFortressRoot() = False Then
                Throw New System.IO.DirectoryNotFoundException("Could not find the path to Dwarf Fortress.exe!")
            End If
            loadFilePaths()
        Catch ex As Exception
            MsgBox("Unable to find and load file paths. Masterwork settings requires Dwarf Fortress to be installed in the same directory!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Me.Close()
        End Try
    End Sub

    Public Sub initialLoad()
        'read init and world gen files
        m_init = fileWorking.readFile(findDfFilePath(m_initFileName))
        m_world_gen = fileWorking.readFile(findDfFilePath(m_worldGenFileName))
        m_dinit = fileWorking.readFile(findDfFilePath(m_dInitFileName))
        If m_init <> "" And m_dinit <> "" And m_world_gen <> "" Then

            'load init and world gen tokens
            m_tokensInit = tokenLoading.loadFileTokens(m_init)
            m_tokensDInit = tokenLoading.loadFileTokens(m_dinit)
            tokenLoading.loadWorldGenTokens()

            'load all our current options, and format our controls to the current theme
            initControls(Me, True, True, True)

            'load the world gen templates
            loadWorldGenCombo()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub setupRibbonHandlers(ByVal items As RibbonItemCollection)
        For Each item As RibbonItem In items
            If item.Tag IsNot Nothing AndAlso item.Tag.ToString <> "" Then
                If item.Tag.ToString.ToLower.Contains("http") Or item.Tag.ToString.ToLower.Contains("www") Then
                    AddHandler item.Click, AddressOf ribbonUrl_Click
                ElseIf item.Tag.ToString.ToLower.Contains("exe") Or item.Tag.ToString.ToLower.Contains("jar") Then
                    'this assumes the executable is within the masterwork folder somewhere. ie. it won't work for Dwarf Fortress.exe
                    'as it's in a different folder
                    AddHandler item.Click, AddressOf ribbonExe_Click
                End If
            End If

            Dim btn As RibbonButton = TryCast(item, RibbonButton)
            If btn IsNot Nothing AndAlso btn.DropDownItems.Count > 0 Then
                setupRibbonHandlers(btn.DropDownItems)
            End If
        Next
    End Sub

    Private Sub initControls(ByVal parentControl As Control, ByVal loadSetting As Boolean, ByVal loadTooltip As Boolean, ByVal loadTheme As Boolean)
        For Each c As Control In parentControl.Controls

            If loadTheme Then
                Dim cTheme As iTheme = TryCast(c, iTheme)
                If cTheme IsNot Nothing Then
                    cTheme.applyTheme()
                Else
                    formatControl(c)
                End If

            End If

            If loadTooltip Then
                Dim cTool As iTooltip = TryCast(c, iTooltip)
                If cTool IsNot Nothing Then
                    Dim tt As String = ToolTipMaker.GetToolTip(cTool)
                    Dim conTooltip As String = cTool.getToolTip
                    If tt.ToString = "" Then
                        tt = conTooltip
                    Else
                        tt = conTooltip & vbCrLf & vbCrLf & tt
                    End If
                    ToolTipMaker.SetToolTip(cTool, tt)
                End If
            End If

            If loadSetting Then
                Dim conOpt As iToken = TryCast(c, iToken)
                If conOpt IsNot Nothing Then If c.Enabled Then conOpt.loadOption() 'only load options of enabled controls
            End If

            If c.HasChildren Then
                initControls(c, loadSetting, loadTooltip, loadTheme)
            End If
        Next
    End Sub

#Region "formatting and themes"
    Private Sub rBtnThemes_DropDownItemClicked(sender As Object, e As RibbonItemEventArgs) Handles rBtnThemes.DropDownItemClicked
        If My.Settings.THEME = e.Item.Tag.ToString.ToUpper Then Exit Sub

        My.Settings.THEME = e.Item.Tag.ToString.ToUpper

        setTheme()
        Dim frmWait As New frmThemeChange
        Me.Opacity = 0
        frmWait.Show()
        Application.DoEvents() 'hurrghh, this is a bad idea
        initControls(Me, False, False, True)
        Me.Opacity = 1
        frmWait.Hide()
    End Sub

    Private Sub formatControl(ByVal c As Control)
        'this formats any non-custom controls with the ribbon's theme colors
        'currently groupboxes and labels only have their forecolor changed,
        'since solid backgrounds hides our beautiful background image

        Select Case c.GetType
            Case GetType(Button)
                Dim btn As Button = DirectCast(c, Button)
                btn.ForeColor = Theme.ColorTable.Text

                btn.FlatAppearance.MouseOverBackColor = Theme.ColorTable.ButtonSelected_2013
                btn.FlatAppearance.MouseDownBackColor = Theme.ColorTable.ButtonSelected_2013

                If Theme.ThemeColor <> RibbonTheme.Normal Then
                    btn.BackgroundImage = Nothing
                    btn.BackgroundImageLayout = ImageLayout.None
                    btn.BackColor = Theme.ColorTable.RibbonBackground_2013
                    btn.FlatAppearance.CheckedBackColor = Theme.ColorTable.RibbonBackground_2013
                Else
                    btn.BackgroundImage = My.Resources.transp_1
                    btn.BackgroundImageLayout = ImageLayout.Tile
                    btn.BackColor = Color.Transparent
                    btn.FlatAppearance.CheckedBackColor = Color.Transparent
                End If

            Case GetType(Label)
                c.ForeColor = Theme.ColorTable.Caption1
                c.BackColor = Color.Transparent

            Case GetType(GroupBox)
                c.ForeColor = Theme.ColorTable.Caption1

            Case GetType(ComboBox)
                Dim cb As ComboBox = DirectCast(c, ComboBox)
                cb.ForeColor = Theme.ColorTable.Text
                cb.BackColor = Theme.ColorTable.ButtonPressed_2013
                cb.FlatStyle = FlatStyle.Flat

            Case GetType(CustomTabControl)

                If Theme.ThemeColor = RibbonTheme.Normal Then
                    tabMain.DisplayStyle = TabStyle.VS2010
                    tabMain.DisplayStyleProvider.BorderColor = Theme.ColorTable.RibbonBackground_2013
                    tabMain.DisplayStyleProvider.Radius = 1
                Else
                    tabMain.DisplayStyle = TabStyle.Angled
                    tabMain.DisplayStyleProvider.BorderColor = Theme.ColorTable.PanelDarkBorder
                End If

                tabMain.DisplayStyleProvider.SelectedTabColor = Theme.ColorTable.TabActiveBackground_2013
                tabMain.DisplayStyleProvider.SelectedTabColorTop = Theme.ColorTable.TabActiveBackground_2013

                tabMain.DisplayStyleProvider.TextColor = Theme.ColorTable.TabText_2013
                tabMain.DisplayStyleProvider.TextColorDisabled = Theme.ColorTable.TabText_2013
                tabMain.DisplayStyleProvider.TextColorSelected = Theme.ColorTable.TabText_2013

                tabMain.DisplayStyleProvider.ShowTabCloser = False
                tabMain.DisplayStyleProvider.HotTrack = False
                tabMain.DisplayStyleProvider.FocusTrack = False
        End Select
    End Sub

#End Region



#Region "tileset change and preview"

    Private Sub btnUpdateSaves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateSaves.Click
        graphicsSets.updateSavedGames()
    End Sub

    Private Sub btnTilesetPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTilesetPreview.Click
        If Not m_frmPreview.Visible Then
            m_frmPreview.Show()
        Else
            m_frmPreview.BringToFront()
        End If
    End Sub

    Private Sub cmbTileSets_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbTileSets.SelectionChangeCommitted
        graphicsSets.switchGraphics(cmbTileSets.SelectedValue)
    End Sub
#End Region



#Region "color preview"

    Private Sub optCbColors_Leave(sender As Object, e As EventArgs) Handles optCbColors.Leave
        tileSetColorPreviewer.Hide()
    End Sub

    Private Sub optCbColors_LostFocus(sender As Object, e As EventArgs) Handles optCbColors.LostFocus
        tileSetColorPreviewer.Hide()
    End Sub


    Private Sub optCbColors_MouseLeave(sender As Object, e As EventArgs) Handles optCbColors.MouseLeave
        'Debug.WriteLine("Hiding color preview")
        tileSetColorPreviewer.Hide()
    End Sub

    Private Sub optCbColors_MouseMove(sender As Object, e As MouseEventArgs) Handles optCbColors.MouseMove
        'Debug.WriteLine("Showing color preview")
        Dim strPath As String = ""
        Try
            strPath = findMwFilePath(CType(optCbColors.SelectedItem, comboFileItem).fileName)
            If strPath.Trim <> "" Then
                tileSetColorPreviewer.refreshColors(strPath)
                Dim loc As Point = optCbColors.FindForm().PointToClient(optCbColors.Parent.PointToScreen(optCbColors.Location))
                tileSetColorPreviewer.Location = New Point(loc.X + optCbColors.Width + 4, loc.Y - (Me.Height - Me.ClientSize.Height) - ribbonMain.Height)
                tileSetColorPreviewer.Visible = True
                tileSetColorPreviewer.BringToFront()
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
        End Try
    End Sub

    Private Sub optCbColors_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles optCbColors.SelectionChangeCommitted
        tileSetColorPreviewer.refreshColors(findMwFilePath(CType(optCbColors.SelectedItem, comboFileItem).fileName))
    End Sub

#End Region



#Region "world gen"

    Private Sub loadWorldGenCombo()
        cmbWorldGenIndex.Items.Clear()

        Dim tempItem As New comboItem
        tempItem.value = -1
        tempItem.display = "ALL"
        cmbWorldGenIndex.Items.Add(tempItem)
        For Each key As Integer In globals.m_tokensWorldGen.Keys
            tempItem = New comboItem
            tempItem.display = globals.m_tokensWorldGen.Item(key).Item("TITLE")(0).ToString
            tempItem.value = key
            cmbWorldGenIndex.Items.Add(tempItem)
        Next
        cmbWorldGenIndex.ValueMember = "value"
        cmbWorldGenIndex.DisplayMember = "display"

        cmbWorldGenIndex.SelectedIndex = 0
    End Sub

    Private Sub refreshWorldGen(sender As Object, e As EventArgs)
        'set the global world gen being edit
        globals.currentWorldGenIndex = CType(cmbWorldGenIndex.SelectedItem, comboItem).value
        'refresh our world gen controls. the internal loading will check the global var
        'm_refreshingWorldGen = True
        initControls(tabWorldGen, True, False, False)
        'm_refreshingWorldGen = False
    End Sub

    Private Sub tabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
        If tabMain.SelectedTab Is tabWorldGen Then
            AddHandler cmbWorldGenIndex.SelectionChangeCommitted, AddressOf refreshWorldGen
        Else
            RemoveHandler cmbWorldGenIndex.SelectionChangeCommitted, AddressOf refreshWorldGen
            globals.currentWorldGenIndex = -2
        End If
    End Sub

#End Region



#Region "random creature generation"

    Private Sub randomcreatureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateCreatures.Click
        If btnDelRandoms.Enabled Then
            If MsgBox("It appears some random creatures already exist, are you sure you want to overwrite them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Overwrite") = MsgBoxResult.No Then
                Exit Sub
            End If

        End If

        Try
            Dim f_info As IO.FileInfo = findDfFile("RandCreatures.exe")

            If f_info IsNot Nothing Then
                Dim pExec As New Process
                Dim pInfo As New ProcessStartInfo
                With pInfo
                    .WorkingDirectory = f_info.Directory.Parent.FullName 'run in objects folder
                    .UseShellExecute = True
                    .FileName = f_info.FullName
                    .WindowStyle = ProcessWindowStyle.Normal
                    .Verb = "runas"
                End With
                pExec = Process.Start(pInfo)
                pExec.WaitForExit()
                pExec.Close()
            End If

        Catch ex As Exception
            MsgBox("Failed to run RandCreatures.exe!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End Try
        'refresh the file list
        fileWorking.loadDfFilePaths()
        randomCreaturesExistCheck()
    End Sub

    Private Sub btnDelRandoms_Click(sender As Object, e As EventArgs) Handles btnDelRandoms.Click
        If MsgBox("Remove all random creatures, civilizations and languages?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Delete") = MsgBoxResult.No Then
            Exit Sub
        End If
        'clear the random creature, entity and language files
        Dim mgr As New optionManager()
        mgr.saveFile(findDfFilePath("entity_random_rc.txt"), "", False)
        mgr.saveFile(findDfFilePath("language_RANDOM.txt"), "", False)
        mgr.saveFile(findDfFilePath("creature_random_rc.txt"), "", False)
        btnDelRandoms.Enabled = False
    End Sub

    Private Function randomCreaturesExistCheck() As Boolean
        Try
            Dim data As String = readFile(findDfFilePath("creature_random_rc.txt"), False)
            If data.Contains("[CREATURE:") Then
                btnDelRandoms.Enabled = True
                Return True
            Else
                btnDelRandoms.Enabled = False
                Return False
            End If
        Catch ex As Exception
            btnDelRandoms.Enabled = False
            Return False
        End Try
    End Function
#End Region



#Region "ribbon handlers"

#Region "general menu buttons"

    Private Sub rBtnPlayDF_Click(sender As Object, e As EventArgs) Handles rBtnPlayDF.Click
        runAppViaBat(findDfFile("Dwarf Fortress.exe"))
    End Sub

    Private Sub rBtnOpenDwarfFortress_Click(sender As Object, e As EventArgs) Handles rBtnOpenDwarfFortress.Click
        Process.Start("explorer.exe", m_dwarfFortressRootDir)
    End Sub
    Private Sub rBtnOpenSaves_Click(sender As Object, e As EventArgs) Handles rBtnOpenSaves.Click
        Process.Start("explorer.exe", IO.Path.Combine(globals.m_dwarfFortressRootDir, "data", "save"))
    End Sub
    Private Sub rBtnOpenUtilities_Click(sender As Object, e As EventArgs) Handles rBtnOpenUtilities.Click
        Process.Start("explorer.exe", m_utilityDir)
    End Sub

    Private Sub rBtnAbout_Click(sender As Object, e As EventArgs) Handles rBtnAbout.Click
        Dim f As New frmAbout
        frmAbout.ShowDialog()
    End Sub
#End Region

#Region "manuals and donate"
    Private Sub rBtnManualDwarf_Click(sender As Object, e As EventArgs) Handles rBtnManualDwarf.Click
        Process.Start("Dwarf Manual.html")
    End Sub
    Private Sub rBtnManualKobold_Click(sender As Object, e As EventArgs) Handles rBtnManualKobold.Click
        Process.Start("Kobold Manual.html")
    End Sub
    Private Sub rBtnManualOrc_Click(sender As Object, e As EventArgs) Handles rBtnManualOrc.Click
        Process.Start("Orc Manual.html")
    End Sub
    Private Sub rBtnDonations_Click(sender As Object, e As EventArgs) Handles rBtnDonations.Click
        Process.Start(IO.Path.Combine(globals.m_masterworkRootDir, "repository", "donate.html"))
    End Sub
#End Region

#Region "url menus"

    Private Sub ribbonUrl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If TypeOf sender Is RibbonItem Then
                Process.Start(CType(sender, RibbonItem).Tag.ToString)
            End If
        Catch ex As Exception
            MsgBox("Failed to open specified URL!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Debug.WriteLine(ex.ToString)
        End Try
    End Sub

    Private Sub ribbonExe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If TypeOf sender Is RibbonItem Then
                runAppViaBat(findMwFile(CType(sender, RibbonItem).Tag.ToString))
            End If
        Catch ex As Exception
            MsgBox("Failed to launch executable!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Debug.WriteLine(ex.ToString)
        End Try
    End Sub

#End Region

#End Region


#Region "basic test of all options"

    'this doesn't include applying graphic tilsets, or launching the ulilities or menu urls

    Private Sub MainForm_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If Not Debugger.IsAttached Then Exit Sub
        If e.Alt And e.Control And e.KeyCode = Keys.Oemtilde Then
            If MsgBox("Run test? This will change raws!", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            testSettings(tabMain)
            MsgBox("Test Complete.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
        End If
    End Sub


    Private Sub testSettings(ByVal parentControl As Control)
        If Not Debugger.IsAttached Then Exit Sub

        For Each c As Control In parentControl.Controls
            If Not c.Enabled Then
                Debug.WriteLine("skipping disabled control: " & c.Name)
            Else
                Dim conOpt As iTest = TryCast(c, iTest)
                If conOpt IsNot Nothing Then
                    Try
                        Debug.WriteLine("TESTING... " & c.Name)
                        conOpt.runtTest()
                        Debug.WriteLine("TEST ENDED")
                    Catch ex As Exception
                        Debug.WriteLine("!TEST EXCEPTION! " & ex.ToString)
                    End Try
                End If

                If c.HasChildren Then
                    testSettings(c)
                End If
            End If
        Next
    End Sub

#End Region

 
End Class

