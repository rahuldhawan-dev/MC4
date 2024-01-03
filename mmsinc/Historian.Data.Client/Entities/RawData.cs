using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Data.Client.Entities
{
    public class RawData
    {
        public virtual string TagName { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual decimal Value { get; set; }
    }
}
