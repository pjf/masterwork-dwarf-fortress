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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms.RibbonHelpers;

namespace System.Windows.Forms
{
    /// <summary>
    /// Provides a Ribbon toolbar
    /// </summary>
    [Designer(typeof(RibbonDesigner))]
    public class Ribbon
         : Control
    {
        delegate void HandlerCallbackMethode();

        #region Const

        private const int DefaultTabSpacing = 6;
        private const int DefaultPanelSpacing = 3;

        #endregion

        #region Static

        public static int CaptionBarHeight = 24;

        #endregion

        #region Fields
        internal bool ForceOrbMenu;
        private Size _lastSizeMeasured;
        private RibbonTabCollection _tabs;
        private Padding _tabsMargin;
        private Padding _tabsPadding;
        private Padding _orbsPadding;
        private RibbonContextCollection _contexts;
        internal bool _minimized = true;//is the ribbon minimized?
        internal bool _expanded; //is the ribbon currently expanded when in minimize mode
        internal bool _expanding; //is the ribbon expanding. Flag used to prevent calling methods multiple time while the size changes
        //private int _minimizedHeight;//height when minimized
        private int _expandedHeight; //height when expanded
        private RibbonRenderer _renderer;
        private int _tabSpacing;
        private Padding _tabContentMargin;
        private Padding _tabContentPadding;
        private Padding _panelPadding;
        private Padding _panelMargin;
        private RibbonTab _activeTab;
        private int _panelSpacing;
        private Padding _itemMargin;
        private Padding _itemPadding;
        private RibbonTab _lastSelectedTab;
        private RibbonMouseSensor _sensor;
        private Padding _dropDownMargin;
        private Padding _tabTextMargin;
        //private float _tabSum;
        private bool _updatingSuspended;
        private bool _orbSelected;
        private bool _orbPressed;
        private bool _orbVisible;
        private Image _orbImage;
        private string _orbText;
        private RibbonQuickAccessToolbar _quickAcessToolbar;
        //private bool _quickAcessVisible;
        private RibbonOrbDropDown _orbDropDown;
        private RibbonWindowMode _borderMode;
        private RibbonWindowMode _actualBorderMode;
        private RibbonCaptionButton _CloseButton;
        private RibbonCaptionButton _MaximizeRestoreButton;
        private RibbonCaptionButton _MinimizeButton;
        private bool _CaptionButtonsVisible;
        private GlobalHook _mouseHook;
        private GlobalHook _keyboardHook;
        private Font _RibbonItemFont = new Font("Trebuchet MS", 9);
        private Font _RibbonTabFont = new Font("Trebuchet MS", 9);

        internal RibbonItem ActiveTextBox; //tracks the current active textbox so we can hide it when you click off it
        #endregion

        #region Events

        /// <summary>
        /// Occours when the Orb is clicked
        /// </summary>
        public event EventHandler OrbClicked;

        /// <summary>
        /// Occours when the Orb is double-clicked
        /// </summary>
        public event EventHandler OrbDoubleClick;

        /// <summary>
        /// Occours when the <see cref="ActiveTab"/> property value has changed
        /// </summary>
        public event EventHandler ActiveTabChanged;

        /// <summary>
        /// Occours when the <see cref="ActualBorderMode"/> property has changed
        /// </summary>
        public event EventHandler ActualBorderModeChanged;

        /// <summary>
        /// Occours when the <see cref="CaptionButtonsVisible"/> property value has changed
        /// </summary>
        public event EventHandler CaptionButtonsVisibleChanged;

        ///// <summary>
        ///// Occours when the Ribbon changes its miminized state
        ///// </summary>
        public event EventHandler ExpandedChanged;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Ribbon control
        /// </summary>
        public Ribbon()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Dock = DockStyle.Top;

            _tabs = new RibbonTabCollection(this);
            _contexts = new RibbonContextCollection(this);

            _orbsPadding = new Padding(8, 5, 8, 3);
            _tabsPadding = new Padding(8, 5, 8, 3);
            _tabsMargin = new Padding(12, 24 + 2, 20, 0);
            _tabTextMargin = new Padding(4, 2, 4, 2);

            _tabContentMargin = new Padding(1, 0, 1, 2);
            _panelPadding = new Padding(3);
            _panelMargin = new Padding(3, 2, 3, 15);
            _panelSpacing = DefaultPanelSpacing;
            _itemPadding = new Padding(1, 0, 1, 0);
            _itemMargin = new Padding(4, 2, 4, 2);
            _tabSpacing = DefaultTabSpacing;
            _dropDownMargin = new Padding(2);
            _renderer = new RibbonProfessionalRenderer();
            _orbVisible = true;
            _orbDropDown = new RibbonOrbDropDown(this);
            _quickAcessToolbar = new RibbonQuickAccessToolbar(this);
            //_quickAcessVisible = true;
            _MinimizeButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Minimize);
            _MaximizeRestoreButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Maximize);
            _CloseButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Close);

            _MinimizeButton.SetOwner(this);
            _MaximizeRestoreButton.SetOwner(this);
            _CloseButton.SetOwner(this);
            _CaptionBarVisible = true;

            Font = SystemFonts.CaptionFont;

            BorderMode = RibbonWindowMode.NonClientAreaGlass;

            _minimized = false;
            _expanded = true;

            RibbonPopupManager.PopupRegistered += OnPopupRegistered;
            RibbonPopupManager.PopupUnRegistered += OnPopupUnregistered;
            //Theme.MainRibbon = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                foreach (RibbonTab tab in _tabs)
                    tab.Dispose();
                _orbDropDown.Dispose();
                _quickAcessToolbar.Dispose();
                _MinimizeButton.Dispose();
                _MaximizeRestoreButton.Dispose();
                _CloseButton.Dispose();

                RibbonPopupManager.PopupRegistered -= OnPopupRegistered;
                RibbonPopupManager.PopupUnRegistered -= OnPopupUnregistered;

                GC.SuppressFinalize(this);
            }

            DisposeHooks();

            base.Dispose(disposing);
        }

        ~Ribbon()
        {
            Dispose(false);
        }

        private void DisposeHooks()
        {
            if (_mouseHook != null)
            {
                _mouseHook.MouseWheel -= new MouseEventHandler(_mouseHook_MouseWheel);
                _mouseHook.MouseDown -= new MouseEventHandler(_mouseHook_MouseDown);
                _mouseHook.Dispose();
                _mouseHook = null;
            }
            if (_keyboardHook != null)
            {
                _keyboardHook.KeyDown -= new KeyEventHandler(_keyboardHook_KeyDown);
                _keyboardHook.Dispose();
                _keyboardHook = null;
            }
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the tabs expanded state when in minimize mode
        /// </summary>
        [DefaultValue(true)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                _expanded = value;
                if (!IsDesignMode() && Minimized)
                {
                    _expanding = true;
                    if (_expanded)
                        this.Height = _expandedHeight;
                    else
                        this.Height = MinimizedHeight;

                    OnExpandedChanged(EventArgs.Empty);
                    if (_expanded)
                        SetUpHooks();
                    else if (!_expanded && RibbonPopupManager.PopupCount == 0)
                        DisposeHooks();
                    //UpdateRegions();
                    Invalidate();
                    _expanding = false;
                }
            }
        }

        /// <summary>
        /// Gets the height of the ribbon when collapsed <see cref="MinimizedHeight"/>
        /// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets the height of the ribbon when collapsed")]
        public int MinimizedHeight
        {
            get
            {
                int tabBottom = Tabs.Count > 0 ? Tabs[0].Bounds.Bottom : 0;
                return Math.Max(OrbBounds.Bottom, tabBottom) + 1;
            }
        }

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                Height = value.Height;
                if (!Minimized || (!_expanding && Expanded))
                    _expandedHeight = this.Height;
            }
        }

        internal Rectangle CaptionTextBounds
        {
            get
            {
                if (RightToLeft == RightToLeft.No)
                {
                    int left = 0;
                    if (OrbVisible) left = OrbBounds.Right;
                    if (QuickAcessToolbar.Visible) left = QuickAcessToolbar.Bounds.Right + 20;
                    if (QuickAcessToolbar.Visible && QuickAcessToolbar.DropDownButtonVisible) left = QuickAcessToolbar.DropDownButton.Bounds.Right;
                    Rectangle r = Rectangle.FromLTRB(left, 0, Width - 100, CaptionBarSize);
                    return r;
                }
                else
                {
                    int right = ClientRectangle.Right;
                    if (OrbVisible) right = OrbBounds.Left;
                    if (QuickAcessToolbar.Visible) right = QuickAcessToolbar.Bounds.Left - 20;
                    if (QuickAcessToolbar.Visible && QuickAcessToolbar.DropDownButtonVisible) right = QuickAcessToolbar.DropDownButton.Bounds.Left;
                    Rectangle r = Rectangle.FromLTRB(100, 0, right, CaptionBarSize);
                    return r;
                }
            }
        }

        /// <summary>
        /// Gets if the caption buttons are currently visible, according to the value specified in <see cref="BorderMode"/>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CaptionButtonsVisible
        {
            get { return _CaptionButtonsVisible; }
        }

        /// <summary>
        /// Gets the Ribbon's close button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton CloseButton
        {
            get { return _CloseButton; }
        }

        /// <summary>
        /// Gets the Ribbon's maximize-restore button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MaximizeRestoreButton
        {
            get { return _MaximizeRestoreButton; }
        }

        /// <summary>
        /// Gets the Ribbon's minimize button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MinimizeButton
        {
            get { return _MinimizeButton; }
        }

        /// <summary>
        /// Gets or sets the RibbonFormHelper object if the parent form is IRibbonForm
        /// </summary>
        [Browsable(false)]
        public RibbonFormHelper FormHelper
        {
            get
            {
                IRibbonForm irf = Parent as IRibbonForm;

                if (irf != null)
                {
                    return irf.Helper;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="RibbonWindowMode"/> that the ribbon has. 
        /// It's value may vary from <see cref="BorderMode"/>
        /// because of computer and operative system capabilities.
        /// </summary>
        [Browsable(false)]
        public RibbonWindowMode ActualBorderMode
        {
            get { return _actualBorderMode; }
        }

        /// <summary>
        /// Gets or sets the border mode of the ribbon relative to the window where it is contained
        /// </summary>
        [DefaultValue(RibbonWindowMode.NonClientAreaGlass)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Specifies how the Ribbon is placed on the window border and the non-client area")]
        public RibbonWindowMode BorderMode
        {
            get { return _borderMode; }
            set
            {
                _borderMode = value;

                RibbonWindowMode actual = value;

                if (value == RibbonWindowMode.NonClientAreaGlass && !WinApi.IsGlassEnabled)
                {
                    actual = RibbonWindowMode.NonClientAreaCustomDrawn;
                }

                if (FormHelper == null || (value == RibbonWindowMode.NonClientAreaCustomDrawn && Environment.OSVersion.Platform != PlatformID.Win32NT))
                {
                    actual = RibbonWindowMode.InsideWindow;
                }

                SetActualBorderMode(actual);
            }
        }

        /// <summary>
        /// Gets the Orb's DropDown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public RibbonOrbDropDown OrbDropDown
        {
            get { return _orbDropDown; }
        }

        /// <summary>
        /// Gets or sets the height of the Panel Caption area.
        /// </summary>
        [DefaultValue(15)]
        [Description("Gets or sets the height of the Panel Caption area")]
        public int PanelCaptionHeight
        {
            get { return _panelMargin.Bottom; }
            set { _panelMargin.Bottom = value; Invalidate(); }
        }

        /// <summary>
        /// Gets  the QuickAcessToolbar
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbar QuickAcessToolbar
        {
            get { return _quickAcessToolbar; }
        }

        /// <summary>
        /// Gets or sets the Style of the orb
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(RibbonOrbStyle.Office_2007)]
        public RibbonOrbStyle OrbStyle
        {
            get { return Theme.ThemeStyle; }
            set
            {
                Theme.blnSetOnly = true;
                Theme.ThemeStyle = value;
                Theme.blnSetOnly = false;

                if (value == RibbonOrbStyle.Office_2007)
                {
                    _tabsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2010)
                {
                    _tabsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2013)
                {
                    _tabsPadding = new Padding(8, 3, 8, 3);
                }

                if (value == RibbonOrbStyle.Office_2007)
                {
                    _orbsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2010)
                {
                    _orbsPadding = new Padding(8, 4, 8, 4);
                }
                else if (value == RibbonOrbStyle.Office_2013)
                {
                    _orbsPadding = new Padding(15, 3, 15, 3);
                }

                UpdateRegions();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the theme of the ribbon control
        /// </summary>
        //Michael Spradlin 07/05/2013
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(RibbonTheme.Normal)]
        public RibbonTheme ThemeColor
        {
            get { return Theme.ThemeColor; }
            set
            {
                Theme.ThemeColor = value;
                OnRegionsChanged(); Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the Text in the orb. Only available when the OrbStyle is set to Office2010
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string OrbText
        {
            get { return _orbText; }
            set { _orbText = value; OnRegionsChanged(); Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the Image of the orb
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image OrbImage
        {
            get { return _orbImage; }
            set { _orbImage = value; OnRegionsChanged(); Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets or sets if the Ribbon should show an orb on the corner
        /// </summary>
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OrbVisible
        {
            get { return _orbVisible; }
            set { _orbVisible = value; OnRegionsChanged(); }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbSelected
        {
            get { return _orbSelected; }
            set { _orbSelected = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbPressed
        {
            get { return _orbPressed; }
            set { _orbPressed = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets the Height of the caption bar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionBarSize
        {
            get { return CaptionBarHeight; }
        }

        /// <summary>
        /// Gets the bounds of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle OrbBounds
        {
            get
            {
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    if (OrbVisible && RightToLeft == RightToLeft.No && CaptionBarVisible)
                    {
                        return new Rectangle(4, 4, 36, 36);
                    }
                    else if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(this.Width - 36 - 4, 4, 36, 36);
                    }
                    else if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    else
                    {
                        return new Rectangle(this.Width - 4, 4, 0, 0);
                    }
                }
                else if (OrbStyle == RibbonOrbStyle.Office_2010)//Kevin Carbis - office 2010 style orb
                {
                    //Measure the string size of the button text so we know how big to make the button
                    Size contentSize = TextRenderer.MeasureText(OrbText, RibbonTabFont);
                    //If we are using an image adjust the size
                    if (OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, OrbImage.Size.Height);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }
                    else if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(this.Width - contentSize.Width - OrbsPadding.Left - OrbsPadding.Right - 4, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }
                    else if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    else
                    {
                        return new Rectangle(this.Width - 4, 4, 0, 0);
                    }
                }
                else  //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                {
                    //Measure the string size of the button text so we know how big to make the button
                    Size contentSize = TextRenderer.MeasureText(OrbText, RibbonTabFont);
                    //If we are using an image adjust the size
                    if (OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, OrbImage.Size.Height);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(0, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }
                    else if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(this.Width - contentSize.Width - OrbsPadding.Left - OrbsPadding.Right - 4, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }
                    else if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }
                    else
                    {
                        return new Rectangle(this.Width - 4, 4, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab NextTab
        {
            get
            {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == Tabs.Count - 1)
                {
                    return ActiveTab;
                }
                else
                {

                    return Tabs[index + 1];
                }
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab PreviousTab
        {
            get
            {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == 0)
                {
                    return ActiveTab;
                }
                else
                {
                    return Tabs[index - 1];
                }
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing between the tab and its text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabTextMargin
        {
            get { return _tabTextMargin; }
            set { _tabTextMargin = value; }
        }

        /// <summary> 
        /// Gets or sets the margis of the DropDowns shown by the Ribbon
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding DropDownMargin
        {
            get { return _dropDownMargin; }
            set { _dropDownMargin = value; }
        }

        /// <summary>
        /// Gets or sets the external spacing of items on panels
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemPadding
        {
            get { return _itemPadding; }
            set { _itemPadding = value; }
        }

        /// <summary>
        /// Gets or sets the internal spacing of items
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemMargin
        {
            get { return _itemMargin; }
            set { _itemMargin = value; }
        }

        /// <summary>
        /// Gets or sets the tab that is currently active
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab ActiveTab
        {
            get { return _activeTab; }
            set
            {
                RibbonTab NewTab = _activeTab;
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab != value)
                    {
                        tab.SetActive(false);
                    }
                    else
                    {
                        NewTab = tab;
                    }
                }
                NewTab.SetActive(true);

                _activeTab = value;

                RemoveHelperControls();

                value.UpdatePanelsRegions();

                Invalidate();

                RenewSensor();

                OnActiveTabChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the spacing leaded between panels
        /// </summary>
        [DefaultValue(DefaultPanelSpacing)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PanelSpacing
        {
            get { return _panelSpacing; }
            set { _panelSpacing = value; }
        }

        /// <summary>
        /// Gets or sets the external spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelPadding
        {
            get { return _panelPadding; }
            set { _panelPadding = value; }
        }

        /// <summary>
        /// Gets or sets the internal spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelMargin
        {
            get { return _panelMargin; }
            set { _panelMargin = value; }
        }

        /// <summary>
        /// Gets or sets the spacing between tabs
        /// </summary>
        [DefaultValue(DefaultTabSpacing)]
        public int TabSpacing
        {
            get { return _tabSpacing; }
            set { _tabSpacing = value; }
        }

        /// <summary>
        /// Gets the collection of RibbonTab tabs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonTabCollection Tabs
        {
            get
            {
                return _tabs;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the Ribbon supports being minimized
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Minimized
        {
            get
            {
                return _minimized;
            }
            set
            {
                _minimized = value;
                if (!IsDesignMode())
                {
                    if (_minimized)
                    {
                        this.Height = MinimizedHeight;
                    }
                    else
                    {
                        this.Height = _expandedHeight;
                    }
                    Expanded = !Minimized;
                    UpdateRegions();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the collection of Contexts of this Ribbon
        /// </summary>
        public RibbonContextCollection Contexts
        {
            get
            {
                return _contexts;
            }
        }

        /// <summary>
        /// Gets or sets the Renderer for this Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonRenderer Renderer
        {
            get
            {
                return _renderer;
            }
            set
            {
                if (value == null) throw new ApplicationException("Null renderer!");
                _renderer = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing of the tab content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentMargin
        {
            get { return _tabContentMargin; }
            set { _tabContentMargin = value; }
        }

        /// <summary>
        /// Gets or sets the external spacing of the tabs content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentPadding
        {
            get { return _tabContentPadding; }
            set { _tabContentPadding = value; }
        }

        /// <summary>
        /// Gets a value indicating the external spacing of tabs
        /// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsMargin
        {
            get
            {
                return _tabsMargin;
            }
            set
            {
                _tabsMargin = value;
                UpdateRegions();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating the internal spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsPadding
        {
            get
            {
                return _tabsPadding;
            }
            set
            {
                _tabsPadding = value;
            }
        }

        /// <summary>
        /// Gets a value indicating the internal spacing of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding OrbsPadding
        {
            get
            {
                return _orbsPadding;
            }
            set
            {
                _orbsPadding = value;
            }
        }

        /// <summary>
        /// Overriden. The maximum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize
        {
            get
            {
                return new System.Drawing.Size(0, 200); //115 was the old one
            }
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The minimum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get
            {
                return new System.Drawing.Size(0, 27); //115);
            }
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The default dock of the ribbon is top
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(DockStyle.Top)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }

        /// <summary>
        /// Gets or sets the current panel sensor for this ribbon
        /// </summary>
        [Browsable(false)]
        public RibbonMouseSensor Sensor
        {
            get
            {
                return _sensor;
            }
        }

        [DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
                OnRegionsChanged();
            }
        }

        private bool _CaptionBarVisible;
        /// <summary>
        /// sets or gets the visibility of the caption bar
        /// </summary>
        [DefaultValue(true)]
        public bool CaptionBarVisible
        {
            get { return _CaptionBarVisible; }
            set
            {
                _CaptionBarVisible = value;

                if (CaptionBarVisible)
                {
                    Padding tm = TabsMargin;
                    tm.Top = CaptionBarHeight + 2;
                    TabsMargin = tm;
                }
                else
                {
                    Padding tm = TabsMargin;
                    tm.Top = 2;
                    TabsMargin = tm;
                }

                UpdateRegions();
                Invalidate();
            }
        }

        public override void Refresh()
        {
            try
            {
                if (this.IsDisposed == false)
                {
                    if (InvokeRequired)
                    {
                        HandlerCallbackMethode del = new HandlerCallbackMethode(Refresh);
                        this.Invoke(del);
                    }
                    else
                    {
                        base.Refresh();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region cr
        private string cr { get { return "Professional Ribbon\n\n2009 Jos?Manuel Menéndez Poo\nwww.menendezpoo.com"; } }
        #endregion

        ///// <summary>
        ///// Gets or sets the Font associated with Ribbon Items.
        ///// </summary>
        //[DefaultValue(null)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public Font RibbonItemFont
        //{
        //    get { return _RibbonItemFont; }
        //    set { _RibbonItemFont = value;}
        //}


        /// <summary>
        /// Gets or sets the Font associated with Ribbon tabs and the ORB.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font RibbonTabFont
        {
            get { return _RibbonTabFont; }
            set { _RibbonTabFont = value; }
        }

        #endregion

        #region Handler Methods

        /// <summary>
        /// Resends the mousedown to PopupManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            bool handled = false;
            if (!RectangleToScreen(OrbBounds).Contains(e.Location))
            {
                handled = RibbonPopupManager.FeedHookClick(e);
            }
            if (RectangleToScreen(this.Bounds).Contains(e.Location))
            {
                //they clicked inside the ribbon
                handled = true;
            }
            if (Minimized && !handled)
                Expanded = false;
        }

        /// <summary>
        /// Checks if MouseWheel should be raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!RibbonPopupManager.FeedMouseWheel(e))
            {
                if (RectangleToScreen(
                     new Rectangle(Point.Empty, Size)
                     ).Contains(e.Location))
                {
                    OnMouseWheel(e);
                }
            }
        }


        /// <summary>
        /// Raises the OrbClicked event
        /// </summary>
        /// <param name="e">event data</param>
        internal virtual void OnOrbClicked(EventArgs e)
        {
            if (OrbPressed)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
            }
            else
            {
                ShowOrbDropDown();
            }

            if (OrbClicked != null)
            {
                OrbClicked(this, e);
            }
        }

        /// <summary>
        /// Raises the OrbDoubleClicked
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnOrbDoubleClicked(EventArgs e)
        {
            if (OrbDoubleClick != null)
            {
                OrbDoubleClick(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes hooks
        /// </summary>
        private void SetUpHooks()
        {
            if (RibbonDesigner.Current == null)
            {
                if (_mouseHook == null)
                {
                    _mouseHook = new GlobalHook(GlobalHook.HookTypes.Mouse);
                    _mouseHook.MouseWheel += new MouseEventHandler(_mouseHook_MouseWheel);
                    _mouseHook.MouseDown += new MouseEventHandler(_mouseHook_MouseDown);
                }

                if (_keyboardHook == null)
                {
                    _keyboardHook = new GlobalHook(GlobalHook.HookTypes.Keyboard);
                    _keyboardHook.KeyDown += new KeyEventHandler(_keyboardHook_KeyDown);
                }
            }
        }

        private void _keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.EscapePressed);
            }
        }

        /// <summary>
        /// Shows the Orb's dropdown
        /// </summary>
        public void ShowOrbDropDown()
        {
            OrbPressed = true;
            if (RightToLeft == RightToLeft.No)
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.X - 4, OrbBounds.Bottom - OrbDropDown.ContentMargin.Top + 2)));
                else if (OrbStyle == RibbonOrbStyle.Office_2010 | OrbStyle == RibbonOrbStyle.Office_2013)//Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.X - 4, OrbBounds.Bottom)));
                else
                    if (OrbStyle == RibbonOrbStyle.Office_2007)
                        OrbDropDown.Show(PointToScreen(new Point(OrbBounds.Right + 4 - OrbDropDown.Width, OrbBounds.Bottom - OrbDropDown.ContentMargin.Top + 2)));
                    else if (OrbStyle == RibbonOrbStyle.Office_2010 | OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                        OrbDropDown.Show(PointToScreen(new Point(OrbBounds.Right + 4 - OrbDropDown.Width, OrbBounds.Bottom)));
        }

        /// <summary>
        /// Shows the Orb's dropdown at the specified point.
        /// </summary>
        public void ShowOrbDropDown(Point pt)
        {
            OrbPressed = true;
            OrbDropDown.Show(PointToScreen(pt));
        }

        /// <summary>
        /// Drops out the old sensor and creates a new one
        /// </summary>
        private void RenewSensor()
        {
            if (ActiveTab == null)
            {
                return;
            }

            if (Sensor != null) Sensor.Dispose();

            _sensor = new RibbonMouseSensor(this, this, ActiveTab);

            if (CaptionButtonsVisible)
            {
                Sensor.Items.AddRange(new RibbonItem[] { CloseButton, MaximizeRestoreButton, MinimizeButton });
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="BorderMode"/> property
        /// </summary>
        /// <param name="borderMode">Actual border mode accquired</param>
        private void SetActualBorderMode(RibbonWindowMode borderMode)
        {
            bool trigger = _actualBorderMode != borderMode;

            _actualBorderMode = borderMode;

            if (trigger)
                OnActualBorderModeChanged(EventArgs.Empty);

            SetCaptionButtonsVisible(borderMode == RibbonWindowMode.NonClientAreaCustomDrawn);


        }

        /// <summary>
        /// Sets the value of the <see cref="CaptionButtonsVisible"/> property
        /// </summary>
        /// <param name="visible">Value to set to the caption buttons</param>
        private void SetCaptionButtonsVisible(bool visible)
        {
            bool trigger = _CaptionButtonsVisible != visible;

            _CaptionButtonsVisible = visible;

            if (trigger)
                OnCaptionButtonsVisibleChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Suspends any drawing/regions update operation
        /// </summary>
        public void SuspendUpdating()
        {
            _updatingSuspended = true;
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating()
        {
            ResumeUpdating(true);
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating(bool update)
        {
            _updatingSuspended = false;

            if (update)
            {
                OnRegionsChanged();
            }
        }

        /// <summary>
        /// Removes all helper controls placed by any reason.
        /// Contol's visibility is set to false before removed.
        /// </summary>
        private void RemoveHelperControls()
        {
            RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppClicked);

            while (Controls.Count > 0)
            {
                Control ctl = Controls[0];

                ctl.Visible = false;

                Controls.Remove(ctl);
            }
        }

        /// <summary>
        /// Hittest on tab
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if a tab has been clicked</returns>
        internal bool TabHitTest(int x, int y)
        {
            //if (Rectangle.FromLTRB(Right - 10, Bottom - 10, Right, Bottom).Contains(x, y))
            //{
            //   MessageBox.Show(cr);
            //}

            //look for mouse on tabs
            foreach (RibbonTab tab in Tabs)
            {
                if (tab.TabBounds.Contains(x, y))
                {
                    ActiveTab = tab;
                    Expanded = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions()
        {
            UpdateRegions(null);
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions(Graphics g)
        {
            bool graphicsCreated = false;

            if (IsDisposed || _updatingSuspended) return;

            ///Graphics for measurement
            if (g == null)
            {
                g = CreateGraphics();
                graphicsCreated = true;
            }

            ///Saves the bottom of the tabs
            int tabsBottom = 0;

            if (RightToLeft == RightToLeft.No)
            {
                ///X coordinate reminder
                int curLeft = TabsMargin.Left + OrbBounds.Width;

                ///Saves the width of the larger tab
                int maxWidth = 0;

                #region Assign default tab bounds (best case)
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Visible || IsDesignMode())
                    {
                        Size tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                        Rectangle bounds = new Rectangle(curLeft, TabsMargin.Top,
                             TabsPadding.Left + tabSize.Width + TabsPadding.Right,
                             TabsPadding.Top + tabSize.Height + TabsPadding.Bottom);

                        tab.SetTabBounds(bounds);

                        curLeft = bounds.Right + TabSpacing;

                        maxWidth = Math.Max(bounds.Width, maxWidth);
                        tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                        tab.SetTabContentBounds(Rectangle.FromLTRB(
                             TabContentMargin.Left, tabsBottom + TabContentMargin.Top,
                             ClientSize.Width - 1 - TabContentMargin.Right, ClientSize.Height - 1 - TabContentMargin.Bottom));

                        if (tab.Active)
                        {
                            tab.UpdatePanelsRegions();
                        }
                    }
                    else
                    {
                        tab.SetTabBounds(Rectangle.Empty);
                        tab.SetTabContentBounds(Rectangle.Empty);
                    }
                }

                #endregion

                #region Reduce bounds of tabs if needed

                while (curLeft > ClientRectangle.Right && maxWidth > 0)
                {

                    curLeft = TabsMargin.Left + OrbBounds.Width;
                    maxWidth--;

                    foreach (RibbonTab tab in Tabs)
                    {
                        if (tab.Visible)
                        {
                            if (tab.TabBounds.Width >= maxWidth)
                            {
                                tab.SetTabBounds(new Rectangle(curLeft, TabsMargin.Top,
                                     maxWidth, tab.TabBounds.Height));
                            }
                            else
                            {
                                tab.SetTabBounds(new Rectangle(
                                     new Point(curLeft, TabsMargin.Top),
                                     tab.TabBounds.Size));
                            }

                            curLeft = tab.TabBounds.Right + TabSpacing;
                        }
                    }
                }

                #endregion

                #region Update QuickAccess bounds

                QuickAcessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(OrbBounds.Right + QuickAcessToolbar.Margin.Left, OrbBounds.Top - 2), QuickAcessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2010)//2010 - no need to offset for the orb
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(QuickAcessToolbar.Margin.Left, 0), QuickAcessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes : no need to offset for the orb
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(QuickAcessToolbar.Margin.Left, 0), QuickAcessToolbar.LastMeasuredSize));

                #endregion

                #region Update Caption Buttons bounds

                if (CaptionButtonsVisible)
                {
                    Size cbs = new Size(20, 20);
                    int cbg = 2;
                    CloseButton.SetBounds(new Rectangle(new Point(ClientRectangle.Right - cbs.Width - cbg, cbg), cbs));
                    MaximizeRestoreButton.SetBounds(new Rectangle(new Point(CloseButton.Bounds.Left - cbs.Width, cbg), cbs));
                    MinimizeButton.SetBounds(new Rectangle(new Point(MaximizeRestoreButton.Bounds.Left - cbs.Width, cbg), cbs));
                }

                #endregion
            }
            else
            {
                ///X coordinate reminder
                int curRight = OrbBounds.Left - TabsMargin.Left + 4;

                ///Saves the width of the larger tab
                int maxWidth = 0;

                #region Assign default tab bounds (best case)
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Visible || IsDesignMode())
                    {
                        Size tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                        curRight -= (tabSize.Width + TabsPadding.Right + TabsPadding.Left);

                        Rectangle bounds = new Rectangle(curRight, TabsMargin.Top,
                             TabsPadding.Left + tabSize.Width + TabsPadding.Right,
                             TabsPadding.Top + tabSize.Height + TabsPadding.Bottom);

                        tab.SetTabBounds(bounds);

                        maxWidth = Math.Max(bounds.Width, maxWidth);
                        tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                        tab.SetTabContentBounds(Rectangle.FromLTRB(
                             TabContentMargin.Left, tabsBottom + TabContentMargin.Top,
                             ClientSize.Width - 1 - TabContentMargin.Right, ClientSize.Height - 1 - TabContentMargin.Bottom));

                        if (tab.Active)
                        {
                            tab.UpdatePanelsRegions();
                        }
                    }
                    else
                    {
                        tab.SetTabBounds(Rectangle.Empty);
                        tab.SetTabContentBounds(Rectangle.Empty);
                    }
                }

                #endregion

                #region Reduce bounds of tabs if needed

                while (curRight < ClientRectangle.Left && maxWidth > 0)
                {
                    curRight = TabsMargin.Left + OrbBounds.Width;
                    maxWidth--;

                    foreach (RibbonTab tab in Tabs)
                    {
                        if (tab.Visible)
                        {
                            if (tab.TabBounds.Width >= maxWidth)
                            {
                                tab.SetTabBounds(new Rectangle(curRight, TabsMargin.Top,
                                     maxWidth, tab.TabBounds.Height));
                            }
                            else
                            {
                                tab.SetTabBounds(new Rectangle(
                                     new Point(curRight, TabsMargin.Top),
                                     tab.TabBounds.Size));
                            }
                            curRight = tab.TabBounds.Right + TabSpacing;
                        }
                    }
                }

                #endregion

                #region Update QuickAccess bounds

                QuickAcessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(OrbBounds.Left - QuickAcessToolbar.Margin.Right - QuickAcessToolbar.LastMeasuredSize.Width, OrbBounds.Top - 2), QuickAcessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2010) //2010 - no need to offset for the orb
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(this.ClientRectangle.Right - QuickAcessToolbar.Margin.Right - QuickAcessToolbar.LastMeasuredSize.Width, 0), QuickAcessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2013)  //Michael Spradlin - 05/03/2013 Office 2013 Style Changes: no need to offset for the orb
                    QuickAcessToolbar.SetBounds(new Rectangle(new Point(this.ClientRectangle.Right - QuickAcessToolbar.Margin.Right - QuickAcessToolbar.LastMeasuredSize.Width, 0), QuickAcessToolbar.LastMeasuredSize));

                #endregion

                #region Update Caption Buttons bounds

                if (CaptionButtonsVisible)
                {
                    Size cbs = new Size(20, 20);
                    int cbg = 2;
                    CloseButton.SetBounds(new Rectangle(new Point(ClientRectangle.Left, cbg), cbs));
                    MaximizeRestoreButton.SetBounds(new Rectangle(new Point(CloseButton.Bounds.Right, cbg), cbs));
                    MinimizeButton.SetBounds(new Rectangle(new Point(MaximizeRestoreButton.Bounds.Right, cbg), cbs));
                }

                #endregion
            }

            //Update the minimize settings
            //_minimizedHeight = tabsBottom;

            if (graphicsCreated)
                g.Dispose();

            _lastSizeMeasured = Size;

            RenewSensor();
        }

        /// <summary>
        /// Forces a size recalculation on the entire control
        /// </summary>
        internal void OnRegionsChanged()
        {
            if (_updatingSuspended) return;

            //Kevin - Fix when only one tab present and there is a textbox on it. It will loose focus after each char is entered
            //if (Tabs.Count == 1)
            if (Tabs.Count == 1 && ActiveTab != Tabs[0])
            {
                ActiveTab = Tabs[0];
            }

            _lastSizeMeasured = Size.Empty;

            Refresh();
        }

        /// <summary>
        /// Redraws the specified tab
        /// </summary>
        /// <param name="tab"></param>
        internal void RedrawTab(RibbonTab tab)
        {
            using (Graphics g = CreateGraphics())
            {
                Rectangle clip = Rectangle.FromLTRB(
                     tab.TabBounds.Left,
                     tab.TabBounds.Top,
                     tab.TabBounds.Right,
                     tab.TabBounds.Bottom);

                g.SetClip(clip);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None));
            }
        }

        /// <summary>
        /// Sets the currently selected tab
        /// </summary>
        /// <param name="tab"></param>
        private void SetSelectedTab(RibbonTab tab)
        {
            if (tab == _lastSelectedTab) return;

            if (_lastSelectedTab != null)
            {
                _lastSelectedTab.SetSelected(false);
                RedrawTab(_lastSelectedTab);
            }

            if (tab != null)
            {
                tab.SetSelected(true);
                RedrawTab(tab);
            }

            _lastSelectedTab = tab;

        }

        /// <summary>
        /// Suspends the sensor activity
        /// </summary>
        internal void SuspendSensor()
        {
            if (Sensor != null)
                Sensor.Suspend();
        }

        /// <summary>
        /// Resumes the sensor activity
        /// </summary>
        internal void ResumeSensor()
        {
            Sensor.Resume();
        }

        /// <summary>
        /// Redraws the specified area on the sensor's control
        /// </summary>
        /// <param name="area"></param>
        public void RedrawArea(Rectangle area)
        {
            Sensor.Control.Invalidate(area);
        }

        /// <summary>
        /// Activates the next tab available
        /// </summary>
        public void ActivateNextTab()
        {
            RibbonTab tab = NextTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Activates the previous tab available
        /// </summary>
        public void ActivatePreviousTab()
        {
            RibbonTab tab = PreviousTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Handles the mouse down on the orb area
        /// </summary>
        internal void OrbMouseDown()
        {
            OnOrbClicked(EventArgs.Empty);
        }

        protected override void WndProc(ref Message m)
        {

            bool bypassed = false;

            if (WinApi.IsWindows && (ActualBorderMode == RibbonWindowMode.NonClientAreaGlass || ActualBorderMode == RibbonWindowMode.NonClientAreaCustomDrawn))
            {
                if (m.Msg == WinApi.WM_NCHITTEST) //0x84
                {
                    Form f = FindForm();
                    Rectangle caption;

                    if (RightToLeft == RightToLeft.No)
                    {
                        int captionLeft = QuickAcessToolbar.Visible ? QuickAcessToolbar.Bounds.Right : OrbBounds.Right;
                        if (QuickAcessToolbar.Visible && QuickAcessToolbar.DropDownButtonVisible) captionLeft = QuickAcessToolbar.DropDownButton.Bounds.Right;
                        caption = Rectangle.FromLTRB(captionLeft, 0, Width, CaptionBarSize);
                    }
                    else
                    {
                        int captionRight = QuickAcessToolbar.Visible ? QuickAcessToolbar.Bounds.Left : OrbBounds.Left;
                        if (QuickAcessToolbar.Visible && QuickAcessToolbar.DropDownButtonVisible) captionRight = QuickAcessToolbar.DropDownButton.Bounds.Left;
                        caption = Rectangle.FromLTRB(0, 0, captionRight, CaptionBarSize);
                    }

                    Point screenPoint = new Point(WinApi.LoWord((int)m.LParam), WinApi.HiWord((int)m.LParam));
                    Point ribbonPoint = PointToClient(screenPoint);
                    bool onCaptionButtons = false;

                    if (CaptionButtonsVisible)
                    {
                        onCaptionButtons = CloseButton.Bounds.Contains(ribbonPoint) ||
                        MinimizeButton.Bounds.Contains(ribbonPoint) ||
                        MaximizeRestoreButton.Bounds.Contains(ribbonPoint);
                    }

                    if (RectangleToScreen(caption).Contains(screenPoint) && !onCaptionButtons)
                    {
                        //on the caption bar area
                        Point p = PointToScreen(screenPoint);
                        WinApi.SendMessage(f.Handle, WinApi.WM_NCHITTEST, m.WParam, WinApi.MakeLParam(p.X, p.Y));
                        m.Result = new IntPtr(-1);
                        bypassed = true;
                        //Kevin - fix so when you mouse off the caption buttons onto the caption area
                        //the buttons will clear the selection. same with the QAT buttons
                        CloseButton.SetSelected(false);
                        MinimizeButton.SetSelected(false);
                        MaximizeRestoreButton.SetSelected(false);
                        OrbSelected = false;
                        QuickAcessToolbar.DropDownButton.SetSelected(false);
                    }
                }
            }

            if (!bypassed)
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Paints the Ribbon on the specified device
        /// </summary>
        /// <param name="g">Device where to paint on</param>
        /// <param name="clip">Clip rectangle</param>
        private void PaintOn(Graphics g, Rectangle clip)
        {
            try
            {
                if (WinApi.IsWindows && Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                }

                //Caption Background
                Renderer.OnRenderRibbonBackground(new RibbonRenderEventArgs(this, g, clip));

                //Caption Bar
                Renderer.OnRenderRibbonCaptionBar(new RibbonRenderEventArgs(this, g, clip));

                //Caption Buttons
                if (CaptionButtonsVisible)
                {
                    MinimizeButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    MaximizeRestoreButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    CloseButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                }

                //Orb
                Renderer.OnRenderRibbonOrb(new RibbonRenderEventArgs(this, g, clip));

                //QuickAcess toolbar
                QuickAcessToolbar.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Compact));

                //Render Tabs
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Visible || IsDesignMode())
                    {
                        tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None, this));
                    }
                }

                if ((OrbStyle == RibbonOrbStyle.Office_2010) && _expanded == false)
                {
                    //draw the divider line at the bottom of the ribbon
                    Pen p = new Pen(Theme.ColorTable.TabBorder);
                    g.DrawLine(p, OrbBounds.Left, OrbBounds.Bottom, Bounds.Right, OrbBounds.Bottom);
                }
                else if ((OrbStyle == RibbonOrbStyle.Office_2013) && _expanded == false)
                {
                    //draw the divider line at the bottom of the ribbon
                    Pen p = new Pen(Theme.ColorTable.TabBorder_2013);
                    g.DrawLine(p, OrbBounds.Left, OrbBounds.Bottom, Bounds.Right, OrbBounds.Bottom);
                }
            }
            catch (Exception e)
            {
            }
        }

        private void PaintDoubleBuffered(Graphics wndGraphics, Rectangle clip)
        {
            using (Bitmap bmp = new Bitmap(Width, Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black);
                    PaintOn(g, clip);
                    g.Flush();

                    WinApi.BitBlt(wndGraphics.GetHdc(), clip.X, clip.Y, clip.Width, clip.Height, g.GetHdc(), clip.X, clip.Y, WinApi.SRCCOPY);
                    //WinApi.BitBlt(wndGraphics.GetHdc(), 0, 0, Width, Height, g.GetHdc(), 0, 0, WinApi.SRCCOPY);
                }

                //wndGraphics.DrawImage(bmp, Point.Empty);
            }
        }

        internal bool IsDesignMode()
        {
            return Site != null && Site.DesignMode;
        }

        #endregion

        #region Event Overrides

        /// <summary>
        /// Raises the <see cref="ActiveTabChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        protected virtual void OnActiveTabChanged(EventArgs e)
        {
            if (ActiveTabChanged != null)
            {
                ActiveTabChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActualBorderMode"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActualBorderModeChanged(EventArgs e)
        {
            if (ActualBorderModeChanged != null)
            {
                ActualBorderModeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaptionButtonsVisibleChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCaptionButtonsVisibleChanged(EventArgs e)
        {
            if (CaptionButtonsVisibleChanged != null)
            {
                CaptionButtonsVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExpandedChanged(EventArgs e)
        {
            if (ExpandedChanged != null)
            {
                ExpandedChanged(this, e);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (OrbBounds.Contains(e.Location))
            {
                OnOrbDoubleClicked(EventArgs.Empty);
            }

            foreach (RibbonTab tab in Tabs)
            {
                if (tab.Bounds.Contains(e.Location))
                {
                    this.Minimized = !this.Minimized;
                    break;
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Overriden. Raises the Paint event and draws all the Ribbon content
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (_updatingSuspended) return;

            if (Size != _lastSizeMeasured)
                UpdateRegions(e.Graphics);

            PaintOn(e.Graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Overriden. Raises the Click event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);
        }

        /// <summary>
        /// Overriden. Riases the MouseEnter event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseLeave  event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            //Console.WriteLine("Ribbon Mouse Leave");
            SetSelectedTab(null);
            if (!Expanded)
                foreach (RibbonTab tab in Tabs)
                {
                    tab.SetSelected(false);
                }
            Invalidate();
        }

        /// <summary>
        /// Overriden. Raises the MouseMove event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            // Kevin Carbis's new code, edited by adriancs, on behave of Carbis
            // The below fix some minor bug. The cursor is not displayed properly
            // when cursor is entering CheckBound of CheckBox and TextBox.
            // The cursor keep changing from Cursors.Default to Cursors.Hand a few times
            // within a second.
            // The below code is obtain from Kcarbis's website
            #region Kevin Carbis's new code, edited by adriancs
            base.OnMouseMove(e);

            if (ActiveTab == null) return;

            bool someTabHitted = false;

            //Check if mouse on tab
            if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            {
                //Do nothing, everything is on the sensor
            }
            //Check if mouse on orb
            else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            {
                OrbSelected = true;
                Invalidate(OrbBounds);
            }
            //Check if mouse on QuickAccess toolbar
            else if (QuickAcessToolbar.Visible && QuickAcessToolbar.Bounds.Contains(e.Location))
            {

            }
            else
            {
                //look for mouse on tabs
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.TabBounds.Contains(e.X, e.Y))
                    {
                        SetSelectedTab(tab);
                        someTabHitted = true;
                        tab.OnMouseMove(e);
                    }
                }
            }

            if (!someTabHitted)
                SetSelectedTab(null);

            if (OrbSelected && !OrbBounds.Contains(e.Location))
            {
                OrbSelected = false;
                Invalidate(OrbBounds);
            }
            #endregion

            #region Kevin Carbis's old code, commented out by adriancs
            //base.OnMouseMove(e);

            ////Kevin Carbis - Need to reset the curor here so we can pickup the cursor when it moves off of an active textbox.  If we don't we will
            ////have the IBeam cursor over the entire ribbon.
            //Cursor.Current = Cursors.Default;

            //if (ActiveTab == null) return;

            //bool someTabHitted = false;

            ////Check if mouse on tab
            //if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            //{
            //    //Do nothing, everything is on the sensor
            //}
            ////Check if mouse on orb
            //else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            //{
            //    OrbSelected = true;
            //    Invalidate(OrbBounds);
            //}
            ////Check if mouse on QuickAccess toolbar
            //else if (QuickAcessToolbar.Visible && QuickAcessToolbar.Bounds.Contains(e.Location))
            //{

            //}
            //else
            //{
            //    //look for mouse on tabs
            //    foreach (RibbonTab tab in Tabs)
            //    {
            //        if (tab.TabBounds.Contains(e.X, e.Y))
            //        {
            //            SetSelectedTab(tab);
            //            someTabHitted = true;
            //            tab.OnMouseMove(e);
            //        }
            //    }
            //}

            //if (!someTabHitted)
            //    SetSelectedTab(null);

            ////Clear the orb m_highlight
            //if (OrbSelected && !OrbBounds.Contains(e.Location))
            //{
            //    OrbSelected = false;
            //    Invalidate(OrbBounds);
            //}
            #endregion
        }

        /// <summary>
        /// Overriden. Raises the MouseUp event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseDown event and tunnels the message to child elements
        /// </summary>
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (OrbBounds.Contains(e.Location))
            {
                OrbMouseDown();
            }
            else
            {
                TabHitTest(e.X, e.Y);
            }

        }

        /// <summary>
        /// Handles the mouse wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            //if (Tabs.Count == 0 || ActiveTab == null) return;

            //int index = Tabs.IndexOf(ActiveTab);

            //if (e.Delta < 0)
            //{
            //   _tabSum += 0.4f;
            //}
            //else
            //{
            //   _tabSum -= 0.4f;
            //}

            //int tabRounded = Convert.ToInt16(Math.Round(_tabSum));

            //if (tabRounded != 0)
            //{
            //   index += tabRounded;

            //   if (index < 0)
            //   {
            //      index = 0;
            //   }
            //   else if (index >= Tabs.Count - 1)
            //   {
            //      index = Tabs.Count - 1;
            //   }

            //   ActiveTab = Tabs[index];
            //   _tabSum = 0f;
            //}
        }

        /// <summary>
        /// Overriden. Raises the OnSizeChanged event and performs layout calculations
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnSizeChanged(System.EventArgs e)
        {
            UpdateRegions();

            RemoveHelperControls();

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Handles when its parent has changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (!(Site != null && Site.DesignMode))
            {
                BorderMode = BorderMode;

                if (Parent is IRibbonForm)
                {
                    FormHelper.Ribbon = this;
                }
            }

            if (Parent != null)
            {
                Control p = Parent;
                while (p.Parent != null)
                    p = p.Parent;
                Form parentForm = p as Form;
                if (parentForm != null)
                    parentForm.Deactivate += new EventHandler(parentForm_Deactivate);
            }
        }

        private void parentForm_Deactivate(object sender, EventArgs e)
        {
            if (Form.ActiveForm == null)  // check for ActiveForm, because Click in Orb Menu causes the Form as well to fire the Deactivate Event
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppFocusChanged);
            }
        }

        private void OnPopupRegistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 1)
                SetUpHooks();
        }

        private void OnPopupUnregistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 0 && (Minimized == false || (Minimized && Expanded == false)))
                DisposeHooks();
        }

        #endregion

    }
}