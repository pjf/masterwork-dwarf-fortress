Imports MasterworkDwarfFortress.globals
Imports MasterworkDwarfFortress.utils
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Imports System.Web.Script.Serialization
Imports System.Text
Imports Newtonsoft.Json


<Microsoft.VisualBasic.ComClass()> Public Class MainForm

#Region "declarations"
    Private m_frmPreview As New frmTilesetPreviewer
    Private m_currTheme As RibbonProfesionalRendererColorTable
    Private m_frmWait As New frmWait    
#End Region

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        setTheme()

        globals.m_defaultSerializeOptions.NullValueHandling = NullValueHandling.Ignore
        globals.m_defaultSerializeOptions.Formatting = Formatting.Indented
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
        Theme.ColorTable = m_currTheme
        Me.BackColor = Theme.ColorTable.RibbonBackground_2013
        ribbonMain.Refresh()
        m_frmWait.updateTheme()
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If rCbProfiles.SelectedItem IsNot Nothing Then
            If MsgBox("Would you like to save the changes to profile " & rCbProfiles.SelectedItem.Text & "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Save Profile") = MsgBoxResult.Yes Then
                rBtnSaveProfile_Click(rBtnSaveProfile, Nothing)
            End If
        End If
        If My.Settings.Properties("WORLDGEN") IsNot Nothing Then
            My.Settings.WORLDGEN = rCheckWorldGen.Checked
        End If
    End Sub


    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        refreshFileAndDirPaths()
        If m_dwarfFortressRootDir <> "" Then
            graphicsSets.loadColorSchemes(Me.optCbColors)
            initialLoad()
            graphicsSets.findGraphicPacks(m_graphicsDir)
        End If

        setupRibbonHandlers(rPanelGeneral.Items)
        setupRibbonHandlers(rPanelUtilities.Items)

        Me.Text = "Masterwork Settings"

        randomCreaturesExistCheck()

        loadProfileCombo()

        'DISABLED - updating saves is incredibly broken due to the messy raws
        'If fileWorking.savedGameDirs.Count > 1 Then
        '    btnUpdateSaves.Enabled = True : btnUpdateSaves.Visible = True
        'End If

        'cycle through all our tabs to ensure everything is visible immediately
        For Each t As TabPage In tabMain.TabPages
            tabMain.SelectedTab = t
        Next
        tabMain.SelectedIndex = 0

        'add debugging tools to the menu
        If Not Debugger.IsAttached Then
            ribbonMain.Tabs.Remove(rTabDev)
        End If

        If My.Settings.Properties("WORLDGEN") IsNot Nothing Then
            rCheckWorldGen.Checked = My.Settings.WORLDGEN
        End If
    End Sub

    'this override prevents flickering when drawing transparent controls over background images within a tabcontrol
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
            MsgBoxExp("Load Failed", "Initial Load Failed", MessageBoxIcon.Error, "Unable to find and load file paths. Masterwork settings requires Dwarf Fortress to be installed in the same directory!", MessageBoxButtons.OK, ex.ToString)
            Application.Exit()
        End Try
    End Sub

    Public Sub initialLoad()
        'read init and world gen files
        m_init = fileWorking.readFile(findDfFilePath(m_initFileName))
        m_world_gen = fileWorking.readFile(findDfFilePath(m_worldGenFileName))
        m_dinit = fileWorking.readFile(findDfFilePath(m_dInitFileName))
        If m_init <> "" And m_dinit <> "" And m_world_gen <> "" Then
            Dim start As DateTime = Now
            'load init and world gen tokens
            m_tokensInit = tokenLoading.loadFileTokens(m_init)
            m_tokensDInit = tokenLoading.loadFileTokens(m_dinit)
            tokenLoading.loadWorldGenTokens()

            'load all the civ table controls first
            loadCivTable()

            'load all our current options, and format our controls to the current theme
            initControls(Me, ToolTipMaker, True, True, True)

            'load the world gen templates
            loadWorldGenCombo()

            Dim elapsed As TimeSpan = Now - start
            Console.WriteLine("LOADING TIME: " & elapsed.TotalSeconds & " seconds.")
        Else
            Me.Close()
        End If
    End Sub

