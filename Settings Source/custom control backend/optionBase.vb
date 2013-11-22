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
    Protected m_updateTileSets As Boolean

    <DisplayNameAttribute("Update Tilesets"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This token should also be updated in graphics pack raw files.")> _
    Public Overridable Property updateTileSets As Boolean
        Get
            Return m_updateTileSets
        End Get
        Set(value As Boolean)
            m_updateTileSets = value
        End Set
    End Property

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
        Return m_optionManager.loadOption(m_fileManager.getFilePaths, m_tokens, m_settingManager)
    End Function

    'Public Function saveOption(ByVal newValue As String) As Boolean
    '    Return m_optionManager.saveOption(m_fileManager, m_tokens, newValue, m_updateTileSets)
    'End Function

    Public Function saveOption(Optional ByVal enable As Boolean = False) As Boolean
        Return m_optionManager.saveOption(m_fileManager, m_tokens, enable, m_updateTileSets)
    End Function

    Public Sub saveSetting(ByVal newValue As String)
        If m_settingManager IsNot Nothing Then
            m_settingManager.saveSettingValue(newValue)
        End If
    End Sub

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Never), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property valueUpdatingPaused() As Boolean
        Get
            Return m_internalChange
        End Get
        Set(value As Boolean)
            m_internalChange = value
        End Set
    End Property
End Class
