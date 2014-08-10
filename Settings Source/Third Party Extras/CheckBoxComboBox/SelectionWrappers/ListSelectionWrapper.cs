using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PresentationControls
{
    /// <summary>
    /// Maintains an additional "Selected" & "Count" value for each item in a List.
    /// Useful in the CheckBoxComboBox. It holds a reference to the List[Index] Item and 
    /// whether it is selected or not.
    /// It also caters for a Count, if needed.
    /// </summary>
    /// <typeparam name="TSelectionWrapper"></typeparam>
    public class ListSelectionWrapper<T> : List<ObjectSelectionWrapper<T>>
    {
        #region CONSTRUCTOR
        /// <summary>
        /// No property on the object is specified for display purposes, so simple ToString() operation 
        /// will be performed. And no Counts will be displayed
        /// </summary>
        public ListSelectionWrapper(IEnumerable source)
            : this(source, false)
        { }

        /// <summary>
        /// No property on the object is specified for display purposes, so simple ToString() operation 
        /// will be performed.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, bool showCounts)
            : base()
        {
            _source = source;
            _showCounts = showCounts;

            if (_source is IBindingList)
                ((IBindingList)_source).ListChanged += new ListChangedEventHandler(ListSelectionWrapper_ListChanged);

            this.Populate();
        }
        /// <summary>
        /// A Display "Name" property is specified. ToString() will not be performed on items.
        /// This is specifically useful on DataTable implementations, or where PropertyDescriptors are used to read the values.
        /// If a PropertyDescriptor is not found, a Property will be used.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, string usePropertyAsDisplayName)
            : this(source, false, usePropertyAsDisplayName)
        { }

        /// <summary>
        /// A Display "Name" property is specified. ToString() will not be performed on items.
        /// This is specifically useful on DataTable implementations, or where PropertyDescriptors are used to read the values.
        /// If a PropertyDescriptor is not found, a Property will be used.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, bool showCounts, string usePropertyAsDisplayName)
            : this(source, showCounts)
        {
            _displayNameProperty = usePropertyAsDisplayName;
        }
        #endregion

        #region MEMBERS
        /// <summary>
        /// Is a Count indicator used.
        /// </summary>
        private Boolean _showCounts;
        /// <summary>
        /// The original List of values wrapped. A "Selected" and possibly "Count" functionality is added.
        /// </summary>
        private IEnumerable _source;
        /// <summary>
        /// Used to indicate NOT to use ToString(), but read this property instead as a display value.
        /// </summary>
        private String _displayNameProperty = null;
        /// <summary>
        /// TODO: Documentation Member
        /// </summary>
        private String _textSeparator = ", ";

        private String _allDisplay;
        private String _noneDisplay;
        #endregion

        #region PROPERTIES
        public String AllDisplay
        {
            get { return _allDisplay; }
            set { _allDisplay = value; }
        }
        public String NoneDisplay
        {
            get { return _noneDisplay; }
            set { _noneDisplay = value; }
        }

        /// <summary>
        /// When specified, indicates that ToString() should not be performed on the items. 
        /// This property will be read instead. 
        /// This is specifically useful on DataTable implementations, where PropertyDescriptors are used to read the values.
        /// </summary>
        public String DisplayNameProperty
        {
            get { return _displayNameProperty; }
            set { _displayNameProperty = value; }
        }
        /// <summary>
        /// TODO: Documentation Property
        /// </summary>
        public String TextSeparator
        {
            get { return _textSeparator; }
            set { _textSeparator = value; }
        }
        /// <summary>
        /// Builds a concatenation list of selected items in the list. The flag determines whether or not to return the all/none strings.
        /// </summary>
        public String SelectedNames(bool rawNames = false)
        {

            String text = String.Empty;
            int selectedCount = 0;

            foreach (ObjectSelectionWrapper<T> item in this)
            {
                if (item.Selected)
                {
                    text += String.IsNullOrEmpty(text) ? item.Name
                                                       : String.Format("{0}{1}", this.TextSeparator, item.Name);
                    selectedCount++;
                }
            }
            if (!rawNames)
            {
                if (selectedCount == this.Count)
                    text = _allDisplay;
                else if (selectedCount <= 0)
                    text = _noneDisplay;
            }
            return text;

        }
        /// <summary>
        /// Indicates whether the Item display value (Name) should include a count.
        /// </summary>
        public Boolean ShowCounts
        {
            get { return _showCounts; }
            set { _showCounts = value; }
        }
        #endregion

        #region HELPER MEMBERS
        /// <summary>
        /// Reset all counts to zero.
        /// </summary>
        public void ClearCounts()
        {
            foreach (ObjectSelectionWrapper<T> item in this)
                item.Count = 0;
        }

        /// <summary>
        /// Creates a ObjectSelectionWrapper item.
        /// Note that the constructor signature of sub classes classes are important.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private ObjectSelectionWrapper<T> CreateSelectionWrapper(IEnumerator obj)
        {
            Type[] types = new Type[] { typeof(T), this.GetType() };
            ConstructorInfo ci = typeof(ObjectSelectionWrapper<T>).GetConstructor(types);

            if (ci == null)
            {
                throw new Exception(String.Format(
                              "The selection wrapper class {0} must have a constructor with ({1} Item, {2} Container) parameters.",
                              typeof(ObjectSelectionWrapper<T>),
                              typeof(T),
                              this.GetType()));
            }

            object[] parameters = new object[] { obj.Current, this };
            object result = ci.Invoke(parameters);

            return (ObjectSelectionWrapper<T>)result;
        }

        /// <summary>
        /// TODO: Documentation FindObjectWithItem
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ObjectSelectionWrapper<T> FindObjectWithItem(T obj)
        {
            return this.Find(new Predicate<ObjectSelectionWrapper<T>>(
                             (ObjectSelectionWrapper<T> target) =>
                             {
                                 return target.Item.Equals(obj);
                             }));
        }

        /*
        public TSelectionWrapper FindObjectWithKey(object key)
        {
            return FindObjectWithKey(new object[] { key });
        }

        public TSelectionWrapper FindObjectWithKey(object[] keys)
        {
            return Find(new Predicate<TSelectionWrapper>(
                            delegate(TSelectionWrapper target)
                            {
                                return
                                    ReflectionHelper.CompareKeyValues(
                                        ReflectionHelper.GetKeyValuesFromObject(target.Item, target.Item.TableInfo),
                                        keys);
                            }));
        }

        public object[] GetArrayOfSelectedKeys()
        {
            List<object> List = new List<object>();
            foreach (TSelectionWrapper Item in this)
                if (Item.Selected)
                {
                    if (Item.Item.TableInfo.KeyProperties.Length == 1)
                        List.Add(ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
                    else
                        List.Add(ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
                }
            return List.ToArray();
        }

        public T[] GetArrayOfSelectedKeys<T>()
        {
            List<T> List = new List<T>();
            foreach (TSelectionWrapper Item in this)
                if (Item.Selected)
                {
                    if (Item.Item.TableInfo.KeyProperties.Length == 1)
                        List.Add((T)ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
                    else
                        throw new LibraryException("This generator only supports single value keys.");
                    // List.Add((T)ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
                }
            return List.ToArray();
        }
        */

        /// <summary>
        /// TODO: Documentation Populate
        /// </summary>
        private void Populate()
        {
            this.Clear();
            /*
            for(int Index = 0; Index <= _Source.Count -1; Index++)
                Add(CreateSelectionWrapper(_Source[Index]));
             */
            IEnumerator enumerator = _source.GetEnumerator();

            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                    this.Add(CreateSelectionWrapper(enumerator));
            }
        }
        #endregion

        #region EVENT HANDLERS
        /// <summary>
        /// TODO: Documentation ListSelectionWrapper_ListChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListSelectionWrapper_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    this.Add(this.CreateSelectionWrapper((IEnumerator)((IBindingList)_source)[e.NewIndex]));
                    break;

                case ListChangedType.ItemDeleted:
                    this.Remove(this.FindObjectWithItem((T)((IBindingList)_source)[e.OldIndex]));
                    break;

                case ListChangedType.Reset:
                    this.Populate();
                    break;
            }
        }
        #endregion
    }
}
