Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Masterwork Settings"), _
DescriptionAttribute("Settings for multiple tokens across one or more files."), _
CategoryAttribute("~MASTERWORK"), _
TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class optionMulti
    Inherits optionBase    

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

End Class
