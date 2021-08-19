#region using statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Bigwoo.DataAccess;
using BigWoo.Common;
using Bigwoo.DataAccess.Exceptions;
#endregion

namespace Bigwoo.DataAccess
{
    /// <summary>
    /// data structures do not know how to get the raw data to/from
    /// the data store.  The RecordAccessor class is responsible for functionality
    /// of getting the data into and from the datastore (aka db).
    /// </summary>
    [Serializable]
    public class RecordAccesor
    {
        #region protected data
        private static Type EXCLUDE_ID_ATTR_TYPE = typeof(BigWoo.DataAccess.Attributes.ExcludeRecordIdAttribute);
        private static Type EXCLUDE_KEY_ATTR_TYPE = typeof(BigWoo.DataAccess.Attributes.ExcludeRecordKeyAttribute);

        private BaseRecord _dataRecord = null;
        private Type _baseRecordType = null;
        #endregion

        #region properties
        /// <summary>
        /// 
        /// </summary>
        public BaseRecord Record
        {
            get { return _dataRecord; }
        }
        #endregion

        #region protected methods
        #endregion

        #region property setter methods
        #endregion

        #region methods for accessing internal methods of BaseRecord
        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.InserPrep
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="cmd"></param>
        protected void InsertPrep(SqlCommand cmd)
        {
            ReflectionUtility.InvokeInternalMethod("InsertPrep", _dataRecord, new object[] { cmd });
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.UpdatePrep
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="cmd"></param>
        protected void UpdatePrep(SqlCommand cmd)
        {
            ReflectionUtility.InvokeInternalMethod("UpdatePrep", _dataRecord, new object[] { cmd });
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.DeletePrep
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="cmd"></param>
        protected void DeletePrep(SqlCommand cmd)
        {
            ReflectionUtility.InvokeInternalMethod("DeletePrep", _dataRecord, new object[] { cmd });
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.LoadPrep
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="cmd"></param>
        protected void LoadPrep(SqlCommand cmd)
        {
            ReflectionUtility.InvokeInternalMethod("LoadPrep", _dataRecord, new object[] { cmd });
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.Load
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="reader"></param>
        protected void Load(SqlDataReader reader)
        {
            ReflectionUtility.InvokeInternalMethod("Load", _dataRecord, new object[] { reader });
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.Validate
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        protected void Validate()
        {
            ReflectionUtility.InvokeInternalMethod("Validate", _dataRecord, null);
        }

        /// <summary>
        /// To keep consumers from ineherently calling BaseRecord.PostSave
        /// the method is defined internal.  Well that doesnt work since 
        /// this class is in a different name space so we are going to use reflection
        /// to invoke that method
        /// </summary>
        /// <param name="recordID"></param>
        protected void PostSave(int recordID)
        {
            ReflectionUtility.InvokeInternalMethod("PostSave", _dataRecord, new object[] { recordID });
        }
        #endregion

        #region protected methods (data access helper methods)
        /// <summary>
        /// Helper method for creating/getting a connection to the database.  
        /// 
        /// PLEASE NOTE:  consumer of this method, MUST CALL Close() on the connection
        /// </summary>
        /// <returns>SqlConnection</returns>
        protected SqlConnection GetConnection()
        {
            string connectionString = DataAccessConfiguration.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            // if this assert fires the connection string is not valid, or the database doesnt exist
            BigWoo.Common.Diagnostics.Instance.Assert(null != connection);

            // open the connection so the consumer doesnt have to worry about it
            connection.Open();

            return connection;
        }

        /// <summary>
        /// Helper method for creating a command object used for a stored proc.  
        /// </summary>
        /// <param name="connection">SqlConnection</param>
        /// <returns>SqlCommand</returns>
        protected SqlCommand GetStoredProcCommand(SqlConnection connection)
        {
            // callers are expected to pass in a valid connection object
            BigWoo.Common.Diagnostics.Instance.Assert(null != connection);            

            SqlCommand cmd = new SqlCommand();

            // setup for running a stored up
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return cmd;
        }

        /// <summary>
        /// helper method for executing an insert stored proc.  This method supports inserts 
        /// into a table that has the identity attribute as a key field.  It is required
        /// of the stored procedure to properly set the new record identity value to the output parameter for the
        /// return to work correctly.
        /// </summary>
        /// <returns>int, value of the output parameter, expectation is the keyfield.  If no output parameter
        /// is defined, returns -1</returns>
        protected virtual int Insert()
        {
            int retValue = -1;

            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = GetStoredProcCommand(connection))
                {
                    InsertPrep(cmd);
                    
                    // expectation is all insert stored procs contain an variable defined
                    // as such below for the purposes of getting the record id
                    SqlParameter outputParam = null;
                    
                    // it is possible records will not have this recordId field and in that case we 
                    // do not want to create a parameter for it
                    if (false == AssemblyUtility.HasAttribute(EXCLUDE_ID_ATTR_TYPE, _baseRecordType))
                    {
                        outputParam = new SqlParameter();
                        outputParam.Direction = System.Data.ParameterDirection.Output;
                        outputParam.ParameterName = "@RecordID";
                        outputParam.SqlDbType = System.Data.SqlDbType.Int;
                        cmd.Parameters.Add(outputParam);
                    }

                    // it is possible records will not have the key field and in that case we do not
                    // want to create the parameter for it
                    if (false == AssemblyUtility.HasAttribute(EXCLUDE_KEY_ATTR_TYPE, _baseRecordType))
                    {
                        SqlParameter keyParameter = new SqlParameter();
                        keyParameter.Direction = System.Data.ParameterDirection.Input;
                        keyParameter.ParameterName = "@RecordKey";
                        keyParameter.DbType = System.Data.DbType.Guid;
                        keyParameter.Value = _dataRecord.Key;
                        cmd.Parameters.Add(keyParameter);
                    }
                    
                    int rows = cmd.ExecuteNonQuery();

                    // it is a programming error if no rows were inserted
                    BigWoo.Common.Diagnostics.Instance.Assert(1 == rows);

                    // get the output parameter value and return it if
                    // an output parameter was declared
                    if (null != outputParam)
                        retValue = (int) cmd.Parameters[outputParam.ParameterName].Value;
                }

                // close the connection
                connection.Close();
            }

            return retValue;
        }

        /// <summary>
        /// Helper method for executing update stored procedures.
        /// </summary>
        protected virtual void Update()
        {            
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = GetStoredProcCommand(connection))
                {
                    UpdatePrep(cmd);

                    
                    // it is possible records will not have this recordId field and in that case we 
                    // do not want to create a parameter for it
                    if (false == AssemblyUtility.HasAttribute(EXCLUDE_ID_ATTR_TYPE, _baseRecordType))
                    {
                        // expectation is all the update stored procs contain a parameter called RecordId 
                        // as such below for the purposes of identifying the record to be updated
                        SqlParameter recordIdParam = new SqlParameter();
                        recordIdParam.Direction = System.Data.ParameterDirection.Input;
                        recordIdParam.ParameterName = "@RecordID";
                        recordIdParam.SqlDbType = System.Data.SqlDbType.Int;
                        recordIdParam.Value = _dataRecord.ID;
                        cmd.Parameters.Add(recordIdParam);
                    }


                    int rows = cmd.ExecuteNonQuery();

                    // it is a programming error if no rows were updated
                    BigWoo.Common.Diagnostics.Instance.Assert(1 == rows);

                }

                // close the connection
                connection.Close();
            }

        }

        /// <summary>
        /// Helper method for executing delete stored procedures.
        /// </summary>
        protected virtual void Delete()
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = GetStoredProcCommand(connection))
                {
                    DeletePrep(cmd);

                    int rows = cmd.ExecuteNonQuery();

                    // it is a programming error if no rows were updated
                    BigWoo.Common.Diagnostics.Instance.Assert(0 < rows);

                }

