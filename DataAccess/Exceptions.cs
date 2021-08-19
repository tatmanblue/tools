using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bigwoo.DataAccess
{
    /// <summary>
    /// This exception indicates that the record could not properly
    /// configured for the SQL command (LoadPrep etc...).  This is a programming
    /// bug
    /// </summary>
    [Serializable]
    public class DataAccessConfigurationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public DataAccessConfigurationException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public DataAccessConfigurationException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Thrown during function prep (LoadPrep etc...) when the function is INTENTIONALLY not 
    /// implemented because doing so violates a database rule (such as don't delete a record)
    /// </summary>
    [Serializable]
    public class FunctionalityNotAllowedException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public FunctionalityNotAllowedException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public FunctionalityNotAllowedException(string msg) : base(msg) { }
    }
}
