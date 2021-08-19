using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.DataAccess.Exceptions
{
    /// <summary>
    /// This exception is thrown during validation of the record members 
    /// indicating the record is not acceptable and cannot be saved
    /// </summary>
    [Serializable]
    public class DataNotValidException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public DataNotValidException(string msg) : base(msg) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
