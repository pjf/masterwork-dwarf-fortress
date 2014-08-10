Imports System.ComponentModel

<DisplayNameAttribute("Pattern"), _
DescriptionAttribute("Find and replace pattern."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionBasePattern
    Private m_pattern As String
    Private m_replace As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal strFind As String, ByVal strReplace As String)
        m_pattern = strFind
        m_replace = strReplace
    End Sub
    <DisplayNameAttribute("Find Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This pattern is used to update a specific value. Must contain ?<value> to indicate where the value should be replaced in the pattern.")> _
    Public Property find As String
        Get
            Return m_pattern
        End Get
        Set(value As String)
            m_pattern = value
        End Set
    End Property
    <DisplayNameAttribute("Replace Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This will be used as the replacement pattern. Must contain ${value} to indicate where the new value should be placed.")> _
    Public Property replace As String
        Get
            Return m_replace
        End Get
        Set(value As String)
            m_replace = value
        End Set
    End Property
    Public Overrides Function ToString() As String
        If Not m_pattern = "" Or Not m_replace = "" Then
            Return "Find:" & m_pattern & " Replace:" & m_replace
        Else
            Return ""
        End If
    End Function
End Class
