#region using statements
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using BigWoo.Common;
#endregion

namespace Bigwoo.DataAccess
{
    /// <summary>
    /// Base class for all list classes
    /// </summary>
    [Serializable]
    public class BaseList<RecordType> : IPageEnabledList where RecordType : BaseRecord
    {
        #region protected data
        /// <summary>
        /// indicates if the list is readonly and when true prevents save
        /// from functioning
        /// </summary>
        protected bool _isReadOnly = false;
        /// <summary>
        /// 
        /// </summary>
        protected List<RecordType> _items = null;
        #endregion

        #region protected methods
        /// <summary>
        /// makes the list read only, by making the list read only the save
        /// method does nothing
        /// </summary>
        protected void MarkIsReadOnly()
        {
            _isReadOnly = true;
        }
        #endregion

        #region protected methods (data access helper methods)
        /// <summary>
        /// Called from LoadFromStoredProc, used to create a record class from a single record 
        /// 
        /// Please note: do not advance the reader to the next record.  That is handled for you.
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        protected virtual void LoadItemFromReader(SqlDataReader reader) { throw new NotImplementedException(); }
        #endregion

        #region public properties
        /// <summary>
        /// List of BaseRecord
        /// </summary>
        public List<RecordType> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Indicates the list is set to read only.  When the list is set to read only 
        /// calling Save at the list level does nothing.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }
        #endregion

        #region ctor
        /// <summary>
        /// default ctor creates the list of the type specified
        /// </summary>
        protected BaseList()
        {
            _items = new List<RecordType>();
        }
        #endregion

        #region public methods
        #endregion

        #region IPageEnabledList Members
        /// <summary>
        /// List classes should override this method to properly initialize the load functions
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="sortIndex">int</param>
        public virtual void LoadPrep(SqlCommand cmd, int sortIndex) { throw new NotImplementedException(); }

        /// <summary>
        /// Allocates a record of RecordType, calls Load(SqlDataReader) for it and adds it to our list
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        public virtual void LoadFromReader(SqlDataReader reader)
        {
            // TODO: the new() constraint is nasty and need to find a way outta that
            // probably by using reflect in the loadfromreader
            RecordType nextRecord = ReflectionUtility.InvokeDefaultCtor<RecordType>();
            nextRecord.Load(reader);
            _items.Add(nextRecord);
        }
        #endregion
    }
}
