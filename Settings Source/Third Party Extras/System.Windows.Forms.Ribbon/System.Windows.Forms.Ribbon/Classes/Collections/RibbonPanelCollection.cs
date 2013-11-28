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

namespace System.Windows.Forms
{
    /// <summary>
    /// Represents a collection of RibbonPanel objects
    /// </summary>
    public sealed class RibbonPanelCollection
        : List<RibbonPanel>
    {
        private RibbonTab _ownerTab;

        /// <summary>
        /// Creates a new RibbonPanelCollection
        /// </summary>
        /// <param name="ownerTab">RibbonTab that contains this panel collection</param>
        /// <exception cref="ArgumentNullException">ownerTab is null</exception>
        public RibbonPanelCollection(RibbonTab ownerTab)
        {
           if (ownerTab == null) throw new ArgumentNullException("ownerTab");

           _ownerTab = ownerTab;
        }

        /// <summary>
        /// Gets the Ribbon that contains this panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner
        {
            get
            {
                return _ownerTab.Owner;
            }
        }

        /// <summary>
        /// Gets the RibbonTab that contains this panel collection
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
        /// Adds the specified item to the collection
        /// </summary>
        public new void Add(RibbonPanel item)
        {
            item.SetOwner(Owner);
            item.SetOwnerTab(OwnerTab);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of panels to the collection
        /// </summary>
        /// <param name="items">Panels to add</param>
        public new void AddRange(System.Collections.Generic.IEnumerable<System.Windows.Forms.RibbonPanel> items)
        {
            foreach (RibbonPanel p in items)
            {
                p.SetOwner(Owner);
                p.SetOwnerTab(OwnerTab);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified panel at the desired index
        /// </summary>
        /// <param name="index">Desired index to insert the panel</param>
        /// <param name="item">Panel to insert</param>
        public new void Insert(int index, System.Windows.Forms.RibbonPanel item)
        {
            item.SetOwner(Owner);
            item.SetOwnerTab(OwnerTab);
            base.Insert(index, item);
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            foreach (RibbonPanel panel in this)
            {
                panel.SetOwner(owner);
            }
        }

        /// <summary>
        /// Sets the value of the OwnerTab Property
        /// </summary>
        /// <param name="onwerTab"></param>
        internal void SetOwnerTab(RibbonTab ownerTab)
        {
            _ownerTab = ownerTab;

            foreach (RibbonPanel panel in this)
            {
                panel.SetOwnerTab(OwnerTab);
            }
        }
    }

}
