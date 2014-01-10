Imports System.ComponentModel
Imports System.Configuration
Imports MasterworkDwarfFortress.globals

<DisplayNameAttribute("Settings"), _
DescriptionAttribute("This is an optional settings for a variable stored in the app.config of the application. This is primarily for saving/loading certain options where the option is determined by user selection."), _
TypeConverterAttribute(GetType(optionSettingManagerConverter))> _
Public Class optionSettingManager
    Public Sub New()
    End Sub

    Protected m_settingName As String

    <DisplayNameAttribute("Name"), _
    CategoryAttribute("~MASTERWORK"), _
    DescriptionAttribute("This is the name of the setting variable in the app.config file.")> _
    Public Property settingName As String
        Get
            Return m_settingName
        End Get
        Set(value As String)
            m_settingName = value
        End Set
    End Property

    Public Function getSettingValue() As Object
        If Not m_settingName Is Nothing AndAlso m_settingName <> "" Then
            If My.Settings.Properties(m_settingName) IsNot Nothing Then
                Return My.Settings.Item(m_settingName)
            End If
        End If
        Return Nothing
    End Function

    Public Sub saveSettingValue(ByVal value As Object)
        If m_settingName IsNot Nothing AndAlso m_settingName <> "" AndAlso My.Settings.Properties(m_settingName) IsNot Nothing Then
            My.Settings.Item(m_settingName) = value
        End If
    End Sub
End Class

Public Class optionSettingManagerConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(optionSettingManager) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is optionSettingManager Then
            Dim osm As optionSettingManager = CType(value, optionSettingManager)
            Return osm.settingName
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Dim sName As String = CStr(value)
            Dim osm As optionSettingManager = New optionSettingManager
            osm.settingName = sName
            Return osm
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

End Class
