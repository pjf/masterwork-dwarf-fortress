Imports System.ComponentModel
Imports System.ComponentModel.Design

Public Class comboFileItem
    Inherits comboItem

    Private m_fileName As String
    Private m_filePath As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal value As String, ByVal display As String, ByVal fileName As String, ByVal filePath As String)
        MyBase.New(value, display)
        m_fileName = fileName
        m_filePath = filePath
    End Sub

    <CategoryAttribute("Data"),
    DisplayNameAttribute("File Name"),
    DescriptionAttribute("The file this item represents.")> _
    Public Property fileName As String
        Get
            Return m_fileName
        End Get
        Set(value As String)
            m_fileName = value
        End Set
    End Property
    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property filePath As String
        Get
            Return m_filePath
        End Get
        Set(value As String)
            m_filePath = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & " " & m_fileName
    End Function

End Class

Public Class comboFileItemCollection
    Inherits CollectionBase

    Public ReadOnly Property Item(index As Integer) As comboFileItem
        Get
            Return DirectCast(List(index), comboFileItem)
        End Get
    End Property

    Public Sub Add(ByVal t As comboFileItem)
        List.Add(t)
    End Sub

    Public Sub Remove(ByVal t As comboFileItem)
        List.Remove(t)
    End Sub

    Public Sub Replace(ByVal t As comboFileItem, ByVal index As Integer)
        If List.Count > 0 And index < List.Count - 1 Then
            List.RemoveAt(index)
            List.Insert(index, t)
        End If
    End Sub
End Class

Public Class comboFileItemCollectionEditor
    Inherits CollectionEditor

    Public Sub New()
        MyBase.New(GetType(comboFileItemCollection))
    End Sub
End Class

