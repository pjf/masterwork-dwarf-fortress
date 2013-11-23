Imports MasterworkDwarfFortress.globals

Public Class fileWorking

    Private Shared m_dfFilePaths As New List(Of IO.FileInfo) 'DF files
    Private Shared m_mwFilePaths As New List(Of IO.FileInfo) 'masterwork files used for replacing df files
    Private Shared m_mwGraphicFilePaths As New List(Of IO.FileInfo) 'all files in masterwork graphics packs

    Private Shared m_mwGraphicDirs As New List(Of IO.DirectoryInfo) 'masterwork graphics by directory
    Private Shared m_dfDirs As New List(Of IO.DirectoryInfo)


    Public Shared Sub loadFilePaths()
        loadDfFilePaths()
        loadMwFilePaths()
        loadMwGraphics()
    End Sub

    Public Shared Sub loadDfFilePaths()
        If m_dfFilePaths IsNot Nothing Then m_dfFilePaths.Clear()
        'load the init files (txt)
        m_dfFilePaths.AddRange(getFiles(m_dwarfFortressRootDir & "\data\init", True, New String() {".txt"}))
        'load the art files (png and ttf files)
        m_dfFilePaths.AddRange(getFiles(m_dwarfFortressRootDir & "\data\art", True, New String() {".png", ".ttf"}))

        'add the raw folder, exe extension is for randcreatures
        m_dfFilePaths.AddRange(getFiles(m_dwarfFortressRootDir & "\raw", True, New String() {".txt", ".exe"}))
        'include the top level folder for dfhack.init and df
        m_dfFilePaths.AddRange(getFiles(m_dwarfFortressRootDir, False, New String() {".init", ".exe"}))
    End Sub

    Private Shared Sub loadMwFilePaths()
        'load all setting files (keybinds, colors, etc.)
        m_mwFilePaths.AddRange(getFiles(m_masterworkRootDir & "\Settings", True, New String() {".txt", ".ttf"}))
        'load utility executables
        m_mwFilePaths.AddRange(getFiles(m_masterworkRootDir & "\Utilities", True, New String() {".exe", ".jar"}))
    End Sub

    Private Shared Sub loadMwGraphics()
        m_mwGraphicDirs.AddRange(getDirectories(m_graphicsDir, False))
        m_mwGraphicFilePaths.AddRange(getFiles(m_graphicsDir, True, New String() {".txt", ".png", ".xml"}))
    End Sub


    Public Shared ReadOnly Property dfFilePaths As List(Of IO.FileInfo)
        Get
            Return m_dfFilePaths
        End Get
    End Property

    Public Shared ReadOnly Property mwFilePaths As List(Of IO.FileInfo)
        Get
            Return m_mwFilePaths
        End Get
    End Property

    Public Shared ReadOnly Property mwGraphicDirs As List(Of IO.DirectoryInfo)
        Get
            Return m_mwGraphicDirs
        End Get
    End Property

    Public Shared ReadOnly Property mwGraphicFilePaths As List(Of IO.FileInfo)
        Get
            Return m_mwGraphicFilePaths
        End Get
    End Property

    Public Shared Function getDirectories(ByVal rootDirectory As String, ByVal recursive As Boolean) As List(Of IO.DirectoryInfo)
        Dim searchOpt As IO.SearchOption = If(recursive, IO.SearchOption.AllDirectories, IO.SearchOption.TopDirectoryOnly)
        Return IO.Directory.GetDirectories(rootDirectory, "*.*", searchOpt).Select(Function(d) New IO.DirectoryInfo(d)).ToList
    End Function

    Private Shared Function getFiles(ByVal rootDirectory As String, ByVal recursive As Boolean, ParamArray exts() As String) As List(Of IO.FileInfo)
        Dim searchOpt As IO.SearchOption = If(recursive, IO.SearchOption.AllDirectories, IO.SearchOption.TopDirectoryOnly)
        Return IO.Directory.GetFiles(rootDirectory, "*.*", searchOpt).Where(Function(o) exts.Contains(IO.Path.GetExtension(o))).Select(Function(p) New IO.FileInfo(p)).ToList
    End Function

    Public Shared Function findDfFilePath(ByVal fileName As String) As String
        Return findFilePath(fileName, dfFilePaths)
    End Function

    Public Shared Function findDfFile(ByVal fileName As String) As IO.FileInfo
        Return findFile(fileName, dfFilePaths)
    End Function

    Public Shared Function findMwFile(ByVal fileName As String) As IO.FileInfo
        Return findFile(fileName, mwFilePaths)
    End Function

    Public Shared Function findMwFilePath(ByVal fileName As String) As String
        Return findFilePath(fileName, mwFilePaths)
    End Function

    Private Shared Function findFilePath(ByVal fileName As String, ByVal pathList As List(Of IO.FileInfo)) As String
        Dim f_info As IO.FileInfo = pathList.Find(Function(f As IO.FileInfo) String.Compare(f.Name, fileName, True) = 0)
        If f_info IsNot Nothing Then
            Return f_info.FullName
        Else
            Return ""
        End If
    End Function
    Private Shared Function findFile(ByVal fileName As String, ByVal pathList As List(Of IO.FileInfo)) As IO.FileInfo
        Dim f_info As IO.FileInfo = pathList.Find(Function(f As IO.FileInfo) String.Compare(f.Name, fileName, True) = 0)
        If f_info IsNot Nothing Then
            Return f_info
        Else
            Return Nothing
        End If
    End Function


    'runs a program from its root directory by creating and calling a batch file (then deleting)
    Public Shared Sub runAppViaBat(ByVal f_info As IO.FileInfo)
        Dim bName = "runProgram.bat"
        Dim batch = _
        "@ECHO off" + vbCrLf + _
        "SETLOCAL" + vbCrLf + _
        "CD %~dp0" + vbCrLf + _
        "START """" " + """" + f_info.FullName + """" + vbCrLf + _
        "start /min cmd /c del /q %~s0" 'deletes self afterwards

        Dim bPath As String = IO.Path.Combine(f_info.FullName.Replace(f_info.Name, ""), bName)
        saveFile(bPath, batch) 'save the .bat file
        Process.Start(bPath) 'run the .bat
    End Sub

    Public Sub runApp(ByVal f_info As IO.FileInfo)
        Process.Start(f_info.FullName)
    End Sub

    Public Shared Function setDwarfFortressRoot() As Boolean
        Try
            Dim dfFile As List(Of IO.FileInfo) = IO.Directory.GetFiles(".", "Dwarf Fortress.exe", IO.SearchOption.AllDirectories).Select(Function(p) New IO.FileInfo(p)).ToList()
            If dfFile.Count <= 0 Then
                Return False
            Else
                m_dwarfFortressRootDir = dfFile(0).DirectoryName
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function readFile(ByVal fullFilePath As String, Optional ByVal showWarning As Boolean = True) As String
        If IO.File.Exists(fullFilePath) Then
            Dim sReader As New IO.StreamReader(fullFilePath)
            Dim data As String = sReader.ReadToEnd
            sReader.Close()
            If showWarning And data = "" Then
                MsgBox("File " & fullFilePath & " is empty!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End If
            Return data
        ElseIf showWarning Then
            MsgBox("File " & fullFilePath & " was not found!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
        Return ""
    End Function

    Public Shared Function saveFile(ByVal fullFilePath As String, ByVal newText As String) As Boolean
        Try
            If newText = "" Then
                MsgBox("The target RAW file is missing. Did you move or replace files in your <dwarf fortress>/raw/objects folder?", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                Dim sWriter As New IO.StreamWriter(fullFilePath)
                sWriter.Write(newText)
                sWriter.Close()
            End If
        Catch ex As Exception
            MsgBox("There has been a problem saving file " & fullFilePath & "." & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End Try
    End Function

    Public Shared Function savedGameDirs() As List(Of IO.DirectoryInfo)
        Dim saveDirs As New List(Of IO.DirectoryInfo)
        Try
            Dim dfRawPath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "raw")
            Dim savePath As String = IO.Path.Combine(globals.m_dwarfFortressRootDir, "data", "save")

            If IO.Directory.Exists(savePath) Then
                'find all save games
                saveDirs = getDirectories(savePath, False)
            End If

            Return saveDirs
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Return saveDirs
        End Try
    End Function
End Class