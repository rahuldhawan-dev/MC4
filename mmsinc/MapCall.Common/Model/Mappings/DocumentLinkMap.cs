using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class DocumentLinkMap : ClassMap<DocumentLink>
    {
        private readonly string _nextReviewDateSqlServerFormula = $@"(CASE 
                    WHEN ReviewFrequency IS NOT NULL AND ReviewFrequencyUnitId = {RecurringFrequencyUnit.Indices.YEAR}
                        THEN dbo.dateaddplus('{"y"}', ReviewFrequency, UpdatedAt)
                    WHEN ReviewFrequency IS NOT NULL AND ReviewFrequencyUnitId = {RecurringFrequencyUnit.Indices.MONTH}
                        THEN dbo.dateaddplus('{"m"}', ReviewFrequency, UpdatedAt)
                    WHEN ReviewFrequency IS NOT NULL AND ReviewFrequencyUnitId = {RecurringFrequencyUnit.Indices.WEEK}
                        THEN dbo.dateaddplus('{"w"}', ReviewFrequency, UpdatedAt)
                    WHEN ReviewFrequency IS NOT NULL AND ReviewFrequencyUnitId = {RecurringFrequencyUnit.Indices.DAY}
                        THEN dbo.dateaddplus('{"d"}', ReviewFrequency, UpdatedAt)
                    ELSE
                        NULL
                    END)";

        #region Constants

        public const string TABLE_NAME = "DocumentLink";

        #endregion

        #region Constructors

        public DocumentLinkMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "DocumentLinkId");
            Map(x => x.LinkedId, "DataLinkId");
            References(x => x.Document);
            References(x => x.DataType);
            References(x => x.DocumentType);
            Map(x => x.UpdatedAt).Not.Nullable();
            References(x => x.UpdatedBy);
            References(x => x.DocumentStatus).Column("DocumentStatusId").Nullable();
            Map(x => x.ReviewFrequency).Nullable();
            References(x => x.ReviewFrequencyUnit).Column("ReviewFrequencyUnitId").Nullable();
            Map(x => x.NextReviewDate)
               .DbSpecificFormula(_nextReviewDateSqlServerFormula, _nextReviewDateSqlServerFormula.Replace("dbo.dateaddplus", "dateaddplus"));
        }

        #endregion
    }
}
