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

namespace System.Windows.Forms
{
    /// <summary>
    /// Holds data and tools to draw the element
    /// </summary>
    public class RibbonElementPaintEventArgs
        : EventArgs
    {
        private System.Drawing.Rectangle _clip;
        private Graphics _graphics;
        private RibbonElementSizeMode _mode;
        private Control _control;

        /// <param name="clip">Rectangle clip</param>
        /// <param name="graphics">Device to draw</param>
        /// <param name="mode">Size mode to draw</param>
        internal RibbonElementPaintEventArgs(Rectangle clip, Graphics graphics, RibbonElementSizeMode mode)
        {
            _clip = clip;
            _graphics = graphics;
            _mode = mode;
        }

        internal RibbonElementPaintEventArgs(Rectangle clip, Graphics graphics, RibbonElementSizeMode mode, Control control)
            : this(clip, graphics, mode)
        {
            _control = control;
        }

        /// <summary>
        /// Area that element should occupy
        /// </summary>
        public Rectangle Clip
        {
            get
            {
                return _clip;
            }
        }

        /// <summary>
        /// Gets the Device where to draw
        /// </summary>
        public System.Drawing.Graphics Graphics
        {
            get
            {
                return _graphics;
            }
        }

        /// <summary>
        /// Gets the mode to draw the element
        /// </summary>
        public RibbonElementSizeMode Mode
        {
            get
            {
                return _mode;
            }
        }


        /// <summary>
        /// Gets the control where element is being painted
        /// </summary>
        public Control Control
        {
            get { return _control; }
        }

    }
}
