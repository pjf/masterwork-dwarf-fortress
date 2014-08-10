Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for multiple tokens across one or more files."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionListMulti
    Inherits optionMulti

    Public Sub New()
        MyBase.New()
    End Sub

    Private m_items As New comboItemCollection

    <CategoryAttribute("~RAW Options"), _
    DescriptionAttribute("These are the possible values this option can be changed to."), _
    EditorAttribute(GetType(comboItemCollectionEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Overridable Property itemList As comboItemCollection
        Get
            Return m_items
        End Get
        Set(value As comboItemCollection)
            m_items = value
        End Set
    End Property

End Class
