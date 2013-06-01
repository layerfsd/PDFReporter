using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using AxiomCoders.PdfTemplateEditor.EditorStuff;
using System.Windows.Forms;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    public class EditorItemsCollection : IList<EditorItem>
    {
        private List<EditorItem> items;

        /// <summary>
        /// Occurs after a new item is added to the <see cref="EditorItemsCollection"/>.
        /// </summary>
        public event EditorItemEventHandler ItemAdded;

        /// <summary>
        /// Occurs when a item is deleted from the <see cref="EditorItemsCollection"/>.
        /// </summary>
        public event EditorItemEventHandler ItemRemoved;

        /// <summary>
        /// Occurs after all item are removed from <see cref="EditorItemsCollection"/>.
        /// </summary>
        public event EventHandler CollectionCleared;
        
        public EditorItemsCollection()
        {
            items = new List<EditorItem>();
        }


        /// <summary>
        /// Raises the <see cref="ItemAdded"/> event.
        /// </summary>
        /// <param name="item"></param>
        private void OnItemAdded(EditorItem item)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, item);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemRemowed"/> event.
        /// </summary>
        /// <param name="item"></param>
        private void OnItemRemoved(EditorItem item)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, item);
            }
        }

        private void OnCollectionCleared()
        {
            if (CollectionCleared != null)
            {
                CollectionCleared(this, EventArgs.Empty);
            }
        }


        #region IList<EditorItem> Members

        public int IndexOf(EditorItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, EditorItem item)
        {
            items.Insert(index, item);
            OnItemAdded(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            OnItemRemoved(items[index]);
        }

        public EditorItem this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        #endregion

        #region ICollection<EditorItem> Members

        public void Add(EditorItem item)
        {
            items.Add(item);
            OnItemAdded(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(EditorItem item)
        {
            return items.Contains(item);
        }

        public void CopyTo(EditorItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(EditorItem item)
        {
            bool removed = items.Remove(item);
            if (removed)
            {
                OnItemRemoved(item);
            }
            return removed;
            
        }

        #endregion

        #region IEnumerable<EditorItem> Members

        public IEnumerator<EditorItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        #endregion
    }
}
