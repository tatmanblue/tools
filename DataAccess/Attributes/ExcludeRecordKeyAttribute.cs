using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigWoo.DataAccess.Attributes
{
    /// <summary>
    /// when this attribute is applied to a class derived from BaseRecord
    /// the insert method automatically adds parameter for _recordKey
    /// </summary>
    public class ExcludeRecordKeyAttribute : Attribute {}
}
