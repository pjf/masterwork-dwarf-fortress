using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PresentationControls
{
    /// <summary>
    /// Martin Lottering : 2007-10-27
    /// --------------------------------
    /// This is a usefull control in Filters. Allows you to save space and can replace a Grouped Box of CheckBoxes.
    /// Currently used on the TasksFilter for TaskStatusses, which means the user can select which Statusses to include
    /// in the "Search".
    /// This control does not implement a CheckBoxListBox, instead it adds a wrapper for the normal ComboBox and Items. 
    /// See the CheckBoxItems property.
    /// ----------------
    /// ALSO IMPORTANT: In Data Binding when setting the DataSource. The ValueMember must be a bool type property, because it will 
    /// be binded to the Checked property of the displayed CheckBox. Also see the DisplayMemberSingleItem for more information.
    /// ----------------
    /// Extends the CodeProject PopupComboBox "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp"
    /// by Lukasz Swiatkowski.
    /// </summary>
    public partial class CheckBoxComboBox : PopupComboBox
    {
        #region CONSTRUCTOR
        /// <summary>
        /// TODO: Documentation Constructor
        /// </summary>
        public CheckBoxComboBox()
            : base()
        {
            InitializeComponent();

            _checkBoxProperties = new CheckBoxProperties();
            _checkBoxProperties.PropertyChanged += new EventHandler(_CheckBoxProperties_PropertyChanged);

            // Dumps the ListControl in a(nother) Container to ensure the ScrollBar on the ListControl does not
            // Paint over the Size grip. Setting the Padding or Margin on the Popup or host control does
            // not work as I expected. I don't think it can work that way.
            CheckBoxComboBoxListControlContainer ContainerControl = new CheckBoxComboBoxListControlContainer();
            _checkBoxComboBoxListControl = new CheckBoxComboBoxListControl(this);
            _checkBoxComboBoxListControl.Items.CheckBoxCheckedChanged += new EventHandler(Items_CheckBoxCheckedChanged);
            ContainerControl.Controls.Add(_checkBoxComboBoxListControl);
            // This padding spaces neatly on the left-hand side and allows space for the size grip at the bottom.
            ContainerControl.Padding = new Padding(4, 0, 0, 14);
            // The ListControl FILLS the ListContainer.
            _checkBoxComboBoxListControl.Dock = DockStyle.Fill;
            // The DropDownControl used by the base class. Will be wrapped in a popup by the base class.
            DropDownControl = ContainerControl;
            // Must be set after the DropDownControl is set, since the popup is recreated.
            // NOTE: I made the dropDown protected so that it can be accessible here. It was private.
            //dropdown.Resizable = true;
        }
        #endregion

        #region MEMBERS
        /// <summary>
        /// The checkbox list control. The public CheckBoxItems property provides a direct reference to its Items.
        /// </summary>
        internal CheckBoxComboBoxListControl _checkBoxComboBoxListControl;
        /// <summary>
        /// In DataBinding operations, this property will be used as the DisplayMember in the CheckBoxComboBoxListBox.
        /// The normal/existing "DisplayMember" property is used by the TextBox of the ComboBox to display 
        /// a concatenated Text of the items selected. This concatenation and its formatting however is controlled 
        /// by the Binded object, since it owns that property.
        /// </summary>
        private String _displayMemberSingleItem = null;
        /// <summary>
        /// TODO: Documentation Member
        /// </summary>
        private String _textSeparator = ", ";
        /// <summary>
        /// TODO: Documentation Member
        /// </summary>
        internal Boolean _mustAddHiddenItem = false;
        #endregion

        #region PRIVATE OPERATIONS
        /// <summary>
        /// Builds a CSV string of the items selected.
        /// </summary>
        internal string GetCSVText(bool skipFirstItem)
        {
            String listText = String.Empty;

            Int32 startIndex = this.DropDownStyle == ComboBoxStyle.DropDownList
                                    && this.DataSource == null
                                    && skipFirstItem ? 1 : 0;

            for (Int32 index = startIndex; index <= _checkBoxComboBoxListControl.Items.Count - 1; index++)
            {
                CheckBoxComboBoxItem item = _checkBoxComboBoxListControl.Items[index];

                if (item.Checked)
                    listText += String.IsNullOrEmpty(listText) ? item.Text : String.Format("{0}{1}", this.TextSeparator, item.Text);
            }
            return listText;
        }

        /// <summary>
        /// TODO: Documentation WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // 323 : Item Added
            // 331 : Clearing
            if (this.DropDownStyle == ComboBoxStyle.DropDownList &&
                this.DataSource == null &&
                m.Msg == 331)
            {
                _mustAddHiddenItem = true;
            }

            base.WndProc(ref m);
        }
        #endregion

        #region PROPERTIES
        [Browsable(true)]
        public Color ComboBoxListBackColor
        {
            get
            {
                return _checkBoxComboBoxListControl.BackColor;
            }
            set
            {
                _checkBoxComboBoxListControl.BackColor = value;
                DropDownControl.BackColor = value;
            }
        }


        /// <summary>
        /// A direct reference to the Items of CheckBoxComboBoxListControl.
        /// You can use it to Get or Set the Checked status of items manually if you want.
        /// But do not manipulate the List itself directly, e.g. Adding and Removing, 
        /// since the list is synchronised when shown with the ComboBox.Items. So for changing 
        /// the list contents, use Items instead.
        /// </summary>
        [Browsable(false)]
        public CheckBoxComboBoxItemList CheckBoxItems
        {
            get 
            { 
                // Added to ensure the CheckBoxItems are ALWAYS available for modification via code.
                if (_checkBoxComboBoxListControl.Items.Count != Items.Count)
                    _checkBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();

                return _checkBoxComboBoxListControl.Items; 
            }
        }
        /// <summary>
        /// The DataSource of the combobox. Refreshes the CheckBox wrappers when this is set.
        /// </summary>
        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;

                // This ensures that at least the checkboxitems are available to be initialised.
                if (!String.IsNullOrEmpty(this.ValueMember))
                    _checkBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }
        /// <summary>
        /// The ValueMember of the combobox. Refreshes the CheckBox wrappers when this is set.
        /// </summary>
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set
            {
                base.ValueMember = value;

                // This ensures that at least the checkboxitems are available to be initialised.
                if (!String.IsNullOrEmpty(this.ValueMember))
                    _checkBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }
        /// <summary>
        /// In DataBinding operations, this property will be used as the DisplayMember in the CheckBoxComboBoxListBox.
        /// The normal/existing "DisplayMember" property is used by the TextBox of the ComboBox to display 
        /// a concatenated Text of the items selected. This concatenation however is controlled by the Binded 
        /// object, since it owns that property.
        /// </summary>
        public string DisplayMemberSingleItem
        {
            get
            {
                if (String.IsNullOrEmpty(_displayMemberSingleItem))
                    return this.DisplayMember;
                else
                    return _displayMemberSingleItem;
            }
            set { _displayMemberSingleItem = value; }
        }
        /// <summary>
        /// TODO: Documentation Property
        /// </summary>
        [Browsable(true)]
        [DefaultValue(", ")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TextSeparator
        {
            get { return _textSeparator; }
            set { _textSeparator = value; }
        }
        /// <summary>
        /// Made this property Browsable again, since the Base Popup hides it. This class uses it again.
        /// Gets an object representing the collection of the items contained in this 
        /// System.Windows.Forms.ComboBox.
        /// </summary>
        /// <returns>A System.Windows.Forms.ComboBox.ObjectCollection representing the items in 
        /// the System.Windows.Forms.ComboBox.
        /// </returns>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }
        #endregion

        #region EVENTS & EVENT HANDLERS
        /// <summary>
        /// TODO: Documentation Event
        /// </summary>
        public event EventHandler CheckBoxCheckedChanged;

        /// <summary>
        /// TODO: Documentation Items_CheckBoxCheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Items_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this.OnCheckBoxCheckedChanged(sender, e);
        }

        /// <summary>
        /// TODO: Documentation OnCheckBoxCheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            String listText = this.GetCSVText(true);

            // The DropDownList style seems to require that the text
            // part of the "textbox" should match a single item.
            if (this.DropDownStyle != ComboBoxStyle.DropDownList)
                this.Text = listText;

            // This refreshes the Text of the first item (which is not visible)
            else if (this.DataSource == null)
            {
                this.Items[0] = listText;

                // Keep the hidden item and first checkbox item in 
                // sync in order to ensure the Synchronise process can match the items.
                this.CheckBoxItems[0].ComboBoxItem = listText;
            }

            if (this.CheckBoxCheckedChanged != null)
            {
                this.CheckBoxCheckedChanged(sender, e);
                    
            }
        }

        /// <summary>
        /// Will add an invisible item when the style is DropDownList,
        /// to help maintain the correct text in main TextBox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDownStyleChanged(EventArgs e)
        {
            base.OnDropDownStyleChanged(e);

            if (this.DropDownStyle == ComboBoxStyle.DropDownList &&
                this.DataSource == null && !this.DesignMode)
            {
                _mustAddHiddenItem = true;
            }
        }

        /// <summary>
        /// When the ComboBox is resized, the width of the dropdown 
        /// is also resized to match the width of the ComboBox. I think it looks better.
        /// </summary>
        /// <param name="e"></param>
        //protected override void OnResize(EventArgs e)
        //{
        //    dropDown.Size = new Size(this.Width, this.DropDownControl.Height);

        //    base.OnResize(e);
        //}
        #endregion

        #region PUBLIC OPERATIONS
        /// <summary>
        /// A function to clear/reset the list.
        /// (Ubiklou : http://www.codeproject.com/KB/combobox/extending_combobox.aspx?msg=2526813#xx2526813xx)
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();

            if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.DataSource == null)
                _mustAddHiddenItem = true;                
        }
        /// <summary>
        /// Uncheck all items.
        /// </summary>
        public void ClearSelection()
        {
            foreach (CheckBoxComboBoxItem item in CheckBoxItems)
            {
                if (item.Checked)
                    item.Checked = false;
            }
        }
        #endregion

        #region CHECKBOX PROPERTIES (DEFAULTS)
        /// <summary>
        /// TODO: Documentation Member
        /// </summary>
        private CheckBoxProperties _checkBoxProperties;
        /// <summary>
        /// The properties that will be assigned to the checkboxes as default values.
        /// </summary>
        [Description("The properties that will be assigned to the checkboxes as default values.")]
        [Browsable(true)]
        public CheckBoxProperties CheckBoxProperties
        {
            get { return _checkBoxProperties; }
            set
            {
                _checkBoxProperties = value;
                _CheckBoxProperties_PropertyChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// TODO: Documentation _CheckBoxProperties_PropertyChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CheckBoxProperties_PropertyChanged(object sender, EventArgs e)
        {
            foreach (CheckBoxComboBoxItem item in this.CheckBoxItems)
                item.ApplyProperties(this.CheckBoxProperties);
        }
        #endregion
    }

    /// <summary>
    /// A container control for the ListControl to ensure the ScrollBar on the ListControl does not
    /// Paint over the Size grip. Setting the Padding or Margin on the Popup or host control does
    /// not work as I expected.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControlContainer : UserControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControlContainer()
            : base()
        {
            this.BackColor = SystemColors.Window;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScaleMode = AutoScaleMode.Inherit;
            this.ResizeRedraw = true;

            // If you don't set this, then resize operations cause an error in the base class.
            this.MinimumSize = new Size(1, 1);
            this.MaximumSize = new Size(500, 500);
        }
        #endregion

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        /// <summary>
        /// Prescribed by the Popup class to ensure Resize operations work correctly.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if ((this.Parent as Popup).ProcessResizing(ref m))
                return;

            base.WndProc(ref m);
        }
        #endregion
    }

    /// <summary>
    /// This ListControl that pops up to the User. It contains the CheckBoxComboBoxItems. 
    /// The items are docked DockStyle.Top in this control.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControl : ScrollableControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControl(CheckBoxComboBox owner)
            : base()
        {
            this.DoubleBuffered = true;

            _checkBoxComboBox = owner;
            _items = new CheckBoxComboBoxItemList(_checkBoxComboBox);
            BackColor = SystemColors.Window;
            // AutoScaleMode = AutoScaleMode.Inherit;
            AutoScroll = true;
            ResizeRedraw = true;
            // if you don't set this, a Resize operation causes an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }

        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// Simply a reference to the CheckBoxComboBox.
        /// </summary>
        private CheckBoxComboBox _checkBoxComboBox;
        /// <summary>
        /// A Typed list of ComboBoxCheckBoxItems.
        /// </summary>
        private CheckBoxComboBoxItemList _items;

        #endregion

        #region PUBLIC PROPERTIES"
        [DefaultValue(typeof(Color), "")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        #endregion


        public CheckBoxComboBoxItemList Items { get { return _items; } }

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        /// <summary>
        /// Prescribed by the Popup control to enable Resize operations.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if ((Parent.Parent as Popup).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnVisibleChanged(EventArgs e)
        {
            // Synchronises the CheckBox list with the items in the ComboBox.
            SynchroniseControlsWithComboBoxItems();
            base.OnVisibleChanged(e);
        }
        /// <summary>
        /// Maintains the controls displayed in the list by keeping them in sync with the actual 
        /// items in the combobox. (e.g. removing and adding as well as ordering)
        /// </summary>
        public void SynchroniseControlsWithComboBoxItems()
        {
            SuspendLayout();

            if (_checkBoxComboBox._mustAddHiddenItem && _checkBoxComboBox.DataSource == null)
            {
                _checkBoxComboBox.Items.Insert(0, _checkBoxComboBox.GetCSVText(false)); // INVISIBLE ITEM
                _checkBoxComboBox.SelectedIndex = 0;
                _checkBoxComboBox._mustAddHiddenItem = false;
            }
            Controls.Clear();

            #region Disposes all items that are no longer in the combo box list

            for (int Index = _items.Count - 1; Index >= 0; Index--)
            {
                CheckBoxComboBoxItem Item = _items[Index];
                if (!_checkBoxComboBox.Items.Contains(Item.ComboBoxItem))
                {
                    _items.Remove(Item);
                    Item.Dispose();
                }
            }
            #endregion

            #region Recreate the list in the same order of the combo box items

            bool HasHiddenItem = _checkBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList
                                    && _checkBoxComboBox.DataSource == null
                                    && !this.DesignMode;

            CheckBoxComboBoxItemList NewList = new CheckBoxComboBoxItemList(_checkBoxComboBox);

            for(int Index0 = 0; Index0 <= _checkBoxComboBox.Items.Count - 1; Index0 ++)
            {
                Object obj = _checkBoxComboBox.Items[Index0];
                CheckBoxComboBoxItem item = null;

                // The hidden item could match any other item when only
                // one other item was selected.
                if (Index0 == 0 && HasHiddenItem && _items.Count > 0)
                    item = _items[0];
                else
                {
                    int StartIndex = HasHiddenItem
                        ? 1 // Skip the hidden item, it could match 
                        : 0;
                    for (int Index1 = StartIndex; Index1 <= _items.Count - 1; Index1++)
                    {
                        if (_items[Index1].ComboBoxItem == obj)
                        {
                            item = _items[Index1];
                            break;
                        }
                    }
                }
                if (item == null)
                {
                    item = new CheckBoxComboBoxItem(_checkBoxComboBox, obj);
                    item.ApplyProperties(_checkBoxComboBox.CheckBoxProperties);
                }
                NewList.Add(item);
                item.Dock = DockStyle.Top;
            }
            _items.Clear();
            _items.AddRange(NewList);

            #endregion
            #region Add the items to the controls in reversed order to maintain correct docking order

            if (NewList.Count > 0)
            {
                // This reverse helps to maintain correct docking order.
                NewList.Reverse();
                // If you get an error here that "Cannot convert to the desired 
                // type, it probably means the controls are not binding correctly.
                // The Checked property is binded to the ValueMember property. 
                // It must be a bool for example.
                Controls.AddRange(NewList.ToArray());
            }

            #endregion

            // Keep the first item invisible
            if (_checkBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList
                && _checkBoxComboBox.DataSource == null
                && !DesignMode)
                _checkBoxComboBox.CheckBoxItems[0].Visible = false; 
            
            ResumeLayout();
        }

        #endregion
    }

    /// <summary>
    /// The CheckBox items displayed in the Popup of the ComboBox.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItem : CheckBox
    {
        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">A reference to the CheckBoxComboBox.</param>
        /// <param name="comboBoxItem">A reference to the item in the ComboBox.Items that this object is extending.</param>
        public CheckBoxComboBoxItem(CheckBoxComboBox owner, object comboBoxItem)
            : base()
        {
            DoubleBuffered = true;
            _CheckBoxComboBox = owner;
            _ComboBoxItem = comboBoxItem;
            if (_CheckBoxComboBox.DataSource != null)
                AddBindings();
            else
                Text = comboBoxItem.ToString();
        }
        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// A reference to the CheckBoxComboBox.
        /// </summary>
        private CheckBoxComboBox _CheckBoxComboBox;
        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        private object _ComboBoxItem;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        public object ComboBoxItem
        {
            get { return _ComboBoxItem; }
            internal set { _ComboBoxItem = value; }
        }

        #endregion

        #region BINDING HELPER

        /// <summary>
        /// When using Data Binding operations via the DataSource property of the ComboBox. This
        /// adds the required Bindings for the CheckBoxes.
        /// </summary>
        public void AddBindings()
        {
            // Note, the text uses "DisplayMemberSingleItem", not "DisplayMember" (unless its not assigned)
            DataBindings.Add(
                "Text",
                _ComboBoxItem,
                _CheckBoxComboBox.DisplayMemberSingleItem
                );
            // The ValueMember must be a bool type property usable by the CheckBox.Checked.
            DataBindings.Add(
                "Checked",
                _ComboBoxItem,
                _CheckBoxComboBox.ValueMember,
                false,
                // This helps to maintain proper selection state in the Binded object,
                // even when the controls are added and removed.
                DataSourceUpdateMode.OnPropertyChanged,
                false, null, null);
            // Helps to maintain the Checked status of this
            // checkbox before the control is visible
            if (_ComboBoxItem is INotifyPropertyChanged)
                ((INotifyPropertyChanged)_ComboBoxItem).PropertyChanged += 
                    new PropertyChangedEventHandler(
                        CheckBoxComboBoxItem_PropertyChanged);
        }

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnCheckedChanged(EventArgs e)
        {
            // Found that when this event is raised, the bool value of the binded item is not yet updated.
            if (_CheckBoxComboBox.DataSource != null)
            {
                PropertyInfo PI = ComboBoxItem.GetType().GetProperty(_CheckBoxComboBox.ValueMember);
                PI.SetValue(ComboBoxItem, Checked, null);
            }
            base.OnCheckedChanged(e);
            // Forces a refresh of the Text displayed in the main TextBox of the ComboBox,
            // since that Text will most probably represent a concatenation of selected values.
            // Also see DisplayMemberSingleItem on the CheckBoxComboBox for more information.
            if (_CheckBoxComboBox.DataSource != null)
            {
                string OldDisplayMember = _CheckBoxComboBox.DisplayMember;
                _CheckBoxComboBox.DisplayMember = null;
                _CheckBoxComboBox.DisplayMember = OldDisplayMember;
            }
        }

        #endregion

        #region HELPER MEMBERS

        internal void ApplyProperties(CheckBoxProperties properties)
        {
            this.Appearance = properties.Appearance;
            this.AutoCheck = properties.AutoCheck;
            this.AutoEllipsis = properties.AutoEllipsis;
            this.AutoSize = properties.AutoSize;
            this.CheckAlign = properties.CheckAlign;
            this.FlatAppearance.BorderColor = properties.FlatAppearanceBorderColor;
            this.FlatAppearance.BorderSize = properties.FlatAppearanceBorderSize;
            this.FlatAppearance.CheckedBackColor = properties.FlatAppearanceCheckedBackColor;
            this.FlatAppearance.MouseDownBackColor = properties.FlatAppearanceMouseDownBackColor;
            this.FlatAppearance.MouseOverBackColor = properties.FlatAppearanceMouseOverBackColor;
            this.FlatStyle = properties.FlatStyle;
            this.ForeColor = properties.ForeColor;
            this.RightToLeft = properties.RightToLeft;
            this.TextAlign = properties.TextAlign;
            this.ThreeState = properties.ThreeState;
            this.BackColor = properties.BackColor;            
        }

        #endregion

        #region EVENT HANDLERS - ComboBoxItem (DataSource)

        /// <summary>
        /// Added this handler because the control doesn't seem 
        /// to initialize correctly until shown for the first
        /// time, which also means the summary text value
        /// of the combo is out of sync initially.
        /// </summary>
        private void CheckBoxComboBoxItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _CheckBoxComboBox.ValueMember)
                Checked = 
                    (bool)_ComboBoxItem
                        .GetType()
                        .GetProperty(_CheckBoxComboBox.ValueMember)
                        .GetValue(_ComboBoxItem, null);
        }

        #endregion
    }

    /// <summary>
    /// A Typed List of the CheckBox items.
    /// Simply a wrapper for the CheckBoxComboBox.Items. A list of CheckBoxComboBoxItem objects.
    /// This List is automatically synchronised with the Items of the ComboBox and extended to
    /// handle the additional boolean value. That said, do not Add or Remove using this List, 
    /// it will be lost or regenerated from the ComboBox.Items.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItemList : List<CheckBoxComboBoxItem>
    {
        #region CONSTRUCTORS

        public CheckBoxComboBoxItemList(CheckBoxComboBox checkBoxComboBox)
        {
            _CheckBoxComboBox = checkBoxComboBox;
        }

        #endregion

        #region PRIVATE FIELDS

        private CheckBoxComboBox _CheckBoxComboBox;

        #endregion

        #region EVENTS, This could be moved to the list control if needed

        public event EventHandler CheckBoxCheckedChanged;

        protected void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
                handler(sender, e);
        }
        private void item_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxCheckedChanged(sender, e);
        }

        #endregion

        #region LIST MEMBERS & OBSOLETE INDICATORS

        [Obsolete("Do not add items to this list directly. Use the ComboBox items instead.", false)]
        public new void Add(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<CheckBoxComboBoxItem> collection)
        {
            foreach (CheckBoxComboBoxItem Item in collection)
                Item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.AddRange(collection);
        }

        public new void Clear()
        {
            foreach (CheckBoxComboBoxItem Item in this)
                Item.CheckedChanged -= item_CheckedChanged;
            base.Clear();
        }

        [Obsolete("Do not remove items from this list directly. Use the ComboBox items instead.", false)]
        public new bool Remove(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged -= item_CheckedChanged;
            return base.Remove(item);
        }

        #endregion

        #region DEFAULT PROPERTIES

        /// <summary>
        /// Returns the item with the specified displayName or Text.
        /// </summary>
        public CheckBoxComboBoxItem this[string displayName]
        {
            get
            {
                int StartIndex =
                    // An invisible item exists in this scenario to help 
                    // with the Text displayed in the TextBox of the Combo
                    _CheckBoxComboBox.DropDownStyle == ComboBoxStyle.DropDownList 
                    && _CheckBoxComboBox.DataSource == null
                        ? 1 // Ubiklou : 2008-04-28 : Ignore first item. (http://www.codeproject.com/KB/combobox/extending_combobox.aspx?fid=476622&df=90&mpp=25&noise=3&sort=Position&view=Quick&select=2526813&fr=1#xx2526813xx)
                        : 0;
                for(int Index = StartIndex; Index <= Count - 1; Index ++)
                {
                    CheckBoxComboBoxItem Item = this[Index];

                    string Text;
                    // The binding might not be active yet
                    if (string.IsNullOrEmpty(Item.Text)
                        // Ubiklou : 2008-04-28 : No databinding
                        && Item.DataBindings != null 
                        && Item.DataBindings["Text"] != null
                        )
                    {
                        PropertyInfo PropertyInfo
                            = Item.ComboBoxItem.GetType().GetProperty(
                                Item.DataBindings["Text"].BindingMemberInfo.BindingMember);
                        Text = (string)PropertyInfo.GetValue(Item.ComboBoxItem, null);
                    }
                    else
                        Text = Item.Text;
                    if (Text.CompareTo(displayName) == 0)
                        return Item;
                }
                throw new ArgumentOutOfRangeException(String.Format("\"{0}\" does not exist in this combo box.", displayName));
            }
        }
        
        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckBoxProperties
    {
        public CheckBoxProperties() { }

        #region PRIVATE PROPERTIES

        private Appearance _Appearance = Appearance.Normal;
        private bool _AutoSize = false;
        private bool _AutoCheck = true;
        private bool _AutoEllipsis = false;
        private ContentAlignment _CheckAlign = ContentAlignment.MiddleLeft;
        private Color _FlatAppearanceBorderColor = Color.Empty;
        private int _FlatAppearanceBorderSize = 1;
        private Color _FlatAppearanceCheckedBackColor = Color.Empty;
        private Color _FlatAppearanceMouseDownBackColor = Color.Empty;
        private Color _FlatAppearanceMouseOverBackColor = Color.Empty;
        private Color _BackColor = Color.Empty;
        private FlatStyle _FlatStyle = FlatStyle.Standard;
        private Color _ForeColor = SystemColors.ControlText;
        private RightToLeft _RightToLeft = RightToLeft.No;
        private ContentAlignment _TextAlign = ContentAlignment.MiddleLeft;
        private bool _ThreeState = false;

        #endregion

        #region PUBLIC PROPERTIES

        [DefaultValue(Appearance.Normal)]
        public Appearance Appearance
        {
            get { return _Appearance; }
            set { _Appearance = value; OnPropertyChanged(); }
        }
        [DefaultValue(true)]
        public bool AutoCheck
        {
            get { return _AutoCheck; }
            set { _AutoCheck = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoEllipsis
        {
            get { return _AutoEllipsis; }
            set { _AutoEllipsis = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return _AutoSize; }
            set { _AutoSize = true; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment CheckAlign
        {
            get { return _CheckAlign; }
            set { _CheckAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceBorderColor
        {
            get { return _FlatAppearanceBorderColor; }
            set { _FlatAppearanceBorderColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(1)]
        public int FlatAppearanceBorderSize
        {
            get { return _FlatAppearanceBorderSize; }
            set { _FlatAppearanceBorderSize = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceCheckedBackColor
        {
            get { return _FlatAppearanceCheckedBackColor; }
            set { _FlatAppearanceCheckedBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseDownBackColor
        {
            get { return _FlatAppearanceMouseDownBackColor; }
            set { _FlatAppearanceMouseDownBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseOverBackColor
        {
            get { return _FlatAppearanceMouseOverBackColor; }
            set { _FlatAppearanceMouseOverBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle
        {
            get { return _FlatStyle; }
            set { _FlatStyle = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(RightToLeft.No)]
        public RightToLeft RightToLeft
        {
            get { return _RightToLeft; }
            set { _RightToLeft = value; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool ThreeState
        {
            get { return _ThreeState; }
            set { _ThreeState = value; OnPropertyChanged(); }
        }

        #endregion

        #region EVENTS AND EVENT CALLERS

        /// <summary>
        /// Called when any property changes.
        /// </summary>
        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged()
        {
            EventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
