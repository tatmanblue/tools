#region using statements
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using Bigwoo.DataAccess;
using Bigwoo.DataAccess.Exceptions;
using BigWoo.DataAccess.Exceptions;
#endregion

namespace Bigwoo.DataAccess
{
    /// <summary>
    /// Base class for all list classes
    /// </summary>
    [Serializable]
    public class ListAccessor
    {
        #region protected data
        private int _maxResults = 100;
        private int _pageIndex = 0;
        private int _sortIndex = 0;
        #endregion

        #region protected methods
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
        /// 
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="list">IPageEnabledList</param>
        private void ExecuteReader(SqlCommand cmd, IPageEnabledList list)
        {
            // exexcute the SQL getting a reader for the results
            SqlDataReader reader = cmd.ExecuteReader();

            // need to advance to pageIndex specified, if the pageIndex > 0
            if (0 < _pageIndex)
                AdvanceToPage(reader, _pageIndex);

            int rowsRead = 0;

            // now iterate through the list at the "current" poiint
            // until we get all the rows requested:  which is either
            // rows == _maxResults or we are at the end of the results
            while (true == reader.Read())
            {
                rowsRead++;

                list.LoadFromReader(reader);

                if (rowsRead >= _maxResults)
                    break;
            }

            reader.Close();
        }

        /// <summary>
        /// Page is a metaphorical term used to describe where record list should start 
        /// and end at....its all based on the value _maxResults which defines the size of
        /// a page.
        /// </summary>
        /// <param name="reader">SqlDataReader</param>
        /// <param name="pageIndex">int</param>
        private void AdvanceToPage(SqlDataReader reader, int pageIndex)
        {
            int totalRows = reader.RecordsAffected;

            // compute the number of whole and partial pages
            decimal pagesAndParts = totalRows / _maxResults;

            // there is always 1 page
            int numberOfPages = Convert.ToInt32(pagesAndParts) + 1;

            // if the pageIndex request is outside whats available 
            // then there is no point in going on...
            // this is a problem, probably should throw an exception here
            if (numberOfPages < pageIndex)
            {
                BigWoo.Common.Diagnostics.Instance.Assert(false, "calculations took us past row set");
                throw new OperationalException("calculations took us past row set");
            }

            int currentPage = 0;

            // now its 
            while (currentPage < pageIndex)
            {
                // if the current page count exceeds number of pages (less 1)
                // we have a programming problem as we shouldnt have gotten this
                // value....probably should throw an exception here
                if (currentPage >= numberOfPages - 1)
                {
                    BigWoo.Common.Diagnostics.Instance.Assert(false, "calculations took us past row set");
                    throw new OperationalException("calculations took us past row set");
                }

                // new page, new number of rows read
                int rowsAdvanced = 0;

                // advance the rows
                while (rowsAdvanced < _maxResults)
                {
                    // if we read past the row set, we have a problem with our 
                    // calculations above....probably should throw an exception here
                    if (false == reader.Read())
                    {
                        BigWoo.Common.Diagnostics.Instance.Assert(false, "calculations took us past row set");
                        throw new OperationalException("calculations took us past row set");
                    }

                    // advance our row counter
                    rowsAdvanced++;
                }

                // advance our page counter
                currentPage++;
            }

        }

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
        /// client can decide how many rows to return in one request, -1 means all
        /// </summary>
        public int MaxResults
        {
            get { return _maxResults; }
            set { _maxResults = value; }
        }

        /// <summary>
        /// indicates which page of the rows to return page is a set of rows starting and
        /// ending determined by the totalrows / maxresults
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// Indicates how the data is sorted for determines page
        /// </summary>
        public int SortIndex
        {
            get { return _sortIndex; }
            set { _sortIndex = value; }
        }        
        #endregion

        #region ctor        
        /// <summary>
        /// Default Ctor....creates instance
        /// </summary>
        public ListAccessor() { }
        #endregion

        #region public methods
        /// <summary>
        /// Loads a list of BaseRecord using a list class instance for initialize and list management
        /// </summary>
        /// <param name="list">IPageEnabledList, class methods used to populate a list</param>
        public void LoadList(IPageEnabledList list)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = GetStoredProcCommand(connection))
                {
                    list.LoadPrep(cmd, _sortIndex);

                    // get and load the record
                    ExecuteReader(cmd, list);
                }

                // close the connection
                connection.Close();
            }

        }

        /// <summary>
        /// Helper method for saving all of the items in the list.  Simply iterates through
        /// the list, calling save on each record in the list.   If the list maintains a mix
        /// of new and updated records, it is required that the record type implements both
        /// Insert and Update methods
        /// </summary>
        public void Save()
        {
            throw new NotImplementedException();
        } 
        #endregion

    }
}
