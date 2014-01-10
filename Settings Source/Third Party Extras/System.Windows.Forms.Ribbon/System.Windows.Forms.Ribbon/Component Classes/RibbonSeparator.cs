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
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
   public sealed class RibbonSeparator : RibbonItem
   {
      bool _drawBackground = true;

      public RibbonSeparator()
      {

      }

      public RibbonSeparator(string text)
      {
         Text = text;
      }

      public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
      {
			if ((Owner == null || !DrawBackground) && !Owner.IsDesignMode())
            return;

         Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(
         Owner, e.Graphics, e.Clip, this));

         if (!string.IsNullOrEmpty(Text))
         {
            Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(
                    Owner, e.Graphics, e.Clip, this,
                    Rectangle.FromLTRB(
                        Bounds.Left + Owner.ItemMargin.Left,
                        Bounds.Top + Owner.ItemMargin.Top,
                        Bounds.Right - Owner.ItemMargin.Right,
                        Bounds.Bottom - Owner.ItemMargin.Bottom), Text, FontStyle.Bold));
         }
      }

      public override void SetBounds(System.Drawing.Rectangle bounds)
      {
         base.SetBounds(bounds);
      }

      public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
      {
         if (e.SizeMode == RibbonElementSizeMode.DropDown)
         {
            if (string.IsNullOrEmpty(Text))
            {
               SetLastMeasuredSize(new Size(1, 3));
            }
            else
            {
               Size sz = e.Graphics.MeasureString(Text, new Font(Owner.Font, FontStyle.Bold)).ToSize();
               SetLastMeasuredSize(new Size(sz.Width + Owner.ItemMargin.Horizontal, sz.Height + Owner.ItemMargin.Vertical));
            }
         }
         else
         {
            SetLastMeasuredSize(new Size(2, OwnerPanel.ContentBounds.Height - Owner.ItemPadding.Vertical - Owner.ItemMargin.Vertical));
         }

         return LastMeasuredSize;
      }

      /// <summary>
      /// Gets or sets a value indicating if the separator should draw the divider lines
      /// </summary>
      [DefaultValue(true)]
      [Description("Background drawing should be avoided when group contains only TextBoxes and ComboBoxes")]
      public bool DrawBackground
      {
         get { return _drawBackground; }
         set { _drawBackground = value; }
      }

   }
}
