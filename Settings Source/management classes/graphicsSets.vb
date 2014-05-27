Imports System.Text.RegularExpressions
Imports MasterworkDwarfFortress.fileWorking
Imports System.Xml

Public Class graphicsSets

    Public Shared Sub findGraphicPacks(ByVal directory As String)
        MainForm.cmbTileSets.DataSource = Nothing

        MainForm.cmbTileSets.DataSource = mwGraphicDirs
        MainForm.cmbTileSets.ValueMember = "Name"
        MainForm.cmbTileSets.DisplayMember = "Name"

        Try
            MainForm.cmbTileSets.SelectedValue = My.Settings.Item("GRAPHICS")
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub switchGraphics(ByVal packName As String, Optional ByVal showPrompts As Boolean = True)
        If showPrompts AndAlso MsgBox("This will change raw files and update the graphics!" & vbNewLine & vbNewLine & "It will NOT update your saved games!" & vbNewLine & vbNewLine & "Continue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Graphics") = MsgBoxResult.No Then
            Exit Sub
        End If
        Try

            'first update the graphics section (sky,chasm,pillar,tracks) from the graphics d_init to the df d_init
            Dim om As New optionManager
            Dim basePattern As String = ".*(\\" & packName & "\\)"
            Dim rx As New Regex(basePattern & ".*(d_init.txt)", RegexOptions.IgnoreCase)
            Dim f_info As IO.FileInfo = mwGraphicFilePaths.Find(Function(f As IO.FileInfo) rx.IsMatch(f.FullName))
            If f_info IsNot Nothing Then
                Dim graphicsDInit As String = f_info.FullName
                'Dim dfDInit As String = globals.findDfFilePath("d_init.txt")
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

            'next, copy the raw files. for now it's assumed these are in the <graphics pack name dir>\raw\ folder
            Try
                Dim gDir As IO.DirectoryInfo = mwGraphicDirs.Find(Function(d As IO.DirectoryInfo) String.Compare(d.Name, packName, True) = 0)
                If gDir IsNot Nothing Then
                    Dim fsp As MyServices.FileSystemProxy = My.Computer.FileSystem
                    Dim mwPath As String = IO.Path.Combine(gDir.FullName, "raw")
                    Dim dfPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")
                    If fsp.DirectoryExists(mwPath) And fsp.DirectoryExists(dfPath) Then
                        fsp.CopyDirectory(mwPath, dfPath, True)
                        Dim gFilePaths As List(Of String) = IO.Directory.GetFiles(mwPath).ToList
                        For Each fi As IO.FileInfo In globals.m_dfRaws.Keys.Where(Function(f As IO.FileInfo) gFilePaths.Contains(f.Name))
                            globals.m_dfRaws.Item(fi) = readFile(fi.FullName, False)
                        Next
                    Else
                        MsgBox("Invalid paths for graphics pack raws.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
                    End If

                    Try
                        'read the graphic pack to color map file, find the color, and set it
                        Dim mapFile As IO.FileInfo = mwGraphicFilePaths.Find(Function(f As IO.FileInfo) String.Compare(f.Name, "color_map.xml", True) = 0)
                        'if we're not showing prompts, we don't want to mess with the colors
                        If mapFile IsNot Nothing AndAlso showPrompts Then
                            Dim xmlDoc As XmlDocument
                            Dim nodes As XmlNodeList

                            xmlDoc = New XmlDocument
                            xmlDoc.Load(mapFile.FullName)
                            nodes = xmlDoc.SelectNodes("/graphics/pack")

                            For Each node As XmlNode In nodes
                                If String.Compare(node.ChildNodes(0).InnerText, packName, True) = 0 Then
                                    Dim color As String = node.ChildNodes(1).InnerText
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

            My.Settings.Item("GRAPHICS") = packName
            If showPrompts Then MsgBox("Graphics successfully switched.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Success")

        Catch ex As Exception
            MsgBox("Something went horribly wrong while attempting to switch graphics!" & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
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
