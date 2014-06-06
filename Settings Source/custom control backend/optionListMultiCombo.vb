Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for multiple tokens across one or more files."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionListMultiCombo
    Inherits optionBase

    Public Sub New()
        MyBase.New()
    End Sub

    Private m_items As New comboMultiTokenItemCollection

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("These are the possible values this option can be changed to, and the associated tokens each option changes."), _
    EditorAttribute(GetType(comboMultiTokenItemCollectionEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property itemList As comboMultiTokenItemCollection
        Get
            Return m_items
        End Get
        Set(value As comboMultiTokenItemCollection)
            m_items = value
        End Set
    End Property

    Public Sub setCurrentTokens(ByVal item As comboMultiTokenItem)
        m_tokens = item.tokens
    End Sub
End Class
