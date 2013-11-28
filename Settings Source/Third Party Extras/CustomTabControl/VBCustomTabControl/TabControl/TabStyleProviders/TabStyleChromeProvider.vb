'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms
	<System.ComponentModel.ToolboxItem(False)> _
	Public Class TabStyleChromeProvider
		Inherits TabStyleProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
			Me._Overlap = 16
			Me._Radius = 16
			Me._ShowTabCloser = True
			Me._CloserColorActive = Color.White

			'	Must set after the _Radius as this is used in the calculations of the actual padding
			Me.Padding = New Point(7, 5)
		End Sub

		Public Overrides Sub AddTabBorder(path As System.Drawing.Drawing2D.GraphicsPath, tabBounds As System.Drawing.Rectangle)

			Dim spread As Integer
			Dim eigth As Integer
			Dim sixth As Integer
			Dim quarter As Integer

			If Me._TabControl.Alignment <= TabAlignment.Bottom Then
				spread = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Height) * 2 / 3)))
				eigth = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Height) * 1 / 8)))
				sixth = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Height) * 1 / 6)))
				quarter = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Height) * 1 / 4)))
			Else
				spread = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Width) * 2 / 3)))
				eigth = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Width) * 1 / 8)))
				sixth = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Width) * 1 / 6)))
				quarter = CInt(Math.Truncate(Math.Floor(CDec(tabBounds.Width) * 1 / 4)))
			End If

			Select Case Me._TabControl.Alignment
				Case TabAlignment.Top

					path.AddCurve(New Point() {New Point(tabBounds.X, tabBounds.Bottom), New Point(tabBounds.X + sixth, tabBounds.Bottom - eigth), New Point(tabBounds.X + spread - quarter, tabBounds.Y + eigth), New Point(tabBounds.X + spread, tabBounds.Y)})
					path.AddLine(tabBounds.X + spread, tabBounds.Y, tabBounds.Right - spread, tabBounds.Y)
					path.AddCurve(New Point() {New Point(tabBounds.Right - spread, tabBounds.Y), New Point(tabBounds.Right - spread + quarter, tabBounds.Y + eigth), New Point(tabBounds.Right - sixth, tabBounds.Bottom - eigth), New Point(tabBounds.Right, tabBounds.Bottom)})
					Exit Select
				Case TabAlignment.Bottom
					path.AddCurve(New Point() {New Point(tabBounds.Right, tabBounds.Y), New Point(tabBounds.Right - sixth, tabBounds.Y + eigth), New Point(tabBounds.Right - spread + quarter, tabBounds.Bottom - eigth), New Point(tabBounds.Right - spread, tabBounds.Bottom)})
					path.AddLine(tabBounds.Right - spread, tabBounds.Bottom, tabBounds.X + spread, tabBounds.Bottom)
					path.AddCurve(New Point() {New Point(tabBounds.X + spread, tabBounds.Bottom), New Point(tabBounds.X + spread - quarter, tabBounds.Bottom - eigth), New Point(tabBounds.X + sixth, tabBounds.Y + eigth), New Point(tabBounds.X, tabBounds.Y)})
					Exit Select
				Case TabAlignment.Left
					path.AddCurve(New Point() {New Point(tabBounds.Right, tabBounds.Bottom), New Point(tabBounds.Right - eigth, tabBounds.Bottom - sixth), New Point(tabBounds.X + eigth, tabBounds.Bottom - spread + quarter), New Point(tabBounds.X, tabBounds.Bottom - spread)})
					path.AddLine(tabBounds.X, tabBounds.Bottom - spread, tabBounds.X, tabBounds.Y + spread)
					path.AddCurve(New Point() {New Point(tabBounds.X, tabBounds.Y + spread), New Point(tabBounds.X + eigth, tabBounds.Y + spread - quarter), New Point(tabBounds.Right - eigth, tabBounds.Y + sixth), New Point(tabBounds.Right, tabBounds.Y)})

					Exit Select
				Case TabAlignment.Right
					path.AddCurve(New Point() {New Point(tabBounds.X, tabBounds.Y), New Point(tabBounds.X + eigth, tabBounds.Y + sixth), New Point(tabBounds.Right - eigth, tabBounds.Y + spread - quarter), New Point(tabBounds.Right, tabBounds.Y + spread)})
					path.AddLine(tabBounds.Right, tabBounds.Y + spread, tabBounds.Right, tabBounds.Bottom - spread)
					path.AddCurve(New Point() {New Point(tabBounds.Right, tabBounds.Bottom - spread), New Point(tabBounds.Right - eigth, tabBounds.Bottom - spread + quarter), New Point(tabBounds.X + eigth, tabBounds.Bottom - sixth), New Point(tabBounds.X, tabBounds.Bottom)})
					Exit Select
			End Select
		End Sub

		Protected Overrides Sub DrawTabCloser(index As Integer, graphics As Graphics)
			If Me._ShowTabCloser Then
				Dim closerRect As Rectangle = Me._TabControl.GetTabCloserRect(index)
				graphics.SmoothingMode = SmoothingMode.AntiAlias
				If closerRect.Contains(Me._TabControl.MousePosition) Then
					Using closerPath As GraphicsPath = GetCloserButtonPath(closerRect)
						Using closerBrush As New SolidBrush(Color.FromArgb(193, 53, 53))
							graphics.FillPath(closerBrush, closerPath)
						End Using
					End Using
					Using closerPath As GraphicsPath = GetCloserPath(closerRect)
						Using closerPen As New Pen(Me._CloserColorActive)
							graphics.DrawPath(closerPen, closerPath)
						End Using
					End Using
				Else
					Using closerPath As GraphicsPath = GetCloserPath(closerRect)
						Using closerPen As New Pen(Me._CloserColor)
							graphics.DrawPath(closerPen, closerPath)
						End Using
					End Using


				End If
			End If
		End Sub

		Private Shared Function GetCloserButtonPath(closerRect As Rectangle) As GraphicsPath
			Dim closerPath As New GraphicsPath()
			closerPath.AddEllipse(New Rectangle(closerRect.X - 2, closerRect.Y - 2, closerRect.Width + 4, closerRect.Height + 4))
			closerPath.CloseFigure()
			Return closerPath
		End Function
	End Class
End Namespace
