Imports MasterworkDwarfFortress.globals
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions

<Microsoft.VisualBasic.ComClass()> Public Class MainForm

#Region "declarations"
    Private m_randomEntityName As String
    Private m_randomCreatureName As String

    Private m_frmPreview As New frmTilesetPreviewer
    Private m_refreshingWorldGen As Boolean = False
#End Region

    Public Sub New()
        'load the proper theme before we initialize controls
        Theme.ColorTable = New ribbonThemeDark

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ribbonMain.Refresh()
    End Sub



    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        refreshFileAndDirPaths()
        If m_dwarfFortressRootDir <> "" Then
            loadTokenData()
            GraphicsSets.findGraphicPacks(m_graphicsDir) 'and graphics
        End If

        setupRibbonHandlers(rPanelGeneral.Items)
        setupRibbonHandlers(rPanelUtilities.Items)

        Me.BackColor = Theme.ColorTable.OrbDropDownDarkBorder

        Me.Text = "Masterwork Settings Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString

        randomCreaturesExistCheck()

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

    Private Sub initControls(ByVal parentControl As Control)
        For Each c As Control In parentControl.Controls

            Dim conOpt As iToken = TryCast(c, iToken)
            If conOpt IsNot Nothing Then                
                If c.Enabled Then conOpt.loadOption() 'only load options of enabled controls
                If Not m_refreshingWorldGen Then 'when refreshing from world gen, we just want to reload

                    If TypeOf (c) Is mwCheckBox Then
                        'for an unknown reason, this is the only way to get the theme forecolor to stick for
                        'the checkbox buttons
                        c.ForeColor = Theme.ColorTable.Text
                    End If

                    If TypeOf conOpt Is iTooltip Then 'set the tooltip
                        Dim tt As String = ToolTipMaker.GetToolTip(conOpt)
                        Dim conTooltip As String = CType(conOpt, iTooltip).getToolTip
                        If tt.ToString = "" Then
                            tt = conTooltip
                        Else
                            tt = conTooltip & vbCrLf & vbCrLf & tt
                        End If
                        ToolTipMaker.SetToolTip(conOpt, tt)
                    End If
                End If
            Else
                'regular control, needs formatting
                If Not m_refreshingWorldGen Then formatControl(c)
            End If

            If c.HasChildren Then
                initControls(c)
            End If
        Next
    End Sub

    Private Sub formatControl(ByVal c As Control)
        Select Case c.GetType
            Case GetType(Button)
                Dim btn As Button = DirectCast(c, Button)
                btn.ForeColor = Theme.ColorTable.Text
                btn.FlatAppearance.MouseOverBackColor = Theme.ColorTable.TabSelectedGlow
                btn.FlatAppearance.MouseDownBackColor = Theme.ColorTable.TabSelectedGlow
            Case GetType(Label)
                c.ForeColor = Theme.ColorTable.Text
            Case GetType(GroupBox)
                c.ForeColor = Theme.ColorTable.Text
            Case GetType(ComboBox)
                Dim cb As ComboBox = DirectCast(c, ComboBox)
                cb.ForeColor = Theme.ColorTable.Text
                cb.BackColor = Theme.ColorTable.TabSelectedGlow
                cb.FlatStyle = FlatStyle.Standard
        End Select
    End Sub

    Public Sub loadTokenData()
        'load variables with contents of init files
        m_init = fileWorking.readFile(findDfFilePath(m_initFileName))
        m_world_gen = fileWorking.readFile(findDfFilePath(m_worldGenFileName))
        m_dinit = fileWorking.readFile(findDfFilePath(m_dInitFileName))
        If m_init <> "" And m_dinit <> "" And m_world_gen <> "" Then

            m_tokensInit = tokenLoading.loadFileTokens(m_init)
            m_tokensDInit = tokenLoading.loadFileTokens(m_dinit)
            tokenLoading.loadWorldGenTokens()

            initControls(Me.tabMain)
            loadWorldGenCombo()
        Else
            Me.Close()
        End If
    End Sub


#Region "tileset change and preview"

    Private Sub ChangeGraphicsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstallGraphics.Click
        If cmbTileSets.SelectedItem IsNot Nothing Then
            GraphicsSets.switchGraphics(cmbTileSets.SelectedValue)
        Else
            MsgBox("Select a graphics set from the list first!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub UpdateSaveGamesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateSaves.Click
        GraphicsSets.updateSavedGames()
    End Sub

    Private Sub tilesetPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTilesetPreview.Click
        If Not m_frmPreview.Visible Then
            m_frmPreview.Show()
        Else
            m_frmPreview.BringToFront()
        End If
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
        m_refreshingWorldGen = True
        initControls(tabWorldGen)
        m_refreshingWorldGen = False
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

