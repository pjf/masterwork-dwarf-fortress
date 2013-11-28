'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms

    '51,84,99 rgb

	<System.ComponentModel.ToolboxItem(False)> _
	Public Class TabStyleVS2010Provider
		Inherits TabStyleRoundedProvider
		Public Sub New(tabControl As CustomTabControl)
			MyBase.New(tabControl)
            Me._Radius = 1
			Me._ShowTabCloser = True
			Me._CloserColorActive = Color.Black
			Me._CloserColor = Color.FromArgb(117, 99, 61)
			Me._TextColor = Color.White
			Me._TextColorDisabled = Color.WhiteSmoke
			Me._BorderColor = Color.Transparent
            Me._BorderColorHot = Color.FromArgb(155, 167, 183)
            Me._SelectedTabColor = Color.FromArgb(229, 195, 101)
            Me._SelectedTabColorTop = SystemColors.Window

            '	Must set after the _Radius as this is used in the calculations of the actual padding

            Me.Padding = New Point(6, 5)
        End Sub

        Protected Overrides Function GetTabBackgroundBrush(index As Integer) As Brush
            Dim fillBrush As LinearGradientBrush = Nothing

            '	Capture the colours dependant on selection state of the tab
            Dim dark As Color = Color.Transparent
            Dim light As Color = Color.Transparent

            If Me._TabControl.SelectedIndex = index Then
                dark = Me._SelectedTabColor 'Color.FromArgb(229, 195, 101)
                light = Me._SelectedTabColorTop 'SystemColors.Window
            ElseIf Not Me._TabControl.TabPages(index).Enabled Then
                light = dark
            ElseIf Me.HotTrack AndAlso index = Me._TabControl.ActiveIndex Then
                '	Enable hot tracking
                dark = Color.FromArgb(108, 116, 118)
                light = dark
            End If

            '	Get the correctly aligned gradient
            Dim tabBounds As Rectangle = Me.GetTabRect(index)
            tabBounds.Inflate(3, 3)
            tabBounds.X -= 1
            tabBounds.Y -= 1
            Select Case Me._TabControl.Alignment
                Case TabAlignment.Top
                    fillBrush = New LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical)
                    Exit Select
                Case TabAlignment.Bottom
                    fillBrush = New LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Vertical)
                    Exit Select
                Case TabAlignment.Left
                    fillBrush = New LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal)
                    Exit Select
                Case TabAlignment.Right
                    fillBrush = New LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal)
                    Exit Select
            End Select

            '	Add the blend
            fillBrush.Blend = GetBackgroundBlend()

            Return fillBrush
        End Function

		Private Shared Overloads Function GetBackgroundBlend() As Blend
			Dim relativeIntensities As Single() = New Single() {0F, 0.5F, 1F, 1F}
			Dim relativePositions As Single() = New Single() {0F, 0.5F, 0.51F, 1F}


			Dim blend As New Blend()
			blend.Factors = relativeIntensities
			blend.Positions = relativePositions

			Return blend
		End Function

		Public Overrides Function GetPageBackgroundBrush(index As Integer) As Brush

			'	Capture the colours dependant on selection state of the tab
			Dim light As Color = Color.Transparent

			If Me._TabControl.SelectedIndex = index Then
                light = Me._SelectedTabColor 'Color.FromArgb(229, 195, 101)
			ElseIf Not Me._TabControl.TabPages(index).Enabled Then
				light = Color.Transparent
			ElseIf Me._HotTrack AndAlso index = Me._TabControl.ActiveIndex Then
				'	Enable hot tracking
				light = Color.Transparent
			End If

			Return New SolidBrush(light)
		End Function

		Protected Overrides Sub DrawTabCloser(index As Integer, graphics As Graphics)
			If Me._ShowTabCloser Then
				Dim closerRect As Rectangle = Me._TabControl.GetTabCloserRect(index)
				graphics.SmoothingMode = SmoothingMode.AntiAlias
				If closerRect.Contains(Me._TabControl.MousePosition) Then
					Using closerPath As GraphicsPath = GetCloserButtonPath(closerRect)
						graphics.FillPath(Brushes.White, closerPath)
                        Using closerPen As New Pen(Me._SelectedTabColor) 'Color.FromArgb(229, 195, 101))
                            graphics.DrawPath(closerPen, closerPath)
                        End Using
					End Using
					Using closerPath As GraphicsPath = GetCloserPath(closerRect)
						Using closerPen As New Pen(Me._CloserColorActive)
							closerPen.Width = 2
							graphics.DrawPath(closerPen, closerPath)
						End Using
					End Using
				Else
					If index = Me._TabControl.SelectedIndex Then
						Using closerPath As GraphicsPath = GetCloserPath(closerRect)
							Using closerPen As New Pen(Me._CloserColor)
								closerPen.Width = 2
								graphics.DrawPath(closerPen, closerPath)
							End Using
						End Using
					ElseIf index = Me._TabControl.ActiveIndex Then
						Using closerPath As GraphicsPath = GetCloserPath(closerRect)
							Using closerPen As New Pen(Color.FromArgb(155, 167, 183))
								closerPen.Width = 2
								graphics.DrawPath(closerPen, closerPath)
							End Using
						End Using
					End If

				End If
			End If
		End Sub

		Private Shared Function GetCloserButtonPath(closerRect As Rectangle) As GraphicsPath
			Dim closerPath As New GraphicsPath()
			closerPath.AddLine(closerRect.X - 1, closerRect.Y - 2, closerRect.Right + 1, closerRect.Y - 2)
			closerPath.AddLine(closerRect.Right + 2, closerRect.Y - 1, closerRect.Right + 2, closerRect.Bottom + 1)
			closerPath.AddLine(closerRect.Right + 1, closerRect.Bottom + 2, closerRect.X - 1, closerRect.Bottom + 2)
			closerPath.AddLine(closerRect.X - 2, closerRect.Bottom + 1, closerRect.X - 2, closerRect.Y - 1)
			closerPath.CloseFigure()
			Return closerPath
		End Function

	End Class
End Namespace
