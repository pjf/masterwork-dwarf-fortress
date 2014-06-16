#region License LGPL 3
// Copyright © Łukasz Świątkowski 2007–2010.
// http://www.lukesw.net/
//
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using VS = System.Windows.Forms.VisualStyles;

/*
<li>Base class for custom tooltips.</li>
<li>Office-2007-like tooltip class.</li>
*/
namespace PresentationControls
{
    /// <summary>
    /// Represents a pop-up window.
    /// </summary>
    [CLSCompliant(true), ToolboxItem(false)]
    public partial class Popup : ToolStripDropDown
    {
        #region " Fields & Properties "

        /// <summary>
        /// Gets the content of the pop-up.
        /// </summary>
        public Control Content { get; private set; }

        /// <summary>
        /// Determines which animation to use while showing the pop-up window.
        /// </summary>
        //public PopupAnimations ShowingAnimation { get; set; }

        /// <summary>
        /// Determines which animation to use while hiding the pop-up window.
        /// </summary>
        //public PopupAnimations HidingAnimation { get; set; }

        /// <summary>
        /// Determines the duration of the animation.
        /// </summary>
        public int AnimationDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content should receive the focus after the pop-up has been opened.
        /// </summary>
        /// <value><c>true</c> if the content should be focused after the pop-up has been opened; otherwise, <c>false</c>.</value>
        /// <remarks>If the FocusOnOpen property is set to <c>false</c>, then pop-up cannot use the fade effect.</remarks>
        public bool FocusOnOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pressing the alt key should close the pop-up.
        /// </summary>
        /// <value><c>true</c> if pressing the alt key does not close the pop-up; otherwise, <c>false</c>.</value>
        public bool AcceptAlt { get; set; }

        private ToolStripControlHost _host;
        private Control _opener;
        private Popup _ownerPopup;
        private Popup _childPopup;
        private bool _resizableTop;
        private bool _resizableLeft;

        private bool _isChildPopupOpened;
        private bool _resizable;
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PopupControl.Popup" /> is resizable.
        /// </summary>
        /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
        public bool Resizable
        {
            get { return _resizable && !_isChildPopupOpened; }
            set { _resizable = value; }
        }

        private bool _nonInteractive;
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PopupControl.Popup"></see> acts like a transparent windows (so it cannot be clicked).
        /// </summary>
        /// <value>
        /// <c>true</c> if the popup is noninteractive; otherwise, <c>false</c>.</value>
        public bool NonInteractive
        {
            get { return _nonInteractive; }
            set
            {
                if (value != _nonInteractive)
                {
                    _nonInteractive = value;
                    if (IsHandleCreated) RecreateHandle();
                }
            }
        }

        /// <summary>
        /// Gets or sets a minimum size of the pop-up.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets a maximum size of the pop-up.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MaximumSize { get; set; }

        /// <summary>
        /// Gets parameters of a new window.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Windows.Forms.CreateParams" /> used when creating a new window.</returns>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
                if (NonInteractive) cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        #endregion

        #region " Constructors "

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl.Popup"/> class.
        /// </summary>
        /// <param name="content">The content of the pop-up.</param>
        /// <remarks>
        /// Pop-up will be disposed immediately after disposion of the content control.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="content" /> is <code>null</code>.</exception>
        public Popup(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            Content = content;
            FocusOnOpen = true;
            AcceptAlt = true;
            //ShowingAnimation = PopupAnimations.SystemDefault;
            //HidingAnimation = PopupAnimations.None;
            AnimationDuration = 100;
            InitializeComponent();
            AutoSize = false;
            DoubleBuffered = true;
            ResizeRedraw = true;
            _host = new ToolStripControlHost(content);
            Padding = Margin = _host.Padding = _host.Margin = Padding.Empty;
            if (NativeMethods.IsRunningOnMono) content.Margin = Padding.Empty;
            MinimumSize = content.MinimumSize;
            content.MinimumSize = content.Size;
            MaximumSize = content.MaximumSize;
            content.MaximumSize = content.Size;
            Size = content.Size;
            if (NativeMethods.IsRunningOnMono) _host.Size = content.Size;
            TabStop = content.TabStop = true;
            content.Location = Point.Empty;
            Items.Add(_host);
            content.Disposed += (sender, e) =>
            {
                content = null;
                Dispose(true);
            };
            content.RegionChanged += (sender, e) => UpdateRegion();
            content.Paint += (sender, e) => PaintSizeGrip(e);
            UpdateRegion();
        }

