Public Interface iToken
    Sub loadOption(Optional ByVal value As Object = Nothing)
    Sub saveOption()
    Function currentValue() As Object
End Interface

Public Interface iEnabled
    Property isEnabled() As Boolean
End Interface