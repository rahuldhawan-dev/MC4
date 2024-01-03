using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TailgateTalkMap : ClassMap<TailgateTalk>
    {
        public const string TABLE_NAME = "tblTailgateTalks",
                            SQL_SERVER_FORMULA =
                                "(SELECT TOP 1 e.{0} FROM {1} e WHERE e.{2} = {3})",
                            SQLITE_FORMULA =
                                "(SELECT e.{0} FROM {1} e WHERE e.{2} = {3} LIMIT 1)";

        public TailgateTalkMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "TailgateTalkId");

            Map(x => x.HeldOn);
            Map(x => x.TrainingTimeHours);

            References(x => x.Topic, "TailgateTopicId").Nullable().Not.LazyLoad();
            References(x => x.PresentedBy, "PresentedBy").Nullable().Not.LazyLoad();

            var sqlServerOpCntr = String.Format(SQL_SERVER_FORMULA, "OperatingCenterId", EmployeeMap.TABLE_NAME,
                "tblEmployeeID", "PresentedBy");
            var sqliteOpCntr = String.Format(SQLITE_FORMULA, "OperatingCenterId", EmployeeMap.TABLE_NAME,
                "tblEmployeeID", "PresentedBy");
            var sqlServerCategory = String.Format(SQL_SERVER_FORMULA, "TailgateCategory",
                TailgateTalkTopicMap.TABLE_NAME, "TailgateTopicId", "TailgateTopicId");
            var sqliteCategory = String.Format(SQLITE_FORMULA, "TailgateCategory",
                TailgateTalkTopicMap.TABLE_NAME, "TailgateTopicId", "TailgateTopicId");

            References(x => x.OperatingCenter)
               .DbSpecificFormula(sqlServerOpCntr, sqliteOpCntr).Not.LazyLoad();
            References(x => x.Category)
               .DbSpecificFormula(sqlServerCategory, sqliteCategory).Not.LazyLoad();

            HasMany(x => x.TailgateTalkEmployees)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TailgateTalkDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TailgateTalkNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
