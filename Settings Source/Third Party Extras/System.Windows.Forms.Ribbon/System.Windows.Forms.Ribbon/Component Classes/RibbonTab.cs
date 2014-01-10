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
	/// <summary>
	/// Represents a tab that can contain RibbonPanel objects
	/// </summary>
	[DesignTimeVisible(false)]
	[Designer(typeof(RibbonTabDesigner))]
	public class RibbonTab : Component, IRibbonElement, IRibbonToolTip, IContainsRibbonComponents
	{
		#region Fields
		private RibbonPanelCollection _panels;
		private Rectangle _tabBounds;
		private Rectangle _tabContentBounds;
		private Ribbon _owner;
		private bool _pressed;
		private bool _selected;
		private bool _active;
		private object _tag;
		private string _value;
		private string _text;
		private RibbonContext _context;
		private bool _scrollLeftVisible;
		private Rectangle _scrollLeftBounds;
		private bool _scrollLeftSelected;
		private bool _scrollLeftPressed;
		private Rectangle _scrollRightBounds;
		private bool _scrollRightSelected;
		private bool _scrollRightVisible;
		private bool _scrollRightPressed;
		private int _offset;
		private bool _visible = true;

		RibbonToolTip _TT;
		private string _tooltip;

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

		#endregion

		#region Ctor

		public RibbonTab()
		{
			_panels = new RibbonPanelCollection(this);

			//Initialize the ToolTip for this Item
			_TT = new RibbonToolTip(this);
			_TT.InitialDelay = 100;
			_TT.AutomaticDelay = 800;
			_TT.AutoPopDelay = 8000;
			_TT.UseAnimation = true;
			_TT.Active = false;
			_TT.Popup += new PopupEventHandler(_TT_Popup);
		}

      public RibbonTab(string text)
         : this()
      {
         _text = text;
      }
      
      /// <summary>
		/// Creates a new RibbonTab
		/// </summary>
      [Obsolete("Use 'public RibbonTab(string text)' instead!")]
      public RibbonTab(Ribbon owner, string text)
         : this(text)
		{
		}

      protected override void Dispose(bool disposing)
      {
         if (disposing && RibbonDesigner.Current == null)
         {
             // ADDED
             _TT.Popup -= _TT_Popup;

            _TT.Dispose();
            foreach (RibbonPanel p in _panels)
               p.Dispose();
         }

         base.Dispose(disposing);
      }

		#endregion

		#region Events


		public event EventHandler ScrollRightVisibleChanged;
		public event EventHandler ScrollRightPressedChanged;
		public event EventHandler ScrollRightBoundsChanged;
		public event EventHandler ScrollRightSelectedChanged;
		public event EventHandler ScrollLeftVisibleChanged;
		public event EventHandler ScrollLeftPressedChanged;
		public event EventHandler ScrollLeftSelectedChanged;
		public event EventHandler ScrollLeftBoundsChanged;
		public event EventHandler TabBoundsChanged;
		public event EventHandler TabContentBoundsChanged;
		public event EventHandler OwnerChanged;
		public event EventHandler PressedChanged;
		public event EventHandler ActiveChanged;
		public event EventHandler TextChanged;
		public event EventHandler ContextChanged;
		public virtual event RibbonElementPopupEventHandler ToolTipPopUp;

		#endregion

		#region Props

		/// <summary>
		/// Gets if the right-side scroll button is currently visible
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollRightVisible
		{
			get { return _scrollRightVisible; }
		}

		/// <summary>
		/// Gets if the right-side scroll button is currently selected
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollRightSelected
		{
			get { return _scrollRightSelected; }
		}

		/// <summary>
		/// Gets if the right-side scroll button is currently pressed
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollRightPressed
		{
			get { return _scrollRightPressed; }
		}

		/// <summary>
		/// Gets if the right-side scroll button bounds
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Rectangle ScrollRightBounds
		{
			get { return _scrollRightBounds; }
		}

		/// <summary>
		/// Gets if the left scroll button is currently visible
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollLeftVisible
		{
			get { return _scrollLeftVisible; }
		}

		/// <summary>
		/// Gets if the left scroll button bounds
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Rectangle ScrollLeftBounds
		{
			get { return _scrollLeftBounds; }
		}

		/// <summary>
		/// Gets if the left scroll button is currently selected
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollLeftSelected
		{
			get { return _scrollLeftSelected; }
		}

		/// <summary>
		/// Gets if the left scroll button is currently pressed
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public bool ScrollLeftPressed
		{
			get { return _scrollLeftPressed; }
		}

		/// <summary>
		/// Gets the <see cref="TabBounds"/> property value
		/// </summary>
		public Rectangle Bounds
		{
			get { return TabBounds; }
		}

		/// <summary>
		/// Gets the collection of panels that belong to this tab
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public RibbonPanelCollection Panels
		{
			get
			{
				return _panels;
			}
		}

		/// <summary>
		/// Gets the bounds of the little tab showing the text
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Drawing.Rectangle TabBounds
		{
			get
			{
				return _tabBounds;
			}
		}

		/// <summary>
		/// Gets the bounds of the tab content on the Ribbon
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Drawing.Rectangle TabContentBounds
		{
			get
			{
				return _tabContentBounds;
			}
		}

		/// <summary>
		/// Gets the Ribbon that contains this tab
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
		/// Gets a value indicating whether the state of the tab is being pressed by the mouse or a key
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Pressed
		{
			get
			{
				return _pressed;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the tab is selected
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return _selected;
			}
		}

		/// <summary>
		/// Gets a value indicating if the tab is currently the active tab
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Active
		{
			get
			{
				return _active;
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
		/// Gets or sets the custom string data associated with this control
		/// </summary>
		[DefaultValue(null)]
		[DescriptionAttribute("A string field for associating custom data for this control")]
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		/// <summary>
		/// Gets or sets the text that is to be displayed on the tab
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

				OnTextChanged(EventArgs.Empty);

				if (Owner != null) Owner.OnRegionsChanged();
			}
		}

		/// <summary>
		/// Gets a value indicating whether the tab is attached to a  Context
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Contextual
		{
			get
			{
				return _context != null;
			}
		}

		/// <summary>
		/// Gets or sets the context this tab belongs to
		/// </summary>
		/// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RibbonContext Context
		{
			get
			{
				return _context;
			}
		}

		/// <summary>
		/// Gets or sets the visibility of this tab
		/// </summary>
		/// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
		[Localizable(true), DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
				Owner.UpdateRegions();
			}
		}

		/// <summary>
		/// Gets or sets the tool tip title
		/// </summary>
		[DefaultValue("")]
		public string ToolTipTitle
		{
			get
			{
				return _TT.ToolTipTitle;
			}
			set
			{
				_TT.ToolTipTitle = value;
			}
		}

		/// <summary>
		/// Gets or sets the image of the tool tip
		/// </summary>
		[DefaultValue(ToolTipIcon.None)]
		public ToolTipIcon ToolTipIcon
		{
			get
			{
				return _TT.ToolTipIcon;
			}
			set
			{
				_TT.ToolTipIcon = value;
			}
		}

		/// <summary>
		/// Gets or sets the tool tip text
		/// </summary>
		[DefaultValue(null)]
		[Localizable(true)]
		public string ToolTip
		{
			get
			{
				return _tooltip;
			}
			set
			{
				_tooltip = value;
			}
		}

		/// <summary>
		/// Gets or sets the tool tip image
		/// </summary>
		[DefaultValue(null)]
		[Localizable(true)]
		public Image ToolTipImage
		{
			get
			{
				return _TT.ToolTipImage;
			}
			set
			{
				_TT.ToolTipImage = value;
			}
		}

		#endregion

		#region IRibbonElement Members

		public void OnPaint(object sender, RibbonElementPaintEventArgs e)
		{
			if (Owner == null) return;

			Owner.Renderer.OnRenderRibbonTab(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));
			Owner.Renderer.OnRenderRibbonTabText(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));

			if (Active && (!Owner.Minimized || (Owner.Minimized && Owner.Expanded)))
			{
				foreach (RibbonPanel panel in Panels)
				{
					if (panel.Visible)
						panel.OnPaint(this, new RibbonElementPaintEventArgs(e.Clip, e.Graphics, panel.SizeMode, e.Control));
				}
			}

			Owner.Renderer.OnRenderTabScrollButtons(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));
		}

		/// <summary>
		/// This method is not relevant for this class
		/// </summary>
		/// <exception cref="NotSupportedException">Always</exception>
		public void SetBounds(Rectangle bounds)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Sets the context of the tab
		/// </summary>
		/// <param name="context"></param>
		public void SetContext(RibbonContext context)
		{
			bool trigger = !context.Equals(context);

			if (trigger)
				OnContextChanged(EventArgs.Empty);

			_context = context;

			throw new NotImplementedException();
		}

		/// <summary>
		/// Measures the size of the tab. The tab content bounds is measured by the Ribbon control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
		{
			if (!Visible && !Owner.IsDesignMode()) return new Size(0, 0);

            Size textSize = TextRenderer.MeasureText(Text, Owner.RibbonTabFont);
			return textSize;
		}

		/// <summary>
		/// Sets the value of the Owner Property
		/// </summary>
		internal void SetOwner(Ribbon owner)
		{
			_owner = owner;

			Panels.SetOwner(owner);

			OnOwnerChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the Pressed property
		/// </summary>
		/// <param name="pressed">Value that indicates if the element is pressed</param>
		internal void SetPressed(bool pressed)
		{
			_pressed = pressed;

			OnPressedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the Selected property
		/// </summary>
		/// <param name="selected">Value that indicates if the element is selected</param>
		internal void SetSelected(bool selected)
		{
			_selected = selected;

			if (selected)
			{
				OnMouseEnter(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
			}
			else
			{
				OnMouseLeave(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
			}
		}

		#endregion

		#region Method Triggers

		public void OnContextChanged(EventArgs e)
		{
			if (ContextChanged != null)
			{
				ContextChanged(this, e);
			}
		}

		public void OnTextChanged(EventArgs e)
		{
			if (TextChanged != null)
			{
				TextChanged(this, e);
			}
		}

		public void OnActiveChanged(EventArgs e)
		{
			if (ActiveChanged != null)
			{
				ActiveChanged(this, e);
			}
		}

		public void OnPressedChanged(EventArgs e)
		{
			if (PressedChanged != null)
			{
				PressedChanged(this, e);
			}
		}

		public void OnOwnerChanged(EventArgs e)
		{
			if (OwnerChanged != null)
			{
				OwnerChanged(this, e);
			}
		}

		public void OnTabContentBoundsChanged(EventArgs e)
		{
			if (TabContentBoundsChanged != null)
			{
				TabContentBoundsChanged(this, e);
			}
		}

		public void OnTabBoundsChanged(EventArgs e)
		{
			if (TabBoundsChanged != null)
			{
				TabBoundsChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollRightVisibleChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollRightVisibleChanged(EventArgs e)
		{
			if (ScrollRightVisibleChanged != null)
			{
				ScrollRightVisibleChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollRightPressedChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollRightPressedChanged(EventArgs e)
		{
			if (ScrollRightPressedChanged != null)
			{
				ScrollRightPressedChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollRightBoundsChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollRightBoundsChanged(EventArgs e)
		{
			if (ScrollRightBoundsChanged != null)
			{
				ScrollRightBoundsChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollRightSelectedChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollRightSelectedChanged(EventArgs e)
		{
			if (ScrollRightSelectedChanged != null)
			{
				ScrollRightSelectedChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollLeftVisibleChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollLeftVisibleChanged(EventArgs e)
		{
			if (ScrollLeftVisibleChanged != null)
			{
				ScrollLeftVisibleChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollLeftPressedChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollLeftPressedChanged(EventArgs e)
		{
			if (ScrollLeftPressedChanged != null)
			{
				ScrollLeftPressedChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollLeftBoundsChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollLeftBoundsChanged(EventArgs e)
		{
			if (ScrollLeftBoundsChanged != null)
			{
				ScrollLeftBoundsChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="ScrollLeftSelectedChanged"/> event
		/// </summary>
		/// <param name="e">Event data</param>
		public void OnScrollLeftSelectedChanged(EventArgs e)
		{
			if (ScrollLeftSelectedChanged != null)
			{
				ScrollLeftSelectedChanged(this, e);
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Sets the tab as active without sending the message to the Ribbon
		/// </summary>
		internal void SetActive(bool active)
		{
			bool trigger = _active != active;

			_active = active;

			if (trigger)
				OnActiveChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the TabBounds property
		/// </summary>
		/// <param name="tabBounds">Rectangle representing the bounds of the tab</param>
		internal void SetTabBounds(Rectangle tabBounds)
		{
			bool tigger = _tabBounds != tabBounds;

			_tabBounds = tabBounds;

			OnTabBoundsChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the TabContentBounds
		/// </summary>
		/// <param name="tabContentBounds">Rectangle representing the bounds of the tab's content</param>
		internal void SetTabContentBounds(Rectangle tabContentBounds)
		{
			bool trigger = _tabContentBounds != tabContentBounds;

			_tabContentBounds = tabContentBounds;

			OnTabContentBoundsChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Gets the panel with the larger width and the specified size mode
		/// </summary>
		/// <param name="size">Size mode of panel to search</param>
		/// <returns>Larger panel. Null if none of the specified size mode</returns>
		private RibbonPanel GetLargerPanel(RibbonElementSizeMode size)
		{
			RibbonPanel result = null;

			foreach (RibbonPanel panel in Panels)
			{
				if (panel.SizeMode != size) continue;

				if (result == null) result = panel;

				if (panel.Bounds.Width > result.Bounds.Width)
				{
					result = panel;
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the panel with a larger size
		/// </summary>
		/// <returns></returns>
		private RibbonPanel GetLargerPanel()
		{
			RibbonPanel largeLarger = GetLargerPanel(RibbonElementSizeMode.Large);

			if (largeLarger != null) return largeLarger;

			RibbonPanel mediumLarger = GetLargerPanel(RibbonElementSizeMode.Medium);

			if (mediumLarger != null) return mediumLarger;

			RibbonPanel compactLarger = GetLargerPanel(RibbonElementSizeMode.Compact);

			if (compactLarger != null) return compactLarger;

			RibbonPanel overflowLarger = GetLargerPanel(RibbonElementSizeMode.Overflow);

			if (overflowLarger != null) return overflowLarger;

			return null;
		}

		private bool AllPanelsOverflow()
		{

			foreach (RibbonPanel panel in Panels)
			{
				if (panel.SizeMode != RibbonElementSizeMode.Overflow)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Updates the regions of the panels and its contents
		/// </summary>
		internal void UpdatePanelsRegions()
		{
			if (Panels.Count == 0) return;

			if (!Owner.IsDesignMode())
				_offset = 0;

			int curRight = TabContentBounds.Left + Owner.PanelPadding.Left + _offset;
			int curLeft = TabContentBounds.Right - Owner.PanelPadding.Right;
			int panelsTop = TabContentBounds.Top + Owner.PanelPadding.Top;

			using (Graphics g = Owner.CreateGraphics())
			{
				//Check all at full size
				foreach (RibbonPanel panel in Panels)
				{
					if (panel.Visible && Owner.RightToLeft == RightToLeft.No)
					{
						RibbonElementSizeMode sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;
						//Set the bounds of the panel to let it know it's height
						panel.SetBounds(new Rectangle(0, 0, 1, TabContentBounds.Height - Owner.PanelPadding.Vertical));

						///Size of the panel
						Size size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

						///Creates the bounds of the panel
						Rectangle bounds = new Rectangle(
							 curRight, panelsTop,
							 size.Width, size.Height);

						///Set the bounds of the panel
						panel.SetBounds(bounds);

						///Let the panel know what size we have decided for it
						panel.SetSizeMode(sMode);

						///Update curLeft
						curRight = bounds.Right + 1 + Owner.PanelSpacing;
					}
					else if (panel.Visible && Owner.RightToLeft == RightToLeft.Yes)
					{
						RibbonElementSizeMode sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;

						//Set the bounds of the panel to let it know it's height
						panel.SetBounds(new Rectangle(0, 0, 1, TabContentBounds.Height - Owner.PanelPadding.Vertical));

						///Size of the panel
						Size size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

						curLeft -= size.Width + Owner.PanelSpacing;

						///Creates the bounds of the panel
						Rectangle bounds = new Rectangle(
							 curLeft, panelsTop,
							 size.Width, size.Height);

						///Set the bounds of the panel
						panel.SetBounds(bounds);

						///Let the panel know what size we have decided for it
						panel.SetSizeMode(sMode);

						///Update curLeft
						curLeft = bounds.Left - 1 - Owner.PanelSpacing;
					}
					else
					{
						panel.SetBounds(Rectangle.Empty);
					}
				}

				if (!Owner.IsDesignMode())
				{
					while (curRight > TabContentBounds.Right && !AllPanelsOverflow())
					{
						#region Down grade the larger panel one position

						RibbonPanel larger = GetLargerPanel();

						if (larger.SizeMode == RibbonElementSizeMode.Large)
							larger.SetSizeMode(RibbonElementSizeMode.Medium);
						else if (larger.SizeMode == RibbonElementSizeMode.Medium)
							larger.SetSizeMode(RibbonElementSizeMode.Compact);
						else if (larger.SizeMode == RibbonElementSizeMode.Compact)
							larger.SetSizeMode(RibbonElementSizeMode.Overflow);

						Size size = larger.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, larger.SizeMode));

						larger.SetBounds(new Rectangle(larger.Bounds.Location, new Size(size.Width + Owner.PanelMargin.Horizontal, size.Height)));

						#endregion

						///Reset x-axis reminder
						curRight = TabContentBounds.Left + Owner.PanelPadding.Left;

						///Re-arrange location because of the new bounds
						foreach (RibbonPanel panel in Panels)
						{
							Size s = panel.Bounds.Size;
							panel.SetBounds(new Rectangle(new Point(curRight, panelsTop), s));
							curRight += panel.Bounds.Width + 1 + Owner.PanelSpacing;
						}

					}
				}

				///Update regions of all panels
				foreach (RibbonPanel panel in Panels)
				{
					panel.UpdateItemsRegions(g, panel.SizeMode);
				}
			}

			UpdateScrollBounds();
		}

		/// <summary>
		/// Updates the bounds of the scroll bounds
		/// </summary>
		private void UpdateScrollBounds()
		{
			int w = 13;
			bool scrBuffer = _scrollRightVisible;
			bool sclBuffer = _scrollLeftVisible;
			Rectangle rrBuffer = _scrollRightBounds;
			Rectangle rlBuffer = _scrollLeftBounds;

			if (Panels.Count == 0) return;



			if (Panels[Panels.Count - 1].Bounds.Right > TabContentBounds.Right)
			{
				_scrollRightVisible = true;
			}
			else
			{
				_scrollRightVisible = false;
			}

			if (_scrollRightVisible != scrBuffer)
			{
				OnScrollRightVisibleChanged(EventArgs.Empty);
			}



			if (_offset < 0)
			{
				_scrollLeftVisible = true;
			}
			else
			{
				_scrollLeftVisible = false;
			}

			if (_scrollRightVisible != scrBuffer)
			{
				OnScrollLeftVisibleChanged(EventArgs.Empty);
			}

			if (_scrollLeftVisible || _scrollRightVisible)
			{
				_scrollRightBounds = Rectangle.FromLTRB(
					 Owner.ClientRectangle.Right - w,
					 TabContentBounds.Top,
					 Owner.ClientRectangle.Right,
					 TabContentBounds.Bottom);

				_scrollLeftBounds = Rectangle.FromLTRB(
					 0,
					 TabContentBounds.Top,
					 w,
					 TabContentBounds.Bottom);

				if (_scrollRightBounds != rrBuffer)
				{
					OnScrollRightBoundsChanged(EventArgs.Empty);
				}

				if (_scrollLeftBounds != rlBuffer)
				{
					OnScrollLeftBoundsChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Overriden. Returns a string representation of the tab
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("Tab: {0}", Text);
		}

		/// <summary>
		/// Raises the MouseEnter event
		/// </summary>
		/// <param name="e">Event data</param>
		public virtual void OnMouseEnter(MouseEventArgs e)
		{
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
         _TT.Active = false;

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
         if (MouseMove != null)
         {
            MouseMove(this, e);
         }
         if (!_TT.Active && !string.IsNullOrEmpty(this.ToolTip))  // ToolTip should be working without title as well - to get Office 2007 Look & Feel
         {
            if (this.ToolTip != _TT.GetToolTip(this.Owner))
            {
               _TT.SetToolTip(this.Owner, this.ToolTip);
            }
            _TT.Active = true;
         }
      }

		/// <summary>
		/// Sets the value of the <see cref="ScrollLeftPressed"/>
		/// </summary>
		/// <param name="pressed"></param>
		internal void SetScrollLeftPressed(bool pressed)
		{
			_scrollLeftPressed = pressed;

			if (pressed)
				ScrollLeft();

			OnScrollLeftPressedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the <see cref="ScrollLeftSelected"/>
		/// </summary>
		/// <param name="selected"></param>
		internal void SetScrollLeftSelected(bool selected)
		{
			_scrollLeftSelected = selected;

			OnScrollLeftSelectedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the <see cref="ScrollRightPressed"/>
		/// </summary>
		/// <param name="pressed"></param>
		internal void SetScrollRightPressed(bool pressed)
		{
			_scrollRightPressed = pressed;

			if (pressed) ScrollRight();

			OnScrollRightPressedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Sets the value of the <see cref="ScrollRightSelected"/>
		/// </summary>
		/// <param name="selected"></param>
		internal void SetScrollRightSelected(bool selected)
		{
			_scrollRightSelected = selected;

			OnScrollRightSelectedChanged(EventArgs.Empty);
		}

		/// <summary>
		/// Presses the lef-scroll button
		/// </summary>
		public void ScrollLeft()
		{
			ScrollOffset(50);
		}

		/// <summary>
		/// Presses the left-scroll button
		/// </summary>
		public void ScrollRight()
		{
			ScrollOffset(-50);
		}

		public void ScrollOffset(int amount)
		{
			_offset += amount;

			foreach (RibbonPanel p in Panels)
			{
				p.SetBounds(new Rectangle(p.Bounds.Left + amount,
					 p.Bounds.Top, p.Bounds.Width, p.Bounds.Height));
			}

			if (Site != null && Site.DesignMode)
				UpdatePanelsRegions();

			UpdateScrollBounds();

			Owner.Invalidate();
		}

		private void _TT_Popup(object sender, PopupEventArgs e)
		{
			if (ToolTipPopUp != null)
			{
				ToolTipPopUp(sender, new RibbonElementPopupEventArgs(this, e));
				if (this.ToolTip != _TT.GetToolTip(this.Owner))
					_TT.SetToolTip(this.Owner, this.ToolTip);
			}
		}

		#endregion

		#region IContainsRibbonComponents Members

		public IEnumerable<Component> GetAllChildComponents()
		{
			return Panels.ToArray();
		}

		#endregion
	}
}
