using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingModuleCategoryMap : ClassMap<TrainingModuleCategory>
    {
        #region Constants

        public const string TABLE_NAME = "TrainingModuleCategories";

        #endregion

        #region Constructors

        public TrainingModuleCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "TrainingModuleCategoryID");
            Map(x => x.Description);
        }

        #endregion
    }
}
