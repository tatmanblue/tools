using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.Sql
{
    /// <summary>
    /// Used by load queries where at least 1 row is expected. default for list is no
    /// rows expected so then it doesnt throw exception on empty list.  for
    /// record classes is record excepted and exception will be thrown.
    /// </summary>
    public class RowsExpectedException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public RowsExpectedException(string msg) : base(msg) { }
    }
}
