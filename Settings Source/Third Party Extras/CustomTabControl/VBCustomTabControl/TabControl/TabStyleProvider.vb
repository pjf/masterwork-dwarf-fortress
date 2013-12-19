'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace System.Windows.Forms
	<System.ComponentModel.ToolboxItem(False)> _
	Public MustInherit Class TabStyleProvider
		Inherits Component
		#Region "Constructor"

		Protected Sub New(tabControl As CustomTabControl)
			Me._TabControl = tabControl

			Me._BorderColor = Color.Empty
			Me._BorderColorSelected = Color.Empty
			Me._FocusColor = Color.Orange

			If Me._TabControl.RightToLeftLayout Then
				Me._ImageAlign = ContentAlignment.MiddleRight
			Else
				Me._ImageAlign = ContentAlignment.MiddleLeft
			End If

			Me.HotTrack = True

			'	Must set after the _Overlap as this is used in the calculations of the actual padding
			Me.Padding = New Point(6, 3)
		End Sub

		#End Region

		#Region "Factory Methods"

		Public Shared Function CreateProvider(tabControl As CustomTabControl) As TabStyleProvider
			Dim provider As TabStyleProvider

			'	Depending on the display style of the tabControl generate an appropriate provider.
			Select Case tabControl.DisplayStyle
				Case TabStyle.None
					provider = New TabStyleNoneProvider(tabControl)
					Exit Select

				Case TabStyle.[Default]
					provider = New TabStyleDefaultProvider(tabControl)
					Exit Select

				Case TabStyle.Angled
					provider = New TabStyleAngledProvider(tabControl)
					Exit Select

				Case TabStyle.Rounded
					provider = New TabStyleRoundedProvider(tabControl)
					Exit Select

				Case TabStyle.VisualStudio
					provider = New TabStyleVisualStudioProvider(tabControl)
					Exit Select

				Case TabStyle.Chrome
					provider = New TabStyleChromeProvider(tabControl)
					Exit Select

				Case TabStyle.IE8
					provider = New TabStyleIE8Provider(tabControl)
					Exit Select

				Case TabStyle.VS2010
					provider = New TabStyleVS2010Provider(tabControl)
					Exit Select
				Case Else

					provider = New TabStyleDefaultProvider(tabControl)
					Exit Select
			End Select

			provider._Style = tabControl.DisplayStyle
			Return provider
		End Function

		#End Region

		#Region "Protected variables"

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _TabControl As CustomTabControl

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _Padding As Point
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _HotTrack As Boolean
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _Style As TabStyle = TabStyle.[Default]


		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _ImageAlign As ContentAlignment
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _Radius As Integer = 1
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _Overlap As Integer
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _FocusTrack As Boolean
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _Opacity As Single = 1
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _ShowTabCloser As Boolean

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _BorderColorSelected As Color = Color.Empty
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _BorderColor As Color = Color.Empty
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _BorderColorHot As Color = Color.Empty

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _CloserColorActive As Color = Color.Black
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _CloserColor As Color = Color.DarkGray

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _FocusColor As Color = Color.Empty

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _TextColor As Color = Color.Empty
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _TextColorSelected As Color = Color.Empty
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
		Protected _TextColorDisabled As Color = Color.Empty

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
        Protected _SelectedTabColor As Color = Color.Empty

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")> _
        Protected _SelectedTabColorTop As Color = Color.Empty
#End Region

