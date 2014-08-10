Imports System.ComponentModel
Imports System.ComponentModel.Design

Public Class rawToken
    Private m_optionOnValue As String = ""
    Private m_optionOffValue As String = ""
    Private m_tokenName As String = ""

    Public Sub New()
    End Sub

    Public Sub New(ByVal name As String, ByVal optionOn As String, ByVal optionOff As String)
        m_tokenName = name
        m_optionOnValue = optionOn
        m_optionOffValue = optionOff
    End Sub

    Public Overrides Function ToString() As String
        Return m_tokenName & " ON:" & m_optionOnValue & " OFF:" & m_optionOffValue
    End Function

    <CategoryAttribute("Data"),
    DisplayNameAttribute("Token"),
    DescriptionAttribute("The name of the token. Not required if performing replacement.")> _
    Public Property tokenName As String
        Get
            Return m_tokenName
        End Get
        Set(value As String)
            m_tokenName = value
        End Set
    End Property

    <CategoryAttribute("Data"),
    DisplayNameAttribute("Enabled Value"),
    DescriptionAttribute("This is the value to use in the files when this option is ENABLED.")> _
    Public Property optionOnValue As String
        Get
            Return m_optionOnValue
        End Get
        Set(value As String)
            m_optionOnValue = value
        End Set
    End Property

    <CategoryAttribute("Data"),
    DisplayNameAttribute("Disabled Value"),
    DescriptionAttribute("This is the value to use in the files when this option is DISABLED.")> _
    Public Property optionOffValue As String
        Get
            Return m_optionOffValue
        End Get
        Set(value As String)
            m_optionOffValue = value
        End Set
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property isMultiLine As Boolean
        Get
            Return (m_optionOnValue.Contains(Environment.NewLine) OrElse m_optionOffValue.Contains(Environment.NewLine))
        End Get
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Function getMultilinePattern(ByVal onTokenValue As Boolean) As String
        If onTokenValue Then
            Return m_optionOnValue.Replace(Environment.NewLine, "(\n|\r|\r\n)")
        Else
            Return m_optionOffValue.Replace(Environment.NewLine, "(\n|\r|\r\n)")
        End If
    End Function
    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Function getMultilinePattern(ByVal value As String) As String
        Return value.Replace(Environment.NewLine, "(\n|\r|\r\n)")
    End Function

End Class

Public Class rawTokenCollection
    Inherits CollectionBase

    Public ReadOnly Property Item(index As Integer) As rawToken
        Get
            Return DirectCast(List(index), rawToken)
        End Get
    End Property

    Public Sub Add(ByVal t As rawToken)
        List.Add(t)
    End Sub

    Public Sub Remove(ByVal t As rawToken)
        List.Remove(t)
    End Sub

    Public Sub Replace(ByVal t As rawToken, ByVal index As Integer)
        If List.Count > 0 And index < List.Count - 1 Then
            List.RemoveAt(index)
            List.Insert(index, t)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim strTokens As New List(Of String)
        For Each t As rawToken In List
            strTokens.Add(t.ToString)
        Next
        Return String.Join(vbCrLf, strTokens)
    End Function
End Class

Public Class rawTokenCollectionEditor
    Inherits CollectionEditor

    Public Sub New()
        MyBase.New(GetType(rawTokenCollection))
    End Sub
End Class
