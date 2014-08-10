Imports System.ComponentModel
Imports MasterworkDwarfFortress.fileWorking
Imports MasterworkDwarfFortress.globals
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text

<DisplayNameAttribute("Other Locations"), _
DescriptionAttribute("These are files that may contain the options which can be used in place of the file list."), _
CategoryAttribute("~RAW Options"), _
TypeConverterAttribute(GetType(optionManagerConverter))> _
Public Class optionManager
    Public Sub New()
    End Sub

    Private m_loadFromInit As Boolean
    Private m_loadFromDInit As Boolean
    Private m_loadFromWorldGen As Boolean

    Private m_worldGenIndex As Integer

    Private m_checkAllOnLoad As Boolean = False

    <DescriptionAttribute("This option's token(s) are found in init.txt file."), _
    DisplayNameAttribute("Init File")> _
    Public Property loadFromInit() As Boolean
        Get
            Return m_loadFromInit
        End Get
        Set(value As Boolean)
            m_loadFromInit = value
        End Set
    End Property

    <DescriptionAttribute("This option's token(s) are found in d_init.txt."), _
    DisplayNameAttribute("D_Init File")> _
    Public Property loadFromDInit() As Boolean
        Get
            Return m_loadFromDInit
        End Get
        Set(value As Boolean)
            m_loadFromDInit = value
        End Set
    End Property

    <DescriptionAttribute("This option's token(s) are found in world_gen.txt."), _
    DisplayNameAttribute("World Gen File")> _
    Public Property loadFromWorldGen() As Boolean
        Get
            Return m_loadFromWorldGen
        End Get
        Set(value As Boolean)
            m_loadFromWorldGen = value
        End Set
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Never), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property worldGenIndex() As Integer
        Get
            Return m_worldGenIndex
        End Get
        Set(value As Integer)
            m_worldGenIndex = value
        End Set
    End Property


    <DescriptionAttribute("Determines if all 'enabled' values are required to be checked when loading."), _
    DisplayNameAttribute("Check All")> _
    Public Property checkAllOnLoad() As Boolean
        Get
            Return m_checkAllOnLoad
        End Get
        Set(value As Boolean)
            m_checkAllOnLoad = value
        End Set
    End Property

    Public Function loadOption(ByVal files As List(Of FileInfo), ByVal tokens As rawTokenCollection, ByVal settingManager As optionSettingManager) As String
        'Try
        Dim retValue As String = ""

        'if we have a settings variable specified, check that first
        If settingManager IsNot Nothing Then
            Dim val As Object = settingManager.getSettingValue
            If val IsNot Nothing Then
                Return val
            End If
        End If

        'loading from a specific init file means we're loading a value from the format of [token:value]
        If loadFromInit Or loadFromDInit Or loadFromWorldGen Then
            For Each t As rawToken In tokens
                If loadFromInit Then
                    If m_tokensInit.Contains(t.tokenName) Then Return m_tokensInit(t.tokenName)
                End If
                If loadFromDInit Then
                    If m_tokensDInit.Contains(t.tokenName) Then Return m_tokensDInit(t.tokenName)
                End If
                If loadFromWorldGen Then
                    If globals.currentWorldGenIndex > -2 Then
                        m_worldGenIndex = globals.currentWorldGenIndex
                    End If

                    If m_worldGenIndex > -1 Then
                        If m_tokensWorldGen.ContainsKey(m_worldGenIndex) Then
                            retValue = m_tokensWorldGen(m_worldGenIndex)(t.tokenName)(0) 'for now we don't load multiple values                    
                        End If
                    Else
                        'return the first template's parameters
                        If m_tokensWorldGen.ContainsKey(0) Then
                            retValue = m_tokensWorldGen(0)(t.tokenName)(0)
                        End If
                    End If
                End If
            Next
        End If

        'if we already have a result by this point, don't bother searching in additional files
        If retValue = "" Then
            'looking for a match to the 'enabled' option specified with the tokens, in the specified files
            'this is most commonly used with the raw files where an option is toggled on/off by replacing a token
            If Not hasSingleValueTokens(tokens) Then
                If Not m_checkAllOnLoad Then
                    Dim strPattern As New List(Of String)
                    For Each t As rawToken In tokens
                        strPattern.Add(Regex.Escape(t.optionOnValue))
                    Next
                    If findTokensInFiles(String.Format("({0})", String.Join(")|(", strPattern)), files.Where(AddressOf rawFilter).ToList) Then
                        retValue = "1"
                    Else
                        retValue = "0"
                    End If
                Else
                    Dim passCount As Integer = 0
                    For Each t As rawToken In tokens                        
                        If findTokensInFiles(String.Format("({0})", Regex.Escape(t.optionOnValue)), files.Where(AddressOf rawFilter).ToList) Then                            
                            passCount += 1
                        End If
                    Next                    
                    If passCount = tokens.Count Then
                        Return "1"
                    Else
                        Return "0"
                    End If
                End If
            Else
                'in this case we're looking for token value(s) within multiple files. 
                'we're only looking to return a single value however, so return the first one
                Dim pattern As String = String.Format("(\[" & tokens.Item(0).tokenName & ":)(?<value>\w+)\]")
                Dim rx As New Regex(pattern, RegexOptions.IgnoreCase)
                Dim m As Match
                For Each fi As FileInfo In files.Where(AddressOf rawFilter).ToList
                    m = rx.Match(m_dfRaws.Item(fi))
                    If m.Success Then Return m.Groups("value").Value
                Next
            End If
        End If

        Return retValue

        'Catch ex As Exception
        '    MsgBox("Failed to load option!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        '    Return ""
        'End Try
    End Function

    Private Function rawFilter(ByVal fi As IO.FileInfo) As Boolean
        Return (Not fi.FullName.Contains(globals.m_graphicsDir))
    End Function

    Private Function findTokensInFiles(ByVal pattern As String, ByVal files As List(Of FileInfo)) As Boolean
        Dim regex As Regex = New Regex(pattern, RegexOptions.IgnoreCase)

        For Each fi As FileInfo In files
            If regex.Match(m_dfRaws.Item(fi)).Success Then Return True
        Next
        Return False
    End Function

    'when changing a specific token's value, we still use the same rawToken structure.
    'since we're not toggling between an on/off value, we only need one of the two values (enabled/disabled)
    'from the rawToken structure. the enabled value is what we use

    'so if a token has only an 'on/enabled' value, we know we're doing a token value update
    Private Function hasSingleValueTokens(ByVal tokens As rawTokenCollection) As Boolean
        For Each t As rawToken In tokens
            If singleValueToken(t) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function singleValueToken(ByVal token As rawToken) As Boolean
        Return (token.optionOffValue = "" And token.optionOnValue <> "")
    End Function

    Public Function saveOption(ByVal fileManager As fileListManager, ByVal tokens As rawTokenCollection, Optional ByVal enabling As Boolean = True) As Boolean
        Try
            'if the tokens have both a enable and disable option, then we do a regex replace in all the specified files
            'if we only have a token name, and an 'on' value, then this indicates we're changing a specific token's value
            If fileManager.files.Count > 0 Then
                If hasSingleValueTokens(tokens) Then
                    If tokens.Item(0).optionOnValue <> "" Then
                        'we have file(s), a token, and a new value, so find the token in each file, and change it's value
                        updateTokensInFiles(fileManager, tokens)
                    Else
                        'we have files, a single token, but no value!
                        Throw New Exception("Attempted to save an empty value!")
                    End If
                Else
                    'we have file(s), and tokens with on/off values, so we need to toggle on/off for the tokens (pattern replace)
                    toggleTokensInFiles(fileManager, tokens, enabling)
                End If
            End If

            If m_loadFromDInit Or m_loadFromInit Or m_loadFromWorldGen Then
                'no specific files given, but we have one of the core DF files selected                
                For Each t As rawToken In tokens

                    'if it's a single value token, our new value will be in the ON variable
                    'otherwise, use whatever is in the on/off depending on whether or not we're enabling/disabling
                    Dim newVal As String = t.optionOnValue
                    If Not enabling AndAlso Not singleValueToken(t) Then newVal = t.optionOffValue

                    If newVal = "" Then
                        Throw New Exception("Attempted to save an empty value!")
                    End If

                    'change the token's value
                    If loadFromDInit Then
                        stringTokenSet(t.tokenName, newVal, m_dinit)
                        saveFile(findDfFilePath(globals.m_dInitFileName), globals.m_dinit)
                    End If
                    If loadFromInit Then
                        stringTokenSet(t.tokenName, newVal, m_init)
                        saveFile(findDfFilePath(globals.m_initFileName), globals.m_init)
                    End If
                    If loadFromWorldGen AndAlso m_worldGenIndex > -2 Then
                        'world gen requires a bit more work, since the tokens are all ready into a data structure
                        'world gen updates also require a token name
                        If t.tokenName.Trim = "" Then
                            Throw New Exception("Attempted to update a world gen. value without a specified token!")
                        End If
                        changeWorldGenValue(t.tokenName, newVal)
                        saveFile(findDfFilePath(globals.m_worldGenFileName), globals.m_world_gen)
                    End If

                Next
            End If

            Return True

        Catch ex As Exception
            MsgBox("Failed to save option!" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return False
        End Try
    End Function


#Region "specific token value replacement"

    'finds the specific tokens specified, and updates their value with the 'on' value of the token
    Private Function updateTokensInFiles(ByVal fManager As fileListManager, ByVal tokens As rawTokenCollection) As Boolean
        Dim success As Boolean = False
        success = updateFileTokens(fManager.files(), tokens) : updateFileTokens(fManager.files(True), tokens)
        Return success
    End Function

    Private Function updateFileTokens(ByVal files As List(Of FileInfo), ByVal tokens As rawTokenCollection)
        Try
            For Each fi As FileInfo In files
                Dim data As String = getFileData(fi) 'm_dfRaws.Item(fi)
                For Each t As rawToken In tokens
                    updateToken(data, t.tokenName, t.optionOnValue)
                Next
                saveFile(fi, data)
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub stringTokenSet(ByVal token As String, ByVal value As String, ByRef fileText As String)
        If value = "" Then Exit Sub
        updateToken(fileText, token, value)
    End Sub

    Private Sub updateToken(ByRef fileText As String, ByVal token As String, ByVal newValue As String)
        Dim pattern As String = "\[" + token + ":.*\]"
        Dim replacement As String = "[" + token + ":" + newValue + "]"
        fileText = Regex.Replace(fileText, pattern, replacement)
    End Sub

#End Region


#Region "toggle on/off value in multiple files and tilesets for multiple tokens"


    Private Function toggleTokensInFiles(ByVal fManager As fileListManager, ByVal tokens As rawTokenCollection, ByVal enable As Boolean) As Boolean
        toggleOption(fManager.files, tokens, enable)
        toggleOption(fManager.files(True), tokens, enable, False)
        Return True
    End Function

    Private Sub toggleOption(ByVal files As List(Of FileInfo), ByVal tokens As rawTokenCollection, ByVal enable As Boolean, Optional ByVal isCriticalChange As Boolean = True)
        If files.Count <= 0 Then Exit Sub

        'keep track of tokens changed
        Dim results As New Dictionary(Of rawToken, Boolean)

        Dim strTokens As New List(Of String)
        For Each t As rawToken In tokens
            results.Add(t, False)
            strTokens.Add(t.ToString)
        Next

        'toggle all on/off values for each token in each file
        For Each fi As FileInfo In files
            If fi IsNot Nothing Then

                Dim originalData As String = getFileData(fi) 'm_dfRaws.Item(fi)
                Dim newData As String = originalData
                Dim updatedData As String = newData

                For Each t As rawToken In tokens
                    Dim oldValue As String
                    Dim newValue As String

                    If enable Then
                        'replace off with on
                        oldValue = t.optionOffValue : newValue = t.optionOnValue
                    Else
                        'replace on with off
                        newValue = t.optionOffValue : oldValue = t.optionOnValue
                    End If

                    If t.isMultiLine Then                        
                        Dim rx As New Regex(t.getMultilinePattern(newValue))
                        If Not rx.IsMatch(newData) Then                            
                            rx = New Regex(t.getMultilinePattern(oldValue))
                            updatedData = rx.Replace(newData, newValue)
                            If Not results(t) Then results(t) = (Not updatedData = newData)
                        Else
                            results(t) = True
                        End If
                    Else
                        If newData.Contains(oldValue) Then
                            updatedData = Replace(newData, oldValue, newValue)
                            If Not results(t) Then results(t) = (Not updatedData = newData)
                        Else
                            results(t) = True
                        End If
                    End If

                    If updatedData = newData And tokens.Count > 1 Then
                        Debug.WriteLine(fi.FullName & " remained unchanged after changing option " & _
                                          IIf(enable, "ON. ", "OFF. ") & _
                                          IIf(t.tokenName <> "", " Token:" & t.tokenName, "") & _
                                          IIf(enable, t.optionOnValue.ToString, t.optionOffValue))
                    Else
                        'save our changes
                        newData = updatedData
                    End If
                Next

                'if our original file has been changed, save it, otherwise report it
                If newData <> originalData Then
                    saveFile(fi, newData)
                Else
                    Debug.WriteLine("File " & fi.FullName & " remained unchanged after applying options: " & String.Join(vbCrLf, strTokens))
                End If
            End If
        Next

        'if any tokens have not been updated, this is a major cause for concern and we want to notify/report somehow
        Dim exMsg As String = "Invalid/missing tokens: " & vbCrLf
        Dim failed As Boolean = False
        For Each t As rawToken In results.Keys
            If Not results(t) Then
                If Not failed Then failed = True
                If enable Then
                    exMsg &= t.optionOnValue.ToString & vbCrLf
                Else
                    exMsg &= t.optionOffValue.ToString & vbCrLf
                End If
            End If
        Next
        If failed Then
            If isCriticalChange Then
                Throw New Exception(exMsg)
            Else
                Debug.WriteLine(exMsg)
            End If
        End If
    End Sub


#End Region

#Region "world gen"

    Public Sub changeWorldGenValue(ByVal strToken As String, ByVal newValue As String)
        'find all matching tokens we're looking to update        
        Dim pattern As String = "\[(?(" & strToken & ")\w+):(?<value>.*)\]"
        Dim strReplace As String = String.Format("[{0}:{1}]", strToken, newValue)
        Dim matches As MatchCollection = Regex.Matches(m_world_gen, pattern)

        If m_worldGenIndex = -1 Then
            'replace ALL tokens
            Dim rx As New Regex(pattern.Replace("?<value>", ""), RegexOptions.IgnoreCase) 'remove the value part since we're replacing the whole line
            m_world_gen = rx.Replace(m_world_gen, strReplace)
            'update the global world gen values
            For Each key As Integer In globals.m_tokensWorldGen.Keys
                globals.m_tokensWorldGen.Item(key).Item(strToken)(0) = newValue
            Next
        Else
            'replace the token for the specific world gen template based on the index by splitting the file
            If matches.Count > 0 And m_worldGenIndex < matches.Count Then
                Dim strBefore As String = m_world_gen.Substring(0, matches(m_worldGenIndex).Index)
                Dim strAfter As String = m_world_gen.Substring(matches(m_worldGenIndex).Index + matches(m_worldGenIndex).Length)
                m_world_gen = strBefore & strReplace & strAfter
                'update the global world gen value
                globals.m_tokensWorldGen.Item(m_worldGenIndex).Item(strToken)(0) = newValue 'currently only handles single tokens
            End If
        End If
    End Sub
#End Region

#Region "pattern replacement loading/saving"
    'these functions are used when replacing specific values within patterns. see the invader skills for an example
    Public Function loadPatternValue(ByVal pattern As String, ByVal files As List(Of FileInfo)) As String
        Dim rx As New Regex(pattern, RegexOptions.IgnoreCase)
        Dim m As Match
        For Each fi As FileInfo In files.Where(AddressOf rawFilter).ToList
            m = rx.Match(m_dfRaws.Item(fi))
            If m.Success Then Return m.Groups("value").Value
        Next
        Return ""
    End Function

    Public Function replacePatternsInFiles(ByVal pattern As String, ByVal replacement As String, ByVal fManager As fileListManager) As Boolean
        Dim success As Boolean = replaceWithPatterns(pattern, replacement, fManager.files)
        replaceWithPatterns(pattern, replacement, fManager.files(True), False)
        Return success
    End Function

    Private Function replaceWithPatterns(ByVal pattern As String, ByVal replacement As String, ByVal files As List(Of FileInfo), Optional ByVal isCritical As Boolean = True) As Boolean
        If files.Count <= 0 Then Return True

        Dim rx As New Regex(pattern, RegexOptions.IgnoreCase)
        Dim changeCount As Integer = 0
        For Each fi As FileInfo In files
            Dim data As String = getFileData(fi)
            Dim updated As String = rx.Replace(data, replacement)
            If data <> updated Then
                saveFile(fi, updated)
                changeCount += 1
            Else
                Debug.WriteLine("File " & fi.FullName & " was unchanged after replacing " & pattern & " with " & replacement & ".")
            End If
        Next

        Return True

        'If isCritical Then
        '    If changeCount > 0 Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'Else
        '    Return True
        'End If
    End Function

    Private Function getFileData(ByVal fi As IO.FileInfo) As String
        If fi.FullName.Contains(globals.m_graphicsDir) Then
            Return m_mwRaws.Item(fi)
        Else
            Return m_dfRaws.Item(fi)
        End If
    End Function

#End Region



    Public Function saveFile(ByVal fullFilePath As String, ByVal newText As String, Optional ByVal showWarning As Boolean = True) As Boolean
        Try
            If newText = "" And showWarning Then
                MsgBox("The source RAW file is missing. Did you move or replace files in your <dwarf fortress>/raw/objects folder?", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                If Not IO.File.Exists(fullFilePath) Then
                    Throw New System.IO.FileNotFoundException(fullFilePath & " not found.")
                End If
                Dim sWriter As New IO.StreamWriter(fullFilePath)
                sWriter.Write(newText)
                sWriter.Close()
            End If
        Catch ex As Exception
            MsgBox("There has been a problem saving file " & fullFilePath & "." & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End Try
    End Function

    Public Function saveFile(ByVal fi As FileInfo, ByVal newText As String, Optional ByVal showWarning As Boolean = True) As Boolean
        Try
            If newText = "" And showWarning Then
                MsgBox("The source RAW file is missing. Did you move or replace files in your <dwarf fortress>/raw/objects folder?", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            Else
                Dim fs As FileStream = fi.OpenWrite
                Dim data As Byte() = New UTF8Encoding(True).GetBytes(newText)
                fs.Write(data, 0, data.Length)
                fs.Close()
                If fi.FullName.Contains(globals.m_graphicsDir) Then
                    m_mwRaws.Item(fi) = newText
                Else
                    m_dfRaws.Item(fi) = newText
                End If
            End If
        Catch ex As Exception
            MsgBox("There has been a problem saving file " & fi.FullName & "." & vbCrLf & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End Try
    End Function
End Class





Public Class optionManagerConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(optionManager) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is optionManager Then
            Dim om As optionManager = CType(value, optionManager)
            Dim names As List(Of String) = New List(Of String)
            names.Add(IIf(om.loadFromDInit, "d_init", ""))
            names.Add(IIf(om.loadFromInit, "init", ""))
            names.Add(IIf(om.loadFromWorldGen, "world gen", ""))
            Return String.Join(",", names.Where(Function(s) Not String.IsNullOrEmpty(s)))
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class