#Region "overridable Methods"

        Public MustOverride Sub AddTabBorder(path As GraphicsPath, tabBounds As Rectangle)

        Public Overridable Function GetTabRect(index As Integer) As Rectangle

            If index < 0 Then
                Return New Rectangle()
            End If
            Dim tabBounds As Rectangle = Me._TabControl.GetTabRect(index)
            If Me._TabControl.RightToLeftLayout Then
                tabBounds.X = Me._TabControl.Width - tabBounds.Right
            End If
            Dim firstTabinRow As Boolean = Me._TabControl.IsFirstTabInRow(index)

            '	Expand to overlap the tabpage
            Select Case Me._TabControl.Alignment
                Case TabAlignment.Top
                    tabBounds.Height += 2
                    Exit Select
                Case TabAlignment.Bottom
                    tabBounds.Height += 2
                    tabBounds.Y -= 2
                    Exit Select
                Case TabAlignment.Left
                    tabBounds.Width += 2
                    Exit Select
                Case TabAlignment.Right
                    tabBounds.X -= 2
                    tabBounds.Width += 2
                    Exit Select
            End Select


            '	Greate Overlap unless first tab in the row to align with tabpage
            If (Not firstTabinRow OrElse Me._TabControl.RightToLeftLayout) AndAlso Me._Overlap > 0 Then
                If Me._TabControl.Alignment <= TabAlignment.Bottom Then
                    tabBounds.X -= Me._Overlap
                    tabBounds.Width += Me._Overlap
                Else
                    tabBounds.Y -= Me._Overlap
                    tabBounds.Height += Me._Overlap
                End If
            End If

            '	Adjust first tab in the row to align with tabpage
            Me.EnsureFirstTabIsInView(tabBounds, index)

            Return tabBounds
        End Function


        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")> _
        Protected Overridable Sub EnsureFirstTabIsInView(ByRef tabBounds As Rectangle, index As Integer)
            '	Adjust first tab in the row to align with tabpage
            '	Make sure we only reposition visible tabs, as we may have scrolled out of view.

            Dim firstTabinRow As Boolean = Me._TabControl.IsFirstTabInRow(index)

            If firstTabinRow Then
                If Me._TabControl.Alignment <= TabAlignment.Bottom Then
                    If Me._TabControl.RightToLeftLayout Then
                        If tabBounds.Left < Me._TabControl.Right Then
                            Dim tabPageRight As Integer = Me._TabControl.GetPageBounds(index).Right
                            If tabBounds.Right > tabPageRight Then
                                tabBounds.Width -= (tabBounds.Right - tabPageRight)
                            End If
                        End If
                    Else
                        If tabBounds.Right > 0 Then
                            Dim tabPageX As Integer = Me._TabControl.GetPageBounds(index).X
                            If tabBounds.X < tabPageX Then
                                tabBounds.Width -= (tabPageX - tabBounds.X)
                                tabBounds.X = tabPageX
                            End If
                        End If
                    End If
                Else
                    If Me._TabControl.RightToLeftLayout Then
                        If tabBounds.Top < Me._TabControl.Bottom Then
                            Dim tabPageBottom As Integer = Me._TabControl.GetPageBounds(index).Bottom
                            If tabBounds.Bottom > tabPageBottom Then
                                tabBounds.Height -= (tabBounds.Bottom - tabPageBottom)
                            End If
                        End If
                    Else
                        If tabBounds.Bottom > 0 Then
                            Dim tabPageY As Integer = Me._TabControl.GetPageBounds(index).Location.Y
                            If tabBounds.Y < tabPageY Then
                                tabBounds.Height -= (tabPageY - tabBounds.Y)
                                tabBounds.Y = tabPageY
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Overridable Function GetTabBackgroundBrush(index As Integer) As Brush
            Dim fillBrush As LinearGradientBrush = Nothing

            '	Capture the colours dependant on selection state of the tab
            Dim dark As Color = Color.FromArgb(207, 207, 207)
            Dim light As Color = Color.FromArgb(242, 242, 242)

            If Me._TabControl.SelectedIndex = index Then
                dark = SystemColors.ControlLight
                light = SystemColors.Window
            ElseIf Not Me._TabControl.TabPages(index).Enabled Then
                light = dark
            ElseIf Me._HotTrack AndAlso index = Me._TabControl.ActiveIndex Then
                '	Enable hot tracking
                light = Color.FromArgb(234, 246, 253)
                dark = Color.FromArgb(167, 217, 245)
            End If

            '	Get the correctly aligned gradient
            Dim tabBounds As Rectangle = Me.GetTabRect(index)
            tabBounds.Inflate(3, 3)
            tabBounds.X -= 1
            tabBounds.Y -= 1
            Select Case Me._TabControl.Alignment
                Case TabAlignment.Top
                    If Me._TabControl.SelectedIndex = index Then
                        dark = light
                    End If
                    fillBrush = New LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical)
                    Exit Select
                Case TabAlignment.Bottom
                    fillBrush = New LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical)
                    Exit Select
                Case TabAlignment.Left
                    fillBrush = New LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal)
                    Exit Select
                Case TabAlignment.Right
                    fillBrush = New LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal)
                    Exit Select
            End Select

            '	Add the blend
            fillBrush.Blend = Me.GetBackgroundBlend()

            Return fillBrush
        End Function