                // close the connection
                connection.Close();
            }

        }

        /// <summary>
        /// Using the SqlCommand object passed in creates a SqlDataRead and calls load on the results.
        /// Exception will be thrown if no record is found. The virtual method Load(SqlDataReader) will
        /// be called if record is retrieved.
        /// 
        /// Please note:  this method assumes there will only be one row retrieved
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        protected virtual void ExecuteReader(SqlCommand cmd)
        {
            ExecuteReader(cmd, true);
        }

        /// <summary>
        /// Using the SqlCommand object passed in creates a SqlDataRead and calls load on the results.
        /// Exception will be thrown if no record is found and rowsExpected is true. The virtual method Load(SqlDataReader) will
        /// be called if record is retrieved.  
        /// 
        /// Please note:  this method assumes there will only be one row retrieved
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="rowsExpected">bool, true if you want to force check for records returned</param>
        /// <exception cref="RowsExpectedException">RowsExpectedException when no rows are returned on rowsExpected is true</exception>
        protected void ExecuteReader(SqlCommand cmd, bool rowsExpected)
        {
            // this assert checks that the SqlCommand object was created proir to calling this 
            // method as its required the consumer creates the command object
            BigWoo.Common.Diagnostics.Instance.Assert(null != cmd);

            // running it
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // mostly likely, if this exception is thrown, this is either a programming error or 
                // a system error such as uninitalized database or a corrupt database.
                // but we need to account for the possiblility that there could actually be progamtic reasons for
                // this condition to be true.  So we throw the exception (rather than an assert).  
                // Consumers must be prepared to catch this exception if they set rowsExpected = true;
                if (true == rowsExpected)
                    if (false == reader.HasRows)
                        throw new RowsExpectedException(string.Format("load methods for {0}(proc: {1}) was expected to return at least 1 row", GetType().Name, cmd.CommandText));

                // and process the rows
                if (true == reader.Read())
                {
                    Load(reader);
                }

                reader.Close();
            }
        }
        #endregion

        #region ctor/init
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">BaseRecord</param>
        public RecordAccesor(BaseRecord record) 
        {
            _dataRecord = record;
            _baseRecordType = _dataRecord.GetType();
        }
        #endregion

        #region public methods
        /// <summary>
        /// helper method for retrieving message records from the database using a stored procedure.   
        /// </summary>
        public virtual void Load()
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = GetStoredProcCommand(connection))
                {

                    LoadPrep(cmd);

                    // get and load the record
                    ExecuteReader(cmd);
                }

                // close the connection
                connection.Close();
            }

        }

        /// <summary>
        /// Causes the record to be validated and then inserted or updated in the database
        /// if the record has changed.  A DataNotValidException exception will be
        /// thrown if the data does not validate.   Standard .NET exceptions may be
        /// thrown as well.
        /// </summary>
        public void Save()
        {
            // performance gain if we only save records that are
            // really changed.  ATM, this requires derived classes
            // to correctly identify when the record has changed by
            // calling MarkIsDirty() in their property setter methods
            if (false == _dataRecord.IsDirty)
                return;

            try
            {
                // Validate the data conforms to the rules.  Rules are 
                // defined and validated in the derived class.  The 
                // derived class must implement this method.   It should
                // throw an exception if the data does not validate
                Validate();
            }
            catch (NotImplementedException) { }

            int recordID = _dataRecord.ID;

            // now save the record...insert or update accordingly
            // the derived class must implement the insert and or
            // update methods...exceptions get thrown if method used
            // is not implemented in the derived class.
            if (true == _dataRecord.IsNew)
                recordID = Insert();
            else
                Update();

            // give derived class to do post save functionality
            // typically applicable when a record class contains
            // subclasses that need to be saved as well
            PostSave(recordID);
        }
        #endregion

        #region static helper methods
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRecord"></typeparam>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static TRecord LoadById<TRecord>(int recordId) where TRecord : BaseRecord
        {
            TRecord record = null;
            ReflectionUtility.SetPrivateMemberValueType<TRecord, int>(ref record, "_recordID", recordId);
            RecordAccesor accessor = new RecordAccesor(record);
            accessor.Load();

            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRecord"></typeparam>
        /// <param name="recordKey"></param>
        /// <returns></returns>
        public static TRecord LoadByKey<TRecord>(Guid recordKey) where TRecord : BaseRecord
        {
            TRecord record = null;
            ReflectionUtility.SetPrivateMemberValueType<TRecord, Guid>(ref record, "_recordKey", recordKey);
            RecordAccesor accessor = new RecordAccesor(record);
            accessor.Load();

            return record;
        }

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
            return RecordAccesor.RecordID(record, -1);
        }
        #endregion
    }
}
