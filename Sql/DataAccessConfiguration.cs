using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.Sql
{
    /// <summary>
    /// this class contains the interchangable database connection string data
    /// </summary>
    public static class DataAccessConfiguration
    {
        /// <summary>
        /// 2
        /// </summary>
        public static DataAccessToken Default { get; set; }
    }

    /// <summary>
    /// this class contains the a single database connection token
    /// </summary>
    public class DataAccessToken
    {
        /// <summary>
        /// 
        /// </summary>
        public string Connection { get; internal set; }
    }

}
