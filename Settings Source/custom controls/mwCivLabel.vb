Imports System.ComponentModel

Public Class mwCivLabel
    Inherits Label

    Private m_entityFileName As String
    Private m_creatureFileName As String
    Private m_skillsTag As String
    Private m_playableAdvMode As Boolean
    Private m_playableFortMode As Boolean
    Private m_triggerTag As String
    Private m_faction As Boolean

    <DisplayNameAttribute("Playable Fortress"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This race can be played in fortress mode.")> _
    Public Overridable Property playableFortMode As Boolean
        Get
            Return m_playableFortMode
        End Get
        Set(value As Boolean)
            m_playableFortMode = value
        End Set
    End Property

    <DisplayNameAttribute("Playable Adventure"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("this race can be played in adventure mode.")> _
    Public Overridable Property playableAdvMode As Boolean
        Get
            Return m_playableAdvMode
        End Get
        Set(value As Boolean)
            m_playableAdvMode = value
        End Set
    End Property

    <DisplayNameAttribute("Civ Name"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("Used as part of various tags: X_SIEGE, X_TRADE, X_HOSTILE, X_MATERIALS")> _
    Public Overridable Property simpleCivName As String
        Get
            Return m_triggerTag
        End Get
        Set(value As String)
            m_triggerTag = value
        End Set
    End Property

    <DisplayNameAttribute("Factionable"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("Determines if the civilization can have their faction changed.")> _
    Public Overridable Property factionable As Boolean
        Get
            Return m_faction
        End Get
        Set(value As Boolean)
            m_faction = value
        End Set
    End Property

End Class
