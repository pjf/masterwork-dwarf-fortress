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
	Public Class TabStyleDefaultProvider
		Inherits TabStyleProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
			Me._FocusTrack = True
			Me._Radius = 2
		End Sub

		Public Overrides Sub AddTabBorder(path As System.Drawing.Drawing2D.GraphicsPath, tabBounds As System.Drawing.Rectangle)
			Select Case Me._TabControl.Alignment
				Case TabAlignment.Top
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y)
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y)
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom)
					Exit Select
				Case TabAlignment.Bottom
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom)
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom)
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y)
					Exit Select
				Case TabAlignment.Left
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom)
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y)
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y)
					Exit Select
				Case TabAlignment.Right
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y)
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom)
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom)
					Exit Select
			End Select
		End Sub

		Public Overrides Function GetTabRect(index As Integer) As Rectangle
			If index < 0 Then
				Return New Rectangle()
			End If

			Dim tabBounds As Rectangle = MyBase.GetTabRect(index)
			Dim firstTabinRow As Boolean = Me._TabControl.IsFirstTabInRow(index)

			'	Make non-SelectedTabs smaller and selected tab bigger
			If index <> Me._TabControl.SelectedIndex Then
				Select Case Me._TabControl.Alignment
					Case TabAlignment.Top
						tabBounds.Y += 1
						tabBounds.Height -= 1
						Exit Select
					Case TabAlignment.Bottom
						tabBounds.Height -= 1
						Exit Select
					Case TabAlignment.Left
						tabBounds.X += 1
						tabBounds.Width -= 1
						Exit Select
					Case TabAlignment.Right
						tabBounds.Width -= 1
						Exit Select
				End Select
			Else
				Select Case Me._TabControl.Alignment
					Case TabAlignment.Top
						If tabBounds.Y > 0 Then
							tabBounds.Y -= 1
							tabBounds.Height += 1
						End If

						If firstTabinRow Then
							tabBounds.Width += 1
						Else
							tabBounds.X -= 1
							tabBounds.Width += 2
						End If
						Exit Select
					Case TabAlignment.Bottom
						If tabBounds.Bottom < Me._TabControl.Bottom Then
							tabBounds.Height += 1
						End If
						If firstTabinRow Then
							tabBounds.Width += 1
						Else
							tabBounds.X -= 1
							tabBounds.Width += 2
						End If
						Exit Select
					Case TabAlignment.Left
						If tabBounds.X > 0 Then
							tabBounds.X -= 1
							tabBounds.Width += 1
						End If

						If firstTabinRow Then
							tabBounds.Height += 1
						Else
							tabBounds.Y -= 1
							tabBounds.Height += 2
						End If
						Exit Select
					Case TabAlignment.Right
						If tabBounds.Right < Me._TabControl.Right Then
							tabBounds.Width += 1
						End If
						If firstTabinRow Then
							tabBounds.Height += 1
						Else
							tabBounds.Y -= 1
							tabBounds.Height += 2
						End If
						Exit Select
				End Select
			End If

			'	Adjust first tab in the row to align with tabpage
			Me.EnsureFirstTabIsInView(tabBounds, index)

			Return tabBounds
		End Function
	End Class
End Namespace
