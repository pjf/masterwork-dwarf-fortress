Imports System.ComponentModel

Public Class fileListConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(List(Of String)) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) AndAlso TypeOf value Is String() Then
            Dim fileNames As String() = CType(value, String())
            If fileNames.Count > 0 Then
                Return String.Join(",", fileNames)
            Else
                Return ""
            End If
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
            Dim values As String = CStr(value)
            If values.Length > 0 Then
                Dim fileNames As String() = (From s In values.Split(vbCrLf) Select s).ToArray
                Return fileNames
            Else
                Return New String() {}
            End If
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function
End Class