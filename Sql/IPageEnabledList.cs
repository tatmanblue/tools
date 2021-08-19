#region using statements
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
#endregion

namespace BigWoo.Sql
{
    /// <summary>
    /// Interface defines a list of BaseRecords that can be 
    /// loaded from the database.  The interface is defined by
    /// BaseList and is used by ListAccessor
    /// </summary>
    public interface IPageEnabledList
    {
        /// <summary>
        /// List classes should provide this method to initialize the command
        /// with the SQL required to generate the list
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="sortIndex">int</param>
        void LoadPrep(SqlCommand cmd, int sortIndex);

        /// <summary>
        /// Once the reader is initialized, the list must create records from
        /// the rows in the list using this method
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        void LoadFromReader(SqlDataReader reader);
    }
}
