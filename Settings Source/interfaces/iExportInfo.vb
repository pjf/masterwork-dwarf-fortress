Public Interface iExportInfo
    Function fileInfo() As List(Of String)
    Function tagItems() As rawTokenCollection
    Function comboItems() As comboItemCollection
    Function patternInfo() As optionPattern
    Function hasFileOverrides() As Boolean
    Function affectsGraphics() As Boolean
End Interface
