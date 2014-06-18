Imports System.Web.Script.Serialization
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Globalization
Imports System.ComponentModel

Public Class simpleExportObject
    Implements IExtensibleDataObject

    Private m_con As Control
    Private m_tooltip As String

    Public Sub New(ByVal c As Control, ByVal mainTooltip As ToolTip)
        m_con = c
        If mainTooltip.GetToolTip(c) <> "" Then
            m_tooltip = mainTooltip.GetToolTip(c).Replace(vbNewLine, " ").Replace("""", "'").Trim
        Else
            m_tooltip = Nothing
        End If
    End Sub

    Private ReadOnly Property currInfo As iExportInfo
        Get
            Return CType(m_con, iExportInfo)
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return m_con.Name
        End Get
    End Property

    Public ReadOnly Property Text As Object
        Get
            If m_con.Text = "" Then
                Return Nothing
            Else
                If m_con.GetType().BaseType Is GetType(mwCheckBox) Then
                    Return m_con.Text
                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property Tooltip As Object
        Get
            Return m_tooltip
        End Get
    End Property

    Public ReadOnly Property Files As List(Of String)
        Get
            Return currInfo.fileInfo
        End Get
    End Property

    Public ReadOnly Property AffectsGraphicPacks As Boolean
        Get
            Return currInfo.affectsGraphics
        End Get
    End Property

    Public ReadOnly Property HasFileOverrides As Boolean
        Get
            Return currInfo.hasFileOverrides
        End Get
    End Property

    Public ReadOnly Property DropDownItems As comboItemCollection
        Get
            Return currInfo.comboItems
        End Get
    End Property

    Public ReadOnly Property Pattern As optionBasePattern
        Get
            Return currInfo.patternInfo
        End Get
    End Property

    Public ReadOnly Property Tags As rawTokenCollection
        Get
            Dim blnValidTokens As Boolean = False
            If currInfo.tagItems IsNot Nothing Then
                For Each t As rawToken In currInfo.tagItems
                    If t.optionOnValue.ToString <> "" Or t.optionOffValue.ToString <> "" Or t.tokenName <> "" Then
                        blnValidTokens = True : Exit For
                    End If
                Next
            End If
            If blnValidTokens Then
                Return currInfo.tagItems
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property DropDownItemsWithTokens As comboMultiTokenItemCollection
        Get
            If m_con.GetType Is GetType(optionComboBoxMultiToken) Then
                Return CType(m_con, optionComboBoxMultiToken).options.itemList
            End If
            Return Nothing
        End Get
    End Property

    Private extensionData_Value As ExtensionDataObject
    Public Property ExtensionData() As ExtensionDataObject Implements IExtensibleDataObject.ExtensionData
        Get
            Return extensionData_Value
        End Get
        Set(value As ExtensionDataObject)
            extensionData_Value = value
        End Set
    End Property
End Class



