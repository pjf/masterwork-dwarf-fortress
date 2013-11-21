Imports System.Text.RegularExpressions
Imports MasterworkDwarfFortress.fileWorking

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

    Public Shared Sub switchGraphics(ByVal packName As String)
        If MsgBox("This will change raw files and update the graphics, continue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm Graphics") = MsgBoxResult.No Then
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

            'finally, copy the raw files. for now it's assumed these are in the <graphics pack name dir>\raw\ folder
            Try
                Dim gDir As IO.DirectoryInfo = mwGraphicDirs.Find(Function(d As IO.DirectoryInfo) d.Name = packName)
                If gDir IsNot Nothing Then
                    Dim fsp As MyServices.FileSystemProxy = My.Computer.FileSystem
                    Dim mwPath As String = IO.Path.Combine(gDir.FullName, "raw")
                    Dim dfPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")
                    If fsp.DirectoryExists(mwPath) And fsp.DirectoryExists(dfPath) Then
                        fsp.CopyDirectory(mwPath, dfPath, True)
                    Else
                        MsgBox("Invalid paths for graphics pack raws.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
                    End If
                End If
            Catch ex As Exception
                MsgBox("Failed to copy graphics pack raw folder." & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
            End Try

            My.Settings.Item("GRAPHICS") = packName
            MsgBox("Graphics successfully switched.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Success")

        Catch ex As Exception
            MsgBox("Something went horribly wrong while attempting to switch graphics!" & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
        End Try
    End Sub

    Public Shared Sub updateSavedGames()
        If MsgBox("WARNING: This can break the save if you changed settings for creatures, plants or inorganics." & vbCrLf & vbCrLf & "Changes to buildings, entities and reactions are fine; it is best if you only do this if your current settings are the same as when you generated the world." & vbCrLf & vbCrLf & "If you are unsure, abort now!", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Confirm Save Game Update") = MsgBoxResult.Yes Then
            Try
                Dim fsp As MyServices.FileSystemProxy = My.Computer.FileSystem
                Dim dfRawPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")

                'find all savegames
                Dim saveGameDirs As List(Of IO.DirectoryInfo) = getDirectories(IO.Path.Combine(globals.m_dwarfFortressRootDir, "data", "save"), False)
                If saveGameDirs.Count > 0 Then
                    For Each save As IO.DirectoryInfo In saveGameDirs.Where(Function(d As IO.DirectoryInfo) d.Name <> "current")
                        Dim saveRawPath As String = IO.Path.Combine(save.FullName, "raw")
                        Dim saveGraphicsPath As String = IO.Path.Combine(saveRawPath, "graphics")
                        'delete the old graphics
                        If fsp.DirectoryExists(saveGraphicsPath) Then fsp.DeleteDirectory(saveGraphicsPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                        'copy all the raws
                        fsp.CopyDirectory(dfRawPath, saveRawPath, True)
                        MsgBox("Saved games have been updated successfully!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
                    Next
                Else
                    MsgBox("No saved games found to update.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
                End If

            Catch ex As Exception
                MsgBox("Something went horribly wrong while attempting to update the saved games!" & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Failed")
            End Try
        End If
    End Sub

End Class
