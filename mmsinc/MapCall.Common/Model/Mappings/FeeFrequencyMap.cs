using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FeeFrequencyMap : EntityLookupMap<FeeFrequency>
    {
        public const string TABLE_NAME = "FeeFrequencies";

        public FeeFrequencyMap()
        {
            Table(TABLE_NAME);
        }
    }
}