#End Region

#Region "Base Properties"

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Property DisplayStyle() As TabStyle
            Get
                Return Me._Style
            End Get
            Set(value As TabStyle)
                Me._Style = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property ImageAlign() As ContentAlignment
            Get
                Return Me._ImageAlign
            End Get
            Set(value As ContentAlignment)
                Me._ImageAlign = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance")> _
        Public Property Padding() As Point
            Get
                Return Me._Padding
            End Get
            Set(value As Point)
                Me._Padding = value
                '	This line will trigger the handle to recreate, therefore invalidating the control
                If Me._ShowTabCloser Then
                    If value.X + CInt(Me._Radius \ 2) < -6 Then
                        DirectCast(Me._TabControl, TabControl).Padding = New Point(0, value.Y)
                    Else
                        DirectCast(Me._TabControl, TabControl).Padding = New Point(value.X + CInt(Me._Radius \ 2) + 6, value.Y)
                    End If
                Else
                    If value.X + CInt(Me._Radius \ 2) < 1 Then
                        DirectCast(Me._TabControl, TabControl).Padding = New Point(0, value.Y)
                    Else
                        DirectCast(Me._TabControl, TabControl).Padding = New Point(value.X + CInt(Me._Radius \ 2) - 1, value.Y)
                    End If
                End If
            End Set
        End Property


        <Category("Appearance"), DefaultValue(1), Browsable(True)> _
        Public Property Radius() As Integer
            Get
                Return Me._Radius
            End Get
            Set(value As Integer)
                If value < 1 Then
                    Throw New ArgumentException("The radius must be greater than 1", "value")
                End If
                Me._Radius = value
                '	Adjust padding
                Me.Padding = Me._Padding
            End Set
        End Property

        <Category("Appearance")> _
        Public Property Overlap() As Integer
            Get
                Return Me._Overlap
            End Get
            Set(value As Integer)
                If value < 0 Then
                    Throw New ArgumentException("The tabs cannot have a negative overlap", "value")
                End If

                Me._Overlap = value
            End Set
        End Property


        <Category("Appearance")> _
        Public Property FocusTrack() As Boolean
            Get
                Return Me._FocusTrack
            End Get
            Set(value As Boolean)
                Me._FocusTrack = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance")> _
        Public Property HotTrack() As Boolean
            Get
                Return Me._HotTrack
            End Get
            Set(value As Boolean)
                Me._HotTrack = value
                DirectCast(Me._TabControl, TabControl).HotTrack = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property ShowTabCloser() As Boolean
            Get
                Return Me._ShowTabCloser
            End Get
            Set(value As Boolean)
                Me._ShowTabCloser = value
                '	Adjust padding
                Me.Padding = Me._Padding
            End Set
        End Property

        <Category("Appearance")> _
        Public Property Opacity() As Single
            Get
                Return Me._Opacity
            End Get
            Set(value As Single)
                If value < 0 Then
                    Throw New ArgumentException("The opacity must be between 0 and 1", "value")
                End If
                If value > 1 Then
                    Throw New ArgumentException("The opacity must be between 0 and 1", "value")
                End If
                Me._Opacity = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property BorderColorSelected() As Color
            Get
                If Me._BorderColorSelected.IsEmpty Then
                    Return ThemedColors.ToolBorder
                Else
                    Return Me._BorderColorSelected
                End If
            End Get
            Set(value As Color)
                If value.Equals(ThemedColors.ToolBorder) Then
                    Me._BorderColorSelected = Color.Empty
                Else
                    Me._BorderColorSelected = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property BorderColorHot() As Color
            Get
                If Me._BorderColorHot.IsEmpty Then
                    Return SystemColors.ControlDark
                Else
                    Return Me._BorderColorHot
                End If
            End Get
            Set(value As Color)
                If value.Equals(SystemColors.ControlDark) Then
                    Me._BorderColorHot = Color.Empty
                Else
                    Me._BorderColorHot = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property BorderColor() As Color
            Get
                If Me._BorderColor.IsEmpty Then
                    Return SystemColors.ControlDark
                Else
                    Return Me._BorderColor
                End If
            End Get
            Set(value As Color)
                If value.Equals(SystemColors.ControlDark) Then
                    Me._BorderColor = Color.Empty
                Else
                    Me._BorderColor = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property TextColor() As Color
            Get
                If Me._TextColor.IsEmpty Then
                    Return SystemColors.ControlText
                Else
                    Return Me._TextColor
                End If
            End Get
            Set(value As Color)
                If value.Equals(SystemColors.ControlText) Then
                    Me._TextColor = Color.Empty
                Else
                    Me._TextColor = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property TextColorSelected() As Color
            Get
                If Me._TextColorSelected.IsEmpty Then
                    Return SystemColors.ControlText
                Else
                    Return Me._TextColorSelected
                End If
            End Get
            Set(value As Color)
                If value.Equals(SystemColors.ControlText) Then
                    Me._TextColorSelected = Color.Empty
                Else
                    Me._TextColorSelected = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "")> _
        Public Property TextColorDisabled() As Color
            Get
                If Me._TextColor.IsEmpty Then
                    Return SystemColors.ControlDark
                Else
                    Return Me._TextColorDisabled
                End If
            End Get
            Set(value As Color)
                If value.Equals(SystemColors.ControlDark) Then
                    Me._TextColorDisabled = Color.Empty
                Else
                    Me._TextColorDisabled = value
                End If
                Me._TabControl.Invalidate()
            End Set
        End Property


        <Category("Appearance"), DefaultValue(GetType(Color), "Orange")> _
        Public Property FocusColor() As Color
            Get
                Return Me._FocusColor
            End Get
            Set(value As Color)
                Me._FocusColor = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "Black")> _
        Public Property CloserColorActive() As Color
            Get
                Return Me._CloserColorActive
            End Get
            Set(value As Color)
                Me._CloserColorActive = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "DarkGrey")> _
        Public Property CloserColor() As Color
            Get
                Return Me._CloserColor
            End Get
            Set(value As Color)
                Me._CloserColor = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "DarkGrey")> _
        Public Property SelectedTabColor() As Color
            Get
                Return Me._SelectedTabColor
            End Get
            Set(value As Color)
                Me._SelectedTabColor = value
                Me._TabControl.Invalidate()
            End Set
        End Property

        <Category("Appearance"), DefaultValue(GetType(Color), "DarkGrey")> _
        Public Property SelectedTabColorTop() As Color
            Get
                Return Me._SelectedTabColorTop
            End Get
            Set(value As Color)
                Me._SelectedTabColorTop = value
                Me._TabControl.Invalidate()
            End Set
        End Property

