'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms

	<System.ComponentModel.ToolboxItem(False)> _
	Public Class TabStyleVisualStudioProvider
		Inherits TabStyleProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
			Me._ImageAlign = ContentAlignment.MiddleRight
			Me._Overlap = 7

			'	Must set after the _Radius as this is used in the calculations of the actual padding
			Me.Padding = New Point(14, 1)
		End Sub

		Public Overrides Sub AddTabBorder(path As System.Drawing.Drawing2D.GraphicsPath, tabBounds As System.Drawing.Rectangle)
			Select Case Me._TabControl.Alignment
				Case TabAlignment.Top
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + tabBounds.Height - 4, tabBounds.Y + 2)
					path.AddLine(tabBounds.X + tabBounds.Height, tabBounds.Y, tabBounds.Right - 3, tabBounds.Y)
					path.AddArc(tabBounds.Right - 6, tabBounds.Y, 6, 6, 270, 90)
					path.AddLine(tabBounds.Right, tabBounds.Y + 3, tabBounds.Right, tabBounds.Bottom)
					Exit Select
				Case TabAlignment.Bottom
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - 3)
					path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90)
					path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X + tabBounds.Height, tabBounds.Bottom)
					path.AddLine(tabBounds.X + tabBounds.Height - 4, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y)
					Exit Select
				Case TabAlignment.Left
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 3, tabBounds.Bottom)
					path.AddArc(tabBounds.X, tabBounds.Bottom - 6, 6, 6, 90, 90)
					path.AddLine(tabBounds.X, tabBounds.Bottom - 3, tabBounds.X, tabBounds.Y + tabBounds.Width)
					path.AddLine(tabBounds.X + 2, tabBounds.Y + tabBounds.Width - 4, tabBounds.Right, tabBounds.Y)
					Exit Select
				Case TabAlignment.Right
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + tabBounds.Width - 4)
					path.AddLine(tabBounds.Right, tabBounds.Y + tabBounds.Width, tabBounds.Right, tabBounds.Bottom - 3)
					path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90)
					path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom)
					Exit Select
			End Select
		End Sub

	End Class
End Namespace
