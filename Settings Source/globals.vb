Public Class globals

    Public Const m_masterworkRootDir As String = "MasterworkDwarfFortress"
    Public Const m_graphicsDir As String = m_masterworkRootDir + "\graphics"
    Public Const m_utilityDir As String = m_masterworkRootDir + "\Utilities"

    Public Const m_worldGenFileName As String = "world_gen.txt"
    Public Const m_initFileName As String = "init.txt"
    Public Const m_dInitFileName As String = "d_init.txt"

    Public Shared m_dwarfFortressRootDir As String

    'Store contents of text files
    Public Shared m_init As String
    Public Shared m_dinit As String
    Public Shared m_world_gen As String

    Public Shared m_tokensInit As Hashtable
    Public Shared m_tokensDInit As Hashtable
    Public Shared m_tokensWorldGen As New Dictionary(Of Integer, Dictionary(Of String, List(Of String)))

    Public Shared currentWorldGenIndex As Integer

    Public Shared m_dfRaws As New Dictionary(Of IO.FileInfo, String)
    Public Shared m_mwRaws As New Dictionary(Of IO.FileInfo, String)
End Class