        #endregion

        #region " Methods "

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            //if (NativeMethods.IsRunningOnMono) return; // in case of non-Windows
            //if ((Visible && ShowingAnimation == PopupAnimations.None) || (!Visible && HidingAnimation == PopupAnimations.None))
            //{
            //    return;
            //}
            //NativeMethods.AnimationFlags flags = Visible ? NativeMethods.AnimationFlags.Roll : NativeMethods.AnimationFlags.Hide;
            //PopupAnimations _flags = Visible ? ShowingAnimation : HidingAnimation;
            //if (_flags == PopupAnimations.SystemDefault)
            //{
            //    if (SystemInformation.IsMenuAnimationEnabled)
            //    {
            //        if (SystemInformation.IsMenuFadeEnabled)
            //        {
            //            _flags = PopupAnimations.Blend;
            //        }
            //        else
            //        {
            //            _flags = PopupAnimations.Slide | (Visible ? PopupAnimations.TopToBottom : PopupAnimations.BottomToTop);
            //        }
            //    }
            //    else
            //    {
            //        _flags = PopupAnimations.None;
            //    }
            //}
            //if ((_flags & (PopupAnimations.Blend | PopupAnimations.Center | PopupAnimations.Roll | PopupAnimations.Slide)) == PopupAnimations.None)
            //{
            //    return;
            //}
            //if (_resizableTop) // popup is “inverted”, so the animation must be
            //{
            //    if ((_flags & PopupAnimations.BottomToTop) != PopupAnimations.None)
            //    {
            //        _flags = (_flags & ~PopupAnimations.BottomToTop) | PopupAnimations.TopToBottom;
            //    }
            //    else if ((_flags & PopupAnimations.TopToBottom) != PopupAnimations.None)
            //    {
            //        _flags = (_flags & ~PopupAnimations.TopToBottom) | PopupAnimations.BottomToTop;
            //    }
            //}
            //if (_resizableLeft) // popup is “inverted”, so the animation must be
            //{
            //    if ((_flags & PopupAnimations.RightToLeft) != PopupAnimations.None)
            //    {
            //        _flags = (_flags & ~PopupAnimations.RightToLeft) | PopupAnimations.LeftToRight;
            //    }
            //    else if ((_flags & PopupAnimations.LeftToRight) != PopupAnimations.None)
            //    {
            //        _flags = (_flags & ~PopupAnimations.LeftToRight) | PopupAnimations.RightToLeft;
            //    }
            //}
            //flags = flags | (NativeMethods.AnimationFlags.Mask & (NativeMethods.AnimationFlags)(int)_flags);
            //NativeMethods.SetTopMost(this);
            //NativeMethods.AnimateWindow(this, AnimationDuration, flags);
        }

