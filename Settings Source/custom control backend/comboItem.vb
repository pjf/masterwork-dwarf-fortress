Imports System.ComponentModel.Design
Imports System.ComponentModel

Public Class comboItem
    Private m_value As String = ""
    Private m_text As String = ""

    Public Sub New()
    End Sub

    Public Sub New(ByVal value As String, ByVal display As String)
        m_value = value
        m_text = display
    End Sub

    Public Overrides Function ToString() As String
        Return m_value & " " & m_text
    End Function

    <CategoryAttribute("Data"),
    DisplayNameAttribute("Value"),
    DescriptionAttribute("The list item's value.")> _
    Public Property value As String
        Get
            Return m_value
        End Get
        Set(value As String)
            m_value = value
        End Set
    End Property

    <CategoryAttribute("Data"),
    DisplayNameAttribute("Display"),
    DescriptionAttribute("The list item's displayed text.")> _
    Public Property display As String
        Get
            Return m_text
        End Get
        Set(value As String)
            m_text = value
        End Set
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType Is obj.GetType Then
            Return False
        End If
        Return Me.value = CType(obj, comboItem).value
    End Function

End Class

Public Class comboItemCollection
    Inherits CollectionBase

    Public ReadOnly Property Item(index As Integer) As comboItem
        Get
            Return DirectCast(List(index), comboItem)
        End Get
    End Property

    Public Sub Add(ByVal t As comboItem)
        List.Add(t)
    End Sub

    Public Sub Remove(ByVal t As comboItem)
        List.Remove(t)
    End Sub

    Public Sub Replace(ByVal t As comboItem, ByVal index As Integer)
        If List.Count > 0 And index < List.Count - 1 Then
            List.RemoveAt(index)
            List.Insert(index, t)
        End If
    End Sub

    'Public ReadOnly Property valueList As List(Of String)
    '    Get
    '        Dim result As New List(Of String)
    '        For Each i As comboItem In List
    '            result.Add(i.value)
    '        Next
    '        Return result
    '    End Get
    'End Property
End Class

Public Class comboItemCollectionEditor
    Inherits CollectionEditor

    Public Sub New()
        MyBase.New(GetType(comboItemCollection))
    End Sub
End Class