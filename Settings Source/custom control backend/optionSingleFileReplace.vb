Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for a masterwork option which replaces a single file."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionSingleFileReplace

    Public Sub New()
    End Sub

    Protected m_internalChange As Boolean = False

    Private m_fileName As String
    Protected m_settingManager As New optionSettingManager
    Protected m_fileReplaceManager As New fileReplaceManager
    Private m_items As New comboFileItemCollection

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

    <DisplayNameAttribute("File Name"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the target file to be replaced based on the item selected.")> _
    Public Property fileName As String
        Get
            Return m_fileName
        End Get
        Set(value As String)
            m_fileName = value
        End Set
    End Property

    <DescriptionAttribute("These are the possible values this option can be changed to."), _
    EditorAttribute(GetType(comboFileItemCollectionEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property itemList As comboFileItemCollection
        Get
            Return m_items
        End Get
        Set(value As comboFileItemCollection)
            m_items = value
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

    <Browsable(False), _
    EditorBrowsable(EditorBrowsableState.Never), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property fileManager As fileReplaceManager
        Get
            Return m_fileReplaceManager
        End Get
        Set(value As fileReplaceManager)
            m_fileReplaceManager = value
        End Set
    End Property

    Public Sub saveOption(ByVal newFileName As String, Optional ByVal settingValue As String = "")
        If m_fileReplaceManager.replaceFile(m_fileName, newFileName) Then
            If m_settingManager IsNot Nothing Then
                m_settingManager.saveSettingValue(settingValue)
            End If
        End If
    End Sub
End Class
