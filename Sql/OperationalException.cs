using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.Sql
{
    /// <summary>
    /// indicates that the operation was not functional as is
    /// </summary>
    public class OperationalException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">string</param>
        public OperationalException(string msg) : base(msg) { }
    }
}
