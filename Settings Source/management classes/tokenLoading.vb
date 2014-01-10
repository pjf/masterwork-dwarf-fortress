Imports MasterworkDwarfFortress.globals
Imports System.Text.RegularExpressions

Module tokenLoading

    Public Function loadFileTokens(ByVal fileText As String) As Hashtable
        Dim pattern As String = "\[(?<token>\w+):(?<value>.*)\]" 'find anything with a [token:value] pattern and stores content in "token" and "value" groups
        Dim matches As MatchCollection = Regex.Matches(fileText, pattern) 'Do the search with above pattern. Creates a collection of matches
        'Dim dict As New Dictionary(Of String, List(Of String))
        Dim h As New Hashtable

        'Iterates through all matches and stores data from them in a 2d array {{token, value}, {token, value}, {token, value}, ...
        For Each match As Match In matches
            Dim token As String = match.Groups("token").Value 'gets the current matches token/value groups (defined in "pattern") 
            Dim value As String = match.Groups("value").Value
            If token <> "" Then 'If token not empty store them into array
                h.Add(token, value)
            End If
        Next
        If h.Count < 2 Then
            MsgBox("Problem loading options: Few or no tokens found in a file!")
        End If
        Return h
    End Function

    Public Function loadFileTokensToDict(ByVal fileText As String) As SortedDictionary(Of String, String)
        Dim pattern As String = "\[(?<token>\w+):(?<value>.*)\]" 'find anything with a [token:value] pattern and stores content in "token" and "value" groups
        Dim matches As MatchCollection = Regex.Matches(fileText, pattern) 'Do the search with above pattern. Creates a collection of matches
        'Dim dict As New Dictionary(Of String, List(Of String))
        Dim d As New SortedDictionary(Of String, String)

        'Iterates through all matches and stores data from them in a 2d array {{token, value}, {token, value}, {token, value}, ...
        For Each match As Match In matches
            Dim token As String = match.Groups("token").Value 'gets the current matches token/value groups (defined in "pattern") 
            Dim value As String = match.Groups("value").Value
            If token <> "" Then 'If token not empty store them into array
                d.Add(token, value)
            End If
        Next
        If d.Count < 2 Then
            MsgBox("Problem loading options: Few or no tokens found in a file!")
        End If
        Return d
    End Function

    Public Sub loadWorldGenTokens()
        m_tokensWorldGen.Clear()
        Dim pattern As String = "\[(?<token>\w+):(?<value>.*)\]" 'find anything with a [token:value] pattern and stores content in "token" and "value" groups
        Dim matches As MatchCollection = Regex.Matches(m_world_gen, pattern) 'Do the search with above pattern. Creates a collection of matches

        'Iterates through all matches and stores data from them in a 2d array {{token, value}, {token, value}, {token, value}, ...
        Dim idx As Integer = 0
        Dim tokens As New Dictionary(Of String, List(Of String))

        For Each match As Match In matches
            Dim token As String = match.Groups("token").Value 'gets the current matches token/value groups (defined in "pattern") 
            Dim value As String = match.Groups("value").Value

            If token <> "" Then
                If tokens.Count > 0 AndAlso token = "TITLE" Then
                    m_tokensWorldGen.Add(idx, tokens)
                    tokens = New Dictionary(Of String, List(Of String))
                    idx += 1
                End If

                Dim values As List(Of String) = Nothing
                If Not tokens.TryGetValue(token, values) Then
                    values = New List(Of String)
                    tokens.Add(token, values)
                End If
                tokens(token).Add(value)
            End If
        Next
        If tokens.Count > 0 Then
            m_tokensWorldGen.Add(idx, tokens)
        End If
        If m_tokensWorldGen.Count <= 0 Then
            MsgBox("No world gen tokens found!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End If
    End Sub

End Module
