#region using statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using BigWoo.DataAccess.Exceptions;
#endregion

namespace BigWoo.DataAccess.Helpers
{
    /// <summary>
    /// This class is a data access utility class used to assign in correctly
    /// converting fields retrieved from the database into C# native types
    /// </summary>
    [Serializable]
    public class SqlReaderFieldConversion
    {
        #region  private data
        private SqlDataReader _reader = null;
        #endregion

        #region  private constants
        /// <summary>
        /// Defines the default return for ints, typically returned if
        /// the column contains null
        /// </summary>
        private const int DEFAULT_INT = -1;
        /// <summary>
        /// Defines the default return for bool, typically returned if
        /// the column contains null
        /// </summary>
        private const bool DEFAULT_BOOL = false;
        /// <summary>
        /// Defines the default return for string, typically returned if
        /// the column contains null
        /// </summary>
        private const string DEFAULT_STRING = "";
        /// <summary>
        /// Defines the default return for decimal, typically returned if
        /// the column contains null
        /// </summary>
        private const decimal DEFAULT_DECIMAL = 0M;
        /// <summary>
        /// Defines the default return for dates, typically returned if
        /// the column contains null
        /// </summary>
        private readonly DateTime DEFAULT_DATETIME = BigWoo.Common.DateTimeValidation.MinDefaultValidDate;
        /// <summary>
        /// Defines the default return for GUID, typically returned if
        /// the column contains null
        /// </summary>
        private Guid DEFAULT_GUID =Guid.Empty;
        #endregion

        #region private methods
        /// <summary>
        /// this method tests for valid class instance.  Consder this method a 
        /// good way of helping to catch bugs during developer unit testing
        /// </summary>
        [DebuggerStepThrough]
        private void InsistIsFunctional()
        {

            // asserts are in place so that the developer can catch errors during debugging

            if (null == _reader)
            {
                BigWoo.Common.Diagnostics.Instance.Assert(null != _reader);
                throw new OperationalException("reader is not valid");
            }

            if (true == _reader.IsClosed)
            {
                BigWoo.Common.Diagnostics.Instance.Assert(false == _reader.IsClosed);
                throw new OperationalException("reader is closed");
            }

            if (false == _reader.HasRows)
            {
                BigWoo.Common.Diagnostics.Instance.Assert(true == _reader.HasRows);
                throw new OperationalException("reader is empty");
            }

        } 
        #endregion

        #region ctor
        /// <summary>
        /// Initialize SqlReaderFieldConversion with an open/valid
        /// SqlDataReader
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        public SqlReaderFieldConversion(SqlDataReader reader)
        {
            _reader = reader;
        }
        #endregion

        #region public conversion methods
        /// <summary>
        /// Converts input to an int32
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="DataNotValidException">All exceptions get converted to DataNotValidException</exception>
        /// <returns>int</returns>
        [DebuggerStepThrough]
        public int AsInt32(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_INT;

                return Convert.ToInt32(_reader[fieldName]);
            }
            catch
            {
                string msg = string.Format("{0} did not convert to Int32", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }

        /// <summary>
        /// Converts input to an string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="DataNotValidException">All exceptions get converted to DataNotValidException</exception>
        /// <returns>string</returns>
        [DebuggerStepThrough]
        public string AsString(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_STRING;

                return _reader[fieldName].ToString().Trim();
            }
            catch
            {
                string msg = string.Format("{0} did not convert to string", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }

        /// <summary>
        /// Converts input to an bool
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="DataNotValidException">All exceptions get converted to DataNotValidException</exception>
        /// <returns>bool</returns>
        [DebuggerStepThrough]
        public bool AsBool(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_BOOL;

                return Convert.ToBoolean(_reader[fieldName]);
            }
            catch
            {
                string msg = string.Format("{0} did not convert to bool", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }

        /// <summary>
        /// Converts input to an decimal
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="DataNotValidException">All exceptions get converted to DataNotValidException</exception>
        /// <returns>decimal</returns>
        [DebuggerStepThrough]
        public decimal AsDecimal(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_DECIMAL;

                return Convert.ToDecimal(_reader[fieldName]);
            }
            catch
            {
                string msg = string.Format("{0} did not convert to decimal", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }

        /// <summary>
        /// Converts input to DateTime
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>DateTime</returns>
        [DebuggerStepThrough]
        public DateTime AsDateTime(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_DATETIME;

                return Convert.ToDateTime(_reader[fieldName]);
            }
            catch
            {
                string msg = string.Format("{0} did not convert to DateTime", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }
        /// <summary>
        /// Converts input to GUID
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>DateTime</returns>
        [DebuggerStepThrough]
        public Guid AsGuid(string fieldName)
        {
            // determine if this instance has be initialized through 
            // a database reader, if not throw exception
            InsistIsFunctional();

            try
            {
                int fieldIndex = _reader.GetOrdinal(fieldName);

                if (true == _reader.IsDBNull(fieldIndex))
                    return DEFAULT_GUID;
                _reader.GetGuid(fieldIndex);

                return _reader.GetGuid(fieldIndex);
            }
            catch
            {
                string msg = string.Format("{0} did not convert to GUID", fieldName);
                // good chance this is a programming bug, so asserting on the exception giving
                // developers a better view into the error during debug.  More than likely
                // this means fieldName (method input) did not exist in the query (stored procedure)
                // result set.
                BigWoo.Common.Diagnostics.Instance.Assert(false, msg);

                // all exceptions get wrapped up into our exception so that it is easier
                // to catch
                throw new DataNotValidException(msg);
            }
        }
        #endregion
    }
}
