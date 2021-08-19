#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
#endregion

namespace Bigwoo.DataAccess.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class RowsExpectedException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public RowsExpectedException() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public RowsExpectedException(string msg) : base(msg) { }
    }
}
