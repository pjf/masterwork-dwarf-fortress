Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for a single masterwork option."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionSingle
    Inherits optionBase

    Public Sub New()
        MyBase.New()
        'for a single option, ensure we always have a token to work with
        Dim defaultTag As New rawToken
        defaultTag.tokenName = ""
        m_tokens.Add(defaultTag)
    End Sub

    <DisplayNameAttribute("Token"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the specific token this option controls.")> _
    Public Overridable Property tokenName As String
        Get
            Return m_tokens.Item(0).tokenName
        End Get
        Set(value As String)
            m_tokens.Item(0).tokenName = value
        End Set
    End Property

    Public Sub valueChanged(ByVal newValue As String)
        m_tokens.Item(0).optionOnValue = newValue
    End Sub

End Class
