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
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace System.Windows.Forms
{
   [DesignTimeVisible(false)]
   [Designer(typeof(RibbonPanelDesigner))]
   public class RibbonPanel :
       Component, IRibbonElement, IContainsSelectableRibbonItems, IContainsRibbonComponents
   {
      #region Fields
      private bool _enabled;
      private System.Drawing.Image _image;
      private RibbonItemCollection _items;
      private string _text;
      private Ribbon _owner;
      private Rectangle _bounds;
      private Rectangle _contentBounds;
      private bool _selected;
      private object _tag;
      private RibbonTab _ownerTab;
      private RibbonElementSizeMode _sizeMode;
      private RibbonPanelFlowDirection _flowsTo;
      private Control _popUp;
      private bool _pressed;
      private Rectangle _buttonMoreBounds;
      private bool _buttonMorePressed;
      private bool _butonMoreSelected;
      private bool _buttonMoreVisible;
      private bool _buttonMoreEnabled;
      internal Rectangle overflowBoundsBuffer;
      private bool _popupShowed;
      private bool _visible = true;
      private bool _IsFirstPanel = false;
      private bool _IsLastPanel = false;
      private int _Index = -1;
      #endregion

      #region Events
      /// <summary>
      /// Occurs when the mouse pointer enters the panel
      /// </summary>
      public event MouseEventHandler MouseEnter;

      /// <summary>
      /// Occurs when the mouse pointer leaves the panel
      /// </summary>
      public event MouseEventHandler MouseLeave;

      /// <summary>
      /// Occurs when the mouse pointer is moved inside the panel
      /// </summary>
      public event MouseEventHandler MouseMove;

      /// <summary>
      /// Occurs when the panel is redrawn
      /// </summary>
      public event PaintEventHandler Paint;

      /// <summary>
      /// Occurs when the panel is resized
      /// </summary>
      public event EventHandler Resize;

      public event EventHandler ButtonMoreClick;

      public virtual event EventHandler Click;

      public virtual event EventHandler DoubleClick;

      public virtual event System.Windows.Forms.MouseEventHandler MouseDown;

      public virtual event System.Windows.Forms.MouseEventHandler MouseUp;
      #endregion

      #region Ctor
      /// <summary>
      /// Creates a new RibbonPanel
      /// </summary>
      public RibbonPanel()
      {
         _items = new RibbonItemCollection();
         _sizeMode = RibbonElementSizeMode.None;
         _flowsTo = RibbonPanelFlowDirection.Bottom;
         _buttonMoreEnabled = true;
         _buttonMoreVisible = true;
         _items.SetOwnerPanel(this);
         _enabled = true;

      }

      /// <summary>
      /// Creates a new RibbonPanel with the specified text
      /// </summary>
      /// <param name="text">Text of the panel</param>
      public RibbonPanel(string text)
         : this(text, RibbonPanelFlowDirection.Bottom)
      {

      }

      /// <summary>
      /// Creates a new RibbonPanel with the specified text and panel flow direction
      /// </summary>
      /// <param name="text">Text of the panel</param>
      /// <param name="flowsTo">Flow direction of the content items</param>
      public RibbonPanel(string text, RibbonPanelFlowDirection flowsTo)
         : this(text, flowsTo, new RibbonItem[] { })
      {
      }

      /// <summary>
      /// Creates a new RibbonPanel with the specified text and panel flow direction
      /// </summary>
      /// <param name="text">Text of the panel</param>
      /// <param name="flowsTo">Flow direction of the content items</param>
      public RibbonPanel(string text, RibbonPanelFlowDirection flowsTo, IEnumerable<RibbonItem> items)
         : this()
      {
         _text = text;
         _flowsTo = flowsTo;
         _items.AddRange(items);
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing && RibbonDesigner.Current == null)
         {
            foreach (RibbonItem ri in _items)
               ri.Dispose();
         }

         base.Dispose(disposing);
      }

      #endregion

      #region Props

      [Description("Sets if the panel should be enabled")]
      [DefaultValue(true)]
      public bool Enabled
      {
         get { return _enabled; }
         set
         {
            _enabled = value;

            foreach (RibbonItem item in Items)
            {
               item.Enabled = value;
            }
         }
      }

      [Description("Sets if the panel should be Visible")]
      [DefaultValue(true)]
      public virtual bool Visible
      {
         get
         {
            return _visible;
         }
         set
         {
            _visible = value;
            //this.OwnerTab.UpdatePanelsRegions();
            this.OwnerTab.Owner.PerformLayout();
            this.Owner.Invalidate();
         }
      }

      /// <summary>
      /// Gets if this panel is currenlty collapsed
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool Collapsed
      {
         get { return SizeMode == RibbonElementSizeMode.Overflow; }
      }

      /// <summary>
      /// Gets or sets the visibility of the "More" button
      /// </summary>
      [Description("Sets the visibility of the \"More...\" button")]
      [DefaultValue(true)]
      public bool ButtonMoreVisible
      {
         get { return _buttonMoreVisible; }
         set { _buttonMoreVisible = value; if (Owner != null) Owner.OnRegionsChanged(); }
      }

      /// <summary>
      /// Gets or sets a value indicating if the "More" button should be enabled
      /// </summary>
      [Description(@"Enables/Disables the ""More..."" button")]
      [DefaultValue(true)]
      public bool ButtonMoreEnabled
      {
         get { return _buttonMoreEnabled; }
         set { _buttonMoreEnabled = value; if (Owner != null) Owner.OnRegionsChanged(); }
      }

      /// <summary>
      /// Gets if the "More" button is currently selected
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ButtonMoreSelected
      {
         get { return _butonMoreSelected; }
      }

      /// <summary>
      /// Gets if the "More" button is currently pressed
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool ButtonMorePressed
      {
         get { return _buttonMorePressed; }
      }

      /// <summary>
      /// Gets the bounds of the "More" button
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public Rectangle ButtonMoreBounds
      {
         get { return _buttonMoreBounds; }
      }

      /// <summary>
      /// Gets if the panel is currently on overflow and pressed
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool Pressed
      {
         get { return _pressed; }
      }

      /// <summary>
      /// Gets or sets the pop up where the panel is being drawn (if any)
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      internal Control PopUp
      {
         get { return _popUp; }
         set { _popUp = value; }
      }

      /// <summary>
      /// Gets the current size mode of the panel
      /// </summary>
      public RibbonElementSizeMode SizeMode
      {
         get { return _sizeMode; }
      }

      /// <summary>
      /// Gets the collection of RibbonItem elements of this panel
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public RibbonItemCollection Items
      {
         get
         {
            return _items;
         }
      }

      /// <summary>
      /// Gets or sets the text that is to be displayed on the bottom of the panel
      /// </summary>
      [Localizable(true)]
      public string Text
      {
         get
         {
            return _text;
         }
         set
         {
            _text = value;

            if (Owner != null) Owner.OnRegionsChanged();
         }
      }

      /// <summary>
      /// Gets or sets the image that is to be displayed on the panel when shown as an overflow button
      /// </summary>
      [DefaultValue(null)]
      public System.Drawing.Image Image
      {
         get
         {
            return _image;
         }
         set
         {
            _image = value;

            if (Owner != null) Owner.OnRegionsChanged();
         }
      }

      /// <summary>
      /// Gets if the panel is in overflow mode
      /// </summary>
      /// <remarks>Overflow mode is when the available space to draw the panel is not enough to draw components, so panel is drawn as a button that shows the full content of the panel in a pop-up window when clicked</remarks>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool OverflowMode
      {
         get
         {
            return SizeMode == RibbonElementSizeMode.Overflow;
         }
      }

      /// <summary>
      /// Gets the Ribbon that contains this panel
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public Ribbon Owner
      {
         get
         {
            return _owner;
         }
      }

      /// <summary>
      /// Gets the bounds of the panel relative to the Ribbon control
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public Rectangle Bounds
      {
         get
         {
            return _bounds;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the panel is selected
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public virtual bool Selected
      {
         get
         {
            return _selected;
         }
         set
         {
            _selected = value;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the panel is the first panel on the tab
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public virtual bool IsFirstPanel
      {
          get
          {
              return _IsFirstPanel;
          }
          set
          {
              _IsFirstPanel = value;
          }
      }

      /// <summary>
      /// Gets a value indicating whether the panel is the last panel on the tab
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public virtual bool IsLastPanel
      {
          get
          {
              return _IsLastPanel;
          }
          set
          {
              _IsLastPanel = value;
          }
      }

      /// <summary>
      /// Gets a value indicating what the index of the panel is in the Tabs panel collection
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public virtual int Index
      {
          get
          {
              return _Index;
          }
          set
          {
              _Index = value;
          }
      }

      /// <summary>
      /// Gets or sets the object that contains data about the control
      /// </summary>
      [DescriptionAttribute("An Object field for associating custom data for this control")]
      [DefaultValue(null)]
      [TypeConverter(typeof(StringConverter))]
      public object Tag
      {
         get
         {
            return _tag;
         }
         set
         {
            _tag = value;
         }
      }

      /// <summary>
      /// Gets the bounds of the content of the panel
      /// </summary>
      public Rectangle ContentBounds
      {
         get
         {
            return _contentBounds;
         }
      }

      /// <summary>
      /// Gets the RibbonTab that contains this panel
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public RibbonTab OwnerTab
      {
         get
         {
            return _ownerTab;
         }
      }

      /// <summary>
      /// Gets or sets the flow direction to layout items
      /// </summary>
      [DefaultValue(RibbonPanelFlowDirection.Bottom)]
      public RibbonPanelFlowDirection FlowsTo
      {
         get
         {
            return _flowsTo;
         }
         set
         {
            _flowsTo = value;

            if (Owner != null) Owner.OnRegionsChanged();
         }
      }

      /// <summary>
      /// Gets or sets if the popup is currently showing
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      internal bool PopupShowed
      {
         get { return _popupShowed; }
         set { _popupShowed = value; }
      }


      #endregion

      #region IRibbonElement Members


      public Size SwitchToSize(Control ctl, Graphics g, RibbonElementSizeMode size)
      {
         Size s = MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, size));
         Rectangle r = new Rectangle(0, 0, s.Width, s.Height);

         //if (!(ctl is Ribbon))
         //    r = boundsBuffer;
         //else
         //    r = new Rectangle(0, 0, 0, 0);

         SetBounds(r);
         UpdateItemsRegions(g, size);
         return s;
      }

      /// <summary>
      /// Raises the paint event and draws the
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      public virtual void OnPaint(object sender, RibbonElementPaintEventArgs e)
      {
         if (Paint != null)
         {
            Paint(this, new PaintEventArgs(e.Graphics, e.Clip));
         }

         if (PopupShowed && e.Control == Owner)
         {
            //Draw a fake collapsed and pressed panel

            #region Create fake panel
            RibbonPanel fakePanel = new RibbonPanel(this.Text);
            fakePanel.Image = this.Image;
            fakePanel.SetSizeMode(RibbonElementSizeMode.Overflow);
            fakePanel.SetBounds(overflowBoundsBuffer);
            fakePanel.SetPressed(true);
            fakePanel.SetOwner(Owner);
            #endregion

            Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(Owner, e.Graphics, e.Clip, fakePanel, e.Control));
            Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(Owner, e.Graphics, e.Clip, fakePanel, e.Control));
         }
         else
         {
            //Draw normal
            Owner.Renderer.OnRenderRibbonPanelBackground(new RibbonPanelRenderEventArgs(Owner, e.Graphics, e.Clip, this, e.Control));
            Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(Owner, e.Graphics, e.Clip, this, e.Control));
         }

         if (e.Mode != RibbonElementSizeMode.Overflow ||
             (e.Control != null && e.Control == PopUp))
         {
            foreach (RibbonItem item in Items)
            {
					if (item.Visible || Owner.IsDesignMode())
                  item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, item.SizeMode));
            }
         }
      }

      /// <summary>
      /// Sets the bounds of the panel
      /// </summary>
      /// <param name="bounds"></param>
      public void SetBounds(System.Drawing.Rectangle bounds)
      {
         bool trigger = _bounds != bounds;

         _bounds = bounds;

         OnResize(EventArgs.Empty);

         if (Owner != null)
         {
            //Update contentBounds
            _contentBounds = Rectangle.FromLTRB(
                bounds.X + Owner.PanelMargin.Left + 0,
                bounds.Y + Owner.PanelMargin.Top + 0,
                bounds.Right - Owner.PanelMargin.Right,
                bounds.Bottom - Owner.PanelMargin.Bottom);
         }

         //"More" bounds
         if (ButtonMoreVisible)
         {
            SetMoreBounds(
                Rectangle.FromLTRB(
                    bounds.Right - 15,
                    _contentBounds.Bottom + 1,
                    bounds.Right,
                    bounds.Bottom)
                );
         }
         else
         {
            SetMoreBounds(Rectangle.Empty);
         }

      }

      /// <summary>
      /// Measures the size of the panel on the mode specified by the event object
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <returns></returns>
      public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
      {
         Size result = Size.Empty;
         Size minSize = Size.Empty;

			if (!Visible && !Owner.IsDesignMode()) return new Size(0, 0);
         
         int panelHeight = OwnerTab.TabContentBounds.Height - Owner.PanelPadding.Vertical;

         #region Measure width of minSize

         minSize.Width = e.Graphics.MeasureString(Text, Owner.Font).ToSize().Width + Owner.PanelMargin.Horizontal + 1;

         if (ButtonMoreVisible)
         {
            minSize.Width += ButtonMoreBounds.Width + 3;
         }

         #endregion

         if (e.SizeMode == RibbonElementSizeMode.Overflow)
         {
            Size textSize = RibbonButton.MeasureStringLargeSize(e.Graphics, Text, Owner.Font);

            return new Size(textSize.Width + Owner.PanelMargin.Horizontal, panelHeight);
         }

         switch (FlowsTo)
         {
            case RibbonPanelFlowDirection.Left:
               result = MeasureSizeFlowsToBottom(sender, e);
               break;
            case RibbonPanelFlowDirection.Right:
               result = MeasureSizeFlowsToRight(sender, e);
               break;
            case RibbonPanelFlowDirection.Bottom:
               result = MeasureSizeFlowsToBottom(sender, e);
               break;
            default:
               result = Size.Empty;
               break;
         }

         return new Size(Math.Max(result.Width, minSize.Width), panelHeight);
      }

      /// <summary>
      /// Sets the value of the Owner Property
      /// </summary>
      internal void SetOwner(Ribbon owner)
      {
         _owner = owner;

         Items.SetOwner(owner);
      }

      /// <summary>
      /// Sets the value of the Selected property
      /// </summary>
      /// <param name="selected">Value that indicates if the element is selected</param>
      internal void SetSelected(bool selected)
      {
         _selected = selected;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Raises the <see cref="Resize"/> method
      /// </summary>
      /// <param name="e"></param>
      protected virtual void OnResize(EventArgs e)
      {
         if (Resize != null)
         {
            Resize(this, e);
         }
      }

      /// <summary>
      /// Shows the panel in a popup
      /// </summary>
      private void ShowOverflowPopup()
      {
         Rectangle b = Bounds;
         RibbonPanelPopup f = new RibbonPanelPopup(this);
         Point p = Owner.PointToScreen(new Point(b.Left, b.Bottom));
         PopupShowed = true;
         f.Show(p);
      }

      /// <summary>
      /// Measures the size when flow direction is to right
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <returns></returns>
      private Size MeasureSizeFlowsToRight(object sender, RibbonElementMeasureSizeEventArgs e)
      {
         int widthSum = Owner.PanelMargin.Horizontal;
         int maxWidth = 0;
         int maxHeight = 0;
         int dividedWidth = 0;

         foreach (RibbonItem item in Items)
         {
				if (item.Visible || Owner.IsDesignMode())
            {
               Size itemSize = item.MeasureSize(this, e);

               widthSum += itemSize.Width + Owner.ItemPadding.Horizontal + 1;

               maxWidth = Math.Max(maxWidth, itemSize.Width);
               maxHeight = Math.Max(maxHeight, itemSize.Height);
            }
         }

         switch (e.SizeMode)
         {
            case RibbonElementSizeMode.Large:
               dividedWidth = widthSum / 1; //Show items on one row
               break;
            case RibbonElementSizeMode.Medium:
               dividedWidth = widthSum / 2; //Show items on two rows
               break;
            case RibbonElementSizeMode.Compact:
               dividedWidth = widthSum / 3; //Show items on three rows
               break;
            default:
               break;
         }

         //Add padding
         dividedWidth += Owner.PanelMargin.Horizontal;

         return new Size(Math.Max(maxWidth, dividedWidth) + Owner.PanelMargin.Horizontal, 0); //Height is provided by MeasureSize
      }

      /// <summary>
      /// Measures the size when flow direction is to bottom
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <returns></returns>
      private Size MeasureSizeFlowsToBottom(object sender, RibbonElementMeasureSizeEventArgs e)
      {
         int curRight = Owner.PanelMargin.Left + Owner.ItemPadding.Horizontal;
         int curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical;
         int lastRight = 0;
         int lastBottom = 0;
         int availableHeight = OwnerTab.TabContentBounds.Height - Owner.TabContentMargin.Vertical - Owner.PanelPadding.Vertical - Owner.PanelMargin.Vertical;
         int maxRight = 0;
         int maxBottom = 0;

         foreach (RibbonItem item in Items)
         {
				if (item.Visible || Owner.IsDesignMode() || item.GetType() == typeof(RibbonSeparator))
            {
               Size itemSize = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, e.SizeMode));

               if (curBottom + itemSize.Height > ContentBounds.Bottom)
               {
                  curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical + 0;
                  curRight = maxRight + Owner.ItemPadding.Horizontal + 0;
               }

               Rectangle bounds = new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height);

               lastRight = bounds.Right;
               lastBottom = bounds.Bottom;

               curBottom = bounds.Bottom + Owner.ItemPadding.Vertical + 1;

               maxRight = Math.Max(maxRight, lastRight);
               maxBottom = Math.Max(maxBottom, lastBottom);
            }
         }

         return new Size(maxRight + Owner.ItemPadding.Right + Owner.PanelMargin.Right + 1, 0); //Height is provided by MeasureSize
      }

      /// <summary>
      /// Sets the value of the SizeMode property
      /// </summary>
      /// <param name="sizeMode"></param>
      internal void SetSizeMode(RibbonElementSizeMode sizeMode)
      {
         _sizeMode = sizeMode;

         foreach (RibbonItem item in Items)
         {
            item.SetSizeMode(sizeMode);
         }
      }

      /// <summary>
      /// Sets the value of the ContentBounds property
      /// </summary>
      /// <param name="contentBounds">Bounds of the content on the panel</param>
      internal void SetContentBounds(Rectangle contentBounds)
      {
         _contentBounds = contentBounds;
      }

      /// <summary>
      /// Sets the value of the OwnerTab property
      /// </summary>
      /// <param name="ownerTab">RibbonTab where this item is located</param>
      internal void SetOwnerTab(RibbonTab ownerTab)
      {
         _ownerTab = ownerTab;

         Items.SetOwnerTab(OwnerTab);
      }

      /// <summary>
      /// Updates the bounds of child elements
      /// </summary>
      internal void UpdateItemsRegions(Graphics g, RibbonElementSizeMode mode)
      {
         switch (FlowsTo)
         {
            case RibbonPanelFlowDirection.Right:
               UpdateRegionsFlowsToRight(g, mode);
               break;
            case RibbonPanelFlowDirection.Bottom:
               UpdateRegionsFlowsToBottom(g, mode);
               break;
            case RibbonPanelFlowDirection.Left:
               UpdateRegionsFlowsToLeft(g, mode);
               break;
         }

         ///Center items on the panel
         CenterItems();
      }

      /// <summary>
      /// Updates the bounds of child elements when flow is to bottom
      /// </summary>
      private void UpdateRegionsFlowsToBottom(Graphics g, RibbonElementSizeMode mode)
      {
         int curRight = ContentBounds.Left + Owner.ItemPadding.Horizontal + 0;
         int curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical + 0;
         int lastRight = curRight;
         int lastBottom = 0;
         List<RibbonItem> lastColumn = new List<RibbonItem>();

         ///Iterate thru items on panel
         foreach (RibbonItem item in Items)
         {
            ///Gets the last measured size (to avoid re-measuring calculations)
            Size itemSize;
				if (item.Visible || Owner.IsDesignMode())
               itemSize = item.LastMeasuredSize;
            else
               itemSize = new Size(0, 0);

            ///If not enough space available, reset curBottom and advance curRight
            if (curBottom + itemSize.Height > ContentBounds.Bottom)
            {
               curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical + 0;
               curRight = lastRight + Owner.ItemPadding.Horizontal + 0;
               Items.CenterItemsVerticallyInto(lastColumn, ContentBounds);
               lastColumn.Clear();
            }

            ///Set the item's bounds
            item.SetBounds(new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height));

            ///save last right and bottom
            lastRight = Math.Max(item.Bounds.Right, lastRight);
            lastBottom = item.Bounds.Bottom;

            ///update current bottom
            curBottom = item.Bounds.Bottom + Owner.ItemPadding.Vertical + 1;

            ///Add to the collection of items of the last column
            lastColumn.Add(item);
         }

         ///Center the items vertically on the last column 
         Items.CenterItemsVerticallyInto(lastColumn, ContentBounds);
      }

      /// <summary>
      /// Updates the bounds of child elements when flow is to Left.
      /// </summary>
      private void UpdateRegionsFlowsToLeft(Graphics g, RibbonElementSizeMode mode)
      {
         int curRight = ContentBounds.Left + Owner.ItemPadding.Horizontal + 0;
         int curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical + 0;
         int lastRight = curRight;
         int lastBottom = 0;
         List<RibbonItem> lastColumn = new List<RibbonItem>();

         ///Iterate thru items on panel
         for (int i = Items.Count - 1; i >= 0; i--)
         {
            RibbonItem item = Items[i];

            ///Gets the last measured size (to avoid re-measuring calculations)
            Size itemSize;
            if (item.Visible)
               itemSize = item.LastMeasuredSize;
            else
               itemSize = new Size(0, 0);

            ///If not enough space available, reset curBottom and advance curRight
            if (curBottom + itemSize.Height > ContentBounds.Bottom)
            {
               curBottom = ContentBounds.Top + Owner.ItemPadding.Vertical + 0;
               curRight = lastRight + Owner.ItemPadding.Horizontal + 0;
               Items.CenterItemsVerticallyInto(lastColumn, ContentBounds);
               lastColumn.Clear();
            }

            ///Set the item's bounds
            item.SetBounds(new Rectangle(curRight, curBottom, itemSize.Width, itemSize.Height));

            ///save last right and bottom
            lastRight = Math.Max(item.Bounds.Right, lastRight);
            lastBottom = item.Bounds.Bottom;

            ///update current bottom
            curBottom = item.Bounds.Bottom + Owner.ItemPadding.Vertical + 1;

            ///Add to the collection of items of the last column
            lastColumn.Add(item);
         }

         ///Center the items vertically on the last column 
         Items.CenterItemsVerticallyInto(lastColumn, Items.GetItemsBounds());
      }

      /// <summary>
      /// Updates the bounds of child elements when flow is to bottom
      /// </summary>
      private void UpdateRegionsFlowsToRight(Graphics g, RibbonElementSizeMode mode)
      {
         int curLeft = ContentBounds.Left;
         int curTop = ContentBounds.Top;
         int padding = mode == RibbonElementSizeMode.Medium ? 7 : 0;
         int maxBottom = 0;

         #region Sorts from larger to smaller

         RibbonItem[] array = Items.ToArray();

         for (int i = (array.Length - 1); i >= 0; i--)
         {
            for (int j = 1; j <= i; j++)
            {
               if (array[j - 1].LastMeasuredSize.Width < array[j].LastMeasuredSize.Width)
               {
                  RibbonItem temp = array[j - 1];
                  array[j - 1] = array[j];
                  array[j] = temp;
               }
            }
         }

         #endregion

         List<RibbonItem> list = new List<RibbonItem>(array);

         ///Attend elements, deleting every attended element from the list
         while (list.Count > 0)
         {
            ///Extract item and delete it
            RibbonItem item = list[0];
            list.Remove(item);

            ///If not enough space left, reset left and advance top
            if (curLeft + item.LastMeasuredSize.Width > ContentBounds.Right)
            {
               curLeft = ContentBounds.Left;
               curTop = maxBottom + Owner.ItemPadding.Vertical + 1 + padding;
            }

            ///Set item's bounds
            item.SetBounds(new Rectangle(new Point(curLeft, curTop), item.LastMeasuredSize));

            ///Increment reminders
            curLeft += item.Bounds.Width + Owner.ItemPadding.Horizontal;
            maxBottom = Math.Max(maxBottom, item.Bounds.Bottom);

            ///Check available space after placing item
            int spaceAvailable = ContentBounds.Right - curLeft;

            ///Check for elements that fit on available space
            for (int i = 0; i < list.Count; i++)
            {
               ///If item fits on the available space
               if (list[i].LastMeasuredSize.Width < spaceAvailable)
               {
                  ///Place the item there and reset the counter to check for further items
                  list[i].SetBounds(new Rectangle(new Point(curLeft, curTop), list[i].LastMeasuredSize));
                  curLeft += list[i].Bounds.Width + Owner.ItemPadding.Horizontal;
                  maxBottom = Math.Max(maxBottom, list[i].Bounds.Bottom);
                  spaceAvailable = ContentBounds.Right - curLeft;
                  list.RemoveAt(i);
                  i = 0;
               }
            }
         }
      }

      /// <summary>
      /// Centers the items on the tab conent
      /// </summary>
      private void CenterItems()
      {
         Items.CenterItemsInto(ContentBounds);
      }

      /// <summary>
      /// Overriden. Gives info about the panel as a string
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return string.Format("Panel: {0} ({1})", Text, SizeMode);
      }

      /// <summary>
      /// Sets the value of the Pressed property
      /// </summary>
      /// <param name="pressed"></param>
      public void SetPressed(bool pressed)
      {
         _pressed = pressed;
      }

      /// <summary>
      /// Sets the value of the ButtonMorePressed property
      /// </summary>
      /// <param name="bounds">property value</param>
      internal void SetMorePressed(bool pressed)
      {
         _buttonMorePressed = pressed;
      }

      /// <summary>
      /// Sets the value of the ButtonMoreSelected property
      /// </summary>
      /// <param name="bounds">property value</param>
      internal void SetMoreSelected(bool selected)
      {
         _butonMoreSelected = selected;
      }

      /// <summary>
      /// Sets the value of the ButtonMoreBounds property
      /// </summary>
      /// <param name="bounds">property value</param>
      internal void SetMoreBounds(Rectangle bounds)
      {
         _buttonMoreBounds = bounds;
      }

      /// <summary>
      /// Raised the <see cref="ButtonMoreClick"/> event
      /// </summary>
      /// <param name="e"></param>
      protected void OnButtonMoreClick(EventArgs e)
      {
         if (ButtonMoreClick != null)
         {
            ButtonMoreClick(this, e);
         }
      }

      #endregion

      #region IContainsRibbonItems Members

      public IEnumerable<RibbonItem> GetItems()
      {
         return Items;
      }

      public Rectangle GetContentBounds()
      {
         return ContentBounds;
      }

      /// <summary>
      /// Raises the MouseEnter event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnMouseEnter(MouseEventArgs e)
      {
         if (!Enabled) return;

         if (MouseEnter != null)
         {
            MouseEnter(this, e);
         }
      }

      /// <summary>
      /// Raises the MouseLeave event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnMouseLeave(MouseEventArgs e)
      {
         if (!Enabled) return;

         if (MouseLeave != null)
         {
            MouseLeave(this, e);
         }
      }

      /// <summary>
      /// Raises the MouseMove event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnMouseMove(MouseEventArgs e)
      {
         if (!Enabled) return;

         if (MouseMove != null)
         {
            MouseMove(this, e);
         }

         bool redraw = false;

         if (ButtonMoreEnabled && ButtonMoreVisible && ButtonMoreBounds.Contains(e.X, e.Y) && !Collapsed)
         {
            SetMoreSelected(true);
            redraw = true;
         }
         else
         {
            redraw = ButtonMoreSelected;
            SetMoreSelected(false);
         }

         if (redraw)
         {
            Owner.Invalidate(Bounds);
         }
      }

      /// <summary>
      /// Raises the Click event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnClick(EventArgs e)
      {
         if (!Enabled) return;

         if (Click != null)
         {
            Click(this, e);
         }

         if (Collapsed && PopUp == null)
         {
            ShowOverflowPopup();
         }
      }

      /// <summary>
      /// Raises the DoubleClick event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnDoubleClick(EventArgs e)
      {
         if (!Enabled) return;

         if (DoubleClick != null)
         {
            DoubleClick(this, e);
         }
      }

      /// <summary>
      /// Raises the MouseDown event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnMouseDown(MouseEventArgs e)
      {
         if (!Enabled) return;

         if (MouseDown != null)
         {
            MouseDown(this, e);
         }

         SetPressed(true);

         bool redraw = false;

         if (ButtonMoreEnabled && ButtonMoreVisible && ButtonMoreBounds.Contains(e.X, e.Y) && !Collapsed)
         {
            SetMorePressed(true);
            redraw = true;
         }
         else
         {
            redraw = ButtonMoreSelected;
            SetMorePressed(false);
         }

         if (redraw)
         {
            Owner.Invalidate(Bounds);
         }
      }

      /// <summary>
      /// Raises the MouseUp event
      /// </summary>
      /// <param name="e">Event data</param>
      public virtual void OnMouseUp(MouseEventArgs e)
      {
         if (!Enabled) return;

         if (MouseUp != null)
         {
            MouseUp(this, e);
         }

         if (ButtonMoreEnabled && ButtonMoreVisible && ButtonMorePressed && !Collapsed)
         {
            OnButtonMoreClick(EventArgs.Empty);
         }

         SetPressed(false);
         SetMorePressed(false);
      }

      #endregion

      #region IContainsRibbonComponents Members

      public IEnumerable<Component> GetAllChildComponents()
      {
         return Items.ToArray();
      }

      #endregion
   }
}
