#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
#endregion

namespace BigWoo.DataAccess.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationalException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public OperationalException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public OperationalException(string msg) : base(msg) { }
    }
}
