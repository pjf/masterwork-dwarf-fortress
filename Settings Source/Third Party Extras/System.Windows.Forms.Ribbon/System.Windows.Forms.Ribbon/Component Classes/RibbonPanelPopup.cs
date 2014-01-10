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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [ToolboxItem(false)]
    public partial class RibbonPanelPopup : RibbonPopup
    {
        #region Fields
        private RibbonMouseSensor _sensor;
        private RibbonPanel _panel;
        private bool _ignoreNext;

        #endregion

        #region Ctor
        internal RibbonPanelPopup(RibbonPanel panel)
        {
            DoubleBuffered = true;

            _sensor = new RibbonMouseSensor(this, panel.Owner, panel.Items);
            _sensor.PanelLimit = panel;
            _panel = panel;
            _panel.PopUp = this;
            panel.Owner.SuspendSensor();

            using (Graphics g = CreateGraphics())
            {
                panel.overflowBoundsBuffer = panel.Bounds;
                Size s = panel.SwitchToSize(this, g, GetSizeMode(panel));
                s.Width += 100;
                s.Height += 100;
                Size = s;
                
            }

            foreach (RibbonItem item in panel.Items)
            {
                item.SetCanvas(this);
            }
        } 
        #endregion
        
        #region Props

        public RibbonMouseSensor Sensor
        {
            get
            {
                return _sensor;
            }
        }

        /// <summary>
        /// Gets the panel related to the form
        /// </summary>
        public RibbonPanel Panel
        {
            get
            {
                return _panel;
            }
        } 

        #endregion

        #region Methods

        public RibbonElementSizeMode GetSizeMode(RibbonPanel pnl)
        {
            if (pnl.FlowsTo == RibbonPanelFlowDirection.Right)
            {
                return RibbonElementSizeMode.Medium;
            }
            else
            {
                return RibbonElementSizeMode.Large;
            }
        }

        /// <summary>
        /// Prevents the form from being hidden the next time the mouse clicks on the form.
        /// It is useful for reacting to clicks of items inside items.
        /// </summary>
        public void IgnoreNextClickDeactivation()
        {
            _ignoreNext = true;
        }

        #endregion

        #region Overrides
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);


            if (_ignoreNext)
            {
                _ignoreNext = false;
                return;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Panel.Owner.Renderer.OnRenderPanelPopupBackground(
                new RibbonCanvasEventArgs(Panel.Owner, e.Graphics, new Rectangle(Point.Empty, ClientSize), this, Panel));

            foreach (RibbonItem item in Panel.Items)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.Large));
            }

            Panel.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(Panel.Owner, e.Graphics, e.ClipRectangle, Panel, this));

        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (RibbonItem item in _panel.Items)
            {
                item.SetCanvas(null);
            }

            Panel.SetPressed(false);
            Panel.SetSelected(false);
            Panel.Owner.UpdateRegions();
            Panel.Owner.Refresh();
            Panel.PopUp = null;
            Panel.Owner.ResumeSensor();

            Panel.PopupShowed = false;
            
            Panel.Owner.RedrawArea(Panel.Bounds);
            base.OnClosed(e);
        } 

        #endregion

        #region Shadow

        


        #endregion
    }
}