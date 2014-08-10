Public Class colorPreviewer
    Inherits Panel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        init()
    End Sub

    Private Sub init()
        m_pb = New PictureBox()
        m_pb.SizeMode = PictureBoxSizeMode.CenterImage
        Me.Controls.Add(m_pb)
        m_pb.Dock = DockStyle.Fill

        Me.ResizeRedraw = True
    End Sub

    Private m_path As String = ""

    Private m_displaySize As Size = New Size(30, 30)
    Private m_rows As Integer = 2
    Private m_columns As Integer = 8

    Private m_pb As PictureBox    
    Private m_storage As New Dictionary(Of String, Bitmap)

    Public Sub refreshPreview(ByVal filePath As String)
        If m_storage.ContainsKey(filePath) Then
            m_pb.Image = m_storage.Item(filePath)
        Else
            Dim m_colors As New List(Of Color)
            m_path = filePath
            Dim colors As SortedDictionary(Of String, String) = tokenLoading.loadFileTokensToDict(fileWorking.readFile(m_path))
            'read in as b,g,r
            For idx As Integer = 0 To colors.Values.Count Step 3
                m_colors.Add(Color.FromArgb(colors.Values(idx + 2), colors.Values(idx + 1), colors.Values(idx)))
            Next

            If m_colors.Count <= 0 Then Exit Sub

            Dim newImg As New Bitmap(m_displaySize.Width * m_columns, m_displaySize.Height * m_rows)
            Dim m_graphics As System.Drawing.Graphics = Graphics.FromImage(newImg)
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
            m_storage.Add(m_path, newImg)
            m_pb.Image = newImg
        End If

        refreshSize()
        Me.Invalidate()
    End Sub

    Private Sub refreshSize()        
        If m_pb.Image IsNot Nothing Then
            Dim newSize As New Size(m_pb.Image.Width, m_pb.Image.Height)
            If Me.Size <> newSize Then
                Me.Size = newSize
            End If
            Me.Size = New Size(newSize)
        Else
            Me.Size = New Size(30, 30)
        End If
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        refreshSize()
    End Sub
End Class
