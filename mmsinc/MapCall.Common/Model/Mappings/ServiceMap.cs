using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceMap : ClassMap<Service>
    {
        #region Constants

        public struct Sql
        {
            public const string STREET_ADDRESS = "COALESCE(StreetNumber, '') + " +
                                                 "COALESCE(' Apt ' + ApartmentNumber, '') + " +
                                                 "COALESCE(' ' + (SELECT Top 1 STTT.FullStName from STreets STTT where STTT.StreetID = StreetID AND STTT.TownID = TownID), '') + " +
                                                 "COALESCE(' ' + (SELECT Top 1 TTT.Town from Towns TTT where TTT.TownID = TownID) + ', ', ', ') + " +
                                                 "COALESCE((SELECT top 1 stttt.Abbreviation from States stttt where stttt.stateID = stateID), '') + " +
                                                 "COALESCE(' ' + Zip, '')",
                                STREET_ADDRESS_SQLITE = "COALESCE(StreetNumber, '') + " +
                                                        "COALESCE(' Apt ' + ApartmentNumber, '') + " +
                                                        "COALESCE(' ' + (SELECT STTT.FullStName from STreets STTT where STTT.StreetID = StreetID AND STTT.TownID = TownID LIMIT 1), '') + " +
                                                        "COALESCE(' ' + (SELECT TTT.Town from Towns TTT where TTT.TownID = TownID LIMIT 1) + ', ', ', ') + " +
                                                        "COALESCE((SELECT stttt.Abbreviation from States stttt where stttt.stateID = stateID LIMIT 1), '') + " +
                                                        "COALESCE(' ' + Zip, '')",
                                HAS_TAP_IMAGES =
                                    "(CASE WHEN (SELECT COUNT(1) FROM TapImages TI WHERE TI.ServiceId = Id) > 0 THEN 1 ELSE 0 END)";
        }

        #endregion

        public ServiceMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            DynamicUpdate();

            References(x => x.BackflowDevice).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.Initiator).Nullable();
            References(x => x.ServiceStatus).Nullable();
            References(x => x.MainSize).Nullable();
            References(x => x.MainType).Nullable();
            References(x => x.MeterSettingSize);
            References(x => x.OfferedAgreement, "ServiceOfferedAgreementTypeId").Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PermitType).Nullable();
            References(x => x.PremiseType).Nullable();
            References(x => x.PreviousServiceMaterial).Nullable();
            References(x => x.PreviousServiceCustomerMaterial).Nullable();
            References(x => x.PreviousServiceSize).Nullable();
            References(x => x.PreviousServiceCustomerSize).Nullable();
            References(x => x.ProjectManager, "ProjectManagerUserId").Nullable();
            References(x => x.RenewalOf).Nullable();

            //Purpose Of Installation
            //HasOne(x => x.SampleSite).Cascade.None().PropertyRef("Service");

            References(x => x.ServiceInstallationPurpose, "PurposeOfInstallationId").Nullable();
            References(x => x.ServiceCategory).Nullable();
            References(x => x.ServiceDwellingType).Nullable();
            References(x => x.ServiceMaterial).Nullable();
            References(x => x.ServicePriority).Nullable();
            References(x => x.ServiceRegroundingPremiseType).Nullable();
            References(x => x.ServiceSize).Nullable();
            References(x => x.State).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.StreetMaterial).Nullable();
            References(x => x.SubfloorCondition, "ServiceSubfloorConditionId").Nullable();
            References(x => x.TerminationPoint, "ServiceTerminationPointId").Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.WorkIssuedTo, "WorkIssuedTo").Nullable();
            References(x => x.CustomerSideMaterial).Nullable();
            References(x => x.CustomerSideSize).Nullable();
            References(x => x.CustomerSideSLReplacement).Nullable();
            References(x => x.FlushingOfCustomerPlumbing).Nullable();
            References(x => x.CustomerSideSLReplacedBy).Nullable();
            References(x => x.CustomerSideSLReplacementContractor).Nullable();
            References(x => x.CustomerSideReplacementWBSNumber).Nullable();
            References(x => x.ServiceSideType).Nullable();
            References(x => x.PremiseUnavailableReason).Nullable();
            References(x => x.Premise).Cascade.None().Nullable();

            References(x => x.ServiceType)
               .Columns("OperatingCenterId", "ServiceCategoryId")
               .LazyLoad()
               .Cascade.None()
               .NotFound.Ignore()
               .Fetch.Join().ReadOnly();
            References(x => x.UpdatedBy).Nullable();

            Map(x => x.Agreement).Nullable();
            Map(x => x.AmountReceived).Nullable();
            Map(x => x.ApartmentNumber).Nullable();
            Map(x => x.ApplicationApprovedOn).Nullable();
            Map(x => x.ApplicationReceivedOn).Nullable();
            Map(x => x.ApplicationSentOn).Nullable();
            Map(x => x.Block).Nullable();
            Map(x => x.BureauOfSafeDrinkingWaterPermitRequired).Nullable();
            Map(x => x.BusinessPartner).Nullable();
            Map(x => x.CleanedCoordinates).Not.Nullable();
            Map(x => x.ContactDate).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.DateClosed).Nullable();
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.DateIssuedToField).Nullable();
            Map(x => x.DepthMainFeet).Nullable();
            Map(x => x.DepthMainInches).Nullable();
            Map(x => x.DeveloperServicesDriven).Nullable();
            Map(x => x.Development).Nullable();
            Map(x => x.Email).Nullable();
            Map(x => x.Fax).Nullable();
            Map(x => x.GeoEFunctionalLocation).Nullable();
            Map(x => x.HasTapImages).Formula(Sql.HAS_TAP_IMAGES);
            Map(x => x.InactiveDate).Nullable();
            Map(x => x.InspectionDate).Nullable();
            Map(x => x.InstallationCost).Nullable();
            Map(x => x.InstallationInvoiceDate).Nullable();
            Map(x => x.InstallationInvoiceNumber).Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.JobNotes).Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.LeadAndCopperCommunicationProvided).Nullable();
            Map(x => x.LeadServiceReplacementWbs).Nullable();
            Map(x => x.LeadServiceRetirementWbs).Nullable();
            Map(x => x.LengthOfService).Nullable();
            Map(x => x.Lot).Nullable();
            Map(x => x.MailPhoneNumber).Nullable();
            Map(x => x.MailState).Nullable();
            Map(x => x.MailStreetName).Nullable();
            Map(x => x.MailStreetNumber).Nullable();
            Map(x => x.MailTown).Nullable();
            Map(x => x.MailZip).Nullable();
            Map(x => x.MeterSettingRequirement).Nullable();
            Map(x => x.Name).Nullable();
            Map(x => x.NSINumber).Nullable();
            Map(x => x.ObjectId).Nullable();
            Map(x => x.OfferedAgreementDate).Nullable();
            Map(x => x.OriginalInstallationDate).Nullable();
            Map(x => x.OtherPoint).Length(int.MaxValue).Nullable(); //ntext field
            Map(x => x.ParentTaskNumber).Nullable();
            Map(x => x.PaymentReferenceNumber).Nullable();
            Map(x => x.PermitExpirationDate).Nullable();
            Map(x => x.PermitNumber).Nullable();
            Map(x => x.PermitReceivedDate).Nullable();
            Map(x => x.PermitSentDate).Nullable();
            Map(x => x.PhoneNumber).Nullable();
            Map(x => x.PremiseNumber).Nullable();
            Map(x => x.Installation).Nullable();
            Map(x => x.PurchaseOrderNumber).Nullable();
            Map(x => x.QuestionaireSentDate).Nullable();
            Map(x => x.QuestionaireReceivedDate).Nullable();
            Map(x => x.RetiredAccountNumber).Nullable();
            Map(x => x.RetiredDate).Nullable();
            Map(x => x.RetireMeterSet).Nullable();
            Map(x => x.RoadOpeningFee).Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.SAPWorkOrderNumber).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
            Map(x => x.ServiceInstallationFee).Nullable();
            Map(x => x.ServiceDwellingTypeQuantity).Nullable();
            Map(x => x.ServiceNumber).Nullable();
            Map(x => x.StreetNumber).Nullable();
            Map(x => x.StreetAddress).DbSpecificFormula(Sql.STREET_ADDRESS, Sql.STREET_ADDRESS_SQLITE);
            Map(x => x.TapOrderNotes).Nullable();
            Map(x => x.TaskNumber1).Nullable();
            Map(x => x.TaskNumber2).Nullable();
            Map(x => x.YearOfHomeConstruction).Nullable();
            Map(x => x.Zip).Nullable();
            Map(x => x.ImageActionID);
            Map(x => x.LengthOfCustomerSideSLReplaced).Nullable();
            Map(x => x.CustomerSideSLReplacementCost).Nullable();
            Map(x => x.CustomerSideReplacementDate).Nullable();
            Map(x => x.DateCreditProcessed).Nullable();
            Map(x => x.PitInstalled).Nullable();
            Map(x => x.CompanyOwned).Nullable();
            Map(x => x.PremiseNumberUnavailable).Nullable();

            Map(x => x.Month).Formula("month(DateInstalled)").ReadOnly();
            Map(x => x.Year).Formula("year(DateInstalled)").ReadOnly();
            Map(x => x.YearRetired).Formula("year(RetiredDate)").ReadOnly();
            Map(x => x.Installed).Formula("(case when (DateInstalled is null) then 0 else 1 end)").ReadOnly();
            Map(x => x.IssuedToField).Formula("(case when (DateIssuedToField is null) then 0 else 1 end)").ReadOnly();
            Map(x => x.Invoiced).Formula("(case when (InstallationInvoiceDate is null) then 0 else 1 end)").ReadOnly();
            Map(x => x.Contacted).Formula("(case when (ContactDate is null) then 0 else 1 end)").ReadOnly();
            Map(x => x.CustomerSideSLWarrantyExpiration)
               .DbSpecificFormula("DATEADD(year, 1, CustomerSideReplacementDate)",
                    "dateaddplus('y', 1, CustomerSideReplacementDate)");

            Map(x => x.PlaceHolderNotes).Nullable();
            Map(x => x.DeviceLocation).Nullable();
            Map(x => x.DeviceLocationUnavailable).Nullable();
            Map(x => x.LegacyId).Length(Service.StringLengths.LEGACY_ID).Nullable();

            Map(x => x.NeedsToSync).Not.Nullable();
            Map(x => x.LastSyncedAt).Nullable();

            HasMany(x => x.ServiceNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ServiceDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TapImages).KeyColumn("ServiceID");
            HasMany(x => x.Restorations).KeyColumn("ServiceId");
            HasMany(x => x.ServiceLineProtectionInvestigations).KeyColumn("ServiceId").LazyLoad().Inverse().Cascade
                                                               .None();
            HasMany(x => x.ServiceInstallationMaterials).KeyColumn("ServiceId").LazyLoad().ReadOnly();
            HasMany(x => x.WorkOrders).KeyColumn("ServiceId");
            HasMany(x => x.PremiseContacts).KeyColumn("ServiceId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Flushes).KeyColumn("ServiceId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
