Public Class graphicPackDefinition
    Public Sub New()
    End Sub

    Private m_colorScheme As String
    Private m_type As String
    Private m_name As String
    Private m_tilesetImagePath As String

    Public Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Public Property tilesetType As String
        Get
            Return m_type
        End Get
        Set(value As String)
            m_type = value
        End Set
    End Property

    Public Property colorScheme As String
        Get
            Return m_colorScheme
        End Get
        Set(value As String)
            m_colorScheme = value
        End Set
    End Property

    Public ReadOnly Property tilesetPath As String
        Get
            If m_tilesetImagePath = "" Then
                Dim fi As IO.FileInfo = graphicsSets.findGraphicsPackTilesetImage(m_name)
                If fi IsNot Nothing Then
                    m_tilesetImagePath = fi.FullName
                End If
            End If
            Return m_tilesetImagePath
        End Get
    End Property

End Class

Public Class graphicPackDefinitionTilesetTypeComparer
    Implements IEqualityComparer(Of graphicPackDefinition)

    Public Function Equals1(ByVal x As graphicPackDefinition, ByVal y As graphicPackDefinition) As Boolean Implements IEqualityComparer(Of graphicPackDefinition).Equals
        If x Is y Then Return True
        If x Is Nothing OrElse y Is Nothing Then Return False
        Return (x.tilesetType = y.tilesetType)
    End Function

    Public Function GetHashCode1(ByVal gpd As graphicPackDefinition) As Integer Implements IEqualityComparer(Of graphicPackDefinition).GetHashCode
        If gpd Is Nothing Then Return 0
        Return If(gpd.tilesetType Is Nothing, 0, gpd.tilesetType.GetHashCode())
    End Function
End Class