#End Region

		#Region "Painting"

		Public Sub PaintTab(index As Integer, graphics As Graphics)
			Using tabpath As GraphicsPath = Me.GetTabBorder(index)
				Using fillBrush As Brush = Me.GetTabBackgroundBrush(index)
					'	Paint the background
					graphics.FillPath(fillBrush, tabpath)

					'	Paint a focus indication
					If Me._TabControl.Focused Then
						Me.DrawTabFocusIndicator(tabpath, index, graphics)
					End If

					'	Paint the closer

					Me.DrawTabCloser(index, graphics)
				End Using
			End Using
		End Sub

		Protected Overridable Sub DrawTabCloser(index As Integer, graphics As Graphics)
			If Me._ShowTabCloser Then
				Dim closerRect As Rectangle = Me._TabControl.GetTabCloserRect(index)
				graphics.SmoothingMode = SmoothingMode.AntiAlias
				Using closerPath As GraphicsPath = TabStyleProvider.GetCloserPath(closerRect)
					If closerRect.Contains(Me._TabControl.MousePosition) Then
						Using closerPen As New Pen(Me._CloserColorActive)
							graphics.DrawPath(closerPen, closerPath)
						End Using
					Else
						Using closerPen As New Pen(Me._CloserColor)
							graphics.DrawPath(closerPen, closerPath)
						End Using

					End If
				End Using
			End If
		End Sub

		Protected Shared Function GetCloserPath(closerRect As Rectangle) As GraphicsPath
			Dim closerPath As New GraphicsPath()
			closerPath.AddLine(closerRect.X, closerRect.Y, closerRect.Right, closerRect.Bottom)
			closerPath.CloseFigure()
			closerPath.AddLine(closerRect.Right, closerRect.Y, closerRect.X, closerRect.Bottom)
			closerPath.CloseFigure()

			Return closerPath
		End Function

		Private Sub DrawTabFocusIndicator(tabpath As GraphicsPath, index As Integer, graphics As Graphics)
			If Me._FocusTrack AndAlso Me._TabControl.Focused AndAlso index = Me._TabControl.SelectedIndex Then
				Dim focusBrush As Brush = Nothing
				Dim pathRect As RectangleF = tabpath.GetBounds()
				Dim focusRect As Rectangle = Rectangle.Empty
				Select Case Me._TabControl.Alignment
					Case TabAlignment.Top
						focusRect = New Rectangle(CInt(Math.Truncate(pathRect.X)), CInt(Math.Truncate(pathRect.Y)), CInt(Math.Truncate(pathRect.Width)), 4)
						focusBrush = New LinearGradientBrush(focusRect, Me._FocusColor, SystemColors.Window, LinearGradientMode.Vertical)
						Exit Select
					Case TabAlignment.Bottom
						focusRect = New Rectangle(CInt(Math.Truncate(pathRect.X)), CInt(Math.Truncate(pathRect.Bottom)) - 4, CInt(Math.Truncate(pathRect.Width)), 4)
						focusBrush = New LinearGradientBrush(focusRect, SystemColors.ControlLight, Me._FocusColor, LinearGradientMode.Vertical)
						Exit Select
					Case TabAlignment.Left
						focusRect = New Rectangle(CInt(Math.Truncate(pathRect.X)), CInt(Math.Truncate(pathRect.Y)), 4, CInt(Math.Truncate(pathRect.Height)))
						focusBrush = New LinearGradientBrush(focusRect, Me._FocusColor, SystemColors.ControlLight, LinearGradientMode.Horizontal)
						Exit Select
					Case TabAlignment.Right
						focusRect = New Rectangle(CInt(Math.Truncate(pathRect.Right)) - 4, CInt(Math.Truncate(pathRect.Y)), 4, CInt(Math.Truncate(pathRect.Height)))
						focusBrush = New LinearGradientBrush(focusRect, SystemColors.ControlLight, Me._FocusColor, LinearGradientMode.Horizontal)
						Exit Select
				End Select

				'	Ensure the focus stip does not go outside the tab
				Dim focusRegion As New Region(focusRect)
				focusRegion.Intersect(tabpath)
				graphics.FillRegion(focusBrush, focusRegion)
				focusRegion.Dispose()
				focusBrush.Dispose()
			End If
		End Sub

		#End Region

		#Region "Background brushes"

		Private Function GetBackgroundBlend() As Blend
			Dim relativeIntensities As Single() = New Single() {0F, 0.7F, 1F}
			Dim relativePositions As Single() = New Single() {0F, 0.6F, 1F}

			'	Glass look to top aligned tabs
			If Me._TabControl.Alignment = TabAlignment.Top Then
				relativeIntensities = New Single() {0F, 0.5F, 1F, 1F}
				relativePositions = New Single() {0F, 0.5F, 0.51F, 1F}
			End If

			Dim blend As New Blend()
			blend.Factors = relativeIntensities
			blend.Positions = relativePositions

			Return blend
		End Function

		Public Overridable Function GetPageBackgroundBrush(index As Integer) As Brush

			'	Capture the colours dependant on selection state of the tab
			Dim light As Color = Color.FromArgb(242, 242, 242)
			If Me._TabControl.Alignment = TabAlignment.Top Then
				light = Color.FromArgb(207, 207, 207)
			End If

			If Me._TabControl.SelectedIndex = index Then
				light = SystemColors.Window
			ElseIf Not Me._TabControl.TabPages(index).Enabled Then
				light = Color.FromArgb(207, 207, 207)
			ElseIf Me._HotTrack AndAlso index = Me._TabControl.ActiveIndex Then
				'	Enable hot tracking
				light = Color.FromArgb(234, 246, 253)
			End If

			Return New SolidBrush(light)
		End Function

		#End Region

		#Region "Tab border and rect"

		Public Function GetTabBorder(index As Integer) As GraphicsPath

			Dim path As New GraphicsPath()
			Dim tabBounds As Rectangle = Me.GetTabRect(index)

			Me.AddTabBorder(path, tabBounds)

			path.CloseFigure()
			Return path
		End Function

		#End Region

	End Class
End Namespace
