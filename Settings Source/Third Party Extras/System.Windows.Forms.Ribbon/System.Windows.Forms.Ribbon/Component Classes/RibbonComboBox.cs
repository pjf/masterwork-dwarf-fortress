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
   [Designer(typeof(RibbonComboBoxDesigner))]
   public class RibbonComboBox
       : RibbonTextBox, IContainsRibbonComponents, IDropDownRibbonItem
   {
      #region Fields

      private RibbonItemCollection _dropDownItems;
      private Rectangle _dropDownBounds;
      private bool _dropDownSelected;
      private bool _dropDownPressed;
      private bool _dropDownVisible;
      private bool _dropDownResizable;
      private int _dropDownMaxHeight;
      private bool _iconsBar;
      private bool _DropDownVisible;
      // Steve
      private RibbonItem _selectedItem;

      private Set<RibbonItem> _assignedHandlers = new Set<RibbonItem>();

      #endregion

      #region Events

      /// <summary>
      /// Raised when the DropDown is about to be displayed
      /// </summary>
      public event EventHandler DropDownShowing;

      public event RibbonItemEventHandler DropDownItemClicked;
      public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);

      #endregion

      #region Ctor

      public RibbonComboBox()
      {
         _dropDownItems = new RibbonItemCollection();
         _dropDownVisible = true;
         AllowTextEdit = true;
         _iconsBar = true;
         _dropDownMaxHeight = 0;
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            RemoveHandlers();
         }
         base.Dispose(disposing);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the maximum height for the dropdown window.  0 = autosize.  If the size is smaller than the contents then scrollbars will be shown.
      /// </summary>
      [DefaultValue(0)]
      [Description("Gets or sets the maximum height for the dropdown window.  0 = Autosize.  If the size is smaller than the contents then scrollbars will be shown.")]
      public int DropDownMaxHeight
      {
         get { return _dropDownMaxHeight; }
         set { _dropDownMaxHeight = value; }
      }

      /// <summary>
      /// Gets or sets a value indicating if the DropDown portion of the combo box is currently shown.
      /// </summary>
      [DefaultValue(false)]
      [Description("Indicates if the dropdown window is currently visible")]
      public bool DropDownVisible
      {
         get { return _DropDownVisible; }
      }

      /// <summary>
      /// Gets or sets a value indicating if the DropDown should be resizable
      /// </summary>
      [DefaultValue(false)]
      [Description("Makes the DropDown resizable with a grip on the corner")]
      public bool DropDownResizable
      {
         get { return _dropDownResizable; }
         set { _dropDownResizable = value; }
      }

      /// <summary>
      /// Overriden.
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public override Rectangle TextBoxTextBounds
      {
         get
         {
            Rectangle r = base.TextBoxTextBounds;

            r.Width -= DropDownButtonBounds.Width;

            return r;
         }
      }

      /// <summary>
      /// Gets the collection of items to be displayed on the dropdown
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public RibbonItemCollection DropDownItems
      {
         get { return _dropDownItems; }
      }

      // Steve
      /// <summary>
      /// Gets the selected of item on the dropdown
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public RibbonItem SelectedItem
      {
         get
         {
            if (_selectedItem == null)
            {
               return null;
            }
            else
            {
               if (_dropDownItems.Contains(_selectedItem))
               {
                  return _selectedItem;
               }
               else
               {
                  _selectedItem = null;
                  return null;
               }
            }
         }
         //Steve
         set
         {
            if (value.GetType().BaseType == typeof(RibbonItem))
            {
               if (_dropDownItems.Contains(value))
               {
                  _selectedItem = value;
                  TextBoxText = _selectedItem.Text;
               }
               else
               {
						//_dropDownItems.Add(value);
                  _selectedItem = value;
                  TextBoxText = _selectedItem.Text;
               }
            }
         }
      }
      // Kevin
      /// <summary>
      /// Gets or sets the value of selected item on the dropdown.
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public string SelectedValue
      {
         get
         {
            if (_selectedItem == null)
            {
               return null;
            }
            else
            {
               return _selectedItem.Value;
            }
         }
         set
         {
             if (value == null)
             {
                 _selectedItem = null;
                 TextBoxText = string.Empty;
             }
             else
            {
                foreach (RibbonItem item in DropDownItems)
                {
                    if (item.Value == value)
                    {
                        if (_selectedItem != item)
                        {
                            _selectedItem = item;
                            TextBoxText = _selectedItem.Text;
                            RibbonItemEventArgs arg = new RibbonItemEventArgs(item);
                            OnDropDownItemClicked(ref arg);
                        }
                    }
                }
            }
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Raises the <see cref="DropDownShowing"/> event;
      /// </summary>
      /// <param name="e"></param>
      public void OnDropDownShowing(EventArgs e)
      {
         if (DropDownShowing != null)
         {
            DropDownShowing(this, e);
         }
      }

      /// <summary>
      /// Gets or sets if the icons bar should be drawn
      /// </summary>
      [DefaultValue(true)]
      public bool DrawIconsBar
      {
         get { return _iconsBar; }
         set { _iconsBar = value; }
      }

      /// <summary>
      /// Shows the DropDown
      /// </summary>
      public virtual void ShowDropDown()
      {
         if (!_DropDownVisible)
         {
				AssignHandlers();
				
				OnDropDownShowing(EventArgs.Empty);

            RibbonDropDown dd = new RibbonDropDown(this, DropDownItems, Owner);
            dd.DropDownMaxHeight = _dropDownMaxHeight;
            dd.ShowSizingGrip = DropDownResizable;
            dd.DrawIconsBar = _iconsBar;
            dd.Closed += new EventHandler(DropDown_Closed);
				Point location = OnGetDropDownMenuLocation();
            dd.Show(location);
            _DropDownVisible = true;
         }
      }

      private void DropDown_Closed(object sender, EventArgs e)
      {
         _DropDownVisible = false;

         //Steve - when popup closed, un-m_highlight the dropdown arrow and redraw
         _dropDownPressed = false;
         //Kevin - Unselect it as well
         _dropDownSelected = false;
         RedrawItem();
      }

      private void AssignHandlers()
      {
         foreach (RibbonItem item in DropDownItems)
         {
            if (_assignedHandlers.Contains(item) == false)
            {
               item.Click += new EventHandler(DropDownItem_Click);
               _assignedHandlers.Add(item);
            }
         }
      }

      private void RemoveHandlers()
      {
         foreach (RibbonItem item in _assignedHandlers)
         {
            item.Click -= DropDownItem_Click;
         }
         _assignedHandlers.Clear();
      }

      void DropDownItem_Click(object sender, EventArgs e)
      {
         // Steve
         _selectedItem = (sender as RibbonItem);

         TextBoxText = (sender as RibbonItem).Text;
         //Kevin Carbis
         RibbonItemEventArgs ev = new RibbonItemEventArgs(sender as RibbonItem);
         OnDropDownItemClicked(ref ev);
      }


      #endregion

      #region Overrides

      protected override bool ClosesDropDownAt(Point p)
      {
         return false;
      }

      protected override void InitTextBox(TextBox t)
      {
         base.InitTextBox(t);

         t.Width -= DropDownButtonBounds.Width;
      }

      public override void SetBounds(Rectangle bounds)
      {
         base.SetBounds(bounds);

         _dropDownBounds = Rectangle.FromLTRB(
             bounds.Right - 15,
             bounds.Top,
             bounds.Right + 1,
             bounds.Bottom + 1);
      }

      public virtual void OnDropDownItemClicked(ref RibbonItemEventArgs e)
      {
         if (DropDownItemClicked != null)
         {
            DropDownItemClicked(this, e);
         }
      }

      public override void OnMouseMove(MouseEventArgs e)
      {
         if (!Enabled) return;

         base.OnMouseMove(e);

         bool mustRedraw = false;

         if (DropDownButtonBounds.Contains(e.X, e.Y))
         {
            Owner.Cursor = Cursors.Default;

            mustRedraw = !_dropDownSelected;

            _dropDownSelected = true;
         }
         else if (TextBoxBounds.Contains(e.X, e.Y))
         {
            Owner.Cursor = AllowTextEdit ? Cursors.IBeam : Cursors.Default;

            mustRedraw = _dropDownSelected;

            _dropDownSelected = false;
         }
         else
         {
            Owner.Cursor = Cursors.Default;
         }

         if (mustRedraw)
         {
            RedrawItem();
         }
      }

      public override void OnMouseDown(MouseEventArgs e)
      {
         if (!Enabled) return;
         // Steve - if allowtextedit is false, allow the textbox to bring up the popup
         if (DropDownButtonBounds.Contains(e.X, e.Y) || (TextBoxBounds.Contains(e.X, e.Y) != AllowTextEdit))
         {
            _dropDownPressed = true;

            ShowDropDown();
         }
         else if (TextBoxBounds.Contains(e.X, e.Y) && AllowTextEdit)
         {
            StartEdit();
         }
      }

      public override void OnMouseUp(MouseEventArgs e)
      {
         if (!Enabled) return;

         base.OnMouseUp(e);

         //Steve - pressed if set false when popup is closed
         //_dropDownPressed = false;
      }

      public override void OnMouseLeave(MouseEventArgs e)
      {
         if (!Enabled) return;

         base.OnMouseLeave(e);

         _dropDownSelected = false;

      }

		/// <summary>
		/// Gets the location where the dropdown menu will be shown
		/// </summary>
		/// <returns></returns>
		internal virtual Point OnGetDropDownMenuLocation()
		{
			Point location = Point.Empty;

			if (Canvas is RibbonDropDown)
			{
				location = Canvas.PointToScreen(new Point(TextBoxBounds.Left, Bounds.Bottom));
			}
			else
			{
				location = Owner.PointToScreen(new Point(TextBoxBounds.Left, Bounds.Bottom));
			}

			return location;
		}

      #endregion

      #region IContainsRibbonComponents Members

      public IEnumerable<Component> GetAllChildComponents()
      {
         return DropDownItems.ToArray();
      }

      #endregion

      #region IDropDownRibbonItem Members

      /// <summary>
      /// Gets or sets the bounds of the DropDown button
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public Rectangle DropDownButtonBounds
      {
         get { return _dropDownBounds; }
      }

      /// <summary>
      /// Gets a value indicating if the DropDown is currently visible
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool DropDownButtonVisible
      {
         get { return _dropDownVisible; }
      }

      /// <summary>
      /// Gets a value indicating if the DropDown button is currently selected
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool DropDownButtonSelected
      {
         get { return _dropDownSelected; }
      }

      /// <summary>
      /// Gets a value indicating if the DropDown button is currently pressed
      /// </summary>
      [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public bool DropDownButtonPressed
      {
         get { return _dropDownPressed; }
      }

      internal override void SetOwner(Ribbon owner)
      {
         base.SetOwner(owner);

         _dropDownItems.SetOwner(owner);
      }

      internal override void SetOwnerPanel(RibbonPanel ownerPanel)
      {
         base.SetOwnerPanel(ownerPanel);

         _dropDownItems.SetOwnerPanel(ownerPanel);
      }

      internal override void SetOwnerTab(RibbonTab ownerTab)
      {
         base.SetOwnerTab(ownerTab);

         _dropDownItems.SetOwnerTab(OwnerTab);
      }

      #endregion
   }
}
