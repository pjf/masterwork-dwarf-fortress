Imports System.Text.RegularExpressions

Public Class colorViewer
    Inherits Panel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        init()
    End Sub

    Public Sub New(ByVal colorFilePath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_path = colorFilePath
        init()
        refreshColors(m_path)
    End Sub

    Private Sub init()

        Me.ResizeRedraw = True
    End Sub

    Private m_path As String = ""
    Private m_colors As New List(Of Color)
    Private m_displaySize As Size = New Size(30, 30)
    Private m_rows As Integer = 2
    Private m_columns As Integer = 8

    Private m_graphics As System.Drawing.Graphics

    Public Sub refreshColors(ByVal filePath As String)
        If filePath = "" Then Exit Sub

        m_colors.Clear()
        m_path = filePath
        Dim colors As SortedDictionary(Of String, String) = tokenLoading.loadFileTokensToDict(fileWorking.ReadFile(m_path))
        'read in as b,g,r
        For idx As Integer = 0 To colors.Values.Count Step 3
            m_colors.Add(Color.FromArgb(colors.Values(idx + 2), colors.Values(idx + 1), colors.Values(idx)))
        Next


        Me.Refresh()
    End Sub

    Private Sub paintColors()
        If m_colors.Count <= 0 Then Exit Sub

        m_graphics = Me.CreateGraphics
        Dim counter As Integer = 1
        Dim b As New SolidBrush(Color.Black)
        Dim x As Integer = 0
        Dim y As Integer = 0
        For Each c As Color In m_colors
            b.Color = c
            m_graphics.FillRectangle(b, x, y, m_displaySize.Width, m_displaySize.Height)

            If counter = m_columns Then
                y += m_displaySize.Height
                x = 0
            End If
            x += m_displaySize.Width
            counter += 1
        Next
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        paintColors()
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        Me.Size = New Size(m_displaySize.Width * m_columns, m_displaySize.Height * m_rows)
    End Sub
End Class
