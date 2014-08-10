Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for a single masterwork option which toggles between a specified enabled and disabled token."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionSingleReplace
    Inherits optionBase    

    Public Sub New()
        MyBase.New()
        'for a single option, ensure we always have a token to work with
        Dim defaultTag As New rawToken
        defaultTag.tokenName = ""
        m_tokens.Add(defaultTag)
    End Sub

    <DisplayNameAttribute("Enabled"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the value to place in the files specified when ENABLED.")> _
    Public Overridable Property enabledValue As String
        Get
            Return m_tokens.Item(0).optionOnValue
        End Get
        Set(value As String)
            m_tokens.Item(0).optionOnValue = value
        End Set
    End Property

    <DisplayNameAttribute("Disabled"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the value to place in the files specified when DISABLED.")> _
    Public Overridable Property disabledValue As String
        Get
            Return m_tokens.Item(0).optionOffValue
        End Get
        Set(value As String)
            m_tokens.Item(0).optionOffValue = value
        End Set
    End Property

    <DisplayNameAttribute("Token"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the token name.")> _
    Public Overridable Property tokenName As String
        Get
            Return m_tokens.Item(0).tokenName
        End Get
        Set(value As String)
            m_tokens.Item(0).tokenName = value
        End Set
    End Property

End Class