#Region "profiles"

    Private Sub loadProfileCombo()
        If rCbProfiles.DropDownItems.Count > 0 Then
            rCbProfiles.DropDownItems.Clear()
        End If
        For Each fi As IO.FileInfo In mwProfiles
            addProfileItem(fi)
        Next
        sortProfiles()
    End Sub

    Private Sub addProfileItem(ByVal fi As IO.FileInfo, Optional ByVal sort As Boolean = False)
        Dim rItem As New RibbonLabel()
        rItem.Image = Nothing
        rItem.Text = IO.Path.GetFileNameWithoutExtension(fi.Name)
        rItem.Value = fi.FullName.ToLower
        rCbProfiles.DropDownItems.Add(rItem)
        sortProfiles()
    End Sub

    Private Sub sortProfiles()
        If Not Environment.OSVersion.Platform = PlatformID.Win32Windows AndAlso Not Environment.OSVersion.Platform = PlatformID.Win32NT Then
            rCbProfiles.DropDownItems.Sort()
        End If
    End Sub

    Private Sub rBtnNewProfile_Click(sender As Object, e As EventArgs) Handles rBtnNewProfile.Click
        Try
            Dim dSave As New SaveFileDialog()
            dSave.DefaultExt = ".JSON"
            dSave.OverwritePrompt = True
            dSave.InitialDirectory = IO.Path.Combine(Application.StartupPath, globals.m_profilesDir)
            dSave.Filter = "Profile Files (*.JSON)|*.JSON;*.json|All Files (*.*)|*.*"
            dSave.FilterIndex = 0
            If dSave.ShowDialog = Windows.Forms.DialogResult.OK Then
                saveSettings(dSave.FileName)
                Dim fi As New IO.FileInfo(dSave.FileName)
                fileWorking.mwProfiles.Add(fi)
                addProfileItem(fi)
                rCbProfiles.SelectedValue = dSave.FileName.ToLower
            End If
        Catch ex As Exception
            MsgBoxExp("Create Failed", "Create Failed", MessageBoxIcon.Error, "A new profile could not be created.", MsgBoxStyle.OkOnly, ex.ToString)
        End Try
    End Sub

    Private Sub rBtnSaveProfile_Click(sender As Object, e As EventArgs) Handles rBtnSaveProfile.Click
        Try
            If rCbProfiles.SelectedValue Is Nothing Then Exit Sub
            Dim strPath As String = rCbProfiles.SelectedItem.Value
            If IO.File.Exists(strPath) Then
                saveSettings(strPath)
                MsgBox(rCbProfiles.SelectedItem.Text & " has been successfully updated.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Profile Updated")
            End If
        Catch ex As Exception
            MsgBoxExp("Save Failed", "Save Failed", MessageBoxIcon.Error, "The current profile could not be saved.", MsgBoxStyle.OkOnly, ex.ToString)
        End Try
    End Sub

    Private Sub rBtnApplyProfile_Click(sender As Object, e As EventArgs) Handles rBtnApplyProfile.Click
        Try
            If rCbProfiles.SelectedValue Is Nothing Then Exit Sub
            Dim strPath As String = rCbProfiles.SelectedItem.Value
            If IO.File.Exists(strPath) Then
                showWaitScreen("Loading profile, please wait...")
                loadSettings(strPath)
            End If
        Catch ex As Exception
            MsgBoxExp("Apply Failed", "Apply Failed", MessageBoxIcon.Error, "The selected profile could not be applied.", MsgBoxStyle.OkOnly, ex.ToString)
        Finally
            hideWaitScreen()
        End Try
    End Sub

    Private Sub rBtnDelProfile_Click(sender As Object, e As EventArgs) Handles rBtnDelProfile.Click
        If rCbProfiles.SelectedValue IsNot Nothing Then
            If MsgBox("Are you sure you want to delete " & rCbProfiles.SelectedItem.Text & "?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Deletion") = MsgBoxResult.Yes Then
                Try
                    Dim strPath As String = rCbProfiles.SelectedItem.Value
                    If IO.File.Exists(strPath) Then
                        deleteProfileItem(strPath)
                        IO.File.Delete(strPath)
                    End If
                    rCbProfiles.SelectedValue = Nothing
                Catch ex As Exception
                    MsgBoxExp("Delete Failed", "Delete Failed", MessageBoxIcon.Error, "The selected profile could not be deleted.", MsgBoxStyle.OkOnly, ex.ToString)
                End Try
            End If
        End If
    End Sub
    Private Sub deleteProfileItem(ByVal strPath As String)
        Dim matches As List(Of IO.FileInfo) = fileWorking.mwProfiles.Where(Function(p As IO.FileInfo) String.Compare(strPath, p.FullName, True) = 0).ToList
        If matches.Count > 0 Then
            fileWorking.mwProfiles.Remove(matches(0))
            For Each r As RibbonLabel In rCbProfiles.DropDownItems
                If r.Value.ToLower = matches(0).FullName.ToLower Then
                    rCbProfiles.DropDownItems.Remove(r)
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub rBtnResetProfiles_Click(sender As Object, e As EventArgs) Handles rBtnResetProfiles.Click
        Try
            Dim files As New List(Of IO.FileInfo)
            files = fileWorking.getOriginalProfiles
            Dim strCurrPath As String
            For Each fi As IO.FileInfo In files
                strCurrPath = fi.FullName.Replace(".original", ".JSON")
                IO.File.Copy(fi.FullName, strCurrPath, True)
                If fileWorking.mwProfiles.Where(Function(p As IO.FileInfo) String.Compare(p.FullName, strCurrPath, True) = 0).Count <= 0 Then
                    Dim pfi As New IO.FileInfo(strCurrPath)
                    fileWorking.mwProfiles.Add(pfi)
                    addProfileItem(pfi)
                End If
            Next
            sortProfiles()
            MsgBox("Default profiles have been restored successfully.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Defaults Restored")
        Catch ex As Exception
            MsgBoxExp("Reset Failed", "Reset Failed", MessageBoxIcon.Error, "Failed to reset to the default profiles.", MsgBoxStyle.OkOnly, ex.ToString)
        End Try
    End Sub

    Private Sub saveSettings(ByVal strPath As String)
        Try            
            Dim newSettings As New Dictionary(Of String, Object)
            saveSettings(tabMain, newSettings)
            'add any non-itoken control settings we want to save
            newSettings.Add("cmbTileSets", cmbTileSets.SelectedValue)
            If rCheckWorldGen.Checked Then
                newSettings.Add("WORLD_GEN", m_world_gen)
            Else
                'keep the original world gen details the profile has
                Dim oldSettings As New Dictionary(Of String, Object)
                oldSettings = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(fileWorking.readFile(strPath, False), globals.m_defaultSerializeOptions)
                If oldSettings IsNot Nothing AndAlso oldSettings.ContainsKey("WORLD_GEN") Then
                    newSettings.Add("WORLD_GEN", oldSettings.Item("WORLD_GEN"))
                End If
            End If

            Dim info As String = JsonConvert.SerializeObject(newSettings, newSettings.GetType(), globals.m_defaultSerializeOptions)
            If Not IO.File.Exists(strPath) Then IO.File.Create(strPath).Dispose()
            fileWorking.saveFile(strPath, info)
        Catch ex As Exception
            MsgBoxExp("Profile Save", "Profile Save Failed", MessageBoxIcon.Error, "The selected profile could not be saved.", MessageBoxButtons.OK, ex.ToString)
        End Try
    End Sub
    Private Sub saveSettings(ByVal parentControl As Control, ByRef optionSettings As Dictionary(Of String, Object))
        If Not parentControl Is tabWorldGen Then
            For Each c As Control In parentControl.Controls
                If controlIsValid(c) Then
                    Dim conOpt As iToken = TryCast(c, iToken)
                    If conOpt IsNot Nothing Then
                        Try
                            optionSettings.Add(c.Name, conOpt.currentValue)
                        Catch ex As Exception
                            Debug.WriteLine("!TEST EXCEPTION! " & ex.ToString)
                        End Try
                    End If

                    If c.HasChildren Then
                        saveSettings(c, optionSettings)
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub loadSettings(ByVal filePath As String)
        Try
            If IO.File.Exists(filePath) Then
                Dim optionSettings As New Dictionary(Of String, Object)
                optionSettings = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(fileWorking.readFile(filePath, False), globals.m_defaultSerializeOptions)
                loadSettings(tabMain, optionSettings)
                'load any other non-itoken controls
                If optionSettings.ContainsKey("cmbTileSets") Then
                    Dim value As String = optionSettings.Item("cmbTileSets")
                    cmbTileSets.SelectedValue = value
                    graphicsSets.switchGraphics(value, False)
                End If
                If rCheckWorldGen.Checked AndAlso optionSettings.ContainsKey("WORLD_GEN") Then
                    Dim currPath As String = findDfFilePath(globals.m_worldGenFileName)
                    If currPath <> "" Then
                        fileWorking.saveFile(currPath, m_world_gen)
                        m_world_gen = optionSettings.Item("WORLD_GEN")
                        tokenLoading.loadWorldGenTokens()
                        loadWorldGenCombo()
                        refreshWorldGen(Nothing, Nothing)
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBoxExp("Profile Load", "Profile Load Failed", MessageBoxIcon.Error, "The selected profile could not be loaded.", MessageBoxButtons.OK, ex.ToString)
        End Try
    End Sub
    Private Sub loadSettings(ByVal parentControl As Control, ByVal optionSettings As Dictionary(Of String, Object))
        If Not parentControl Is tabWorldGen Then
            For Each c As Control In parentControl.Controls
                If controlIsValid(c) Then
                    Dim conOpt As iToken = TryCast(c, iToken)
                    If conOpt IsNot Nothing Then
                        Try
                            If optionSettings.ContainsKey(c.Name) Then
                                conOpt.loadOption(optionSettings.Item(c.Name))
                            End If
                        Catch ex As Exception
                            Debug.WriteLine("!SETTINGS EXCEPTION! " & ex.ToString)
                        End Try
                    End If

                    If c.HasChildren Then
                        loadSettings(c, optionSettings)
                    End If
                End If
            Next
        End If
    End Sub


