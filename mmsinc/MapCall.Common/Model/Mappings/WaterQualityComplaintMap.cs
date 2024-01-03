using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterQualityComplaintMap : ClassMap<WaterQualityComplaint>
    {
        #region Constants

        public const string TABLE_NAME = "WaterQualityComplaints";

        #endregion
        
        #region Constructors

        public WaterQualityComplaintMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OrcomOrderType).Column("ORCOMOrderTypeId").Nullable();
            References(x => x.PublicWaterSupply).Column("PWSID").Nullable();
            References(x => x.Type).Column("ComplaintTypeId").Nullable();
            References(x => x.InitialLocalContact).Column("InitialLocalContactId").Nullable();
            References(x => x.InitialLocalResponseType).Nullable();
            References(x => x.State).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.ProblemArea).Nullable();
            References(x => x.Source).Column("ComplaintSourceId").Nullable();
            References(x => x.ProbableCause).Nullable();
            References(x => x.ActionTaken).Nullable();
            References(x => x.CustomerExpectation).Nullable();
            References(x => x.CustomerSatisfaction).Nullable();
            References(x => x.RootCause).Nullable();
            References(x => x.MainSize).Nullable();
            References(x => x.OperatingCenter).Nullable();
            References(x => x.NotificationCompletedBy).Column("NotificationCompletedById").Nullable();

            Map(x => x.Imported).Not.Nullable();
            Map(x => x.OrcomOrderNumber).Column("ORCOMOrderNumber").Nullable().Length(WaterQualityComplaint.StringLengths.ORCOM_ORDER_NUMBER);
            Map(x => x.DateComplaintReceived).Nullable();
            Map(x => x.InitialLocalResponseDate).Nullable();
            Map(x => x.NotificationCreatedBy).Nullable().Length(WaterQualityComplaint.StringLengths.NOTIFICATION_CREATED_BY);
            Map(x => x.NotificationCompletionDate).Nullable();
            Map(x => x.CustomerName).Nullable().Length(WaterQualityComplaint.StringLengths.CUSTOMER_NAME);
            Map(x => x.HomePhoneNumber).Nullable().Length(WaterQualityComplaint.StringLengths.HOME_PHONE_NUMBER);
            Map(x => x.Ext).Nullable().Length(WaterQualityComplaint.StringLengths.EXTENSION);
            Map(x => x.StreetNumber).Nullable().Length(WaterQualityComplaint.StringLengths.STREET_NUMBER);
            Map(x => x.StreetName).Nullable().Length(WaterQualityComplaint.StringLengths.STREET_NAME);
            Map(x => x.ApartmentNumber).Nullable().Length(WaterQualityComplaint.StringLengths.APARTMENT_NUMBER);
            Map(x => x.ZipCode).Nullable().Length(WaterQualityComplaint.StringLengths.ZIP_CODE);
            Map(x => x.PremiseNumber).Nullable().Length(WaterQualityComplaint.StringLengths.PREMISE_NUMBER);
            Map(x => x.ServiceNumber).Nullable().Length(WaterQualityComplaint.StringLengths.SERVICE_NUMBER);
            Map(x => x.AccountNumber).Nullable().Length(WaterQualityComplaint.StringLengths.ACCOUNT_NUMBER);
            Map(x => x.ComplaintDescription).Nullable();
            Map(x => x.ComplaintStartDate).Nullable();
            Map(x => x.SiteVisitRequired).Not.Nullable();
            Map(x => x.SiteVisitBy).Nullable().Length(WaterQualityComplaint.StringLengths.SITE_VISIT_BY);
            Map(x => x.SiteComments).Nullable();
            Map(x => x.WaterFilterOnComplaintSource).Not.Nullable();
            Map(x => x.CrossConnectionDetected).Not.Nullable();
            Map(x => x.MaterialOfService).Column("Material_Of_Service").Nullable().Length(WaterQualityComplaint.StringLengths.MATERIAL_OF_SERVICE);
            Map(x => x.CustomerAnticipatedFollowupDate).Nullable();
            Map(x => x.ActualCustomerFollowupDate).Nullable();
            Map(x => x.CustomerSatisfactionFollowupLetter).Not.Nullable();
            Map(x => x.CustomerSatisfactionFollowupCall).Not.Nullable();
            Map(x => x.CustomerSatisfactionFollowupComments).Nullable();
            Map(x => x.RootCauseIdentified).Not.Nullable();
            Map(x => x.FollowUpPostCardSent).Nullable();

            Map(x => x.IsClosed).Formula("(CASE WHEN NotificationCompletionDate IS NOT NULL THEN 1 ELSE 0 END)");
            Map(x => x.HasCoordinate).Formula("(CASE WHEN CoordinateId IS NOT NULL THEN 1 ELSE 0 END)");

            HasMany(x => x.ComplaintDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.ComplaintNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.SampleResults)
               .KeyColumn("ComplaintId")
               .Cascade.AllDeleteOrphan().Inverse();
        }

        #endregion
    }
}
