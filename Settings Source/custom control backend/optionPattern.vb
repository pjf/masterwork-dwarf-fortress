Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for a single masterwork option."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionPattern
    Inherits optionBase

    Private m_pattern As New optionBasePattern

    Public Sub New()
        MyBase.New()  
    End Sub

    <DisplayNameAttribute("Pattern"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the specific pattern this option controls.")> _
    Public Overridable Property Pattern As optionBasePattern
        Get
            Return m_pattern
        End Get
        Set(value As optionBasePattern)
            m_pattern = value
        End Set
    End Property

End Class
