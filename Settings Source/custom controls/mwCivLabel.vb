Imports System.ComponentModel

Public Class mwCivLabel
    Inherits Label

    Private m_entityFileName As String
    Private m_creatureFileName As String
    Private m_skillsTag As String
    Private m_advTag As String
    Private m_fortTag As String
    Private m_triggerTag As String

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

    <DisplayNameAttribute("Fortress Tag"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The tag identifier to play this civilization in fortress mode.")> _
    Public Overridable Property fortTag As String
        Get
            Return m_fortTag
        End Get
        Set(value As String)
            m_fortTag = value
        End Set
    End Property

    <DisplayNameAttribute("Adventurer Tag"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("The tag identifier to play this civilization in adventurer mode.")> _
    Public Overridable Property advTag As String
        Get
            Return m_advTag
        End Get
        Set(value As String)
            m_advTag = value
        End Set
    End Property

    <DisplayNameAttribute("Generic Tag"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("Used as part of various tags: X_SIEGE, X_TRADE, X_HOSTILE, X_MATERIALS")> _
    Public Overridable Property triggerTag As String
        Get
            Return m_triggerTag
        End Get
        Set(value As String)
            m_triggerTag = value
        End Set
    End Property

End Class
