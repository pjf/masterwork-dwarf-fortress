Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for multiple tokens across one or more files."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionMulti
    Inherits optionBase
    Implements iExportInfo

    Public Sub New()
        MyBase.New()
    End Sub

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("These are the raw token(s) this option controls."), _
    EditorAttribute(GetType(rawTokenCollectionEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property tokenList As rawTokenCollection
        Get
            Return m_tokens
        End Get
        Set(value As rawTokenCollection)
            m_tokens = value
        End Set
    End Property

    Public Function fileInfo() As List(Of String) Implements iExportInfo.fileInfo
        Return fullFileList
    End Function

    Public Function comboItems() As comboItemCollection Implements iExportInfo.comboItems
        Return Nothing
    End Function

    Public Function tagItems() As rawTokenCollection Implements iExportInfo.tagItems
        Return optionTags
    End Function

    Public Function hasFileOverrides() As Boolean Implements iExportInfo.hasFileOverrides
        Return fileManager.isOverriden
    End Function

    Public Function patternInfo() As KeyValuePair(Of String, String) Implements iExportInfo.patternInfo
        Return New KeyValuePair(Of String, String)("", "")
    End Function

    Public Function affectsGraphics() As Boolean Implements iExportInfo.affectsGraphics
        Return fileManager.affectsGraphics
    End Function
End Class
