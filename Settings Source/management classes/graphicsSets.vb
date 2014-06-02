Imports System.Text.RegularExpressions
Imports MasterworkDwarfFortress.fileWorking
Imports Newtonsoft.Json

Public Class graphicsSets

    Private Shared m_tilesetManager As New optionMulti

    Public Shared Sub findGraphicPacks(ByVal directory As String)
        Try
            Dim defFile As IO.FileInfo = mwGraphicFilePaths.Find(Function(f As IO.FileInfo) String.Compare(f.Name, "graphics_definitions.JSON", True) = 0)
            If defFile IsNot Nothing Then
                globals.m_graphicPackDefs = JsonConvert.DeserializeObject(Of List(Of graphicPackDefinition))(readFile(defFile.FullName), globals.m_defaultSerializeOptions)
                For Each gdf As graphicPackDefinition In globals.m_graphicPackDefs.Distinct(New graphicPackDefinitionTilesetTypeComparer)
                    m_tilesetManager.tokenList.Add(New rawToken(gdf.tilesetType, getGraphicTag(True, gdf.tilesetType), getGraphicTag(False, gdf.tilesetType)))
                Next
            End If

            MainForm.cmbTileSets.DataSource = Nothing

            'MainForm.cmbTileSets.DataSource = mwGraphicDirs
            MainForm.cmbTileSets.DataSource = globals.m_graphicPackDefs
            MainForm.cmbTileSets.ValueMember = "Name"
            MainForm.cmbTileSets.DisplayMember = "Name"

            MainForm.cmbTileSets.SelectedValue = My.Settings.Item("GRAPHICS")
        Catch ex As Exception

        End Try
    End Sub

    Private Shared Function getGraphicTag(ByVal yesOption As Boolean, ByVal typeName As String) As String
        Dim result As String = ""
        If yesOption Then
            result = String.Format("YES{0}_GRAPHICS[", typeName.ToUpper)
        Else
            result = String.Format("!NO{0}_GRAPHICS!", typeName.ToUpper)
        End If
        Return result
    End Function

    Public Shared Sub switchGraphics(ByVal selectedPackName As String, Optional ByVal showPrompts As Boolean = True)
        If showPrompts AndAlso MsgBox("This will change raw files and update the graphics!" & vbNewLine & vbNewLine & "It will NOT update your saved games!" & vbNewLine & vbNewLine & "Continue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Graphics") = MsgBoxResult.No Then
            Exit Sub
        End If
        Try
            'first update the graphics section (sky,chasm,pillar,tracks) from the graphics d_init to the df d_init
            Dim om As New optionManager
            Dim basePattern As String = ".*(\\" & selectedPackName & "\\)"
            Dim rx As New Regex(basePattern & ".*(d_init.txt)", RegexOptions.IgnoreCase)
            Dim f_info As IO.FileInfo = mwGraphicFilePaths.Find(Function(f As IO.FileInfo) rx.IsMatch(f.FullName))
            If f_info IsNot Nothing Then
                Dim graphicsDInit As String = f_info.FullName
                If graphicsDInit <> "" Then
                    Dim pattern As String = "\[(?(SKY|IDLERS)\w+):(?<value>.*)\]"
                    Dim data As String = readFile(graphicsDInit)
                    Dim graphicsData As String = ""
                    Dim matches As MatchCollection = Regex.Matches(data, pattern)
                    If matches.Count = 2 Then
                        graphicsData = data.Substring(matches(0).Index, matches(1).Index - matches(0).Index)
                        data = globals.m_dinit
                        matches = Regex.Matches(data, pattern)
                        If matches.Count = 2 Then
                            Dim strBefore As String = data.Substring(0, matches(0).Index)
                            Dim strAfter As String = data.Substring(matches(1).Index)
                            If strBefore <> "" And strAfter <> "" And graphicsData <> "" Then
                                data = strBefore & graphicsData & strAfter
                                om.saveFile(findDfFilePath("d_init.txt"), data)
                            End If
                        End If
                    End If
                Else
                    MsgBox("Failed to update tileset data in d_init.txt!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Failed")
                End If
            End If

            'next copy the tileset; currently these are all named 'Phoebus_16x16.png'
            Dim tileSetName As String = "Phoebus_16x16.png"
            rx = New Regex(basePattern & ".*(" & tileSetName & ")", RegexOptions.IgnoreCase)
            f_info = mwGraphicFilePaths.Find(Function(f As IO.FileInfo) rx.IsMatch(f.FullName))
            If f_info IsNot Nothing Then
                Dim gTileSet As String = f_info.FullName
                Dim dfTileSet As String = findDfFilePath(tileSetName)
                If gTileSet <> "" And dfTileSet <> "" Then
                    IO.File.Copy(gTileSet, dfTileSet, True)
                End If
            End If

            'next copy the raw files. for now it's assumed these are in the <graphics pack name dir>\raw\ folder
            Try
                Dim gDir As IO.DirectoryInfo = mwGraphicDirs.Find(Function(d As IO.DirectoryInfo) String.Compare(d.Name, selectedPackName, True) = 0)
                If gDir IsNot Nothing Then
                    Dim fsp As MyServices.FileSystemProxy = My.Computer.FileSystem
                    Dim mwPath As String = IO.Path.Combine(gDir.FullName, "raw")
                    Dim dfPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")
                    If fsp.DirectoryExists(mwPath) And fsp.DirectoryExists(dfPath) Then
                        fsp.CopyDirectory(mwPath, dfPath, True)
                        Dim gFilePaths As List(Of String) = IO.Directory.GetFiles(IO.Path.Combine(mwPath, "objects"), "*.txt", IO.SearchOption.AllDirectories).ToList
                        For idx As Integer = 0 To gFilePaths.Count - 1
                            gFilePaths(idx) = IO.Path.GetFileName(gFilePaths(idx))
                        Next
                        Dim relatedRaws As List(Of IO.FileInfo) = globals.m_dfRaws.Keys.Where(Function(f As IO.FileInfo) gFilePaths.Contains(f.Name)).ToList
                        For Each fi As IO.FileInfo In relatedRaws
                            globals.m_dfRaws.Item(fi) = readFile(fi.FullName, False)
                        Next
                    Else
                        MsgBox("Invalid paths for graphics pack raws.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
                    End If

                    Try
                        'if we're not showing prompts, we don't want to mess with the colors
                        If globals.m_graphicPackDefs.Count > 0 AndAlso showPrompts Then
                            For Each gpd As graphicPackDefinition In globals.m_graphicPackDefs
                                If String.Compare(gpd.name, selectedPackName, True) = 0 Then
                                    Dim color As String = gpd.colorScheme
                                    If color <> MainForm.optCbColors.SelectedValue.ToString AndAlso MsgBox("This tileset has a color scheme associated with it, would you like apply it as well?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Change Colors") = MsgBoxResult.Yes Then
                                        MainForm.optCbColors.SelectedValue = color
                                        MainForm.optCbColors.saveOption()
                                    End If
                                    Exit For
                                End If
                            Next
                        End If

                    Catch ex As Exception
                        MsgBox("Failed to apply the default color scheme!" & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
                    End Try
                Else
                    MsgBox("Could not find the graphics pack directory! No changes have been applied!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
                End If
            Catch ex As Exception
                MsgBox("Failed to copy graphics pack raw folder." & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
            End Try

            'finally toggle the relevant tags in the files affected by the selected graphics pack
            Try
                Dim currGpd As graphicPackDefinition = Nothing
                For Each gpd As graphicPackDefinition In globals.m_graphicPackDefs
                    If gpd.name = selectedPackName Then
                        currGpd = gpd : Exit For
                    End If
                Next
                'this manager will always be 'enabling' the tags, so swap the on to off value if we need to disable the other graphics tags
                If currGpd IsNot Nothing Then
                    Dim yesVal As String = ""
                    For Each t As rawToken In m_tilesetManager.tokenList
                        If t.tokenName <> currGpd.tilesetType Then
                            If t.optionOnValue.Contains("YES") Then
                                yesVal = t.optionOnValue
                                t.optionOnValue = t.optionOffValue
                                t.optionOffValue = yesVal
                            End If
                        Else
                            t.optionOnValue = getGraphicTag(True, currGpd.tilesetType)
                            t.optionOffValue = getGraphicTag(False, currGpd.tilesetType)
                        End If
                    Next
                    'refresh to ensure we have all the files we need
                    m_tilesetManager.loadOption()
                    'save and update the files with the tags
                    m_tilesetManager.saveOption(True)
                End If
            Catch ex As Exception
                utils.MsgBoxExp("Failed", "Graphic Options Problem", MessageBoxIcon.Error, "Failed to change the graphic pack's options in one or more files.", MessageBoxButtons.OK, ex.ToString)
            End Try

            My.Settings.Item("GRAPHICS") = selectedPackName
            If showPrompts Then MsgBox("Graphics successfully switched.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Success")

        Catch ex As Exception
            utils.MsgBoxExp("Graphics Error", "Error Changing Graphics", MessageBoxIcon.Error, "There has been a problem while attempting to switch graphics and/or colors.", MessageBoxButtons.OK, ex.ToString)
        End Try
    End Sub

    Public Shared Sub updateSavedGames()
        If MsgBox("WARNING: This can break the save if you changed settings for creatures, plants or inorganics." & vbCrLf & vbCrLf & _
                  "Changes to buildings, entities and reactions are fine; it is best if you only do this if your current settings are the same as when you generated the world." & _
                  vbCrLf & vbCrLf & "If you are unsure, abort now!", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Confirm Save Game Update") = MsgBoxResult.Yes Then
            Try
                Dim fsp As MyServices.FileSystemProxy = My.Computer.FileSystem
                Dim dfRawPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")

                'find all save games
                Dim saveGameDirs As List(Of IO.DirectoryInfo) = fileWorking.savedGameDirs
                If saveGameDirs.Count > 1 Then 'exclude current

                    For Each save As IO.DirectoryInfo In saveGameDirs.Where(Function(d As IO.DirectoryInfo) d.Name <> "current")
                        Dim saveRawPath As String = IO.Path.Combine(save.FullName, "raw")
                        Dim saveGraphicsPath As String = IO.Path.Combine(saveRawPath, "graphics")
                        'delete the old graphics
                        If fsp.DirectoryExists(saveGraphicsPath) Then fsp.DeleteDirectory(saveGraphicsPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                        'copy all the raws
                        fsp.CopyDirectory(dfRawPath, saveRawPath, True)
                    Next
                    MsgBox("Saved games have been updated successfully!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
                Else
                    MsgBox("No saved games found to update.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
                End If

            Catch ex As Exception
                MsgBox("Something went horribly wrong while attempting to update the saved games!" & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
            End Try
        End If
    End Sub


End Class
