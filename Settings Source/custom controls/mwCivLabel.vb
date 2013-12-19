Imports System.ComponentModel

Public Class mwCivLabel
    Inherits Label

    Private m_entityFileName As String
    Private m_creatureFileName As String
    Private m_skillsTag As String
    Private m_materialsTag As String

    <DisplayNameAttribute("Entity file"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The entity file name.")> _
    Public Overridable Property entityFileName As String
        Get
            Return m_entityFileName
        End Get
        Set(value As String)
            m_entityFileName = value
        End Set
    End Property

    <DisplayNameAttribute("Creature file"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The creature file name.")> _
    Public Overridable Property creatureFileName As String
        Get
            Return m_creatureFileName
        End Get
        Set(value As String)
            m_creatureFileName = value
        End Set
    End Property

    <DisplayNameAttribute("Skills Tag"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The tag identifier for this civilization's creatures' skills.")> _
    Public Overridable Property skillsTag As String
        Get
            Return m_skillsTag
        End Get
        Set(value As String)
            m_skillsTag = value
        End Set
    End Property

    <DisplayNameAttribute("Materials Tag"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The tag identifier for this civilization's creatures' materials.")> _
    Public Overridable Property materialsTag As String
        Get
            Return m_materialsTag
        End Get
        Set(value As String)
            m_materialsTag = value
        End Set
    End Property

End Class
