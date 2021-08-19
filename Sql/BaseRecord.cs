#region using statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
#endregion

namespace BigWoo.Sql
{
    /// <summary>
    /// All record classess should be derived from here.  This class provides a standard identity 
    /// for all data classes.  Derived classes must override the validate method if the classes
    /// permit saving the record.  Derived classes must override the insert and/or update methods
    /// as well (its possible some records do not permit new records)
    /// 
    /// The derived class is responsible for maintaining if the record has changed.
    /// 
    /// The derived class is responsible for declaring the record as new
    /// </summary>
    [Serializable]
    public class BaseRecord
    {
        #region protected data
        /// <summary>
        /// Indicates this is a new record and Insert should be used for saving
        /// </summary>
        protected bool _isNewRecord = false;
        /// <summary>
        /// Indicates the record has changed and should be saved
        /// </summary>
        protected bool _isDirtyRecord = false;
        /// <summary>
        /// pretty much every record in the system has a int ID unique field 
        /// assigned to it...this member contains its value
        /// </summary>
        protected int _recordID = -1;
        #endregion

        #region properties
        /// <summary>
        /// Record ID, key field 
        /// </summary>
        public int ID
        {
            get { return _recordID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirtyRecord; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNew
        {
            get { return _isNewRecord; }
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Flags the record as dirty.  Will allow calls to the save method to
        /// update the database with the changes (either insert or update)
        /// </summary>
        protected void MarkIsDirty()
        {
            _isDirtyRecord = true;
        }

        /// <summary>
        /// Sets the record as new and unsaved.  Meaning call to Save will 
        /// execute the insert method.   A new record is a dirty record since
        /// it does not exist in the database as a new record.
        /// </summary>
        protected void MarkIsNewRecord()
        {
            _isNewRecord = true;
            MarkIsDirty();
        }

        #endregion

        #region property setter methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected virtual C SetProperty<C>(C oldValue, C newValue) where C : IComparable
        {
            if (false == oldValue.Equals(newValue))
            {
                MarkIsDirty();
            }

            return newValue;
        }
        #endregion

        #region protected methods (data access helper methods)
        #endregion

        #region ctor/init
        /// <summary>
        /// Hide the default constructor as consumers should never to able to
        /// directly create instances of a base record class.
        /// </summary>
        public BaseRecord() 
        {
            MarkIsNewRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordId"></param>
        public BaseRecord(int recordId)
        {
            _recordID = recordId;
        }
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadPrep(SqlCommand cmd) { throw new NotImplementedException(); }
        
        /// <summary>
        /// Override this method if the consumer is allowed to create new records
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        public virtual void InsertPrep(SqlCommand cmd) { throw new NotImplementedException(); }

        /// <summary>
        /// Override this method if the consumer is allowed to update existing records.
        /// If you do not permit updating records, then make sure MarkIsDirty() is
        /// never called in your class implementation (??? readonly properties)
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        public virtual void UpdatePrep(SqlCommand cmd) { throw new NotImplementedException(); }

        /// <summary>
        /// Override this method if the consumer is allowed to delete existing records.
        /// If you do not permit updating records, then make sure MarkIsDirty() is
        /// never called in your class implementation (??? readonly properties)
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        public virtual void DeletePrep(SqlCommand cmd) { throw new NotImplementedException(); }

        /// <summary>
        /// This method is used to validate that the data is valid and can be
        /// saved.   Rules are defined and the data checked against the rules
        /// in derived classes.  Derived classes should throw a DataNotValidException 
        /// if the data is not valid
        /// </summary>
        public virtual void Validate() { throw new NotImplementedException(); }

        /// <summary>
        /// This method is used to load a record from the database into the class members
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        public virtual void Load(SqlDataReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordId">int</param>
        public virtual void PostSave(int recordId) 
        {
            if (-1 == _recordID)
                _recordID = recordId;

            _isDirtyRecord = false;
            _isNewRecord = false;
        }
        #endregion

        #region static helper methods
        /// <summary>
        /// Helper method, returns the record id unless record is null
        /// in which case it returns the default
        /// </summary>
        /// <param name="record">BaseRecord</param>
        /// <param name="defaultVal">int</param>
        /// <returns>int</returns>
        public static int RecordID(BaseRecord record, int defaultVal)
        {
            int ret = defaultVal;

            if (null != record)
                ret = record.ID;

            return ret;
        }

        /// <summary>
        /// Helper method, returns the record id unless record is null
        /// in which case it returns -1
        /// </summary>
        /// <param name="record">BaseRecord</param>
        /// <returns>int</returns>
        public static int RecordID(BaseRecord record)
        {
            return BaseRecord.RecordID(record, -1);
        }
        #endregion
    }
}
