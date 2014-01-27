Imports System.ComponentModel
Imports MasterworkDwarfFortress.globals
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions

<DisplayNameAttribute("Files"), _
DescriptionAttribute("This is a list of all filenames this token is found in."), _
CategoryAttribute("~RAW Options"), _
TypeConverterAttribute(GetType(fileManagerConverter))> _
Public Class fileListManager

    Public Sub New()
    End Sub

    Private m_fileNames As String()
    Private m_files As New List(Of IO.FileInfo)
    Private m_currentPattern As Regex

    <DescriptionAttribute("The file name(s) which contains this option's token(s). Filenames can include wildcards (ie. creature_*.txt)"), _
    EditorAttribute(GetType(MultilineStringArrayConverter), GetType(System.ComponentModel.Design.MultilineStringEditor)), _
    TypeConverter(GetType(fileListConverter))> _
    Public Property fileNames As String()
        Get
            Return m_fileNames
        End Get
        Set(value As String())
            m_fileNames = value
        End Set
    End Property


    Private Function findFiles(ByVal tokens As rawTokenCollection) As List(Of IO.FileInfo)
        Dim results As New List(Of IO.FileInfo)
        If tokens.Count <= 0 Then Return results
        Dim blnContinue As Boolean = False
        For Each t As rawToken In tokens
            If t.optionOnValue <> "" Or t.optionOffValue <> "" Then blnContinue = True : Exit For
        Next
        If Not blnContinue Then Return results

        Dim fNames As New List(Of String)
        For Each fi As KeyValuePair(Of IO.FileInfo, String) In globals.m_dfRaws
            'If fNames.Contains(fi.Key.Name) Then results.Add(fi.Key) : Continue For

            For Each t As rawToken In tokens
                If Not singleValueToken(t) Then
                    If fi.Value.Contains(t.optionOnValue) Then
                        results.Add(fi.Key) : fNames.Add(fi.Key.Name) : Exit For
                    ElseIf fi.Value.Contains(t.optionOffValue) Then
                        results.Add(fi.Key) : fNames.Add(fi.Key.Name) : Exit For
                    End If
                Else
                    If fi.Value.Contains(String.Format("[{0}:", t.tokenName)) Then results.Add(fi.Key) : fNames.Add(fi.Key.Name) : Exit For
                End If
            Next
        Next

        Dim resultCount As Integer = results.Count

        'load the related graphics pack files as well
        If results.Count > 0 Then
            Dim fPaths As New List(Of String)
            Dim rx As New Regex("(" & String.Join(")|(", fNames) & ")", RegexOptions.IgnoreCase)
            results.AddRange(mwGraphicFilePaths.FindAll(Function(f) rx.IsMatch(f.Name)))
        End If

        'If resultCount <> results.Count Then
        '    Debug.WriteLine("break")
        'End If

        Return results
    End Function

    Private Function findFiles(ByVal pattern As String) As List(Of IO.FileInfo)
        Dim rx As New Regex(pattern)
        Dim results As New List(Of IO.FileInfo)
        If pattern = "" Then Return results

        For Each fi As KeyValuePair(Of IO.FileInfo, String) In globals.m_dfRaws
            If rx.IsMatch(fi.Value) Then results.Add(fi.Key)
        Next

        Return results
    End Function

    Private Function singleValueToken(ByVal token As rawToken) As Boolean
        Return (token.optionOffValue = "" And token.optionOnValue <> "")
    End Function

    Public Function loadFiles(ByVal tokens As rawTokenCollection) As List(Of IO.FileInfo)
        If m_files.Count <= 0 Then
            m_files = findFiles(tokens)
        End If

        Return m_files
    End Function

    Public Function loadFiles(ByVal pattern As String) As List(Of IO.FileInfo)
        If m_files.Count <= 0 Then
            m_files = findFiles(pattern)
        End If
        Return m_files
    End Function

    Public ReadOnly Property files As List(Of IO.FileInfo)
        Get
            Return m_files
        End Get
    End Property

    'Private Function findFilePaths(ByVal fileName As String) As List(Of String)
    '    Dim f_info As IO.FileInfo = Nothing
    '    Dim paths As New List(Of String)
    '    If fileName.Contains("*") Then
    '        buildPattern(fileName)
    '        Dim f_infos As List(Of IO.FileInfo) = dfFilePaths.FindAll(Function(f As IO.FileInfo) m_currentPattern.IsMatch(f.Name))
    '        For Each f_info In f_infos
    '            If Not paths.Contains(f_info.FullName) Then
    '                paths.Add(f_info.FullName)
    '            End If
    '        Next
    '    Else
    '        Dim strPath As String = findDfFilePath(fileName)
    '        If strPath <> "" Then
    '            paths.Add(strPath)
    '        Else
    '            Throw New Exception("File " & fileName & " not found!")
    '        End If
    '    End If

    '    Return paths
    'End Function

    'Private Sub buildPattern(ByVal fileName As String)
    '    m_currentPattern = New Regex("^" & fileName.Replace("*", ".*"), RegexOptions.IgnoreCase)
    'End Sub


    'Private Sub findRelatedGraphicsFilePaths()
    '    Dim fPaths As New List(Of String)
    '    Dim rx As New Regex("(" & String.Join(")|(", m_fileNames.ToArray) & ")", RegexOptions.IgnoreCase)
    '    Dim f_infos As List(Of IO.FileInfo) = mwGraphicFilePaths.FindAll(Function(f) rx.IsMatch(f.Name))
    '    For Each f_info As IO.FileInfo In f_infos
    '        fPaths.Add(f_info.FullName)
    '    Next
    '    'for tilesets, report errors, but they're not crucial as not all files/tokens may be present
    '    If fPaths.Count <= 0 Then
    '        Debug.WriteLine("One or more of the following files were not found in any graphics packs! " & String.Join(", ", m_fileNames))
    '    End If
    '    'Return fPaths
    '    m_filePaths.AddRange(fPaths)
    'End Sub

End Class


Public Class fileManagerConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(fileListManager) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is fileListManager Then
            Dim flm As fileListManager = CType(value, fileListManager)
            Return String.Join(",", flm.fileNames)
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class