        /// <summary>
        /// Processes a dialog box key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (AcceptAlt && ((keyData & Keys.Alt) == Keys.Alt))
            {
                if ((keyData & Keys.F4) != Keys.F4)
                {
                    return false;
                }
                else
                {
                    Close();
                }
            }
            bool processed = base.ProcessDialogKey(keyData);
            if (!processed && (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift)))
            {
                bool backward = (keyData & Keys.Shift) == Keys.Shift;
                Content.SelectNextControl(null, !backward, true, true, true);
            }
            return processed;
        }

        /// <summary>
        /// Updates the pop-up region.
        /// </summary>
        protected void UpdateRegion()
        {
            if (Region != null)
            {
                Region.Dispose();
                Region = null;
            }
            if (Content.Region != null)
            {
                Region = Content.Region.Clone();
            }
        }

        /// <summary>
        /// Shows the pop-up window below the specified control.
        /// </summary>
        /// <param name="control">The control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below the specified control, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            Show(control, control.ClientRectangle);
        }

        /// <summary>
        /// Shows the pop-up window below the specified area.
        /// </summary>
        /// <param name="area">The area of desktop below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below specified area, the pop-up control is shown above it.
        /// </remarks>
        public void Show(Rectangle area)
        {
            _resizableTop = _resizableLeft = false;
            Point location = new Point(area.Left, area.Top + area.Height);
            Rectangle screen = Screen.FromControl(this).WorkingArea;
            if (location.X + Size.Width > (screen.Left + screen.Width))
            {
                _resizableLeft = true;
                location.X = (screen.Left + screen.Width) - Size.Width;
            }
            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                _resizableTop = true;
                location.Y -= Size.Height + area.Height;
            }
            //location = control.PointToClient(location);
            Show(location, ToolStripDropDownDirection.BelowRight);
        }

        /// <summary>
        /// Shows the pop-up window below the specified area of the specified control.
        /// </summary>
        /// <param name="control">The control used to compute screen location of specified area.</param>
        /// <param name="area">The area of control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below specified area, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control, Rectangle area)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);

            _resizableTop = _resizableLeft = false;
            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            Rectangle screen = Screen.FromControl(control).WorkingArea;
            if (location.X + Size.Width > (screen.Left + screen.Width))
            {
                _resizableLeft = true;
                location.X = (screen.Left + screen.Width) - Size.Width;
            }
            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                _resizableTop = true;
                location.Y -= Size.Height + area.Height;
            }
            location = control.PointToClient(location);
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        private void SetOwnerItem(Control control)
        {
            if (control == null)
            {
                return;
            }
            if (control is Popup)
            {
                Popup popupControl = control as Popup;
                _ownerPopup = popupControl;
                _ownerPopup._childPopup = this;
                OwnerItem = popupControl.Items[0];
                return;
            }
            else if (_opener == null)
            {
                _opener = control;
            }
            if (control.Parent != null)
            {
                SetOwnerItem(control.Parent);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (Content != null)
            {
                Content.MinimumSize = Size;
                Content.MaximumSize = Size;
                Content.Size = Size;
                Content.Location = Point.Empty;
            }
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
        protected override void OnLayout(LayoutEventArgs e)
        {
            if (!NativeMethods.IsRunningOnMono)
            {
                base.OnLayout(e);
                return;
            }
            Size suggestedSize = GetPreferredSize(Size.Empty);
            if (AutoSize && suggestedSize != Size)
            {
                Size = suggestedSize;
            }
            SetDisplayedItems();
            OnLayoutCompleted(EventArgs.Empty);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opening" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnOpening(CancelEventArgs e)
        {
            if (Content.IsDisposed || Content.Disposing)
            {
                e.Cancel = true;
                return;
            }
            UpdateRegion();
            base.OnOpening(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opened" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnOpened(EventArgs e)
        {
            if (_ownerPopup != null)
            {
                _ownerPopup._isChildPopupOpened = true;
            }
            if (FocusOnOpen)
            {
                Content.Focus();
            }
            base.OnOpened(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Closed"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripDropDownClosedEventArgs"/> that contains the event data.</param>
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            _opener = null;
            if (_ownerPopup != null)
            {
                _ownerPopup._isChildPopupOpened = false;
            }
            base.OnClosed(e);
        }

        #endregion

        #region " Resizing Support "

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            //if (m.Msg == NativeMethods.WM_PRINT && !Visible)
            //{
            //    Visible = true;
            //}
            if (InternalProcessResizing(ref m, false))
            {
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Processes the resizing messages.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessResizing(ref Message m)
        {
            return InternalProcessResizing(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool InternalProcessResizing(ref Message m, bool contentControl)
        {
            if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam != IntPtr.Zero && _childPopup != null && _childPopup.Visible)
            {
                _childPopup.Hide();
            }
            if (!Resizable && !NonInteractive)
            {
                return false;
            }
            if (m.Msg == NativeMethods.WM_NCHITTEST)
            {
                return OnNcHitTest(ref m, contentControl);
            }
            else if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
            {
                return OnGetMinMaxInfo(ref m);
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            NativeMethods.MINMAXINFO minmax = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
            if (!MaximumSize.IsEmpty)
            {
                minmax.maxTrackSize = MaximumSize;
            }
            minmax.minTrackSize = MinimumSize;
            Marshal.StructureToPtr(minmax, m.LParam, false);
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            if (NonInteractive)
            {
                m.Result = (IntPtr)NativeMethods.HTTRANSPARENT;
                return true;
            }

            int x = Cursor.Position.X; // NativeMethods.LOWORD(m.LParam);
            int y = Cursor.Position.Y; // NativeMethods.HIWORD(m.LParam);
            Point clientLocation = PointToClient(new Point(x, y));

            GripBounds gripBouns = new GripBounds(contentControl ? Content.ClientRectangle : ClientRectangle);
            IntPtr transparent = new IntPtr(NativeMethods.HTTRANSPARENT);

            if (_resizableTop)
            {
                if (_resizableLeft && gripBouns.TopLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPLEFT;
                    return true;
                }
                if (!_resizableLeft && gripBouns.TopRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPRIGHT;
                    return true;
                }
                if (gripBouns.Top.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOP;
                    return true;
                }
            }
            else
            {
                if (_resizableLeft && gripBouns.BottomLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMLEFT;
                    return true;
                }
                if (!_resizableLeft && gripBouns.BottomRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return true;
                }
                if (gripBouns.Bottom.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOM;
                    return true;
                }
            }
            if (_resizableLeft && gripBouns.Left.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTLEFT;
                return true;
            }
            if (!_resizableLeft && gripBouns.Right.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTRIGHT;
                return true;
            }
            return false;
        }

        private VS.VisualStyleRenderer _sizeGripRenderer;
        /// <summary>
        /// Paints the sizing grip.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
        public void PaintSizeGrip(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null || !_resizable)
            {
                return;
            }
            Size clientSize = Content.ClientSize;
            using (Bitmap gripImage = new Bitmap(0x10, 0x10))
            {
                using (Graphics g = Graphics.FromImage(gripImage))
                {
                    if (Application.RenderWithVisualStyles)
                    {
                        if (_sizeGripRenderer == null)
                        {
                            _sizeGripRenderer = new VS.VisualStyleRenderer(VS.VisualStyleElement.Status.Gripper.Normal);
                        }
                        _sizeGripRenderer.DrawBackground(g, new Rectangle(0, 0, 0x10, 0x10));
                    }
                    else
                    {
                        ControlPaint.DrawSizeGrip(g, Content.BackColor, 0, 0, 0x10, 0x10);
                    }
                }
                GraphicsState gs = e.Graphics.Save();
                e.Graphics.ResetTransform();
                if (_resizableTop)
                {
                    if (_resizableLeft)
                    {
                        e.Graphics.RotateTransform(180);
                        e.Graphics.TranslateTransform(-clientSize.Width, -clientSize.Height);
                    }
                    else
                    {
                        e.Graphics.ScaleTransform(1, -1);
                        e.Graphics.TranslateTransform(0, -clientSize.Height);
                    }
                }
                else if (_resizableLeft)
                {
                    e.Graphics.ScaleTransform(-1, 1);
                    e.Graphics.TranslateTransform(-clientSize.Width, 0);
                }
                e.Graphics.DrawImage(gripImage, clientSize.Width - 0x10, clientSize.Height - 0x10 + 1, 0x10, 0x10);
                e.Graphics.Restore(gs);
            }
        }

        #endregion
    }
}







//using System;
//using System.ComponentModel;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Security.Permissions;
//using System.Windows.Forms;
//using VS = System.Windows.Forms.VisualStyles;

///*
//<li>Base class for custom tooltips.</li>
//<li>Office-2007-like tooltip class.</li>
//*/
//namespace PresentationControls
//{
//    /// <summary>
//    /// CodeProject.com "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp".
//    /// Represents a pop-up window.
//    /// </summary>
//    [CLSCompliant(true), ToolboxItem(false)]
//    public partial class Popup : ToolStripDropDown
//    {
//        #region " Fields & Properties "

//        private Control content;
//        /// <summary>
//        /// Gets the content of the pop-up.
//        /// </summary>
//        public Control Content
//        {
//            get { return content; }
//        }

//        private bool fade;
//        /// <summary>
//        /// Gets a value indicating whether the <see cref="PopupControl.Popup"/> uses the fade effect.
//        /// </summary>
//        /// <value><c>true</c> if pop-up uses the fade effect; otherwise, <c>false</c>.</value>
//        /// <remarks>To use the fade effect, the FocusOnOpen property also has to be set to <c>true</c>.</remarks>
//        public bool UseFadeEffect
//        {
//            get { return fade; }
//            set
//            {
//                if (fade == value) return;
//                fade = value;
//            }
//        }

//        private bool focusOnOpen = true;
//        /// <summary>
//        /// Gets or sets a value indicating whether to focus the content after the pop-up has been opened.
//        /// </summary>
//        /// <value><c>true</c> if the content should be focused after the pop-up has been opened; otherwise, <c>false</c>.</value>
//        /// <remarks>If the FocusOnOpen property is set to <c>false</c>, then pop-up cannot use the fade effect.</remarks>
//        public bool FocusOnOpen
//        {
//            get { return focusOnOpen; }
//            set { focusOnOpen = value; }
//        }

//        private bool acceptAlt = true;
//        /// <summary>
//        /// Gets or sets a value indicating whether presing the alt key should close the pop-up.
//        /// </summary>
//        /// <value><c>true</c> if presing the alt key does not close the pop-up; otherwise, <c>false</c>.</value>
//        public bool AcceptAlt
//        {
//            get { return acceptAlt; }
//            set { acceptAlt = value; }
//        }

//        private Popup ownerPopup;
//        private Popup childPopup;

//        private bool _isChildPopupOpened;
//        private bool _resizable;
//        //private bool resizable;
//        /// <summary>
//        /// Gets or sets a value indicating whether this <see cref="PopupControl.Popup" /> is resizable.
//        /// </summary>
//        /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
//        public bool Resizable
//        {
//            //get { return resizable && _resizable; }
//            get { return _resizable && !_isChildPopupOpened; }
//            set { resizable = value; }
//        }

//        private ToolStripControlHost host;

//        private Size minSize;
//        /// <summary>
//        /// Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
//        /// </summary>
//        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
//        public new Size MinimumSize
//        {
//            get { return minSize; }
//            set { minSize = value; }
//        }

//        private Size maxSize;
//        /// <summary>
//        /// Gets or sets the size that is the upper limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
//        /// </summary>
//        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
//        public new Size MaximumSize
//        {
//            get { return maxSize; }
//            set { maxSize = value; }
//        }

//        /// <summary>
//        /// Gets parameters of a new window.
//        /// </summary>
//        /// <returns>An object of type <see cref="T:System.Windows.Forms.CreateParams" /> used when creating a new window.</returns>
//        protected override CreateParams CreateParams
//        {
//            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
//            get
//            {
//                CreateParams cp = base.CreateParams;
//                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
//                return cp;
//            }
//        }

//        #endregion

//        #region " Constructors "

//        /// <summary>
//        /// Initializes a new instance of the <see cref="PopupControl.Popup"/> class.
//        /// </summary>
//        /// <param name="content">The content of the pop-up.</param>
//        /// <remarks>
//        /// Pop-up will be disposed immediately after disposion of the content control.
//        /// </remarks>
//        /// <exception cref="T:System.ArgumentNullException"><paramref name="content" /> is <code>null</code>.</exception>
//        public Popup(Control content)
//        {
//            if (content == null)
//            {
//                throw new ArgumentNullException("content");
//            }
//            this.content = content;
//            this.fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
//            this._resizable = true;
//            InitializeComponent();
//            AutoSize = false;
//            DoubleBuffered = true;
//            ResizeRedraw = true;
//            host = new ToolStripControlHost(content);
//            Padding = Margin = host.Padding = host.Margin = Padding.Empty;
//            MinimumSize = content.MinimumSize;
//            content.MinimumSize = content.Size;
//            MaximumSize = content.MaximumSize;
//            content.MaximumSize = content.Size;
//            Size = content.Size;
//            content.Location = Point.Empty;
//            Items.Add(host);
//            content.Disposed += delegate(object sender, EventArgs e)
//            {
//                content = null;
//                Dispose(true);
//            };
//            content.RegionChanged += delegate(object sender, EventArgs e)
//            {
//                UpdateRegion();
//            };
//            content.Paint += delegate(object sender, PaintEventArgs e)
//            {
//                PaintSizeGrip(e);
//            };
//            UpdateRegion();
//        }

//        #endregion

//        #region " Methods "

//        /// <summary>
//        /// Processes a dialog box key.
//        /// </summary>
//        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
//        /// <returns>
//        /// true if the key was processed by the control; otherwise, false.
//        /// </returns>
//        protected override bool ProcessDialogKey(Keys keyData)
//        {
//            if (acceptAlt && ((keyData & Keys.Alt) == Keys.Alt)) return false;
//            return base.ProcessDialogKey(keyData);
//        }

//        /// <summary>
//        /// Updates the pop-up region.
//        /// </summary>
//        protected void UpdateRegion()
//        {
//            if (this.Region != null)
//            {
//                this.Region.Dispose();
//                this.Region = null;
//            }
//            if (content.Region != null)
//            {
//                this.Region = content.Region.Clone();
//            }
//        }

//        /// <summary>
//        /// Shows pop-up window below the specified control.
//        /// </summary>
//        /// <param name="control">The control below which the pop-up will be shown.</param>
//        /// <remarks>
//        /// When there is no space below the specified control, the pop-up control is shown above it.
//        /// </remarks>
//        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
//        public void Show(Control control)
//        {
//            if (control == null)
//            {
//                throw new ArgumentNullException("control");
//            }
//            SetOwnerItem(control);
//            Show(control, control.ClientRectangle);
//        }

//        /// <summary>
//        /// Shows pop-up window below the specified area of specified control.
//        /// </summary>
//        /// <param name="control">The control used to compute screen location of specified area.</param>
//        /// <param name="area">The area of control below which the pop-up will be shown.</param>
//        /// <remarks>
//        /// When there is no space below specified area, the pop-up control is shown above it.
//        /// </remarks>
//        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
//        public void Show(Control control, Rectangle area)
//        {
//            if (control == null)
//            {
//                throw new ArgumentNullException("control");
//            }
//            SetOwnerItem(control);
//            resizableTop = resizableRight = false;
//            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
//            Rectangle screen = Screen.FromControl(control).WorkingArea;
//            if (location.X + Size.Width > (screen.Left + screen.Width))
//            {
//                resizableRight = true;
//                location.X = (screen.Left + screen.Width) - Size.Width;
//            }
//            if (location.Y + Size.Height > (screen.Top + screen.Height))
//            {
//                resizableTop = true;
//                location.Y -= Size.Height + area.Height;
//            }
//            location = control.PointToClient(location);
//            Show(control, location, ToolStripDropDownDirection.BelowRight);
//        }

//        private const int frames = 1;
//        private const int totalduration = 0; // ML : 2007-11-05 : was 100 but caused a flicker.
//        private const int frameduration = totalduration / frames;
//        /// <summary>
//        /// Adjusts the size of the owner <see cref="T:System.Windows.Forms.ToolStrip" /> to accommodate the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> if the owner <see cref="T:System.Windows.Forms.ToolStrip" /> is currently displayed, or clears and resets active <see cref="T:System.Windows.Forms.ToolStripDropDown" /> child controls of the <see cref="T:System.Windows.Forms.ToolStrip" /> if the <see cref="T:System.Windows.Forms.ToolStrip" /> is not currently displayed.
//        /// </summary>
//        /// <param name="visible">true if the owner <see cref="T:System.Windows.Forms.ToolStrip" /> is currently displayed; otherwise, false.</param>
//        protected override void SetVisibleCore(bool visible)
//        {
//            double opacity = Opacity;
//            if (visible && fade && focusOnOpen) Opacity = 0;
//            base.SetVisibleCore(visible);
//            if (!visible || !fade || !focusOnOpen) return;
//            for (int i = 1; i <= frames; i++)
//            {
//                if (i > 1)
//                {
//                    System.Threading.Thread.Sleep(frameduration);
//                }
//                Opacity = opacity * (double)i / (double)frames;
//            }
//            Opacity = opacity;
//        }

//        private bool resizableTop;
//        private bool resizableRight;

//        private void SetOwnerItem(Control control)
//        {
//            if (control == null)
//            {
//                return;
//            }
//            if (control is Popup)
//            {
//                Popup popupControl = control as Popup;
//                ownerPopup = popupControl;
//                ownerPopup.childPopup = this;
//                OwnerItem = popupControl.Items[0];
//                return;
//            }
//            if (control.Parent != null)
//            {
//                SetOwnerItem(control.Parent);
//            }
//        }

//        /// <summary>
//        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
//        /// </summary>
//        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
//        protected override void OnSizeChanged(EventArgs e)
//        {
//            content.MinimumSize = Size;
//            content.MaximumSize = Size;
//            content.Size = Size;
//            content.Location = Point.Empty;
//            base.OnSizeChanged(e);
//        }

//        /// <summary>
//        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opening" /> event.
//        /// </summary>
//        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
//        protected override void OnOpening(CancelEventArgs e)
//        {
//            if (content.IsDisposed || content.Disposing)
//            {
//                e.Cancel = true;
//                return;
//            }
//            UpdateRegion();
//            base.OnOpening(e);
//        }

//        /// <summary>
//        /// Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.Opened" /> event.
//        /// </summary>
//        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
//        protected override void OnOpened(EventArgs e)
//        {
//            if (ownerPopup != null)
//            {
//                //ownerPopup._resizable = false;
//                ownerPopup._isChildPopupOpened = true;
//            }
//            if (focusOnOpen)
//            {
//                content.Focus();
//            }
//            base.OnOpened(e);
//        }

//        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
//        {
//            if (ownerPopup != null)
//            {
//                //ownerPopup._resizable = true;                
//                ownerPopup._isChildPopupOpened = false;
//            }
//            base.OnClosed(e);
//        }

//        public DateTime LastClosedTimeStamp = DateTime.Now;

//        protected override void OnVisibleChanged(EventArgs e)
//        {
//            if (Visible == false)
//                LastClosedTimeStamp = DateTime.Now;
//            base.OnVisibleChanged(e);
//        }

//        #endregion

//        #region " Resizing Support "

//        /// <summary>
//        /// Processes Windows messages.
//        /// </summary>
//        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
//        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
//        protected override void WndProc(ref Message m)
//        {
//            if (InternalProcessResizing(ref m, false))
//            {
//                return;
//            }
//            base.WndProc(ref m);
//        }

//        /// <summary>
//        /// Processes the resizing messages.
//        /// </summary>
//        /// <param name="m">The message.</param>
//        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
//        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
//        public bool ProcessResizing(ref Message m)
//        {
//            return InternalProcessResizing(ref m, true);
//        }

//        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
//        private bool InternalProcessResizing(ref Message m, bool contentControl)
//        {
//            if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam != IntPtr.Zero && childPopup != null && childPopup.Visible)
//            {
//                childPopup.Hide();
//            }
//            if (!Resizable)
//            {
//                return false;
//            }
//            if (m.Msg == NativeMethods.WM_NCHITTEST)
//            {
//                return OnNcHitTest(ref m, contentControl);
//            }
//            else if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
//            {
//                return OnGetMinMaxInfo(ref m);
//            }
//            return false;
//        }

//        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
//        private bool OnGetMinMaxInfo(ref Message m)
//        {
//            NativeMethods.MINMAXINFO minmax = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
//            minmax.maxTrackSize = this.MaximumSize;
//            minmax.minTrackSize = this.MinimumSize;
//            Marshal.StructureToPtr(minmax, m.LParam, false);
//            return true;
//        }

//        private bool OnNcHitTest(ref Message m, bool contentControl)
//        {
//            int x = NativeMethods.LOWORD(m.LParam);
//            int y = NativeMethods.HIWORD(m.LParam);
//            Point clientLocation = PointToClient(new Point(x, y));

//            GripBounds gripBouns = new GripBounds(contentControl ? content.ClientRectangle : ClientRectangle);
//            IntPtr transparent = new IntPtr(NativeMethods.HTTRANSPARENT);

//            if (resizableTop)
//            {
//                if (resizableRight && gripBouns.TopLeft.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPLEFT;
//                    return true;
//                }
//                if (!resizableRight && gripBouns.TopRight.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPRIGHT;
//                    return true;
//                }
//                if (gripBouns.Top.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOP;
//                    return true;
//                }
//            }
//            else
//            {
//                if (resizableRight && gripBouns.BottomLeft.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMLEFT;
//                    return true;
//                }
//                if (!resizableRight && gripBouns.BottomRight.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
//                    return true;
//                }
//                if (gripBouns.Bottom.Contains(clientLocation))
//                {
//                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOM;
//                    return true;
//                }
//            }
//            if (resizableRight && gripBouns.Left.Contains(clientLocation))
//            {
//                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTLEFT;
//                return true;
//            }
//            if (!resizableRight && gripBouns.Right.Contains(clientLocation))
//            {
//                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTRIGHT;
//                return true;
//            }
//            return false;
//        }

//        private VS.VisualStyleRenderer sizeGripRenderer;
//        /// <summary>
//        /// Paints the size grip.
//        /// </summary>
//        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
//        public void PaintSizeGrip(PaintEventArgs e)
//        {
//            if (e == null || e.Graphics == null || !resizable)
//            {
//                return;
//            }
//            Size clientSize = content.ClientSize;
//            if (Application.RenderWithVisualStyles)
//            {
//                if (this.sizeGripRenderer == null)
//                {
//                    this.sizeGripRenderer = new VS.VisualStyleRenderer(VS.VisualStyleElement.Status.Gripper.Normal);
//                }
//                this.sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10));
//            }
//            else
//            {
//                ControlPaint.DrawSizeGrip(e.Graphics, content.BackColor, clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10);
//            }
//        }

//        #endregion
//    }
//}
