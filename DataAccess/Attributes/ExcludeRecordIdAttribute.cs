using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.DataAccess.Attributes
{
    /// <summary>
    /// if this attribute is on a base record class then the record accessor class
    /// will not attempt to add a RecordId parameter to insert sql 
    /// </summary>
    public class ExcludeRecordIdAttribute : Attribute {}
}
