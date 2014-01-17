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
    Private m_filePaths As New List(Of String)
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

    Public Function getFilePaths(ByVal includeGraphicsFiles As Boolean) As List(Of String)
        If m_filePaths.Count <= 0 AndAlso m_fileNames IsNot Nothing AndAlso m_fileNames.Count > 0 Then
            buildFilePaths()
        End If

        If Not includeGraphicsFiles And m_filePaths.Count > 0 Then
            Return m_filePaths.FindAll(Function(f As String) Not f.Contains(m_graphicsDir))
        Else
            Return m_filePaths
        End If

    End Function

    Private Sub buildFilePaths()
        Dim path As String = ""
        For Each f As String In m_fileNames
            m_filePaths.AddRange(findFilePaths(f))
        Next
        'append any files also found in the graphics packs
        findRelatedGraphicsFilePaths()
    End Sub

    Private Function findFilePaths(ByVal fileName As String) As List(Of String)
        Dim f_info As IO.FileInfo = Nothing
        Dim paths As New List(Of String)
        If fileName.Contains("*") Then
            buildPattern(fileName)
            Dim f_infos As List(Of IO.FileInfo) = dfFilePaths.FindAll(Function(f As IO.FileInfo) m_currentPattern.IsMatch(f.Name))
            For Each f_info In f_infos
                If Not paths.Contains(f_info.FullName) Then
                    paths.Add(f_info.FullName)
                End If
            Next
        Else
            Dim strPath As String = findDfFilePath(fileName)
            If strPath <> "" Then
                paths.Add(strPath)
            Else
                Throw New Exception("File " & fileName & " not found!")
            End If
        End If

        Return paths
    End Function

    Private Sub buildPattern(ByVal fileName As String)
        m_currentPattern = New Regex("^" & fileName.Replace("*", ".*"), RegexOptions.IgnoreCase)
    End Sub


    Private Sub findRelatedGraphicsFilePaths()
        Dim fPaths As New List(Of String)
        Dim rx As New Regex("(" & String.Join(")|(", m_fileNames.ToArray) & ")", RegexOptions.IgnoreCase)
        Dim f_infos As List(Of IO.FileInfo) = mwGraphicFilePaths.FindAll(Function(f) rx.IsMatch(f.Name))
        For Each f_info As IO.FileInfo In f_infos
            fPaths.Add(f_info.FullName)
        Next
        'for tilesets, report errors, but they're not crucial as not all files/tokens may be present
        If fPaths.Count <= 0 Then
            Debug.WriteLine("One or more of the following files were not found in any graphics packs! " & String.Join(", ", m_fileNames))
        End If
        'Return fPaths
        m_filePaths.AddRange(fPaths)
    End Sub

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