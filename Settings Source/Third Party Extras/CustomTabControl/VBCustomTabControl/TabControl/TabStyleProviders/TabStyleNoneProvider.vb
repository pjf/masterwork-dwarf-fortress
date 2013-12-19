'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms

	<System.ComponentModel.ToolboxItem(False)> _
	Public Class TabStyleNoneProvider
		Inherits TabStyleProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
		End Sub

		Public Overrides Sub AddTabBorder(path As System.Drawing.Drawing2D.GraphicsPath, tabBounds As System.Drawing.Rectangle)
		End Sub
	End Class
End Namespace
