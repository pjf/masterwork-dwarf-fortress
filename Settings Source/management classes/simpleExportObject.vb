Public Class simpleExportObject

    Private m_con As Control
    Private m_tooltip As String

    Public Structure structPattern
        Public findPattern As String
        Public replacePattern As String
    End Structure
    Private m_pattern As structPattern

    Public Sub New(ByVal c As Control, ByVal mainTooltip As ToolTip)
        m_con = c
        If mainTooltip.GetToolTip(c) <> "" Then
            m_tooltip = mainTooltip.GetToolTip(c).Replace(vbNewLine, " ").Replace("""", "'").Trim
        Else
            m_tooltip = ""
        End If
        m_pattern.findPattern = CType(m_con, iExportInfo).patternInfo.Key.ToString
        m_pattern.replacePattern = CType(m_con, iExportInfo).patternInfo.Value.ToString
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return m_con.Name
        End Get
    End Property

    Public ReadOnly Property Text As String
        Get
            Return m_con.Text
        End Get
    End Property

    Public ReadOnly Property Tooltip As String
        Get
            Return m_tooltip
        End Get
    End Property

    Public ReadOnly Property Files As List(Of String)
        Get
            Return CType(m_con, iExportInfo).fileInfo
        End Get
    End Property

    Public ReadOnly Property AffectsGraphicPacks As Boolean
        Get
            Return CType(m_con, iExportInfo).affectsGraphics
        End Get
    End Property

    Public ReadOnly Property HasFileOverrides As Boolean
        Get
            Return CType(m_con, iExportInfo).hasFileOverrides
        End Get
    End Property

    Public ReadOnly Property DropDownItems As comboItemCollection
        Get
            Return CType(m_con, iExportInfo).comboItems
        End Get
    End Property

    Public ReadOnly Property Pattern As structPattern
        Get
            Return m_pattern
        End Get
    End Property

End Class
