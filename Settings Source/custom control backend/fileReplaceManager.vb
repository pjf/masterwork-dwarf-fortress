Imports System.ComponentModel
Imports MasterworkDwarfFortress.fileWorking
Imports System.Text.RegularExpressions

Public Class fileReplaceManager
    Public Sub New()
    End Sub

    'it's expected that we're replacing files from the MasterworkDwarfFortress folder to the actual game folder
    Public Function replaceFile(ByVal baseFileName As String, ByVal newFileName As String) As Boolean
        Try
            Dim strOriginalPath As String = findDfFilePath(baseFileName)
            Dim newFilePath As String = findMwFilePath(newFileName)
            If strOriginalPath <> "" And newFilePath <> "" Then
                IO.File.Copy(newFilePath, strOriginalPath, True)
                Return True
            Else
                Throw New Exception("File not found.")
            End If

        Catch ex As Exception
            MsgBox("Failed to replace " & baseFileName & " with " & newFileName, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return False
        End Try
    End Function


End Class
