#region using statements
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace Bigwoo.DataAccess
{
    /// <summary>
    /// Static utility class for getting connection strings and other project 'static' data affecting database
    /// </summary>
    public static class DataAccessConfiguration 
    {
        #region private data
        private static string _connectionString = string.Empty;
        #endregion

        #region properties
        /// <summary>
        /// 
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
        }
        #endregion

        #region ctor
        /// <summary>
        /// on first use, load the connection strings
        /// </summary>
        static DataAccessConfiguration() 
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }
        #endregion

        /// <summary>
        /// Allows custom connection string to built on the fly
        /// 
        /// Primarily a place holder for future where program might need several different connections
        /// </summary>
        /// <param name="connectionStrings"></param>
        public static void Initialize(string[] connectionStrings)
        {
            if (0 == connectionStrings.Length)
                return;

            // for now....
            _connectionString = connectionStrings[0];
        }
    }

}
