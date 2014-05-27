Imports System.ComponentModel
Imports MasterworkDwarfFortress.globals
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions
Imports System.ComponentModel.Design

<DisplayNameAttribute("Override Files"), _
DescriptionAttribute("Specify which file(s) to change this option's tags in. Files will not be searched for the tags."), _
CategoryAttribute("~RAW Options"), _
TypeConverterAttribute(GetType(fileManagerConverter))> _
Public Class fileListManager

    Public Sub New()
    End Sub

    Private m_fileNames As New List(Of String)
    Private m_files As New List(Of IO.FileInfo)
    Private m_currentPattern As Regex
    Private m_fileOverrides As Boolean
    'EditorAttribute(GetType(fileListConverter), GetType(System.ComponentModel.Design.MultilineStringEditor)), _

    <DescriptionAttribute("Specify which file(s) to change this option's tags in. Files will not be searched for the tags."), _
    Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing"), _
    TypeConverter(GetType(fileListConverter))> _
    Public Property fileNames As List(Of String)
        Get
            Return m_fileNames
        End Get
        Set(value As List(Of String))
            m_fileNames = value
        End Set
    End Property


    Private Function findFiles(ByVal optm As optionManager, ByVal tokens As rawTokenCollection) As List(Of IO.FileInfo)
        Dim start As DateTime = Now

        Dim results As New List(Of IO.FileInfo)
        'if we have an override specified, then just find those files
        If m_fileNames.Count > 0 Then
            results = findSpecificFiles()
        Else
            'loading from the init files is handled differently, we don't need to search raws
            If optm.loadFromDInit Or optm.loadFromInit Or optm.loadFromWorldGen Then Return results
            If tokens.Count <= 0 Then Return results

            Dim blnContinue As Boolean = False
            For Each t As rawToken In tokens
                If t.optionOnValue <> "" Or t.optionOffValue <> "" Then blnContinue = True : Exit For
            Next
            If Not blnContinue Then Return results

            For Each fi As KeyValuePair(Of IO.FileInfo, String) In globals.m_dfRaws '.Where(AddressOf gameRawFilter)
                For Each t As rawToken In tokens
                    If Not singleValueToken(t) Then
                        If fi.Value.Contains(t.optionOnValue) OrElse fi.Value.Contains(t.optionOffValue) Then
                            results.Add(fi.Key) : m_fileNames.Add(fi.Key.Name) : Exit For
                        End If
                    Else
                        If fi.Value.Contains(String.Format("[{0}:", t.tokenName)) Then results.Add(fi.Key) : m_fileNames.Add(fi.Key.Name) : Exit For
                    End If
                Next
            Next
            addGraphicFiles(results)
        End If

        Dim elapsed As TimeSpan = Now - start
        Debug.WriteLine("took " & elapsed.TotalMilliseconds & " ms to find the files for tokens " & tokens.ToString)
        Return results
    End Function

    Private Function findFiles(ByVal optm As optionManager, ByVal pattern As String) As List(Of IO.FileInfo)
        Dim start As DateTime = Now
        Dim results As New List(Of IO.FileInfo)

        'if we have an override specified, then just find those files
        If m_fileNames.Count > 0 Then
            results = findSpecificFiles()
        Else
            If optm.loadFromDInit Or optm.loadFromInit Or optm.loadFromWorldGen Then Return results

            Dim rx As New Regex(pattern)
            If pattern = "" Then Return results

            For Each fi As KeyValuePair(Of IO.FileInfo, String) In globals.m_dfRaws '.Where(AddressOf gameRawFilter)
                If rx.IsMatch(fi.Value) Then results.Add(fi.Key) : m_fileNames.Add(fi.Key.Name)
            Next
            addGraphicFiles(results)
        End If
        Dim elapsed As TimeSpan = Now - start
        Debug.WriteLine("took " & elapsed.TotalMilliseconds & " ms to find the files for pattern " & pattern)
        Return results
    End Function

    Private Function findSpecificFiles() As List(Of IO.FileInfo)
        Dim results As New List(Of IO.FileInfo)
        Dim tmpFiles As New List(Of KeyValuePair(Of IO.FileInfo, String))
        For Each fName As String In m_fileNames
            tmpFiles.AddRange(globals.m_dfRaws.Where(Function(raw As KeyValuePair(Of IO.FileInfo, String)) raw.Key.Name.Equals(fName, StringComparison.CurrentCultureIgnoreCase)))
            tmpFiles.AddRange(globals.m_mwRaws.Where(Function(raw As KeyValuePair(Of IO.FileInfo, String)) raw.Key.Name.Equals(fName, StringComparison.CurrentCultureIgnoreCase)))            
        Next
        For Each raw As KeyValuePair(Of IO.FileInfo, String) In tmpFiles
            results.Add(raw.Key)
        Next
        m_fileOverrides = True
        Return results
    End Function

    Private Sub addGraphicFiles(ByVal raws As List(Of IO.FileInfo))
        If raws.Count <= 0 Then Exit Sub
        For Each fi As KeyValuePair(Of IO.FileInfo, String) In globals.m_mwRaws.Where(Function(raw As KeyValuePair(Of IO.FileInfo, String)) (m_fileNames.Contains(raw.Key.Name))).ToList 'm_dfRaws.Where(AddressOf graphicRawFilter)
            If Not raws.Contains(fi.Key) Then raws.Add(fi.Key)
        Next
    End Sub

    Private Function gameRawFilter(ByVal item As KeyValuePair(Of IO.FileInfo, String)) As Boolean
        Return (Not item.Key.FullName.Contains(globals.m_graphicsDir))
    End Function

    Private Function graphicRawFilter(ByVal item As KeyValuePair(Of IO.FileInfo, String)) As Boolean
        Return (item.Key.FullName.Contains(globals.m_graphicsDir) AndAlso m_fileNames.Contains(item.Key.Name))
    End Function


    Private Function singleValueToken(ByVal token As rawToken) As Boolean
        Return (token.optionOffValue = "" And token.optionOnValue <> "")
    End Function

    Public Function loadFiles(ByVal optm As optionManager, ByVal tokens As rawTokenCollection) As List(Of IO.FileInfo)
        If m_files.Count <= 0 Then
            m_files = findFiles(optm, tokens)
        End If

        Return m_files
    End Function

    Public Function loadFiles(ByVal optm As optionManager, ByVal pattern As String) As List(Of IO.FileInfo)
        If m_files.Count <= 0 Then
            m_files = findFiles(optm, pattern)
        End If
        Return m_files
    End Function

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property files(Optional ByVal mwRawsOnly As Boolean = False) As List(Of IO.FileInfo)
        Get
            If mwRawsOnly Then
                Return m_files.Where(Function(fi As IO.FileInfo) (fi.FullName.Contains(globals.m_graphicsDir))).ToList
            Else
                Return m_files.Where(Function(fi As IO.FileInfo) (fi.FullName.Contains(globals.m_graphicsDir) = False)).ToList
            End If
        End Get
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property isOverriden() As Boolean
        Get
            Return m_fileOverrides
        End Get
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property affectsGraphics As Boolean
        Get
            Return files(True).Count > 0
        End Get
    End Property

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
            Return String.Join(", ", flm.fileNames)
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class
