' Copyright © Neil Price 2006
' Free for use
' Just leave this message intact

Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing

''' <summary>
''' Derived ComboBox that shows current position of the cursor inside the drop down
''' as it's actually moving around in there.  This combobox will raise the custom
''' event we have defined above whenever a new item is hovered over in the drop down list
''' </summary>
Public Class hoverComboBox
    Inherits ComboBox
    ' Internal variables to hold the mouse position and the control
    ' position for calculating whether the mouse is inside the drop down
    ' and whether we scrolled with the mouse inside the drop down
    Private yPos As Integer = 0
    Private xPos As Integer = 0
    Private scrollPos As Integer = 0
    Private xFactor As Integer = -1
    Private simpleOffset As Integer = 0

    ' Import the GetScrollInfo function from user32.dll
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetScrollInfo(hWnd As IntPtr, n As Integer, ByRef lpScrollInfo As ScrollInfoStruct) As Integer
    End Function

    ' Win32 constants
    Private Const SB_VERT As Integer = 1
    Private Const SIF_TRACKPOS As Integer = &H10
    Private Const SIF_RANGE As Integer = &H1
    Private Const SIF_POS As Integer = &H4
    Private Const SIF_PAGE As Integer = &H2
    Private Const SIF_ALL As Integer = SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS

    Private Const SCROLLBAR_WIDTH As Integer = 17
    Private Const LISTBOX_YOFFSET As Integer = 21

    ' Return structure for the GetScrollInfo method
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure ScrollInfoStruct
        Public cbSize As Integer
        Public fMask As Integer
        Public nMin As Integer
        Public nMax As Integer
        Public nPage As Integer
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure

    Public Event Hover As HoverEventHandler

    Protected Overridable Sub OnHover(e As HoverEventArgs)
        'Dim handler As HoverEventHandler = Hover
        ' Invokes the delegates. 
        RaiseEvent Hover(Me, e)
    End Sub

    'Capture messages coming to our combobox
    Protected Overrides Sub WndProc(ByRef msg As Message)
        'This message code indicates the value in the list is changing
        '32 is for DropDownStyle == Simple
        If (msg.Msg = 308) OrElse (msg.Msg = 32) Then
            Dim onScreenIndex As Integer = 0

            ' Get the mouse position relative to this control
            Dim LocalMousePosition As Point = Me.PointToClient(Cursor.Position)
            xPos = LocalMousePosition.X

            If Me.DropDownStyle = ComboBoxStyle.Simple Then
                yPos = LocalMousePosition.Y - (Me.ItemHeight + 10)
            Else
                yPos = LocalMousePosition.Y - Me.Size.Height - 1
            End If

            ' save our y position which we need to ensure the cursor is
            ' inside the drop down list for updating purposes
            Dim oldYPos As Integer = yPos

            ' get the 0-based index of where the cursor is on screen
            ' as if it were inside the listbox
            While yPos >= Me.ItemHeight
                yPos -= Me.ItemHeight
                onScreenIndex += 1
            End While

            'if (yPos < 0) { onScreenIndex = -1; }
            Dim si As New ScrollInfoStruct()
            si.fMask = SIF_ALL
            si.cbSize = Marshal.SizeOf(si)
            ' msg.LParam holds the hWnd to the drop down list that appears
            Dim getScrollInfoResult As Integer = 0
            getScrollInfoResult = GetScrollInfo(msg.LParam, SB_VERT, si)

            ' k returns 0 on error, so if there is no error add the current
            ' track position of the scrollbar to our index
            If getScrollInfoResult > 0 Then
                onScreenIndex += si.nTrackPos

                If Me.DropDownStyle = ComboBoxStyle.Simple Then
                    simpleOffset = si.nTrackPos
                End If
            End If

            ' Add our offset modifier if we're a simple combobox since we don't
            ' continuously receive scrollbar information in this mode.
            ' Then make sure the item we're previewing is actually on screen.
            If Me.DropDownStyle = ComboBoxStyle.Simple Then
                onScreenIndex += simpleOffset
                If onScreenIndex > ((Me.DropDownHeight \ Me.ItemHeight) + simpleOffset) Then
                    onScreenIndex = ((Me.DropDownHeight \ Me.ItemHeight) + simpleOffset - 1)
                End If
            End If

            ' Check we're actually inside the drop down window that appears and 
            ' not just over its scrollbar before we actually try to update anything
            ' then if we are raise the Hover event for this comboBox
            'If Not (xPos > Me.Width - SCROLLBAR_WIDTH OrElse xPos < 1 OrElse oldYPos < 0) Then
            If Not (xPos > Me.Width - SCROLLBAR_WIDTH OrElse xPos < 1 OrElse oldYPos < 0 OrElse ((oldYPos > Me.ItemHeight * Me.MaxDropDownItems) AndAlso Me.DropDownStyle <> ComboBoxStyle.Simple)) Then
                Dim e As New HoverEventArgs()
                e.itemIndex = If((onScreenIndex > Me.Items.Count - 1), Me.Items.Count - 1, onScreenIndex)
                OnHover(e)
                ' if scrollPos doesn't equal the nPos from our ScrollInfoStruct then
                ' the mousewheel was most likely used to scroll the drop down list
                ' while the mouse was inside it - this means we have to manually
                ' tell the drop down to repaint itself to update where it is hovering
                ' still posible to "outscroll" this method but it works better than
                ' without it present
                If scrollPos <> si.nPos Then
                    Cursor.Position = New Point(Cursor.Position.X + xFactor, Cursor.Position.Y)
                    xFactor = -xFactor
                End If
            End If
            scrollPos = si.nPos
        End If
        ' Pass on our message
        MyBase.WndProc(msg)
    End Sub
End Class

''' <summary>
''' Class that contains data for the hover event 
''' </summary>
Public Class HoverEventArgs
    Inherits EventArgs
    Private _itemIndex As Integer = 0
    Public Property itemIndex() As Integer
        Get
            Return _itemIndex
        End Get
        Set(value As Integer)
            _itemIndex = Value
        End Set
    End Property
End Class

''' <summary>
''' Delegate declaration 
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
Public Delegate Sub HoverEventHandler(sender As Object, e As HoverEventArgs)