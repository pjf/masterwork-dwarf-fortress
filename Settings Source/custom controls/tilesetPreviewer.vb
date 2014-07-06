Imports System.ComponentModel

Public Class tilesetPreviewer
    Inherits Panel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        init()
    End Sub

    'Public Sub New(ByVal imgPath As String)

    '    ' This call is required by the designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.
    '    m_path = imgPath
    '    init()
    '    refreshPreview(m_path, 0, 0)
    'End Sub

    Private Sub init()
        m_pb = New PictureBox()
        m_pb.SizeMode = PictureBoxSizeMode.CenterImage
        Me.Controls.Add(m_pb)
        m_pb.Dock = DockStyle.Fill

        Me.ResizeRedraw = True
    End Sub

    Private m_currentKey As String    
    Private m_pb As PictureBox
    Private m_storage As New Dictionary(Of String, Bitmap)

    Public Sub refreshPreview(ByVal key As String, ByVal filePath As String, ByVal rowStart As Integer, ByVal rowEnd As Integer)
        'If filePath = "" Or key = m_currentKey Then Exit Sub

        'If m_storage.ContainsKey(key) Then
        '    m_pb.Image = m_storage.Item(key)
        'Else
        '    m_pb.Image = loadImage(key, filePath, rowStart, rowEnd)
        'End If

        'm_currentKey = key
        m_pb.Image = loadImage(key, filePath, rowStart, rowEnd)

        Me.Refresh()
        refreshSize()
    End Sub

    Public Function loadImage(ByVal key As String, ByVal filePath As String, ByVal rowStart As Integer, ByVal rowEnd As Integer) As Bitmap
        Dim original As Bitmap = New Bitmap(filePath)
        Dim newImg As Bitmap
        Dim rowHeight As Integer = original.Height / 16
        Dim cropHeight As Integer = (rowEnd - rowStart) * rowHeight

        Dim croppedRect As New Rectangle(0, rowHeight * rowStart, original.Width, cropHeight)
        newImg = original.Clone(croppedRect, original.PixelFormat)
        'm_storage.Add(key, newImg)
        Return newImg
    End Function

    Private Sub refreshSize()
        If m_pb.Image IsNot Nothing Then
            Me.Size = New Size(m_pb.Image.Width, m_pb.Image.Height)
        Else
            Me.Size = New Size(256, 32)
        End If
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        refreshSize()
    End Sub
End Class
