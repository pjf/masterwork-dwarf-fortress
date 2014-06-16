Imports System.ComponentModel.Design
Imports System.ComponentModel

'<DisplayNameAttribute("Masterwork Settings"), _
'DescriptionAttribute("Settings for multiple tokens across one or more files."), _
'CategoryAttribute("~MASTERWORK"), _
'TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class comboMultiTokenItem
    Inherits comboItem

    Public Sub New()
    End Sub

    Public Sub New(ByVal value As String, ByVal display As String)
        MyBase.New(value, display)
    End Sub

    Private m_tokens As New rawTokenCollection

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("These are the raw token(s) this option controls."), _
    EditorAttribute(GetType(rawTokenCollectionEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property tokens As rawTokenCollection
        Get
            Return m_tokens
        End Get
        Set(value As rawTokenCollection)
            m_tokens = value
        End Set
    End Property

End Class

Public Class comboMultiTokenItemCollection
    Inherits CollectionBase

    Public Sub New()
    End Sub

    Public ReadOnly Property Item(index As Integer) As comboMultiTokenItem
        Get
            Return DirectCast(List(index), comboMultiTokenItem)
        End Get
    End Property

    Public Sub Add(ByVal t As comboMultiTokenItem)
        List.Add(t)
    End Sub

    Public Sub Remove(ByVal t As comboMultiTokenItem)
        List.Remove(t)
    End Sub

    Public Sub Replace(ByVal t As comboItem, ByVal index As Integer)
        If List.Count > 0 And index < List.Count - 1 Then
            List.RemoveAt(index)
            List.Insert(index, t)
        End If
    End Sub

End Class

Public Class comboMultiTokenItemCollectionEditor
    Inherits CollectionEditor

    Public Sub New()
        MyBase.New(GetType(comboMultiTokenItemCollection))
    End Sub
End Class