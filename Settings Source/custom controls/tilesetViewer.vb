Imports System.ComponentModel

Public Class tilesetViewer
    Inherits Panel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        init()
    End Sub

    Public Sub New(ByVal imgPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_path = imgPath
        init()
        refreshPreview(m_path, 0, 0)
    End Sub

    Private Sub init()
        m_pb = New PictureBox()
        m_pb.SizeMode = PictureBoxSizeMode.CenterImage
        Me.Controls.Add(m_pb)
        m_pb.Dock = DockStyle.Fill

        Me.ResizeRedraw = True
    End Sub

    Private m_path As String = ""
    Private m_img As Image
    Private m_pb As PictureBox
    Private m_storage As New Dictionary(Of String, Bitmap)

    Public ReadOnly Property currentPath As String
        Get
            Return m_path
        End Get
    End Property

    Public Sub refreshPreview(ByVal filePath As String, ByVal rowStart As Integer, ByVal rowEnd As Integer)
        If filePath = "" Or filePath = m_path Then Exit Sub

        If m_storage.ContainsKey(filePath) Then
            m_img = m_storage.Item(filePath)
        Else
            m_path = filePath
            Dim original As Bitmap = New Bitmap(m_path)
            Dim rowHeight As Integer = original.Height / 16
            Dim cropHeight As Integer = (rowEnd - rowStart) * rowHeight

            Dim croppedRect As New Rectangle(0, rowHeight * rowStart, original.Width, cropHeight)
            m_img = original.Clone(croppedRect, original.PixelFormat)
            m_storage.Add(filePath, m_img)
        End If

        Console.WriteLine("showing tileset " & filePath)
        m_pb.Image = m_img

        Me.Refresh()
        refreshSize()
    End Sub

    Private Sub refreshSize()
        If m_img IsNot Nothing Then
            Me.Size = New Size(m_img.Width, m_img.Height)
        Else
            Me.Size = New Size(256, 32)
        End If
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        refreshSize()
    End Sub
End Class
