Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for a single masterwork option."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionBase

    Public Sub New()
        m_tokens = New rawTokenCollection
    End Sub

    Protected m_tokens As New rawTokenCollection    
    Protected m_internalChange As Boolean = False

    Protected m_optionManager As New optionManager
    Protected m_fileManager As New fileListManager
    Protected m_settingManager As New optionSettingManager

    Public Property settingManager As optionSettingManager
        Get
            Return m_settingManager
        End Get
        Set(value As optionSettingManager)
            m_settingManager = value
        End Set
    End Property

    Public Property optionManager As optionManager
        Get
            Return m_optionManager
        End Get
        Set(value As optionManager)
            m_optionManager = value
        End Set
    End Property

    Public Property fileManager As fileListManager
        Get
            Return m_fileManager
        End Get
        Set(value As fileListManager)
            m_fileManager = value
        End Set
    End Property

    Public Function loadOption() As Object
        'only load settings from our current raws
        Return m_optionManager.loadOption(m_fileManager.loadFiles(m_optionManager, m_tokens), m_tokens, m_settingManager)
    End Function
    Public Function loadOption(ByVal tokens As rawTokenCollection)
        Return m_optionManager.loadOption(m_fileManager.loadFiles(m_optionManager, m_tokens), tokens, m_settingManager)
    End Function

    Public Function saveOption(Optional ByVal enable As Boolean = False) As Boolean
        Return m_optionManager.saveOption(m_fileManager, m_tokens, enable)
    End Function

    Public Function saveOption(ByVal tokens As rawTokenCollection, Optional ByVal enable As Boolean = False) As Boolean
        Return m_optionManager.saveOption(m_fileManager, tokens, enable)
    End Function

    Public Sub saveSetting(ByVal newValue As String)
        If m_settingManager IsNot Nothing Then
            m_settingManager.saveSettingValue(newValue)
        End If
    End Sub

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property valueUpdatingPaused() As Boolean
        Get
            Return m_internalChange
        End Get
        Set(value As Boolean)
            m_internalChange = value
        End Set
    End Property

#Region "option export info"

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property optionTags As rawTokenCollection
        Get
            Return m_tokens
        End Get
    End Property

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Advanced), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property fullFileList As List(Of String)
        Get
            Dim files As New List(Of String)
            If m_optionManager.loadFromDInit Then files.Add(m_dInitFileName)
            If m_optionManager.loadFromInit Then files.Add(m_initFileName)
            If m_optionManager.loadFromWorldGen Then files.Add(m_worldGenFileName)

            If m_fileManager.fileNames IsNot Nothing Then
                For Each name As String In m_fileManager.fileNames
                    If Not files.Contains(name) Then files.Add(name)
                Next
            End If

            Return files
        End Get
    End Property

#End Region


End Class
