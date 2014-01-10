// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
   public class RibbonHost : RibbonItem
   {
      private Control ctl;
      private Font ctlFont;
      private Size ctlSize;
      private RibbonElementSizeMode _lastSizeMode;

      /// <summary>
      /// Occurs when the SizeMode of the controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.
      /// </summary>
      [Description("Occurs when the SizeMode of the Controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.")]
      public event RibbonHostSizeModeHandledEventHandler SizeModeChanging;
      public delegate void RibbonHostSizeModeHandledEventHandler(object sender, RibbonHostSizeModeHandledEventArgs e);

      public RibbonHost()
      {
      }
      /// <summary>
      /// Gets or sets the control that this item willl host
      /// </summary>
      public Control HostedControl
      {
         get { return ctl; }
         set
         {
            ctl = value;
            NotifyOwnerRegionsChanged();

            //_mouseHook = new RibbonHelpers.GlobalHook(RibbonHelpers.GlobalHook.HookTypes.Mouse);
            //_mouseHook.MouseDown += new MouseEventHandler(_mouseHook_MouseDown);
            //_mouseHook.MouseUp += new MouseEventHandler(_mouseHook_MouseUp);

            if (ctl != null && Site == null)
            {
               //Initially set the control to be hidden. If it needs to be displayed immediately it will
               //get set in the placecontrol function
               ctl.Visible = false;

               //changing the owner changes the font so let save it for future use
               ctlFont = ctl.Font;
               ctlSize = ctl.Size;

               //hook into some needed events
               ctl.MouseMove += new MouseEventHandler(ctl_MouseMove);
               this.CanvasChanged += new EventHandler(RibbonHost_CanvasChanged);
               //we must know when our tab changes so we can hide the control
               if (OwnerTab != null)
                  Owner.ActiveTabChanged += new EventHandler(Owner_ActiveTabChanged);

               //the control must always have the same parent as the host item so set it here.
               if (Owner != null)
                  Owner.Controls.Add(ctl);

               ctl.Font = ctlFont;
            }
         }
      }
      /// <summary>
      /// Raises the paint event and draws the
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
      {
         if (Owner != null)
         {
            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;
            f.Trimming = StringTrimming.None;
            f.FormatFlags |= StringFormatFlags.NoWrap;
            if (Site != null && Site.DesignMode)
            {
               Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, Bounds, Site.Name, f));
            }
            else
            {
               Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, Bounds, this.Text, f));
               if (ctl != null)
               {
                  if (ctl.Parent == null)
                     Owner.Controls.Add(ctl);

                  //time to show our control
                  ctl.Location = new System.Drawing.Point(Bounds.Left + 1, Bounds.Top + 1);
                  ctl.Visible = true;
                  ctl.BringToFront();
               }
            }
         }
      }
      /// <summary>
      /// Sets the bounds of the panel
      /// </summary>
      /// <param name="bounds"></param>
      public override void SetBounds(System.Drawing.Rectangle bounds)
      {
         base.SetBounds(bounds);
      }
      /// <summary>
      /// Measures the size of the panel on the mode specified by the event object
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <returns></returns>
      public override System.Drawing.Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
      {
         if (Site != null && Site.DesignMode && Owner != null)
         {
            //when in design mode just paint the name of this control
            int Width = Convert.ToInt32(e.Graphics.MeasureString(Site.Name, Owner.Font).Width);
            int Height = 20;
            SetLastMeasuredSize(new System.Drawing.Size(Width, Height));
         }
         else if (ctl == null || !Visible)
            SetLastMeasuredSize(new System.Drawing.Size(0, 0));
         else
         {
            ctl.Visible = false;
            if (_lastSizeMode != e.SizeMode)
            {
               _lastSizeMode = e.SizeMode;
               RibbonHostSizeModeHandledEventArgs hev = new RibbonHostSizeModeHandledEventArgs(e.Graphics, e.SizeMode);
               OnSizeModeChanging(ref hev);
            }
            SetLastMeasuredSize(new System.Drawing.Size(ctl.Size.Width + 2, ctl.Size.Height + 2));
         }
         return LastMeasuredSize;
      }
      /// <summary>
      /// Call this method when you need to close a popup that the control is contained in
      /// </summary>
      public void HostCompleted()
      {
         //Kevin Carbis - Clear everything by simulating the click event on the parent item
         //just in case we are in a popup window
         OnClick(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));
      }
      /// <summary>
      /// Raises the <see cref="SizeModeChanged"/> event
      /// </summary>
      /// <param name="e"></param>
      public virtual void OnSizeModeChanging(ref RibbonHostSizeModeHandledEventArgs e)
      {
         if (SizeModeChanging != null)
         {
            SizeModeChanging(this, e);
         }
      }

      private void PlaceControls()
      {
         if (ctl != null && Site == null)
         {
            ctl.Location = new System.Drawing.Point(Bounds.Left + 1, Bounds.Top + 1);
            //if we are located directly in a panel then we need to make sure the panel is not in overflow
            //mode or we will look bad showing on the panel when we shouldn't be
            if ((Canvas is Ribbon) && (OwnerPanel != null && OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow))
               ctl.Visible = false;
         }
      }
      private void ctl_MouseMove(object sender, MouseEventArgs e)
      {
         //convert the controls mousemove to the items mousemove to keep the highlighting on the panels in sync
         OnMouseMove(e);
         //Console.WriteLine(e.Location.ToString());
      }
      private void Owner_ActiveTabChanged(object sender, EventArgs e)
      {
         //hide this control if our tab is not the active tab
         if (OwnerTab != null && Owner.ActiveTab != OwnerTab)
            ctl.Visible = false;
      }
      private void RibbonHost_CanvasChanged(object sender, EventArgs e)
      {
         if (ctl != null)
         {
            Canvas.Controls.Add(ctl);
            ctl.Font = ctlFont;
            //ctl.Location = new System.Drawing.Point(Bounds.Left + 1, Bounds.Top + 1);
         }
      }
      internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
      {
         base.SetSizeMode(sizeMode);
         if (OwnerPanel != null && OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow)
         {
            ctl.Visible = false;
         }
      }
   }
}
