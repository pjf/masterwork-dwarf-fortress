using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>
	/// Provides mouse functionality to RibbonTab, RibbonPanel and RibbonItem objects on a specified Control
	/// </summary>
	public class RibbonMouseSensor : IDisposable
	{

		#region Fields
		private Control _control;
		private Ribbon _ribbon;
		private List<RibbonTab> _tabs;
		private List<RibbonPanel> _panels;
		private List<RibbonItem> _items;
		private RibbonTab _tabLimit;
		private RibbonPanel _panelLimit;
		private IEnumerable<RibbonItem> _itemsLimit;
		private bool _disposed;
		private bool _suspended;
		private RibbonTab _hittedTab;
		private RibbonPanel _hittedPanel;
		private RibbonItem _hittedItem;
		private RibbonItem _hittedSubItem;
		private bool _hittedTabScrollLeft;
		private bool _hittedTabScrollRight;
		private RibbonTab _selectedTab;
		private RibbonPanel _selectedPanel;
		private RibbonItem _selectedItem;
		private RibbonItem _selectedSubItem;
		private RibbonItem _lastMouseDown;
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes inner fields
		/// </summary>
		private RibbonMouseSensor()
		{
			_tabs = new List<RibbonTab>();
			_panels = new List<RibbonPanel>();
			_items = new List<RibbonItem>();

		}

		/// <summary>
		/// Creates a new Empty Sensor
		/// </summary>
		/// <param name="control">Control to listen mouse events</param>
		/// <param name="ribbon">Ribbon that will be affected</param>
		public RibbonMouseSensor(Control control, Ribbon ribbon)
			: this()
		{
			if (control == null) throw new ArgumentNullException("control");
			if (ribbon == null) throw new ArgumentNullException("ribbon");

			_control = control;
			_ribbon = ribbon;

			AddHandlers();
		}

		/// <summary>
		/// Creates a new Sensor for specified objects
		/// </summary>
		/// <param name="control">Control to listen mouse events</param>
		/// <param name="ribbon">Ribbon that will be affected</param>
		/// <param name="tabs">Tabs that will be sensed</param>
		/// <param name="panels">Panels that will be sensed</param>
		/// <param name="items">Items that will be sensed</param>
		public RibbonMouseSensor(Control control, Ribbon ribbon, IEnumerable<RibbonTab> tabs, IEnumerable<RibbonPanel> panels, IEnumerable<RibbonItem> items)
			: this(control, ribbon)
		{
			if (tabs != null) Tabs.AddRange(tabs);
			if (panels != null) Panels.AddRange(panels);
			if (items != null) Items.AddRange(items);
		}

		/// <summary>
		/// Creates a new Sensor for the specified RibbonTab
		/// </summary>
		/// <param name="control">Control to listen to mouse events</param>
		/// <param name="ribbon">Ribbon that will be affected</param>
		/// <param name="tab">Tab that will be sensed, from which all panels and items will be extracted to sensing also.</param>
		public RibbonMouseSensor(Control control, Ribbon ribbon, RibbonTab tab)
			: this(control, ribbon)
		{
			Tabs.Add(tab);
			Panels.AddRange(tab.Panels);

			foreach (RibbonPanel panel in tab.Panels)
			{
				Items.AddRange(panel.Items);
			}
		}

		/// <summary>
		/// Creates a new Sensor for only the specified items
		/// </summary>
		/// <param name="control">Control to listen to mouse events</param>
		/// <param name="ribbon">Ribbon that will be affected</param>
		/// <param name="items">Items that will be sensed</param>
		public RibbonMouseSensor(Control control, Ribbon ribbon, IEnumerable<RibbonItem> itemsSource)
			: this(control, ribbon)
		{
			ItemsSource = itemsSource;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the control where the sensor listens to mouse events
		/// </summary>
		public Control Control
		{
			get { return _control; }
		}

		/// <summary>
		/// Gets if the sensor has already been 
		/// </summary>
		public bool Disposed
		{
			get { return _disposed; }
		}

		/// <summary>
		/// Gets the RibbonTab hitted by the last <see cref="HitTest"/>
		/// </summary>
		internal RibbonTab HittedTab
		{
			get { return _hittedTab; }
			set { _hittedTab = value; }
		}

		/// <summary>
		/// Gets if the test hit resulted on some scroll button of the hitted tab
		/// </summary>
		internal bool HittedTabScroll
		{
			get { return HittedTabScrollLeft || HittedTabScrollRight; }
		}

		/// <summary>
		/// Gets or sets if the last hit test resulted on the left scroll of the hitted tab
		/// </summary>
		internal bool HittedTabScrollLeft
		{
			get { return _hittedTabScrollLeft; }
			set { _hittedTabScrollLeft = value; }
		}

		/// <summary>
		/// Gets or sets if the last hit test resulted on the right scroll of the hitted tab
		/// </summary>
		internal bool HittedTabScrollRight
		{
			get { return _hittedTabScrollRight; }
			set { _hittedTabScrollRight = value; }
		}

		/// <summary>
		/// Gets the RibbonPanel hitted by the last <see cref="HitTest"/>
		/// </summary>
		internal RibbonPanel HittedPanel
		{
			get { return _hittedPanel; }
			set { _hittedPanel = value; }
		}

		/// <summary>
		/// Gets the RibbonItem hitted by the last <see cref="HitTest"/>
		/// </summary>
		internal RibbonItem HittedItem
		{
			get { return _hittedItem; }
			set { _hittedItem = value; }
		}

		/// <summary>
		/// Gets the RibbonItem (on other RibbonItem) hitted by the last <see cref="HitTest"/>
		/// </summary>
		internal RibbonItem HittedSubItem
		{
			get { return _hittedSubItem; }
			set { _hittedSubItem = value; }
		}

		/// <summary>
		/// Gets if the sensor is currently suspended
		/// </summary>
		public bool IsSupsended
		{
			get { return _suspended; }
		}

		/// <summary>
		/// Gets or ests the source of items what limits the sensing.
		/// If collection is null, all items on the <see cref="Items"/> property will be sensed.
		/// </summary>
		public IEnumerable<RibbonItem> ItemsSource
		{
			get { return _itemsLimit; }
			set { _itemsLimit = value; }
		}

		/// <summary>
		/// Gets the collection of items this sensor affects.
		/// Sensing can be limitated by the <see cref="ItemsLimit"/> property
		/// </summary>
		public List<RibbonItem> Items
		{
			get { return _items; }
		}

		/// <summary>
		/// Gets or sets the Panel that will be the limit to be sensed.
		/// If set to null, all panels in the <see cref="Panels"/> property will be sensed.
		/// </summary>
		public RibbonPanel PanelLimit
		{
			get { return _panelLimit; }
			set { _panelLimit = value; }
		}

		/// <summary>
		/// Gets the collection of panels this sensor affects.
		/// Sensing can be limitated by the <see cref="PanelLimit"/> property
		/// </summary>
		public List<RibbonPanel> Panels
		{
			get { return _panels; }
		}

		/// <summary>
		/// Gets the ribbon this sensor responds to
		/// </summary>
		public Ribbon Ribbon
		{
			get { return _ribbon; }
		}


		/// <summary>
		/// Gets or sets the last selected tab
		/// </summary>
		internal RibbonTab SelectedTab
		{
			get { return _selectedTab; }
			set { _selectedTab = value; }
		}


		/// <summary>
		/// Gets or sets the last selected panel
		/// </summary>
		internal RibbonPanel SelectedPanel
		{
			get { return _selectedPanel; }
			set { _selectedPanel = value; }
		}


		/// <summary>
		/// Gets or sets the last selected item
		/// </summary>
		internal RibbonItem SelectedItem
		{
			get { return _selectedItem; }
			set { _selectedItem = value; }
		}

		/// <summary>
		/// Gets or sets the last selected sub-item
		/// </summary>
		internal RibbonItem SelectedSubItem
		{
			get { return _selectedSubItem; }
			set { _selectedSubItem = value; }
		}


		/// <summary>
		/// Gets or sets the Tab that will be the only to be sensed. 
		/// If set to null, all tabs in the <see cref="Tabs"/> property will be sensed.
		/// </summary>
		public RibbonTab TabLimit
		{
			get { return _tabLimit; }
			set { _tabLimit = value; }
		}

		/// <summary>
		/// Gets the collection of tabs this sensor affects. 
		/// Sensing can be limitated by the <see cref="TabLimit"/> property
		/// </summary>
		public List<RibbonTab> Tabs
		{
			get { return _tabs; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds the necessary handlers to the control
		/// </summary>
		private void AddHandlers()
		{
			if (Control == null)
			{
				throw new ApplicationException("Control is Null, cant Add RibbonMouseSensor Handles");
			}

			Control.MouseMove += new MouseEventHandler(Control_MouseMove);
			Control.MouseLeave += new EventHandler(Control_MouseLeave);
			Control.MouseDown += new MouseEventHandler(Control_MouseDown);
			Control.MouseUp += new MouseEventHandler(Control_MouseUp);
			Control.MouseClick += new MouseEventHandler(Control_MouseClick);
			Control.MouseDoubleClick += new MouseEventHandler(Control_MouseDoubleClick);
			//Control.MouseEnter 
		}

		private void Control_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (IsSupsended || Disposed) return;

			#region Panel
			if (HittedPanel != null)
			{
				HittedPanel.OnDoubleClick(e);
			}
			#endregion

			#region Item

			if (HittedItem != null)
			{
				HittedItem.OnDoubleClick(e);
			}

			#endregion

			#region SubItem

			if (HittedSubItem != null)
			{
				HittedSubItem.OnDoubleClick(e);
			}

			#endregion

		}

		private void Control_MouseClick(object sender, MouseEventArgs e)
		{
			if (IsSupsended || Disposed) return;

			#region Panel
			if (HittedPanel != null)
			{
				HittedPanel.OnClick(e);
			}
			#endregion

			#region Item

			//Kevin Carbis - Added _lastMouseDown variable to track if our click originated with this control.
			//Sometimes when scrolling your mouse moves off the thumb while dragging and your mouseup event
			//is not the same control that you started with.  This omits firing the click event on the control you
			//eventually release the mouse over if it wasn't the control you started with.
			if (HittedItem != null && HittedItem == _lastMouseDown)
			{
				//Kevin Carbis - this fixes the focus problem with textboxes when a different item is clicked
				//and the edit box is still visible. This will now close the edit box for the textbox that previously
				//had the focus. Otherwise the first click on a button would close the textbox and you would have to click twice.
				//Control.Focus();
				if (_ribbon.ActiveTextBox != null)
					(_ribbon.ActiveTextBox as RibbonTextBox).EndEdit();

				//foreach (RibbonPanel pnl in HittedItem.OwnerTab.Panels)
				//{
				//   foreach (RibbonItem itm in pnl.Items)
				//   {
				//      if (itm is RibbonTextBox && itm != HittedItem)
				//      {
				//         RibbonTextBox txt = (RibbonTextBox)itm;
				//         txt.EndEdit();
				//      }
				//   }
				//}

				HittedItem.OnClick(e);
			}

			#endregion

			#region SubItem

			if (HittedSubItem != null)
			{
				HittedSubItem.OnClick(e);
			}

			#endregion
		}

		/// <summary>
		/// Handles the MouseUp event on the control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseUp(object sender, MouseEventArgs e)
		{
			if (IsSupsended || Disposed) return;

			#region Tab Scrolls
			if (HittedTab != null)
			{
				if (HittedTab.ScrollLeftVisible)
				{
					HittedTab.SetScrollLeftPressed(false);
					Control.Invalidate(HittedTab.ScrollLeftBounds);
				}
				if (HittedTab.ScrollRightVisible)
				{
					HittedTab.SetScrollRightPressed(false);
					Control.Invalidate(HittedTab.ScrollRightBounds);
				}
			}
			#endregion

			#region Panel
			if (HittedPanel != null)
			{
				HittedPanel.SetPressed(false);
				HittedPanel.OnMouseUp(e);
				Control.Invalidate(HittedPanel.Bounds);
			}
			#endregion

			#region Item

			if (HittedItem != null)
			{
				HittedItem.SetPressed(false);
				HittedItem.OnMouseUp(e);
				Control.Invalidate(HittedItem.Bounds);
			}

			#endregion

			#region SubItem

			if (HittedSubItem != null)
			{
				HittedSubItem.SetPressed(false);
				HittedSubItem.OnMouseUp(e);
				Control.Invalidate(Rectangle.Intersect(HittedItem.Bounds, HittedSubItem.Bounds));
			}

			#endregion
		}

		/// <summary>
		/// Handles the MouseDown on the control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsSupsended || Disposed) return;

			HitTest(e.Location);
			_lastMouseDown = HittedItem;

			#region Tab Scrolls
			if (HittedTab != null)
			{
				if (HittedTabScrollLeft)
				{
					HittedTab.SetScrollLeftPressed(true);
					Control.Invalidate(HittedTab.ScrollLeftBounds);
				}

				if (HittedTabScrollRight)
				{
					HittedTab.SetScrollRightPressed(true);
					Control.Invalidate(HittedTab.ScrollRightBounds);
				}
			}
			#endregion

			#region Panel
			if (HittedPanel != null)
			{
				HittedPanel.SetPressed(true);
				HittedPanel.OnMouseDown(e);
				Control.Invalidate(HittedPanel.Bounds);
			}
			#endregion

			#region Item

			if (HittedItem != null)
			{
				HittedItem.SetPressed(true);
				HittedItem.OnMouseDown(e);
				Control.Invalidate(HittedItem.Bounds);
			}

			#endregion

			#region SubItem

			if (HittedSubItem != null)
			{
				HittedSubItem.SetPressed(true);
				HittedSubItem.OnMouseDown(e);
				Control.Invalidate(Rectangle.Intersect(HittedItem.Bounds, HittedSubItem.Bounds));
			}

			#endregion
		}

		/// <summary>
		/// Handles the MouseLeave on the control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseLeave(object sender, EventArgs e)
		{
			if (IsSupsended || Disposed) return;
		}

		/// <summary>
		/// Handles the MouseMove on the control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsSupsended || Disposed) return;
			//Console.WriteLine("MouseMove " + Control.Name);
			HitTest(e.Location);

			#region Selected ones

			if (SelectedPanel != null && SelectedPanel != HittedPanel)
			{
				SelectedPanel.SetSelected(false);
				SelectedPanel.OnMouseLeave(e);
				Control.Invalidate(SelectedPanel.Bounds);
			}

			if (SelectedItem != null && SelectedItem != HittedItem)
			{
				SelectedItem.SetSelected(false);
				SelectedItem.OnMouseLeave(e);
				Control.Invalidate(SelectedItem.Bounds);
			}

			if (SelectedSubItem != null && SelectedSubItem != HittedSubItem)
			{
				SelectedSubItem.SetSelected(false);
				SelectedSubItem.OnMouseLeave(e);
				Control.Invalidate(Rectangle.Intersect(SelectedItem.Bounds, SelectedSubItem.Bounds));
			}

			#endregion

			#region Tab Scrolls
			if (HittedTab != null)
			{
				if (HittedTab.ScrollLeftVisible)
				{
					HittedTab.SetScrollLeftSelected(HittedTabScrollLeft);
					Control.Invalidate(HittedTab.ScrollLeftBounds);
				}
				if (HittedTab.ScrollRightVisible)
				{
					HittedTab.SetScrollRightSelected(HittedTabScrollRight);
					Control.Invalidate(HittedTab.ScrollRightBounds);
				}
			}
			#endregion

			#region Panel
			if (HittedPanel != null)
			{
				if (HittedPanel == SelectedPanel)
				{
					HittedPanel.OnMouseMove(e);
				}
				else
				{

					HittedPanel.SetSelected(true);
					HittedPanel.OnMouseEnter(e);
					Control.Invalidate(HittedPanel.Bounds);
				}
			}
			#endregion

			#region Item

			if (HittedItem != null)
			{
				if (HittedItem == SelectedItem)
				{
					HittedItem.OnMouseMove(e);
				}
				else
				{

					HittedItem.SetSelected(true);
					HittedItem.OnMouseEnter(e);
					Control.Invalidate(HittedItem.Bounds);
				}
			}

			#endregion

			#region SubItem

			if (HittedSubItem != null)
			{
				if (HittedSubItem == SelectedSubItem)
				{
					HittedSubItem.OnMouseMove(e);
				}
				else
				{
					HittedSubItem.SetSelected(true);
					HittedSubItem.OnMouseEnter(e);
					Control.Invalidate(Rectangle.Intersect(HittedItem.Bounds, HittedSubItem.Bounds));
				}
			}

			#endregion
		}

		/// <summary>
		/// Performs a hit-test and specifies hitted objects on properties: <see cref="HittedPanel"/>, 
		/// <see cref="HittedTab"/>, <see cref="HittedItem"/> and <see cref="HittedSubItem"/>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		internal void HitTest(Point p)
		{
			SelectedTab = HittedTab;
			SelectedPanel = HittedPanel;
			SelectedItem = HittedItem;
			SelectedSubItem = HittedSubItem;

			HittedTab = null;
			HittedTabScrollLeft = false;
			HittedTabScrollRight = false;
			HittedPanel = null;
			HittedItem = null;
			HittedSubItem = null;

			#region Tabs
			if (TabLimit != null)
			{
				if (TabLimit.TabContentBounds.Contains(p))
				{
					HittedTab = TabLimit;
				}
			}
			else
			{
				foreach (RibbonTab tab in Tabs)
				{
					if (tab.TabContentBounds.Contains(p))
					{
						HittedTab = tab; break;
					}
				}
			}
			#endregion

			#region TabScrolls

			if (HittedTab != null)
			{
				HittedTabScrollLeft = HittedTab.ScrollLeftVisible && HittedTab.ScrollLeftBounds.Contains(p);
				HittedTabScrollRight = HittedTab.ScrollRightVisible && HittedTab.ScrollRightBounds.Contains(p);
			}

			#endregion

			if (!HittedTabScroll)
			{
				#region Panels

				if (PanelLimit != null)
				{
					if (PanelLimit.Bounds.Contains(p))
					{
						HittedPanel = PanelLimit;
					}
				}
				else
				{
					foreach (RibbonPanel pnl in Panels)
					{
						if (pnl.Bounds.Contains(p))
						{
							HittedPanel = pnl; break;
						}
					}
				}

				#endregion

				#region Item

				IEnumerable<RibbonItem> items = Items;

				if (ItemsSource != null) items = ItemsSource;

				foreach (RibbonItem item in items)
				{
					if (item.OwnerPanel != null && item.OwnerPanel.OverflowMode && !(Control is RibbonPanelPopup)) continue;

					if (item.Bounds.Contains(p))
					{
						HittedItem = item; break;
					}
				}

				#endregion

				#region Subitem

				IContainsSelectableRibbonItems container = HittedItem as IContainsSelectableRibbonItems;
				IScrollableRibbonItem scrollable = HittedItem as IScrollableRibbonItem;


				if (container != null)
				{
					Rectangle sensibleBounds = scrollable != null ? scrollable.ContentBounds : HittedItem.Bounds;

					foreach (RibbonItem item in container.GetItems())
					{
						Rectangle actualBounds = item.Bounds; actualBounds.Intersect(sensibleBounds);

						if (actualBounds.Contains(p))
						{
							HittedSubItem = item;
						}
					}
				}

				#endregion
			}
		}

		/// <summary>
		/// Removes the added handlers to the Control
		/// </summary>
		private void RemoveHandlers()
		{
            //Do not Change State because if Text or Image of RibbonItem is Changed in Runtime RemoveHandlers() is called
            /*
             foreach (RibbonItem item in Items)
            {
               item.SetSelected(false);
               item.SetPressed(false);
            }
            */

			Control.MouseMove -= Control_MouseMove;
			Control.MouseLeave -= Control_MouseLeave;
			Control.MouseDown -= Control_MouseDown;
			Control.MouseUp -= Control_MouseUp;

            // ADDED
            Control.MouseClick -= Control_MouseClick;
            Control.MouseDoubleClick -= Control_MouseDoubleClick;
		}

		/// <summary>
		/// Resumes the sensing after being suspended by <see cref="Suspend"/>
		/// </summary>
		public void Resume()
		{
			_suspended = false;
		}

		/// <summary>
		/// Suspends sensing until <see cref="Resume"/> is called
		/// </summary>
		public void Suspend()
		{
			_suspended = true;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_disposed = true;
			RemoveHandlers();
		}

		#endregion
	}
}
