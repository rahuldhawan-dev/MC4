﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ReviewFrequencyMap : EntityLookupMap<ReviewFrequency>
    {
        public ReviewFrequencyMap()
        {
            Table("ReviewFrequencies");
        }
    }
}
