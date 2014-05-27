Public Interface iExportInfo
    Function fileInfo() As List(Of String)
    Function tagItems() As rawTokenCollection
    Function comboItems() As comboItemCollection
    Function patternInfo() As KeyValuePair(Of String, String)
    Function hasFileOverrides() As Boolean
    Function affectsGraphics() As Boolean
End Interface
