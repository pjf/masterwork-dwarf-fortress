'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms

	<System.ComponentModel.ToolboxItem(False)> _
	Public Class TabStyleRoundedProvider
		Inherits TabStyleProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
			Me._Radius = 10
			'	Must set after the _Radius as this is used in the calculations of the actual padding
			Me.Padding = New Point(6, 3)
		End Sub

		Public Overrides Sub AddTabBorder(path As System.Drawing.Drawing2D.GraphicsPath, tabBounds As System.Drawing.Rectangle)

			Select Case Me._TabControl.Alignment
				Case TabAlignment.Top
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y + Me._Radius)
					path.AddArc(tabBounds.X, tabBounds.Y, Me._Radius * 2, Me._Radius * 2, 180, 90)
					path.AddLine(tabBounds.X + Me._Radius, tabBounds.Y, tabBounds.Right - Me._Radius, tabBounds.Y)
					path.AddArc(tabBounds.Right - Me._Radius * 2, tabBounds.Y, Me._Radius * 2, Me._Radius * 2, 270, 90)
					path.AddLine(tabBounds.Right, tabBounds.Y + Me._Radius, tabBounds.Right, tabBounds.Bottom)
					Exit Select
				Case TabAlignment.Bottom
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - Me._Radius)
					path.AddArc(tabBounds.Right - Me._Radius * 2, tabBounds.Bottom - Me._Radius * 2, Me._Radius * 2, Me._Radius * 2, 0, 90)
					path.AddLine(tabBounds.Right - Me._Radius, tabBounds.Bottom, tabBounds.X + Me._Radius, tabBounds.Bottom)
					path.AddArc(tabBounds.X, tabBounds.Bottom - Me._Radius * 2, Me._Radius * 2, Me._Radius * 2, 90, 90)
					path.AddLine(tabBounds.X, tabBounds.Bottom - Me._Radius, tabBounds.X, tabBounds.Y)
					Exit Select
				Case TabAlignment.Left
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + Me._Radius, tabBounds.Bottom)
					path.AddArc(tabBounds.X, tabBounds.Bottom - Me._Radius * 2, Me._Radius * 2, Me._Radius * 2, 90, 90)
					path.AddLine(tabBounds.X, tabBounds.Bottom - Me._Radius, tabBounds.X, tabBounds.Y + Me._Radius)
					path.AddArc(tabBounds.X, tabBounds.Y, Me._Radius * 2, Me._Radius * 2, 180, 90)
					path.AddLine(tabBounds.X + Me._Radius, tabBounds.Y, tabBounds.Right, tabBounds.Y)
					Exit Select
				Case TabAlignment.Right
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - Me._Radius, tabBounds.Y)
					path.AddArc(tabBounds.Right - Me._Radius * 2, tabBounds.Y, Me._Radius * 2, Me._Radius * 2, 270, 90)
					path.AddLine(tabBounds.Right, tabBounds.Y + Me._Radius, tabBounds.Right, tabBounds.Bottom - Me._Radius)
					path.AddArc(tabBounds.Right - Me._Radius * 2, tabBounds.Bottom - Me._Radius * 2, Me._Radius * 2, Me._Radius * 2, 0, 90)
					path.AddLine(tabBounds.Right - Me._Radius, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom)
					Exit Select
			End Select
		End Sub
	End Class
End Namespace