#End Region

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

#Region "formatting and themes"

    Private Sub rBtnThemes_DropDownItemClicked(sender As Object, e As RibbonItemEventArgs) Handles rBtnThemes.DropDownItemClicked
        If My.Settings.THEME = e.Item.Tag.ToString.ToUpper Then Exit Sub
        Try
            My.Settings.THEME = e.Item.Tag.ToString.ToUpper
            Dim currTab As TabPage = tabMain.SelectedTab

            setTheme()

            showWaitScreen("Changing theme, please wait...")
            initControls(Me, ToolTipMaker, False, False, True)
            Me.tabMain.SelectedTab = currTab

        Catch ex As Exception
            MsgBox("There has been a problem changing the theme.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Theme Failed")
        Finally
            hideWaitScreen()
        End Try
    End Sub

#End Region

    Private Sub showWaitScreen(ByVal message As String, Optional ByVal opacity As Double = 0.0)
        Me.Opacity = opacity
        'Me.tabMain.SelectedTab = tabSettings 'choose a tab with few controls as the refresh causes massive flickering
        m_frmWait.lblMsg.Text = message
        m_frmWait.Show()
        Application.DoEvents() 'hurrghh, this is a bad idea
    End Sub

    Private Sub hideWaitScreen()
        If m_frmWait IsNot Nothing AndAlso m_frmWait.Visible Then
            Me.Opacity = 1
            m_frmWait.Hide()
        End If
    End Sub

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
        If optCbColors.SelectedItem Is Nothing Then Exit Sub
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
        initControls(tabWorldGen, ToolTipMaker, True, False, False)
    End Sub

    Private Sub tabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
        If tabMain.SelectedTab Is tabWorldGen Then
            AddHandler cmbWorldGenIndex.SelectionChangeCommitted, AddressOf refreshWorldGen
        Else
            RemoveHandler cmbWorldGenIndex.SelectionChangeCommitted, AddressOf refreshWorldGen
            globals.currentWorldGenIndex = -2
        End If
    End Sub

    Private Sub btnResetWorldGen_Click(sender As Object, e As EventArgs) Handles btnResetWorldGen.Click
        If MsgBox("This will reset all the world generation parameters and the map templates to the defaults. Continue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm") = MsgBoxResult.No Then Exit Sub
        Try
            Dim currPath As String = findDfFilePath(globals.m_worldGenFileName)
            Dim originalPath As String
            If currPath <> "" Then
                originalPath = currPath.Replace(".txt", ".original")
                If Not IO.File.Exists(originalPath) Then
                    MsgBox("No default world generation file (world_gen.original) could be found to restore!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Missing File")
                    Exit Sub
                End If
                IO.File.Copy(originalPath, currPath, True)
                m_world_gen = fileWorking.readFile(findDfFilePath(m_worldGenFileName))
                tokenLoading.loadWorldGenTokens()
                loadWorldGenCombo()
                refreshWorldGen(Nothing, Nothing)
            End If
        Catch ex As Exception
            MsgBoxExp("World Gen", "World Gen Error", MessageBoxIcon.Error, "There was a problem attempting to reset all the world generation settings and maps to the defaults.", MessageBoxButtons.OK, ex.ToString)
        End Try
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
            runApp(f_info, f_info.Directory.Parent.FullName, True) 'run in objects folder

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
        tabMain.SelectedTab.Focus()
        runApp(findDfFile("Dwarf Fortress.exe"))
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
    Private Sub rBtnManualGnome_Click(sender As Object, e As EventArgs) Handles rBtnManualGnome.Click
        Process.Start("Gnome Manual.html")
    End Sub
    Private Sub rBtnManualWarlock_Click(sender As Object, e As EventArgs) Handles rBtnManualWarlock.Click
        Process.Start("Warlock Manual.html")
    End Sub
    Private Sub rBtnManualSuccubi_Click(sender As Object, e As EventArgs) Handles rBtnManualSuccubi.Click
        Process.Start("Succubus Manual.html")
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
                runApp(findMwFile(CType(sender, RibbonItem).Tag.ToString))
            End If
        Catch ex As Exception
            MsgBox("Failed to launch executable!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Debug.WriteLine(ex.ToString)
        End Try
    End Sub

#End Region

#End Region


#Region "option testing and exporting"

    'this doesn't include applying graphic tilesets, or launching the utilities or menu urls
    Private Sub rBtnTest_Click(sender As Object, e As EventArgs) Handles rBtnTest.Click
        If Not Debugger.IsAttached Then Exit Sub
        If MsgBox("Run test? This will change raws!", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        testOptions(tabMain)
        MsgBox("Test Complete.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
    End Sub

    Private Sub rBtnExport_Click(sender As Object, e As EventArgs) Handles rBtnExport.Click
        If Not Debugger.IsAttached Then Exit Sub
        Dim frmInfo As New Form
        frmInfo.Text = "Exported Options"
        frmInfo.Size = New Size(500, 400)
        Dim btnSaveExportData As New Button
        btnSaveExportData.Text = "Save"
        AddHandler btnSaveExportData.Click, AddressOf saveExport
        frmInfo.Controls.Add(btnSaveExportData)
        btnSaveExportData.Dock = DockStyle.Bottom
        Dim rtext As New RichTextBox
        frmInfo.Controls.Add(rtext)
        rtext.Dock = DockStyle.Fill
        rtext.BringToFront()

        Dim exportedObjects As New List(Of simpleExportObject)
        exportOptions(tabMain, exportedObjects)

        Dim strInfo As String = JsonConvert.SerializeObject(exportedObjects, globals.m_defaultSerializeOptions)
        rtext.Text = strInfo
        btnSaveExportData.Tag = strInfo
        rtext.ReadOnly = True
        frmInfo.Show()
    End Sub

    Private Sub saveExport(ByVal sender As Object, ByVal e As EventArgs)
        Dim s As New SaveFileDialog()
        s.InitialDirectory = Application.StartupPath
        If s.ShowDialog() = Windows.Forms.DialogResult.OK Then
            saveFile(s.FileName, CType(sender, Control).Tag.ToString)
        End If
    End Sub

    Private Sub testOptions(ByVal parentControl As Control)
        If Not Debugger.IsAttached Then Exit Sub

        For Each c As Control In parentControl.Controls
            If controlIsValid(c) Then
                Dim conOpt As iTest = TryCast(c, iTest)
                If conOpt IsNot Nothing Then
                    Try
                        Debug.WriteLine("TESTING... " & c.Name)
                        conOpt.runTest()
                        Debug.WriteLine("TEST ENDED")
                    Catch ex As Exception
                        Debug.WriteLine("!TEST EXCEPTION! " & ex.ToString)
                    End Try
                End If

                If c.HasChildren Then
                    testOptions(c)
                End If
            End If
        Next
    End Sub

    Private Sub exportOptions(ByVal parentControl As Control, ByRef exportedObjects As List(Of simpleExportObject))
        If Not Debugger.IsAttached Then Exit Sub

        For Each c As Control In parentControl.Controls
            If controlIsValid(c) Then
                Dim conOpt As iExportInfo = TryCast(c, iExportInfo)
                If conOpt IsNot Nothing Then
                    Try
                        Dim sobj As New simpleExportObject(c, ToolTipMaker)
                        exportedObjects.Add(sobj)
                    Catch ex As Exception
                        Debug.WriteLine("!PRINT EXCEPTION! " & ex.ToString)
                    End Try
                End If

                If c.HasChildren Then
                    exportOptions(c, exportedObjects)
                End If
            End If
        Next
    End Sub

#End Region


#Region "civ table loading"
    Private m_comboItemNames As List(Of String) = New List(Of String)(New String() {"Never", "Very Early", "Early", "Default", "Late", "Very Late"})
    Private m_popLevels As List(Of String) = New List(Of String)(New String() {"N/A", "20", "50", "80", "110", "140"})
    Private m_wealthLevels As List(Of String) = New List(Of String)(New String() {"N/A", "5000", "25000", "100000", "200000", "300000"})
    Private m_exportLevels As List(Of String) = New List(Of String)(New String() {"N/A", "500", "2500", "10000", "20000", "30000"})

    'column indexes
    Enum colIndexes
        idxActive = 1
        idxPlaybleFort = 2
        idxPlayableAdv = 3
        idxFaction = 4
        idxCaravan = 5
        idxBodyguards = 6
        idxInvasion = 7
        idxAi = 8
        idxSkulk = 9
        idxMaterials = 10
        idxSkills = 11
        idxSeason = 12
    End Enum


    Dim civControlHeight As Integer = 24

    Private Sub loadCivTable()
        'width/height based on table cell sizes
        civControlHeight = Me.tableLayoutCivs.GetControlFromPosition(0, 1).Height
        Dim intCtrlWidth As Integer = 50

        'our main label that has all the information we'll need to load the various options
        Dim civLabel As mwCivLabel
        Dim civName As String = ""

        'set some tooltips
        ToolTipMaker.SetToolTip(lblCivCaravans, buildTriggerTooltip())
        ToolTipMaker.SetToolTip(lblCivInvasions, buildTriggerTooltip())

        Me.tableLayoutCivs.SuspendLayout()
        For idxRow As Integer = 1 To Me.tableLayoutCivs.RowCount - 1
            civLabel = Me.tableLayoutCivs.GetControlFromPosition(0, idxRow)
            If civLabel Is Nothing Then
                Continue For
            Else
                civName = civLabel.simpleCivName.ToLower
                civName = StrConv(civName, VbStrConv.ProperCase)
                civName = civName.Replace(" ", "")

                'add active option
                buildSimpleCivButton("Active", civLabel.simpleCivName, "CIV_ACTIVE", colIndexes.idxActive, idxRow)
                AddHandler CType(Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxActive, idxRow), optionSingleReplaceButton).CheckedChanged, AddressOf civActiveCheckedChanged
                AddHandler CType(Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxActive, idxRow), optionSingleReplaceButton).optionLoaded, AddressOf civActiveLoaded

                'add playable fortress mode option
                buildSimpleCivButton("FortMode", civLabel.simpleCivName, "FORT_MODE", colIndexes.idxPlaybleFort, idxRow)
                Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxPlaybleFort, idxRow).Enabled = civLabel.playableFortMode

                'add playable adventure mode option
                buildSimpleCivButton("AdvMode", civLabel.simpleCivName, "ADV_MODE", colIndexes.idxPlayableAdv, idxRow)
                Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxPlayableAdv, idxRow).Enabled = civLabel.playableAdvMode

                'add the faction option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxFaction, 0).Width
                Dim cbMultiFaction As New optionComboBoxMultiToken
                cbMultiFaction.Name = "optCbMultiCivFaction" & civName
                formatCivTableControl(cbMultiFaction, intCtrlWidth, civControlHeight)
                buildFactionOption(cbMultiFaction, civLabel.simpleCivName, civLabel.factionable)
                Me.tableLayoutCivs.Controls.Add(cbMultiFaction, colIndexes.idxFaction, idxRow)
                If Not civLabel.factionable Then cbMultiFaction.SelectedValue = "N/A" : cbMultiFaction.Enabled = False

                'add a caravan option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxCaravan, 0).Width
                Dim cbCaravans As New optionComboPatternToken
                cbCaravans.Name = "optCbPatternCivCaravans" & civName
                formatCivTableControl(cbCaravans, intCtrlWidth, civControlHeight)
                buildTriggerOption(cbCaravans, civLabel.simpleCivName & "_TRADE")
                Me.tableLayoutCivs.Controls.Add(cbCaravans, colIndexes.idxCaravan, idxRow)

                'add caravan bodyguard option
                buildSimpleCivButton("Bodyguards", civLabel.simpleCivName, "BODYGUARDS", colIndexes.idxBodyguards, idxRow)

                'add an invasion option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxInvasion, 0).Width
                Dim cbInvasions As New optionComboPatternToken
                cbInvasions.Name = "optCbPatternCivInvasions" & civName
                formatCivTableControl(cbInvasions, intCtrlWidth, civControlHeight)
                buildTriggerOption(cbInvasions, civLabel.simpleCivName & "_SIEGE")
                Me.tableLayoutCivs.Controls.Add(cbInvasions, colIndexes.idxInvasion, idxRow)

                'add AI type
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxAi, 0).Width
                Dim cbAi As New optionComboCheckbox
                cbAi.Name = "optCbCheckAi" & civName
                formatCivTableControl(cbAi, intCtrlWidth, civControlHeight)
                buildAiOption(cbAi, civLabel.simpleCivName)
                Me.tableLayoutCivs.Controls.Add(cbAi, colIndexes.idxAi, idxRow)

                'add skulking option
                buildSimpleCivButton("Skulking", civLabel.simpleCivName, "SKULKING", colIndexes.idxSkulk, idxRow)

                'add a material option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxMaterials, 0).Width
                Dim cbMats As optionComboPatternToken = New optionComboPatternToken
                cbMats.Name = "optCbPatternCivMats" & civName
                formatCivTableControl(cbMats, intCtrlWidth, civControlHeight)
                buildMatOption(cbMats, civLabel.simpleCivName & "_MATERIALS")
                Me.tableLayoutCivs.Controls.Add(cbMats, colIndexes.idxMaterials, idxRow)

                'add a skill option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxSkills, 0).Width
                Dim cbSkills As optionComboPatternToken = New optionComboPatternToken
                cbSkills.Name = "optCbPatternCivSkills" & civName
                formatCivTableControl(cbSkills, intCtrlWidth, civControlHeight)
                buildSkillOption(cbSkills, civLabel.simpleCivName)
                Me.tableLayoutCivs.Controls.Add(cbSkills, colIndexes.idxSkills, idxRow)

                'add a seasonal option
                intCtrlWidth = Me.tableLayoutCivs.GetControlFromPosition(colIndexes.idxSeason, 0).Width
                Dim cbSeasons As New optionComboCheckbox
                cbSeasons.Name = "optCbSeasons" & civName
                formatCivTableControl(cbSeasons, intCtrlWidth, civControlHeight)
                buildSeasonOption(cbSeasons, civLabel.simpleCivName)
                Me.tableLayoutCivs.Controls.Add(cbSeasons, colIndexes.idxSeason, idxRow)
            End If

        Next
        Me.tableLayoutCivs.ResumeLayout()
        'Me.tableLayoutCivs.AutoScroll = True 'need this if we add any more civs to the table
    End Sub

    Private Sub civActiveCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim civActive As optionSingleReplaceButton = TryCast(sender, optionSingleReplaceButton)
        If civActive IsNot Nothing Then
            Dim pos As TableLayoutPanelCellPosition = Me.tableLayoutCivs.GetPositionFromControl(CType(sender, Control))
            'get the civLabel
            Dim civLabel As mwCivLabel = TryCast(Me.tableLayoutCivs.GetControlFromPosition(0, pos.Row), mwCivLabel)
            If civLabel IsNot Nothing Then
                For colIdx As Integer = colIndexes.idxActive + 1 To Me.tableLayoutCivs.ColumnCount - 1
                    If colIdx = colIndexes.idxPlaybleFort AndAlso civLabel.playableFortMode = False Then Continue For
                    If colIdx = colIndexes.idxPlayableAdv AndAlso civLabel.playableAdvMode = False Then Continue For
                    If colIdx = colIndexes.idxFaction AndAlso civLabel.factionable = False Then Continue For 'can't toggle faction

                    Dim colCon As Control = Me.tableLayoutCivs.GetControlFromPosition(colIdx, pos.Row)
                    If colCon IsNot Nothing Then colCon.Enabled = civActive.Checked
                Next
            End If
        End If
    End Sub

    'to aid in loading the civ table, once the active button has been loaded, copy the file name to the other 
    'controls in the same row, since most of them will use the exact same entity file
    'additionally, check for missing controllable tags and disable the buttons accordingly
    Private Sub civActiveLoaded(ByVal btn As optionSingleReplaceButton)
        If btn IsNot Nothing Then
            Dim pos As TableLayoutPanelCellPosition = Me.tableLayoutCivs.GetPositionFromControl(CType(btn, Control))

            'get the civLabel
            Dim civLabel As mwCivLabel = TryCast(Me.tableLayoutCivs.GetControlFromPosition(0, pos.Row), mwCivLabel)
            If civLabel IsNot Nothing Then
                If btn.options.fileManager.files.Count > 0 Then
                    civLabel.playableFortMode = globals.m_dfRaws.Item(btn.options.fileManager.files()(0)).Contains("CIV_CONTROLLABLE")
                    civLabel.playableAdvMode = globals.m_dfRaws.Item(btn.options.fileManager.files()(0)).Contains("INDIV_CONTROLLABLE")
                Else
                    civLabel.playableAdvMode = False
                    civLabel.playableFortMode = False
                End If
            End If

            For colIdx As Integer = colIndexes.idxActive + 1 To Me.tableLayoutCivs.ColumnCount - 1
                If colIdx <> colIndexes.idxSkills Then 'any options that are NOT using the entity file(s) should be excluded
                    Dim c As Control = TryCast(Me.tableLayoutCivs.GetControlFromPosition(colIdx, pos.Row), Control)
                    If c IsNot Nothing Then
                        Dim cEnabled As iEnabled = TryCast(c, iEnabled)
                        If cEnabled IsNot Nothing Then
                            If colIdx = colIndexes.idxPlayableAdv Then cEnabled.isEnabled = civLabel.playableAdvMode
                            If colIdx = colIndexes.idxPlaybleFort Then cEnabled.isEnabled = civLabel.playableFortMode
                        End If
                        If c.GetType.GetProperty("options") IsNot Nothing Then
                            CObj(c).options.fileManager.fileNames.AddRange(btn.options.fileManager.fileNames)
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Dim formatSnatcherOn As String = "YES_BABYSNATCHER_{0}["
    Dim formatSnatcherOff As String = "!NO_BABYSNATCHER_{0}!"
    Dim formatThiefOn As String = "YES_ITEMTHIEF_{0}["
    Dim formatThiefOff As String = "!NO_ITEMTHIEF_{0}!"

    Private Sub buildFactionOption(ByRef cb As optionComboBoxMultiToken, ByVal civ As String, ByVal factionable As Boolean)
        cb.options.optionManager.checkAllOnLoad = True

        If factionable Then
            Dim snatcherOff As String = String.Format(formatSnatcherOff, civ)
            Dim snatcherOn As String = String.Format(formatSnatcherOn, civ)
            Dim thiefOff As String = String.Format(formatThiefOff, civ)
            Dim thiefOn As String = String.Format(formatThiefOn, civ)

            'civilized enable = !snatcher && !thief
            cb.options.itemList.Add(New comboMultiTokenItem("CIVILIZED", "Civilized"))
            cb.options.itemList.Item(0).tokens.Add(New rawToken("", snatcherOff, snatcherOn))
            cb.options.itemList.Item(0).tokens.Add(New rawToken("", thiefOff, thiefOn))

            'slaver enable = snatcher && !thief
            cb.options.itemList.Add(New comboMultiTokenItem("SLAVERS", "Slavers"))
            cb.options.itemList.Item(1).tokens.Add(New rawToken("", snatcherOn, snatcherOff))
            cb.options.itemList.Item(1).tokens.Add(New rawToken("", thiefOff, thiefOn))

            'savage enable = !snatcher && thief
            cb.options.itemList.Add(New comboMultiTokenItem("SAVAGE", "Savage"))
            cb.options.itemList.Item(2).tokens.Add(New rawToken("", snatcherOff, snatcherOn))
            cb.options.itemList.Item(2).tokens.Add(New rawToken("", thiefOn, thiefOff))

            'evil enable = snatcher && thief
            cb.options.itemList.Add(New comboMultiTokenItem("EVIL", "Evil"))
            cb.options.itemList.Item(3).tokens.Add(New rawToken("", snatcherOn, snatcherOff))
            cb.options.itemList.Item(3).tokens.Add(New rawToken("", thiefOn, thiefOff))
        Else
            'placeholder
            cb.options.itemList.Add(New comboMultiTokenItem("N/A", "N/A"))
        End If
    End Sub

    Private Sub buildAiOption(ByRef cb As optionComboCheckbox, ByVal civ As String)        
        cb.options.optionTags.Add(New rawToken("Ambusher", String.Format("YES_AMBUSHER_{0}[", civ), String.Format("!NO_AMBUSHER_{0}!", civ)))
        cb.options.optionTags.Add(New rawToken("Sieger", String.Format("YES_SIEGER_{0}[", civ), String.Format("!NO_SIEGER_{0}!", civ)))
    End Sub

    Private Sub buildSeasonOption(ByRef cb As optionComboCheckbox, ByVal civ As String)        
        cb.options.optionTags.Add(New rawToken("Spring", String.Format("YES_ACTIVE_SPRING_{0}[", civ), String.Format("!NO_ACTIVE_SPRING_{0}!", civ)))
        cb.options.optionTags.Add(New rawToken("Summer", String.Format("YES_ACTIVE_SUMMER_{0}[", civ), String.Format("!NO_ACTIVE_SUMMER_{0}!", civ)))
        cb.options.optionTags.Add(New rawToken("Autumn", String.Format("YES_ACTIVE_AUTUMN_{0}[", civ), String.Format("!NO_ACTIVE_AUTUMN_{0}!", civ)))
        cb.options.optionTags.Add(New rawToken("Winter", String.Format("YES_ACTIVE_WINTER_{0}[", civ), String.Format("!NO_ACTIVE_WINTER_{0}!", civ)))
    End Sub

    Private Sub formatCivTableControl(ByRef c As Control, ByVal w As Integer, ByVal h As Integer)
        c.Size = New Size(w, h)
        c.Margin = New Padding(3, 1, 3, 1)
        c.Anchor = AnchorStyles.Top
    End Sub

    Private Sub buildSimpleCivButton(ByVal btnName As String, ByVal civName As String, ByVal tag As String, ByVal idxCol As Integer, ByVal idxRow As Integer)
        'add active option
        Dim ctrlWidth As Integer = Me.tableLayoutCivs.GetControlFromPosition(idxCol, 0).Width
        Dim btn As New optionSingleReplaceButton
        btn.Name = "optBtn" & btnName & "Civ" & civName

        If tag = "" Then
            btn.Enabled = False
        Else
            btn.options.enabledValue = String.Format("YES_{0}_{1}[", tag, civName)
            btn.options.disabledValue = String.Format("!NO_{0}_{1}!", tag, civName)
        End If

        btn.ImageAlign = ContentAlignment.MiddleCenter
        btn.Text = ""

        formatCivTableControl(btn, ctrlWidth, civControlHeight)
        Me.tableLayoutCivs.Controls.Add(btn, idxCol, idxRow)
    End Sub

    Private Sub buildSkillOption(ByRef cb As optionComboPatternToken, ByVal civName As String)
        Dim skillComboItems As New comboItemCollection
        For i As Integer = 0 To 15
            skillComboItems.Add(New comboItem(CStr(i), CStr(i)))
        Next

        cb.options.itemList = skillComboItems
        If civName Is Nothing OrElse civName.Trim <> "" Then
            cb.optPattern = New optionBasePattern("(\[NATURAL_SKILL:.*:)(?<value>\d+)(\]YESHARDERINVADER_" & civName & "\b)", "${1}${value}${2}")
        Else
            cb.Enabled = False
        End If
    End Sub

    Private Sub buildMatOption(ByRef cb As optionComboPatternToken, ByVal tag As String)
        Dim matComboItems As New comboItemCollection
        matComboItems.Add(New comboItem("DEFAULT", "Default"))
        matComboItems.Add(New comboItem("WEAK", "Weak"))
        matComboItems.Add(New comboItem("NORMAL", "Normal"))
        matComboItems.Add(New comboItem("STRONG", "Strong"))

        cb.options.itemList = matComboItems

        cb.optPattern = New optionBasePattern("(\[PERMITTED_REACTION:MATERIALS_)(?<value>[A-Z]*)(\]" & tag & "\b)", "${1}${value}${2}")
    End Sub

    Private Sub buildTriggerOption(ByRef cb As optionComboPatternToken, ByVal tag As String)
        'add the combobox items and associated values 0-5
        loadTriggerItems(cb)
        cb.optPattern = New optionBasePattern("(\[PROGRESS_TRIGGER_\w+:)(?<value>\d+)(\]" & tag & ")", "${1}${value}${2}")
    End Sub

    Private Sub loadTriggerItems(ByRef cb As optionComboPatternToken)
        Dim idx As Integer = 0
        For Each s As String In m_comboItemNames
            Dim newItem As New comboItem
            newItem.display = s
            newItem.value = idx
            cb.options.itemList.Add(newItem)
            idx += 1
        Next
    End Sub

    Private Function buildTriggerTooltip() As String
        Dim msg As New List(Of String)
        Dim idx As Integer = 0
        For Each s In m_comboItemNames
            msg.Add(String.Format("{0} - {1}", s, String.Format("{0}: {1} or {2}: {3} or {4}: {5}", "Population", m_popLevels(idx), "Wealth", m_wealthLevels(idx), "Exported", m_exportLevels(idx))))
            idx += 1
        Next
        Return String.Join(vbCrLf & vbCrLf, msg)
    End Function

#End Region

End Class

