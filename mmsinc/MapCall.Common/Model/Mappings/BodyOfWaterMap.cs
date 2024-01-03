using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BodyOfWaterMap : ClassMap<BodyOfWater>
    {
        #region Constants

        public const string TABLE_NAME = "BodiesOfWater";

        #endregion

        #region Constructors

        public BodyOfWaterMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "BodyOfWaterID");
            Map(x => x.Name).Nullable();
            Map(x => x.CriticalNotes).Nullable();
            References(x => x.OperatingCenter).Nullable();
        }

        #endregion
    }
}
