'
' * This code is provided under the Code Project Open Licence (CPOL)
' * See http://www.codeproject.com/info/cpol10.aspx for details
' 


Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace System.Windows.Forms

	<ToolboxBitmapAttribute(GetType(TabControl))> _
	Public Class CustomTabControl
		Inherits TabControl

		#Region "Construction"

		Public Sub New()

            'Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)

            Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)

            Me._BackBuffer = New Bitmap(Me.Width, Me.Height)
            Me._BackBufferGraphics = Graphics.FromImage(Me._BackBuffer)
            Me._TabBuffer = New Bitmap(Me.Width, Me.Height)
            Me._TabBufferGraphics = Graphics.FromImage(Me._TabBuffer)


			Me.DisplayStyle = TabStyle.[Default]
		End Sub

		Protected Overrides Sub OnCreateControl()
			MyBase.OnCreateControl()
			Me.OnFontChanged(EventArgs.Empty)
		End Sub


		Protected Overrides ReadOnly Property CreateParams() As CreateParams
			<SecurityPermission(SecurityAction.LinkDemand, Flags := SecurityPermissionFlag.UnmanagedCode)> _
			Get
				Dim cp As CreateParams = MyBase.CreateParams
				If Me.RightToLeftLayout Then
					cp.ExStyle = cp.ExStyle Or NativeMethods.WS_EX_LAYOUTRTL Or NativeMethods.WS_EX_NOINHERITLAYOUT
				End If
				Return cp
			End Get
		End Property

		Protected Overrides Sub Dispose(disposing As Boolean)
			MyBase.Dispose(disposing)
			If disposing Then
				If Me._BackImage IsNot Nothing Then
					Me._BackImage.Dispose()
				End If
				If Me._BackBufferGraphics IsNot Nothing Then
					Me._BackBufferGraphics.Dispose()
				End If
				If Me._BackBuffer IsNot Nothing Then
					Me._BackBuffer.Dispose()
				End If
				If Me._TabBufferGraphics IsNot Nothing Then
					Me._TabBufferGraphics.Dispose()
				End If
				If Me._TabBuffer IsNot Nothing Then
					Me._TabBuffer.Dispose()
				End If

				If Me._StyleProvider IsNot Nothing Then
					Me._StyleProvider.Dispose()
				End If
			End If
		End Sub

		#End Region

		#Region "Private variables"

		Private _BackImage As Bitmap
		Private _BackBuffer As Bitmap
		Private _BackBufferGraphics As Graphics
		Private _TabBuffer As Bitmap
		Private _TabBufferGraphics As Graphics

		Private _oldValue As Integer
		Private _dragStartPosition As Point = Point.Empty

		Private _Style As TabStyle
		Private _StyleProvider As TabStyleProvider

		Private _TabPages As List(Of TabPage)

		#End Region

		#Region "Public properties"

		<Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Property DisplayStyleProvider() As TabStyleProvider
			Get
				If Me._StyleProvider Is Nothing Then
					Me.DisplayStyle = TabStyle.[Default]
				End If

				Return Me._StyleProvider
			End Get
			Set
				Me._StyleProvider = value
			End Set
		End Property

		<Category("Appearance"), DefaultValue(GetType(TabStyle), "Default"), RefreshProperties(RefreshProperties.All)> _
		Public Property DisplayStyle() As TabStyle
			Get
				Return Me._Style
			End Get
			Set
				If Me._Style <> value Then
					Me._Style = value
					Me._StyleProvider = TabStyleProvider.CreateProvider(Me)
					Me.Invalidate()
				End If
			End Set
		End Property

		<Category("Appearance"), RefreshProperties(RefreshProperties.All)> _
		Public Shadows Property Multiline() As Boolean
			Get
				Return MyBase.Multiline
			End Get
			Set
				MyBase.Multiline = value
				Me.Invalidate()
			End Set
		End Property


		'	Hide the Padding attribute so it can not be changed
		'	We are handling this on the Style Provider
		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		Public Shadows Property Padding() As Point
			Get
				Return Me.DisplayStyleProvider.Padding
			End Get
			Set
				Me.DisplayStyleProvider.Padding = value
			End Set
		End Property

		Public Overrides Property RightToLeftLayout() As Boolean
			Get
				Return MyBase.RightToLeftLayout
			End Get
			Set
				MyBase.RightToLeftLayout = value
				Me.UpdateStyles()
			End Set
		End Property


		'	Hide the HotTrack attribute so it can not be changed
		'	We are handling this on the Style Provider
		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		Public Shadows Property HotTrack() As Boolean
			Get
				Return Me.DisplayStyleProvider.HotTrack
			End Get
			Set
				Me.DisplayStyleProvider.HotTrack = value
			End Set
		End Property

		<Category("Appearance")> _
		Public Shadows Property Alignment() As TabAlignment
			Get
				Return MyBase.Alignment
			End Get
			Set
				MyBase.Alignment = value
				Select Case value
					Case TabAlignment.Top, TabAlignment.Bottom
						Me.Multiline = False
						Exit Select
					Case TabAlignment.Left, TabAlignment.Right
						Me.Multiline = True
						Exit Select

				End Select
			End Set
		End Property

		'	Hide the Appearance attribute so it can not be changed
		'	We don't want it as we are doing all the painting.
		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId := "value")> _
		Public Shadows Property Appearance() As TabAppearance
			Get
				Return MyBase.Appearance
			End Get
			Set
				'	Don't permit setting to other appearances as we are doing all the painting
				MyBase.Appearance = TabAppearance.Normal
			End Set
		End Property

		Public Overrides ReadOnly Property DisplayRectangle() As Rectangle
			Get
				'	Special processing to hide tabs
				If Me._Style = TabStyle.None Then
					Return New Rectangle(0, 0, Width, Height)
				Else
					Dim tabStripHeight As Integer = 0
					Dim itemHeight As Integer = 0

					If Me.Alignment <= TabAlignment.Bottom Then
						itemHeight = Me.ItemSize.Height
					Else
						itemHeight = Me.ItemSize.Width
					End If

					tabStripHeight = 5 + (itemHeight * Me.RowCount)

					Dim rect As New Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4)
					Select Case Me.Alignment
						Case TabAlignment.Top
							rect = New Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4)
							Exit Select
						Case TabAlignment.Bottom
							rect = New Rectangle(4, 4, Width - 8, Height - tabStripHeight - 4)
							Exit Select
						Case TabAlignment.Left
							rect = New Rectangle(tabStripHeight, 4, Width - tabStripHeight - 4, Height - 8)
							Exit Select
						Case TabAlignment.Right
							rect = New Rectangle(4, 4, Width - tabStripHeight - 4, Height - 8)
							Exit Select
					End Select
					Return rect
				End If
			End Get
		End Property

		<Browsable(False)> _
		Public ReadOnly Property ActiveIndex() As Integer
			Get
				Dim hitTestInfo As New NativeMethods.TCHITTESTINFO(Me.PointToClient(Control.MousePosition))
				Dim index As Integer = NativeMethods.SendMessage(Me.Handle, NativeMethods.TCM_HITTEST, IntPtr.Zero, NativeMethods.ToIntPtr(hitTestInfo)).ToInt32()
				If index = -1 Then
					Return -1
				Else
					If Me.TabPages(index).Enabled Then
						Return index
					Else
						Return -1
					End If
				End If
			End Get
		End Property

		<Browsable(False)> _
		Public ReadOnly Property ActiveTab() As TabPage
			Get
				Dim activeIndex As Integer = Me.ActiveIndex
				If activeIndex > -1 Then
					Return Me.TabPages(activeIndex)
				Else
					Return Nothing
				End If
			End Get
		End Property

		#End Region

		#Region "Extension methods"

		Public Sub HideTab(page As TabPage)
			If page IsNot Nothing AndAlso Me.TabPages.Contains(page) Then
				Me.BackupTabPages()
				Me.TabPages.Remove(page)
			End If
		End Sub

		Public Sub HideTab(index As Integer)
			If Me.IsValidTabIndex(index) Then
				Me.HideTab(Me._TabPages(index))
			End If
		End Sub

		Public Sub HideTab(key As String)
			If Me.TabPages.ContainsKey(key) Then
				Me.HideTab(Me.TabPages(key))
			End If
		End Sub

		Public Sub ShowTab(page As TabPage)
			If page IsNot Nothing Then
				If Me._TabPages IsNot Nothing Then
					If Not Me.TabPages.Contains(page) AndAlso Me._TabPages.Contains(page) Then

						'	Get insert point from backup of pages
						Dim pageIndex As Integer = Me._TabPages.IndexOf(page)
						If pageIndex > 0 Then
							Dim start As Integer = pageIndex - 1

							'	Check for presence of earlier pages in the visible tabs
							For index As Integer = start To 0 Step -1
								If Me.TabPages.Contains(Me._TabPages(index)) Then

									'	Set insert point to the right of the last present tab
									pageIndex = Me.TabPages.IndexOf(Me._TabPages(index)) + 1
									Exit For
								End If
							Next
						End If

						'	Insert the page, or add to the end
						If (pageIndex >= 0) AndAlso (pageIndex < Me.TabPages.Count) Then
							Me.TabPages.Insert(pageIndex, page)
						Else
							Me.TabPages.Add(page)
						End If
					End If
				Else

					'	If the page is not found at all then just add it
					If Not Me.TabPages.Contains(page) Then
						Me.TabPages.Add(page)
					End If
				End If
			End If
		End Sub

		Public Sub ShowTab(index As Integer)
			If Me.IsValidTabIndex(index) Then
				Me.ShowTab(Me._TabPages(index))
			End If
		End Sub

		Public Sub ShowTab(key As String)
			If Me._TabPages IsNot Nothing Then
				Dim tab As TabPage = Me._TabPages.Find(Function(page As TabPage) page.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
				Me.ShowTab(tab)
			End If
		End Sub

		Private Function IsValidTabIndex(index As Integer) As Boolean
			Me.BackupTabPages()
			Return ((index >= 0) AndAlso (index < Me._TabPages.Count))
		End Function

		Private Sub BackupTabPages()
			If Me._TabPages Is Nothing Then
				Me._TabPages = New List(Of TabPage)()
				For Each page As TabPage In Me.TabPages
					Me._TabPages.Add(page)
				Next
			End If
		End Sub

		#End Region

		#Region "Drag 'n' Drop"

		Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
			MyBase.OnMouseDown(e)
			If Me.AllowDrop Then
				Me._dragStartPosition = New Point(e.X, e.Y)
			End If
		End Sub

		Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
			MyBase.OnMouseUp(e)
			If Me.AllowDrop Then
				Me._dragStartPosition = Point.Empty
			End If
		End Sub

		Protected Overrides Sub OnDragOver(drgevent As DragEventArgs)
			MyBase.OnDragOver(drgevent)

			If drgevent.Data.GetDataPresent(GetType(TabPage)) Then
				drgevent.Effect = DragDropEffects.Move
			Else
				drgevent.Effect = DragDropEffects.None
			End If
		End Sub

		Protected Overrides Sub OnDragDrop(drgevent As DragEventArgs)
			MyBase.OnDragDrop(drgevent)
			If drgevent.Data.GetDataPresent(GetType(TabPage)) Then
				drgevent.Effect = DragDropEffects.Move

				Dim dragTab As TabPage = DirectCast(drgevent.Data.GetData(GetType(TabPage)), TabPage)

				If Me.ActiveTab Is dragTab Then
					Return
				End If

				'	Capture insert point and adjust for removal of tab
				'	We cannot assess this after removal as differeing tab sizes will cause
				'	inaccuracies in the activeTab at insert point.
				Dim insertPoint As Integer = Me.ActiveIndex
				If dragTab.Parent.Equals(Me) AndAlso Me.TabPages.IndexOf(dragTab) < insertPoint Then
					insertPoint -= 1
				End If
				If insertPoint < 0 Then
					insertPoint = 0
				End If

				'	Remove from current position (could be another tabcontrol)
				DirectCast(dragTab.Parent, TabControl).TabPages.Remove(dragTab)

				'	Add to current position
				Me.TabPages.Insert(insertPoint, dragTab)

					'	deal with hidden tab handling?
				Me.SelectedTab = dragTab
			End If
		End Sub

		Private Sub StartDragDrop()
			If Not Me._dragStartPosition.IsEmpty Then
				Dim dragTab As TabPage = Me.SelectedTab
				If dragTab IsNot Nothing Then
					'	Test for movement greater than the drag activation trigger area
					Dim dragTestRect As New Rectangle(Me._dragStartPosition, Size.Empty)
					dragTestRect.Inflate(SystemInformation.DragSize)
					Dim pt As Point = Me.PointToClient(Control.MousePosition)
					If Not dragTestRect.Contains(pt) Then
						Me.DoDragDrop(dragTab, DragDropEffects.All)
						Me._dragStartPosition = Point.Empty
					End If
				End If
			End If
		End Sub

		#End Region

		#Region "Events"

		<Category("Action")> _
		Public Event HScroll As ScrollEventHandler
		<Category("Action")> _
		Public Event TabImageClick As EventHandler(Of TabControlEventArgs)
		<Category("Action")> _
		Public Event TabClosing As EventHandler(Of TabControlCancelEventArgs)

		#End Region

		#Region "Base class event processing"

		Protected Overrides Sub OnFontChanged(e As EventArgs)
			Dim hFont As IntPtr = Me.Font.ToHfont()
			NativeMethods.SendMessage(Me.Handle, NativeMethods.WM_SETFONT, hFont, CType(-1, IntPtr))
			NativeMethods.SendMessage(Me.Handle, NativeMethods.WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero)
			Me.UpdateStyles()
			If Me.Visible Then
				Me.Invalidate()
			End If
		End Sub

		Protected Overrides Sub OnResize(e As EventArgs)
			'	Recreate the buffer for manual double buffering
			If Me.Width > 0 AndAlso Me.Height > 0 Then
				If Me._BackImage IsNot Nothing Then
					Me._BackImage.Dispose()
					Me._BackImage = Nothing
				End If
				If Me._BackBufferGraphics IsNot Nothing Then
					Me._BackBufferGraphics.Dispose()
				End If
				If Me._BackBuffer IsNot Nothing Then
					Me._BackBuffer.Dispose()
				End If

				Me._BackBuffer = New Bitmap(Me.Width, Me.Height)
				Me._BackBufferGraphics = Graphics.FromImage(Me._BackBuffer)

				If Me._TabBufferGraphics IsNot Nothing Then
					Me._TabBufferGraphics.Dispose()
				End If
				If Me._TabBuffer IsNot Nothing Then
					Me._TabBuffer.Dispose()
				End If

				Me._TabBuffer = New Bitmap(Me.Width, Me.Height)
				Me._TabBufferGraphics = Graphics.FromImage(Me._TabBuffer)

				If Me._BackImage IsNot Nothing Then
					Me._BackImage.Dispose()
					Me._BackImage = Nothing

				End If
			End If
			MyBase.OnResize(e)
		End Sub

		Protected Overrides Sub OnParentBackColorChanged(e As EventArgs)
			If Me._BackImage IsNot Nothing Then
				Me._BackImage.Dispose()
				Me._BackImage = Nothing
			End If
			MyBase.OnParentBackColorChanged(e)
		End Sub

		Protected Overrides Sub OnParentBackgroundImageChanged(e As EventArgs)
			If Me._BackImage IsNot Nothing Then
				Me._BackImage.Dispose()
				Me._BackImage = Nothing
			End If
			MyBase.OnParentBackgroundImageChanged(e)
		End Sub

		Private Sub OnParentResize(sender As Object, e As EventArgs)
			If Me.Visible Then
				Me.Invalidate()
			End If
		End Sub


		Protected Overrides Sub OnParentChanged(e As EventArgs)
			MyBase.OnParentChanged(e)
			If Me.Parent IsNot Nothing Then
				AddHandler Me.Parent.Resize, AddressOf Me.OnParentResize
			End If
		End Sub

		Protected Overrides Sub OnSelecting(e As TabControlCancelEventArgs)
			MyBase.OnSelecting(e)

			'	Do not allow selecting of disabled tabs
			If e.Action = TabControlAction.Selecting AndAlso e.TabPage IsNot Nothing AndAlso Not e.TabPage.Enabled Then
				e.Cancel = True
			End If
		End Sub

		Protected Overrides Sub OnMove(e As EventArgs)
			If Me.Width > 0 AndAlso Me.Height > 0 Then
				If Me._BackImage IsNot Nothing Then
					Me._BackImage.Dispose()
					Me._BackImage = Nothing
				End If
			End If
			MyBase.OnMove(e)
			Me.Invalidate()
		End Sub

		Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
			MyBase.OnControlAdded(e)
			If Me.Visible Then
				Me.Invalidate()
			End If
		End Sub

		Protected Overrides Sub OnControlRemoved(e As ControlEventArgs)
			MyBase.OnControlRemoved(e)
			If Me.Visible Then
				Me.Invalidate()
			End If
		End Sub


		<UIPermission(SecurityAction.LinkDemand, Window := UIPermissionWindow.AllWindows)> _
		Protected Overrides Function ProcessMnemonic(charCode As Char) As Boolean
			For Each page As TabPage In Me.TabPages
				If IsMnemonic(charCode, page.Text) Then
					Me.SelectedTab = page
					Return True
				End If
			Next
			Return MyBase.ProcessMnemonic(charCode)
		End Function

		Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
			MyBase.OnSelectedIndexChanged(e)
		End Sub

		<SecurityPermission(SecurityAction.LinkDemand, Flags := SecurityPermissionFlag.UnmanagedCode)> _
		<System.Diagnostics.DebuggerStepThrough> _
		Protected Overrides Sub WndProc(ByRef m As Message)

			Select Case m.Msg
				Case NativeMethods.WM_HSCROLL

					'	Raise the scroll event when the scroller is scrolled
					MyBase.WndProc(m)
					Me.OnHScroll(New ScrollEventArgs(CType(NativeMethods.LoWord(m.WParam), ScrollEventType), _oldValue, NativeMethods.HiWord(m.WParam), ScrollOrientation.HorizontalScroll))
					Exit Select
				Case Else
					'				case NativeMethods.WM_PAINT:
					'					
					'					//	Handle painting ourselves rather than call the base OnPaint.
					'					CustomPaint(ref m);
					'					break;

					MyBase.WndProc(m)
					Exit Select

			End Select
		End Sub

		Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
			Dim index As Integer = Me.ActiveIndex

			'	If we are clicking on an image then raise the ImageClicked event before raising the standard mouse click event
			'	if there if a handler.
			If index > -1 AndAlso Me.TabImageClickEvent IsNot Nothing AndAlso (Me.TabPages(index).ImageIndex > -1 OrElse Not String.IsNullOrEmpty(Me.TabPages(index).ImageKey)) AndAlso Me.GetTabImageRect(index).Contains(Me.MousePosition) Then
				Me.OnTabImageClick(New TabControlEventArgs(Me.TabPages(index), index, TabControlAction.Selected))

				'	Fire the base event

				MyBase.OnMouseClick(e)
			ElseIf Not Me.DesignMode AndAlso index > -1 AndAlso Me._StyleProvider.ShowTabCloser AndAlso Me.GetTabCloserRect(index).Contains(Me.MousePosition) Then

				'	If we are clicking on a closer then remove the tab instead of raising the standard mouse click event
				'	But raise the tab closing event first
				Dim tab As TabPage = Me.ActiveTab
				Dim args As New TabControlCancelEventArgs(tab, index, False, TabControlAction.Deselecting)
				Me.OnTabClosing(args)

				If Not args.Cancel Then
					Me.TabPages.Remove(tab)
					tab.Dispose()
				End If
			Else
				'	Fire the base event
				MyBase.OnMouseClick(e)
			End If
		End Sub

		Protected Overridable Sub OnTabImageClick(e As TabControlEventArgs)
			RaiseEvent TabImageClick(Me, e)
		End Sub

		Protected Overridable Sub OnTabClosing(e As TabControlCancelEventArgs)
			RaiseEvent TabClosing(Me, e)
		End Sub

		Protected Overridable Sub OnHScroll(e As ScrollEventArgs)
			'	repaint the moved tabs
			Me.Invalidate()

			'	Raise the event
			RaiseEvent HScroll(Me, e)

			If e.Type = ScrollEventType.EndScroll Then
				Me._oldValue = e.NewValue
			End If
		End Sub

		Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
			MyBase.OnMouseMove(e)
			If Me._StyleProvider.ShowTabCloser Then
				Dim tabRect As Rectangle = Me._StyleProvider.GetTabRect(Me.ActiveIndex)
				If tabRect.Contains(Me.MousePosition) Then
					Me.Invalidate()
				End If
			End If

			'	Initialise Drag Drop
			If Me.AllowDrop AndAlso e.Button = MouseButtons.Left Then
				Me.StartDragDrop()
			End If
		End Sub

		#End Region

		#Region "Basic drawing methods"

		'		private void CustomPaint(ref Message m){
		'			NativeMethods.PAINTSTRUCT paintStruct = new NativeMethods.PAINTSTRUCT();
		'			NativeMethods.BeginPaint(m.HWnd, ref paintStruct);
		'			using (Graphics screenGraphics = this.CreateGraphics()) {
		'				this.CustomPaint(screenGraphics);
		'			}
		'			NativeMethods.EndPaint(m.HWnd, ref paintStruct);
		'		}

        Private nopaintbounce As Boolean
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            ''        We must always paint the entire area of the tab control
            'If e.ClipRectangle.Equals(Me.ClientRectangle) Then
            '    Me.CustomPaint(e.Graphics)
            'Else
            '    '        it is less intensive to just reinvoke the paint with the whole surface available to draw on.
            '    Me.Invalidate()
            'End If

            If nopaintbounce Then
                Me.CustomPaint(e.Graphics)
                nopaintbounce = False
                Exit Sub
            End If
            If e.ClipRectangle.Equals(Me.ClientRectangle) Then
                Me.CustomPaint(e.Graphics)
            Else
                nopaintbounce = True
                Me.Invalidate()
            End If
        End Sub

		Private Sub CustomPaint(screenGraphics As Graphics)
			'	We render into a bitmap that is then drawn in one shot rather than using
			'	double buffering built into the control as the built in buffering
			' 	messes up the background painting.
			'	Equally the .Net 2.0 BufferedGraphics object causes the background painting
			'	to mess up, which is why we use this .Net 1.1 buffering technique.

			'	Buffer code from Gil. Schmidt http://www.codeproject.com/KB/graphics/DoubleBuffering.aspx

			If Me.Width > 0 AndAlso Me.Height > 0 Then
				If Me._BackImage Is Nothing Then
					'	Cached Background Image
					Me._BackImage = New Bitmap(Me.Width, Me.Height)
					Dim backGraphics As Graphics = Graphics.FromImage(Me._BackImage)
					backGraphics.Clear(Color.Transparent)
					Me.PaintTransparentBackground(backGraphics, Me.ClientRectangle)
				End If

				Me._BackBufferGraphics.Clear(Color.Transparent)
				Me._BackBufferGraphics.DrawImageUnscaled(Me._BackImage, 0, 0)

				Me._TabBufferGraphics.Clear(Color.Transparent)

				If Me.TabCount > 0 Then

					'	When top or bottom and scrollable we need to clip the sides from painting the tabs.
					'	Left and right are always multiline.
					If Me.Alignment <= TabAlignment.Bottom AndAlso Not Me.Multiline Then
						Me._TabBufferGraphics.Clip = New Region(New RectangleF(Me.ClientRectangle.X + 3, Me.ClientRectangle.Y, Me.ClientRectangle.Width - 6, Me.ClientRectangle.Height))
					End If

					'	Draw each tabpage from right to left.  We do it this way to handle
					'	the overlap correctly.
					If Me.Multiline Then
						For row As Integer = 0 To Me.RowCount - 1
							For index As Integer = Me.TabCount - 1 To 0 Step -1
								If index <> Me.SelectedIndex AndAlso (Me.RowCount = 1 OrElse Me.GetTabRow(index) = row) Then
									Me.DrawTabPage(index, Me._TabBufferGraphics)
								End If
							Next
						Next
					Else
						For index As Integer = Me.TabCount - 1 To 0 Step -1
							If index <> Me.SelectedIndex Then
								Me.DrawTabPage(index, Me._TabBufferGraphics)
							End If
						Next
					End If

					'	The selected tab must be drawn last so it appears on top.
					If Me.SelectedIndex > -1 Then
						Me.DrawTabPage(Me.SelectedIndex, Me._TabBufferGraphics)
					End If
				End If
				Me._TabBufferGraphics.Flush()

				'	Paint the tabs on top of the background

				' Create a new color matrix and set the alpha value to 0.5
				Dim alphaMatrix As New ColorMatrix()
				alphaMatrix.Matrix00 = InlineAssignHelper(alphaMatrix.Matrix11, InlineAssignHelper(alphaMatrix.Matrix22, InlineAssignHelper(alphaMatrix.Matrix44, 1)))
				alphaMatrix.Matrix33 = Me._StyleProvider.Opacity

				' Create a new image attribute object and set the color matrix to
				' the one just created
				Using alphaAttributes As New ImageAttributes()

					alphaAttributes.SetColorMatrix(alphaMatrix)

					' Draw the original image with the image attributes specified
					Me._BackBufferGraphics.DrawImage(Me._TabBuffer, New Rectangle(0, 0, Me._TabBuffer.Width, Me._TabBuffer.Height), 0, 0, Me._TabBuffer.Width, Me._TabBuffer.Height, _
						GraphicsUnit.Pixel, alphaAttributes)
				End Using

				Me._BackBufferGraphics.Flush()

				'	Now paint this to the screen


				'	We want to paint the whole tabstrip and border every time
				'	so that the hot areas update correctly, along with any overlaps

				'	paint the tabs etc.
				If Me.RightToLeftLayout Then
					screenGraphics.DrawImageUnscaled(Me._BackBuffer, -1, 0)
				Else
					screenGraphics.DrawImageUnscaled(Me._BackBuffer, 0, 0)
				End If
			End If
		End Sub

		Protected Sub PaintTransparentBackground(graphics As Graphics, clipRect As Rectangle)

			If (Me.Parent IsNot Nothing) Then

				'	Set the cliprect to be relative to the parent
				clipRect.Offset(Me.Location)

				'	Save the current state before we do anything.
				Dim state As GraphicsState = graphics.Save()

				'	Set the graphicsobject to be relative to the parent
				graphics.TranslateTransform(CSng(-Me.Location.X), CSng(-Me.Location.Y))
				graphics.SmoothingMode = SmoothingMode.HighSpeed

				'	Paint the parent
				Dim e As New PaintEventArgs(graphics, clipRect)
				Try
					Me.InvokePaintBackground(Me.Parent, e)
					Me.InvokePaint(Me.Parent, e)
				Finally
					'	Restore the graphics state and the clipRect to their original locations
					graphics.Restore(state)
					clipRect.Offset(-Me.Location.X, -Me.Location.Y)
				End Try
			End If
		End Sub

		Private Sub DrawTabPage(index As Integer, graphics As Graphics)
			graphics.SmoothingMode = SmoothingMode.HighSpeed

			'	Get TabPageBorder
			Using tabPageBorderPath As GraphicsPath = Me.GetTabPageBorder(index)

				'	Paint the background
				Using fillBrush As Brush = Me._StyleProvider.GetPageBackgroundBrush(index)
					graphics.FillPath(fillBrush, tabPageBorderPath)
				End Using

				If Me._Style <> TabStyle.None Then

					'	Paint the tab
					Me._StyleProvider.PaintTab(index, graphics)

					'	Draw any image
					Me.DrawTabImage(index, graphics)

					'	Draw the text

					Me.DrawTabText(index, graphics)
				End If

				'	Paint the border

				Me.DrawTabBorder(tabPageBorderPath, index, graphics)
			End Using
		End Sub

		Private Sub DrawTabBorder(path As GraphicsPath, index As Integer, graphics As Graphics)
			graphics.SmoothingMode = SmoothingMode.HighQuality
			Dim borderColor As Color
			If index = Me.SelectedIndex Then
				borderColor = Me._StyleProvider.BorderColorSelected
			ElseIf Me._StyleProvider.HotTrack AndAlso index = Me.ActiveIndex Then
				borderColor = Me._StyleProvider.BorderColorHot
			Else
				borderColor = Me._StyleProvider.BorderColor
			End If

			Using borderPen As New Pen(borderColor)
				graphics.DrawPath(borderPen, path)
			End Using
		End Sub

		Private Sub DrawTabText(index As Integer, graphics As Graphics)
			graphics.SmoothingMode = SmoothingMode.HighQuality
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
			Dim tabBounds As Rectangle = Me.GetTabTextRect(index)

			If Me.SelectedIndex = index Then
				Using textBrush As Brush = New SolidBrush(Me._StyleProvider.TextColorSelected)
					graphics.DrawString(Me.TabPages(index).Text, Me.Font, textBrush, tabBounds, Me.GetStringFormat())
				End Using
			Else
				If Me.TabPages(index).Enabled Then
					Using textBrush As Brush = New SolidBrush(Me._StyleProvider.TextColor)
						graphics.DrawString(Me.TabPages(index).Text, Me.Font, textBrush, tabBounds, Me.GetStringFormat())
					End Using
				Else
					Using textBrush As Brush = New SolidBrush(Me._StyleProvider.TextColorDisabled)
						graphics.DrawString(Me.TabPages(index).Text, Me.Font, textBrush, tabBounds, Me.GetStringFormat())
					End Using
				End If
			End If
		End Sub

		Private Sub DrawTabImage(index As Integer, graphics As Graphics)
			Dim tabImage As Image = Nothing
			If Me.TabPages(index).ImageIndex > -1 AndAlso Me.ImageList IsNot Nothing AndAlso Me.ImageList.Images.Count > Me.TabPages(index).ImageIndex Then
				tabImage = Me.ImageList.Images(Me.TabPages(index).ImageIndex)
			ElseIf (Not String.IsNullOrEmpty(Me.TabPages(index).ImageKey) AndAlso Not Me.TabPages(index).ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase)) AndAlso Me.ImageList IsNot Nothing AndAlso Me.ImageList.Images.ContainsKey(Me.TabPages(index).ImageKey) Then
				tabImage = Me.ImageList.Images(Me.TabPages(index).ImageKey)
			End If

			If tabImage IsNot Nothing Then
				If Me.RightToLeftLayout Then
					tabImage.RotateFlip(RotateFlipType.RotateNoneFlipX)
				End If
				Dim imageRect As Rectangle = Me.GetTabImageRect(index)
				If Me.TabPages(index).Enabled Then
					graphics.DrawImage(tabImage, imageRect)
				Else
					ControlPaint.DrawImageDisabled(graphics, tabImage, imageRect.X, imageRect.Y, Color.Transparent)
				End If
			End If
		End Sub

		#End Region

		#Region "String formatting"

		Private Function GetStringFormat() As StringFormat
			Dim format As StringFormat = Nothing

			'	Rotate Text by 90 degrees for left and right tabs
			Select Case Me.Alignment
				Case TabAlignment.Top, TabAlignment.Bottom
					format = New StringFormat()
					Exit Select
				Case TabAlignment.Left, TabAlignment.Right
					format = New StringFormat(StringFormatFlags.DirectionVertical)
					Exit Select
			End Select
			format.Alignment = StringAlignment.Center
			format.LineAlignment = StringAlignment.Center
			If Me.FindForm() IsNot Nothing AndAlso Me.FindForm().KeyPreview Then
				format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show
			Else
				format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide
			End If
			If Me.RightToLeft = RightToLeft.Yes Then
				format.FormatFlags = format.FormatFlags Or StringFormatFlags.DirectionRightToLeft
			End If
			Return format
		End Function

		#End Region

		#Region "Tab borders and bounds properties"

		Private Function GetTabPageBorder(index As Integer) As GraphicsPath

			Dim path As New GraphicsPath()
			Dim pageBounds As Rectangle = Me.GetPageBounds(index)
			Dim tabBounds As Rectangle = Me._StyleProvider.GetTabRect(index)
			Me._StyleProvider.AddTabBorder(path, tabBounds)
			Me.AddPageBorder(path, pageBounds, tabBounds)

			path.CloseFigure()
			Return path
		End Function

		Public Function GetPageBounds(index As Integer) As Rectangle
			Dim pageBounds As Rectangle = Me.TabPages(index).Bounds
			pageBounds.Width += 1
			pageBounds.Height += 1
			pageBounds.X -= 1
			pageBounds.Y -= 1

			If pageBounds.Bottom > Me.Height - 4 Then
				pageBounds.Height -= (pageBounds.Bottom - Me.Height + 4)
			End If
			Return pageBounds
		End Function

		Private Function GetTabTextRect(index As Integer) As Rectangle
			Dim textRect As New Rectangle()
			Using path As GraphicsPath = Me._StyleProvider.GetTabBorder(index)
				Dim tabBounds As RectangleF = path.GetBounds()

				textRect = New Rectangle(CInt(Math.Truncate(tabBounds.X)), CInt(Math.Truncate(tabBounds.Y)), CInt(Math.Truncate(tabBounds.Width)), CInt(Math.Truncate(tabBounds.Height)))

				'	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
				Select Case Me.Alignment
					Case TabAlignment.Top
						textRect.Y += 4
						textRect.Height -= 6
						Exit Select
					Case TabAlignment.Bottom
						textRect.Y += 2
						textRect.Height -= 6
						Exit Select
					Case TabAlignment.Left
						textRect.X += 4
						textRect.Width -= 6
						Exit Select
					Case TabAlignment.Right
						textRect.X += 2
						textRect.Width -= 6
						Exit Select
				End Select

				'	If there is an image allow for it
				If Me.ImageList IsNot Nothing AndAlso (Me.TabPages(index).ImageIndex > -1 OrElse (Not String.IsNullOrEmpty(Me.TabPages(index).ImageKey) AndAlso Not Me.TabPages(index).ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase))) Then
					Dim imageRect As Rectangle = Me.GetTabImageRect(index)
					If (Me._StyleProvider.ImageAlign And NativeMethods.AnyLeftAlign) <> CType(0, ContentAlignment) Then
						If Me.Alignment <= TabAlignment.Bottom Then
							textRect.X = imageRect.Right + 4
							textRect.Width -= (textRect.Right - CInt(Math.Truncate(tabBounds.Right)))
						Else
							textRect.Y = imageRect.Y + 4
							textRect.Height -= (textRect.Bottom - CInt(Math.Truncate(tabBounds.Bottom)))
						End If
						'	If there is a closer allow for it
						If Me._StyleProvider.ShowTabCloser Then
							Dim closerRect As Rectangle = Me.GetTabCloserRect(index)
							If Me.Alignment <= TabAlignment.Bottom Then
								If Me.RightToLeftLayout Then
									textRect.Width -= (closerRect.Right + 4 - textRect.X)
									textRect.X = closerRect.Right + 4
								Else
									textRect.Width -= (CInt(Math.Truncate(tabBounds.Right)) - closerRect.X + 4)
								End If
							Else
								If Me.RightToLeftLayout Then
									textRect.Height -= (closerRect.Bottom + 4 - textRect.Y)
									textRect.Y = closerRect.Bottom + 4
								Else
									textRect.Height -= (CInt(Math.Truncate(tabBounds.Bottom)) - closerRect.Y + 4)
								End If
							End If
						End If
					ElseIf (Me._StyleProvider.ImageAlign And NativeMethods.AnyCenterAlign) <> CType(0, ContentAlignment) Then
						'	If there is a closer allow for it
						If Me._StyleProvider.ShowTabCloser Then
							Dim closerRect As Rectangle = Me.GetTabCloserRect(index)
							If Me.Alignment <= TabAlignment.Bottom Then
								If Me.RightToLeftLayout Then
									textRect.Width -= (closerRect.Right + 4 - textRect.X)
									textRect.X = closerRect.Right + 4
								Else
									textRect.Width -= (CInt(Math.Truncate(tabBounds.Right)) - closerRect.X + 4)
								End If
							Else
								If Me.RightToLeftLayout Then
									textRect.Height -= (closerRect.Bottom + 4 - textRect.Y)
									textRect.Y = closerRect.Bottom + 4
								Else
									textRect.Height -= (CInt(Math.Truncate(tabBounds.Bottom)) - closerRect.Y + 4)
								End If
							End If
						End If
					Else
						If Me.Alignment <= TabAlignment.Bottom Then
							textRect.Width -= (CInt(Math.Truncate(tabBounds.Right)) - imageRect.X + 4)
						Else
							textRect.Height -= (CInt(Math.Truncate(tabBounds.Bottom)) - imageRect.Y + 4)
						End If
						'	If there is a closer allow for it
						If Me._StyleProvider.ShowTabCloser Then
							Dim closerRect As Rectangle = Me.GetTabCloserRect(index)
							If Me.Alignment <= TabAlignment.Bottom Then
								If Me.RightToLeftLayout Then
									textRect.Width -= (closerRect.Right + 4 - textRect.X)
									textRect.X = closerRect.Right + 4
								Else
									textRect.Width -= (CInt(Math.Truncate(tabBounds.Right)) - closerRect.X + 4)
								End If
							Else
								If Me.RightToLeftLayout Then
									textRect.Height -= (closerRect.Bottom + 4 - textRect.Y)
									textRect.Y = closerRect.Bottom + 4
								Else
									textRect.Height -= (CInt(Math.Truncate(tabBounds.Bottom)) - closerRect.Y + 4)
								End If
							End If
						End If
					End If
				Else
					'	If there is a closer allow for it
					If Me._StyleProvider.ShowTabCloser Then
						Dim closerRect As Rectangle = Me.GetTabCloserRect(index)
						If Me.Alignment <= TabAlignment.Bottom Then
							If Me.RightToLeftLayout Then
								textRect.Width -= (closerRect.Right + 4 - textRect.X)
								textRect.X = closerRect.Right + 4
							Else
								textRect.Width -= (CInt(Math.Truncate(tabBounds.Right)) - closerRect.X + 4)
							End If
						Else
							If Me.RightToLeftLayout Then
								textRect.Height -= (closerRect.Bottom + 4 - textRect.Y)
								textRect.Y = closerRect.Bottom + 4
							Else
								textRect.Height -= (CInt(Math.Truncate(tabBounds.Bottom)) - closerRect.Y + 4)
							End If
						End If
					End If
				End If


				'	Ensure it fits inside the path at the centre line
				If Me.Alignment <= TabAlignment.Bottom Then
					While Not path.IsVisible(textRect.Right, textRect.Y) AndAlso textRect.Width > 0
						textRect.Width -= 1
					End While
					While Not path.IsVisible(textRect.X, textRect.Y) AndAlso textRect.Width > 0
						textRect.X += 1
						textRect.Width -= 1
					End While
				Else
					While Not path.IsVisible(textRect.X, textRect.Bottom) AndAlso textRect.Height > 0
						textRect.Height -= 1
					End While
					While Not path.IsVisible(textRect.X, textRect.Y) AndAlso textRect.Height > 0
						textRect.Y += 1
						textRect.Height -= 1
					End While
				End If
			End Using
			Return textRect
		End Function

		Public Function GetTabRow(index As Integer) As Integer
			'	All calculations will use this rect as the base point
			'	because the itemsize does not return the correct width.
			Dim rect As Rectangle = Me.GetTabRect(index)

			Dim row As Integer = -1

			Select Case Me.Alignment
				Case TabAlignment.Top
					row = (rect.Y - 2) \ rect.Height
					Exit Select
				Case TabAlignment.Bottom
					row = ((Me.Height - rect.Y - 2) \ rect.Height) - 1
					Exit Select
				Case TabAlignment.Left
					row = (rect.X - 2) \ rect.Width
					Exit Select
				Case TabAlignment.Right
					row = ((Me.Width - rect.X - 2) \ rect.Width) - 1
					Exit Select
			End Select
			Return row
		End Function

		Public Function GetTabPosition(index As Integer) As Point

			'	If we are not multiline then the column is the index and the row is 0.
			If Not Me.Multiline Then
				Return New Point(0, index)
			End If

			'	If there is only one row then the column is the index
			If Me.RowCount = 1 Then
				Return New Point(0, index)
			End If

			'	We are in a true multi-row scenario
			Dim row As Integer = Me.GetTabRow(index)
			Dim rect As Rectangle = Me.GetTabRect(index)
			Dim column As Integer = -1

			'	Scan from left to right along rows, skipping to next row if it is not the one we want.
			For testIndex As Integer = 0 To Me.TabCount - 1
				Dim testRect As Rectangle = Me.GetTabRect(testIndex)
				If Me.Alignment <= TabAlignment.Bottom Then
					If testRect.Y = rect.Y Then
						column += 1
					End If
				Else
					If testRect.X = rect.X Then
						column += 1
					End If
				End If

				If testRect.Location.Equals(rect.Location) Then
					Return New Point(row, column)
				End If
			Next

			Return New Point(0, 0)
		End Function

		Public Function IsFirstTabInRow(index As Integer) As Boolean
			If index < 0 Then
				Return False
			End If
			Dim firstTabinRow As Boolean = (index = 0)
			If Not firstTabinRow Then
				If Me.Alignment <= TabAlignment.Bottom Then
					If Me.GetTabRect(index).X = 2 Then
						firstTabinRow = True
					End If
				Else
					If Me.GetTabRect(index).Y = 2 Then
						firstTabinRow = True
					End If
				End If
			End If
			Return firstTabinRow
		End Function

		Private Sub AddPageBorder(path As GraphicsPath, pageBounds As Rectangle, tabBounds As Rectangle)
			Select Case Me.Alignment
				Case TabAlignment.Top
					path.AddLine(tabBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Y)
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom)
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom)
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y)
					path.AddLine(pageBounds.X, pageBounds.Y, tabBounds.X, pageBounds.Y)
					Exit Select
				Case TabAlignment.Bottom
					path.AddLine(tabBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom)
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y)
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y)
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom)
					path.AddLine(pageBounds.Right, pageBounds.Bottom, tabBounds.Right, pageBounds.Bottom)
					Exit Select
				Case TabAlignment.Left
					path.AddLine(pageBounds.X, tabBounds.Y, pageBounds.X, pageBounds.Y)
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y)
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom)
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom)
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, tabBounds.Bottom)
					Exit Select
				Case TabAlignment.Right
					path.AddLine(pageBounds.Right, tabBounds.Bottom, pageBounds.Right, pageBounds.Bottom)
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom)
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y)
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y)
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, tabBounds.Y)
					Exit Select
			End Select
		End Sub

		Private Function GetTabImageRect(index As Integer) As Rectangle
			Using tabBorderPath As GraphicsPath = Me._StyleProvider.GetTabBorder(index)
				Return Me.GetTabImageRect(tabBorderPath)
			End Using
		End Function
		Private Function GetTabImageRect(tabBorderPath As GraphicsPath) As Rectangle
			Dim imageRect As New Rectangle()
			Dim rect As RectangleF = tabBorderPath.GetBounds()

			'	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
			Select Case Me.Alignment
				Case TabAlignment.Top
					rect.Y += 4
					rect.Height -= 6
					Exit Select
				Case TabAlignment.Bottom
					rect.Y += 2
					rect.Height -= 6
					Exit Select
				Case TabAlignment.Left
					rect.X += 4
					rect.Width -= 6
					Exit Select
				Case TabAlignment.Right
					rect.X += 2
					rect.Width -= 6
					Exit Select
			End Select

			'	Ensure image is fully visible
			If Me.Alignment <= TabAlignment.Bottom Then
				If (Me._StyleProvider.ImageAlign And NativeMethods.AnyLeftAlign) <> CType(0, ContentAlignment) Then
					imageRect = New Rectangle(CInt(Math.Truncate(rect.X)), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Height)) - 16) / 2))), 16, 16)
					While Not tabBorderPath.IsVisible(imageRect.X, imageRect.Y)
						imageRect.X += 1
					End While

					imageRect.X += 4
				ElseIf (Me._StyleProvider.ImageAlign And NativeMethods.AnyCenterAlign) <> CType(0, ContentAlignment) Then
					imageRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl((CInt(Math.Truncate(rect.Right)) - CInt(Math.Truncate(rect.X)) - CInt(Math.Truncate(rect.Height)) + 2) \ 2)))), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Height)) - 16) / 2))), 16, 16)
				Else
					imageRect = New Rectangle(CInt(Math.Truncate(rect.Right)), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Height)) - 16) / 2))), 16, 16)
					While Not tabBorderPath.IsVisible(imageRect.Right, imageRect.Y)
						imageRect.X -= 1
					End While
					imageRect.X -= 4

					'	Move it in further to allow for the tab closer
					If Me._StyleProvider.ShowTabCloser AndAlso Not Me.RightToLeftLayout Then
						imageRect.X -= 10
					End If
				End If
			Else
				If (Me._StyleProvider.ImageAlign And NativeMethods.AnyLeftAlign) <> CType(0, ContentAlignment) Then
					imageRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Width)) - 16) / 2))), CInt(Math.Truncate(rect.Y)), 16, 16)
					While Not tabBorderPath.IsVisible(imageRect.X, imageRect.Y)
						imageRect.Y += 1
					End While
					imageRect.Y += 4
				ElseIf (Me._StyleProvider.ImageAlign And NativeMethods.AnyCenterAlign) <> CType(0, ContentAlignment) Then
					imageRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Width)) - 16) / 2))), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl((CInt(Math.Truncate(rect.Bottom)) - CInt(Math.Truncate(rect.Y)) - CInt(Math.Truncate(rect.Width)) + 2) \ 2)))), 16, 16)
				Else
					imageRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Width)) - 16) / 2))), CInt(Math.Truncate(rect.Bottom)), 16, 16)
					While Not tabBorderPath.IsVisible(imageRect.X, imageRect.Bottom)
						imageRect.Y -= 1
					End While
					imageRect.Y -= 4

					'	Move it in further to allow for the tab closer
					If Me._StyleProvider.ShowTabCloser AndAlso Not Me.RightToLeftLayout Then
						imageRect.Y -= 10
					End If
				End If
			End If
			Return imageRect
		End Function

		Public Function GetTabCloserRect(index As Integer) As Rectangle
			Dim closerRect As New Rectangle()
			Using path As GraphicsPath = Me._StyleProvider.GetTabBorder(index)
				Dim rect As RectangleF = path.GetBounds()

				'	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
				Select Case Me.Alignment
					Case TabAlignment.Top
						rect.Y += 4
						rect.Height -= 6
						Exit Select
					Case TabAlignment.Bottom
						rect.Y += 2
						rect.Height -= 6
						Exit Select
					Case TabAlignment.Left
						rect.X += 4
						rect.Width -= 6
						Exit Select
					Case TabAlignment.Right
						rect.X += 2
						rect.Width -= 6
						Exit Select
				End Select
				If Me.Alignment <= TabAlignment.Bottom Then
					If Me.RightToLeftLayout Then
						closerRect = New Rectangle(CInt(Math.Truncate(rect.Left)), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Height)) - 6) / 2))), 6, 6)
						While Not path.IsVisible(closerRect.Left, closerRect.Y) AndAlso closerRect.Right < Me.Width
							closerRect.X += 1
						End While
						closerRect.X += 4
					Else
						closerRect = New Rectangle(CInt(Math.Truncate(rect.Right)), CInt(Math.Truncate(rect.Y)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Height)) - 6) / 2))), 6, 6)
						While Not path.IsVisible(closerRect.Right, closerRect.Y) AndAlso closerRect.Right > -6
							closerRect.X -= 1
						End While
						closerRect.X -= 4
					End If
				Else
					If Me.RightToLeftLayout Then
						closerRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Width)) - 6) / 2))), CInt(Math.Truncate(rect.Top)), 6, 6)
						While Not path.IsVisible(closerRect.X, closerRect.Top) AndAlso closerRect.Bottom < Me.Height
							closerRect.Y += 1
						End While
						closerRect.Y += 4
					Else
						closerRect = New Rectangle(CInt(Math.Truncate(rect.X)) + CInt(Math.Truncate(Math.Floor(CDbl(CInt(Math.Truncate(rect.Width)) - 6) / 2))), CInt(Math.Truncate(rect.Bottom)), 6, 6)
						While Not path.IsVisible(closerRect.X, closerRect.Bottom) AndAlso closerRect.Top > -6
							closerRect.Y -= 1
						End While
						closerRect.Y -= 4
					End If
				End If
			End Using
			Return closerRect
		End Function

		Public Shadows ReadOnly Property MousePosition() As Point
			Get
				Dim loc As Point = Me.PointToClient(Control.MousePosition)
				If Me.RightToLeftLayout Then
					loc.X = (Me.Width - loc.X)
				End If
				Return loc
			End Get
		End Property
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function

		#End Region

	End Class
End Namespace
