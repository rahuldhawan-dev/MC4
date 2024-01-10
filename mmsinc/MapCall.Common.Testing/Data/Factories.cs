using Humanizer;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.ISetExtensions;
using MMSINC.ClassExtensions.Int32Extensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Employee = MapCall.Common.Model.Entities.Employee;
using Site = MapCall.Common.Model.Entities.Site;

namespace MapCall.Common.Testing.Data
{
    // Alphabetical Please

    #region ABCIndicatorFactory

    public class ABCIndicatorFactory : EntityLookupTestDataFactory<ABCIndicator>
    {
        public ABCIndicatorFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AbbreviationType

    public class AbbreviationTypeFactory : UniqueEntityLookupFactory<AbbreviationType>
    {
        public AbbreviationTypeFactory(IContainer container) : base(container) { }
    }

    public class TownAbbreviationTypeFactory : AbbreviationTypeFactory
    {
        static TownAbbreviationTypeFactory()
        {
            Defaults(new {Description = "Town"});
        }

        public TownAbbreviationTypeFactory(IContainer container) : base(container) { }
    }

    public class TownSectionAbbreviationTypeFactory : AbbreviationTypeFactory
    {
        static TownSectionAbbreviationTypeFactory()
        {
            Defaults(new {Description = "Town Section"});
        }

        public TownSectionAbbreviationTypeFactory(IContainer container) : base(container) { }
    }

    public class FireDistrictAbbreviationTypeFactory : AbbreviationTypeFactory
    {
        static FireDistrictAbbreviationTypeFactory()
        {
            Defaults(new {Description = "Fire District"});
        }

        public FireDistrictAbbreviationTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AbsenceNotification

    public class AbsenceNotificationFactory : TestDataFactory<AbsenceNotification>
    {
        #region Constructors

        static AbsenceNotificationFactory()
        {
            Defaults(new {
                SubmittedBy = typeof(UserFactory),
                Employee = typeof(EmployeeFactory),
                CreatedAt = Lambdas.GetNow,
                HumanResourcesReviewed = false,
                EmployeeFMLANotification = typeof(EmployeeFMLANotificationFactory)
            });
        }

        public AbsenceNotificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AccountabilityActionTakenType

    public class AccountabilityActionTakenTypeFactory : UniqueEntityLookupFactory<AccountabilityActionTakenType>
    {
        public AccountabilityActionTakenTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AccountingType

    public class AccountingTypeFactory : StaticListEntityLookupFactory<AccountingType, AccountingTypeFactory>
    {
        #region Constructors

        public AccountingTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CapitalAccountingTypeFactory : AccountingTypeFactory
    {
        #region Constructors

        static CapitalAccountingTypeFactory()
        {
            Defaults(new {
                Description = "Capital"
            });
            OnSaving((a, s) => a.Id = (int)AccountingType.Indices.CAPITAL);
        }

        public CapitalAccountingTypeFactory(IContainer container) : base(container) { }

        #endregion
    }
    
    public class OAndMAccountingTypeFactory : AccountingTypeFactory
    {
        #region Constructors

        static OAndMAccountingTypeFactory()
        {
            Defaults(new {
                Description = "O&M"
            });
            OnSaving((a, s) => a.Id = (int)AccountingType.Indices.O_AND_M);
        }

        public OAndMAccountingTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class RetirementAccountingTypeFactory : AccountingTypeFactory
    {
        #region Constructors

        static RetirementAccountingTypeFactory()
        {
            Defaults(new {
                Description = "Retirement"
            });
            OnSaving((a, s) => a.Id = (int)AccountingType.Indices.RETIREMENT);
        }

        public RetirementAccountingTypeFactory(IContainer container) : base(container) { }

        #endregion
    }
    
    #endregion

    #region AcousticMonitoringType

    public class AcousticMonitoringTypeFactory : StaticListEntityLookupFactory<AcousticMonitoringType, AcousticMonitoringTypeFactory>
    {
        public AcousticMonitoringTypeFactory(IContainer container) : base(container) { }
    }

    public class SmartCoverAcousticMonitoringTypeFactory : AcousticMonitoringTypeFactory
    {
        public SmartCoverAcousticMonitoringTypeFactory(IContainer container) : base(container) { }

        static SmartCoverAcousticMonitoringTypeFactory()
        {
            Defaults(new { Description = "Smart Cover" });
            OnSaving((a, s) => a.Id = (int)AcousticMonitoringType.Indices.SMART_COVER);
        }
    }

    #endregion

    #region ActionFactory

    public class RoleActionFactory : TestDataFactory<RoleAction>
    {
        #region Properties

        protected virtual RoleActions DefaultActionId
        {
            get { return RoleActions.Read; }
        }

        #endregion

        #region Constructors

        public RoleActionFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override RoleAction Save(RoleAction entity)
        {
            var repo = _container.GetInstance<RepositoryBase<RoleAction>>();
            var existing = repo.Find(entity.Id);
            return existing ?? base.Save(entity);
        }

        #endregion

        #region Exposed Methods

        public override RoleAction Build(object overrides = null)
        {
            var act = base.Build(overrides);
            if (act.Id == 0)
            {
                act.Id = (int)DefaultActionId;
            }

            if (string.IsNullOrEmpty(act.Name))
            {
                act.Name = ((RoleActions)act.Id).ToString(); // enum name
            }

            return act;
        }

        #endregion
    }
     
    // TODO: This intermediate factory needs to go away, but refactoring the 150+ places
    // where it's used is going to add to much noise to an already huge pull request. The
    // rename to RoleActionFactory is needed so the magical data thing finds the correct
    // factory. It otherwise tries to make a factory for the wrong RoleAction class(the one used
    // by webforms stuff). -Ross 3/15/2023
    public class ActionFactory : RoleActionFactory
    {
        #region Constructors

        public ActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ReadActionFactory : RoleActionFactory
    {
        #region Properties

        protected override RoleActions DefaultActionId
        {
            get { return RoleActions.Read; }
        }

        #endregion

        #region Constructors

        public ReadActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EditActionFactory : RoleActionFactory
    {
        #region Properties

        protected override RoleActions DefaultActionId
        {
            get { return RoleActions.Edit; }
        }

        #endregion

        #region Constructors

        public EditActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class AddActionFactory : RoleActionFactory
    {
        #region Properties

        protected override RoleActions DefaultActionId
        {
            get { return RoleActions.Add; }
        }

        #endregion

        #region Constructors

        public AddActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class DeleteActionFactory : RoleActionFactory
    {
        #region Properties

        protected override RoleActions DefaultActionId
        {
            get { return RoleActions.Delete; }
        }

        #endregion

        #region Constructors

        public DeleteActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class AdminActionFactory : RoleActionFactory
    {
        #region Properties

        protected override RoleActions DefaultActionId
        {
            get { return RoleActions.UserAdministrator; }
        }

        #endregion

        #region Constructors

        public AdminActionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ActionItemFactory

    public class ActionItemFactory : TestDataFactory<ActionItem>
    {
        static ActionItemFactory()
        {
            Defaults(new {
                Type = typeof(ActionItemTypeFactory),
                NotListedType = "This is my not listed type",
                ResponsibleOwner = typeof(UserFactory),
                Note = "These are the notes for the Action Item",
                TargetedCompletionDate = Lambdas.GetNowDate,
                DateCompleted = Lambdas.GetYesterdayDate,
                CreatedAt = Lambdas.GetNowDate,
                LinkedId = 404,
                CreatedBy = "Some guy",
                DataType = typeof(DataTypeFactory)
            });
        }

        public ActionItemFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ActionItemType

    public class ActionItemTypeFactory : TestDataFactory<ActionItemType>
    {
        static ActionItemTypeFactory()
        {
            Defaults(new {
                Id = 1,
                Description = "This is my description"
            });
        }

        public ActionItemTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AddressFactory

    public class AddressFactory : TestDataFactory<Address>
    {
        public const string DEFAULT_ZIP_CODE = "99999",
                            DEFAULT_ADDRESS_1 = "123 Fake St.";

        public AddressFactory(IContainer container) : base(container) { }

        static AddressFactory()
        {
            Defaults(new {
                ZipCode = DEFAULT_ZIP_CODE,
                Town = typeof(TownFactory),
                Address1 = DEFAULT_ADDRESS_1
            });
        }
    }

    #endregion

    #region AddressFactory

    public class AllocationPermitFactory : TestDataFactory<AllocationPermit>
    {
        public AllocationPermitFactory(IContainer container) : base(container) { }

        static AllocationPermitFactory()
        {
            Defaults(new {
                CreatedAt = Lambdas.GetNow
            });
        }
    }

    #endregion

    #region ApcInspectionItemFactory

    public class ApcInspectionItemFactory : TestDataFactory<ApcInspectionItem>
    {
        public ApcInspectionItemFactory(IContainer container) : base(container) { }

        static ApcInspectionItemFactory()
        {
            int i = 0;
            Func<string> areaFn = () => $"Area {++i}";
            Func<string> descriptionFn = () => $"Item {i}";

            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Facility = typeof(FacilityFactory),
                Area = areaFn,
                Description = descriptionFn,
                Type = typeof(ApcInspectionItemTypeFactory),
                DateReported = DateTime.Now,
                AssignedTo = typeof(ActiveEmployeeFactory)
            });
        }
    }

    public class ApcInspectionItemTypeFactory : EntityLookupTestDataFactory<ApcInspectionItemType>
    {
        public ApcInspectionItemTypeFactory(IContainer container) : base(container) { }
    }

    #region Facility Inspection Area Type

    public class FacilityInspectionAreaTypeFactory : StaticListEntityLookupFactory<FacilityInspectionAreaType, FacilityInspectionAreaTypeFactory>
    {
        #region Constructors

        public FacilityInspectionAreaTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class LabFacilityInspectionAreaTypeFactory : FacilityInspectionAreaTypeFactory
    {
        #region Constructors

        static LabFacilityInspectionAreaTypeFactory()
        {
            Defaults(new { Description = "LAB" });
            OnSaving((a, s) => a.Id = (int)FacilityInspectionAreaType.Indices.LAB);
        }

        public LabFacilityInspectionAreaTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class GroundsFacilityInspectionAreaTypeFactory : FacilityInspectionAreaTypeFactory
    {
        #region Constructors

        static GroundsFacilityInspectionAreaTypeFactory()
        {
            Defaults(new { Description = "GROUNDS" });
            OnSaving((a, s) => a.Id = (int)FacilityInspectionAreaType.Indices.GROUNDS);
        }

        public GroundsFacilityInspectionAreaTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FacilityInspectionFormQuestion

    public class FacilityInspectionFormQuestionCategoriesFactory : StaticListEntityLookupFactory<FacilityInspectionFormQuestionCategory,
        FacilityInspectionFormQuestionCategoriesFactory>
    {
        public FacilityInspectionFormQuestionCategoriesFactory(IContainer container) : base(container) { }
    }

    public class GeneralWorkAreaConditionsFacilityInspectionFormQuestionCategoriesFactory : FacilityInspectionFormQuestionCategoriesFactory
    {
        #region Constants

        public const string DESCRIPTION = "GENERAL WORK AREA/CONDITIONS";

        #endregion

        static GeneralWorkAreaConditionsFacilityInspectionFormQuestionCategoriesFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS);
        }

        public GeneralWorkAreaConditionsFacilityInspectionFormQuestionCategoriesFactory(IContainer container) : base(container) { }
    }

    public class EmergencyResponseFirstAidFacilityInspectionFormQuestionCategoriesFactory : FacilityInspectionFormQuestionCategoriesFactory
    {
        #region Constants

        public const string DESCRIPTION = "EMERGENCY_RESPONSE_FIRST_AID";

        #endregion

        static EmergencyResponseFirstAidFacilityInspectionFormQuestionCategoriesFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID);
        }

        public EmergencyResponseFirstAidFacilityInspectionFormQuestionCategoriesFactory(IContainer container) : base(container) { }
    }

    public class SecurityFacilityInspectionFormQuestionCategoriesFactory : FacilityInspectionFormQuestionCategoriesFactory
    {
        #region Constants

        public const string DESCRIPTION = "SECURITY";

        #endregion

        static SecurityFacilityInspectionFormQuestionCategoriesFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = FacilityInspectionFormQuestionCategory.Indices.SECURITY);
        }

        public SecurityFacilityInspectionFormQuestionCategoriesFactory(IContainer container) : base(container) { }
    }
    public class FireSafetyFacilityInspectionFormQuestionCategoriesFactory : FacilityInspectionFormQuestionCategoriesFactory
    {
        #region Constants

        public const string DESCRIPTION = "FIRE SAFETY";

        #endregion

        static FireSafetyFacilityInspectionFormQuestionCategoriesFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY);
        }

        public FireSafetyFacilityInspectionFormQuestionCategoriesFactory(IContainer container) : base(container) { }
    }

    #endregion

    #endregion

    #region ApplicationFactory

    public class ApplicationFactory : TestDataFactory<Application>
    {
        #region Consts

        private const RoleApplications DEFAULT_ID = RoleApplications.FieldServices;

        #endregion

        #region Constructors

        static ApplicationFactory()
        {
            Defaults(new {
                Id = DEFAULT_ID
            });
        }

        public ApplicationFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override Application Save(Application entity)
        {
            var repo = _container.GetInstance<RepositoryBase<Application>>();
            var existing = repo.Find(entity.Id);
            return existing ?? base.Save(entity);
        }

        #endregion

        #region Exposed Methods

        public override Application Build(object overrides = null)
        {
            // Since Build doesn't pass in the default values, and
            // I don't think it's necessary to create 900 factory classes
            // for each Application, I'm going to use DEFAULT_ID here
            // if there aren't any overrides.

            var app = base.Build(overrides);
            if (app.Id == 0)
            {
                app.Id = (int)DEFAULT_ID;
            }
            if (string.IsNullOrEmpty(app.Name))
            {
                app.Name = ((RoleApplications)app.Id).ToString(); // enum name
            }

            return app;
        }

        #endregion
    }

    #endregion

    #region ArcFlashStatus

    public class ArcFlashStatusFactory : StaticListEntityLookupFactory<ArcFlashStatus, ArcFlashStatusFactory>
    {
        public ArcFlashStatusFactory(IContainer container) : base(container) { }
    }

    public class CompletedArcFlashStatusFactory : ArcFlashStatusFactory
    {
        public const string DESCRIPTION = "Completed";

        static CompletedArcFlashStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.SetPublicPropertyValueByName("Id", ArcFlashStatus.Indices.COMPLETED));
        }

        public CompletedArcFlashStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingArcFlashStatusFactory : ArcFlashStatusFactory
    {
        public const string DESCRIPTION = "Pending";

        static PendingArcFlashStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.SetPublicPropertyValueByName("Id", ArcFlashStatus.Indices.PENDING));
        }

        public PendingArcFlashStatusFactory(IContainer container) : base(container) { }
    }

    public class DeferredArcFlashStatusFactory : ArcFlashStatusFactory
    {
        public const string DESCRIPTION = "Deferred";

        static DeferredArcFlashStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.SetPublicPropertyValueByName("Id", ArcFlashStatus.Indices.DEFFERED));
        }

        public DeferredArcFlashStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ArcFlashStudy

    public class ArcFlashStudyFactory : TestDataFactory<ArcFlashStudy>
    {
        #region Constructors

        static ArcFlashStudyFactory()
        {
            Defaults(new {
                Facility = typeof(FacilityFactory),
            });
        }

        public ArcFlashStudyFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AsBuiltImage

    public class AsBuiltImageFactory : TestDataFactory<AsBuiltImage>
    {
        #region Constructors

        static AsBuiltImageFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                CreatedAt = Lambdas.GetNow,
                Coordinate = typeof(CoordinateFactory),
                CoordinatesModifiedOn = Lambdas.GetNow,
                DateInstalled = Lambdas.GetNow,
                FileName = "some file.tif",
                Directory = "SomeDirectory",
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public AsBuiltImageFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetCategory

    public class AssetCategoryFactory : UniqueEntityLookupFactory<AssetCategory>
    {
        #region Constructors

        public AssetCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetConditionReasons

    public class AssetConditionReasonFactory : UniqueEntityLookupFactory<AssetConditionReason>
    {
        #region Constants

        public const string DEFAULT_CODE = "AFCL",
                            DEFAULT_DESCRIPTION = "Cannot Locate";

        #endregion

        #region Constructors

        static AssetConditionReasonFactory()
        {
            Defaults(new {
                Code = DEFAULT_CODE,
                Description = DEFAULT_DESCRIPTION,
                ConditionDescription = typeof(ConditionDescriptionFactory)
            });
        }

        public AssetConditionReasonFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetInvestmentCategory

    public class AssetInvestmentCategoryFactory : TestDataFactory<AssetInvestmentCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "AssetInvestmentCategory",
                            CREATED_BY = "Factory Test";

        #endregion

        #region Constructors

        static AssetInvestmentCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                CreatedBy = CREATED_BY,
                CreatedAt = Lambdas.GetNow
            });
        }

        public AssetInvestmentCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetReliability

    public class AssetReliabilityFactory : TestDataFactory<AssetReliability>
    {
        static AssetReliabilityFactory()
        {
            Defaults(new {
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                Equipment = typeof(EquipmentFactory),
                Employee = typeof(EmployeeFactory),
                AssetReliabilityTechnologyUsedType = typeof(AssetReliabilityTechnologyUsedTypeFactory),
                DateTimeEntered = Lambdas.GetNow,
                CostAvoidanceNote = "Some note"
            });
        }

        public AssetReliabilityFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AssetReliabilityTechnologyUsedType

    public class AssetReliabilityTechnologyUsedTypeFactory : StaticListEntityLookupFactory<AssetReliabilityTechnologyUsedType, AssetReliabilityTechnologyUsedTypeFactory>
    {
        #region Constructors

        public AssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Infrared Thermography";

        #endregion

        #region Constructors

        static InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.INFRARED_THERMOGRAPHY);
        }

        public InfraredThermographyAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Vibration Analysis";

        #endregion

        #region Constructors

        static VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.VIBRATION_ANALYSIS);
        }

        public VibrationAnalysisAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Motor Winding Analysis/Insulation Resistance";

        #endregion

        #region Constructors

        static MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.MOTOR_WINDING_ANALYSIS_INSULATION_RESISTANCE);
        }

        public MotorWindingAnalysisInsulationResistanceAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class VisualInspectionAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Visual Inspection";

        #endregion

        #region Constructors

        static VisualInspectionAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.VISUAL_INSPECTION);
        }

        public VisualInspectionAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Airborne Ultrasound";

        #endregion

        #region Constructors

        static AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.AIRBORNE_ULTRASOUND);
        }

        public AirborneUltrasoundAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Laser Alignment";

        #endregion

        #region Constructors

        static LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.LASER_ALIGNMENT);
        }

        public LaserAlignmentAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Earth Ground Testing";

        #endregion

        #region Constructors

        static EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.EARTH_GROUND_TESTING);
        }

        public EarthGroundTestingAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Electrical Testing";

        #endregion

        #region Constructors

        static ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.ELECTRICAL_TESTING);
        }

        public ElectricalTestingAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Wire to Water/Pump Performance";

        #endregion

        #region Constructors

        static WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.WIRE_TO_WATER_PUMP_PERFORMANCE);
        }

        public WireToWaterPumpPerformanceAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Motion Amplification";

        #endregion

        #region Constructors

        static MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.MOTION_AMPLIFICATION);
        }

        public MotionAmplificationAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Protective Relay Testing";

        #endregion

        #region Constructors

        static ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.PROTECTIVE_RELAY_TESTING);
        }

        public ProtectiveRelayTestingAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OtherAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Other";

        #endregion

        #region Constructors

        static OtherAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.OTHER);
        }

        public OtherAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class BatteryTestingAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Battery Testing";

        #endregion

        #region Constructors

        static BatteryTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.BATTERY_TESTING);
        }

        public BatteryTestingAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Dynamic Motor Testing/ESA";

        #endregion

        #region Constructors

        static DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.DYNAMIC_MOTOR_TESTING_ESA);
        }

        public DynamicMotorTestingESAAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory : AssetReliabilityTechnologyUsedTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Micro-Ohmmeter Testing";

        #endregion

        #region Constructors

        static MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = AssetReliabilityTechnologyUsedType.Indices.MICRO_OHMMETER_TESTING);
        }

        public MicroOhmmeterTestingAssetReliabilityTechnologyUsedTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetStatus

    public class AssetStatusFactory : UniqueEntityLookupFactory<AssetStatus>
    {
        public AssetStatusFactory(IContainer container) : base(container) { }

        protected override AssetStatus Save(AssetStatus entity)
        {
            var repository = _container.GetInstance<RepositoryBase<AssetStatus>>();
            var ret = repository.Find(entity.Id);
            if (ret == null)
            {
                ret = base.Save(entity);
                Session.Flush();
            }

            return ret;
        }

        public IList<AssetStatus> CreateAll()
        {
            var type = GetType();

            return
                type.Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(type))
                    .Select(sub => ((AssetStatusFactory)_container.GetInstance(sub)).Create())
                    .ToList();
        }
    }

    public class ActiveAssetStatusFactory : AssetStatusFactory
    {
        static ActiveAssetStatusFactory()
        {
            Defaults(new {
                Description = "ACTIVE",
                IsUserAdminOnly = true,
            });

            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.ACTIVE);
        }

        public ActiveAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingMergerAssetStatusFactory : PublicWaterSupplyStatusFactory
    {
        static PendingMergerAssetStatusFactory()
        {
            Defaults(new {
                Description = "Pending Merger",
            });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.PENDING_MERGER);
        }

        public PendingMergerAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class CancelledAssetStatusFactory : AssetStatusFactory
    {
        static CancelledAssetStatusFactory()
        {
            Defaults(new {
                Description = "CANCELLED",
                IsUserAdminOnly = true,
            });

            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.CANCELLED);
        }

        public CancelledAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingAssetStatusFactory : AssetStatusFactory
    {
        static PendingAssetStatusFactory()
        {
            Defaults(new {
                Description = "PENDING",
                IsUserAdminOnly = true,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.PENDING);
        }

        public PendingAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class NsiPendingAssetStatusFactory : AssetStatusFactory
    {
        static NsiPendingAssetStatusFactory()
        {
            Defaults(new {
                Description = "NSI_PENDING",
                IsUserAdminOnly = true,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.NSI_PENDING);
        }

        public NsiPendingAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class InactiveAssetStatusFactory : AssetStatusFactory
    {
        static InactiveAssetStatusFactory()
        {
            Defaults(new {
                Description = "INACTIVE",
                IsUserAdminOnly = true,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.INACTIVE);
        }

        public InactiveAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class InstalledAssetStatusFactory : AssetStatusFactory
    {
        static InstalledAssetStatusFactory()
        {
            Defaults(new {
                Description = "INSTALLED",
                IsUserAdminOnly = false,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.INSTALLED);
        }

        public InstalledAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class RemovedAssetStatusFactory : AssetStatusFactory
    {
        static RemovedAssetStatusFactory()
        {
            Defaults(new {
                Description = "REMOVED",
                IsUserAdminOnly = true,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.REMOVED);
        }

        public RemovedAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class RetiredAssetStatusFactory : AssetStatusFactory
    {
        static RetiredAssetStatusFactory()
        {
            Defaults(new {
                Description = "RETIRED",
                IsUserAdminOnly = true,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.RETIRED);
        }

        public RetiredAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class RequestCancellationAssetStatusFactory : AssetStatusFactory
    {
        static RequestCancellationAssetStatusFactory()
        {
            Defaults(new {
                Description = "REQUEST CANCELLATION",
                IsUserAdminOnly = false,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.REQUEST_CANCELLATION);
        }

        public RequestCancellationAssetStatusFactory(IContainer container) : base(container) { }
    }

    public class RequestRetirementAssetStatusFactory : AssetStatusFactory
    {
        static RequestRetirementAssetStatusFactory()
        {
            Defaults(new {
                Description = "REQUEST RETIREMENT",
                IsUserAdminOnly = false,
            });
            OnSaving((a, s) => a.Id = (int)AssetStatus.Indices.REQUEST_RETIREMENT);
        }

        public RequestRetirementAssetStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AssetType

    public class AssetTypeFactory : StaticListEntityLookupFactory<AssetType, AssetTypeFactory>
    {
        #region Constructors

        public AssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EquipmentAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Equipment";

        #endregion

        #region Constructors

        static EquipmentAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.EQUIPMENT);
        }

        public EquipmentAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class FacilityAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Facility";

        #endregion

        #region Constructors

        static FacilityAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.FACILITY);
        }

        public FacilityAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class HydrantAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Hydrant";

        #endregion

        #region Constructors

        static HydrantAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.HYDRANT);
        }

        public HydrantAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MainAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Main";

        #endregion

        #region Constructors

        static MainAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.MAIN);
        }

        public MainAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MainCrossingAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Main Crossing";

        #endregion

        #region Constructors

        static MainCrossingAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.MAIN_CROSSING);
        }

        public MainCrossingAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CrossingTypeFactory : UniqueEntityLookupFactory<CrossingType>
    {
        public CrossingTypeFactory(IContainer container) : base(container) { }
    }

    public class SupportStructureFactory : UniqueEntityLookupFactory<SupportStructure>
    {
        public SupportStructureFactory(IContainer container) : base(container) { }
    }

    public class ServiceAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Service";

        #endregion

        #region Constructors

        static ServiceAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.SERVICE);
        }

        public ServiceAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SewerOpeningAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Sewer Opening";

        #endregion

        #region Constructors

        static SewerOpeningAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.SEWER_OPENING);
        }

        public SewerOpeningAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SewerLateralAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SewerLateral";

        #endregion

        #region Constructors

        static SewerLateralAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.SEWER_LATERAL);
        }

        public SewerLateralAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SewerMainAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Sewer Main";

        #endregion

        #region Constructors

        static SewerMainAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.SEWER_MAIN);
        }

        public SewerMainAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class StormCatchAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "StormCatch";

        #endregion

        #region Constructors

        static StormCatchAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.STORM_CATCH);
        }

        public StormCatchAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ValveAssetTypeFactory : AssetTypeFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Valve";

        #endregion

        #region Constructors

        static ValveAssetTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)AssetType.Indices.VALVE);
        }

        public ValveAssetTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AssetUpload

    public class AssetUploadFactory : TestDataFactory<AssetUpload>
    {
        static AssetUploadFactory()
        {
            Defaults(new {
                CreatedAt = Lambdas.GetNow,
                CreatedBy = typeof(UserFactory),
                Status = typeof(PendingAssetUploadStatusFactory),
                FileName = "foo",
                FileGuid = Guid.NewGuid()
            });
        }

        public AssetUploadFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AssetUploadStatus

    public class AssetUploadStatusFactory : StaticListEntityLookupFactory<AssetUploadStatus, AssetUploadStatusFactory>
    {
        public AssetUploadStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingAssetUploadStatusFactory : AssetUploadStatusFactory
    {
        static PendingAssetUploadStatusFactory()
        {
            Defaults(new {Description = "Pending"});
            OnSaving((a, s) => a.Id = (int)AssetUploadStatus.Indices.PENDING);
        }

        public PendingAssetUploadStatusFactory(IContainer container) : base(container) { }
    }

    public class SuccessAssetUploadStatusFactory : AssetUploadStatusFactory
    {
        static SuccessAssetUploadStatusFactory()
        {
            Defaults(new {Description = "Success"});
            OnSaving((a, s) => a.Id = (int)AssetUploadStatus.Indices.SUCCESS);
        }

        public SuccessAssetUploadStatusFactory(IContainer container) : base(container) { }
    }

    public class ErrorAssetUploadStatusFactory : AssetUploadStatusFactory
    {
        static ErrorAssetUploadStatusFactory()
        {
            Defaults(new {Description = "Error"});
            OnSaving((a, s) => a.Id = (int)AssetUploadStatus.Indices.ERROR);
        }

        public ErrorAssetUploadStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AtRiskBehaviorSection

    public class AtRiskBehaviorSectionFactory : UniqueEntityLookupFactory<AtRiskBehaviorSection>
    {
        public AtRiskBehaviorSectionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AtRiskBehaviorSubSection

    public class AtRiskBehaviorSubSectionFactory : UniqueEntityLookupFactory<AtRiskBehaviorSubSection>
    {
        static AtRiskBehaviorSubSectionFactory()
        {
            Defaults(new {
                Section = typeof(AtRiskBehaviorSectionFactory)
            });
        }

        public AtRiskBehaviorSubSectionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region AuditLogEntry

    public class AuditLogEntryFactory : TestDataFactory<AuditLogEntry>
    {
        #region Constants

        public const string DEFAULT_AUDIT_ENTRY_TYPE = "Show",
                            DEFAULT_ENTITY_NAME = "Hydrant";

        public const int DEFAULT_ENTITY_ID = 1;

        #endregion

        #region Constructors

        static AuditLogEntryFactory()
        {
            Defaults(new {
                User = typeof(UserFactory),
                AuditEntryType = DEFAULT_AUDIT_ENTRY_TYPE,
                EntityName = DEFAULT_ENTITY_NAME,
                EntityId = 1,
                Timestamp = Lambdas.GetNow()
            });
        }

        public AuditLogEntryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AuthenticationLog

    public class AuthenticationLogFactory : TestDataFactory<AuthenticationLog>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "AssetInvestmentCategory",
                            CREATED_BY = "Factory Test";

        #endregion

        #region Constructors

        static AuthenticationLogFactory()
        {
            Func<Guid> getHash = Guid.NewGuid;
            Defaults(new {
                IpAddress = "0.0.0.0",
                LoggedInAt = Lambdas.GetNow,
                User = typeof(UserFactory),
                ExpiresAt = Lambdas.GetNow,
                AuthCookieHash = getHash
            });
        }

        public AuthenticationLogFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AwiaComplianceCertificationType

    public class AwiaComplianceCertificationTypeFactory : UniqueEntityLookupFactory<AwiaComplianceCertificationType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Emergency Response Plan";

        #endregion

        #region Constructors

        static AwiaComplianceCertificationTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public AwiaComplianceCertificationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region AwiaCompliance

    public class AwiaComplianceFactory : TestDataFactory<AwiaCompliance>
    {
        #region Constructors

        static AwiaComplianceFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                CertificationType = typeof(AwiaComplianceCertificationTypeFactory),
                CreatedBy = typeof(UserFactory),
                CertifiedBy = typeof(UserFactory),                  
                DateAccepted = DateTime.Now,
                DateSubmitted = DateTime.Now,
                RecertificationDue = DateTime.Now
            });
        }

        public AwiaComplianceFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region BacterialSampleType

    public class
        BacterialSampleTypeFactory : StaticListEntityLookupFactory<BacterialSampleType, BacterialSampleTypeFactory>
    {
        public BacterialSampleTypeFactory(IContainer container) : base(container) { }
    }

    public class ConfirmationBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Confirmation";

        public ConfirmationBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static ConfirmationBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.CONFIRMATION);
        }
    }

    public class NewMainBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "New Main";

        public NewMainBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static NewMainBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.NEW_MAIN);
        }
    }

    public class ProcessControlBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Process Control";

        public ProcessControlBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static ProcessControlBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.PROCESS_CONTROL);
        }
    }

    public class RepeatBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Repeat";

        public RepeatBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static RepeatBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.REPEAT);
        }
    }

    public class RoutineBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Routine";

        public RoutineBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static RoutineBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.ROUTINE);
        }
    }

    public class ShippingBlankBacterialSampleTypeFactory : BacterialSampleTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Shipping Blank";

        public ShippingBlankBacterialSampleTypeFactory(IContainer container) : base(container) { }

        static ShippingBlankBacterialSampleTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)BacterialSampleType.Indices.SHIPPING_BLANK);
        }
    }

    #endregion

    #region BacterialWaterSample

    public class BacterialWaterSampleFactory : TestDataFactory<BacterialWaterSample>
    {
        #region Constants

        public const decimal DEFAULT_CL2_FREE = 0.05m, DEFAULT_CL2_TOTAL = 0.3m;

        #endregion

        #region Constructors

        static BacterialWaterSampleFactory()
        {
            Defaults(new {
                Cl2Free = DEFAULT_CL2_FREE,
                Cl2Total = DEFAULT_CL2_TOTAL,
                SampleCollectionDTM = Lambdas.GetNow,
                BacterialSampleType = typeof(RoutineBacterialSampleTypeFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                SampleSite = typeof(SampleSiteFactory),
                CollectedBy = typeof(UserFactory),
                LIMSStatus = typeof(NotReadyLIMSStatusFactory)
            });
        }

        public BacterialWaterSampleFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region BacterialWaterSample

    public class BacterialWaterSampleAnalystFactory : TestDataFactory<BacterialWaterSampleAnalyst>
    {
        #region Constants

        public const decimal DEFAULT_CL2_FREE = 0.05m, DEFAULT_CL2_TOTAL = 0.3m;

        #endregion

        #region Constructors

        static BacterialWaterSampleAnalystFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                IsActive = true
            });
        }

        public BacterialWaterSampleAnalystFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region BappTeamIdea

    public class BappTeamFactory : TestDataFactory<BappTeam>
    {
        public BappTeamFactory(IContainer container) : base(container) { }

        static BappTeamFactory()
        {
            int i = 1;
            Func<string> descriptionFn = () => String.Format("BAPP Team {0}", i++);

            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Description = descriptionFn,
                CreatedBy = typeof(UserFactory)
            });
        }
    }

    public class SafetyImplementationCategoryFactory : EntityLookupTestDataFactory<SafetyImplementationCategory>
    {
        public SafetyImplementationCategoryFactory(IContainer container) : base(container) { }
    }

    public class BappTeamIdeaFactory : TestDataFactory<BappTeamIdea>
    {
        static BappTeamIdeaFactory()
        {
            int i = 1;
            Func<string> descriptionFn = () => String.Format("Team Idea {0}", i++);

            Defaults(new {
                Contact = typeof(EmployeeFactory),
                SafetyImplementationCategory = typeof(SafetyImplementationCategoryFactory),
                BappTeam = typeof(BappTeamFactory),
                Description = descriptionFn,
                CreatedAt = Lambdas.GetNow
            });
        }

        public BappTeamIdeaFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region BelowGroundHazard

    public class BelowGroundHazardFactory : TestDataFactory<BelowGroundHazard>
    {
        #region Constructors

        static BelowGroundHazardFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                TownSection = typeof(TownSectionFactory),
                HazardDescription = "Description",
                HazardArea = 25,
                HazardType = typeof(HazardTypeFactory),
                Street = typeof(StreetFactory),
                CrossStreet = typeof(StreetFactory),
                Coordinate = typeof(CoordinateFactory),
                AssetStatus = typeof(ActiveAssetStatusFactory)
            });
        }

        public BelowGroundHazardFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion
    
    #region BillingParty

    public class BillingPartyFactory : TestDataFactory<BillingParty>
    {
        public BillingPartyFactory(IContainer container) : base(container) { }

        static BillingPartyFactory()
        {
            var i = 0;
            Func<string> descriptionFn = () => String.Format("Billing Party {0}", ++i);

            Defaults(new {
                Description = descriptionFn
            });
        }
    }

    #endregion

    #region BillingPartyContactType

    public class BillingPartyContactTypeFactory : TestDataFactory<BillingPartyContactType>
    {
        public BillingPartyContactTypeFactory(IContainer container) : base(container) { }

        static BillingPartyContactTypeFactory()
        {
            Defaults(new {
                ContactType = typeof(ContactTypeFactory)
            });
        }
    }

    #endregion

    #region BlockConditionFactory

    public class BlockConditionFactory : UniqueEntityLookupFactory<BlockCondition>
    {
        public BlockConditionFactory(IContainer container) : base(container) { }
    }

    public class InBlockConditionFactory
        : BlockConditionFactory
    {
        public InBlockConditionFactory(IContainer container) : base(container) { }

        static InBlockConditionFactory()
        {
            Defaults(new {
                Description = "IN"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.IN);
        }
    }

    public class OutBlockConditionFactory
        : BlockConditionFactory
    {
        public OutBlockConditionFactory(IContainer container) : base(container) { }

        static OutBlockConditionFactory()
        {
            Defaults(new {
                Description = "OUT"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.OUT);
        }
    }

    public class StuckBlockConditionFactory
        : BlockConditionFactory
    {
        public StuckBlockConditionFactory(IContainer container) : base(container) { }

        static StuckBlockConditionFactory()
        {
            Defaults(new {
                Description = "STUCK"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.STUCK);
        }
    }

    public class FloatingBlockConditionFactory
        : BlockConditionFactory
    {
        public FloatingBlockConditionFactory(IContainer container) : base(container) { }

        static FloatingBlockConditionFactory()
        {
            Defaults(new {
                Description = "FLOATING"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.FLOATING);
        }
    }

    public class MissingBlockConditionFactory
        : BlockConditionFactory
    {
        public MissingBlockConditionFactory(IContainer container) : base(container) { }

        static MissingBlockConditionFactory()
        {
            Defaults(new {
                Description = "MISSING"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.MISSING);
        }
    }

    public class InSewerMainBlockConditionFactory
        : BlockConditionFactory
    {
        public InSewerMainBlockConditionFactory(IContainer container) : base(container) { }

        static InSewerMainBlockConditionFactory()
        {
            Defaults(new {
                Description = "IN SEWER MAIN"
            });
            OnSaving((x, _) => x.Id = BlockCondition.Indices.IN_SEWER_MAIN);
        }
    }

    #endregion

    #region BlowOffInspection

    public class BlowOffInspectionFactory : TestDataFactory<BlowOffInspection>
    {
        #region Constructors

        static BlowOffInspectionFactory()
        {
            Defaults(new {
                Valve = typeof(BlowOffValveFactory),
                DateInspected = Lambdas.GetNow,
                InspectedBy = typeof(UserFactory),
                //HydrantInspectionType = typeof(HydrantInspectionTypeFactory),
                CreatedAt = Lambdas.GetNow,
                FreeNoReadReason = typeof(NoReadReasonFactory),
                TotalNoReadReason = typeof(NoReadReasonFactory)
            });
        }

        public BlowOffInspectionFactory(IContainer container) : base(container) { }

        #endregion

        public override BlowOffInspection Create(object overrides = null)
        {
            var boi = base.Create(overrides);

            if (boi.Valve != null && !boi.Valve.BlowOffInspections.Contains(boi))
            {
                boi.Valve.BlowOffInspections.Add(boi);
            }

            return boi;
        }
    }

    #endregion

    #region BoardFactory

    public class BoardFactory : TestDataFactory<Board>
    {
        static BoardFactory()
        {
            Defaults(new {
                Name = "Board",
                Site = typeof(SiteFactory)
            });
        }

        public BoardFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region BodyOfWaterFactory

    public class BodyOfWaterFactory : TestDataFactory<BodyOfWater>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Body of Water";
        public const string DEFAULT_NAME = "Blue Water";

        #endregion

        #region Constructors

        static BodyOfWaterFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Name = DEFAULT_NAME,
                Description = DEFAULT_DESCRIPTION
            });
        }

        public BodyOfWaterFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Bond

    public class BondFactory : TestDataFactory<Bond>
    {
        #region Constants

        #endregion

        #region Constructors

        static BondFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                County = typeof(CountyFactory)
            });
        }

        public BondFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region BondPurpose

    public class BondPurposeFactory : StaticListEntityLookupFactory<BondPurpose, BondPurposeFactory>
    {
        #region Constructors

        public BondPurposeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class RoadOpeningPermitBondPurposeFactory : BondPurposeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Road Opening Permit";

        #region Constructors

        static RoadOpeningPermitBondPurposeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)BondPurpose.Indices.ROAD_OPENING_PERMIT);
        }

        public RoadOpeningPermitBondPurposeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PerformanceBondPurposeFactory : BondPurposeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Performance";

        #region Constructors

        static PerformanceBondPurposeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)BondPurpose.Indices.PERFORMANCE_BOND);
        }

        public PerformanceBondPurposeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region BusinessUnitFactory

    public class BusinessUnitFactory : TestDataFactory<BusinessUnit>
    {
        static BusinessUnitFactory()
        {
            var i = 0;
            Func<String> buFn = () => String.Format("BU {0}", ++i);
            Defaults(new {
                BU = buFn,
                OperatingCenter = typeof(OperatingCenterFactory),
            });
        }

        public BusinessUnitFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CompanyLaborCost

    public class CompanyLaborCostFactory : TestDataFactory<CompanyLaborCost>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "CompanyLaborCost",
                            DEFAULT_UNIT = "Ft";

        public const decimal DEFAULT_COST = 10.50m;

        #endregion

        #region Constructors

        static CompanyLaborCostFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Cost = DEFAULT_COST,
                Unit = DEFAULT_UNIT
            });
        }

        public CompanyLaborCostFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ClaimsRepresentative

    public class ClaimsRepresentativeFactory : TestDataFactory<ClaimsRepresentative>
    {
        public const string DESCRIPTION = "Gladys";
        protected static int _count = 1;

        static ClaimsRepresentativeFactory()
        {
            Func<string> description = () => {
                _count++;
                return DESCRIPTION + " #" + _count;
            };

            Defaults(new {
                Description = description
            });
        }

        public ClaimsRepresentativeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ClassLocation

    public class ClassLocationFactory : TestDataFactory<ClassLocation>
    {
        #region Constants

        public const string DEFAULT_NAME = "ClassLocation";

        #endregion

        #region Constructors

        static ClassLocationFactory()
        {
            Defaults(new {
                Name = DEFAULT_NAME,
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public ClassLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Chemical

    public class ChemicalFactory : TestDataFactory<Chemical>
    {
        static ChemicalFactory()
        {
            int name = 1;
            int partNumber = 1;
            Func<string> nameFn = () => "Chemical " + (name++).ToString();
            Func<string> partNumberFn = () => (partNumber++).ToString();
            Defaults(new {
                PartNumber = partNumberFn,
                Name = nameFn
            });
        }

        public ChemicalFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalDelivery

    public class ChemicalDeliveryFactory : TestDataFactory<ChemicalDelivery>
    {
        static ChemicalDeliveryFactory()
        {
            Defaults(new {
                Storage = typeof(ChemicalStorageFactory),
                Chemical = typeof(ChemicalFactory)
            });
        }

        public ChemicalDeliveryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalInventoryTransaction

    public class ChemicalInventoryTransactionFactory : TestDataFactory<ChemicalInventoryTransaction>
    {
        static ChemicalInventoryTransactionFactory()
        {
            Defaults(new {
                Storage = typeof(ChemicalStorageFactory)
            });
        }

        public ChemicalInventoryTransactionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalStorage

    public class ChemicalStorageFactory : TestDataFactory<ChemicalStorage>
    {
        static ChemicalStorageFactory()
        {
            Defaults(new {
                Chemical = typeof(ChemicalFactory),
                Facility = typeof(FacilityFactory),
                WarehouseNumber = typeof(ChemicalWarehouseNumberFactory)
            });
        }

        public ChemicalStorageFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalStorageLocation

    public class ChemicalStorageLocationFactory : TestDataFactory<ChemicalStorageLocation>
    {
        static ChemicalStorageLocationFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                PlanningPlant = typeof(PlanningPlantFactory),
                ChemicalWarehouseNumber = typeof(ChemicalWarehouseNumberFactory),
                StorageLocationNumber = "2016",
                StorageLocationDescription = "Cubs Win",
                CreatedBy = typeof(UserFactory),
                CreatedAt = Lambdas.GetNow,
                UpdatedBy = typeof(UserFactory),
                UpdatedAt = Lambdas.GetNow
            });
        }

        public ChemicalStorageLocationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalUnitCost

    public class ChemicalUnitCostFactory : TestDataFactory<ChemicalUnitCost>
    {
        static ChemicalUnitCostFactory()
        {
            Defaults(new {
                Chemical = typeof(ChemicalFactory)
            });
        }

        public ChemicalUnitCostFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalVendor

    public class ChemicalVendorFactory : TestDataFactory<ChemicalVendor>
    {
        static ChemicalVendorFactory()
        {
            var vendor = 1;
            Func<string> vendorFn = () => $"Vendor {vendor++}";
            Defaults(new {
                Vendor = vendorFn
            });
        }

        public ChemicalVendorFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ChemicalWarehouseNumber

    public class ChemicalWarehouseNumberFactory : TestDataFactory<ChemicalWarehouseNumber>
    {
        static ChemicalWarehouseNumberFactory()
        {
            int warehouseNumber = 1;
            Func<string> warehouseNumberFn = () => "Warehouse " + (warehouseNumber++).ToString();
            Defaults(new {
                WarehouseNumber = warehouseNumberFn,
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public ChemicalWarehouseNumberFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CommunityRightToKnow

    public class CommunityRightToKnowFactory : TestDataFactory<CommunityRightToKnow>
    {
        #region Constructors

        static CommunityRightToKnowFactory()
        {
            Defaults(new {
                Facility = typeof(FacilityFactory)
            });
        }

        public CommunityRightToKnowFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ConfinedSpaceForm

    public class ConfinedSpaceFormFactory : TestDataFactory<ConfinedSpaceForm>
    {
        static ConfinedSpaceFormFactory()
        {
            Defaults(new {
                BumpTestConfirmedBy =
                    typeof(EmployeeFactory), // needed to add atmospheric tests. This is the more common scenario.
                BumpTestConfirmedAt = Lambdas.GetNow,
                CreatedBy = typeof(UserFactory),
                GeneralDateTime = Lambdas.GetNow,
                LocationAndDescriptionOfConfinedSpace = "Some location",
                OperatingCenter = typeof(OperatingCenterFactory),
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                PurposeOfEntry = "Some purpose of entry"
            });
        }

        public ConfinedSpaceFormFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CompletedConfinedSpaceForm

    public class CompletedConfinedSpaceFormFactory : ConfinedSpaceFormFactory
    {
        static CompletedConfinedSpaceFormFactory()
        {
            Defaults(new {
                ReclassificationSignedBy = typeof(EmployeeFactory),
                CanBeControlledByVentilationAlone = false,
                HazardSignedBy = typeof(EmployeeFactory),
            });
        }

        public CompletedConfinedSpaceFormFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ConfinedSpaceFormAtmosphericTest

    public class ConfinedSpaceFormAtmosphericTestFactory : TestDataFactory<ConfinedSpaceFormAtmosphericTest>
    {
        static ConfinedSpaceFormAtmosphericTestFactory()
        {
            Defaults(new {
                ConfinedSpaceForm = typeof(ConfinedSpaceFormFactory),
                ConfinedSpaceFormReadingCaptureTime = typeof(ConfinedSpaceFormReadingCaptureTimeFactory),
                TestedBy = typeof(EmployeeFactory),
                TestedAt = DateTime.Now,
                OxygenPercentageBottom = 20m,
                OxygenPercentageMiddle = 20m,
                OxygenPercentageTop = 20m,
            });
        }

        public ConfinedSpaceFormAtmosphericTestFactory(IContainer container) : base(container) { }

        public override ConfinedSpaceFormAtmosphericTest Create(object overrides = null)
        {
            var entity = base.Create(overrides);
            entity.ConfinedSpaceForm.AtmosphericTests.AddIfMissing(entity);
            return entity;
        }
    }

    #endregion

    #region ConfinedSpaceFormEntrant

    public class ConfinedSpaceFormEntrantFactory : TestDataFactory<ConfinedSpaceFormEntrant>
    {
        static ConfinedSpaceFormEntrantFactory()
        {
            Defaults(new {
                ConfinedSpaceForm = typeof(ConfinedSpaceFormFactory),
                EntrantType = typeof(ConfinedSpaceFormEntrantTypeFactory),
                Employee = typeof(EmployeeFactory)
            });
        }

        public ConfinedSpaceFormEntrantFactory(IContainer container) : base(container) { }

        public override ConfinedSpaceFormEntrant Create(object overrides = null)
        {
            var csfe = base.Create(overrides);
            csfe.ConfinedSpaceForm.Entrants.AddIfMissing(csfe);
            return csfe;
        }
    }

    #endregion

    #region ConfinedSpaceFormHazard

    public class ConfinedSpaceFormHazardFactory : TestDataFactory<ConfinedSpaceFormHazard>
    {
        static ConfinedSpaceFormHazardFactory()
        {
            Defaults(new {
                ConfinedSpaceForm = typeof(ConfinedSpaceFormFactory),
                Notes = "Some hazard notes.",
                HazardType = typeof(ConfinedSpaceFormHazardTypeFactory),
            });
        }

        public ConfinedSpaceFormHazardFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ConfinedSpaceFormEntrantTypeFactory

    public class ConfinedSpaceFormEntrantTypeFactory : StaticListEntityLookupFactory<ConfinedSpaceFormEntrantType,
        ConfinedSpaceFormEntrantTypeFactory>
    {
        public ConfinedSpaceFormEntrantTypeFactory(IContainer container) : base(container) { }
    }

    public class EntrantConfinedSpaceFormEntrantTypeFactory : ConfinedSpaceFormEntrantTypeFactory
    {
        public EntrantConfinedSpaceFormEntrantTypeFactory(IContainer container) : base(container) { }

        static EntrantConfinedSpaceFormEntrantTypeFactory()
        {
            Defaults(new {
                Description = "Entrant"
            });
            OnSaving((a, s) =>
                a.Id = ConfinedSpaceFormEntrantType.Indices.ENTRANT
            );
        }
    }

    public class AttendantConfinedSpaceFormEntrantTypeFactory : ConfinedSpaceFormEntrantTypeFactory
    {
        public AttendantConfinedSpaceFormEntrantTypeFactory(IContainer container) : base(container) { }

        static AttendantConfinedSpaceFormEntrantTypeFactory()
        {
            Defaults(new {
                Description = "Attendant"
            });
            OnSaving((a, s) =>
                a.Id = ConfinedSpaceFormEntrantType.Indices.ATTENDANT
            );
        }
    }

    public class EntrySupervisorConfinedSpaceFormEntrantTypeFactory : ConfinedSpaceFormEntrantTypeFactory
    {
        public EntrySupervisorConfinedSpaceFormEntrantTypeFactory(IContainer container) : base(container) { }

        static EntrySupervisorConfinedSpaceFormEntrantTypeFactory()
        {
            Defaults(new {
                Description = "Entry Supervisor"
            });
            OnSaving((a, s) =>
                a.Id = ConfinedSpaceFormEntrantType.Indices.ENTRY_SUPERVISOR
            );
        }
    }

    #endregion

    #region ConfinedSpaceFormHazardType

    public class ConfinedSpaceFormHazardTypeFactory : TestDataFactory<ConfinedSpaceFormHazardType>
    {
        static ConfinedSpaceFormHazardTypeFactory()
        {
            var counter = 0;
            Func<string> descFn = () => $"Some hazard type #{counter++}";
            Defaults(new {
                Description = descFn
            });
        }

        public ConfinedSpaceFormHazardTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ConfinedSpaceFormMethodOfCommunication

    public class ConfinedSpaceFormMethodOfCommunicationFactory : StaticListEntityLookupFactory<
        ConfinedSpaceFormMethodOfCommunication, ConfinedSpaceFormMethodOfCommunicationFactory>
    {
        public ConfinedSpaceFormMethodOfCommunicationFactory(IContainer container) : base(container) { }
    }

    public class OtherMethodOfCommunicationFactory : ConfinedSpaceFormMethodOfCommunicationFactory
    {
        public OtherMethodOfCommunicationFactory(IContainer container) : base(container) { }

        static OtherMethodOfCommunicationFactory()
        {
            Defaults(new {Description = "Other"});
            OnSaving((a, s) => a.Id = ConfinedSpaceFormMethodOfCommunication.Indices.OTHER);
        }
    }

    public class RadioMethodOfCommunicationFactory : ConfinedSpaceFormMethodOfCommunicationFactory
    {
        public RadioMethodOfCommunicationFactory(IContainer container) : base(container) { }

        static RadioMethodOfCommunicationFactory()
        {
            Defaults(new {Description = "Radio"});
            OnSaving((a, s) => a.Id = ConfinedSpaceFormMethodOfCommunication.Indices.RADIO);
        }
    }

    public class VoiceMethodOfCommunicationFactory : ConfinedSpaceFormMethodOfCommunicationFactory
    {
        public VoiceMethodOfCommunicationFactory(IContainer container) : base(container) { }

        static VoiceMethodOfCommunicationFactory()
        {
            Defaults(new {Description = "Voice"});
            OnSaving((a, s) => a.Id = ConfinedSpaceFormMethodOfCommunication.Indices.VOICE);
        }
    }

    #endregion

    #region ConfinedSpaceFormReadingCaptureTime

    public class
        ConfinedSpaceFormReadingCaptureTimeFactory : UniqueEntityLookupFactory<ConfinedSpaceFormReadingCaptureTime>
    {
        public ConfinedSpaceFormReadingCaptureTimeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region DriversLicense

    public class DriversLicenseFactory : TestDataFactory<DriversLicense>
    {
        public const string DEFAULT_LICENSE_NUMBER = "R9690 12345 43215";

        public DriversLicenseFactory(IContainer container) : base(container) { }

        static DriversLicenseFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                DriversLicenseClass = typeof(DriversLicenseClassFactory),
                State = typeof(StateFactory),
                LicenseNumber = DEFAULT_LICENSE_NUMBER,
                IssuedDate = Lambdas.GetYesterday,
                RenewalDate = Lambdas.GetNow
            });
        }
    }

    #endregion

    #region DriversLicenseType

    public class DriversLicenseClassFactory : TestDataFactory<DriversLicenseClass>
    {
        public DriversLicenseClassFactory(IContainer container) : base(container) { }

        static DriversLicenseClassFactory()
        {
            var i = 65;
            Func<string> classFn = () => ((char)i++).ToString();

            Defaults(new {
                Description = classFn
            });
        }
    }

    #endregion

    #region CommercialDriversLicenseProgramStatus

    public class CommercialDriversLicenseProgramStatusFactory : StaticListEntityLookupFactory<
        CommercialDriversLicenseProgramStatus, CommercialDriversLicenseProgramStatusFactory>
    {
        #region Constructors

        public CommercialDriversLicenseProgramStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InProgramCommercialDriversLicenseProgramStatusFactory : CommercialDriversLicenseProgramStatusFactory
    {
        #region Constructors

        static InProgramCommercialDriversLicenseProgramStatusFactory()
        {
            var i = 0;
            Func<string> descriptionFn =
                () => String.Format("{0} {1}", CommercialDriversLicenseProgramStatus.Descriptions.IN_PROGRAM, ++i);
            Defaults(new {
                Description = descriptionFn
            });
            OnSaving((a, s) => a.Id = (int)CommercialDriversLicenseProgramStatus.Indices.IN_PROGRAM);
        }

        public InProgramCommercialDriversLicenseProgramStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PursingCommercialDriversLicenseProgramStatusFactory : CommercialDriversLicenseProgramStatusFactory
    {
        #region Constructors

        static PursingCommercialDriversLicenseProgramStatusFactory()
        {
            var i = 0;
            Func<string> descriptionFn =
                () => String.Format("{0} {1}", CommercialDriversLicenseProgramStatus.Descriptions.PURSUING, ++i);
            Defaults(new {
                Description = descriptionFn
            });
            OnSaving((a, s) => a.Id = (int)CommercialDriversLicenseProgramStatus.Indices.PURSUING);
        }

        public PursingCommercialDriversLicenseProgramStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class NotInProgramCommercialDriversLicenseProgramStatusFactory : CommercialDriversLicenseProgramStatusFactory
    {
        #region Constructors

        static NotInProgramCommercialDriversLicenseProgramStatusFactory()
        {
            var i = 0;
            Func<string> descriptionFn =
                () => String.Format("{0} {1}", CommercialDriversLicenseProgramStatus.Descriptions.NOT_IN_PROGRAM, ++i);
            Defaults(new {
                Description = descriptionFn
            });
            OnSaving((a, s) => a.Id = (int)CommercialDriversLicenseProgramStatus.Indices.NOT_IN_PROGRAM);
        }

        public NotInProgramCommercialDriversLicenseProgramStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CommunicationTypes

    public class CommunicationTypeFactory : StaticListEntityLookupFactory<CommunicationType, CommunicationTypeFactory>
    {
        public CommunicationTypeFactory(IContainer container) : base(container) { }
    }

    public class LetterCommunicationTypeFactory : CommunicationTypeFactory
    {
        public LetterCommunicationTypeFactory(IContainer container) : base(container) { }

        static LetterCommunicationTypeFactory()
        {
            Defaults(new {Description = "Letter"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.LETTER);
        }
    }

    public class ElectronicCommunicationTypeFactory : CommunicationTypeFactory
    {
        public ElectronicCommunicationTypeFactory(IContainer container) : base(container) { }

        static ElectronicCommunicationTypeFactory()
        {
            Defaults(new {Description = "Electronic"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.ELECTRONIC);
        }
    }

    public class UploadCommunicationTypeFactory : CommunicationTypeFactory
    {
        public UploadCommunicationTypeFactory(IContainer container) : base(container) { }

        static UploadCommunicationTypeFactory()
        {
            Defaults(new {Description = "Upload"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.UPLOAD);
        }
    }

    public class EmailCommunicationTypeFactory : CommunicationTypeFactory
    {
        public EmailCommunicationTypeFactory(IContainer container) : base(container) { }

        static EmailCommunicationTypeFactory()
        {
            Defaults(new {Description = "Email"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.EMAIL);
        }
    }

    public class PdfCommunicationTypeFactory : CommunicationTypeFactory
    {
        public PdfCommunicationTypeFactory(IContainer container) : base(container) { }

        static PdfCommunicationTypeFactory()
        {
            Defaults(new {Description = "Pdf"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.PDF);
        }
    }

    public class AgencySubmittalFormCommunicationTypeFactory : CommunicationTypeFactory
    {
        public AgencySubmittalFormCommunicationTypeFactory(IContainer container) : base(container) { }

        static AgencySubmittalFormCommunicationTypeFactory()
        {
            Defaults(new {Description = "AgencySubmittalForm"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.AGENCY_SUBMITTAL_FORM);
        }
    }

    public class OtherCommunicationTypeFactory : CommunicationTypeFactory
    {
        public OtherCommunicationTypeFactory(IContainer container) : base(container) { }

        static OtherCommunicationTypeFactory()
        {
            Defaults(new {Description = "Other"});
            OnSaving((a, s) => a.Id = (int)CommunicationType.Indices.OTHER);
        }
    }

    #endregion

    #region ComplianceRequirements

    public class ComplianceRequirementFactory : StaticListEntityLookupFactory<ComplianceRequirement, ComplianceRequirementFactory>
    {
        public ComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    public class CompanyComplianceRequirementFactory : ComplianceRequirementFactory
    {
        static CompanyComplianceRequirementFactory()
        {
            Defaults(new {Description = "Company"});
            OnSaving((a, s) => a.Id = (int)ComplianceRequirement.Indices.COMPANY);
        }

        public CompanyComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    public class OSHAComplianceRequirementFactory : ComplianceRequirementFactory
    {
        static OSHAComplianceRequirementFactory()
        {
            Defaults(new {Description = "OSHA"});
            OnSaving((a, s) => a.Id = (int)ComplianceRequirement.Indices.OSHA);
        }

        public OSHAComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    public class PSMComplianceRequirementFactory : ComplianceRequirementFactory
    {
        static PSMComplianceRequirementFactory()
        {
            Defaults(new {Description = "PSM"});
            OnSaving((a, s) => a.Id = (int)ComplianceRequirement.Indices.PSM);
        }

        public PSMComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    public class RegulatoryComplianceRequirementFactory : ComplianceRequirementFactory
    {
        static RegulatoryComplianceRequirementFactory()
        {
            Defaults(new {Description = "Regulatory"});
            OnSaving((a, s) => a.Id = (int)ComplianceRequirement.Indices.REGULATORY);
        }

        public RegulatoryComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    public class TCPAComplianceRequirementFactory : ComplianceRequirementFactory
    {
        static TCPAComplianceRequirementFactory()
        {
            Defaults(new {Description = "TCPA"});
            OnSaving((a, s) => a.Id = (int)ComplianceRequirement.Indices.TCPA);
        }

        public TCPAComplianceRequirementFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region ConsolidatedCustomerSideMaterialFactory

    public class ConsolidatedCustomerSideMaterialFactory : TestDataFactory<ConsolidatedCustomerSideMaterial>
    {
        public ConsolidatedCustomerSideMaterialFactory(IContainer container) : base(container) { }

        static ConsolidatedCustomerSideMaterialFactory()
        {
            Defaults(new {
                ConsolidatedEPACode = typeof(EPACodeFactory)
            });
        }
    } 

    #endregion

    #region ContactFactory

    public class ContactFactory : TestDataFactory<Contact>
    {
        public ContactFactory(IContainer container) : base(container) { }

        static ContactFactory()
        {
            var i = 0;
            Func<string> emailFn = () => $"user_{++i}@foobar.com";
            Func<string> createdByFn = () => $"user {i}";
            Defaults(new {
                Email = emailFn,
                CreatedBy = createdByFn,
                CreatedAt = DateTime.Today,
                FirstName = "Default",
                LastName = "Dude",
                Address = typeof(AddressFactory)
            });
        }
    }

    public class ContactWithoutAddressFactory : ContactFactory
    {
        public ContactWithoutAddressFactory(IContainer container) : base(container) { }

        public override Contact Build(object overrides = null)
        {
            var c = base.Build(overrides);
            c.Address = null;
            return c;
        }
    }

    #region ContactTypeFactory

    public class ContactTypeFactory : TestDataFactory<ContactType>
    {
        public const string DEFAULT_DESCRIPTION = "Some Contact Type";
        private static int _contactTypeCount = 0;

        public ContactTypeFactory(IContainer container) : base(container) { }

        static ContactTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public override ContactType Build(object overrides = null)
        {
            var ct = base.Build(overrides);

            if (ct.Description == DEFAULT_DESCRIPTION)
            {
                _contactTypeCount++;
                ct.Description += " " + _contactTypeCount;
            }

            return ct;
        }

        protected override ContactType Save(ContactType entity)
        {
            var existing = Session.Query<ContactType>().SingleOrDefault(x => x.Description == entity.Description);
            if (existing != null)
            {
                return existing;
            }

            return base.Save(entity);
        }
    }

    #endregion

    #endregion

    #region Contractor

    public class ContractorFactory : TestDataFactory<Contractor>
    {
        #region Constants

        public const string DEFAULT_NAME = "MMSI Electrical";

        private static int _count;

        #endregion

        #region Constructors

        static ContractorFactory()
        {
            Func<string> name = () => {
                _count++;
                return DEFAULT_NAME + " #" + _count;
            };
            Defaults(new {
                Name = name,
                CreatedBy = "mcadmin"
            });
        }

        public ContractorFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public static void ResetDefaultNameCount()
        {
            _count = 0;
        }

        #endregion
    }

    #endregion

    #region ContractorAgreement

    public class ContractorAgreementFactory : TestDataFactory<ContractorAgreement>
    {
        #region Constructors

        static ContractorAgreementFactory()
        {
            Defaults(new {
                Contractor = typeof(ContractorFactory),
                ContractorCompany = typeof(ContractorCompanyFactory),
                ContractorWorkCategoryType = typeof(ContractorWorkCategoryTypeFactory),
                ContractorAgreementStatusType = typeof(ContractorAgreementStatusTypeFactory),
                Title = "SMALL METER REPLACEMENT AGREEMENT",
                Description = "SMALL METER REPLACEMENT AGREEMENT",
                AgreementOwner = "Shroba",
                AgreementStartDate = DateTime.Now,
                AgreementEndDate = DateTime.Now.AddDays(1),
                EstimatedContractValue = 1000000.0m,
                CreatedBy = "Sunil",
                CreatedAt = DateTime.Now
            });
        }

        public ContractorAgreementFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorInsurance

    public class ContractorInsuranceFactory : TestDataFactory<ContractorInsurance>
    {
        #region Constructors

        static ContractorInsuranceFactory()
        {
            Defaults(new {
                Contractor = typeof(ContractorFactory),
                ContractorInsuranceMinimumRequirement = typeof(ContractorInsuranceMinimumRequirementFactory),
                EffectiveDate = DateTime.Now,
                TerminationDate = DateTime.Now,
                InsuranceProvider = "Alan Hostetler Insurance",
                MeetsCurrentContractualLimits = false,
                PolicyNumber = "BP90094946",
                CreatedBy = "Sunil",
                CreatedAt = DateTime.Now
            });
        }

        public ContractorInsuranceFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorCompany

    public class ContractorCompanyFactory : TestDataFactory<ContractorCompany>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "AW";

        #endregion

        #region Constructors

        static ContractorCompanyFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ContractorCompanyFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorWorkCategoryType

    public class ContractorWorkCategoryTypeFactory : TestDataFactory<ContractorWorkCategoryType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Architectural Services";

        #endregion

        #region Constructors

        static ContractorWorkCategoryTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ContractorWorkCategoryTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorAgreementStatusType

    public class ContractorAgreementStatusTypeFactory : TestDataFactory<ContractorAgreementStatusType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Active";

        #endregion

        #region Constructors

        static ContractorAgreementStatusTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ContractorAgreementStatusTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorInsuranceMinimumRequirement

    public class ContractorInsuranceMinimumRequirementFactory : TestDataFactory<ContractorInsuranceMinimumRequirement>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "TRANSPORTATION LEVEL 1";

        #endregion

        #region Constructors

        static ContractorInsuranceMinimumRequirementFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ContractorInsuranceMinimumRequirementFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ContractorLaborCost

    public class ContractorLaborCostFactory : TestDataFactory<ContractorLaborCost>
    {
        public ContractorLaborCostFactory(IContainer container) : base(container) { }

        static ContractorLaborCostFactory()
        {
            var i = 1;
            Func<string> stockNumberFn = () => i.ToString().PadLeft(4, '0');
            Func<string> unitFn = () => "A" + stockNumberFn();
            Func<string> jobDescriptionFn = () => string.Format("Do stuff {0}", i++);

            Defaults(new {
                StockNumber = stockNumberFn,
                Unit = unitFn,
                JobDescription = jobDescriptionFn,
                Cost = 6.66m
            });
        }
    }

    #endregion

    #region ContractorMeterCrew

    public class ContractorMeterCrewFactory : TestDataFactory<ContractorMeterCrew>
    {
        static ContractorMeterCrewFactory()
        {
            Defaults(new {
                Contractor = typeof(ContractorFactory),
                Description = "Some crew"
            });
        }

        public ContractorMeterCrewFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ContractorOverrideLaborCost

    public class ContractorOverrideLaborCostFactory : TestDataFactory<ContractorOverrideLaborCost>
    {
        public ContractorOverrideLaborCostFactory(IContainer container) : base(container) { }

        static ContractorOverrideLaborCostFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Contractor = typeof(ContractorFactory),
                ContractorLaborCost = typeof(ContractorLaborCostFactory),
                Cost = 1m,
                Percentage = 1,
                EffectiveDate = Lambdas.GetNowDate
            });
        }
    }

    #endregion

    #region ContractorUser

    public class ContractorUserFactory : TestDataFactory<ContractorUser>
    {
        #region Constants

        public const string QUESTION = "What's the meaning of life, the universe, and everything?";
        public const string ANSWER = "42";

        #endregion

        #region Constructors

        static ContractorUserFactory()
        {
            var i = 0;
            Func<string> emailFn = () => string.Format("foo{0}@bar.baz", i++);
            Defaults(new {
                Email = emailFn,
                PasswordQuestion = QUESTION,
                PasswordAnswer = ANSWER,
                Password = "foo",
                Contractor = typeof(ContractorFactory),
                IsAdmin = false
            });
        }

        public ContractorUserFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override ContractorUser Save(ContractorUser entity)
        {
            //  entity.Email = entity.Email.SanitizeAndDowncase();
            entity.PasswordSalt = Guid.NewGuid();
            entity.Password = entity.Password.Salt(entity.PasswordSalt);
            entity.PasswordAnswer = entity
                                   .PasswordAnswer
                                   .SanitizeAndDowncase()
                                   .Salt(entity.PasswordSalt);
            return base.Save(entity);
        }

        #endregion
    }

    #endregion

    #region ConditionType

    public class ConditionTypeFactory : UniqueEntityLookupFactory<ConditionType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "As Found";

        #endregion

        #region Constructors

        static ConditionTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ConditionTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ConditionDescription

    public class ConditionDescriptionFactory : UniqueEntityLookupFactory<ConditionDescription>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Unable to Inspect";

        #endregion

        #region Constructors

        static ConditionDescriptionFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                ConditionType = typeof(ConditionTypeFactory)
            });
        }
        
        public ConditionDescriptionFactory(IContainer container) : base(container) { }
        
        #endregion
    }

    #endregion

    #region CoordinateFactory

    public class CoordinateFactory : TestDataFactory<Coordinate>
    {
        #region Constants

        public const decimal LATITUDE = 40.32246702m,
                             LONGITUDE = -74.14810180m;

        #endregion

        #region Constructors

        static CoordinateFactory()
        {
            Defaults(new {
                Latitude = LATITUDE,
                Longitude = LONGITUDE,
                Icon = typeof(MapIconFactory)
            });
        }

        public CoordinateFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CorrectiveOrderProblemCode

    public class CorrectiveOrderProblemCodeFactory : TestDataFactory<CorrectiveOrderProblemCode>
    {
        public CorrectiveOrderProblemCodeFactory(IContainer container) : base(container) { }

        static CorrectiveOrderProblemCodeFactory()
        {
            var code = 0;
            Func<string> codeFn = () => $"Code{++code}";
            var description = 0;
            Func<string> descFn = () => $"Problem Code {++description}";

            Defaults(new {
                Code = codeFn,
                Description = descFn
            });
        }
    }

    #endregion

    #region CountyFactory

    public class CountyFactory : TestDataFactory<County>
    {
        #region Constants

        public static string NAME = "Monmouth";

        #endregion

        #region Fields

        public static int _count;

        #endregion

        #region Constructors

        static CountyFactory()
        {
            Func<string> nameCreatorThing = () => {
                _count++;
                return String.Format("{0} {1}", NAME, _count);
            };
            Defaults(new {
                Name = nameCreatorThing,
                State = typeof(StateFactory),
                Enabled = true
            });
        }

        public CountyFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override County Create(object overrides = null)
        {
            var c = base.Create(overrides);
            if (!c.State.Counties.Contains(c))
            {
                c.State.Counties.Add(c);
            }

            return c;
        }

        protected override County Save(County entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<County>>();
            var existing = repo.GetAll().Where(x => x.Name == entity.Name).SingleOrDefault();
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    #endregion

    #region CovidAnswerType

    public class CovidAnswerTypeFactory : UniqueEntityLookupFactory<CovidAnswerType>
    {
        public CovidAnswerTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CovidIssue

    public class CovidIssueFactory : TestDataFactory<CovidIssue>
    {
        #region Constants

        public const string DEFAULT_QUESTION = "CovidIssue";

        #endregion

        #region Constructors

        static CovidIssueFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                PersonnelArea = typeof(PersonnelAreaFactory),
                RequestType = typeof(CovidRequestTypeFactory),
                SubmissionDate = new DateTime(2020, 4, 1),
                QuestionFromEmail = "Game of questions?",
                SubmissionStatus = typeof(CovidSubmissionStatusFactory),
                OutcomeDescription = "outcome description",
                OutcomeCategory = typeof(CovidOutcomeCategoryFactory)
            });
        }

        public CovidIssueFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CovidRequestType

    public class CovidRequestTypeFactory : UniqueEntityLookupFactory<CovidRequestType>
    {
        public CovidRequestTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CovidSubmissionStatus

    public class CovidSubmissionStatusFactory : UniqueEntityLookupFactory<CovidSubmissionStatus>
    {
        public CovidSubmissionStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CovidOutcomeCategory

    public class CovidOutcomeCategoryFactory : UniqueEntityLookupFactory<CovidOutcomeCategory>
    {
        public CovidOutcomeCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CrashType

    public class CrashTypeFactory : StaticListEntityLookupFactory<CrashType, CrashTypeFactory>
    {
        #region Constructors

        public CrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class RearEndCrashTypeFactory : CrashTypeFactory
    {
        #region Constructors

        static RearEndCrashTypeFactory()
        {
            Defaults(new {
                Description = "Rear-End"
            });
            OnSaving((a, s) => a.Id = (int)CrashType.Indices.REAR_END);
        }

        public RearEndCrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SideswipeCrashTypeFactory : CrashTypeFactory
    {
        #region Constructors

        static SideswipeCrashTypeFactory()
        {
            Defaults(new {
                Description = "Sideswipe"
            });
            OnSaving((a, s) => a.Id = (int)CrashType.Indices.SIDEWIPE);
        }

        public SideswipeCrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class FrontalCrashTypeFactory : CrashTypeFactory
    {
        #region Constructors

        static FrontalCrashTypeFactory()
        {
            Defaults(new {
                Description = "Frontal"
            });
            OnSaving((a, s) => a.Id = (int)CrashType.Indices.FRONTAL);
        }

        public FrontalCrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SideCrashTypeFactory : CrashTypeFactory
    {
        #region Constructors

        static SideCrashTypeFactory()
        {
            Defaults(new {
                Description = "Side"
            });
            OnSaving((a, s) => a.Id = (int)CrashType.Indices.SIDE);
        }

        public SideCrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OtherCrashTypeFactory : CrashTypeFactory
    {
        #region Constructors

        static OtherCrashTypeFactory()
        {
            Defaults(new {
                Description = "Other"
            });
            OnSaving((a, s) => a.Id = (int)CrashType.Indices.OTHER);
        }

        public OtherCrashTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Crew

    public class CrewFactory : TestDataFactory<Crew>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Crew Default";

        #endregion

        #region Constructors

        static CrewFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Active = true
            });
        }

        public CrewFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CrewAssignment

    public class CrewAssignmentFactory : TestDataFactory<CrewAssignment>
    {
        #region Constants

        public const int DEFAULT_PRIORITY = 1;

        #endregion

        #region Constructors

        static CrewAssignmentFactory()
        {
            Defaults(new {
                AssignedFor = Lambdas.GetNow,
                AssignedOn = Lambdas.GetNow,
                Priority = DEFAULT_PRIORITY,
                Crew = typeof(CrewFactory),
                WorkOrder = typeof(WorkOrderFactory),
            });
        }

        public CrewAssignmentFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override CrewAssignment Create(object overrides = null)
        {
            var ass = base.Create(overrides);
            Session.Flush();
            Session.Clear();
            return ass;
        }

        #endregion
    }

    public class OpenCrewAssignmentFactory : CrewAssignmentFactory
    {
        #region Constructors

        static OpenCrewAssignmentFactory()
        {
            Defaults(new {
                AssignedFor = Lambdas.GetYesterdayDate,
                AssignedOn = Lambdas.GetYesterday,
                DateStarted = Lambdas.GetYesterday
            });
        }

        public OpenCrewAssignmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ClosedCrewAssignmentFactory : OpenCrewAssignmentFactory
    {
        #region Constants

        public const int DEFAULT_EMPLOYEES_ON_JOB = 2;

        #endregion

        #region Constructors

        static ClosedCrewAssignmentFactory()
        {
            Defaults(new {
                DateEnded = DateTime.Now,
                EmployeesOnJob = (float)DEFAULT_EMPLOYEES_ON_JOB
            });
        }

        public ClosedCrewAssignmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CrossingCategory

    public class CrossingCategoryFactory : UniqueEntityLookupFactory<CrossingCategory>
    {
        public CrossingCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CustomerCoordinate

    public class CustomerCoordinateFactory : TestDataFactory<CustomerCoordinate>
    {
        #region Constants

        public const int DEFAULT_SOURCE = 1;
        public const float DEFAULT_LATITUDE = 12, DEFAULT_LONGITUDE = 13;

        #endregion

        #region Constructors

        static CustomerCoordinateFactory()
        {
            Defaults(new {
                Source = DEFAULT_SOURCE,
                Latitude = DEFAULT_LATITUDE,
                Longitude = DEFAULT_LONGITUDE,
                CustomerLocation = typeof(CustomerLocationFactory)
            });
        }

        public CustomerCoordinateFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CustomerImpactRange

    public class CustomerImpactRangeFactory : UniqueEntityLookupFactory<CustomerImpactRange>
    {
        public CustomerImpactRangeFactory(IContainer container) : base(container) { }
    }

    public class ZeroToFiftyCustomerImpactRangeFactory : CustomerImpactRangeFactory
    {
        static ZeroToFiftyCustomerImpactRangeFactory()
        {
            Defaults(new {
                Description = "0-50"
            });

            OnSaving((a, s) => a.Id = CustomerImpactRange.Indices.ZERO_TO_FIFTY);
        }

        public ZeroToFiftyCustomerImpactRangeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region CustomerLocation

    public class CustomerLocationFactory : TestDataFactory<CustomerLocation>
    {
        #region Constants

        public const string DEFAULT_PREMISE_NUMBER = "7328675309",
                            DEFAULT_ADDRESS = "123 Easy St.",
                            DEFAULT_CITY = "Anytown",
                            DEFAULT_STATE = "NJ",
                            DEFAULT_ZIP = "07700-0000";

        #endregion

        #region Constructors

        static CustomerLocationFactory()
        {
            Defaults(new {
                PremiseNumber = DEFAULT_PREMISE_NUMBER,
                Address = DEFAULT_ADDRESS,
                City = DEFAULT_CITY,
                State = DEFAULT_STATE,
                Zip = DEFAULT_ZIP
            });
        }

        public CustomerLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CutoffSawQuestion

    public class CutoffSawQuestionFactory : TestDataFactory<CutoffSawQuestion>
    {
        #region Constants

        public const string DEFAULT_QUESTION = "What is your favorite color?";
        public const int DEFAULT_SORT_ORDER = 2;

        #endregion

        #region Constructors

        static CutoffSawQuestionFactory()
        {
            Defaults(new {
                Question = DEFAULT_QUESTION,
                SortOrder = DEFAULT_SORT_ORDER,
                IsActive = true
            });
        }

        public CutoffSawQuestionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region CutoffSawQuestionnaire

    public class CutoffSawQuestionnaireFactory : TestDataFactory<CutoffSawQuestionnaire>
    {
        #region Constants

        //public const string DEFAULT_DESCRIPTION = "CutoffSawQuestionnaire";

        #endregion

        #region Constructors

        static CutoffSawQuestionnaireFactory()
        {
            Defaults(new {
                LeadPerson = typeof(EmployeeFactory),
                SawOperator = typeof(EmployeeFactory),
                OperatedOn = Lambdas.GetNow,
            });
        }

        public CutoffSawQuestionnaireFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region DataTableLayout

    public class DataTableLayoutFactory : TestDataFactory<DataTableLayout>
    {
        public DataTableLayoutFactory(IContainer container) : base(container) { }

        static DataTableLayoutFactory()
        {
            int i = 1;
            Func<string> layoutNameFn = () => String.Format("Layout Name {0}", i++);

            Defaults(new {
                Area = string.Empty,
                Controller = "SomeController",
                LayoutName = layoutNameFn,
            });
        }
    }

    #endregion

    #region DataTypeFactory

    public class DataTypeFactory : TestDataFactory<DataType>
    {
        public const string NAME = "default name",
                            TABLE_ID = "default table id",
                            TABLE_NAME = "some table";

        public DataTypeFactory(IContainer container) : base(container) { }

        static DataTypeFactory()
        {
            Defaults(new {
                Name = NAME,
                TableID = TABLE_ID,
                TableName = TABLE_NAME
            });
        }
    }

    public class WorkOrdersDataTypeFactory : DataTypeFactory
    {
        public WorkOrdersDataTypeFactory(IContainer container) : base(container) { }

        static WorkOrdersDataTypeFactory()
        {
            Defaults(new {
                Name = NAME,
                TableID = TABLE_ID,
                TableName = "WorkOrders"
            });
        }
    }

    #endregion

    #region Department

    public class DepartmentFactory : TestDataFactory<Department>
    {
        #region Constants

        public const string DESCRIPTION = "Department";

        #endregion

        #region Constructors

        static DepartmentFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }

        public DepartmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region DischargeCauseFactory

    public class DischargeCauseFactory : UniqueEntityLookupFactory<DischargeCause>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Discharge Cause Description";

        #endregion

        public DischargeCauseFactory(IContainer container) : base(container) { }

        static DischargeCauseFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }
    }

    #endregion

    #region DischargeWeatherRelatedTypeFactory

    #region DischargeWeatherRelatedType

    public class DischargeWeatherRelatedTypeFactory : StaticListEntityLookupFactory<DischargeWeatherRelatedType, DischargeWeatherRelatedTypeFactory>
    {
        public DischargeWeatherRelatedTypeFactory(IContainer container) : base(container) { }
    }

    public class DryDischargeWeatherRelatedTypeFactory : DischargeWeatherRelatedTypeFactory
    {
        static DryDischargeWeatherRelatedTypeFactory()
        {
            Defaults(new {
                Description = "Dry"
            });

            OnSaving((x, _) => x.Id = DischargeWeatherRelatedType.Indices.DRY);
        }

        public DryDischargeWeatherRelatedTypeFactory(IContainer container) : base(container) { }
    }

    public class WetDischargeWeatherRelatedTypeFactory : DischargeWeatherRelatedTypeFactory
    {
        static WetDischargeWeatherRelatedTypeFactory()
        {
            Defaults(new {
                Description = "Wet"
            });

            OnSaving((x, _) => x.Id = DischargeWeatherRelatedType.Indices.WET);
        }

        public WetDischargeWeatherRelatedTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #endregion

    #region Division

    public class DivisionFactory : TestDataFactory<Division>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Division";

        #endregion

        #region Constructors

        static DivisionFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                State = typeof(StateFactory)
            });
        }

        public DivisionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region DocumentFactory

    public class DocumentFactory : TestDataFactory<Document>
    {
        public const string FILENAME = "default file name";

        public DocumentFactory(IContainer container) : base(container) { }

        static DocumentFactory()
        {
            Defaults(new {
                CreatedAt = Lambdas.GetNow,
                UpdatedAt = Lambdas.GetNow,
                FileName = FILENAME,
                CreatedBy = typeof(UserFactory),
                UpdatedBy = typeof(UserFactory),
                DocumentData = typeof(DocumentDataFactory),
                DocumentType = typeof(DocumentTypeFactory)
            });
        }
    }

    #endregion

    #region DocumentDataFactory

    public class DocumentDataFactory : TestDataFactory<DocumentData>
    {
        private static int _currentBinaryData = 0;

        public DocumentDataFactory(IContainer container) : base(container) { }

        static DocumentDataFactory()
        {
            // This func's for making sure DocumentDataFactory
            // creates a unique hash instance by default.
            Func<byte[]> incrementalBinaryData = () => {
                _currentBinaryData++;
                return BitConverter.GetBytes(_currentBinaryData);
            };

            Defaults(new {
                BinaryData = incrementalBinaryData
            });
        }

        public override DocumentData Build(object overrides = null)
        {
            var data = base.Build(overrides);
            if (data.BinaryData != null)
            {
                var docServ = _container.GetInstance<IDocumentService>();
                data.Hash = docServ.Save(data.BinaryData);
                data.FileSize = data.BinaryData.Length;
            }

            return data;
        }

        protected override DocumentData Save(DocumentData entity)
        {
            // Hash is a unique field, so we need to return the same instance.
            var existing = Session.Query<DocumentData>().SingleOrDefault(x => x.Hash == entity.Hash);
            if (existing != null)
            {
                return existing;
            }

            return base.Save(entity);
        }
    }

    #endregion

    #region DocumentLinkFactory

    public class DocumentLinkFactory : TestDataFactory<DocumentLink>
    {
        public DocumentLinkFactory(IContainer container) : base(container) { }

        static DocumentLinkFactory()
        {
            Defaults(new {
                Document = typeof(DocumentFactory),
                DataType = typeof(DataTypeFactory),
                DocumentType = typeof(DocumentTypeFactory)
            });
        }
    }

    #endregion

    #region DocumentTypeFactory

    public class DocumentTypeFactory : TestDataFactory<DocumentType>
    {
        public const string NAME = "default name";

        public DocumentTypeFactory(IContainer container) : base(container) { }

        static DocumentTypeFactory()
        {
            Defaults(new {
                Name = NAME,
                DataType = typeof(DataTypeFactory)
            });
        }
    }

    #endregion

    #region DocumentStatusFactory

    public class DocumentStatusFactory : UniqueEntityLookupFactory<DocumentStatus>
    {
        public DocumentStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region DriversLicenseEndorsement

    public class DriversLicenseEndorsementFactory : TestDataFactory<DriversLicenseEndorsement>
    {
        public DriversLicenseEndorsementFactory(IContainer container) : base(container) { }

        static DriversLicenseEndorsementFactory()
        {
            var i = 65;
            Func<string> letterFn = () => ((char)i).ToString();
            Func<string> titleFn = () => String.Format("Endorsement {0}", i++);

            Defaults(new {
                Letter = letterFn,
                Title = titleFn
            });
        }
    }

    #endregion

    #region DriversLicenseRestriction

    public class DriversLicenseRestrictionFactory : TestDataFactory<DriversLicenseRestriction>
    {
        public DriversLicenseRestrictionFactory(IContainer container) : base(container) { }

        static DriversLicenseRestrictionFactory()
        {
            var i = 65;
            Func<string> letterFn = () => ((char)i).ToString();
            Func<string> titleFn = () => String.Format("Restriction {0}", i++);

            Defaults(new {
                Letter = letterFn,
                Title = titleFn
            });
        }
    }

    #endregion

    #region EchoshoreSiteAlert

    public class EchoshoreLeakAlertFactory : TestDataFactory<EchoshoreLeakAlert>
    {
        #region Constructors

        static EchoshoreLeakAlertFactory()
        {
            Defaults(new {
                Hydrant1Text = "Hydrant1 Text",
                Hydrant2Text = "Hydrant2 Text",
                PointOfInterestStatus = typeof(PointOfInterestStatusFactory),
                Coordinate = typeof(CoordinateFactory),
                EchoshoreSite = typeof(EchoshoreSiteFactory)
            });
        }

        public EchoshoreLeakAlertFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EchoshoreSite

    public class EchoshoreSiteFactory : TestDataFactory<EchoshoreSite>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EchoshoreSite";

        #endregion

        #region Constructors

        static EchoshoreSiteFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Town = typeof(TownFactory),
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public EchoshoreSiteFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EmergencyPowerType

    public class EmergencyPowerTypeFactory : TestDataFactory<EmergencyPowerType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EmergencyPowerType";

        #endregion

        #region Constructors

        static EmergencyPowerTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public EmergencyPowerTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EmergencyResponsePlan

    public class EmergencyResponsePlanFactory : TestDataFactory<EmergencyResponsePlan>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EmergencyResponsePlan", DEFAULT_TITLE = "DEFAULT_TITLE";

        #endregion

        #region Constructors

        static EmergencyResponsePlanFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                Title = DEFAULT_TITLE,
                EmergencyPlanCategory = typeof(EntityLookupTestDataFactory<EmergencyPlanCategory>)
            });
        }

        public EmergencyResponsePlanFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Employee

    public class EmployeeFactory : TestDataFactory<Employee>
    {
        #region Constants

        public static readonly object[] NAMES = {
            new {First = "Bill", Last = "S. Preston, Esq."},
            new {First = "Ted", Last = "\"Theodore\" Logan"},
            new {First = "Namrock", Last = "Namrock"},
            new {First = "Emmanuel", Last = "Goldstein"},
            new {First = "René", Last = "Margritte"},
            new {First = "Bill", Last = "Ward"}
        };

        #endregion

        #region Constructors

        static EmployeeFactory()
        {
            int idx = 1;
            Func<string> employeeIdFn = () => (idx++).ToString().PadLeft(7, '0');
            Func<string> firstNameFn = () => ((dynamic)NAMES[idx.Indexify(NAMES.Count())]).First;
            Func<string> lastNameFn = () => ((dynamic)NAMES[idx.Indexify(NAMES.Count())]).Last;
            Func<string> emailFn = () => $"user{idx}@somecompany.com";
            Defaults(new {
                FirstName = firstNameFn,
                LastName = lastNameFn,
                EmployeeId = employeeIdFn,
                EmailAddress = emailFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                CommercialDriversLicenseProgramStatus =
                    typeof(NotInProgramCommercialDriversLicenseProgramStatusFactory),
                PositionGroup = typeof(PositionGroupFactory),
            });
        }

        public EmployeeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ActiveEmployeeFactory : EmployeeFactory
    {
        static ActiveEmployeeFactory()
        {
            Defaults(new {
                Status = typeof(ActiveEmployeeStatusFactory)
            });
        }

        public ActiveEmployeeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeAccountabilityActionFactory

    public class EmployeeAccountabilityActionFactory : TestDataFactory<EmployeeAccountabilityAction>
    {
        static EmployeeAccountabilityActionFactory()
        {
            Defaults(new {
                DateAdministered = Lambdas.GetNow,
                AccountabilityActionTakenDescription = " ",
                OperatingCenter = typeof(OperatingCenterFactory),
                Incident = typeof(IncidentFactory),
                Grievance = typeof(GrievanceFactory),
                Employee = typeof(EmployeeFactory),
                AccountabilityActionTakenType = typeof(AccountabilityActionTakenTypeFactory),
                DisciplineAdministeredBy = typeof(EmployeeFactory)
            });
        }

        public EmployeeAccountabilityActionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeAssignmentFactory

    public class EmployeeAssignmentFactory : TestDataFactory<EmployeeAssignment>
    {
        static EmployeeAssignmentFactory()
        {
            Defaults(new {
                AssignedOn = Lambdas.GetNow,
                AssignedFor = Lambdas.GetNow,
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                AssignedTo = typeof(EmployeeFactory)
            });
        }

        public EmployeeAssignmentFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeAssignmentFactory

    public class EmployeeProductionSkillSetFactory : TestDataFactory<EmployeeProductionSkillSet>
    {
        static EmployeeProductionSkillSetFactory()
        {
            Defaults(new {
                ProductionSkillSet = typeof(ProductionSkillSetFactory),
                Employee = typeof(EmployeeFactory)
            });
        }

        public EmployeeProductionSkillSetFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeDepartment

    public class EmployeeDepartmentFactory : TestDataFactory<EmployeeDepartment>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EmployeeDepartment";

        #endregion

        #region Constructors

        static EmployeeDepartmentFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public EmployeeDepartmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EmployeeFMLANotification

    public class EmployeeFMLANotificationFactory : UniqueEntityLookupFactory<EmployeeFMLANotification>
    {
        public EmployeeFMLANotificationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeHeadCount

    public class EmployeeHeadCountFactory : TestDataFactory<EmployeeHeadCount>
    {
        #region Constructors

        static EmployeeHeadCountFactory()
        {
            Defaults(new {
                CreatedBy = "some user"
            });
        }

        public EmployeeHeadCountFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EmployeeSpokeWithNurse

    public class EmployeeSpokeWithNurseFactory : UniqueEntityLookupFactory<EmployeeSpokeWithNurse>
    {
        public EmployeeSpokeWithNurseFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeStatus

    public class EmployeeStatusFactory : StaticListEntityLookupFactory<EmployeeStatus, EmployeeStatusFactory>
    {
        public EmployeeStatusFactory(IContainer container) : base(container) { }
    }

    public class ActiveEmployeeStatusFactory : EmployeeStatusFactory
    {
        static ActiveEmployeeStatusFactory()
        {
            Defaults(new {
                Description = "Active"
            });
            OnSaving((es, _) => es.SetPublicPropertyValueByName("Id", EmployeeStatus.Indices.ACTIVE));
        }

        public ActiveEmployeeStatusFactory(IContainer container) : base(container) { }
    }

    public class InactiveEmployeeStatusFactory : EmployeeStatusFactory
    {
        static InactiveEmployeeStatusFactory()
        {
            Defaults(new {
                Description = "Inactive"
            });
            OnSaving((es, _) => es.SetPublicPropertyValueByName("Id", EmployeeStatus.Indices.INACTIVE));
        }

        public InactiveEmployeeStatusFactory(IContainer container) : base(container) { }
    }

    public class WithdrawnEmployeeStatusFactory : EmployeeStatusFactory
    {
        static WithdrawnEmployeeStatusFactory()
        {
            Defaults(new {
                Description = "Withdrawn"
            });
            OnSaving((es, _) => es.SetPublicPropertyValueByName("Id", EmployeeStatus.Indices.WITHDRAWN));
        }

        public WithdrawnEmployeeStatusFactory(IContainer container) : base(container) { }
    }

    public class RetireeEmployeeStatusFactory : EmployeeStatusFactory
    {
        static RetireeEmployeeStatusFactory()
        {
            Defaults(new {
                Description = "Retiree"
            });
            OnSaving((es, _) => es.SetPublicPropertyValueByName("Id", EmployeeStatus.Indices.RETIREE));
        }

        public RetireeEmployeeStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EmployeeType

    public class EmployeeTypeFactory : UniqueEntityLookupFactory<EmployeeType>
    {
        public EmployeeTypeFactory(IContainer container) : base(container) { }
    }

    public class EmployeeTypeContractorFactory : EmployeeTypeFactory
    {
        static EmployeeTypeContractorFactory()
        {
            Defaults(new {
                Description = "Contractor"
            });
            OnSaving((es, _) => es.SetPublicPropertyValueByName("Id", EmployeeType.Indices.CONTRACTOR));
        }

        public EmployeeTypeContractorFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EndOfPipeExceedance

    public class EndOfPipeExceedanceFactory : TestDataFactory<EndOfPipeExceedance>
    {
        public EndOfPipeExceedanceFactory(IContainer container) : base(container) { }

        static EndOfPipeExceedanceFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                WasteWaterSystem = typeof(WasteWaterSystemFactory),
                Facility = typeof(FacilityFactory),
                EventDate = Lambdas.GetNow,
                EndOfPipeExceedanceType = typeof(EntityLookupTestDataFactory<EndOfPipeExceedanceType>),
                LimitationType = typeof(EntityLookupTestDataFactory<LimitationType>),
                EndOfPipeExceedanceRootCause = typeof(EntityLookupTestDataFactory<EndOfPipeExceedanceRootCause>),
                ConsentOrder = true,
                NewAcquisition = true,
                BriefDescription = "Troy & Abed in the morning!"
            });
        }
    }

    #endregion

    #region EndorsementStatus

    public class EndorsementStatusFactory : TestDataFactory<EndorsementStatus>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EndorsementStatus";

        #endregion

        #region Constructors

        static EndorsementStatusFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public EndorsementStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EnvironmentalNonComplianceEventAndTypesActionItemsFailuresEntityLevels
    
    public class EnvironmentalNonComplianceEventFactory : TestDataFactory<EnvironmentalNonComplianceEvent>
    {
        public EnvironmentalNonComplianceEventFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventFactory()
        {
            var i = 0;

            Func<string> summaryFn = () => $"Symmary {++i}";

            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                Facility = typeof(FacilityFactory),
                IssueType = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventType>),
                Responsibility = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventResponsibility>),
                IssueStatus = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventStatus>),
                IssuingEntity = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventEntityLevel>),
                SummaryOfEvent = summaryFn,
                AwarenessDate = Lambdas.GetNow,
                EventDate = Lambdas.GetNow,
                NOVWorkGroupReviewDate = Lambdas.GetNow,
                ChiefEnvOfficerApprovalDate = Lambdas.GetNow,
                CountsAgainstTarget = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventCountsAgainstTarget>)
            });
        }
    }

    public class EnvironmentalNonComplianceEventTypeFactory : TestDataFactory<EnvironmentalNonComplianceEventType>
    {
        public EnvironmentalNonComplianceEventTypeFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventTypeFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Sub Type {++i}";

            Defaults(new {
                Description = descFn
            });
        }
    }

    public class EnvironmentalNonComplianceEventSubTypeFactory : TestDataFactory<EnvironmentalNonComplianceEventSubType>
    {
        public EnvironmentalNonComplianceEventSubTypeFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventSubTypeFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Sub Type {++i}";

            Defaults(new {
                EnvironmentalNonComplianceEventType = typeof(EnvironmentalNonComplianceEventTypeFactory),
                Description = descFn
            });
        }
    }

    public class
        EnvironmentalNonComplianceEventActionItemFactory : TestDataFactory<EnvironmentalNonComplianceEventActionItem>
    {
        public EnvironmentalNonComplianceEventActionItemFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventActionItemFactory()
        {
            var i = 0;
            Func<string> actionItemFn = () => $"ActionItem {++i}";
            Defaults(new {
                EnvironmentalNonComplianceEvent = typeof(EnvironmentalNonComplianceEventFactory),
                Type = typeof(EntityLookupTestDataFactory<EnvironmentalNonComplianceEventActionItemType>),
                ActionItem = actionItemFn,
                TargetedCompletionDate = Lambdas.GetNow
            });
        }
    }

    public class EnvironmentalNonComplianceEventFailureTypeFactory : TestDataFactory<EnvironmentalNonComplianceEventFailureType>
    {
        public EnvironmentalNonComplianceEventFailureTypeFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventFailureTypeFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Failure Type {++i}";

            Defaults(new {
                Description = descFn
            });
        }
    }

    public class EnvironmentalNonComplianceEventEntityLevelFactory : TestDataFactory<EnvironmentalNonComplianceEventEntityLevel>
    {
        public EnvironmentalNonComplianceEventEntityLevelFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventEntityLevelFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Entity Level {++i}";

            Defaults(new {
                Description = descFn
            });
        }
    }

    #endregion

    #region EnvironmentalNonComplianceEventCountsAgainstTarget

    public class EnvironmentalNonComplianceEventCountsAgainstTargetFactory : TestDataFactory<EnvironmentalNonComplianceEventCountsAgainstTarget>
    {
        public EnvironmentalNonComplianceEventCountsAgainstTargetFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventCountsAgainstTargetFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Counts Against Target {++i}";

            Defaults(new {
                Description = descFn
            });
        }
    }

    #endregion

    #region EnvironmentalNonComplianceEventResponsibility

    public class EnvironmentalNonComplianceEventResponsibilityFactory : EntityLookupTestDataFactory<EnvironmentalNonComplianceEventResponsibility>
    {
        public EnvironmentalNonComplianceEventResponsibilityFactory(IContainer container) : base(container) { }

        static EnvironmentalNonComplianceEventResponsibilityFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Environmental Non Compliance Event Responsibilty {++i}";

            Defaults(new {
                Description = descFn
            });
        }
    }

    #endregion

    #region EnvironmentalPermit

    public class EnvironmentalPermitFactory : TestDataFactory<EnvironmentalPermit>
    {
        #region Constants

        public const string PERMIT_NUMBER = "P#1123";

        #endregion

        #region Constructors

        static EnvironmentalPermitFactory()
        {
            Defaults(new {
                PermitNumber = PERMIT_NUMBER,
                State = typeof(StateFactory),
                FacilityType = typeof(WaterTypeFactory),
                EnvironmentalPermitType = typeof(EnvironmentalPermitTypeFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                PermitEffectiveDate = Lambdas.GetNowDate,
                ReportingRequired = false
            });
        }

        public EnvironmentalPermitFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EnvironmentalExpiringPermitFactory : TestDataFactory<EnvironmentalPermit>
    {
        #region Constants

        public const string PERMIT_NUMBER = "P#2246";

        #endregion

        #region Constructors

        static EnvironmentalExpiringPermitFactory()
        {
            Defaults(new {
                PermitNumber = PERMIT_NUMBER,
                State = typeof(StateFactory),
                PermitExpirationDate = Lambdas.GetNow
            });
        }

        public EnvironmentalExpiringPermitFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class RequiresFeesEnvironmentalPermitFactory : EnvironmentalPermitFactory
    {
        #region Constructors

        static RequiresFeesEnvironmentalPermitFactory()
        {
            Defaults(new {
                RequiresFees = true
            });
        }

        public RequiresFeesEnvironmentalPermitFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EnvironmentalPermitFeePaymentMethod

    public class EnvironmentalPermitFeePaymentMethodFactory : StaticListEntityLookupFactory<
        EnvironmentalPermitFeePaymentMethod, EnvironmentalPermitFeePaymentMethodFactory>
    {
        public EnvironmentalPermitFeePaymentMethodFactory(IContainer container) : base(container) { }
    }

    public class MailEnvironmentalPermitFeePaymentMethodFactory : EnvironmentalPermitFeePaymentMethodFactory
    {
        static MailEnvironmentalPermitFeePaymentMethodFactory()
        {
            Defaults(new {Description = "Mail"});
            OnSaving((x, s) => x.Id = EnvironmentalPermitFeePaymentMethod.Indices.MAIL);
        }

        public MailEnvironmentalPermitFeePaymentMethodFactory(IContainer container) : base(container) { }
    }

    public class PhoneEnvironmentalPermitFeePaymentMethodFactory : EnvironmentalPermitFeePaymentMethodFactory
    {
        static PhoneEnvironmentalPermitFeePaymentMethodFactory()
        {
            Defaults(new {Description = "Phone"});
            OnSaving((x, s) => x.Id = EnvironmentalPermitFeePaymentMethod.Indices.PHONE);
        }

        public PhoneEnvironmentalPermitFeePaymentMethodFactory(IContainer container) : base(container) { }
    }

    public class UrlEnvironmentalPermitFeePaymentMethodFactory : EnvironmentalPermitFeePaymentMethodFactory
    {
        static UrlEnvironmentalPermitFeePaymentMethodFactory()
        {
            Defaults(new {Description = "URL"});
            OnSaving((x, s) => x.Id = EnvironmentalPermitFeePaymentMethod.Indices.URL);
        }

        public UrlEnvironmentalPermitFeePaymentMethodFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EnvironmentalPermitFee

    public class EnvironmentalPermitFeeFactory : TestDataFactory<EnvironmentalPermitFee>
    {
        #region Constants

        public const string PERMIT_NUMBER = "P#1123";

        #endregion

        #region Constructors

        static EnvironmentalPermitFeeFactory()
        {
            Defaults(new {
                EnvironmentalPermit = typeof(RequiresFeesEnvironmentalPermitFactory),
                Fee = 424.84m,
                PaymentMethod = typeof(UrlEnvironmentalPermitFeePaymentMethodFactory),
                PaymentMethodUrl = "http://www.example.com"
            });
        }

        public EnvironmentalPermitFeeFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override EnvironmentalPermitFee Build(object overrides = null)
        {
            var fee = base.Build(overrides);
            if (!fee.EnvironmentalPermit.Fees.Contains(fee))
            {
                fee.EnvironmentalPermit.Fees.Add(fee);
            }

            return fee;
        }

        #endregion
    }

    #endregion

    #region EnvironmentalPermitRequirement

    public class EnvironmentalPermitRequirementFactory : TestDataFactory<EnvironmentalPermitRequirement>
    {
        public EnvironmentalPermitRequirementFactory(IContainer container) : base(container) { }

        static EnvironmentalPermitRequirementFactory()
        {
            var i = 0;
            Func<string> requirementFn = () => string.Format("Requirement {0}", ++i);

            Defaults(new {
                Requirement = requirementFn,
                EnvironmentalPermit = typeof(EnvironmentalPermitFactory),
                RequirementType = typeof(EntityLookupTestDataFactory<EnvironmentalPermitRequirementType>),
                ValueUnit = typeof(EntityLookupTestDataFactory<EnvironmentalPermitRequirementValueUnit>),
                ValueDefinition = typeof(EntityLookupTestDataFactory<EnvironmentalPermitRequirementValueDefinition>),
                TrackingFrequency =
                    typeof(EntityLookupTestDataFactory<EnvironmentalPermitRequirementTrackingFrequency>),
                ReportingFrequency =
                    typeof(EntityLookupTestDataFactory<EnvironmentalPermitRequirementReportingFrequency>),
                ReportingOwner = typeof(EmployeeFactory)
            });
        }
    }

    #endregion

    #region EnvironmentalPermitType

    public class EnvironmentalPermitTypeFactory : UniqueEntityLookupFactory<EnvironmentalPermitType>
    {
        public EnvironmentalPermitTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Equipment

    public class EquipmentFactory : TestDataFactory<Equipment>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Equipment";
        public const int NUMBER = 21;

        #endregion

        #region Constructors

        static EquipmentFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Facility = typeof(FacilityFactory),
                EquipmentPurpose = typeof(EquipmentPurposeFactory),
                EquipmentManufacturer = typeof(EquipmentManufacturerFactory),
                Number = NUMBER,
                DateInstalled = Lambdas.GetNow,
                ABCIndicator = typeof(ABCIndicatorFactory),
                EquipmentStatus = typeof(InServiceEquipmentStatusFactory)
            });
        }

        public EquipmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class GasMonitorEquipmentFactory : EquipmentFactory
    {
        #region Constructors

        static GasMonitorEquipmentFactory()
        {
            Defaults(new {
                EquipmentPurpose = typeof(PersonalGasDetectorEquipmentPurposeFactory),
                EquipmentType = typeof(EquipmentTypeGasDetectorFactory)
            });
        }

        public GasMonitorEquipmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EquipmentCategory

    public class EquipmentCategoryFactory : TestDataFactory<EquipmentCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EquipmentCategory";

        #endregion

        #region Constructors

        static EquipmentCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public EquipmentCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EquipmentCategoryFlowMeterFactory : EquipmentCategoryFactory
    {
        public const string DEFAULT_DESCRIPTION = "FLOW METER";

        static EquipmentCategoryFlowMeterFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = EquipmentCategory.Indices.FLOW_METER);
        }

        public EquipmentCategoryFlowMeterFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentCharacteristicField

    public class EquipmentCharacteristicFieldTypeFactory : TestDataFactory<EquipmentCharacteristicFieldType>
    {
        public EquipmentCharacteristicFieldTypeFactory(IContainer container) : base(container) { }

        static EquipmentCharacteristicFieldTypeFactory()
        {
            Defaults(new {
                DataType = "SomeDataType"
            });
        }
    }

    public class EquipmentCharacteristicFieldFactory : TestDataFactory<EquipmentCharacteristicField>
    {
        public EquipmentCharacteristicFieldFactory(IContainer container) : base(container) { }

        static EquipmentCharacteristicFieldFactory()
        {
            var i = 0;
            Func<string> fieldNameFn = () => String.Format("Field {0}", i);

            Defaults(new {
                EquipmentType = typeof(EquipmentTypeGeneratorFactory),
                FieldType = typeof(EquipmentCharacteristicFieldTypeFactory),
                FieldName = fieldNameFn,
                IsActive = true
            });
        }
    }

    #endregion

    #region EquipmentLifespan

    public class
        EquipmentLifespanFactory : StaticListEntityLookupFactory<EquipmentLifespan, EquipmentLifespanFactory>
    {
        #region Constants

        public const decimal DEFAULT_ESTIMATED_LIFE_SPAN = 21.0M,
                             DEFAULT_EXTENDED_LIFE_MAJOR = 5.5M;

        #endregion

        #region Constructors

        static EquipmentLifespanFactory()
        {
            Defaults(new { EstimatedLifespan = DEFAULT_ESTIMATED_LIFE_SPAN, ExtendedLifeMajor = DEFAULT_EXTENDED_LIFE_MAJOR });
        }

        public EquipmentLifespanFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ChemicalFeedDryEquipmentLifespanFactory : EquipmentLifespanFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Chemical Feed Dry";

        #endregion

        #region Constructors

        static ChemicalFeedDryEquipmentLifespanFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
            OnSaving((x, s) => x.Id = EquipmentLifespan.Indices.CHEMICAL_FEED_DRY);
        }

        public ChemicalFeedDryEquipmentLifespanFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EngineEquipmentLifespanFactory : EquipmentLifespanFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Engine";

        #endregion

        #region Constructors

        static EngineEquipmentLifespanFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
            OnSaving((x, s) => x.Id = EquipmentLifespan.Indices.ENGINE);
        }

        public EngineEquipmentLifespanFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class FilterEquipmentLifespanFactory : EquipmentLifespanFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Filter";

        #endregion

        #region Constructors

        static FilterEquipmentLifespanFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
            OnSaving((x, s) => x.Id = EquipmentLifespan.Indices.FILTER);
        }

        public FilterEquipmentLifespanFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class GeneratorEquipmentLifespanFactory : EquipmentLifespanFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Generator";

        #endregion

        #region Constructors

        static GeneratorEquipmentLifespanFactory()
        {
            Defaults(new { Description = DEFAULT_DESCRIPTION });
            OnSaving((x, s) => x.Id = EquipmentLifespan.Indices.GENERATOR);
        }

        public GeneratorEquipmentLifespanFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EquipmentModel

    public class EquipmentModelFactory : TestDataFactory<EquipmentModel>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EquipmentModel";

        #endregion

        #region Constructors

        static EquipmentModelFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                EquipmentManufacturer = typeof(EquipmentManufacturerFactory)
            });
        }

        public EquipmentModelFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EquipmentSensor

    public class EquipmentSensorFactory : TestDataFactory<EquipmentSensor>
    {
        public EquipmentSensorFactory(IContainer container) : base(container) { }

        public override EquipmentSensor Create(object overrides = null)
        {
            var entity = base.Create(overrides);
            entity.Equipment.Sensors.Add(entity);
            entity.Sensor.Equipment = entity;

            return entity;
        }
    }

    #endregion

    #region Event

    public class EventFactory : TestDataFactory<Event>
    {
        static EventFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public EventFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EventDocument

    public class EventDocumentFactory : TestDataFactory<EventDocument>
    {
        static EventDocumentFactory()
        {
            Defaults(new {
                EventType = typeof(EventTypeFactory),
                Description = "Default Description"
            });
        }

        public EventDocumentFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EventType

    public class EventTypeFactory : TestDataFactory<EventType>
    {
        static EventTypeFactory()
        {
            Defaults(new {
                Description = "Default Description"
            });
        }

        public EventTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EZPass

    public class VehicleEZPassFactory : TestDataFactory<VehicleEZPass>
    {
        static VehicleEZPassFactory()
        {
            Defaults(new {
                EZPassSerialNumber = "1234567890",
                BillingInfo = "Some guy"
            });
        }

        public VehicleEZPassFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityArea

    public class FacilityAreaFactory : UniqueEntityLookupFactory<FacilityArea>
    {
        public FacilityAreaFactory(IContainer container) : base(container){}
    }

    #endregion

    #region FacilityFacilityArea

    public class FacilityFacilityAreaFactory : TestDataFactory<FacilityFacilityArea>
    {
        static FacilityFacilityAreaFactory()
        {
            Defaults(new {
                Facility = typeof(FacilityFactory),
                FacilityArea = typeof(FacilityAreaFactory),
                FacilitySubArea = typeof(FacilitySubAreaFactory)
            });
        }

        public FacilityFacilityAreaFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilitySubArea

    public class FacilitySubAreaFactory : TestDataFactory<FacilitySubArea>
    {
        public FacilitySubAreaFactory(IContainer container) : base(container){}

        static FacilitySubAreaFactory()
        {
            Defaults(new {
                Description = "Things and junk and things and junk and stuff",
                Area = typeof(FacilityAreaFactory)
            });
        }
    }

    #endregion

    #region FacilityCondition

    public class FacilityConditionFactory : StaticListEntityLookupFactory<FacilityCondition, FacilityConditionFactory>
    {
        public FacilityConditionFactory(IContainer container) : base(container) { }
    }

    public class GoodFacilityConditionFactory : FacilityConditionFactory
    {
        static GoodFacilityConditionFactory()
        {
            Defaults(new {Description = "Good"});
            OnSaving((x, s) => x.Id = FacilityCondition.Indices.GOOD);
        }

        public GoodFacilityConditionFactory(IContainer container) : base(container) { }
    }

    public class AverageFacilityConditionFactory : FacilityConditionFactory
    {
        static AverageFacilityConditionFactory()
        {
            Defaults(new {Description = "Average"});
            OnSaving((x, s) => x.Id = FacilityCondition.Indices.AVERAGE);
        }

        public AverageFacilityConditionFactory(IContainer container) : base(container) { }
    }

    public class PoorFacilityConditionFactory : FacilityConditionFactory
    {
        static PoorFacilityConditionFactory()
        {
            Defaults(new {Description = "Poor"});
            OnSaving((x, s) => x.Id = FacilityCondition.Indices.POOR);
        }

        public PoorFacilityConditionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityPerformance

    public class
        FacilityPerformanceFactory : StaticListEntityLookupFactory<FacilityPerformance, FacilityPerformanceFactory>
    {
        public FacilityPerformanceFactory(IContainer container) : base(container) { }
    }

    public class GoodFacilityPerformanceFactory : FacilityPerformanceFactory
    {
        static GoodFacilityPerformanceFactory()
        {
            Defaults(new {Description = "Good"});
            OnSaving((x, s) => x.Id = FacilityPerformance.Indices.GOOD);
        }

        public GoodFacilityPerformanceFactory(IContainer container) : base(container) { }
    }

    public class AverageFacilityPerformanceFactory : FacilityPerformanceFactory
    {
        static AverageFacilityPerformanceFactory()
        {
            Defaults(new {Description = "Average"});
            OnSaving((x, s) => x.Id = FacilityPerformance.Indices.AVERAGE);
        }

        public AverageFacilityPerformanceFactory(IContainer container) : base(container) { }
    }

    public class PoorFacilityPerformanceFactory : FacilityPerformanceFactory
    {
        static PoorFacilityPerformanceFactory()
        {
            Defaults(new {Description = "Poor"});
            OnSaving((x, s) => x.Id = FacilityPerformance.Indices.POOR);
        }

        public PoorFacilityPerformanceFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityLikelihoodOfFailure

    public class FacilityLikelihoodOfFailureFactory : StaticListEntityLookupFactory<FacilityLikelihoodOfFailure,
        FacilityLikelihoodOfFailureFactory>
    {
        public FacilityLikelihoodOfFailureFactory(IContainer container) : base(container) { }
    }

    public class LowFacilityLikelihoodOfFailureFactory : FacilityLikelihoodOfFailureFactory
    {
        static LowFacilityLikelihoodOfFailureFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((x, s) => x.Id = FacilityLikelihoodOfFailure.Indices.LOW);
        }

        public LowFacilityLikelihoodOfFailureFactory(IContainer container) : base(container) { }
    }

    public class MediumFacilityLikelihoodOfFailureFactory : FacilityLikelihoodOfFailureFactory
    {
        static MediumFacilityLikelihoodOfFailureFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((x, s) => x.Id = FacilityLikelihoodOfFailure.Indices.MEDIUM);
        }

        public MediumFacilityLikelihoodOfFailureFactory(IContainer container) : base(container) { }
    }

    public class HighFacilityLikelihoodOfFailureFactory : FacilityLikelihoodOfFailureFactory
    {
        static HighFacilityLikelihoodOfFailureFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((x, s) => x.Id = FacilityLikelihoodOfFailure.Indices.HIGH);
        }

        public HighFacilityLikelihoodOfFailureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityConsequenceOfFailure

    public class FacilityConsequenceOfFailureFactory : StaticListEntityLookupFactory<FacilityConsequenceOfFailure,
        FacilityConsequenceOfFailureFactory>
    {
        public FacilityConsequenceOfFailureFactory(IContainer container) : base(container) { }
    }

    public class LowFacilityConsequenceOfFailureFactory : FacilityConsequenceOfFailureFactory
    {
        static LowFacilityConsequenceOfFailureFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((x, s) => x.Id = FacilityConsequenceOfFailure.Indices.LOW);
        }

        public LowFacilityConsequenceOfFailureFactory(IContainer container) : base(container) { }
    }

    public class MediumFacilityConsequenceOfFailureFactory : FacilityConsequenceOfFailureFactory
    {
        static MediumFacilityConsequenceOfFailureFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((x, s) => x.Id = FacilityConsequenceOfFailure.Indices.MEDIUM);
        }

        public MediumFacilityConsequenceOfFailureFactory(IContainer container) : base(container) { }
    }

    public class HighFacilityConsequenceOfFailureFactory : FacilityConsequenceOfFailureFactory
    {
        static HighFacilityConsequenceOfFailureFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((x, s) => x.Id = FacilityConsequenceOfFailure.Indices.HIGH);
        }

        public HighFacilityConsequenceOfFailureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityMaintenanceRiskOfFailure

    public class FacilityMaintenanceRiskOfFailureFactory : StaticListEntityLookupFactory<FacilityMaintenanceRiskOfFailure, FacilityMaintenanceRiskOfFailureFactory>
    {
        public FacilityMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityLowMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityLowMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 1,
                Description = "1 - Low Risk Control"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.LOW);
        }
        public FacilityLowMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityLowModMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityLowModMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 2,
                Description = "2 - Low-Moderate Risk"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.LOW_MODERATE);
        }
        public FacilityLowModMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityModMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityModMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 3,
                Description = "3 - Moderate Risk"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.MODERATE);
        }
        public FacilityModMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityModHighMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityModHighMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 4,
                Description = "4 - Moderate-High Risk"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.MODERATE_HIGH);
        }
        public FacilityModHighMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityHighMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityHighMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 6,
                Description = "6 - High Risk"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.HIGH);
        }
        public FacilityHighMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    public class FacilityHighCriticalMaintenanceRiskOfFailureFactory : FacilityMaintenanceRiskOfFailureFactory
    {
        static FacilityHighCriticalMaintenanceRiskOfFailureFactory()
        {
            Defaults(new {
                RiskScore = 9,
                Description = "9 - High-Critical Risk"
            });
            OnSaving((x, s) => x.Id = FacilityMaintenanceRiskOfFailure.Indices.CRITICAL);
        }
        public FacilityHighCriticalMaintenanceRiskOfFailureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityAssetManagementMaintenanceStrategyTier

    public class FacilityAssetManagementMaintenanceStrategyTierFactory : StaticListEntityLookupFactory<
        FacilityAssetManagementMaintenanceStrategyTier, FacilityAssetManagementMaintenanceStrategyTierFactory>
    {
        public FacilityAssetManagementMaintenanceStrategyTierFactory(IContainer container) : base(container) { }
    }

    public class Tier1FacilityAssetManagementMaintenanceStrategyTierFactory :
        FacilityAssetManagementMaintenanceStrategyTierFactory
    {
        static Tier1FacilityAssetManagementMaintenanceStrategyTierFactory()
        {
            Defaults(new {Description = "Tier 1"});
            OnSaving((x, s) => x.Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1);
        }

        public Tier1FacilityAssetManagementMaintenanceStrategyTierFactory(IContainer container) : base(container) { }
    }

    public class Tier2FacilityAssetManagementMaintenanceStrategyTierFactory : 
        FacilityAssetManagementMaintenanceStrategyTierFactory
    {
        static Tier2FacilityAssetManagementMaintenanceStrategyTierFactory()
        {
            Defaults(new {Description = "Tier 2"});
            OnSaving((x, s) => x.Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2);
        }

        public Tier2FacilityAssetManagementMaintenanceStrategyTierFactory(IContainer container) : base(container) { }
    }

    public class Tier3FacilityAssetManagementMaintenanceStrategyTierFactory :
        FacilityAssetManagementMaintenanceStrategyTierFactory
    {
        static Tier3FacilityAssetManagementMaintenanceStrategyTierFactory()
        {
            Defaults(new {Description = "Tier 3"});
            OnSaving((x, s) => x.Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3);
        }

        public Tier3FacilityAssetManagementMaintenanceStrategyTierFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStep

    public class FacilityProcessStepFactory : TestDataFactory<FacilityProcessStep>
    {
        static FacilityProcessStepFactory()
        {
            var idx = 0;
            Func<string> facilityIdFn = () => String.Format("FPSDesc{0}", ++idx);
            Defaults(new {
                ElevationInFeet = 5,
                ProcessTarget = 2m,
                NormalRangeMin = 1.23m,
                NormalRangeMax = 4.56m,
                FacilityProcess = typeof(FacilityProcessFactory),
                FacilityProcessStepSubProcess = typeof(FacilityProcessStepSubProcessFactory),
                UnitOfMeasure = typeof(UnitOfMeasureFactory),
                Description = facilityIdFn
            });
        }

        public FacilityProcessStepFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepTrigger

    public class FacilityProcessStepTriggerFactory : TestDataFactory<FacilityProcessStepTrigger>
    {
        static FacilityProcessStepTriggerFactory()
        {
            var idx = 0;
            Func<string> facilityIdFn = () => String.Format("FPSTDesc{0}", ++idx);
            Defaults(new {
                Description = facilityIdFn,
                FacilityProcessStep = typeof(FacilityProcessStepFactory),
                Sequence = 2,
                TriggerType = typeof(FacilityProcessStepTriggerTypeFactory),
                TriggerLevel = typeof(FacilityProcessStepTriggerLevelFactory),
                Alarm = typeof(FacilityProcessStepAlarmFactory)
            });
        }

        public FacilityProcessStepTriggerFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepTriggerAction

    public class FacilityProcessStepTriggerActionFactory : TestDataFactory<FacilityProcessStepTriggerAction>
    {
        static FacilityProcessStepTriggerActionFactory()
        {
            Defaults(new {
                Trigger = typeof(FacilityProcessStepTriggerFactory),
                Sequence = 2,
                Action = "Some action",
                ActionResponse = "Some action response"
            });
        }

        public FacilityProcessStepTriggerActionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepSubProcess

    public class FacilityProcessStepSubProcessFactory : UniqueEntityLookupFactory<FacilityProcessStepSubProcess>
    {
        public FacilityProcessStepSubProcessFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepTriggerType

    public class FacilityProcessStepTriggerTypeFactory : UniqueEntityLookupFactory<FacilityProcessStepTriggerType>
    {
        public FacilityProcessStepTriggerTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepTriggerLevel

    public class FacilityProcessStepTriggerLevelFactory : UniqueEntityLookupFactory<FacilityProcessStepTriggerLevel>
    {
        public FacilityProcessStepTriggerLevelFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcessStepAlarm

    public class FacilityProcessStepAlarmFactory : UniqueEntityLookupFactory<FacilityProcessStepAlarm>
    {
        public FacilityProcessStepAlarmFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilitySystemDeliveryEntryType

    public class FacilitySystemDeliveryEntryTypeFactory : TestDataFactory<FacilitySystemDeliveryEntryType>
    {
        static FacilitySystemDeliveryEntryTypeFactory()
        {
            Defaults(new {
                Facility = typeof(FacilityFactory),
                SystemDeliveryEntryType = typeof(SystemDeliveryEntryTypeFactory)
            });
        }

        public FacilitySystemDeliveryEntryTypeFactory(IContainer container) : base(container) { }
    }

    #endregion
     
    #region EquipmentGroup

    public class EquipmentGroupFactory : StaticListEntityLookupFactory<EquipmentGroup, EquipmentGroupFactory>
    {
        public EquipmentGroupFactory(IContainer container) : base(container) { }

        #region Private Methods

        protected override EquipmentGroup Save(EquipmentGroup entity)
        {
            // Prevent duplicate Equipment Groups from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<EquipmentGroup>>();
            var existing = repo.GetAll().SingleOrDefault(x => x.Description == entity.Description);
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    public class ElectricalEquipmentGroupFactory : EquipmentGroupFactory
    {
        static ElectricalEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.ELECTRICAL,
                Code = EquipmentGroup.ELECTRICAL_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.ELECTRICAL);
        }

        public ElectricalEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class FlowMeterEquipmentGroupFactory : EquipmentGroupFactory
    {
        static FlowMeterEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.FLOW_METER,
                Code = EquipmentGroup.FLOW_METER_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.FLOW_METER);
        }

        public FlowMeterEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class InstrumentEquipmentGroupFactory : EquipmentGroupFactory
    {
        static InstrumentEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.INSTRUMENT,
                Code = EquipmentGroup.INSTRUMENT_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.INSTRUMENT);
        }

        public InstrumentEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class MechanicalEquipmentGroupFactory : EquipmentGroupFactory
    {
        static MechanicalEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.MECHANICAL,
                Code = EquipmentGroup.MECHANICAL_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.MECHANICAL);
        }

        public MechanicalEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class SafetyEquipmentGroupFactory : EquipmentGroupFactory
    {
        static SafetyEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.SAFETY,
                Code = EquipmentGroup.SAFETY_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.SAFETY);
        }

        public SafetyEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class TankEquipmentGroupFactory : EquipmentGroupFactory
    {
        static TankEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.TANK,
                Code = EquipmentGroup.TANK_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.TANK);
        }

        public TankEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class TreatmentEquipmentGroupFactory : EquipmentGroupFactory
    {
        static TreatmentEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.TREATMENT,
                Code = EquipmentGroup.TREATMENT_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.TREATMENT);
        }

        public TreatmentEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    public class WellEquipmentGroupFactory : EquipmentGroupFactory
    {
        static WellEquipmentGroupFactory()
        {
            Defaults(new {
                Description = EquipmentGroup.WELL,
                Code = EquipmentGroup.WELL_CODE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentGroup.Indices.WELL);
        }

        public WellEquipmentGroupFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentStatus

    /// <summary>
    /// IN_SERVICE = 1, OUT_OF_SERVICE = 2, PENDING = 3, RETIRED = 4, PENDING_RETIREMENT = 5, CANCELLED = 6, FIELD_INSTALLED = 7;
    /// </summary>
    public class EquipmentStatusFactory : StaticListEntityLookupFactory<EquipmentStatus, EquipmentStatusFactory>
    {
        public EquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class InServiceEquipmentStatusFactory : EquipmentStatusFactory
    {
        static InServiceEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.IN_SERVICE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.IN_SERVICE);
        }

        public InServiceEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class OutOfServiceEquipmentStatusFactory : EquipmentStatusFactory
    {
        static OutOfServiceEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.OUT_OF_SERVICE
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.OUT_OF_SERVICE);
        }

        public OutOfServiceEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingEquipmentStatusFactory : EquipmentStatusFactory
    {
        static PendingEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.PENDING
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.PENDING);
        }

        public PendingEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class RetiredEquipmentStatusFactory : EquipmentStatusFactory
    {
        static RetiredEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.RETIRED
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.RETIRED);
        }

        public RetiredEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingRetirementEquipmentStatusFactory : EquipmentStatusFactory
    {
        static PendingRetirementEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.PENDING_RETIREMENT
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.PENDING_RETIREMENT);
        }

        public PendingRetirementEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class CancelledEquipmentStatusFactory : EquipmentStatusFactory
    {
        static CancelledEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.CANCELLED
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.CANCELLED);
        }

        public CancelledEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    public class FieldInstalledEquipmentStatusFactory : EquipmentStatusFactory
    {
        static FieldInstalledEquipmentStatusFactory()
        {
            Defaults(new {
                Description = EquipmentStatus.FIELD_INSTALLED
            });
            OnSaving((a, s) => a.Id = (int)EquipmentStatus.Indices.FIELD_INSTALLED);
            //OnSaving((a,s) => a.Id = EquipmentStatus.Indices.FIELD_INSTALLED);
        }

        public FieldInstalledEquipmentStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentSubCategory

    public class EquipmentSubCategoryFactory : TestDataFactory<EquipmentSubCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EquipmentSubCategory";

        #endregion

        #region Constructors

        static EquipmentSubCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public EquipmentSubCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PurchasedWaterEquipmentSubCategoryFactory : TestDataFactory<EquipmentSubCategory>
    {
        public const string DEFAULT_DESCRIPTION = "Purchased Water";

        static PurchasedWaterEquipmentSubCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((x, s) => x.Id = EquipmentSubCategory.Indices.PURCHASED_WATER);
        }

        public PurchasedWaterEquipmentSubCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentPurpose

    public class EquipmentPurposeFactory : TestDataFactory<EquipmentPurpose>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "EquipmentPurpose",
                            DEFAULT_ABBREVIATION = "ETTT";

        #endregion

        #region Constructors

        static EquipmentPurposeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Abbreviation = DEFAULT_ABBREVIATION
            });
        }

        public EquipmentPurposeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PersonalGasDetectorEquipmentPurposeFactory : EquipmentPurposeFactory
    {
        #region Constructors

        static PersonalGasDetectorEquipmentPurposeFactory()
        {
            Defaults(new {
                Abbreviation = EquipmentPurpose.PERSONAL_GAS_DETECTOR_ABBREVIATION,
            });
        }

        public PersonalGasDetectorEquipmentPurposeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EquipmentLikelyhoodOfFailureRating

    public class EquipmentLikelyhoodOfFailureRatingFactory : StaticListEntityLookupFactory<
        EquipmentLikelyhoodOfFailureRating,
        EquipmentLikelyhoodOfFailureRatingFactory>
    {
        public EquipmentLikelyhoodOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class LowEquipmentLikelyhoodOfFailureRatingFactory : EquipmentLikelyhoodOfFailureRatingFactory
    {
        static LowEquipmentLikelyhoodOfFailureRatingFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((x, s) => x.Id = EquipmentLikelyhoodOfFailureRating.Indices.LOW);
        }

        public LowEquipmentLikelyhoodOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class MediumEquipmentLikelyhoodOfFailureRatingFactory : EquipmentLikelyhoodOfFailureRatingFactory
    {
        static MediumEquipmentLikelyhoodOfFailureRatingFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((x, s) => x.Id = EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM);
        }

        public MediumEquipmentLikelyhoodOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class HighEquipmentLikelyhoodOfFailureRatingFactory : EquipmentLikelyhoodOfFailureRatingFactory
    {
        static HighEquipmentLikelyhoodOfFailureRatingFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((x, s) => x.Id = EquipmentLikelyhoodOfFailureRating.Indices.HIGH);
        }

        public HighEquipmentLikelyhoodOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentConsequencesOfFailureRating

    public class EquipmentConsequencesOfFailureRatingFactory : StaticListEntityLookupFactory<
        EquipmentConsequencesOfFailureRating,
        EquipmentConsequencesOfFailureRatingFactory>
    {
        public EquipmentConsequencesOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class LowEquipmentConsequencesOfFailureRatingFactory : EquipmentConsequencesOfFailureRatingFactory
    {
        static LowEquipmentConsequencesOfFailureRatingFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((x, s) => x.Id = EquipmentConsequencesOfFailureRating.Indices.LOW);
        }

        public LowEquipmentConsequencesOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class MediumEquipmentConsequencesOfFailureRatingFactory : EquipmentConsequencesOfFailureRatingFactory
    {
        static MediumEquipmentConsequencesOfFailureRatingFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((x, s) => x.Id = EquipmentConsequencesOfFailureRating.Indices.MEDIUM);
        }

        public MediumEquipmentConsequencesOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    public class HighEquipmentConsequencesOfFailureRatingFactory : EquipmentConsequencesOfFailureRatingFactory
    {
        static HighEquipmentConsequencesOfFailureRatingFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((x, s) => x.Id = EquipmentConsequencesOfFailureRating.Indices.HIGH);
        }

        public HighEquipmentConsequencesOfFailureRatingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentFailureRiskRating

    public class EquipmentFailureRiskRatingFactory : StaticListEntityLookupFactory<EquipmentFailureRiskRating,
        EquipmentFailureRiskRatingFactory>
    {
        public EquipmentFailureRiskRatingFactory(IContainer container) : base(container) { }
    }

    public class LowEquipmentFailureRiskRatingFactory : EquipmentFailureRiskRatingFactory
    {
        static LowEquipmentFailureRiskRatingFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((x, s) => x.Id = EquipmentFailureRiskRating.Indices.LOW);
        }

        public LowEquipmentFailureRiskRatingFactory(IContainer container) : base(container) { }
    }

    public class MediumEquipmentFailureRiskRatingFactory : EquipmentFailureRiskRatingFactory
    {
        static MediumEquipmentFailureRiskRatingFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((x, s) => x.Id = EquipmentFailureRiskRating.Indices.MEDIUM);
        }

        public MediumEquipmentFailureRiskRatingFactory(IContainer container) : base(container) { }
    }

    public class HighEquipmentFailureRiskRatingFactory : EquipmentFailureRiskRatingFactory
    {
        static HighEquipmentFailureRiskRatingFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((x, s) => x.Id = EquipmentFailureRiskRating.Indices.HIGH);
        }

        public HighEquipmentFailureRiskRatingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EquipmentPerformanceRating

    public class EquipmentPerformanceRatingFactory : StaticListEntityLookupFactory<EquipmentPerformanceRating,
        EquipmentPerformanceRatingFactory>
    {
        public EquipmentPerformanceRatingFactory(IContainer container) : base(container) { }
    }

    public class PoorEquipmentPerformanceRatingFactory : EquipmentPerformanceRatingFactory
    {
        static PoorEquipmentPerformanceRatingFactory()
        {
            Defaults(new {Description = "Poor"});
            OnSaving((x, s) => x.Id = EquipmentPerformanceRating.Indices.POOR);
        }

        public PoorEquipmentPerformanceRatingFactory(IContainer container) : base(container) { }
    }

    public class AverageEquipmentPerformanceRatingFactory : EquipmentPerformanceRatingFactory
    {
        static AverageEquipmentPerformanceRatingFactory()
        {
            Defaults(new {Description = "Average"});
            OnSaving((x, s) => x.Id = EquipmentPerformanceRating.Indices.AVERAGE);
        }

        public AverageEquipmentPerformanceRatingFactory(IContainer container) : base(container) { }
    }

    public class GoodEquipmentPerformanceRatingFactory : EquipmentPerformanceRatingFactory
    {
        static GoodEquipmentPerformanceRatingFactory()
        {
            Defaults(new {Description = "Good"});
            OnSaving((x, s) => x.Id = EquipmentPerformanceRating.Indices.GOOD);
        }

        public GoodEquipmentPerformanceRatingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region EventExposureTypeFactory

    public class EventExposureTypeFactory : UniqueEntityLookupFactory<EventExposureType>
    {
        #region Constructors

        public EventExposureTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EstimatingProject

    public class PermitTypeFactory : EntityLookupTestDataFactory<PermitType>
    {
        public PermitTypeFactory(IContainer container) : base(container) { }
    }

    public class EstimatingProjectPermitFactory : TestDataFactory<EstimatingProjectPermit>
    {
        public EstimatingProjectPermitFactory(IContainer container) : base(container) { }

        static EstimatingProjectPermitFactory()
        {
            Defaults(new {
                EstimatingProject = typeof(EstimatingProjectFactory),
                PermitType = typeof(PermitTypeFactory)
            });
        }
    }

    public class EstimatingProjectContractorLaborCostFactory : TestDataFactory<EstimatingProjectContractorLaborCost>
    {
        public EstimatingProjectContractorLaborCostFactory(IContainer container) : base(container) { }

        static EstimatingProjectContractorLaborCostFactory()
        {
            Defaults(new {
                ContractorLaborCost = typeof(ContractorLaborCostFactory),
                EstimatingProject = typeof(EstimatingProjectFactory),
                AssetType = typeof(ValveAssetTypeFactory),
                Quantity = 1
            });
        }
    }

    public class EstimatingProjectCompanyLaborCostFactory : TestDataFactory<EstimatingProjectCompanyLaborCost>
    {
        public EstimatingProjectCompanyLaborCostFactory(IContainer container) : base(container) { }

        static EstimatingProjectCompanyLaborCostFactory()
        {
            Defaults(new {
                CompanyLaborCost = typeof(CompanyLaborCostFactory),
                EstimatingProject = typeof(EstimatingProjectFactory),
                AssetType = typeof(HydrantAssetTypeFactory)
            });
        }
    }

    public class EstimatingProjectMaterialFactory : TestDataFactory<EstimatingProjectMaterial>
    {
        public EstimatingProjectMaterialFactory(IContainer container) : base(container) { }

        static EstimatingProjectMaterialFactory()
        {
            Defaults(new {
                Material = typeof(MaterialFactory),
                EstimatingProject = typeof(EstimatingProjectFactory),
                AssetType = typeof(ValveAssetTypeFactory),
                Quantity = 1
            });
        }
    }

    public class EstimatingProjectOtherCostFactory : TestDataFactory<EstimatingProjectOtherCost>
    {
        public EstimatingProjectOtherCostFactory(IContainer container) : base(container) { }

        static EstimatingProjectOtherCostFactory()
        {
            Defaults(new {
                EstimatingProject = typeof(EstimatingProjectFactory),
                Description = "Some description",
                AssetType = typeof(HydrantAssetTypeFactory)
            });
        }
    }

    public class EstimatingProjectTypeFactory : EntityLookupTestDataFactory<EstimatingProjectType>
    {
        public EstimatingProjectTypeFactory(IContainer container) : base(container) { }
    }

    public class EstimatingProjectFactory : TestDataFactory<EstimatingProject>
    {
        public EstimatingProjectFactory(IContainer container) : base(container) { }

        static EstimatingProjectFactory()
        {
            var i = 1;
            Func<string> projectNumberFn = () => (i++).ToString();
            Func<string> descriptionFn = () => String.Format("Project {0}", i);
            Func<string> streetFn = () => String.Format("{0} Street", descriptionFn());

            Defaults(new {
                ProjectType = typeof(EstimatingProjectTypeFactory),
                ProjectNumber = projectNumberFn,
                ProjectName = descriptionFn,
                Description = descriptionFn,
                Street = streetFn,
                Estimator = typeof(EmployeeFactory),
                EstimateDate = DateTime.Now,
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                OverheadPercentage = 10,
                ContingencyPercentage = 10,
                LumpSum = 0m
            });
        }
    }

    #endregion

    #region FacilityFactory

    public class FacilityFactory : TestDataFactory<Facility>
    {
        static FacilityFactory()
        {
            int idx = 0;
            Func<string> facilityIdFn = () => String.Format("NJSB-{0}", ++idx);
            Func<string> nameFn = () => "Facility " + idx;
            Defaults(new {
                FacilityId = facilityIdFn,
                FacilityName = nameFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                Coordinate = typeof(CoordinateFactory),
                Department = typeof(DepartmentFactory),
                FacilityStatus = typeof(ActiveFacilityStatusFactory),
                UsedInProductionCapacityCalculation = false,
                ArcFlashStudies = new List<ArcFlashStudy>(),
                ChemicalStorageLocation = typeof(ChemicalStorageLocationFactory),
                FunctionalLocation = "Oz"
            });
        }

        public FacilityFactory(IContainer container) : base(container) { }
    }

    public class FacilityKwhCostFactory : TestDataFactory<FacilityKwhCost>
    {
        public FacilityKwhCostFactory(IContainer container) : base(container) { }

        static FacilityKwhCostFactory()
        {
            int idx = 0;
            Func<decimal> costPerKwhFn = () => (++idx) * 1.11m;
            Defaults(new {
                Facility = typeof(FacilityFactory),
                CostPerKwh = costPerKwhFn,
                StartDate = Lambdas.GetNow,
                EndDate = Lambdas.GetNow
            });
        }
    }

    #endregion

    #region FacilityOwner

    public class FacilityOwnerFactory : StaticListEntityLookupFactory<FacilityOwner, FacilityOwnerFactory>
    {
        public FacilityOwnerFactory(IContainer container) : base(container) { }
    }

    public class AWFacilityOwnerFactory : FacilityOwnerFactory
    {
        public const string DESCRIPTION = "American Water";
        static AWFacilityOwnerFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });

            OnSaving((a, s) => a.Id = (int)FacilityOwner.Indices.AMERICAN_WATER);
        }

        public AWFacilityOwnerFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FacilityProcess

    public class FacilityProcessFactory : TestDataFactory<FacilityProcess>
    {
        #region Constructors

        static FacilityProcessFactory()
        {
            Defaults(new {
                Facility = typeof(FacilityFactory),
                Process = typeof(ProcessFactory)
            });
        }

        public FacilityProcessFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FacilityStatus

    public class FacilityStatusFactory : StaticListEntityLookupFactory<FacilityStatus, FacilityStatusFactory>
    {
        public FacilityStatusFactory(IContainer container) : base(container) { }
    }

    public class ActiveFacilityStatusFactory : FacilityStatusFactory
    {
        public const string DESCRIPTION = "Active";

        static ActiveFacilityStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)FacilityStatus.Indices.ACTIVE);
        }

        public ActiveFacilityStatusFactory(IContainer container) : base(container) { }
    }

    public class InactiveFacilityStatusFactory : FacilityStatusFactory
    {
        public const string DESCRIPTION = "Inactive";

        static InactiveFacilityStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)FacilityStatus.Indices.INACTIVE);
        }

        public InactiveFacilityStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingFacilityStatusFactory : FacilityStatusFactory
    {
        public const string DESCRIPTION = "Pending";

        static PendingFacilityStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = FacilityStatus.Indices.PENDING);
        }

        public PendingFacilityStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingRetirementFacilityStatusFactory : FacilityStatusFactory
    {
        public const string DESCRIPTION = "pending_retirement";

        static PendingRetirementFacilityStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = FacilityStatus.Indices.PENDING_RETIREMENT);
        }

        public PendingRetirementFacilityStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region FamilyMedicalLeaveActCase

    public class FamilyMedicalLeaveActCaseFactory : TestDataFactory<FamilyMedicalLeaveActCase>
    {
        #region Constructors

        static FamilyMedicalLeaveActCaseFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                CertificationExtended = false,
                SendPackage = false,
                ChronicCondition = false
            });
        }

        public FamilyMedicalLeaveActCaseFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FEMAFloodRating

    public class FEMAFloodRatingFactory : TestDataFactory<FEMAFloodRating>
    {
        #region Constants

        public const string DESCRIPTION = "Y*";

        #endregion

        #region Constructors

        static FEMAFloodRatingFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }

        public FEMAFloodRatingFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FilterMedia

    public class FilterMediaFilterTypeFactory : TestDataFactory<FilterMediaFilterType>
    {
        public const string DESCRIPTION = "Some Kinda Filter";

        public FilterMediaFilterTypeFactory(IContainer container) : base(container) { }

        static FilterMediaFilterTypeFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class FilterMediaLevelControlMethodFactory : TestDataFactory<FilterMediaLevelControlMethod>
    {
        public const string DESCRIPTION = "Some Kinda Way of Controlling Levels";

        public FilterMediaLevelControlMethodFactory(IContainer container) : base(container) { }

        static FilterMediaLevelControlMethodFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class FilterMediaWashTypeFactory : TestDataFactory<FilterMediaWashType>
    {
        public const string DESCRIPTION = "Some Kinda Wash";

        public FilterMediaWashTypeFactory(IContainer container) : base(container) { }

        static FilterMediaWashTypeFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class FilterMediaTypeFactory : TestDataFactory<FilterMediaType>
    {
        public const string DESCRIPTION = "Some Kinda Media";

        public FilterMediaTypeFactory(IContainer container) : base(container) { }

        static FilterMediaTypeFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class FilterMediaLocationFactory : TestDataFactory<FilterMediaLocation>
    {
        public const string DESCRIPTION = "Could've sworn it was here somewhere...";

        public FilterMediaLocationFactory(IContainer container) : base(container) { }

        static FilterMediaLocationFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class FilterMediaFactory : TestDataFactory<FilterMedia>
    {
        public FilterMediaFactory(IContainer container) : base(container) { }

        static FilterMediaFactory()
        {
            Defaults(new {
                Equipment = typeof(EquipmentFactory),
                FilterType = typeof(FilterMediaFilterTypeFactory),
                LevelControlMethod = typeof(FilterMediaLevelControlMethodFactory),
                WashType = typeof(FilterMediaWashTypeFactory),
                MediaType = typeof(FilterMediaTypeFactory),
                Location = typeof(FilterMediaLocationFactory)
            });
        }
    }

    #endregion

    #region FireDistrict

    public class FireDistrictFactory : TestDataFactory<FireDistrict>
    {
        public const string ADDRESS = "default address",
                            ADDRESS_CITY = "default city",
                            ADDRESS_ZIP = "12345",
                            CONTACT = "default contact",
                            DISTRICT_NAME = "default district name",
                            FAX = "default fax",
                            PHONE = "default phone",
                            ABBREVIATION = "da",
                            UTILITY_NAME = "default utility name",
                            PREMISE_NUMBER = "987654321";

        public const int UTILITY_DISTRICT = 123;

        public FireDistrictFactory(IContainer container) : base(container) { }

        static FireDistrictFactory()
        {
            Defaults(new {
                Address = ADDRESS,
                AddressCity = ADDRESS_CITY,
                AddressZip = ADDRESS_ZIP,
                Contact = CONTACT,
                DistrictName = DISTRICT_NAME,
                Fax = FAX,
                Phone = PHONE,
                Abbreviation = ABBREVIATION,
                PremiseNumber = PREMISE_NUMBER,
                UtilityName = UTILITY_NAME,
                UtilityDistrict = UTILITY_DISTRICT,
                State = typeof(StateFactory)
            });
        }
    }

    #endregion

    #region FireDistrictTown

    public class FireDistrictTownFactory : TestDataFactory<FireDistrictTown>
    {
        public FireDistrictTownFactory(IContainer container) : base(container) { }

        static FireDistrictTownFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                Town = typeof(TownFactory),
                FireDistrict = typeof(FireDistrictFactory)
            });
        }
    }

    #endregion

    #region FoundationalFilingPeriod

    public class FoundationalFilingPeriodFactory : TestDataFactory<FoundationalFilingPeriod>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "FoundationalFilingPeriod";

        #endregion

        #region Constructors

        static FoundationalFilingPeriodFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public FoundationalFilingPeriodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FuelType

    public class FuelTypeFactory : TestDataFactory<FuelType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "FuelType";

        #endregion

        #region Constructors

        static FuelTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public FuelTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region FunctionalLocation

    public class FunctionalLocationFactory : TestDataFactory<FunctionalLocation>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "FunctionalLocation";

        #endregion

        #region Constructors

        static FunctionalLocationFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                IsActive = true
            });
        }

        public FunctionalLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EquipmentFunctionalLocationFactory : TestDataFactory<FunctionalLocation>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "FunctionalLocation";

        #endregion

        #region Constructors

        static EquipmentFunctionalLocationFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                AssetType = typeof(EquipmentAssetTypeFactory),
                IsActive = true
            });
        }

        public EquipmentFunctionalLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SewerMainFunctionalLocationFactory : TestDataFactory<FunctionalLocation>
    {
        public const string DEFAULT_DESCRIPTION = "FunctionalLocation";

        static SewerMainFunctionalLocationFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                AssetType = typeof(SewerMainAssetTypeFactory),
                IsActive = true
            });
        }

        public SewerMainFunctionalLocationFactory(IContainer container) : base(container) { }
    }

    public class SewerOpeningFunctionalLocationFactory : TestDataFactory<FunctionalLocation>
    {
        public const string DEFAULT_DESCRIPTION = "FunctionalLocation";

        static SewerOpeningFunctionalLocationFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                AssetType = typeof(SewerOpeningAssetTypeFactory),
                IsActive = true
            });
        }

        public SewerOpeningFunctionalLocationFactory(IContainer container) : base(container) { }
    }

    public class ValveFunctionalLocationFactory : TestDataFactory<FunctionalLocation>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "FunctionalLocation";

        #endregion

        #region Constructors

        static ValveFunctionalLocationFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                AssetType = typeof(ValveAssetTypeFactory),
                IsActive = true
            });
        }

        public ValveFunctionalLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GasMonitor

    public class GasMonitorFactory : TestDataFactory<GasMonitor>
    {
        static GasMonitorFactory()
        {
            Defaults(new {
                CalibrationFrequencyDays = 3,
                Equipment = typeof(GasMonitorEquipmentFactory)
            });
        }

        public GasMonitorFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region GasMonitorCalibration

    public class GasMonitorCalibrationFactory : TestDataFactory<GasMonitorCalibration>
    {
        static GasMonitorCalibrationFactory()
        {
            Defaults(new {
                GasMonitor = typeof(GasMonitorFactory),
                CalibrationDate = DateTime.Now,
                CalibrationPassed = true,
                CreatedBy = typeof(UserFactory),
                CreatedAt = DateTime.Now
            });
        }

        public GasMonitorCalibrationFactory(IContainer container) : base(container) { }

        public override GasMonitorCalibration Create(object overrides = null)
        {
            var gmc = base.Create(overrides);
            if (!gmc.GasMonitor.Calibrations.Contains(gmc))
            {
                gmc.GasMonitor.Calibrations.Add(gmc);
            }

            return gmc;
        }
    }

    #endregion

    #region GeneralLiabilityClaim

    public class GeneralLiabilityClaimFactory : TestDataFactory<GeneralLiabilityClaim>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "GeneralLiabilityClaim",
                            DEFAULT_CLAIM_NUMBER = "12345",
                            DEFAULT_REPORTED_BY = "Hall Monitor",
                            CREATED_BY = "Factory Test";

        #endregion

        #region Constructors

        static GeneralLiabilityClaimFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Coordinate = typeof(CoordinateFactory),
                CompanyContact = typeof(EmployeeFactory),
                ClaimsRepresentative = typeof(ClaimsRepresentativeFactory),
                ClaimNumber = DEFAULT_CLAIM_NUMBER,
                Description = DEFAULT_DESCRIPTION,
                ReportedBy = DEFAULT_REPORTED_BY,
                LiabilityType = typeof(LiabilityTypeFactory),
                GeneralLiabilityClaimType = typeof(GeneralLiabilityClaimTypeFactory),
                CrashType = typeof(CrashTypeFactory),
                CreatedBy = CREATED_BY,
                CreatedAt = Lambdas.GetNow,
                IncidentDateTime = Lambdas.GetNow,
                IncidentNotificationDate = Lambdas.GetNow,
                IncidentReportedDate = Lambdas.GetYesterday
            });
        }

        public GeneralLiabilityClaimFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GeneralLiabilityClaimType

    public class GeneralLiabilityClaimTypeFactory : StaticListEntityLookupFactory<GeneralLiabilityClaimType, GeneralLiabilityClaimTypeFactory>
    {
        #region Constructors

        public GeneralLiabilityClaimTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PreventableGeneralLiabilityClaimTypeFactory : GeneralLiabilityClaimTypeFactory
    {
        #region Constructors

        static PreventableGeneralLiabilityClaimTypeFactory()
        {
            Defaults(new {
                Description = "preventable"
            });
            OnSaving((a, s) => a.Id = (int)GeneralLiabilityClaimType.Indices.PREVENTABLE);
        }

        public PreventableGeneralLiabilityClaimTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class NonPreventableGeneralLiabilityClaimTypeFactory : GeneralLiabilityClaimTypeFactory
    {
        #region Constructors

        static NonPreventableGeneralLiabilityClaimTypeFactory()
        {
            Defaults(new {
                Description = "non-preventable"
            });
            OnSaving((a, s) => a.Id = (int)GeneralLiabilityClaimType.Indices.NONPREVENTABLE);
        }

        public NonPreventableGeneralLiabilityClaimTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GeneralLiabilityCode

    public class GeneralLiabilityCodeFactory : UniqueEntityLookupFactory<GeneralLiabilityCode>
    {
        #region Constructors

        public GeneralLiabilityCodeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Generator

    public class GeneratorFactory : TestDataFactory<Generator>
    {
        #region Constructors

        static GeneratorFactory()
        {
            Defaults(new {
                Equipment = typeof(EquipmentFactory)
            });
        }

        public GeneratorFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GISLayerUpdateFactory

    public class GISLayerUpdateFactory : TestDataFactory<GISLayerUpdate>
    {
        static GISLayerUpdateFactory()
        {
            var i = 0;
            Func<string> mapIdFn = () => string.Format("Map{0}", i);

            Defaults(new {
                CreatedBy = typeof(UserFactory),
                MapId = mapIdFn,
                Updated = Lambdas.GetNow
            });
        }

        public GISLayerUpdateFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region GradientFactory

    public class GradientFactory : UniqueEntityLookupFactory<Gradient>
    {
        #region Constructors

        public GradientFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GrievanceFactory

    public class GrievanceFactory : TestDataFactory<Grievance>
    {
        static GrievanceFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                DateReceived = Lambdas.GetNowDate
            });
        }

        public GrievanceFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HazardType

    public class HazardTypeFactory : UniqueEntityLookupFactory<HazardType>
    {
        public HazardTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region GrievanceCategorization

    public class GrievanceCategorizationFactory : TestDataFactory<GrievanceCategorization>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "GrievanceCategorization";

        #endregion

        #region Constructors

        static GrievanceCategorizationFactory()
        {
            Defaults(new {
                GrievanceCategory = typeof(GrievanceCategoryFactory),
                Description = DEFAULT_DESCRIPTION
            });
        }

        public GrievanceCategorizationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GrievanceCategory

    public class GrievanceCategoryFactory : TestDataFactory<GrievanceCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "GrievanceCategory";

        #endregion

        #region Constructors

        static GrievanceCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public GrievanceCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region HelpTopicFactory

    public class HelpCategoryFactory : EntityLookupTestDataFactory<HelpCategory>
    {
        public HelpCategoryFactory(IContainer container) : base(container) { }
    }

    public class HelpTopicFactory : TestDataFactory<HelpTopic>
    {
        public HelpTopicFactory(IContainer container) : base(container) { }

        static HelpTopicFactory()
        {
            var i = 0;
            Func<string> titleFn = () => String.Format("Help Topic {0}", ++i);
            Defaults(new {
                Title = titleFn,
                Description = "Some description of the topic at hand",
                Category = typeof(HelpCategoryFactory),
                SubjectMatter = typeof(EntityLookupTestDataFactory<HelpTopicSubjectMatter>)
            });
        }
    }

    #endregion

    #region HepatitisBVaccineStatus

    public class HepatitisBVaccineStatusFactory : TestDataFactory<HepatitisBVaccineStatus>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "HepatitisBVaccineStatus";

        #endregion

        #region Constructors

        static HepatitisBVaccineStatusFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public HepatitisBVaccineStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region HepatitisBVacciation

    public class HepatitisBVaccinationFactory : TestDataFactory<HepatitisBVaccination>
    {
        #region Constructors

        static HepatitisBVaccinationFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                HepatitisBVaccineStatus = typeof(HepatitisBVaccineStatusFactory),
                ResponseDate = Lambdas.GetYesterday
            });
        }

        public HepatitisBVaccinationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region HighCostFactor

    public class HighCostFactorFactory : TestDataFactory<HighCostFactor>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "HighCostFactor", CREATED_BY = "Factory Test";

        #endregion

        #region Constructors

        static HighCostFactorFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public HighCostFactorFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Hydrant

    public class HydrantFactory : TestDataFactory<Hydrant>
    {
        #region Constants

        public const string DEFAULT_HYDRANT_NUMBER = "HAB-1";
        public const int DEFAULT_HYDRANT_SUFFIX = 1;

        #endregion

        #region Constructors

        static HydrantFactory()
        {
            Defaults(new {
                BillingDate = DateTime.Now,
                Coordinate = typeof(CoordinateFactory),
                DateInstalled = DateTime.Today,
                HydrantNumber = DEFAULT_HYDRANT_NUMBER,
                HydrantSuffix = DEFAULT_HYDRANT_SUFFIX,
                HydrantSize = typeof(HydrantSizeFactory),
                HydrantMainSize = typeof(HydrantMainSizeFactory),
                Status = typeof(ActiveAssetStatusFactory),
                HydrantBilling = typeof(PublicHydrantBillingFactory),
                HydrantManufacturer = typeof(HydrantManufacturerFactory),
                LateralSize = typeof(LateralSizeFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                FireDistrict = typeof(FireDistrictFactory),
                Street = typeof(StreetFactory),
                Town = typeof(TownFactory),
                SAPEquipmentId = 123456,
                FunctionalLocation = typeof(FunctionalLocationFactory),
                WorkOrderNumber = "work order number",
                HasOpenWorkOrder = true
            });
        }

        public HydrantFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region HydrantBilling

    public class HydrantBillingFactory : StaticListEntityLookupFactory<HydrantBilling, HydrantBillingFactory>
    {
        public HydrantBillingFactory(IContainer container) : base(container) { }
    }

    public class PublicHydrantBillingFactory : HydrantBillingFactory
    {
        static PublicHydrantBillingFactory()
        {
            Defaults(new {Description = "Public"});
            OnSaving((a, s) => a.Id = HydrantBilling.Indices.PUBLIC);
        }

        public PublicHydrantBillingFactory(IContainer container) : base(container) { }
    }

    public class PrivateHydrantBillingFactory : HydrantBillingFactory
    {
        static PrivateHydrantBillingFactory()
        {
            Defaults(new {Description = "Private"});
            OnSaving((a, s) => a.Id = (int)HydrantBilling.Indices.PRIVATE);
        }

        public PrivateHydrantBillingFactory(IContainer container) : base(container) { }
    }

    public class MunicipalHydrantBillingFactory : HydrantBillingFactory
    {
        static MunicipalHydrantBillingFactory()
        {
            Defaults(new {Description = "Municipal"});
            OnSaving((a, s) => a.Id = HydrantBilling.Indices.MUNICIPAL);
        }

        public MunicipalHydrantBillingFactory(IContainer container) : base(container) { }
    }

    public class CompanyHydrantBillingFactory : HydrantBillingFactory
    {
        static CompanyHydrantBillingFactory()
        {
            Defaults(new {Description = "Company"});
            OnSaving((a, s) => a.Id = HydrantBilling.Indices.COMPANY);
        }

        public CompanyHydrantBillingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantInspection

    public class HydrantInspectionFactory : TestDataFactory<HydrantInspection>
    {
        #region Constructors

        static HydrantInspectionFactory()
        {
            Defaults(new {
                Hydrant = typeof(HydrantFactory),
                DateInspected = Lambdas.GetNow,
                InspectedBy = typeof(UserFactory)
            });
        }

        public HydrantInspectionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region HydrantInspectionType

    public class HydrantInspectionTypeFactory : UniqueEntityLookupFactory<HydrantInspectionType>
    {
        public HydrantInspectionTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantMainSize

    public class HydrantMainSizeFactory : UniqueEntityLookupFactory<HydrantMainSize>
    {
        public HydrantMainSizeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantManufacturer

    public class HydrantManufacturerFactory : UniqueEntityLookupFactory<HydrantManufacturer>
    {
        public HydrantManufacturerFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantModel

    public class HydrantModelFactory : UniqueEntityLookupFactory<HydrantModel>
    {
        static HydrantModelFactory()
        {
            Defaults(new {
                HydrantManufacturer = typeof(HydrantManufacturerFactory)
            });
        }

        public HydrantModelFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantOutOfService

    public class HydrantOutOfServiceFactory : TestDataFactory<HydrantOutOfService>
    {
        #region Constructors

        static HydrantOutOfServiceFactory()
        {
            Defaults(new {
                Hydrant = typeof(HydrantFactory),
                CreatedAt = DateTime.Now,
                OutOfServiceDate = DateTime.Now,
                OutOfServiceByUser = typeof(UserFactory)
            });
        }

        public HydrantOutOfServiceFactory(IContainer container) : base(container) { }

        #endregion

        public override HydrantOutOfService Build(object overrides = null)
        {
            var hoos = base.Build(overrides);

            if (!hoos.Hydrant.OutOfServiceRecords.Contains(hoos))
            {
                hoos.Hydrant.OutOfServiceRecords.Add(hoos);
            }

            return hoos;
        }
    }

    #endregion
    
    #region HydrantPainting

    public class HydrantPaintingFactory : TestDataFactory<HydrantPainting>
    {
        static HydrantPaintingFactory()
        {
            Defaults(new {
                PaintedAt = DateTime.Now,
                Hydrant = typeof(HydrantFactory),
                CreatedAt = DateTime.Now,
                CreatedBy = typeof(UserFactory),
                UpdatedAt = DateTime.Now,
                UpdatedBy = typeof(UserFactory)
            });
        }

        public HydrantPaintingFactory(IContainer container) : base(container) { }
    }
    
    #endregion

    #region HydrantProblem

    public class HydrantProblemFactory : UniqueEntityLookupFactory<HydrantProblem>
    {
        public HydrantProblemFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region HydrantSize

    public class HydrantSizeFactory : UniqueEntityLookupFactory<HydrantSize>
    {
        public HydrantSizeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region IconSet

    public class IconSetFactory : TestDataFactory<IconSet>
    {
        #region Consts

        private const IconSets DEFAULT_ID = IconSets.Miscellaneous;

        #endregion

        #region Constructors

        static IconSetFactory()
        {
            Defaults(new {
                Id = DEFAULT_ID,
            });
        }

        public IconSetFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override IconSet Save(IconSet entity)
        {
            var repo = _container.GetInstance<RepositoryBase<IconSet>>();
            var existing = repo.Find(entity.Id);
            return existing ?? base.Save(entity);
        }

        #endregion

        #region Exposed Methods

        public override IconSet Build(object overrides = null)
        {
            var id = (overrides != null ? (IconSets)overrides.GetPropertyValueByName("Id") : DEFAULT_ID);
            var app = base.Build(overrides);
            app.Id = (int)id;
            if (string.IsNullOrEmpty(app.Name))
            {
                app.Name = id.ToString(); // enum name
            }

            return app;
        }

        #endregion
    }

    #endregion

    #region IncidentFactory

    public class IncidentFactory : TestDataFactory<Incident>
    {
        #region Constants

        public const string CREATED_BY = "IncidentFactory";

        #endregion

        #region Constructors

        static IncidentFactory()
        {
            Defaults(new {
                AccidentTown = typeof(TownFactory),
                AccidentCoordinate = typeof(CoordinateFactory),
                AccidentStreetName = "Rainbow Rd",
                AccidentStreetNumber = "24",
                CreatedAt = DateTime.Today,
                CreatedBy = CREATED_BY,
                DrugAndAlcoholTestingDecision = typeof(IncidentDrugAndAlcoholTestingDecisionFactory),
                DrugAndAlcoholTestingResult = typeof(IncidentDrugAndAlcoholTestingResultFactory),
                DrugAndAlcoholTestingNotes = "DrugAndAlcoholTestingNotes",
                Employee = typeof(ActiveEmployeeFactory),
                EmployeeSpokeWithNurse = typeof(EmployeeSpokeWithNurseFactory),
                EmployeeType = typeof(EmployeeTypeFactory),
                EventExposureType = typeof(EventExposureTypeFactory),
                IncidentSummary = "Notes about Incident",
                Facility = typeof(FacilityFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                IncidentClassification = typeof(IncidentClassificationFactory),
                IncidentDate = DateTime.Today,
                IncidentReportedDate = DateTime.Today,
                IncidentShift = typeof(IncidentShiftFactory),
                IncidentStatus = typeof(IncidentStatusFactory),
                IncidentType = typeof(IncidentTypeFactory),
                IsOvertime = false,
                Supervisor = typeof(EmployeeFactory),
                WorkersCompensationClaimStatus = typeof(OpenWorkersCompensationClaimStatusFactory),
                ClaimsCarrierId = "Testing Claims Carrier Id",
                QuestionEmployeeDoingBeforeIncidentOccurred = "Transporting GC-161 to secret lab.",
                QuestionWhatHappened =
                    "Some kid ran in front of my truck while I was reaching for my sandwich. I crashed.",
                QuestionInjuryOrIllness =
                    "The kid got covered in GC-161. They started turning into puddles of water and stuff.",
                QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee = "GC-161. I already said that."
            });
        }

        public IncidentFactory(IContainer container) : base(container) { }

        #endregion

        protected override Incident Save(Incident entity)
        {
            if (entity.Position == null)
            {
                var position = new TestDataFactory<Position>(_container).Create();
                entity.Position = position;
            }

            return base.Save(entity);
        }
    }

    #endregion

    #region IncidentClassifcationFactory

    public class IncidentClassificationFactory : UniqueEntityLookupFactory<IncidentClassification>
    {
        #region Constructors

        public IncidentClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentDrugAndAlcoholTestingDecisionFactory

    public class IncidentDrugAndAlcoholTestingDecisionFactory :
        UniqueEntityLookupFactory<IncidentDrugAndAlcoholTestingDecision>
    {
        #region Constructors

        public IncidentDrugAndAlcoholTestingDecisionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentDrugAndAlcoholTestingResultFactory

    public class IncidentDrugAndAlcoholTestingResultFactory :
        UniqueEntityLookupFactory<IncidentDrugAndAlcoholTestingResult>
    {
        #region Constructors

        public IncidentDrugAndAlcoholTestingResultFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentEmployeeAvailabilityType

    public class IncidentEmployeeAvailabilityTypeFactory : StaticListEntityLookupFactory<
        IncidentEmployeeAvailabilityType, IncidentEmployeeAvailabilityTypeFactory>
    {
        public IncidentEmployeeAvailabilityTypeFactory(IContainer container) : base(container) { }
    }

    public class LostTimeIncidentEmployeeAvailabilityTypeFactory : IncidentEmployeeAvailabilityTypeFactory
    {
        static LostTimeIncidentEmployeeAvailabilityTypeFactory()
        {
            Defaults(new {Description = "Lost Time"});
            OnSaving((a, s) => a.Id = (int)IncidentEmployeeAvailabilityType.Indices.LOST_TIME);
        }

        public LostTimeIncidentEmployeeAvailabilityTypeFactory(IContainer container) : base(container) { }
    }

    public class RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory : IncidentEmployeeAvailabilityTypeFactory
    {
        static RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory()
        {
            Defaults(new {Description = "Restrictive Duty"});
            OnSaving((a, s) => a.Id = (int)IncidentEmployeeAvailabilityType.Indices.RESTRICTIVE_DUTY);
        }

        public RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region IncidentShiftFactory

    public class IncidentShiftFactory : UniqueEntityLookupFactory<IncidentShift>
    {
        #region Constructors

        public IncidentShiftFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentStatus

    public class IncidentStatusFactory : UniqueEntityLookupFactory<IncidentStatus>
    {
        public IncidentStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region WorkersCompensationClaimStatus

    public class WorkersCompensationClaimStatusFactory : StaticListEntityLookupFactory<WorkersCompensationClaimStatus, WorkersCompensationClaimStatusFactory>
    {
        public WorkersCompensationClaimStatusFactory(IContainer container) : base(container) { }
    }
    
    public class NoClaimWorkersCompensationClaimStatusFactory : WorkersCompensationClaimStatusFactory
    {
        static NoClaimWorkersCompensationClaimStatusFactory()
        {
            Defaults(new { Description = "No Claim Created" });
            OnSaving((x, s) => x.Id = WorkersCompensationClaimStatus.Indices.NO_CLAIM);
        }

        public NoClaimWorkersCompensationClaimStatusFactory(IContainer container) : base(container) { }
    }

    public class OpenWorkersCompensationClaimStatusFactory : WorkersCompensationClaimStatusFactory
    {
        static OpenWorkersCompensationClaimStatusFactory()
        {
            Defaults(new { Description = "Open" });
            OnSaving((x, s) => x.Id = WorkersCompensationClaimStatus.Indices.OPEN);
        }

        public OpenWorkersCompensationClaimStatusFactory(IContainer container) : base(container) { }
    }

    public class ClosedDeniedWorkersCompensationClaimStatusFactory : WorkersCompensationClaimStatusFactory
    {
        static ClosedDeniedWorkersCompensationClaimStatusFactory()
        {
            Defaults(new { Description = "Closed - Denied (Not Compensable)" });
            OnSaving((x, s) => x.Id = WorkersCompensationClaimStatus.Indices.CLOSED_DENIED);
        }

        public ClosedDeniedWorkersCompensationClaimStatusFactory(IContainer container) : base(container) { }
    }

    public class ClosedAcceptedWorkersCompensationClaimStatusFactory : WorkersCompensationClaimStatusFactory
    {
        static ClosedAcceptedWorkersCompensationClaimStatusFactory()
        {
            Defaults(new { Description = "Closed - Accepted (Compensable)" });
            OnSaving((x, s) => x.Id = WorkersCompensationClaimStatus.Indices.CLOSED_ACCEPTED);
        }

        public ClosedAcceptedWorkersCompensationClaimStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region IncidentTypeFactory

    public class IncidentTypeFactory : UniqueEntityLookupFactory<IncidentType>
    {
        #region Constructors

        public IncidentTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentInvestigation

    public class IncidentInvestigationFactory : TestDataFactory<IncidentInvestigation>
    {
        #region Constructors

        static IncidentInvestigationFactory()
        {
            Defaults(new {
                Incident = typeof(IncidentFactory),
                IncidentInvestigationRootCauseFindingType = typeof(IncidentInvestigationRootCauseFindingTypeFactory),
                IncidentInvestigationRootCauseLevel1Type = typeof(IncidentInvestigationRootCauseLevel1TypeFactory),
                IncidentInvestigationRootCauseLevel2Type = typeof(IncidentInvestigationRootCauseLevel2TypeFactory),
                IncidentInvestigationRootCauseLevel3Type = typeof(IncidentInvestigationRootCauseLevel3TypeFactory),
            });
        }

        public IncidentInvestigationFactory(IContainer container) : base(container) { }

        #endregion

        public override IncidentInvestigation Create(object overrides = null)
        {
            var investigation = base.Create(overrides);
            if (!investigation.Incident.IncidentInvestigations.Contains(investigation))
            {
                investigation.Incident.IncidentInvestigations.Add(investigation);
            }

            return investigation;
        }
    }

    #endregion

    #region IncidentInvestigationRootCauseFindingType

    public class
        IncidentInvestigationRootCauseFindingTypeFactory : UniqueEntityLookupFactory<
            IncidentInvestigationRootCauseFindingType>
    {
        #region Constructors

        public IncidentInvestigationRootCauseFindingTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentInvestigationRootCauseLevel1Type

    public class
        IncidentInvestigationRootCauseLevel1TypeFactory : UniqueEntityLookupFactory<
            IncidentInvestigationRootCauseLevel1Type>
    {
        #region Constructors

        public IncidentInvestigationRootCauseLevel1TypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentInvestigationRootCauseLevel2Type

    public class
        IncidentInvestigationRootCauseLevel2TypeFactory : UniqueEntityLookupFactory<
            IncidentInvestigationRootCauseLevel2Type>
    {
        #region Constructors

        static IncidentInvestigationRootCauseLevel2TypeFactory()
        {
            Defaults(new {
                IncidentInvestigationRootCauseLevel1Type = typeof(IncidentInvestigationRootCauseLevel1TypeFactory)
            });
        }

        public IncidentInvestigationRootCauseLevel2TypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region IncidentInvestigationRootCauseLevel3Type

    public class
        IncidentInvestigationRootCauseLevel3TypeFactory : UniqueEntityLookupFactory<
            IncidentInvestigationRootCauseLevel3Type>
    {
        #region Constructors

        static IncidentInvestigationRootCauseLevel3TypeFactory()
        {
            Defaults(new {
                IncidentInvestigationRootCauseLevel2Type = typeof(IncidentInvestigationRootCauseLevel2TypeFactory)
            });
        }

        public IncidentInvestigationRootCauseLevel3TypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Interconnection

    public class InterconnectionPurchaseSellTransferFactory : TestDataFactory<InterconnectionPurchaseSellTransfer>
    {
        public const string DESCRIPTION = "SELL";

        public InterconnectionPurchaseSellTransferFactory(IContainer container) : base(container) { }

        static InterconnectionPurchaseSellTransferFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionOperatingStatusFactory : TestDataFactory<InterconnectionOperatingStatus>
    {
        public const string DESCRIPTION = "appears to be working vOv";

        public InterconnectionOperatingStatusFactory(IContainer container) : base(container) { }

        static InterconnectionOperatingStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionCategoryFactory : TestDataFactory<InterconnectionCategory>
    {
        public const string DESCRIPTION = "some category";

        public InterconnectionCategoryFactory(IContainer container) : base(container) { }

        static InterconnectionCategoryFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionDeliveryMethodFactory : TestDataFactory<InterconnectionDeliveryMethod>
    {
        public const string DESCRIPTION = "some method";

        public InterconnectionDeliveryMethodFactory(IContainer container) : base(container) { }

        static InterconnectionDeliveryMethodFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionFlowControlMethodFactory : TestDataFactory<InterconnectionFlowControlMethod>
    {
        public const string DESCRIPTION = "wings";

        public InterconnectionFlowControlMethodFactory(IContainer container) : base(container) { }

        static InterconnectionFlowControlMethodFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionDirectionFactory : TestDataFactory<InterconnectionDirection>
    {
        public const string DESCRIPTION = "uphill";

        public InterconnectionDirectionFactory(IContainer container) : base(container) { }

        static InterconnectionDirectionFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionTypeFactory : TestDataFactory<InterconnectionType>
    {
        public const string DESCRIPTION = "some type";

        public InterconnectionTypeFactory(IContainer container) : base(container) { }

        static InterconnectionTypeFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
        }
    }

    public class InterconnectionFactory : TestDataFactory<Interconnection>
    {
        public const float MAXIMUM_FLOW_CAPACITY = 1.1f,
                           MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION = 2.2f,
                           REVERSIBLE_CAPACITY = 3.3f,
                           CONTRACT_MAX_SUMMER = 4.4f,
                           CONTRACT_MIN_SUMMER = 5.5f,
                           CONTRACT_MAX_WINTER = 6.6f,
                           CONTRACT_MIN_WINTER = 7.7f;

        public const string DEP_DESIGNATION = "blah blah",
                            PROGRAM_INTEREST_NUMBER = "12354",
                            PURCHASED_ACCOUNT_NUMBER = "12345678",
                            SOLD_ACCOUNT_NUMBER = "87654321",
                            WATER_QUALITY = "yup, s'good";

        public const int INLET_CONNECTION_SIZE = 1,
                         OUTLET_CONNECTION_SIZE = 2,
                         INLET_STATIC_PRESSURE = 3,
                         OUTLET_STATIC_PRESSURE = 4;

        public InterconnectionFactory(IContainer container) : base(container) { }

        static InterconnectionFactory()
        {
            Defaults(new {
                PurchaseSellTransfer = typeof(InterconnectionPurchaseSellTransferFactory),
                OperatingStatus = typeof(InterconnectionOperatingStatusFactory),
                Category = typeof(InterconnectionCategoryFactory),
                DeliveryMethod = typeof(InterconnectionDeliveryMethodFactory),
                FlowControlMethod = typeof(InterconnectionFlowControlMethodFactory),
                Direction = typeof(InterconnectionDirectionFactory),
                Type = typeof(InterconnectionTypeFactory),
                Facility = typeof(FacilityFactory),
                CreatedAt = Lambdas.GetNow,
                DEPDesignation = DEP_DESIGNATION,
                ProgramInterestNumber = PROGRAM_INTEREST_NUMBER,
                PurchasedWaterAccountNumber = PURCHASED_ACCOUNT_NUMBER,
                SoldWaterAccountNumber = SOLD_ACCOUNT_NUMBER,
                DirectConnection = true,
                InletConnectionSize = INLET_CONNECTION_SIZE,
                OutletConnectionSize = OUTLET_CONNECTION_SIZE,
                InletStaticPressure = INLET_STATIC_PRESSURE,
                OutletStaticPressure = OUTLET_STATIC_PRESSURE,
                MaximumFlowCapacity = MAXIMUM_FLOW_CAPACITY,
                MaximumFlowCapacityStressedCondition = MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION,
                DistributionPipingRestrictions = true,
                WaterQuality = WATER_QUALITY,
                FluoridatedSupplyReceivingPurveyor = true,
                FluoridatedSupplyDeliveryPurveyor = true,
                ChloramineResidualReceivingPurveyor = true,
                ChloramineResidualDeliveryPurveyor = true,
                CorrosionInhibitorReceivingPurveyor = true,
                CorrosionInhibitorDeliveryPurveyor = true,
                ReversibleCapacity = REVERSIBLE_CAPACITY,
                AnnualTestRequired = true,
                Contract = true,
                ContractMaxSummer = CONTRACT_MAX_SUMMER,
                ContractMinSummer = CONTRACT_MIN_SUMMER,
                ContractMaxWinter = CONTRACT_MAX_WINTER,
                ContractMinWinter = CONTRACT_MIN_WINTER,
                InletPWSID = typeof(PublicWaterSupplyFactory),
                OutletPWSID = typeof(PublicWaterSupplyFactory)
            });
        }
    }

    #endregion

    #region JobCategory

    public class JobCategoryFactory : TestDataFactory<JobCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "JobCategory";

        #endregion

        #region Constructors

        static JobCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public JobCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region OverallSafetyRating

    public class OverallSafetyRatingFactory : TestDataFactory<OverallSafetyRating>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "OverallSafetyRating";

        #endregion

        #region Constructors

        static OverallSafetyRatingFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public OverallSafetyRatingFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region OverallQualityRating

    public class OverallQualityRatingFactory : TestDataFactory<OverallQualityRating>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "OverallQualityRating";

        #endregion

        #region Constructors

        static OverallQualityRatingFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public OverallQualityRatingFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region JobObservation

    public class JobObservationFactory : TestDataFactory<JobObservation>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "JobObservation",
                            DEFAULT_LOCATION = "Under the stairs.";

        #endregion

        #region Constructors

        static JobObservationFactory()
        {
            Defaults(new {
                TaskObserved = DEFAULT_DESCRIPTION,
                Coordinate = typeof(CoordinateFactory),
                Department = typeof(JobCategoryFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                OverallQualityRating = typeof(OverallQualityRatingFactory),
                OverallSafetyRating = typeof(OverallSafetyRatingFactory),
                Address = DEFAULT_LOCATION,
                ObservationDate = Lambdas.GetNow,
                CreatedBy = typeof(UserFactory)
            });
        }

        public JobObservationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region JobSiteCheckList

    // TODO: When no override is made for OperatingCenter or CompetentEmployee, the
    //       OperatingCenter is always different from the one on CompetentEmployee.
    //       This leads to an invalid JobSiteCheckList.
    public class JobSiteCheckListFactory : TestDataFactory<JobSiteCheckList>
    {
        #region Constructors

        static JobSiteCheckListFactory()
        {
            // NOTE: Don't set MapCallWorkOrder as a default. 
            // The OperatingCenter for the JSCL and the WO won't end up being the same.
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                CompetentEmployee = typeof(EmployeeFactory),
                MarkoutNumber = "123",
                IsEmergencyMarkoutRequest = false,
                Address = "123 Fake St. Bricktucky, NJ 08724",
                CreatedBy = "JobSiteCheckListFactory",
                CreatedAt = Lambdas.GetNow,
                Coordinate = typeof(CoordinateFactory),
                SAPWorkOrderId = "1234567890",
                CheckListDate = Lambdas.GetNow,
                SupervisorSignOffDate = Lambdas.GetNow,
                SupervisorSignOffEmployee = typeof(ActiveEmployeeFactory),
                SafetyBriefDateTime = Lambdas.GetNow,
                HaveEquipmentToDoJobSafely = true,
                ReviewedErgonomicHazards = true,
                AnyPotentialWeatherHazards = false,
                AnyTimeOfDayConstraints = false,
                AnyTrafficHazards = false,
                InvolveConfinedSpace = false,
                AnyPotentialOverheadHazards = false,
                AnyUndergroundHazards = false,
                AreThereElectricalHazards = false,
                ReviewedLocationOfSafetyEquipment = true,
                OtherHazardsIdentified = false,

                // Old JSCL records have this set to false, but all new records must have this true.
                IsPressurizedRisksRestrainedFieldRequired = true,
                PressurizedRiskRestrainedType = typeof(NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory),
                NoRestraintReason = typeof(JobSiteCheckListNoRestraintReasonTypeFactory)
            });
        }

        public JobSiteCheckListFactory(IContainer container) : base(container) { }

        #endregion

        public override JobSiteCheckList Build(object overrides = null)
        {
            var built = base.Build(overrides);

            var comment = new JobSiteCheckListCommentFactory(_container).BuildWithConcreteDependencies();
            comment.CreatedAt = built.CreatedAt;
            comment.JobSiteCheckList = built;

            built.Comments.Add(comment);

            var crew = new JobSiteCheckListCrewMembersFactory(_container).BuildWithConcreteDependencies();
            crew.CreatedAt = built.CreatedAt;
            crew.JobSiteCheckList = built;

            built.CrewMembers.Add(crew);

            return built;
        }
    }

    public class JobSiteCheckListThatIsNotSignedOffByASupervisorFactory : JobSiteCheckListFactory
    {
        public JobSiteCheckListThatIsNotSignedOffByASupervisorFactory(IContainer container) : base(container) { }

        public override JobSiteCheckList Build(object overrides = null)
        {
            var built = base.Build(overrides);
            built.SupervisorSignOffDate = null;
            built.SupervisorSignOffEmployee = null;
            return built;
        }
    }

    #endregion

    #region JobSiteCheckListComment

    public class JobSiteCheckListCommentFactory : TestDataFactory<JobSiteCheckListComment>
    {
        static JobSiteCheckListCommentFactory()
        {
            Defaults(new {
                Comments = "Here's some comments",
                CreatedAt = DateTime.Now,
                CreatedBy = typeof(UserFactory)
            });
        }

        public JobSiteCheckListCommentFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteCheckListCrewMembers

    public class JobSiteCheckListCrewMembersFactory : TestDataFactory<JobSiteCheckListCrewMembers>
    {
        static JobSiteCheckListCrewMembersFactory()
        {
            Defaults(new {
                CrewMembers = "Crew McDuck",
                CreatedAt = DateTime.Now,
                CreatedBy = typeof(UserFactory)
            });
        }

        public JobSiteCheckListCrewMembersFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteCheckListPressurizedRiskRestrainedType

    public class
        JobSiteCheckListPressurizedRiskRestrainedTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListPressurizedRiskRestrainedType>
    {
        public JobSiteCheckListPressurizedRiskRestrainedTypeFactory(IContainer container) : base(container) { }
    }

    public class
        YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory : JobSiteCheckListPressurizedRiskRestrainedTypeFactory
    {
        static YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory()
        {
            Defaults(new {Description = "Yes"});
            OnSaving((a, s) => a.Id = JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES);
        }

        public YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory(IContainer container) : base(container) { }
    }

    public class
        NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory : JobSiteCheckListPressurizedRiskRestrainedTypeFactory
    {
        static NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory()
        {
            Defaults(new {Description = "No"});
            OnSaving((a, s) => a.Id = JobSiteCheckListPressurizedRiskRestrainedType.Indices.NO);
        }

        public NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteCheckListNoRestraintReasonType

    public class
        JobSiteCheckListNoRestraintReasonTypeFactory : UniqueEntityLookupFactory<JobSiteCheckListNoRestraintReasonType>
    {
        public JobSiteCheckListNoRestraintReasonTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteCheckListRestraintMethodType

    public class
        JobSiteCheckListRestraintMethodTypeFactory : UniqueEntityLookupFactory<JobSiteCheckListRestraintMethodType>
    {
        public JobSiteCheckListRestraintMethodTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteExcavation

    public class JobSiteExcavationFactory : TestDataFactory<JobSiteExcavation>
    {
        #region Constructors

        static JobSiteExcavationFactory()
        {
            Defaults(new {
                JobSiteCheckList = typeof(JobSiteCheckListFactory),
                LocationType = typeof(JobSiteExcavationLocationTypeFactory),
                SoilType = typeof(JobSiteExcavationSoilTypeFactory),
                CreatedBy = "factory",
                DepthInInches = 12m, // minimum allowed value is 12 inches
            });
        }

        public JobSiteExcavationFactory(IContainer container) : base(container) { }

        #endregion

        public override JobSiteExcavation Build(object overrides = null)
        {
            var model = base.Build(overrides);

            if (!model.JobSiteCheckList.Excavations.Contains(model))
            {
                model.JobSiteCheckList.Excavations.Add(model);
            }

            return model;
        }
    }

    #endregion

    #region JobSiteExcavationLocationType

    public class JobSiteExcavationLocationTypeFactory : UniqueEntityLookupFactory<JobSiteExcavationLocationType>
    {
        public JobSiteExcavationLocationTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteExcavationLocationType

    public class JobSiteExcavationProtectionTypeFactory : UniqueEntityLookupFactory<JobSiteExcavationProtectionType>
    {
        public JobSiteExcavationProtectionTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteExcavationSoilType

    public class JobSiteExcavationSoilTypeFactory : UniqueEntityLookupFactory<JobSiteExcavationSoilType>
    {
        public JobSiteExcavationSoilTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobSiteCheckListSafetyBrief

    public class
        JobSiteCheckListSafetyBriefWeatherHazardTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefWeatherHazardType>
    {
        public JobSiteCheckListSafetyBriefWeatherHazardTypeFactory(IContainer container) : base(container) { }
    }

    public class
        JobSiteCheckListSafetyBriefTimeOfDayConstraintTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefTimeOfDayConstraintType>
    {
        public JobSiteCheckListSafetyBriefTimeOfDayConstraintTypeFactory(IContainer container) : base(container) { }
    }

    public class
        JobSiteCheckListSafetyBriefOverheadHazardTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefOverheadHazardType>
    {
        public JobSiteCheckListSafetyBriefOverheadHazardTypeFactory(IContainer container) : base(container) { }
    }

    public class
        JobSiteCheckListSafetyBriefUndergroundHazardTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefUndergroundHazardType>
    {
        public JobSiteCheckListSafetyBriefUndergroundHazardTypeFactory(IContainer container) : base(container) { }
    }

    public class
        JobSiteCheckListSafetyBriefTrafficHazardTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefTrafficHazardType>
    {
        public JobSiteCheckListSafetyBriefTrafficHazardTypeFactory(IContainer container) : base(container) { }
    }

    public class
        JobSiteCheckListSafetyBriefElectricalHazardTypeFactory : UniqueEntityLookupFactory<
            JobSiteCheckListSafetyBriefElectricalHazardType>
    {
        public JobSiteCheckListSafetyBriefElectricalHazardTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region JobTitleCommonName

    public class JobTitleCommonNameFactory : TestDataFactory<JobTitleCommonName>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "JobTitleCommonName";

        #endregion

        #region Constructors

        static JobTitleCommonNameFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public JobTitleCommonNameFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LargeServiceProject

    public class LargeServiceProjectFactory : TestDataFactory<LargeServiceProject>
    {
        #region Constants

        public const string DEFAULT_PROJECT_TITLE = "ProjectTitle", DEFAULT_PROJECT_ADDRESS = "123 Easy St.";

        #endregion

        #region Constructors

        static LargeServiceProjectFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                ProjectTitle = DEFAULT_PROJECT_TITLE,
                ProjectAddress = DEFAULT_PROJECT_ADDRESS
            });
        }

        public LargeServiceProjectFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LateralSize

    public class LateralSizeFactory : UniqueEntityLookupFactory<LateralSize>
    {
        public LateralSizeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region LiabilityType

    public class LiabilityTypeFactory : TestDataFactory<LiabilityType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "LiabilityType";
        protected static int _count = 1;

        #endregion

        #region Constructors

        static LiabilityTypeFactory()
        {
            Func<string> description = () => {
                _count++;
                return DEFAULT_DESCRIPTION + " #" + _count;
            };

            Defaults(new {
                Description = description
            });
        }

        public LiabilityTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LicensedOperatorCategory

    public class LicensedOperatorCategoryFactory : StaticListEntityLookupFactory<LicensedOperatorCategory, LicensedOperatorCategoryFactory>
    {
        public LicensedOperatorCategoryFactory(IContainer container) : base(container) { }
    }

    public class InternalEmployeeLicensedOperatorCategoryFactory : LicensedOperatorCategoryFactory
    {
        static InternalEmployeeLicensedOperatorCategoryFactory()
        {
            Defaults(new { Description = "Internal Employee" });
            OnSaving((a, s) => a.Id = (int)LicensedOperatorCategory.Indices.INTERNAL_EMPLOYEE);
        } 

        public InternalEmployeeLicensedOperatorCategoryFactory(IContainer container) : base(container) { }
    }

    public class NotRequiredLicensedOperatorCategoryFactory : LicensedOperatorCategoryFactory
    {
        static NotRequiredLicensedOperatorCategoryFactory()
        {
            Defaults(new { Description = "No Licensed Operator Required" });
            OnSaving((a, s) => a.Id = (int)LicensedOperatorCategory.Indices.NO_LICENSED_OPERATOR_REQUIRED);
        }

        public NotRequiredLicensedOperatorCategoryFactory(IContainer container) : base(container) { }
    }

    public class ContractedLicensedOperatorCategoryFactory : LicensedOperatorCategoryFactory
    {
        static ContractedLicensedOperatorCategoryFactory()
        {
            Defaults(new { Description = "Contracted Licensed Operator" });
            OnSaving((a, s) => a.Id = (int)LicensedOperatorCategory.Indices.CONTRACTED_LICENSED_OPERATOR);
        }

        public ContractedLicensedOperatorCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region LIMSStatus

    public class LIMSStatusFactory : StaticListEntityLookupFactory<LIMSStatus, LIMSStatusFactory>
    {
        #region Constructors

        public LIMSStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class NotReadyLIMSStatusFactory : LIMSStatusFactory
    {
        #region Constructors

        static NotReadyLIMSStatusFactory()
        {
            Defaults(new {
                Description = "Not Ready"
            });
            OnSaving((a, s) => a.Id = (int)LIMSStatus.Indices.NOT_READY);
        }

        public NotReadyLIMSStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ReadyToSendLIMSStatusFactory : LIMSStatusFactory
    {
        #region Constructors

        static ReadyToSendLIMSStatusFactory()
        {
            Defaults(new {
                Description = "Ready to Send"
            });
            OnSaving((a, s) => a.Id = (int)LIMSStatus.Indices.READY_TO_SEND);
        }

        public ReadyToSendLIMSStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SentSuccessfullyLIMSStatusFactory : LIMSStatusFactory
    {
        #region Constructors

        static SentSuccessfullyLIMSStatusFactory()
        {
            Defaults(new {
                Description = "Sent Successfully"
            });
            OnSaving((a, s) => a.Id = (int)LIMSStatus.Indices.SENT_SUCCESSFULLY);
        }

        public SentSuccessfullyLIMSStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SendFailedLIMSStatusFactory : LIMSStatusFactory
    {
        #region Constructors

        static SendFailedLIMSStatusFactory()
        {
            Defaults(new {
                Description = "Send Failed"
            });
            OnSaving((a, s) => a.Id = (int)LIMSStatus.Indices.SEND_FAILED);
        }

        public SendFailedLIMSStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LocalFactory

    public class LocalFactory : TestDataFactory<Local>
    {
        public LocalFactory(IContainer container) : base(container) { }

        static LocalFactory()
        {
            var idx = 0;
            Func<string> nameFn = () => String.Format("union{0}", ++idx);
            Func<string> sapFn = () => String.Format("sap description {0}", ++idx);
            Defaults(new {
                Union = typeof(UnionFactory),
                Name = nameFn,
                IsActive = true,
                SAPUnionDescription = sapFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                Coordinate = typeof(CoordinateFactory)
            });
        }
    }

    #endregion

    #region LockoutForm

    public class LockoutFormFactory : TestDataFactory<LockoutForm>
    {
        #region Constants

        public const string
            DEFAULT_REASON = "default reason",
            DEFAULT_LOCATION_OF_LOCKOUT_NOTES = "default location of lockout notes";

        #endregion

        #region Constructors

        static LockoutFormFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Coordinate = typeof(CoordinateFactory),
                ReasonForLockout = DEFAULT_REASON,
                LocationOfLockoutNotes = DEFAULT_LOCATION_OF_LOCKOUT_NOTES,
                OutOfServiceAuthorizedEmployee = typeof(EmployeeFactory),
                Facility = typeof(FacilityFactory),
                Equipment = typeof(EquipmentFactory),
                LockoutReason = typeof(LockoutReasonFactory),
                IsolationPoint = typeof(LockoutDeviceLocationFactory),
                LockoutDateTime = DateTime.Today,
                OutOfServiceDateTime = DateTime.Today,
                SameAsInstaller = true,
                LockoutDevice = typeof(LockoutDeviceFactory),
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory)
            });
        }

        public LockoutFormFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LockoutFormQuestion

    //public class LockoutFormQuestionFactory

    #endregion

    #region LockoutFormCategory

    public class LockoutFormQuestionCategoryFactory : StaticListEntityLookupFactory<LockoutFormQuestionCategory,
        LockoutFormQuestionCategoryFactory>
    {
        public LockoutFormQuestionCategoryFactory(IContainer container) : base(container) { }
    }

    public class OutOfServiceLockoutFormQuestionCategoryFactory : LockoutFormQuestionCategoryFactory
    {
        #region Constants

        public const string DESCRIPTION = "Out of Service";

        #endregion

        static OutOfServiceLockoutFormQuestionCategoryFactory()
        {
            Defaults(new {Description = DESCRIPTION});
            OnSaving((a, s) => a.Id = LockoutFormQuestionCategory.Indices.OUT_OF_SERVICE);
        }

        public OutOfServiceLockoutFormQuestionCategoryFactory(IContainer container) : base(container) { }
    }

    public class ReturnToServiceLockoutFormQuestionCategoryFactory : LockoutFormQuestionCategoryFactory
    {
        #region Constants

        public const string DESCRIPTION = "Return to Service";

        #endregion

        static ReturnToServiceLockoutFormQuestionCategoryFactory()
        {
            Defaults(new {Description = DESCRIPTION});
            OnSaving((a, s) => a.Id = LockoutFormQuestionCategory.Indices.RETURN_TO_SERVICE);
        }

        public ReturnToServiceLockoutFormQuestionCategoryFactory(IContainer container) : base(container) { }
    }

    public class ManagementLockoutFormQuestionCategoryFactory : LockoutFormQuestionCategoryFactory
    {
        #region Constants

        public const string DESCRIPTION = "Management";

        #endregion

        static ManagementLockoutFormQuestionCategoryFactory()
        {
            Defaults(new {Description = DESCRIPTION});
            OnSaving((a, s) => a.Id = LockoutFormQuestionCategory.Indices.MANAGEMENT);
        }

        public ManagementLockoutFormQuestionCategoryFactory(IContainer container) : base(container) { }
    }

    public class LockoutConditionsLockoutFormQuestionCategoryFactory : LockoutFormQuestionCategoryFactory
    {
        #region Constants

        public const string DESCRIPTION = "Lockout Conditions";

        #endregion

        static LockoutConditionsLockoutFormQuestionCategoryFactory()
        {
            Defaults(new {Description = DESCRIPTION});
            OnSaving((a, s) => a.Id = LockoutFormQuestionCategory.Indices.LOCKOUT_CONDITIONS);
        }

        public LockoutConditionsLockoutFormQuestionCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region LockoutDevice

    public class LockoutDeviceFactory : TestDataFactory<LockoutDevice>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "LockoutDevice";

        #endregion

        #region Fields

        public static int _count = 1;

        #endregion

        #region Constructors

        static LockoutDeviceFactory()
        {
            Func<string> descriptionGenerator = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_DESCRIPTION, _count);
            };
            Func<string> serialNumberGenerator = () => { return _count.ToString(); };

            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Person = typeof(UserFactory),
                LockoutDeviceColor = typeof(LockoutDeviceColorFactory),
                Description = descriptionGenerator,
                SerialNumber = serialNumberGenerator
            });
        }

        public LockoutDeviceFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LockoutDeviceColor

    public class LockoutDeviceColorFactory : TestDataFactory<LockoutDeviceColor>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "LockoutDeviceColor";

        #endregion

        #region Fields

        public static int _count = 1;

        #endregion

        #region Constructors

        static LockoutDeviceColorFactory()
        {
            Func<string> descriptionGenerator = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_DESCRIPTION, _count);
            };

            Defaults(new {
                Description = descriptionGenerator
            });
        }

        public LockoutDeviceColorFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LockoutDeviceLocation

    public class LockoutDeviceLocationFactory : TestDataFactory<LockoutDeviceLocation>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "LockoutDeviceLocation";

        #endregion

        #region Fields

        public static int _count = 1;

        #endregion

        #region Constructors

        static LockoutDeviceLocationFactory()
        {
            Func<string> descriptionGenerator = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_DESCRIPTION, _count);
            };
            Defaults(new {
                Description = descriptionGenerator,
                IsActive = true
            });
        }

        public LockoutDeviceLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region LockoutReason

    public class LockoutReasonFactory : TestDataFactory<LockoutReason>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "LockoutReason";

        #endregion

        #region Fields

        public static int _count;

        #endregion

        #region Constructors

        static LockoutReasonFactory()
        {
            Func<string> descriptionGenerator = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_DESCRIPTION, _count);
            };
            Defaults(new {
                Description = descriptionGenerator
            });
        }

        public LockoutReasonFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainBreak

    public class MainBreakFactory : TestDataFactory<MainBreak>
    {
        #region Constants

        public const decimal DEFAULT_DEPTH = 4m, DEFAULT_SHUT_DOWN_TIME = 5m;
        public const int DEFAULT_CUSTOMERS_AFFECTED = 5;
        public const bool DEFAULT_BOIL_ALERT_ISSUED = true;

        #endregion

        #region Constructors

        static MainBreakFactory()
        {
            Defaults(new {
                WorkOrder = typeof(WorkOrderFactory),
                MainFailureType = typeof(MainFailureTypeFactory),
                MainBreakMaterial = typeof(MainBreakMaterialFactory),
                MainCondition = typeof(MainConditionFactory),
                MainBreakSoilCondition = typeof(MainBreakSoilConditionFactory),
                MainBreakDisinfectionMethod = typeof(MainBreakDisinfectionMethodFactory),
                MainBreakFlushMethod = typeof(MainBreakFlushMethodFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                Depth = DEFAULT_DEPTH,
                ShutdownTime = DEFAULT_SHUT_DOWN_TIME,
                BoilAlertIssued = DEFAULT_BOIL_ALERT_ISSUED,
                CustomersAffected = DEFAULT_CUSTOMERS_AFFECTED,
                ChlorineResidual = 1m
            });
        }

        public MainBreakFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainFailureType

    public class MainFailureTypeFactory : UniqueEntityLookupFactory<MainFailureType>
    {
        public MainFailureTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MainBreakMaterial

    public class MainBreakMaterialFactory : UniqueEntityLookupFactory<MainBreakMaterial>
    {
        #region Constructors

        public MainBreakMaterialFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainCondition

    public class MainConditionFactory : UniqueEntityLookupFactory<MainCondition>
    {
        #region Constructors

        public MainConditionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainBreakSoilCondition

    public class MainBreakSoilConditionFactory : UniqueEntityLookupFactory<MainBreakSoilCondition>
    {
        #region Constructors

        public MainBreakSoilConditionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainBreakDisinfectionMethod

    public class MainBreakDisinfectionMethodFactory : UniqueEntityLookupFactory<MainBreakDisinfectionMethod>
    {
        #region Constructors

        public MainBreakDisinfectionMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainBreakFlushMethod

    public class MainBreakFlushMethodFactory : UniqueEntityLookupFactory<MainBreakFlushMethod>
    {
        #region Constructors

        public MainBreakFlushMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainCrossing

    public class MainCrossingFactory : TestDataFactory<MainCrossing>
    {
        #region Constants

        #endregion

        #region Constructors

        static MainCrossingFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                Street = typeof(StreetFactory),
                ClosestCrossStreet = typeof(StreetFactory),
                InspectionFrequency = 1,
                InspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                CrossingCategory = typeof(CrossingCategoryFactory),
                MainCrossingStatus = typeof(MainCrossingStatusFactory),
                AssetCategory = typeof(AssetCategoryFactory),
                ConsequenceOfFailure = typeof(MainCrossingConsequenceOfFailureTypeFactory)
            });
        }

        public MainCrossingFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainCrossingConsequenceOfFailureType

    public class
        MainCrossingConsequenceOfFailureTypeFactory : UniqueEntityLookupFactory<MainCrossingConsequenceOfFailureType>
    {
        #region Constructors

        public MainCrossingConsequenceOfFailureTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainCrossingImpactToType

    public class MainCrossingImpactToTypeFactory : UniqueEntityLookupFactory<MainCrossingImpactToType>
    {
        #region Constructors

        public MainCrossingImpactToTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MainCrossingStatus

    public class MainCrossingStatusFactory : UniqueEntityLookupFactory<MainCrossingStatus>
    {
        public MainCrossingStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MainCrossingInspection

    public class MainCrossingInspectionFactory : TestDataFactory<MainCrossingInspection>
    {
        static MainCrossingInspectionFactory()
        {
            Defaults(new {
                CreatedBy = typeof(UserFactory),
                InspectedBy = typeof(UserFactory),
                InspectedOn = DateTime.Now,
                MainCrossing = typeof(MainCrossingFactory),
                AssessmentRating = typeof(MainCrossingInspectionAssessmentRatingFactory)
            });
        }

        public MainCrossingInspectionFactory(IContainer container) : base(container) { }

        public override MainCrossingInspection Build(object overrides = null)
        {
            var mci = base.Build(overrides);
            if (!mci.MainCrossing.Inspections.Contains(mci))
            {
                mci.MainCrossing.Inspections.Add(mci);
            }

            return mci;
        }
    }

    #endregion

    #region MainCrossingInspectionAssessmentRating

    public class MainCrossingInspectionAssessmentRatingFactory :
        UniqueEntityLookupFactory<MainCrossingInspectionAssessmentRating>
    {
        public MainCrossingInspectionAssessmentRatingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MapIconFactory

    public class MapIconFactory : TestDataFactory<MapIcon>
    {
        #region Constants

        public const string FILE_NAME = "SomeIcon.png";
        public const int HEIGHT = 32, WIDTH = 32;

        #endregion

        #region Constructors

        static MapIconFactory()
        {
            var i = 1;
            int IdFn() => i++;
            
            Defaults(new {
                Id = (Func<int>)IdFn,
                FileName = FILE_NAME,
                Height = HEIGHT,
                Width = WIDTH,
                Offset = typeof(MapIconOffsetFactory)
            });
        }

        public MapIconFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override MapIcon Save(MapIcon entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<MapIcon>>();
            var existing = repo.GetAll().SingleOrDefault(x => x.FileName == entity.FileName);
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    public class DefaultMapIconFactory : MapIconFactory
    {
        #region Constructors

        static DefaultMapIconFactory()
        {
            Defaults(new {
                FileName = "pin_black.png",
            });
        }

        public DefaultMapIconFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MapIconOffsetFactory

    public class MapIconOffsetFactory : EntityLookupTestDataFactory<MapIconOffset>
    {
        public MapIconOffsetFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MapImage

    public class MapImageFactory : TestDataFactory<MapImage>
    {
        #region Constructors

        static MapImageFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                FileName = "some file.tif",
                Directory = "SomeDirectory",
            });
        }

        public MapImageFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MarkoutFactory

    public class MarkoutFactory : TestDataFactory<Markout>
    {
        static MarkoutFactory()
        {
            var current = 100000000; // 9 digits
            Func<string> defaultMarkoutNumber = () => {
                current++;
                return current.ToString();
            };

            Defaults(new {
                WorkOrder = typeof(WorkOrderFactory),
                MarkoutNumber = defaultMarkoutNumber,
                DateOfRequest = Lambdas.GetNow,
                ReadyDate = DateTime.Now.AddDays(-1),
                ExpirationDate = DateTime.Now.AddDays(1)
            });
        }

        public MarkoutFactory(IContainer container) : base(container) { }

        public override Markout Create(object overrides = null)
        {
            var markout = base.Create(overrides);
            if (!markout.WorkOrder.Markouts.Contains(markout))
            {
                markout.WorkOrder.Markouts.Add(markout);
            }

            return markout;
        }
    }

    #endregion

    #region MarkoutDamage

    public class MarkoutDamageFactory : TestDataFactory<MarkoutDamage>
    {
        #region Constructors

        static MarkoutDamageFactory()
        {
            Defaults(new {
                Coordinate = typeof(CoordinateFactory),
                CreatedBy = "MarkoutDamageFactory",
                CreatedAt = DateTime.Today,
                DamageOn = DateTime.Today,
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                Street = "123 Electric Avenue",
                NearestCrossStreet = "Take You Higher St.",
                DamageComments = "This is a comment about markout damage. It's called Markout Damage's Theme.",
                MarkoutDamageToType = typeof(MarkoutDamageToTypeFactory)
            });
        }

        public MarkoutDamageFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutDamageNotSignedOffBySupervisorFactory : MarkoutDamageFactory
    {
        #region Constructors

        static MarkoutDamageNotSignedOffBySupervisorFactory() { }

        public MarkoutDamageNotSignedOffBySupervisorFactory(IContainer container) : base(container) { }

        #endregion

        public override MarkoutDamage Build(object overrides = null)
        {
            var built = base.Build(overrides);
            built.SupervisorSignOffEmployee = null;
            return built;
        }
    }

    #endregion

    #region MarkoutDamageUtilityDamageType

    public class MarkoutDamageUtilityDamageTypeFactory : StaticListEntityLookupFactory<MarkoutDamageUtilityDamageType, MarkoutDamageUtilityDamageTypeFactory>
    {
        public MarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }
    }

    public class CommunicationMarkoutDamageUtilityDamageTypeFactory : MarkoutDamageUtilityDamageTypeFactory
    {
        public CommunicationMarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }

        static CommunicationMarkoutDamageUtilityDamageTypeFactory()
        {
            Defaults(new { Description = "Communication" });
            OnSaving((a, s) => a.Id = MarkoutDamageUtilityDamageType.Indices.COMMUNICATION);
        }
    }
    
    public class ElectricMarkoutDamageUtilityDamageTypeFactory : MarkoutDamageUtilityDamageTypeFactory
    {
        public ElectricMarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }

        static ElectricMarkoutDamageUtilityDamageTypeFactory()
        {
            Defaults(new { Description = "Electric" });
            OnSaving((a, s) => a.Id = MarkoutDamageUtilityDamageType.Indices.ELECTRIC);
        }
    }
    
    public class GasMarkoutDamageUtilityDamageTypeFactory : MarkoutDamageUtilityDamageTypeFactory
    {
        public GasMarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }

        static GasMarkoutDamageUtilityDamageTypeFactory()
        {
            Defaults(new { Description = "Gas" });
            OnSaving((a, s) => a.Id = MarkoutDamageUtilityDamageType.Indices.GAS);
        }
    }
    
    public class SewerMarkoutDamageUtilityDamageTypeFactory : MarkoutDamageUtilityDamageTypeFactory
    {
        public SewerMarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }

        static SewerMarkoutDamageUtilityDamageTypeFactory()
        {
            Defaults(new { Description = "Sewer" });
            OnSaving((a, s) => a.Id = MarkoutDamageUtilityDamageType.Indices.SEWER);
        }
    }
    
    public class WaterMarkoutDamageUtilityDamageTypeFactory : MarkoutDamageUtilityDamageTypeFactory
    {
        public WaterMarkoutDamageUtilityDamageTypeFactory(IContainer container) : base(container) { }

        static WaterMarkoutDamageUtilityDamageTypeFactory()
        {
            Defaults(new { Description = "Water" });
            OnSaving((a, s) => a.Id = MarkoutDamageUtilityDamageType.Indices.WATER);
        }
    }

    #endregion

    #region MarkoutDamageToType

    public class MarkoutDamageToTypeFactory : EntityLookupTestDataFactory<MarkoutDamageToType>
    {
        #region Constructor

        public MarkoutDamageToTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MarkoutRequirement

    public class
        MarkoutRequirementFactory : StaticListEntityLookupFactory<MarkoutRequirement, MarkoutRequirementFactory>
    {
        #region Constructor

        public MarkoutRequirementFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class NoneMarkoutRequirementFactory : MarkoutRequirementFactory
    {
        public NoneMarkoutRequirementFactory(IContainer container) : base(container) { }

        static NoneMarkoutRequirementFactory()
        {
            Defaults(new {Description = "None"});
            OnSaving((a, s) => a.Id = (int)MarkoutRequirement.Indices.NONE);
        }
    }

    public class RoutineMarkoutRequirementFactory : MarkoutRequirementFactory
    {
        public RoutineMarkoutRequirementFactory(IContainer container) : base(container) { }

        static RoutineMarkoutRequirementFactory()
        {
            Defaults(new {Description = "Routine"});
            OnSaving((a, s) => a.Id = (int)MarkoutRequirement.Indices.ROUTINE);
        }
    }

    public class EmergencyMarkoutRequirementFactory : MarkoutRequirementFactory
    {
        public EmergencyMarkoutRequirementFactory(IContainer container) : base(container) { }

        static EmergencyMarkoutRequirementFactory()
        {
            Defaults(new {Description = "Emergency"});
            OnSaving((a, s) => a.Id = (int)MarkoutRequirement.Indices.EMERGENCY);
        }
    }

    #endregion

    #region MarkoutType

    public class MarkoutTypeFactory : StaticListEntityLookupFactory<MarkoutType, MarkoutTypeFactory>
    {
        public MarkoutTypeFactory(IContainer container) : base(container) { }
    }

    public class CtoCMarkoutTypeFactory : MarkoutTypeFactory
    {
        public CtoCMarkoutTypeFactory(IContainer container) : base(container) { }

        static CtoCMarkoutTypeFactory()
        {
            Defaults(new { Description = "C TO C" });
            OnSaving((a, s) => a.Id = MarkoutType.Indices.C_TO_C);
        }
    }

    public class NoneMarkoutTypeFactory : MarkoutTypeFactory
    {
        public NoneMarkoutTypeFactory(IContainer container) : base(container) { }

        static NoneMarkoutTypeFactory()
        {
            Defaults(new { Description = "NOT LISTED" });
            OnSaving((a, s) => a.Id = MarkoutType.Indices.NONE);
        }
    }

    #endregion

    #region MarkoutViolationFactory

    public class MarkoutViolationFactory : TestDataFactory<MarkoutViolation>
    {
        static MarkoutViolationFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
            });
        }

        public MarkoutViolationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Material

    public class MaterialFactory : TestDataFactory<Material>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Mercury", DEFAULT_PART_NUMBER = "Hg";

        #endregion

        #region Constructors

        static MaterialFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                PartNumber = DEFAULT_PART_NUMBER,
                IsActive = true
            });
        }

        public MaterialFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MaterialUsed

    public class MaterialUsedFactory : TestDataFactory<MaterialUsed>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Mercury", DEFAULT_PART_NUMBER = "Hg";

        #endregion

        #region Constructors

        static MaterialUsedFactory()
        {
            Defaults(new {
                WorkOrder = typeof(WorkOrderFactory),
                Quantity = 2,
            });
        }

        public MaterialUsedFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MeasurementPointEquipmentType

    public class MeasurementPointEquipmentTypeFactory : TestDataFactory<MeasurementPointEquipmentType>
    {
        #region Constructors

        static MeasurementPointEquipmentTypeFactory()
        {
            Defaults(new {
                Category = "C",
                Description = "some description",
                Min = 0m,
                Max = 10m,
                Position = 42,
                EquipmentType = typeof(EquipmentTypeFactory),
                UnitOfMeasure = typeof(UnitOfMeasureFactory)
            });
        }

        public MeasurementPointEquipmentTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MedicalCertificate

    public class MedicalCertificateFactory : TestDataFactory<MedicalCertificate>
    {
        #region Constants

        #endregion

        #region Constructors

        static MedicalCertificateFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                CertificationDate = Lambdas.GetLastYear,
                ExpirationDate = Lambdas.GetNow
            });
        }

        public MedicalCertificateFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Meter

    public class MeterFactory : TestDataFactory<Meter>
    {
        public MeterFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterChangeOut

    public class MeterChangeOutFactory : TestDataFactory<MeterChangeOut>
    {
        static MeterChangeOutFactory()
        {
            Defaults(new {
                Contract = typeof(MeterChangeOutContractFactory),
                CustomerName = "Some guy",
                ServiceStreetNumber = "123",
                ServiceStreet = "Some Street",
                ServiceCity = typeof(TownFactory)
            });
        }

        public MeterChangeOutFactory(IContainer container) : base(container) { }

        public override MeterChangeOut Build(object overrides = null)
        {
            var mco = base.Build(overrides);
            mco.Contract.MeterChangeOuts.AddIfMissing(mco);
            return mco;
        }
    }

    #endregion

    #region MeterChangeOutContract

    public class MeterChangeOutContractFactory : TestDataFactory<MeterChangeOutContract>
    {
        static MeterChangeOutContractFactory()
        {
            Defaults(new {
                Description = "Some contract",
                OperatingCenter = typeof(OperatingCenterFactory),
                Contractor = typeof(ContractorFactory)
            });
        }

        public MeterChangeOutContractFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterChangeOutStatus

    public class MeterChangeOutStatusFactory : UniqueEntityLookupFactory<MeterChangeOutStatus>
    {
        public MeterChangeOutStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterScheduleTime

    public class MeterScheduleTimeFactory : UniqueEntityLookupFactory<MeterScheduleTime>
    {
        public MeterScheduleTimeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterProfile

    public class MeterProfileFactory : TestDataFactory<MeterProfile>
    {
        public MeterProfileFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MotorVehicleCode

    public class MotorVehicleCodeFactory : UniqueEntityLookupFactory<MotorVehicleCode>
    {
        #region Constructors

        public MotorVehicleCodeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region NearMissFactory

    public class NearMissFactory : TestDataFactory<NearMiss>
    {
        public NearMissFactory(IContainer container) : base(container) { }

        static NearMissFactory()
        {
            var i = 0;

            Func<string> incidentNumberFn = () => $"Incident {++i}";
            Func<string> reportedByFn = () => $"Reported By {i}";
            // this is expected to be a string containing an integer
            Func<string> severityFn = () => i.ToString();
            Func<string> descriptionFn = () => $"Description {i}";

            Defaults(new {
                IncidentNumber = incidentNumberFn,
                OccurredAt = Lambdas.GetNow,
                CreatedAt = Lambdas.GetNow,
                ReportedBy = reportedByFn,
                Severity = severityFn,
                Description = descriptionFn
            });
        }
    }

    #endregion

    #region NearMissType

    public class NearMissTypeFactory : StaticListEntityLookupFactory<NearMissType, NearMissTypeFactory>
    {
        #region Constructors

        public NearMissTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EnvironmentalNearMissTypeFactory : NearMissTypeFactory
    {
        #region Constructors

        static EnvironmentalNearMissTypeFactory()
        {
            Defaults(new {
                Description = "Environmental Near Miss"
            });
            OnSaving((a, s) => a.Id = (int)NearMissType.Indices.ENVIRONMENTAL);
        }

        public EnvironmentalNearMissTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SafetyNearMissTypeFactory : NearMissTypeFactory
    {
        #region Constructors

        static SafetyNearMissTypeFactory()
        {
            Defaults(new {
                Description = "Safety Near Miss"
            });
            OnSaving((a, s) => a.Id = (int)NearMissType.Indices.SAFETY);
        }

        public SafetyNearMissTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region NearMissCategory

    public class NearMissCategoryFactory : StaticListEntityLookupFactory<NearMissCategory, NearMissCategoryFactory>
    {
        #region Constructors

        public NearMissCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ErgonomicsNearMissCategoryFactory : NearMissCategoryFactory
    {
        #region Constructors

        static ErgonomicsNearMissCategoryFactory()
        {
            Defaults(new {
                Description = "Ergonomics",
                Type = typeof(SafetyNearMissTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)NearMissCategory.Indices.ERGONOMICS);
        }

        public ErgonomicsNearMissCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OtherNearMissCategoryFactory : NearMissCategoryFactory
    {
        #region Constructors

        static OtherNearMissCategoryFactory()
        {
            Defaults(new {
                Description = "Other",
                Type = typeof(SafetyNearMissTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)NearMissCategory.Indices.OTHER);
        }

        public OtherNearMissCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class StormWaterNearMissCategoryFactory : NearMissCategoryFactory
    {
        #region Constructors

        static StormWaterNearMissCategoryFactory()
        {
            Defaults(new {
                Description = "StormWater",
                Type = typeof(EnvironmentalNearMissTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)NearMissCategory.Indices.STORMWATER);
        }

        public StormWaterNearMissCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region NoteFactory

    public class NoteFactory : TestDataFactory<Note>
    {
        public const string TEXT = "the note",
                            CREATED_BY = "some guy";

        public const int LINKED_ID = 666;

        public NoteFactory(IContainer container) : base(container) { }

        static NoteFactory()
        {
            Defaults(new {
                Text = TEXT,
                CreatedBy = CREATED_BY,
                LinkedId = LINKED_ID,
                CreatedAt = Lambdas.GetNow,
                DataType = typeof(DataTypeFactory)
            });
        }
    }

    #endregion

    #region NonRevenueWaterEntryFactory

    public class NonRevenueWaterEntryFactory : TestDataFactory<NonRevenueWaterEntry>
    {
        public const int MONTH = 1,
                         YEAR = 2023;

        public NonRevenueWaterEntryFactory(IContainer container) : base(container) { }

        static NonRevenueWaterEntryFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                CreatedAt = Lambdas.GetNow,
                CreatedBy = typeof(UserFactory),
                Month = MONTH,
                Year = YEAR
            });
        }
    }

    #endregion

    #region NonRevenueWaterDetailFactory

    public class NonRevenueWaterDetailFactory : TestDataFactory<NonRevenueWaterDetail>
    {
        public const string BUSINESS_UNIT = "654321",
                            WORK_DESCRIPTION = "Creating an invincible army",
                            MONTH = "Jan",
                            YEAR = "2023";

        public const long TOTAL_GALLONS = 1;

        public NonRevenueWaterDetailFactory(IContainer container) : base(container) { }

        static NonRevenueWaterDetailFactory()
        {
            Defaults(new {
                NonRevenueWaterEntry = typeof(NonRevenueWaterEntryFactory),
                Month = MONTH,
                Year = YEAR,
                BusinessUnit = BUSINESS_UNIT,
                WorkDescription = WORK_DESCRIPTION,
                TotalGallons = TOTAL_GALLONS
            });
        }
    }

    #endregion

    #region NonRevenueWaterAdjustmentFactory

    public class NonRevenueWaterAdjustmentFactory : TestDataFactory<NonRevenueWaterAdjustment>
    {
        public const string COMMENTS = "Moose bites can be nasty",
                            BUSINESS_UNIT = "999999";

        public const long TOTAL_GALLONS = 1;

        public NonRevenueWaterAdjustmentFactory(IContainer container) : base(container) { }

        static NonRevenueWaterAdjustmentFactory()
        {
            Defaults(new {
                NonRevenueWaterEntry = typeof(NonRevenueWaterEntryFactory),
                Comments = COMMENTS,
                TotalGallons = TOTAL_GALLONS,
                BusinessUnit = BUSINESS_UNIT
            });
        }
    }

    #endregion

    #region NotificationConfigurationFactory

    public class NotificationConfigurationFactory : TestDataFactory<NotificationConfiguration>
    {
        public NotificationConfigurationFactory(IContainer container) : base(container) { }

        static NotificationConfigurationFactory()
        {
            Defaults(new {
                Contact = typeof(ContactFactory),
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public override NotificationConfiguration Build(object overrides = null)
        {
            var nc = base.Build(overrides);
            if (nc.Contact != null && !nc.Contact.NotificationConfigurations.Contains(nc))
            {
                nc.Contact.NotificationConfigurations.Add(nc);
            }

            return nc;
        }
    }

    #endregion

    #region NotificationPurposeFactory

    public class NotificationPurposeFactory : TestDataFactory<NotificationPurpose>
    {
        public const string DEFAULT_PURPOSE = "various reasons";

        public NotificationPurposeFactory(IContainer container) : base(container) { }

        static NotificationPurposeFactory()
        {
            Defaults(new {
                Purpose = DEFAULT_PURPOSE,
                Module = typeof(ModuleFactory)
            });
        }
    }

    #endregion

    #region NoReadReasonFactory

    public class NoReadReasonFactory : StaticListEntityLookupFactory<NoReadReason, NoReadReasonFactory>
    {
        public NoReadReasonFactory(IContainer container) : base(container) { }
    }

    public class KitNotAvailableNoReadReasonFactory : NoReadReasonFactory
    {
        public const string DEFAULT_DESCRIPTION = "Kit Not Available";

        static KitNotAvailableNoReadReasonFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = NoReadReason.Indices.KIT_NOT_AVAILABLE);
        }

        public KitNotAvailableNoReadReasonFactory(IContainer container) : base(container) { }
    }

    public class NotDirectedByManagerNoReadReasonFactory : NoReadReasonFactory
    {
        public const string DEFAULT_DESCRIPTION = "Not Directed by Manager";

        static NotDirectedByManagerNoReadReasonFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = NoReadReason.Indices.NOT_DIRECTED_BY_MANAGER);
        }

        public NotDirectedByManagerNoReadReasonFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region NpdesRegulatorInspectionTypeFactory

    public class NpdesRegulatorInspectionTypeFactory
        : UniqueEntityLookupFactory<NpdesRegulatorInspectionType>
    {
        public NpdesRegulatorInspectionTypeFactory(IContainer container) : base(container) { }
    }

    public class StandardNpdesRegulatorInspectionTypeFactory
        : NpdesRegulatorInspectionTypeFactory
    {
        public StandardNpdesRegulatorInspectionTypeFactory(IContainer container) : base(container) { }

        static StandardNpdesRegulatorInspectionTypeFactory()
        {
            Defaults(new {
                Description = "STANDARD"
            });
            OnSaving((x, _) => x.Id = NpdesRegulatorInspectionType.Indices.STANDARD);
        }
    }

    public class RainEventNpdesRegulatorInspectionTypeFactory
        : NpdesRegulatorInspectionTypeFactory
    {
        public RainEventNpdesRegulatorInspectionTypeFactory(IContainer container) : base(container) { }

        static RainEventNpdesRegulatorInspectionTypeFactory()
        {
            Defaults(new {
                Description = "RAIN EVENT"
            });
            OnSaving((x, _) => x.Id = NpdesRegulatorInspectionType.Indices.RAIN_EVENT);
        }
    }

    #endregion

    #region NpdesRegulatorInspectionFactory

    public class NpdesRegulatorInspectionFactory : TestDataFactory<NpdesRegulatorInspection>
    {
        #region Constructors

        static NpdesRegulatorInspectionFactory()
        {
            Defaults(new {
                SewerOpening = typeof(SewerOpeningFactory),
                ArrivalDateTime = Lambdas.GetNow,
                DepartureDateTime = Lambdas.GetNow,
                InspectedBy = typeof(UserFactory),
                BlockCondition = typeof(InBlockConditionFactory),
                OutfallCondition = typeof(OutfallConditionFactory),
                WeatherCondition = typeof(WeatherConditionFactory),
                DischargeWeatherRelatedType = typeof(DischargeWeatherRelatedTypeFactory),
                DischargeCause = typeof(DischargeCauseFactory),
                GateStatusAnswerType = typeof(GateStatusAnswerTypeFactory),
                NpdesRegulatorInspectionType = typeof(StandardNpdesRegulatorInspectionTypeFactory),
                IsDischargePresent = false,
                HasFlowMeterMaintenanceBeenPerformed = false,
                Remarks = "Looks like a remark to me"
            });
        }

        public NpdesRegulatorInspectionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region GateStatusAnswerTypeFactory

    public class GateStatusAnswerTypeFactory : StaticListEntityLookupFactory<
        GateStatusAnswerType, GateStatusAnswerTypeFactory>
    {
        public GateStatusAnswerTypeFactory(IContainer container) : base(container) { }
    }

    public class YesGateStatusAnswerTypeFactory : GateStatusAnswerTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Yes";

        static YesGateStatusAnswerTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = GateStatusAnswerType.Indices.YES);
        }

        public YesGateStatusAnswerTypeFactory(IContainer container) : base(container) { }
    }

    public class NoGateStatusAnswerTypeFactory : GateStatusAnswerTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "No";

        static NoGateStatusAnswerTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = GateStatusAnswerType.Indices.NO);
        }

        public NoGateStatusAnswerTypeFactory(IContainer container) : base(container) { }
    }

    public class NotAvailableGateStatusAnswerTypeFactory : GateStatusAnswerTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "n/a";

        static NotAvailableGateStatusAnswerTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = GateStatusAnswerType.Indices.NOT_AVAILABLE);
        }

        public NotAvailableGateStatusAnswerTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ModuleFactory

    public class ModuleFactory : TestDataFactory<Module>
    {
        #region Consts

        private const RoleModules DEFAULT_ID = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Constructors

        static ModuleFactory()
        {
            Defaults(new {
                Id = DEFAULT_ID,
                Application = typeof(ApplicationFactory)
            });
        }

        public ModuleFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override Module Save(Module entity)
        {
            var repo = _container.GetInstance<RepositoryBase<Module>>();
            var existing = repo.Find(entity.Id);
            return existing ?? base.Save(entity);
        }

        #endregion

        #region Exposed Methods

        public override Module Build(object overrides = null)
        {
            // Just like Applications, since Build doesn't pass in the default values, and
            // I don't think it's necessary to create 900 factory classes
            // for each Module, I'm going to use DEFAULT_ID here
            // if there aren't any overrides.
            var app = base.Build(overrides);
            if (app.Id == 0)
            {
                app.Id = (int)DEFAULT_ID;
            }
            if (string.IsNullOrEmpty(app.Name))
            {
                // Setting this to the enum name won't be the same
                // as the names used in the live database. But we
                // shouldn't be using the names anyway.
                app.Name = ((RoleModules)app.Id).ToString(); // enum name
            }

            return app;
        }

        #endregion
    }

    #endregion

    #region OperatingCenterFactory

    /// <summary>
    /// Avoid using this factory if you can. This factory was originally created
    /// to make things easier by having everything be NJ7 by default, which meant other
    /// factories that used this would always be setup with the same operating center.
    ///
    /// However, this leads to a lot of headscratcher bugs sometimes, particularly when
    /// you use "NJ7" in a functional test and expect new values to be set on it. Those new
    /// values will not be set if anything has created that NJ7 operating center already.
    /// </summary>
    public class OperatingCenterFactory : TestDataFactory<OperatingCenter>
    {
        #region Constants

        public const string DEFAULT_OP_CNTR = "NJ7",
                            DEFAULT_OP_CNTR_NAME = "Shrewsbury";

        public const bool DEFAULT_WORK_ORDERS_ENABLED = true;

        #endregion

        #region Constructors

        static OperatingCenterFactory()
        {
            Defaults(new {
                OperatingCenterCode = DEFAULT_OP_CNTR,
                OperatingCenterName = DEFAULT_OP_CNTR_NAME,
                WorkOrdersEnabled = DEFAULT_WORK_ORDERS_ENABLED,
                State = typeof(StateFactory),
                IsActive = true,
                HydrantInspectionFrequency = 1,
                HydrantInspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                LargeValveInspectionFrequency = 1,
                LargeValveInspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                SmallValveInspectionFrequency = 1,
                SmallValveInspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                SewerOpeningInspectionFrequency = 1,
                SewerOpeningInspectionFrequencyUnit = typeof(YearlyRecurringFrequencyUnitFactory),
                ArcMobileMapId = "some map id",
                Id = 1
            });
        }

        public OperatingCenterFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override OperatingCenter Save(OperatingCenter entity)
        {
            var existing =
                Session.Query<OperatingCenter>()
                       .SingleOrDefault(x => x.OperatingCenterCode == entity.OperatingCenterCode);

            if (existing != null)
            {
                return existing;
            }

            return base.Save(entity);
        }

        #endregion
    }

    /// <summary>
    /// For those times when you need a unique operating center but don't really care 
    /// about the values in it otherwise.
    /// </summary>
    public class UniqueOperatingCenterFactory : OperatingCenterFactory
    {
        #region Fields

        private static int _currentOpCenterNumber = 1;

        #endregion

        #region Construcotr

        public UniqueOperatingCenterFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        // TODO: This should be done in the Defaults().
        protected override OperatingCenter Save(OperatingCenter entity)
        {
            entity.OperatingCenterCode = "OPC" + _currentOpCenterNumber;
            _currentOpCenterNumber++;
            return base.Save(entity);
        }

        public static void ResetOpCenterCodeNumberCount()
        {
            _currentOpCenterNumber = 1;
        }

        #endregion
    }

    #endregion

    #region OperatingLicenseFactory

    public class OperatorLicenseFactory : TestDataFactory<OperatorLicense>
    {
        #region Constructors

        static OperatorLicenseFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                Employee = typeof(EmployeeFactory),
                OperatorLicenseType = typeof(OperatorLicenseTypeFactory),
                LicenseLevel = "1234",
                LicenseNumber = "1234",
                ValidationDate = Lambdas.GetYesterday,
                ExpirationDate = Lambdas.GetNow
            });
        }

        public OperatorLicenseFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OperatorLicenseTypeFactory : TestDataFactory<OperatorLicenseType>
    {
        static OperatorLicenseTypeFactory()
        {
            var i = 0;
            Func<string> desc = () => $"This is my description{i++}";
            Defaults(new {
                Description = desc
            });
        }

        public OperatorLicenseTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region OneCallMarkoutAudit

    public class OneCallMarkoutAuditFactory : TestDataFactory<OneCallMarkoutAudit>
    {
        #region Constants

        public const string DEFAULT_FULL_TEXT = "OneCallMarkoutAudit";

        #endregion

        #region Constructors

        static OneCallMarkoutAuditFactory()
        {
            Defaults(new {
                FullText = DEFAULT_FULL_TEXT
            });
        }

        public OneCallMarkoutAuditFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region OneCallMarkoutResponse

    public class OneCallMarkoutResponseFactory : TestDataFactory<OneCallMarkoutResponse>
    {
        #region Constructors

        static OneCallMarkoutResponseFactory()
        {
            Defaults(new {
                OneCallMarkoutTicket = typeof(OneCallMarkoutTicketFactory),
                CompletedBy = typeof(UserFactory),
                CompletedAt = Lambdas.GetNow
            });
        }

        public OneCallMarkoutResponseFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region OneCallMarkoutTicket

    public class OneCallMarkoutTicketFactory : TestDataFactory<OneCallMarkoutTicket>
    {
        #region Constants

        public const string DEFAULT_FULL_TEXT = "Full Text";

        #endregion

        #region Constructors

        static OneCallMarkoutTicketFactory()
        {
            var i = 0;
            Func<int> requestNumberFn = () => ++i;
            Func<string> cdcCodeFn = () => string.Format("CDCCode{0}", i);
            Func<string> countyFn = () => string.Format("County {0}", i);
            Func<string> townFn = () => string.Format("Town {0}", i);
            Func<string> streetFn = () => string.Format("Street {0}", i);

            Defaults(new {
                FullText = DEFAULT_FULL_TEXT,
                MessageType = typeof(OneCallMarkoutMessageTypeFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                RequestNumber = requestNumberFn,
                CDCCode = cdcCodeFn,
                CountyText = countyFn,
                TownText = townFn,
                StreetText = streetFn
            });
        }

        public OneCallMarkoutTicketFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region OneCallMarkoutMessageType

    public class OneCallMarkoutMessageTypeFactory : UniqueEntityLookupFactory<OneCallMarkoutMessageType>
    {
        public OneCallMarkoutMessageTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region OrderType
    
    public class OrderTypeFactory : StaticListEntityLookupFactory<OrderType, OrderTypeFactory>
    {
        public OrderTypeFactory(IContainer container) : base(container) { }
    }

    public class OperationalOrderTypeFactory : OrderTypeFactory
    {
        static OperationalOrderTypeFactory()
        {
            Defaults(new { Description = "Operational Activity", SAPCode = OrderType.SAPCodes.OPERATIONAL_ACTIVITY_10 });
            OnSaving((x, s) => x.Id = OrderType.Indices.OPERATIONAL_ACTIVITY_10);
        }

        public OperationalOrderTypeFactory(IContainer container) : base(container) { }
    }

    public class PlantMaintenanceOrderTypeFactory : OrderTypeFactory
    {
        static PlantMaintenanceOrderTypeFactory()
        {
            Defaults(new { Description = "PM Work Order", SAPCode = OrderType.SAPCodes.PLANT_MAINTENANCE_WORK_ORDER_11 });
            OnSaving((x, s) => x.Id = OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11);
        }

        public PlantMaintenanceOrderTypeFactory(IContainer container) : base(container) { }
    }

    public class CorrectiveActionOrderTypeFactory : OrderTypeFactory
    {
        static CorrectiveActionOrderTypeFactory()
        {
            Defaults(new { Description = "Corrective Action", SAPCode = OrderType.SAPCodes.CORRECTIVE_ACTION_20 });
            OnSaving((x, s) => x.Id = OrderType.Indices.CORRECTIVE_ACTION_20);
        }

        public CorrectiveActionOrderTypeFactory(IContainer container) : base(container) { }
    }

    public class RpCapitalOrderTypeFactory : OrderTypeFactory
    {
        static RpCapitalOrderTypeFactory()
        {
            Defaults(new { Description = "RP Capital", SAPCode = OrderType.SAPCodes.RP_CAPITAL_40 });
            OnSaving((x, s) => x.Id = OrderType.Indices.RP_CAPITAL_40);
        }

        public RpCapitalOrderTypeFactory(IContainer container) : base(container) { }
    }

    public class RoutineOrderTypeFactory : OrderTypeFactory
    {
        static RoutineOrderTypeFactory()
        {
            Defaults(new { Description = "Routine", SAPCode = OrderType.SAPCodes.ROUTINE_13 });
            OnSaving((x, s) => x.Id = OrderType.Indices.ROUTINE_13);
        }

        public RoutineOrderTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region OutfallConditionFactory

    public class OutfallConditionFactory : UniqueEntityLookupFactory<OutfallCondition>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Outfall Condition Description";

        #endregion

        public OutfallConditionFactory(IContainer container) : base(container) { }

        static OutfallConditionFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }
    }

    #endregion

    #region PDESSystem

    public class WasteWaterSystemFactory : TestDataFactory<WasteWaterSystem>
    {
        static WasteWaterSystemFactory()
        {
            var i = 0;
            Func<string> nameFn = () => $"Water System {++i}";
            Func<string> permitNumberFn = () => $"Permit {i}";

            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                BusinessUnit = typeof(BusinessUnitFactory),
                Status = typeof(WasteWaterSystemStatusFactory),
                WasteWaterSystemName = nameFn,
                PermitNumber = permitNumberFn,
                GravityLength = 1000,
                ForceLength = 500,
                NumberOfLiftStations = 1,
                TreatmentDescription = "some treatment description",
                NumberOfCustomers = 3,
                PeakFlowMGD = 4,
                IsCombinedSewerSystem = false,
                Ownership = typeof(EntityLookupTestDataFactory<WasteWaterSystemOwnership>),
                LicensedOperatorStatus = typeof(InternalEmployeeLicensedOperatorCategoryFactory),
                Type = typeof(EntityLookupTestDataFactory<WasteWaterSystemType>),
                SubType = typeof(EntityLookupTestDataFactory<WasteWaterSystemSubType>),
                HasConsentOrder = false
            });
        }

        public WasteWaterSystemFactory(IContainer container) : base(container) { }
    }

    public class WasteWaterSystemBasinFactory : TestDataFactory<WasteWaterSystemBasin>
    {
        static WasteWaterSystemBasinFactory()
        {
            var i = 0;
            Func<int> WasteWaterSystemIdFn = () => ++i;

            Defaults(new {
                BasinName = "Some Wastewater Basin",
                WasteWaterSystem = typeof(WasteWaterSystemFactory),
                FirmCapacity = (decimal)1.1
            });
        }

        public WasteWaterSystemBasinFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region PersonnelArea

    public class PersonnelAreaFactory : TestDataFactory<PersonnelArea>
    {
        #region Fields

        private static int _idCount;

        #endregion

        #region Constructors

        static PersonnelAreaFactory()
        {
            Func<int> uniquePersonnelAreaID = () => {
                _idCount++;
                return _idCount++;
            };

            Defaults(new {
                PersonnelAreaId = uniquePersonnelAreaID,
                Description = "Some Description",
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        #endregion

        public PersonnelAreaFactory(IContainer container) : base(container) { }
    }

    public class PersonnelAreaWithoutOperatingCenterFactory : PersonnelAreaFactory
    {
        public PersonnelAreaWithoutOperatingCenterFactory(IContainer container) : base(container) { }

        public override PersonnelArea Build(object overrides = null)
        {
            var pa = base.Build(overrides);
            pa.OperatingCenter = null;
            return pa;
        }
    }

    #endregion

    #region PipeDataLookupType

    public class PipeDataLookupTypeFactory : TestDataFactory<PipeDataLookupType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "PipeDataLookupType";

        #endregion

        #region Constructors

        static PipeDataLookupTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public PipeDataLookupTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PipeDataLookupValue

    public class PipeDataLookupValueFactory : TestDataFactory<PipeDataLookupValue>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "PipeDataLookupValue";

        #endregion

        #region Constructors

        static PipeDataLookupValueFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                PipeDataLookupType = typeof(PipeDataLookupTypeFactory)
            });
        }

        public PipeDataLookupValueFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PipeDiameter

    public class PipeDiameterFactory : TestDataFactory<PipeDiameter>
    {
        #region Constants

        public const decimal DIAMETER = 12m;

        #endregion

        #region Constructors

        static PipeDiameterFactory()
        {
            Defaults(new {
                Diameter = DIAMETER
            });
        }

        public PipeDiameterFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PipeMaterial

    public class PipeMaterialFactory : TestDataFactory<PipeMaterial>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "PipeMaterial";

        #endregion

        #region Constructors

        static PipeMaterialFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public PipeMaterialFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PlanningPlant

    public class PlanningPlantFactory : UniqueEntityLookupFactory<PlanningPlant>
    {
        static PlanningPlantFactory()
        {
            Defaults(new {
                Code = "CODE"
            });
        }

        public PlanningPlantFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region PlanReview

    public class PlanReviewFactory : TestDataFactory<PlanReview>
    {
        #region Constructors

        static PlanReviewFactory()
        {
            Defaults(new {
                ReviewChangeNotes = "default note",
                ReviewedBy = typeof(EmployeeFactory),
                CreatedBy = typeof(UserFactory),
                CreatedAt = DateTime.Now,
                ReviewDate = DateTime.Now,
                NextReviewDate = DateTime.Now,
                Plan = typeof(EmergencyResponsePlanFactory)
            });
        }

        public PlanReviewFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PlantMaintenanceActivityType

    public class PlantMaintenanceActivityTypeFactory : UniqueEntityLookupFactory<PlantMaintenanceActivityType>
    {
        static PlantMaintenanceActivityTypeFactory()
        {
            Defaults(new {
                Code = "AA1"
            });
        }

        public PlantMaintenanceActivityTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region PointOfInterestStatus

    public class PointOfInterestStatusFactory : StaticListEntityLookupFactory<PointOfInterestStatus, PointOfInterestStatusFactory>
    {
        public PointOfInterestStatusFactory(IContainer container) : base(container) { }
    }

    public class FieldInvestigationRecommendedPointOfInterestFactory : PointOfInterestStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Field Investigation Recommend";

        static FieldInvestigationRecommendedPointOfInterestFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)PointOfInterestStatus.Indices.FIELD_INVESTIGATION_RECOMMENDED);
        }

        public FieldInvestigationRecommendedPointOfInterestFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MaintenancePlan

    public class MaintenancePlanFactory : TestDataFactory<MaintenancePlan>
    {
        #region Constants

        public const string DEFAULT_LOCAL_TASK_DESCRIPTION = "What is your favorite color?";

        #endregion

        #region Constructors

        static MaintenancePlanFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                PlanningPlant = typeof(PlanningPlantFactory),
                Facility = typeof(FacilityFactory),
                TaskGroup = typeof(TaskGroupFactory),
                TaskGroupCategory = typeof(TaskGroupCategoryFactory),
                WorkDescription = typeof(UniqueMaintenancePlanProductionWorkDescriptionFactory),
                LocalTaskDescription = DEFAULT_LOCAL_TASK_DESCRIPTION,
                Start = DateTime.Now,
                ForecastPeriodMultiplier = 1.0m,
                IsPlanPaused = false,
                ProductionWorkOrderFrequency = typeof(ProductionWorkOrderFrequencyFactory),
                PlanNumber = "900000001"
            });
        }

        public MaintenancePlanFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PositionGroup

    public class PositionGroupFactory : TestDataFactory<PositionGroup>
    {
        public const string BUSINESS_UNIT = "123456";

        static PositionGroupFactory()
        {
            var keyCounter = 0;
            Func<string> keyFn = () => $"X{keyCounter++}";
            Defaults(new {
                BusinessUnit = BUSINESS_UNIT,
                BusinessUnitDescription = "Some BU Description",
                Group = "A11",
                PositionDescription = "Some position description",
                SAPCompanyCode = typeof(SAPCompanyCodeFactory),
                CommonName = typeof(PositionGroupCommonNameFactory),
                State = typeof(StateFactory),
                SAPPositionGroupKey = keyFn
            });
        }

        public PositionGroupFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region PositionGroupCommonName

    public class PositionGroupCommonNameFactory : TestDataFactory<PositionGroupCommonName>
    {
        #region Constants

        #endregion

        #region Constructors

        static PositionGroupCommonNameFactory()
        {
            int i = 1;
            Func<string> descriptionFn = () => String.Format("PGCN {0}", i++);
            Defaults(new {
                Description = descriptionFn
            });
        }

        public PositionGroupCommonNameFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Premise

    public class PremiseFactory : TestDataFactory<Premise>
    {
        #region Constructors

        static PremiseFactory()
        {
            var premiseNumber = 1000000000;
            Func<string> premiseNumberFn = () => premiseNumber++.ToString();
            
            Defaults(new {
                PremiseNumber = premiseNumberFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                StatusCode = typeof(ActivePremiseStatusCodeFactory)
            });
        }

        public PremiseFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PremiseType

    public class PremiseTypeFactory : TestDataFactory<PremiseType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "PremiseType",
                            DEFAULT_ABBREVIATION = "Abbr";

        #endregion

        #region Constructors

        static PremiseTypeFactory()
        {
            Defaults(new {
                Abbreviation = DEFAULT_ABBREVIATION,
                Description = DEFAULT_DESCRIPTION
            });
        }

        public PremiseTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PremiseUnavailableReason

    public class PremiseUnavailableReasonFactory : UniqueEntityLookupFactory<PremiseUnavailableReason>
    {
        public PremiseUnavailableReasonFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProductionSkillSet

    public class ProductionSkillSetFactory : EntityLookupTestDataFactory<ProductionSkillSet>
    {
        #region Constructors

        public ProductionSkillSetFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProductionWorkOrder

    public class ProductionWorkOrderFactory : TestDataFactory<ProductionWorkOrder>
    {
        #region Constants

        public const string DEFAULT_SAP_WORK_ORDER = "123456767";

        #endregion

        #region Constructors

        static ProductionWorkOrderFactory()
        {
            Defaults(new {
                FunctionalLocation = "NJ",
                OperatingCenter = typeof(OperatingCenterFactory),
                PlanningPlant = typeof(PlanningPlantFactory),
                Facility = typeof(FacilityFactory),
                FacilityFacilityArea = typeof(FacilityFacilityAreaFactory),
                EquipmentType = typeof(EquipmentTypeGeneratorFactory),
                Equipment = typeof(EquipmentFactory),
                Coordinate = typeof(CoordinateFactory),
                Priority = typeof(HighProductionWorkOrderPriorityFactory),
                OrderNotes = "This is OrderNotes",
                DateReceived = new DateTime(2017, 01, 01),
                SAPWorkOrder = DEFAULT_SAP_WORK_ORDER,
                SAPErrorCode = "None",
                WBSElement = "Sample WBSElement",
                ProductionWorkDescription = typeof(ProductionWorkDescriptionFactory),
                RequestedBy = typeof(EmployeeFactory),
                BreakdownIndicator = false,
                LocalTaskDescription = "This is the Local Task Description",
                StartDate = new DateTime(2017, 01, 01)
            });
        }

        public ProductionWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProductionWorkDescriptionFactory

    public class ProductionWorkDescriptionFactory : TestDataFactory<ProductionWorkDescription>
    {
        #region Constructors

        static ProductionWorkDescriptionFactory()
        {
            var i = 0;
            Func<string> descriptionFn = () => string.Format("Process {0}", ++i);

            Defaults(new {
                Description = descriptionFn,
                //  Id = 1,
                EquipmentType = typeof(EquipmentTypeGeneratorFactory),
                PlantMaintenanceActivityType = typeof(PlantMaintenanceActivityTypeFactory),
                OrderType = typeof(OrderTypeFactory),
                BreakdownIndicator = false,
                MaintenancePlanTaskType = typeof(MaintenancePlanTaskTypeFactory),
                TaskGroup = typeof(TaskGroupFactory),
                ProductionSkillSet = typeof(ProductionSkillSetFactory)
            });
        }

        public ProductionWorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class UniqueMaintenancePlanProductionWorkDescriptionFactory : TestDataFactory<ProductionWorkDescription>
    {
        #region Constructors

        static UniqueMaintenancePlanProductionWorkDescriptionFactory()
        {
            Defaults(new {
                Description = ProductionWorkDescription.StaticDescriptions.MAINTENANCE_PLAN,
                OrderType = typeof(RoutineOrderTypeFactory)
            });
        }

        public UniqueMaintenancePlanProductionWorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProductionWorkOrderCancellationReason

    public class ProductionWorkOrderCancellationReasonFactory : StaticListEntityLookupFactory<ProductionWorkOrderCancellationReason, ProductionWorkOrderCancellationReasonFactory>
    {
        public ProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class CreatedInErrorProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static CreatedInErrorProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Created In Error" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.CREATED_IN_ERROR);
        }

        public CreatedInErrorProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class CustomerRequestProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static CustomerRequestProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Customer Request" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.CUSTOMER_REQUEST);
        }

        public CustomerRequestProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class CompanyErrorProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static CompanyErrorProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Company Error" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.COMPANY_ERROR);
        }

        public CompanyErrorProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class OrderPastExpirationDateProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static OrderPastExpirationDateProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Order Past Expiration Date" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.ORDER_PAST_EXPIRATION_DATE);
        }

        public OrderPastExpirationDateProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class NoLongerValidProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static NoLongerValidProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "No Longer Valid" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.NO_LONGER_VALID);
        }

        public NoLongerValidProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class SupervisorInstructedProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static SupervisorInstructedProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Supervisor Instructed" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.SUPERVISOR_INSTRUCTED);
        }

        public SupervisorInstructedProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    public class WorkAlreadyCompletedProductionWorkOrderCancellationReasonFactory : ProductionWorkOrderCancellationReasonFactory
    {
        static WorkAlreadyCompletedProductionWorkOrderCancellationReasonFactory()
        {
            Defaults(new { Description = "Work Already Completed" });
            OnSaving((a, s) => a.Id = ProductionWorkOrderCancellationReason.Indices.WORK_ALREADY_COMPLETED);
        }

        public WorkAlreadyCompletedProductionWorkOrderCancellationReasonFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProductionWorkOrderEquipment

    public class ProductionWorkOrderEquipmentFactory : TestDataFactory<ProductionWorkOrderEquipment>
    {
        #region Constructors

        static ProductionWorkOrderEquipmentFactory()
        {
            Defaults(new {
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                Equipment = typeof(EquipmentFactory)
            });
        }

        public ProductionWorkOrderEquipmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProductionWorkOrderFrequency

    public class ProductionWorkOrderFrequencyFactory : StaticListEntityLookupFactory<ProductionWorkOrderFrequency, ProductionWorkOrderFrequencyFactory>
    {
        public ProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }
    
    public class DailyProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static DailyProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Daily",
                Name = "Daily",
                SortOrder = 1,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.DAILY);
        }

        public DailyProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class WeeklyProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static WeeklyProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Weekly",
                Name = "Weekly",
                SortOrder = 2,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.WEEKLY);
        }

        public WeeklyProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class TwicePerMonthProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static TwicePerMonthProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Twice Per Month",
                Name = "Twice Per Month",
                SortOrder = 3,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.BI_MONTHLY);
        }

        public TwicePerMonthProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class MonthlyProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static MonthlyProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Monthly",
                Name = "Monthly",
                SortOrder = 4,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.MONTHLY);
        }

        public MonthlyProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryTwoMonthsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryTwoMonthsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Two Months",
                Name = "Every Two Months",
                SortOrder = 5,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_TWO_MONTHS);
        }

        public EveryTwoMonthsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class QuarterlyProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static QuarterlyProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Quarterly",
                Name = "Quarterly",
                SortOrder = 6,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.QUARTERLY);
        }

        public QuarterlyProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryFourMonthsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryFourMonthsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Four Months",
                Name = "Every Four Months",
                SortOrder = 7,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_FOUR_MONTHS);
        }

        public EveryFourMonthsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EverySixMonthsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EverySixMonthsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Six Months",
                Name = "Every Six Months",
                SortOrder = 8,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.BI_ANNUAL);
        }

        public EverySixMonthsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class AnnualProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static AnnualProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Annual",
                Name = "Annual",
                SortOrder = 9,
                ForecastYearSpan = 1
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.ANNUAL);
        }

        public AnnualProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryTwoYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryTwoYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Two Years",
                Name = "Every Two Years",
                SortOrder = 10,
                ForecastYearSpan = 2
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_TWO_YEARS);
        }

        public EveryTwoYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryThreeYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryThreeYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Three Years",
                Name = "Every Three Years",
                SortOrder = 11,
                ForecastYearSpan = 3
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_THREE_YEARS);
        }

        public EveryThreeYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryFourYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryFourYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Four Years",
                Name = "Every Four Years",
                SortOrder = 12,
                ForecastYearSpan = 4
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_FOUR_YEARS);
        }

        public EveryFourYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryFiveYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryFiveYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Five Years",
                Name = "Every Five Years",
                SortOrder = 13,
                ForecastYearSpan = 5
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_FIVE_YEARS);
        }

        public EveryFiveYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryTenYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryTenYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Ten Years",
                Name = "Every Ten Years",
                SortOrder = 14,
                ForecastYearSpan = 10
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_TEN_YEARS);
        }

        public EveryTenYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    public class EveryFifteenYearsProductionWorkOrderFrequencyFactory : ProductionWorkOrderFrequencyFactory
    {
        static EveryFifteenYearsProductionWorkOrderFrequencyFactory()
        {
            Defaults(new {
                Description = "Every Fifteen Years",
                Name = "Every Fifteen Years",
                SortOrder = 15,
                ForecastYearSpan = 15
            });

            OnSaving((x, _) => x.Id = ProductionWorkOrderFrequency.Indices.EVERY_FIFTEEN_YEARS);
        }

        public EveryFifteenYearsProductionWorkOrderFrequencyFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProductionWorkOrderMeasurementPointValue

    public class
        ProductionWorkOrderMeasurementPointValueFactory : TestDataFactory<ProductionWorkOrderMeasurementPointValue>
    {
        #region Constructors

        static ProductionWorkOrderMeasurementPointValueFactory()
        {
            Defaults(new {
                Value = "some value",
                Equipment = typeof(EquipmentFactory),
                MeasurementPointEquipmentType = typeof(MeasurementPointEquipmentTypeFactory),
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
            });
        }

        public ProductionWorkOrderMeasurementPointValueFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PremiseStatusCode

    public abstract class PremiseStatusCodeFactory
        : StaticListEntityLookupFactory<PremiseStatusCode, PremiseStatusCodeFactory>
    {
        protected PremiseStatusCodeFactory(IContainer container) : base(container) { }
    }

    public class ActivePremiseStatusCodeFactory : PremiseStatusCodeFactory
    {
        static ActivePremiseStatusCodeFactory()
        {
            Defaults(new {
                Description = "Active"
            });

            OnSaving((p, _) => p.Id = (int)PremiseStatusCode.Indices.ACTIVE);
        }

        public ActivePremiseStatusCodeFactory(IContainer container) : base(container) { }
    }

    public class InactivePremiseStatusCodeFactory : PremiseStatusCodeFactory
    {
        static InactivePremiseStatusCodeFactory()
        {
            Defaults(new {
                Description = "Inactive"
            });

            OnSaving((p, _) => p.Id = (int)PremiseStatusCode.Indices.INACTIVE);
        }

        public InactivePremiseStatusCodeFactory(IContainer container) : base(container) { }
    }

    public class KilledPremiseStatusCodeFactory : PremiseStatusCodeFactory
    {
        static KilledPremiseStatusCodeFactory()
        {
            Defaults(new {
                Description = "Killed"
            });

            OnSaving((p, _) => p.Id = (int)PremiseStatusCode.Indices.KILLED);
        }

        public KilledPremiseStatusCodeFactory(IContainer container) : base(container) { }
    }

    public class NonConvertedPremiseStatusCodeFactory : PremiseStatusCodeFactory
    {
        static NonConvertedPremiseStatusCodeFactory()
        {
            Defaults(new {
                Description = "Non Converted"
            });

            OnSaving((p, _) => p.Id = (int)PremiseStatusCode.Indices.NON_CONVERTED);
        }

        public NonConvertedPremiseStatusCodeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region UniquePremiseType

    public class UniquePremiseTypeFactory : PremiseTypeFactory
    {
        #region Fields

        private static int _currentCount = 1;

        #endregion

        #region Constructors

        public UniquePremiseTypeFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        protected override PremiseType Save(PremiseType entity)
        {
            entity.Abbreviation = entity.Abbreviation + _currentCount;
            entity.Description = entity.Description + _currentCount;
            _currentCount++;
            return base.Save(entity);
        }

        #endregion
    }

    #endregion

    #region Process

    public class ProcessFactory : TestDataFactory<Process>
    {
        #region Constructors

        static ProcessFactory()
        {
            var i = 0;
            Func<string> descriptionFn = () => string.Format("Process {0}", ++i);

            Defaults(new {
                Description = descriptionFn,
                Sequence = 1.01m,
                ProcessStage = typeof(ProcessStageFactory)
            });
        }

        public ProcessFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProcessStage

    public class ProcessStageFactory : TestDataFactory<ProcessStage>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "1-Source of Supply";

        #endregion

        #region Constructors

        static ProcessStageFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ProcessStageFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ProductionPreJobSafetyBrief

    public class ProductionPreJobSafetyBriefFactory : TestDataFactory<ProductionPreJobSafetyBrief>
    {
        static ProductionPreJobSafetyBriefFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Facility = typeof(FacilityFactory),
                SafetyBriefDateTime = Lambdas.GetNow,
                CreatedBy = typeof(UserFactory),
                CreatedAt = Lambdas.GetNow,
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),

                // setting all of the below makes it so none of the notes fields are required
                HaveEquipmentToDoJobSafely = true,
                HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection = true,
                ReviewedErgonomicHazards = true,
                HasStretchAndFlexBeenPerformed = true,
                ReviewedLocationOfSafetyEquipment = true,
                CrewMembersRemindedOfStopWorkAuthority = true
            });
        }

        public ProductionPreJobSafetyBriefFactory(IContainer container) : base(container) { }

        public override ProductionPreJobSafetyBrief Build(object overrides = null)
        {
            var ppjsb = base.Build(overrides);
            if (ppjsb.ProductionWorkOrder != null)
            {
                ppjsb.ProductionWorkOrder.ProductionPreJobSafetyBriefs.AddIfMissing(ppjsb);
            }

            return ppjsb;
        }
    }

    #endregion

    #region ProductionPreJobSafetyBriefWorker

    public class ProductionPreJobSafetyBriefWorkerFactory : TestDataFactory<ProductionPreJobSafetyBriefWorker>
    {
        static ProductionPreJobSafetyBriefWorkerFactory()
        {
            Defaults(new {
                ProductionPreJobSafetyBrief = typeof(ProductionPreJobSafetyBriefFactory),
                Employee = typeof(EmployeeFactory),
                SignedAt = DateTime.Now,
            });
        }

        public ProductionPreJobSafetyBriefWorkerFactory(IContainer container) : base(container) { }

        public override ProductionPreJobSafetyBriefWorker Build(object overrides = null)
        {
            var ppjsbe = base.Build(overrides);
            ppjsbe.ProductionPreJobSafetyBrief.Workers.AddIfMissing(ppjsbe);
            return ppjsbe;
        }
    }

    #endregion

    #region ProductionPrerequisite

    public class
        ProductionPrerequisiteFactory : StaticListEntityLookupFactory<ProductionPrerequisite,
            ProductionPrerequisiteFactory>
    {
        public ProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class HasLockoutRequirementProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static HasLockoutRequirementProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Has Lockout Requirement"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
        }

        public HasLockoutRequirementProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class IsConfinedSpaceProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static IsConfinedSpaceProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Is Confined Space"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.IS_CONFINED_SPACE);
        }

        public IsConfinedSpaceProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class JobSafetyChecklistProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static JobSafetyChecklistProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Job Safety Checklist"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.JOB_SAFETY_CHECKLIST);
        }

        public JobSafetyChecklistProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class AirPermitProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static AirPermitProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Air Permit"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.AIR_PERMIT);
        }

        public AirPermitProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class HotWorkProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static HotWorkProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Hot Work"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.HOT_WORK);
        }

        public HotWorkProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    public class RedTagPermitProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static RedTagPermitProductionPrerequisiteFactory()
        {
            Defaults(new {Description = "Red Tag Permit"});
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.RED_TAG_PERMIT);
        }

        public RedTagPermitProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }
    
    public class PreJobSafetyBriefProductionPrerequisiteFactory : ProductionPrerequisiteFactory
    {
        static PreJobSafetyBriefProductionPrerequisiteFactory()
        {
            Defaults(new { Description = "Pre Job Safety Brief" });
            OnSaving((a, s) => a.Id = ProductionPrerequisite.Indices.PRE_JOB_SAFETY_BRIEF);
        }

        public PreJobSafetyBriefProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProductionWorkOrderProductionPrerequisite

    public class
        ProductionWorkOrderProductionPrerequisiteFactory : TestDataFactory<ProductionWorkOrderProductionPrerequisite>
    {
        static ProductionWorkOrderProductionPrerequisiteFactory()
        {
            Defaults(new {
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                ProductionPrerequisite = typeof(ProductionPrerequisiteFactory)
            });
        }

        public ProductionWorkOrderProductionPrerequisiteFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProjectFactory

    public class ProjectFactory : TestDataFactory<Project>
    {
        static ProjectFactory()
        {
            Defaults(new {
                Name = "Project"
            });
        }

        public ProjectFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region DevelopmentProjectFactory

    public class DevelopmentProjectCategoryFactory : EntityLookupTestDataFactory<DevelopmentProjectCategory>
    {
        public DevelopmentProjectCategoryFactory(IContainer container) : base(container) { }
    }

    public class DevelopmentProjectFactory : TestDataFactory<DevelopmentProject>
    {
        static DevelopmentProjectFactory()
        {
            int i = 0;
            Func<string> descriptionFn = () => string.Format("DV Project {0}", ++i);
            Func<int> countsFn = () => i;
            Func<string> wbsFn = () => string.Format("{0}", i);

            Defaults(new {
                ProjectDescription = descriptionFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                Category = typeof(DevelopmentProjectCategoryFactory),
                BusinessUnit = typeof(BusinessUnitFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                DomesticWaterServices = countsFn,
                FireServices = countsFn,
                DomesticSanitaryServices = countsFn,
                CreatedBy = typeof(UserFactory),
                WBSNumber = wbsFn,
                ForecastedInServiceDate = Lambdas.GetNowDate
            });
        }

        public DevelopmentProjectFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ProjectType

    public class RecurringProjectTypeFactory : TestDataFactory<RecurringProjectType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ProjectType", CREATED_BY = "Factory Test";

        #endregion

        #region Constructors

        static RecurringProjectTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                CreatedBy = CREATED_BY,
                CreatedAt = Lambdas.GetNow
            });
        }

        public RecurringProjectTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PublicWaterSupply/PWSID

    public class PublicWaterSupplyFactory : TestDataFactory<PublicWaterSupply>
    {
        public const string DEFAULT_SYSTEM = "System {0}";

        public PublicWaterSupplyFactory(IContainer container) : base(container) { }

        static PublicWaterSupplyFactory()
        {
            var id = 0;
            var system = 0;
            Func<String> idFn = () => $"PWS{++id}";
            Func<String> systemFn = () => $"System{++system}";
            Defaults(new {
                Identifier = idFn,
                Status = typeof(ActivePublicWaterSupplyStatusFactory),
                System = systemFn,
                LocalCertifiedStateId = "12345ABCDE",
                Ownership = typeof(PublicWaterSupplyOwnershipFactory),
                LicensedOperatorStatus = typeof(InternalEmployeeLicensedOperatorCategoryFactory),
                Type = typeof(PublicWaterSupplyTypeFactory),
                UpdatedAt = Lambdas.GetNow,
                HasConsentOrder = false
            });
        }
    }

    #endregion

    #region PublicWaterSupplyFirmCapacity

    public class PublicWaterSupplyFirmCapacityFactory : TestDataFactory<PublicWaterSupplyFirmCapacity>
    {
        public const float CURRENT_SYSTEM_PEAK_DAILY_DEMAND_MGD = 3.14F,
                           TOTAL_SYSTEM_SOURCE_CAPACITY_MGD = 3.14F;

        public const decimal TOTAL_CAPACITY_FACILITY_SUM_MGD = 2.718M;

        public PublicWaterSupplyFirmCapacityFactory(IContainer container) : base(container) { }

        static PublicWaterSupplyFirmCapacityFactory()
        {
            Defaults(new {
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                CurrentSystemPeakDailyDemandMGD = CURRENT_SYSTEM_PEAK_DAILY_DEMAND_MGD,
                CurrentSystemPeakDailyDemandYearMonth = Lambdas.GetNow,
                TotalSystemSourceCapacityMGD = TOTAL_SYSTEM_SOURCE_CAPACITY_MGD,
                TotalCapacityFacilitySumMGD = TOTAL_CAPACITY_FACILITY_SUM_MGD,
                UpdatedAt = Lambdas.GetNow
            });
        }
    }

    #endregion

    #region PublicWaterSupplyOwnership

    public class PublicWaterSupplyOwnershipFactory : UniqueEntityLookupFactory<PublicWaterSupplyOwnership>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "AW Contract";

        #endregion

        #region Constructors

        static PublicWaterSupplyOwnershipFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public PublicWaterSupplyOwnershipFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PublicWaterSupplyPressureZone

    public class PublicWaterSupplyPressureZoneFactory : TestDataFactory<PublicWaterSupplyPressureZone>
    {
        public const string HYDRAULIC_MODEL_NAME = "Pressure Zone - 01",
                            COMMON_NAME = "Common Name";

        public const int HYDRAULIC_GRADIENT_MIN = 0,
                         HYDRAULIC_GRADIENT_MAX = 500,
                         PRESSURE_ZONE_MIN = 0,
                         PRESSURE_ZONE_MAX = 500;

        public PublicWaterSupplyPressureZoneFactory(IContainer container) : base(container) { }

        static PublicWaterSupplyPressureZoneFactory()
        {
            Defaults(new {
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                PublicWaterSupplyFirmCapacity = typeof(PublicWaterSupplyFirmCapacityFactory),
                HydraulicModelName = HYDRAULIC_MODEL_NAME,
                CommonName = COMMON_NAME,
                HydraulicGradientMin = HYDRAULIC_GRADIENT_MIN,
                HydraulicGradientMax = HYDRAULIC_GRADIENT_MAX,
                PressureMin = PRESSURE_ZONE_MIN,
                PressureMax = PRESSURE_ZONE_MAX
            });
        }
    }

    #endregion

    #region PublicWaterSupplyLicensedOperator

    public class PublicWaterSupplyLicensedOperatorFactory : TestDataFactory<PublicWaterSupplyLicensedOperator>
    {
        public PublicWaterSupplyLicensedOperatorFactory(IContainer container) : base(container) { }

        static PublicWaterSupplyLicensedOperatorFactory()
        {
            Defaults(new {
                LicensedOperator = typeof(OperatorLicenseFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory)
            });
        }
    }

    #endregion

    #region PublicWaterSupplyStatus

    public class PublicWaterSupplyStatusFactory : StaticListEntityLookupFactory<PublicWaterSupplyStatus, PublicWaterSupplyStatusFactory>
    {
        public PublicWaterSupplyStatusFactory(IContainer container) : base(container) { }
    }

    public class ActivePublicWaterSupplyStatusFactory : PublicWaterSupplyStatusFactory
    {
        #region Constructors

        static ActivePublicWaterSupplyStatusFactory()
        {
            Defaults(new { Description = "Active" });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.ACTIVE);
        }

        public ActivePublicWaterSupplyStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PendingPublicWaterSupplyStatusFactory : PublicWaterSupplyStatusFactory
    {
        #region Constructors

        static PendingPublicWaterSupplyStatusFactory()
        {
            Defaults(new { Description = "Pending" });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.PENDING);
        }

        public PendingPublicWaterSupplyStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PendingMergerPublicWaterSupplyStatusFactory : PublicWaterSupplyStatusFactory
    {
        #region Constructors

        static PendingMergerPublicWaterSupplyStatusFactory()
        {
            Defaults(new { Description = "Pending Merger" });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.PENDING_MERGER);
        }

        public PendingMergerPublicWaterSupplyStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InactivePublicWaterSupplyStatusFactory : PublicWaterSupplyStatusFactory
    {
        #region Constructors

        static InactivePublicWaterSupplyStatusFactory()
        {
            Defaults(new { Description = "Inactive" });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.INACTIVE);
        }

        public InactivePublicWaterSupplyStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InactiveSeeNotePublicWaterSupplyStatusFactory : PublicWaterSupplyStatusFactory
    {
        #region Constructors

        static InactiveSeeNotePublicWaterSupplyStatusFactory()
        {
            Defaults(new { Description = "Inactive -see note" });
            OnSaving((a, s) => a.Id = (int)PublicWaterSupplyStatus.Indices.INACTIVE_SEE_NOTE);
        }

        public InactiveSeeNotePublicWaterSupplyStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region PublicWaterSupplyType

    public class PublicWaterSupplyTypeFactory : UniqueEntityLookupFactory<PublicWaterSupplyType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Community Water System";

        #endregion

        #region Constructors

        static PublicWaterSupplyTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public PublicWaterSupplyTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ReadingFactory

    public class ReadingFactory : TestDataFactory<Reading>
    {
        static ReadingFactory()
        {
            Defaults(new {
                Sensor = typeof(SensorFactory)
            });
        }

        public ReadingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RecurringFrequencyUnit

    public class
        RecurringFrequencyUnitFactory : StaticListEntityLookupFactory<RecurringFrequencyUnit,
            RecurringFrequencyUnitFactory>
    {
        public RecurringFrequencyUnitFactory(IContainer container) : base(container) { }
    }

    public class YearlyRecurringFrequencyUnitFactory : RecurringFrequencyUnitFactory
    {
        static YearlyRecurringFrequencyUnitFactory()
        {
            Defaults(new {Description = RecurringFrequencyUnit.YEAR});
            OnSaving((a, s) => a.Id = (int)RecurringFrequencyUnit.Indices.YEAR);
        }

        public YearlyRecurringFrequencyUnitFactory(IContainer container) : base(container) { }
    }

    public class MonthlyRecurringFrequencyUnitFactory : RecurringFrequencyUnitFactory
    {
        static MonthlyRecurringFrequencyUnitFactory()
        {
            Defaults(new {Description = RecurringFrequencyUnit.MONTH});
            OnSaving((a, s) => a.Id = (int)RecurringFrequencyUnit.Indices.MONTH);
        }

        public MonthlyRecurringFrequencyUnitFactory(IContainer container) : base(container) { }
    }

    public class WeeklyRecurringFrequencyUnitFactory : RecurringFrequencyUnitFactory
    {
        static WeeklyRecurringFrequencyUnitFactory()
        {
            Defaults(new {Description = RecurringFrequencyUnit.WEEK});
            OnSaving((a, s) => a.Id = (int)RecurringFrequencyUnit.Indices.WEEK);
        }

        public WeeklyRecurringFrequencyUnitFactory(IContainer container) : base(container) { }
    }

    public class DailyRecurringFrequencyUnitFactory : RecurringFrequencyUnitFactory
    {
        static DailyRecurringFrequencyUnitFactory()
        {
            Defaults(new {Description = RecurringFrequencyUnit.DAY});
            OnSaving((a, s) => a.Id = (int)RecurringFrequencyUnit.Indices.DAY);
        }

        public DailyRecurringFrequencyUnitFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RecurringProject

    public class RecurringProjectFactory : TestDataFactory<RecurringProject>
    {
        #region Constants

        public const string PROJECT_TITLE = "Project Title", PROJECT_DESCRIPTION = "Project Description";
        public const int DISTRICT = 9, NJAW_ESTIMATE = 1337, LENGTH = 10;

        #endregion

        #region Constructors

        static RecurringProjectFactory()
        {
            Defaults(new {
                ProjectTitle = PROJECT_TITLE,
                District = DISTRICT,
                NJAWEstimate = NJAW_ESTIMATE,
                CreatedBy = typeof(UserFactory),
                CreatedAt = Lambdas.GetNow,
                OperatingCenter = typeof(OperatingCenterFactory),
                AssetCategory = typeof(AssetCategoryFactory),
                AssetType = typeof(MainAssetTypeFactory),
                Status = typeof(ProposedRecurringProjectStatusFactory),
                Town = typeof(TownFactory),
                RecurringProjectType = typeof(RecurringProjectTypeFactory),
                ProposedDiameter = typeof(PipeDiameterFactory),
                ProposedPipeMaterial = typeof(PipeMaterialFactory),
                AcceleratedAssetInvestmentCategory = typeof(AssetInvestmentCategoryFactory),
                Coordinate = typeof(CoordinateFactory),
                ProjectDescription = PROJECT_DESCRIPTION,
                ProposedLength = LENGTH
            });
        }

        public RecurringProjectFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RecurringProjectEndorsement

    public class RecurringProjectEndorsementFactory : TestDataFactory<RecurringProjectEndorsement>
    {
        #region Constants

        public const string COMMENT = "RecurringProjectEndorsement";

        #endregion

        #region Constructors

        static RecurringProjectEndorsementFactory()
        {
            Defaults(new {
                Comment = COMMENT,
                EndorsementDate = Lambdas.GetNow,
                RecurringProject = typeof(RecurringProjectFactory),
                EndorsementStatus = typeof(EndorsementStatusFactory)
            });
        }

        public RecurringProjectEndorsementFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RecurringProjectMain

    public class RecurringProjectMainFactory : TestDataFactory<RecurringProjectMain>
    {
        #region Constants

        public const string COMMENT = "RecurringProjectEndorsement";

        #endregion

        #region Constructors

        static RecurringProjectMainFactory()
        {
            Defaults(new {
                RecurringProject = typeof(RecurringProjectFactory),
                Layer = "Some layer",
                Guid = Guid.NewGuid().ToString(),
                Length = 1m,
            });
        }

        public RecurringProjectMainFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RecurringProjectMain

    public class RecurringProjectPipeDataLookupValueFactory : TestDataFactory<RecurringProjectPipeDataLookupValue>
    {
        #region Constants

        public const string COMMENT = "RecurringProjectEndorsement";

        #endregion

        #region Constructors

        static RecurringProjectPipeDataLookupValueFactory()
        {
            Defaults(new {
                RecurringProject = typeof(RecurringProjectFactory),
                PipeDataLookupValue = typeof(PipeDataLookupValueFactory)
            });
        }

        public RecurringProjectPipeDataLookupValueFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RecurringProjectStatus

    public class
        RecurringProjectStatusFactory : StaticListEntityLookupFactory<RecurringProjectStatus,
            RecurringProjectStatusFactory>
    {
        #region Constructors

        public RecurringProjectStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ProposedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Proposed";

        public ProposedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static ProposedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.PROPOSED);
        }
    }

    public class CompleteRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Complete";

        public CompleteRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static CompleteRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.COMPLETE);
        }
    }

    public class SubmittedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Submitted";

        public SubmittedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static SubmittedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.SUBMITTED);
        }
    }

    public class CanceledRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Canceled";

        public CanceledRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static CanceledRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.CANCELED);
        }
    }

    public class ManagerEndorsedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Manager Endorsed";

        public ManagerEndorsedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static ManagerEndorsedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.MANAGER_ENDORSED);
        }
    }

    public class ReviewedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Reviewed";

        public ReviewedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static ReviewedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.REVIEWED);
        }
    }

    public class APEndorsedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "AP Endorsed";

        public APEndorsedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static APEndorsedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.AP_ENDORSED);
        }
    }

    public class MunicipalRelocationApprovedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "Municipal Relocation Approved";

        public MunicipalRelocationApprovedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static MunicipalRelocationApprovedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.MUNICIPAL_RELOCATION_APPROVED);
        }
    }

    public class APApprovedRecurringProjectStatusFactory : RecurringProjectStatusFactory
    {
        public const string DEFAULT_DESCRIPTION = "AP Approved";

        public APApprovedRecurringProjectStatusFactory(IContainer container) : base(container) { }

        static APApprovedRecurringProjectStatusFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)RecurringProjectStatus.Indices.AP_APPROVED);
        }
    }

    #endregion

    #region RedTagPermit

    public class RedTagPermitFactory : TestDataFactory<RedTagPermit>
    {
        public RedTagPermitFactory(IContainer container) : base(container) { }

        static RedTagPermitFactory()
        {
            Defaults(new {
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                Equipment = typeof(EquipmentFactory),
                PersonResponsible = typeof(EmployeeFactory),
                ProtectionType = typeof(RedTagPermitProtectionTypeFactory),
                AuthorizedBy = typeof(EmployeeFactory),
                AreaProtected = "This is a description for the area being protected.",
                ReasonForImpairment = "This is a description for the reason of impairment.",
                NumberOfTurnsToClose = 10,
                FireProtectionEquipmentOperator = "This is a description for the fire protection equipment operator.",
                EquipmentImpairedOn = Lambdas.GetNow,
                CreatedAt = Lambdas.GetNow,
            });
        }
    }

    #endregion

    #region RedTagPermitProtectionType

    public class RedTagPermitProtectionTypeFactory : UniqueEntityLookupFactory<RedTagPermitProtectionType>
    {
        public RedTagPermitProtectionTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Regulation

    public class RegulationAgencyFactory : EntityLookupTestDataFactory<RegulationAgency>
    {
        public RegulationAgencyFactory(IContainer container) : base(container) { }
    }

    public class RegulationStatusFactory : EntityLookupTestDataFactory<RegulationStatus>
    {
        public RegulationStatusFactory(IContainer container) : base(container) { }
    }

    public class RegulationFactory : TestDataFactory<Regulation>
    {
        static RegulationFactory()
        {
            var i = 0;
            Func<string> titleFn = (() => String.Format("Regulation {0}", ++i));
            Defaults(new {
                Title = titleFn,
                Agency = typeof(RegulationAgencyFactory),
                Status = typeof(RegulationStatusFactory),
                CreatedAt = Lambdas.GetNow
            });
        }

        public RegulationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RepairTimeRange

    public class RepairTimeRangeFactory : UniqueEntityLookupFactory<RepairTimeRange>
    {
        public RepairTimeRangeFactory(IContainer container) : base(container) { }
    }

    public class FourToSixRepairTimeRangeFactory : RepairTimeRangeFactory
    {
        static FourToSixRepairTimeRangeFactory()
        {
            Defaults(new {
                Description = "4-6"
            });

            OnSaving((a, s) => a.Id = RepairTimeRange.Indices.FOUR_TO_SIX);
        }

        public FourToSixRepairTimeRangeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RequisitionFactory

    public class RequisitionFactory : TestDataFactory<Requisition>
    {
        static RequisitionFactory()
        {
            Defaults(new {
                WorkOrder = typeof(WorkOrderFactory),
                RequisitionType = typeof(RequisitionTypeFactory),
                SAPRequisitionNumber = "123456789"
            });
        }

        public RequisitionFactory(IContainer container) : base(container) { }
    }
    
    #endregion

    #region RequisitionTypeFactory

    public class RequisitionTypeFactory : EntityLookupTestDataFactory<RequisitionType>
    {
        static RequisitionTypeFactory() { }

        public RequisitionTypeFactory(IContainer container) : base(container) { }
    }
    
    #endregion

    #region RestorationAccountingCode

    public class RestorationAccountingCodeFactory : TestDataFactory<RestorationAccountingCode>
    {
        static RestorationAccountingCodeFactory()
        {
            var i = 0;
            string CodeFn() => (i++).ToString();

            Defaults(new {
                Code = (Func<string>)CodeFn
            });
        }

        public RestorationAccountingCodeFactory(IContainer container) : base(container) { }
    }
    
    #endregion

    #region Restoration

    public class RestorationFactory : TestDataFactory<Restoration>
    {
        #region Constants

        public const decimal LINEAR_FEET_OF_CURB = 50m;

        #endregion

        #region Constructors

        static RestorationFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                WorkOrder = typeof(WorkOrderFactory),
                Town = typeof(TownFactory),
                ResponsePriority = typeof(RestorationResponsePriorityFactory),
                RestorationType = typeof(RestorationTypeFactory),
                LinearFeetOfCurb = LINEAR_FEET_OF_CURB,
                EightInchStabilizeBaseByCompanyForces = false,
                AssignedContractor = typeof(ContractorFactory)
            });
        }

        public RestorationFactory(IContainer container) : base(container) { }

        #endregion

        public override Restoration Build(object overrides = null)
        {
            var ret = base.Build(overrides);
            if (ret.WorkOrder != null)
            {
                // These need to be kept in sync with the workorder.
                ret.OperatingCenter = ret.WorkOrder.OperatingCenter;
                ret.Town = ret.WorkOrder.Town;
            }

            return ret;
        }
    }

    #endregion

    #region RestorationType

    public class RestorationTypeFactory : UniqueEntityLookupFactory<RestorationType>
    {
        #region Constructors

        static RestorationTypeFactory()
        {
            Defaults(new {
                //   Description = "RestorationType"
            });
        }

        public RestorationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RestorationTypeCost

    public class RestorationTypeCostFactory : TestDataFactory<RestorationTypeCost>
    {
        #region Constants

        public const decimal LINEAR_FEET_OF_CURB = 50m;

        #endregion

        #region Constructors

        static RestorationTypeCostFactory()
        {
            Defaults(new {
                RestorationType = typeof(RestorationTypeFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                Cost = 2d,
                FinalCost = 6 // The live DB has 6 for the value in every row. No idea what 6 is.
            });
        }

        public RestorationTypeCostFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override RestorationTypeCost Create(object overrides = null)
        {
            var ret = base.Create(overrides);
            if (!ret.RestorationType.RestorationTypeCosts.Contains(ret))
            {
                ret.RestorationType.RestorationTypeCosts.Add(ret);
            }

            return ret;
        }

        #endregion
    }

    #endregion

    #region RestorationMethod

    public class RestorationMethodFactory : UniqueEntityLookupFactory<RestorationMethod>
    {
        #region Constructors

        public RestorationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RestorationResponsePriority

    public class RestorationResponsePriorityFactory : UniqueEntityLookupFactory<RestorationResponsePriority>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Restoration Type";

        #endregion

        #region Constructors

        static RestorationResponsePriorityFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public RestorationResponsePriorityFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RiskRegisterAsset

    public class RiskRegisterAssetFactory : TestDataFactory<RiskRegisterAsset>
    {
        public RiskRegisterAssetFactory(IContainer container) : base(container) { }

        static RiskRegisterAssetFactory()
        {
            Defaults(new {
                RiskRegisterAssetCategory = typeof(RiskRegisterAssetCategoryFactory),
                RiskRegisterAssetGroup = typeof(RiskRegisterAssetGroupFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                WasteWaterSystem = typeof(WasteWaterSystemFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                State = typeof(StateFactory),
                Facility = typeof(FacilityFactory),
                Equipment = typeof(EquipmentFactory),
                Coordinate = typeof(CoordinateFactory),
                Employee = typeof(EmployeeFactory),
                InterimMitigationMeasuresTaken = "These are some measures taken to temporarily mitigate this risk.",
                InterimMitigationMeasuresTakenAt = Lambdas.GetNow,
                FinalMitigationMeasuresTaken = "These are some measures taken to finally mitigate this risk.",
                FinalMitigationMeasuresTakenAt = Lambdas.GetNow,
                ImpactDescription = "Description of impact",
                RiskDescription = "Description of risk",
                IsProjectInComprehensivePlanningStudy = false,
                IsProjectInCapitalPlan = false,
                RelatedWorkBreakdownStructure = "WBS #14234",
                RiskQuadrant = 2,
                IdentifiedAt = Lambdas.GetNow,
            });
        }
    }

    #endregion

    #region RiskRegisterAssetCategory

    public class RiskRegisterAssetCategoryFactory : UniqueEntityLookupFactory<RiskRegisterAssetCategory>
    {
        public RiskRegisterAssetCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RiskRegisterAssetGroup

    public class RiskRegisterAssetGroupFactory : UniqueEntityLookupFactory<RiskRegisterAssetGroup>
    {
        public RiskRegisterAssetGroupFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region RoadwayImprovementNotification

    public class RoadwayImprovementNotificationFactory : TestDataFactory<RoadwayImprovementNotification>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "RoadwayImprovementNotification";

        #endregion

        #region Constructors

        static RoadwayImprovementNotificationFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                RoadwayImprovementNotificationEntity = typeof(RoadwayImprovementNotificationEntityFactory),
                RoadwayImprovementNotificationStatus = typeof(RoadwayImprovementNotificationStatusFactory),
                Description = DEFAULT_DESCRIPTION,
                ExpectedProjectStartDate = Lambdas.GetNow().AddDays(10),
                DateReceived = Lambdas.GetYesterday(),
                Coordinate = typeof(CoordinateFactory)
            });
        }

        public RoadwayImprovementNotificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RoadwayImprovementNotificationStreet

    public class RoadwayImprovementNotificationStreetFactory : TestDataFactory<RoadwayImprovementNotificationStreet>
    {
        #region Constructors

        static RoadwayImprovementNotificationStreetFactory()
        {
            Defaults(new {
                RoadwayImprovementNotification = typeof(RoadwayImprovementNotificationFactory),
                Street = typeof(StreetFactory),
                RoadwayImprovementNotificationStreetStatus = typeof(RoadwayImprovementNotificationStreetStatusFactory)
            });
        }

        public RoadwayImprovementNotificationStreetFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RoadwayImprovementNotificationEntity

    public class RoadwayImprovementNotificationEntityFactory :
        UniqueEntityLookupFactory<RoadwayImprovementNotificationEntity>
    {
        public RoadwayImprovementNotificationEntityFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RoadwayImprovementNotificationStatus

    public class RoadwayImprovementNotificationStatusFactory :
        UniqueEntityLookupFactory<RoadwayImprovementNotificationStatus>
    {
        public RoadwayImprovementNotificationStatusFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region RoadwayImprovementNotificationStreetStatus

    public class RoadwayImprovementNotificationStreetStatusFactory :
        UniqueEntityLookupFactory<RoadwayImprovementNotificationStreetStatus>
    {
        public RoadwayImprovementNotificationStreetStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region RoleFactory

    public class RoleFactory : TestDataFactory<Role>
    {
        #region Constructors

        static RoleFactory()
        {
            Defaults(new {
                User = typeof(UserFactory),
                Module = typeof(ModuleFactory),
                Action = typeof(ReadActionFactory),
            });
        }

        public RoleFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Role Build(object overrides = null)
        {
            var role = base.Build(overrides);
            role.Application = role.Module.Application;

            if (role.OperatingCenter == null)
            {
                role.OperatingCenter = role.User.DefaultOperatingCenter;
            }

            if (!role.User.Roles.Contains(role))
            {
                role.User.Roles.Add(role);
            }

            return role;
        }

        public Role Build(RoleModules module, OperatingCenter opCenter, User user = null)
        {
            return Build(new {
                Module = new ModuleFactory(_container).Build(new {Id = (int)module}),
                OperatingCenter = opCenter,
                User = (object)user ?? typeof(UserFactory)
            });
        }

        [Obsolete("Don't use this. There's no current use case where it makes sense to pass a null User.")]
        public Role Create(RoleModules module, OperatingCenter opCenter, User user = null)
        {
            var createdRole = Create(new {
                Module = new ModuleFactory(_container).Create(new {Id = (int)module}),
                OperatingCenter = opCenter,
                User = (object)user ?? typeof(UserFactory)
            });
            // This doesn't add the Role directly to the User.Roles collection
            // which means you need to evict/refresh to get things to work correctly
            // if you're using the User.Roles property.
            return createdRole;
        }

        public Role Create(RoleModules module, OperatingCenter opCenter, User user, RoleActions action)
        {
            Type actionFactory = null;
            switch (action)
            {
                case RoleActions.Read:
                    actionFactory = typeof(ReadActionFactory);
                    break;

                case RoleActions.Add:
                    actionFactory = typeof(AddActionFactory);
                    break;

                case RoleActions.Edit:
                    actionFactory = typeof(EditActionFactory);
                    break;

                case RoleActions.Delete:
                    actionFactory = typeof(DeleteActionFactory);
                    break;

                case RoleActions.UserAdministrator:
                    actionFactory = typeof(AdminActionFactory);
                    break;
            }

            var createdRole = Create(new {
                Module = new ModuleFactory(_container).Create(new {Id = (int)module}),
                OperatingCenter = opCenter,
                User = (object)user ?? typeof(UserFactory),
                Action = actionFactory
            });
            createdRole.User.Roles.Add(createdRole);
            return createdRole;
        }

        #endregion
    }

    public class WildcardOpCenterRoleFactory : RoleFactory
    {
        #region Constructors

        public WildcardOpCenterRoleFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Role Build(object overrides = null)
        {
            var role = base.Build(overrides);
            // Null OperatingCenter = Any OperatingCenter. 
            role.OperatingCenter = null;
            return role;
        }

        #endregion
    }

    #endregion

    #region RoleGroupFactory

    public class RoleGroupFactory : TestDataFactory<RoleGroup>
    {
        #region Constructors

        static RoleGroupFactory()
        {
            var groupNumber = 0;
            Func<string> nameGetter = () => {
                groupNumber++;
                return $"Role Group #{groupNumber}";
            };
            Defaults(new {
                Name = nameGetter
            });
        }

        public RoleGroupFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region RoleGroupRoleFactory

    public class RoleGroupRoleFactory : TestDataFactory<RoleGroupRole>
    {
        #region Constructors

        static RoleGroupRoleFactory()
        {
            Defaults(new {
                RoleGroup = typeof(RoleGroupFactory),
                Module = typeof(ModuleFactory),
                Action = typeof(ReadActionFactory),
            });
        }

        public RoleGroupRoleFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override RoleGroupRole Build(object overrides = null)
        {
            var role = base.Build(overrides);
            role.RoleGroup.Roles.AddIfMissing(role);
            return role;
        }

        #endregion
    }

    #endregion

    #region SampleIdMatrix

    public class SampleIdMatrixFactory : TestDataFactory<SampleIdMatrix>
    {
        static SampleIdMatrixFactory()
        {
            Defaults(new {
                SampleSite = typeof(SampleSiteFactory)
            });
        }

        public SampleIdMatrixFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SamplePlan

    public class SamplePlanFactory : TestDataFactory<SamplePlan>
    {
        public SamplePlanFactory(IContainer container) : base(container) { }

        static SamplePlanFactory()
        {
            var i = 0;
            Func<string> laboratoryFn = () => $"Laboratory {++i}";

            Defaults(new {
                NameOfCertifiedLaboratory = laboratoryFn,
                PWSID = typeof(PublicWaterSupplyFactory),
                ContactPerson = typeof(EmployeeFactory),
                MonitoringPeriodFrom = Lambdas.GetYesterday,
                MonitoringPeriodTo = Lambdas.GetYesterday
            });
        }
    }

    #endregion

    #region SampleSite

    public class SampleSiteFactory : TestDataFactory<SampleSite>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SampleSite";

        #endregion

        #region Constructors

        static SampleSiteFactory()
        {
            var i = 0;
            // we don't allow numbers or hyphens in the LocationNameDescription field 
            Func<string> locationNameDescriptionFn = () => $"Sample Site {(++i).ToWords().Replace("-", "")}";
            Func<string> commonSiteNameFn = () => $"common site name {i}";
            Defaults(new {
                LocationNameDescription = locationNameDescriptionFn,
                CommonSiteName = commonSiteNameFn,
                OperatingCenter = typeof(OperatingCenterFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                Premise = typeof(PremiseFactory),
                BactiSite = false,
                IsComplianceSampleSite = false,
                IsProcessSampleSite = false,
                IsResearchSampleSite = false,
                IsLimsLocation = false,
                SampleIdMatrices = new List<SampleIdMatrix>()
            });
        }

        public SampleSiteFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SampleSiteAddressLocation

    public class SampleSiteAddressLocationTypeFactory : StaticListEntityLookupFactory<SampleSiteAddressLocationType, SampleSiteAddressLocationTypeFactory>
    {
        public SampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class FacilitySampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        public const string DESCRIPTION = "Facility";

        static FacilitySampleSiteAddressLocationTypeFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.FACILITY);
        }

        public FacilitySampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class PremiseSampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        public const string DESCRIPTION = "Premise";

        static PremiseSampleSiteAddressLocationTypeFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.PREMISE);
        }

        public PremiseSampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class ValveSampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        static ValveSampleSiteAddressLocationTypeFactory() 
        {
            Defaults(new { Description = "Valve" });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.VALVE);
        }

        public ValveSampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class HydrantSampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        static HydrantSampleSiteAddressLocationTypeFactory() 
        {
            Defaults(new { Description = "Hydrant" });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.HYDRANT);
        }

        public HydrantSampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class CustomSampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        public const string DESCRIPTION = "Custom";

        static CustomSampleSiteAddressLocationTypeFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.CUSTOM);
        }

        public CustomSampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class PendingAcquisitionSampleSiteAddressLocationTypeFactory : SampleSiteAddressLocationTypeFactory
    {
        public const string DESCRIPTION = "Pending Acquisition";

        static PendingAcquisitionSampleSiteAddressLocationTypeFactory()
        {
            Defaults(new { Description = DESCRIPTION });
            OnSaving((a, s) => a.Id = (int)SampleSiteAddressLocationType.Indices.PENDING_ACQUISITION);
        }

        public PendingAcquisitionSampleSiteAddressLocationTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteAvailability

    public class SampleSiteAvailabilityFactory : UniqueEntityLookupFactory<SampleSiteAvailability>
    {
        public SampleSiteAvailabilityFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteCollectionType

    public class SampleSiteCollectionTypeFactory : StaticListEntityLookupFactory<SampleSiteCollectionType, SampleSiteCollectionTypeFactory>
    {
        public SampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class RawSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static RawSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "Raw" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.RAW);
        }

        public RawSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class InPlantSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static InPlantSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "InPlant" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.IN_PLANT);
        }

        public InPlantSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class EntryPointSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static EntryPointSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "Entry Point" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.ENTRY_POINT);
        }

        public EntryPointSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class InterconnectSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static InterconnectSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "Interconnect" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.INTERCONNECT);
        }

        public InterconnectSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class DistributionSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static DistributionSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "Distribution" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.DISTRIBUTION);
        }

        public DistributionSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    public class WasteWaterSampleSiteCollectionTypeFactory : SampleSiteCollectionTypeFactory
    {
        static WasteWaterSampleSiteCollectionTypeFactory()
        {
            Defaults(new { Description = "Wastewater" });
            OnSaving((a, s) => a.Id = SampleSiteCollectionType.Indices.WASTEWATER);
        }

        public WasteWaterSampleSiteCollectionTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteCustomerContactMethod

    public class SampleSiteCustomerContactMethodFactory : StaticListEntityLookupFactory<SampleSiteCustomerContactMethod, SampleSiteCustomerContactMethodFactory>
    {
        #region Constructors

        public SampleSiteCustomerContactMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class EmailSampleSiteCustomerContactMethodFactory : SampleSiteCustomerContactMethodFactory
    {
        #region Constructors

        static EmailSampleSiteCustomerContactMethodFactory()
        {
            Defaults(new { Description = "Email"});
            OnSaving((a, s) => a.Id = SampleSiteCustomerContactMethod.Indices.EMAIL);
        }

        public EmailSampleSiteCustomerContactMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MailSampleSiteCustomerContactMethodFactory : SampleSiteCustomerContactMethodFactory
    {
        #region Constructors

        static MailSampleSiteCustomerContactMethodFactory()
        {
            Defaults(new { Description = "Mail" });
            OnSaving((a, s) => a.Id = SampleSiteCustomerContactMethod.Indices.MAIL);
        }

        public MailSampleSiteCustomerContactMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PhoneSampleSiteCustomerContactMethodFactory : SampleSiteCustomerContactMethodFactory
    {
        #region Constructors

        static PhoneSampleSiteCustomerContactMethodFactory()
        {
            Defaults(new { Description = "Phone" });
            OnSaving((a, s) => a.Id = SampleSiteCustomerContactMethod.Indices.PHONE);
        }

        public PhoneSampleSiteCustomerContactMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TextMessageSampleSiteCustomerContactMethodFactory : SampleSiteCustomerContactMethodFactory
    {
        #region Constructors

        static TextMessageSampleSiteCustomerContactMethodFactory()
        {
            Defaults(new { Description = "Text Message" });
            OnSaving((a, s) => a.Id = SampleSiteCustomerContactMethod.Indices.TEXT_MESSAGE);
        }

        public TextMessageSampleSiteCustomerContactMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SampleSiteInactivationReason

    public class SampleSiteInactivationReasonFactory : StaticListEntityLookupFactory<SampleSiteInactivationReason, SampleSiteInactivationReasonFactory>
    {
        public SampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class CustomerDeclinedProgramSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static CustomerDeclinedProgramSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Customer Declined Program" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.CUSTOMER_DECLINED_PROGRAM);
        }

        public CustomerDeclinedProgramSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class CustomerOptedOutSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static CustomerOptedOutSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Customer Opted Out" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.CUSTOMER_OPTED_OUT);
        }

        public CustomerOptedOutSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class CustomerServiceLineReplacedSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static CustomerServiceLineReplacedSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Customer Service Line Replaced" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.CUSTOMER_SERVICE_LINE_REPLACED);
        }

        public CustomerServiceLineReplacedSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class CompanyServiceLineReplacedSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static CompanyServiceLineReplacedSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Company Service Line Replaced" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.COMPANY_SERVICE_LINE_REPLACED);
        }

        public CompanyServiceLineReplacedSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class InternalPlumbingReplacedSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static InternalPlumbingReplacedSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Internal Plumbing Replaced" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.INTERNAL_PLUMBING_REPLACED);
        }

        public InternalPlumbingReplacedSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class BuildingDemolishedSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static BuildingDemolishedSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Building Demolished" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.BUILDING_DEMOLISHED);
        }

        public BuildingDemolishedSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class OtherSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static OtherSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "Other" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.OTHER);
        }

        public OtherSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    public class NewServiceDetailsSampleSiteInactivationReasonFactory : SampleSiteInactivationReasonFactory
    {
        static NewServiceDetailsSampleSiteInactivationReasonFactory()
        {
            Defaults(new { Description = "New Service Details" });
            OnSaving((a, s) => a.Id = SampleSiteInactivationReason.Indices.NEW_SERVICE_DETAILS);
        }

        public NewServiceDetailsSampleSiteInactivationReasonFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteLeadCopperTierClassification

    public class SampleSiteLeadCopperTierClassificationFactory : StaticListEntityLookupFactory<SampleSiteLeadCopperTierClassification, SampleSiteLeadCopperTierClassificationFactory>
    {
        #region Constructors

        public SampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierOneSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierOneSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 1- Single Family Residences with Lead Pipe or Lead Service Lines" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_1_SINGLE_FAMILY_WITH_LEAD_PIPE_OR_LEAD_SERVICE_LINES);
        }

        public TierOneSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierTwoSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierTwoSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 2- Building & Multifamily Residences with Lead Pipe or Lead Service Lines" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_2_BUILDING_AND_MULTI_FAMILY_RESIDENCES_WITH_LEAD_PIPES_OR_LEAD_SERVICE_LINES);
        }

        public TierTwoSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierThreeSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierThreeSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_3_SINGLE_FAMILY_RESIDENCES_WITH_COPPER_PIPES_AND_LEAD_SOLDER_INSTALLED_BEFORE_1983);
        }

        public TierThreeSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OtherSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static OtherSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Other" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.OTHER);
        }

        public OtherSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierOneNoTextSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierOneNoTextSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 1" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_1_NO_TEXT);
        }

        public TierOneNoTextSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierTwoNoTextSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierTwoNoTextSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 2" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_2_NO_TEXT);
        }

        public TierTwoNoTextSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class TierThreeNoTextSampleSiteLeadCopperTierClassificationFactory : SampleSiteLeadCopperTierClassificationFactory
    {
        #region Constructors

        static TierThreeNoTextSampleSiteLeadCopperTierClassificationFactory()
        {
            Defaults(new { Description = "Tier 3" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperTierClassification.Indices.TIER_3_NO_TEXT);
        }

        public TierThreeNoTextSampleSiteLeadCopperTierClassificationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SampleSiteLeadCopperTierSampleCategory

    public class SampleSiteLeadCopperTierSampleCategoryFactory : UniqueEntityLookupFactory<SampleSiteLeadCopperTierSampleCategory>
    {
        static SampleSiteLeadCopperTierSampleCategoryFactory()
        {
            Defaults(new { DisplayValue = "i" });
        }

        public SampleSiteLeadCopperTierSampleCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteLeadCopperValidationMethod

    public class SampleSiteLeadCopperValidationMethodFactory : StaticListEntityLookupFactory<SampleSiteLeadCopperValidationMethod, SampleSiteLeadCopperValidationMethodFactory>
    {
        #region Constructors

        public SampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class VisualConfirmationSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static VisualConfirmationSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Visual confirmation of lead pipe on utility or customer side" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.VISUAL_CONFIRMATION);
        }

        public VisualConfirmationSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class LeadSwapTestSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static LeadSwapTestSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Lead swab test on customer plumbing and three (3) non-consecutive joints" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.LEAD_SWAB_TEST);
        }

        public LeadSwapTestSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class BuildingConstructionDocumentSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static BuildingConstructionDocumentSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Document of building construction after 1986 (Tier 3 only!)" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.BUILD_CONSTRUCTION_DOCUMENT);
        }

        public BuildingConstructionDocumentSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PendingSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static PendingSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Pending" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.PENDING);
        }

        public PendingSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CustomerSurveyResultsSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static CustomerSurveyResultsSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Customer Survey Results" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.CUSTOMER_SURVEY_RESULTS);
        }

        public CustomerSurveyResultsSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class HistoricDocumentationSampleSiteLeadCopperValidationMethodFactory : SampleSiteLeadCopperValidationMethodFactory
    {
        #region Constructors

        static HistoricDocumentationSampleSiteLeadCopperValidationMethodFactory()
        {
            Defaults(new { Description = "Historic Documentation" });
            OnSaving((a, s) => a.Id = SampleSiteLeadCopperValidationMethod.Indices.HISTORIC_DOCUMENTATION);
        }

        public HistoricDocumentationSampleSiteLeadCopperValidationMethodFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SampleSiteLocationType

    public class SampleSiteLocationTypeFactory : StaticListEntityLookupFactory<SampleSiteLocationType, SampleSiteLocationTypeFactory>
    {
        #region Constructors

        public SampleSiteLocationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PrimarySampleSiteLocationTypeFactory : SampleSiteLocationTypeFactory
    {
        #region Constructors

        static PrimarySampleSiteLocationTypeFactory()
        {
            Defaults(new { Description = "Primary", });
            OnSaving((a, s) => a.Id = SampleSiteLocationType.Indices.PRIMARY);
        }

        public PrimarySampleSiteLocationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class UpstreamSampleSiteLocationTypeFactory : SampleSiteLocationTypeFactory
    {
        #region Constructors

        static UpstreamSampleSiteLocationTypeFactory()
        {
            Defaults(new { Description = "Upstream", });
            OnSaving((a, s) => a.Id = SampleSiteLocationType.Indices.UPSTREAM);
        }

        public UpstreamSampleSiteLocationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class DownstreamSampleSiteLocationTypeFactory : SampleSiteLocationTypeFactory
    {
        #region Constructors

        static DownstreamSampleSiteLocationTypeFactory()
        {
            Defaults(new { Description = "Downstream", });
            OnSaving((a, s) => a.Id = SampleSiteLocationType.Indices.DOWNSTREAM);
        }

        public DownstreamSampleSiteLocationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class GroundwaterSampleSiteLocationTypeFactory : SampleSiteLocationTypeFactory
    {
        #region Constructors

        static GroundwaterSampleSiteLocationTypeFactory()
        {
            Defaults(new { Description = "Groundwater", });
            OnSaving((a, s) => a.Id = SampleSiteLocationType.Indices.GROUNDWATER);
        }

        public GroundwaterSampleSiteLocationTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SampleSiteProfile

    public class SampleSiteProfileFactory : TestDataFactory<SampleSiteProfile>
    {
        static SampleSiteProfileFactory()
        {
            var i = 100;
            Func<int> numberFn = () => ++i;
            Defaults(new {
                Number = numberFn,
                SampleSiteProfileAnalysisType = typeof(ChemicalSampleSiteProfileAnalysisTypeFactory),
                PublicWaterSupply = typeof(PublicWaterSupplyFactory)
            });
        }

        public SampleSiteProfileFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteProfileAnalysisType

    public class SampleSiteProfileAnalysisTypeFactory : StaticListEntityLookupFactory<SampleSiteProfileAnalysisType, SampleSiteProfileAnalysisTypeFactory>
    {
        public SampleSiteProfileAnalysisTypeFactory(IContainer container) : base(container) { }
    }

    public class ChemicalSampleSiteProfileAnalysisTypeFactory : SampleSiteProfileAnalysisTypeFactory
    {
        static ChemicalSampleSiteProfileAnalysisTypeFactory()
        {
            Defaults(new { Description = "Chemical" });
            OnSaving((a, s) => a.Id = SampleSiteProfileAnalysisType.Indices.CHEMICAL);
        }

        public ChemicalSampleSiteProfileAnalysisTypeFactory(IContainer container) : base(container) { }
    }

    public class BacterialSampleSiteProfileAnalysisTypeFactory : SampleSiteProfileAnalysisTypeFactory
    {
        static BacterialSampleSiteProfileAnalysisTypeFactory()
        {
            Defaults(new { Description = "Bacterial" });
            OnSaving((a, s) => a.Id = SampleSiteProfileAnalysisType.Indices.BACTERIAL);
        }

        public BacterialSampleSiteProfileAnalysisTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSitePointOfUseTreatmentType

    public class SampleSitePointOfUseTreatmentTypeFactory : StaticListEntityLookupFactory<SampleSitePointOfUseTreatmentType, SampleSitePointOfUseTreatmentTypeFactory>
    {
        public SampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class NoneSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static NoneSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "None" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.NONE);
        }

        public NoneSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class WholeHomeFilterSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static WholeHomeFilterSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "Whole Home Filter" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.WHOLE_HOME_FILTER);
        }

        public WholeHomeFilterSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class WaterSoftenerSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static WaterSoftenerSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "Water Softener" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.WATER_SOFTENER);
        }

        public WaterSoftenerSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class FaucetFilterSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static FaucetFilterSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "FaucetFilter" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.FAUCET_FILTER);
        }

        public FaucetFilterSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class IndividualTapsSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static IndividualTapsSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "Individual Taps" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.INDIVIDUAL_TAPS);
        }

        public IndividualTapsSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class EntireBuildingSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static EntireBuildingSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "Entire Building" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.ENTIRE_BUILDING);
        }

        public EntireBuildingSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    public class OtherSampleSitePointOfUseTreatmentTypeFactory : SampleSitePointOfUseTreatmentTypeFactory
    {
        static OtherSampleSitePointOfUseTreatmentTypeFactory()
        {
            Defaults(new { Description = "Other" });
            OnSaving((a, s) => a.Id = SampleSitePointOfUseTreatmentType.Indices.OTHER);
        }

        public OtherSampleSitePointOfUseTreatmentTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SampleSiteStatus

    public class SampleSiteStatusFactory : StaticListEntityLookupFactory<SampleSiteStatus, SampleSiteStatusFactory>
    {
        public SampleSiteStatusFactory(IContainer container) : base(container) { }
    }

    public class ActiveSampleSiteStatusFactory : SampleSiteStatusFactory
    {
        static ActiveSampleSiteStatusFactory()
        {
            Defaults(new { Description = "Active" });
            OnSaving((a, s) => a.Id = SampleSiteStatus.Indices.ACTIVE);
        }

        public ActiveSampleSiteStatusFactory(IContainer container) : base(container) { }
    }

    public class InactiveSampleSiteStatusFactory : SampleSiteStatusFactory
    {
        static InactiveSampleSiteStatusFactory()
        {
            Defaults(new { Description = "Inactive" });
            OnSaving((a, s) => a.Id = SampleSiteStatus.Indices.INACTIVE);
        }

        public InactiveSampleSiteStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingSampleSiteStatusFactory : SampleSiteStatusFactory
    {
        static PendingSampleSiteStatusFactory()
        {
            Defaults(new { Description = "Pending" });
            OnSaving((a, s) => a.Id = SampleSiteStatus.Indices.PENDING);
        }

        public PendingSampleSiteStatusFactory(IContainer container) : base(container) { }
    }

    public class ArchivedDuplicateSiteSampleSiteStatusFactory : SampleSiteStatusFactory
    {
        static ArchivedDuplicateSiteSampleSiteStatusFactory()
        {
            Defaults(new { Description = "Archived - Duplicate Site" });
            OnSaving((a, s) => a.Id = SampleSiteStatus.Indices.ARCHIVED_DUPLICATE_SITE);
        }

        public ArchivedDuplicateSiteSampleSiteStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SapCommunicationStatus

    public class
        SapCommunicationStatusFactory : StaticListEntityLookupFactory<SapCommunicationStatus,
            SapCommunicationStatusFactory>
    {
        public SapCommunicationStatusFactory(IContainer container) : base(container) { }
    }

    public class PendingSapCommunicationStatusFactory : SapCommunicationStatusFactory
    {
        public const string DESCRIPTION = "Pending";

        static PendingSapCommunicationStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)SapCommunicationStatus.Indices.PENDING);
        }

        public PendingSapCommunicationStatusFactory(IContainer container) : base(container) { }
    }

    public class RetrySapCommunicationStatusFactory : SapCommunicationStatusFactory
    {
        public const string DESCRIPTION = "Retry";

        static RetrySapCommunicationStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)SapCommunicationStatus.Indices.RETRY);
        }

        public RetrySapCommunicationStatusFactory(IContainer container) : base(container) { }
    }

    public class SuccessSapCommunicationStatusFactory : SapCommunicationStatusFactory
    {
        public const string DESCRIPTION = "Success";

        static SuccessSapCommunicationStatusFactory()
        {
            Defaults(new {
                Description = DESCRIPTION
            });
            OnSaving((a, s) => a.Id = (int)SapCommunicationStatus.Indices.SUCCESS);
        }

        public SuccessSapCommunicationStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SAPCompanyCode

    public class SAPCompanyCodeFactory : UniqueEntityLookupFactory<SAPCompanyCode>
    {
        public SAPCompanyCodeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SeriousInjuryOrFatalityType

    public class SeriousInjuryOrFatalityTypeFactory : StaticListEntityLookupFactory<SeriousInjuryOrFatalityType, SeriousInjuryOrFatalityTypeFactory>
    {
        #region Constructors

        public SeriousInjuryOrFatalityTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SifSeriousInjuryOrFatalityTypeFactory : SeriousInjuryOrFatalityTypeFactory
    {
        #region Constructors

        static SifSeriousInjuryOrFatalityTypeFactory()
        {
            Defaults(new {
                Description = "SIF"
            });
            OnSaving((a, s) => a.Id = (int)SeriousInjuryOrFatalityType.Indices.SIF);
        }

        public SifSeriousInjuryOrFatalityTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SifPotentialSeriousInjuryOrFatalityTypeFactory : SeriousInjuryOrFatalityTypeFactory
    {
        #region Constructors

        static SifPotentialSeriousInjuryOrFatalityTypeFactory()
        {
            Defaults(new {
                Description = "SIF_POTENTIAL"
            });
            OnSaving((a, s) => a.Id = (int)SeriousInjuryOrFatalityType.Indices.SIF_POTENTIAL);
        }

        public SifPotentialSeriousInjuryOrFatalityTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region EPACode

    public class EPACodeFactory : TestDataFactory<EPACode>
    {
        public EPACodeFactory(IContainer container) : base(container) { }

        static EPACodeFactory()
        {
            Defaults(new {
                Id = 1,
                Description = "LEAD"
            });
        }
    }

    #endregion

    #region EquipmentManufacturer

    public class EquipmentManufacturerFactory : TestDataFactory<EquipmentManufacturer>
    {
        public EquipmentManufacturerFactory(IContainer container) : base(container) { }

        static EquipmentManufacturerFactory()
        {
            var i = 0;
            Func<string> descriptionFn = (() => String.Format("Equipment Manufacturer {0}", ++i));

            Defaults(new {
                EquipmentType = typeof(EquipmentTypeGeneratorFactory),
                Description = descriptionFn
            });
        }
    }

    #endregion

    #region EquipmentType

    public class EquipmentTypeFactory : StaticListEntityLookupFactory<EquipmentType, EquipmentTypeFactory>
    {
        public EquipmentTypeFactory(IContainer container) : base(container) { }

        static EquipmentTypeFactory()
        {
            //Defaults(new { Abbrevation = "Foo" });
            //var i = 0;
            //Func<string> abbreviationFn = (() => String.Format("ET{0:D2}", ++i));
            //Func<string> descriptionFn = (() => String.Format("Equipment Type {0}", i));
            //Defaults(new
            //{
            //    Abbreviation = abbreviationFn,
            //    Description = descriptionFn
            //});
        }
    }

    public class EquipmentTypeGeneratorFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "GENERATOR";

        static EquipmentTypeGeneratorFactory()
        {
            Defaults(new {
                Abbreviation = "GEN",
                Description = DEFAULT_DESCRIPTION,
                IsLockoutRequired = true,
                EquipmentGroup = typeof(ElectricalEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.GEN);
        }

        public EquipmentTypeGeneratorFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeWellFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "WELL";

        static EquipmentTypeWellFactory()
        {
            Defaults(new {
                Abbreviation = DEFAULT_DESCRIPTION,
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(WellEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.WELL);
        }

        public EquipmentTypeWellFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeEngineFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Engine";

        static EquipmentTypeEngineFactory()
        {
            Defaults(new {
                Abbreviation = "ENG",
                Description = DEFAULT_DESCRIPTION,
                IsLockoutRequired = true,
                EquipmentGroup = typeof(MechanicalEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.ENG);
        }

        public EquipmentTypeEngineFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeAeratorFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Aerator";

        static EquipmentTypeAeratorFactory()
        {
            Defaults(new {
                Abbreviation = "TRT_AER",
                Description = DEFAULT_DESCRIPTION,
                IsLockoutRequired = true,
                EquipmentGroup = typeof(TreatmentEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.TRT_AER);
        }

        public EquipmentTypeAeratorFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeFilterFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Filter";

        static EquipmentTypeFilterFactory()
        {
            Defaults(new {
                Abbreviation = "TRT-FILT",
                Description = DEFAULT_DESCRIPTION,
                IsLockoutRequired = true,
                EquipmentGroup = typeof(TreatmentEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.TRT_FILT);
        }

        public EquipmentTypeFilterFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeFlowMeterFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "FLOW METER (NON PREMISE)";

        static EquipmentTypeFlowMeterFactory()
        {
            Defaults(new {
                Abbreviation = "FLO-MET",
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(FlowMeterEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.FLO_MET);
        }

        public EquipmentTypeFlowMeterFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeGasDetectorFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Gas Detector";

        static EquipmentTypeGasDetectorFactory()
        {
            Defaults(new {
                Abbreviation = "SAFGASDT",
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(SafetyEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.SAFGASDT);
        }

        public EquipmentTypeGasDetectorFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeRTUFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "RTU";

        static EquipmentTypeRTUFactory()
        {
            Defaults(new {
                Abbreviation = "RTU_PLC",
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(InstrumentEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.RTU_PLC);
        }

        public EquipmentTypeRTUFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeFireSuppressionFactory : EquipmentTypeFactory
    {
        static EquipmentTypeFireSuppressionFactory()
        {
            Defaults(new {
                Abbreviation = "FIRE-SUP",
                Description = "Fire Suppression",
                IsEligibleForRedTagPermit = true,
                EquipmentGroup = typeof(SafetyEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.FIRE_SUP);
        }

        public EquipmentTypeFireSuppressionFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeTankFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "TNK";

        static EquipmentTypeTankFactory()
        {
            Defaults(new {
                Abbreviation = "TNK-CHEM",
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(TankEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.TNK_CHEM);
        }

        public EquipmentTypeTankFactory(IContainer container) : base(container) { }
    }

    public class EquipmentTypeWaterTankFactory : EquipmentTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Potable Water Tank";

        static EquipmentTypeWaterTankFactory()
        {
            Defaults(new {
                Abbreviation = "TNK-WPOT",
                Description = DEFAULT_DESCRIPTION,
                EquipmentGroup = typeof(TankEquipmentGroupFactory)
            });
            OnSaving((a, s) => a.Id = EquipmentType.Indices.TNK_WPOT);
        }

        public EquipmentTypeWaterTankFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SAPWorkOrderPurpose

    public class SAPWorkOrderPurposeFactory : EntityLookupTestDataFactory<SAPWorkOrderPurpose>
    {
        public SAPWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static SAPWorkOrderPurposeFactory()
        {
            var i = 0;

            string codeFn() => (++i).ToString().PadLeft(4, '0');
            string codeGroupFn() => $"Group {i}";
            string descFn() => $"SAPWorkOrderPurpose {i}";

            Defaults(new {
                Code = (Func<string>)codeFn,
                CodeGroup = (Func<string>)codeGroupFn,
                Description = (Func<string>)descFn
            });
        }
    }

    #endregion

    #region SAPWorkOrderStep

    public class SAPWorkOrderStepFactory : StaticListEntityLookupFactory<SAPWorkOrderStep, SAPWorkOrderStepFactory>
    {
        #region Constructors

        public SAPWorkOrderStepFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateSAPWorkOrderStepFactory : SAPWorkOrderStepFactory
    {
        public const string DEFAULT_DESCRIPTION = "CREATE";

        public CreateSAPWorkOrderStepFactory(IContainer container) : base(container) { }

        static CreateSAPWorkOrderStepFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)SAPWorkOrderStep.Indices.CREATE);
        }
    }

    public class UpdateSAPWorkOrderStepFactory : SAPWorkOrderStepFactory
    {
        public const string DEFAULT_DESCRIPTION = "UPDATE";

        public UpdateSAPWorkOrderStepFactory(IContainer container) : base(container) { }

        static UpdateSAPWorkOrderStepFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)SAPWorkOrderStep.Indices.UPDATE);
        }
    }

    public class CompleteSAPWorkOrderStepFactory : SAPWorkOrderStepFactory
    {
        public const string DEFAULT_DESCRIPTION = "COMPLETE";

        public CompleteSAPWorkOrderStepFactory(IContainer container) : base(container) { }

        static CompleteSAPWorkOrderStepFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)SAPWorkOrderStep.Indices.COMPLETE);
        }
    }

    public class ApproveGoodsSAPWorkOrderStepFactory : SAPWorkOrderStepFactory
    {
        public const string DEFAULT_DESCRIPTION = "APPROVE GOODS";

        public ApproveGoodsSAPWorkOrderStepFactory(IContainer container) : base(container) { }

        static ApproveGoodsSAPWorkOrderStepFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)SAPWorkOrderStep.Indices.APPROVE_GOODS);
        }
    }

    #endregion

    #region ScadaSignal

    public class ScadaSignalFactory : TestDataFactory<ScadaSignal>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ScadaSignal",
                            DEFAULT_TAG_NAME = "1234",
                            DEFAULT_ENGINEERING_UNITS = "gallons",
                            DEFAULT_TAG_ID = "123221210-asd2-12312";

        #endregion

        #region Constructors

        static ScadaSignalFactory()
        {
            Defaults(new {
                TagName = DEFAULT_TAG_NAME,
                Description = DEFAULT_DESCRIPTION,
                TagId = DEFAULT_TAG_ID,
                EngineeringUnits = DEFAULT_ENGINEERING_UNITS
            });
        }

        public ScadaSignalFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ScadaTagName

    public class ScadaTagNameFactory : TestDataFactory<ScadaTagName>
    {
        public ScadaTagNameFactory(IContainer container) : base(container) { }

        static ScadaTagNameFactory()
        {
            var i = 0;

            string tagNameFn() => (++i).ToString().PadLeft(5, '0');

            Defaults(new {
                TagName = (Func<string>)tagNameFn,
            });
        }
    }

    #endregion

    #region ScheduledAssignments

    public class ScheduledAssignmentFactory : TestDataFactory<ScheduledAssignment>
    {
        #region Constructors

        static ScheduledAssignmentFactory()
        {
            Defaults(new {
                MaintenancePlan = typeof(MaintenancePlanFactory),
                AssignedTo = typeof(EmployeeFactory),
                CreatedBy = typeof(EmployeeFactory),
                AssignedFor = DateTime.Today,
                ScheduledDate = DateTime.Today
            });
        }

        public ScheduledAssignmentFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ScheduleOfValueCategory

    public class ScheduleOfValueCategoryFactory : UniqueEntityLookupFactory<ScheduleOfValueCategory>
    {
        public ScheduleOfValueCategoryFactory(IContainer container) : base(container) { }

        static ScheduleOfValueCategoryFactory()
        {
            Defaults(new {
                ScheduleOfValueType = typeof(ScheduleOfValueTypeFactory)
            });
        }
    }

    #endregion

    #region ScheduleOfValue

    public class ScheduleOfValueFactory : UniqueEntityLookupFactory<ScheduleOfValue>
    {
        public ScheduleOfValueFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ScheduleOfValueType

    public class ScheduleOfValueTypeFactory : UniqueEntityLookupFactory<ScheduleOfValueType>
    {
        public ScheduleOfValueTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SecureFormToken

    public class SecureFormTokenFactory : TestDataFactory<SecureFormToken>
    {
        #region Constructors

        static SecureFormTokenFactory()
        {
            Defaults(new {
                UserId = 1,
                Area = "SomeArea",
                Controller = "SomeController",
                Action = "SomeAction"
            });
        }

        public SecureFormTokenFactory(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override SecureFormToken Build(object overrides = null)
        {
            var token = base.Build(overrides);
            if (token.Token == Guid.Empty)
            {
                token.Token = Guid.NewGuid();
            }

            return token;
        }

        #endregion
    }

    #endregion

    #region SensorFactory

    public class SensorFactory : TestDataFactory<Sensor>
    {
        static SensorFactory()
        {
            Defaults(new {
                Name = "Sensor Name",
                Description = "Sensor Description",
                Board = typeof(BoardFactory),
                MeasurementType = typeof(SensorMeasurementTypeFactory)
            });
        }

        public SensorFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SensorMeasurementTypeFactory

    public class SensorMeasurementTypeFactory : UniqueEntityLookupFactory<SensorMeasurementType>
    {
        public SensorMeasurementTypeFactory(IContainer container) : base(container) { }
    }

    public class KilowattSensorMeasurementTypeFactory : SensorMeasurementTypeFactory
    {
        static KilowattSensorMeasurementTypeFactory()
        {
            Defaults(new {
                Description = SensorMeasurementType.Descriptions.KILOWATT,
            });
        }

        public KilowattSensorMeasurementTypeFactory(IContainer container) : base(container) { }
    }

    public class KilowattHoursSensorMeasurementTypeFactory : SensorMeasurementTypeFactory
    {
        static KilowattHoursSensorMeasurementTypeFactory()
        {
            Defaults(new {
                Description = SensorMeasurementType.Descriptions.KILOWATT_HOURS
            });
        }

        public KilowattHoursSensorMeasurementTypeFactory(IContainer container) : base(container) { }
    }

    public class WattSensorMeasurementTypeFactory : SensorMeasurementTypeFactory
    {
        static WattSensorMeasurementTypeFactory()
        {
            Defaults(new {
                Description = SensorMeasurementType.Descriptions.WATT
            });
        }

        public WattSensorMeasurementTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Service

    public class ServiceFactory : TestDataFactory<Service>
    {
        #region Constants

        public const long DEFAULT_SERVICE_NUMBER = 9876543210;

        #endregion

        #region Constructors

        static ServiceFactory()
        {
            Defaults(new {
                DateInstalled = Lambdas.GetNow,
                ServiceCategory = typeof(ServiceCategoryFactory),
                ServiceMaterial = typeof(ServiceMaterialFactory),
                CustomerSideMaterial = typeof(ServiceMaterialFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                CustomerSideSize = typeof(ServiceSizeFactory),
                ServiceNumber = DEFAULT_SERVICE_NUMBER,
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                Initiator = typeof(UserFactory),
                MeterSettingSize = typeof(ServiceSizeFactory),
                UpdatedBy = typeof(UserFactory)
            });
        }

        public ServiceFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceFlush

    public class ServiceFlushFactory : TestDataFactory<ServiceFlush>
    {
        #region Constructors

        static ServiceFlushFactory()
        {
            Defaults(new {
                Service = typeof(ServiceFactory),
                FlushType = typeof(ServiceFlushFlushTypeFactory),
                SampleType = typeof(ServiceFlushSampleTypeFactory),
                SampleStatus = typeof(ServiceFlushSampleStatusFactory),
                TakenBy = typeof(ServiceFlushSampleTakenByTypeFactory),
                ContactMethod = typeof(ServiceFlushPremiseContactMethodFactory),
                ReplacementType = typeof(ServiceFlushReplacementTypeFactory),
                CreatedBy = typeof(UserFactory)
            });
        }

        public ServiceFlushFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ServiceFlushFlushTypeFactory : UniqueEntityLookupFactory<ServiceFlushFlushType>
    {
        public ServiceFlushFlushTypeFactory(IContainer container) : base(container) { }
    }

    public class ServiceFlushSampleTypeFactory : UniqueEntityLookupFactory<ServiceFlushSampleType>
    {
        public ServiceFlushSampleTypeFactory(IContainer container) : base(container) { }
    }

    public class ServiceFlushSampleTakenByTypeFactory : UniqueEntityLookupFactory<ServiceFlushSampleTakenByType>
    {
        public ServiceFlushSampleTakenByTypeFactory(IContainer container) : base(container) { }
    }

    public class ServiceFlushPremiseContactMethodFactory : UniqueEntityLookupFactory<ServiceFlushPremiseContactMethod>
    {
        public ServiceFlushPremiseContactMethodFactory(IContainer container) : base(container) { }
    }

    public class ServiceFlushReplacementTypeFactory : UniqueEntityLookupFactory<ServiceFlushReplacementType>
    {
        public ServiceFlushReplacementTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ServiceFlushSampleStatus

    public class ServiceFlushSampleStatusFactory : StaticListEntityLookupFactory<ServiceFlushSampleStatus, ServiceFlushSampleStatusFactory>
    {
        public ServiceFlushSampleStatusFactory(IContainer container) : base(container) { }
    }

    public class TakenServiceFlushSampleStatusFactory : ServiceFlushSampleStatusFactory
    {
        static TakenServiceFlushSampleStatusFactory()
        {
            Defaults(new { Description = "Taken" });
            OnSaving((a, s) => a.Id = ServiceFlushSampleStatus.Indices.TAKEN);
        }

        public TakenServiceFlushSampleStatusFactory(IContainer container) : base(container) { }
    }

    public class ErrorResampledServiceFlushSampleStatusFactory : ServiceFlushSampleStatusFactory
    {
        static ErrorResampledServiceFlushSampleStatusFactory()
        {
            Defaults(new { Description = "Error - Resampled" });
            OnSaving((a, s) => a.Id = ServiceFlushSampleStatus.Indices.ERROR_RESAMPLED);
        }

        public ErrorResampledServiceFlushSampleStatusFactory(IContainer container) : base(container) { }
    }

    public class ResultsReceivedServiceFlushSampleStatusFactory : ServiceFlushSampleStatusFactory
    {
        static ResultsReceivedServiceFlushSampleStatusFactory()
        {
            Defaults(new { Description = "Results Received" });
            OnSaving((a, s) => a.Id = ServiceFlushSampleStatus.Indices.RESULTS_RECEIVED);
        }

        public ResultsReceivedServiceFlushSampleStatusFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ServiceInstallation

    //public class MeterSupplementalLocation : EntityLookupTestDataFactory<MeterSupplementalLocation> { }
    public class ServiceInstallationFactory : TestDataFactory<ServiceInstallation>
    {
        #region Constants

        public const string DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER = "12341234",
                            DEFAULT_RFMIU = "123",
                            DEFAULT_CURRENT_READ = "000001",
                            DEFAULT_METER_LOCATION_INFORMATION = " These are some notes about the install.";

        #endregion

        #region Constructors

        static ServiceInstallationFactory()
        {
            Defaults(new {
                MeterManufacturerSerialNumber = DEFAULT_METER_MANUFACTURER_SERIAL_NUMBER,
                Register1RFMIU = DEFAULT_RFMIU,
                Register1CurrentRead = DEFAULT_CURRENT_READ,
                MeterLocationInformation = DEFAULT_METER_LOCATION_INFORMATION,
                MeterLocation = typeof(EntityLookupTestDataFactory<MeterSupplementalLocation>),
                MeterPositionalLocation = typeof(EntityLookupTestDataFactory<SmallMeterLocation>),
                MeterDirectionalLocation = typeof(EntityLookupTestDataFactory<MeterDirection>),
                ReadingDevicePosition = typeof(EntityLookupTestDataFactory<SmallMeterLocation>),
                ReadingDeviceSupplemental = typeof(EntityLookupTestDataFactory<MeterSupplementalLocation>),
                ReadingDeviceDirectionalInformation = typeof(EntityLookupTestDataFactory<MeterDirection>),
                Register1ReadType = typeof(EntityLookupTestDataFactory<ServiceInstallationReadType>),
                Activity1 = typeof(EntityLookupTestDataFactory<ServiceInstallationFirstActivity>),
                ServiceFound = typeof(EntityLookupTestDataFactory<ServiceInstallationPosition>),
                ServiceLeft = typeof(EntityLookupTestDataFactory<ServiceInstallationPosition>),
                ServiceInstallationReason = typeof(EntityLookupTestDataFactory<ServiceInstallationReason>),
                MiuInstallReason = typeof(EntityLookupTestDataFactory<MiuInstallReasonCode>)
            });
        }

        public ServiceInstallationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceLineProtectionInvestigation

    public class ServiceLineProtectionInvestigationFactory : TestDataFactory<ServiceLineProtectionInvestigation>
    {
        #region Constants

        public const string DEFAULT_CUSTOMER_NAME = "Customer Name",
                            DEFAULT_STREET_NUMBER = "123",
                            DEFAULT_CUSTOMER_ZIP = "12345",
                            DEFAULT_PREMISE_NUMBER = "1234567890";

        #endregion

        #region Constructors

        static ServiceLineProtectionInvestigationFactory()
        {
            Defaults(new {
                CustomerName = DEFAULT_CUSTOMER_NAME,
                StreetNumber = DEFAULT_STREET_NUMBER,
                Street = typeof(StreetFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                CustomerCity = typeof(TownFactory),
                CustomerZip = DEFAULT_CUSTOMER_ZIP,
                PremiseNumber = DEFAULT_PREMISE_NUMBER,
                CustomerServiceMaterial = typeof(ServiceMaterialFactory),
                WorkType = typeof(ServiceLineProtectionWorkTypeFactory),
                Contractor = typeof(ContractorFactory),
                Coordinate = typeof(CoordinateFactory)
            });
        }

        public ServiceLineProtectionInvestigationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceLineProtectionWorkType

    public class ServiceLineProtectionWorkTypeFactory : UniqueEntityLookupFactory<ServiceLineProtectionWorkType>
    {
        public ServiceLineProtectionWorkTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ServiceRestoration

    public class ServiceRestorationFactory : TestDataFactory<ServiceRestoration>
    {
        #region Constants

        public const string DEFAULT_PURCHASE_ORDER_NUMBER = "ServiceRestoration";

        #endregion

        #region Constructors

        static ServiceRestorationFactory()
        {
            Defaults(new {
                PurchaseOrderNumber = DEFAULT_PURCHASE_ORDER_NUMBER
            });
        }

        public ServiceRestorationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceCategory

    public class ServiceCategoryFactory : TestDataFactory<ServiceCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ServiceCategory";

        #endregion

        #region Constructors

        static ServiceCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ServiceCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceInstallationMaterial

    public class ServiceInstallationMaterialFactory : TestDataFactory<ServiceInstallationMaterial>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ServiceInstallationMaterial";

        #endregion

        #region Constructors

        static ServiceInstallationMaterialFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                ServiceCategory = typeof(ServiceCategoryFactory),
                ServiceSize = typeof(ServiceSizeFactory),
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ServiceInstallationMaterialFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceMaterial

    public class ServiceMaterialFactory : TestDataFactory<ServiceMaterial>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "ServiceMaterial";

        #endregion

        #region Constructors

        static ServiceMaterialFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public ServiceMaterialFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceMaterialEPACodeOverride

    public class ServiceMaterialEPACodeOverrideFactory : TestDataFactory<ServiceMaterialEPACodeOverride>
    {
        #region Constructors

        static ServiceMaterialEPACodeOverrideFactory()
        {
            Defaults(new {
                State = typeof(StateFactory),
                ServiceMaterial = typeof(ServiceMaterialFactory),
                CustomerEPACode = typeof(EPACodeFactory),
                CompanyEPACode = typeof(EPACodeFactory)
            });
        }

        #endregion
        
        public ServiceMaterialEPACodeOverrideFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ServicePremiseContact

    public class ServicePremiseContactFactory : TestDataFactory<ServicePremiseContact>
    {
        #region Constructors

        static ServicePremiseContactFactory()
        {
            Defaults(new {
                Service = typeof(ServiceFactory),
                ContactMethod = typeof(ServicePremiseContactMethodFactory),
                ContactType = typeof(ServicePremiseContactTypeFactory),
                CreatedBy = typeof(UserFactory),
                ContactDate = Lambdas.GetNowDate
            });
        }

        public ServicePremiseContactFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class ServicePremiseContactMethodFactory : UniqueEntityLookupFactory<ServicePremiseContactMethod>
    {
        public ServicePremiseContactMethodFactory(IContainer container) : base(container) { }
    }

    public class ServicePremiseContactTypeFactory : UniqueEntityLookupFactory<ServicePremiseContactType>
    {
        public ServicePremiseContactTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ServiceRestorationContractor

    public class ServiceRestorationContractorFactory : TestDataFactory<ServiceRestorationContractor>
    {
        #region Constants

        public const string DEFAULT_CONTRACTOR = "MMSINC";

        #endregion

        #region Constructors

        static ServiceRestorationContractorFactory()
        {
            Defaults(new {
                Contractor = DEFAULT_CONTRACTOR,
                OperatingCenter = typeof(OperatingCenterFactory)
            });
        }

        public ServiceRestorationContractorFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceSize

    public class ServiceSizeFactory : TestDataFactory<ServiceSize>
    {
        #region Constants

        public const string DEFAULT_SIZE = "1 1/2";

        #endregion

        #region Constructors

        static ServiceSizeFactory()
        {
            Defaults(new {
                ServiceSizeDescription = DEFAULT_SIZE
            });
        }

        public ServiceSizeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceTerminationPoint

    public class ServiceTerminationPointFactory : TestDataFactory<ServiceTerminationPoint>
    {
        #region Constructors

        static ServiceTerminationPointFactory()
        {
            Defaults(new {
                Description = "Some termination point"
            });
        }

        public ServiceTerminationPointFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InsideShutoffServiceTerminationPointFactory : ServiceTerminationPointFactory
    {
        #region Constructors

        static InsideShutoffServiceTerminationPointFactory()
        {
            Defaults(new {
                Description = "Inside Shutoff"
            });
            OnSaving((a, s) => a.Id = (int)ServiceTerminationPoint.Indices.INSIDE_SHUTOFF);
        }

        public InsideShutoffServiceTerminationPointFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class OtherServiceTerminationPointFactory : ServiceTerminationPointFactory
    {
        #region Constructors

        static OtherServiceTerminationPointFactory()
        {
            Defaults(new {
                Description = "Other"
            });
            OnSaving((a, s) => a.Id = (int)ServiceTerminationPoint.Indices.OTHER);
        }

        public OtherServiceTerminationPointFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ServiceUtilityType

    public class ServiceUtilityTypeFactory : EntityLookupTestDataFactory<ServiceUtilityType>
    {
        public ServiceUtilityTypeFactory(IContainer container) : base(container) { }

        static ServiceUtilityTypeFactory()
        {
            var i = 0;

            string typeFn() => $"Type {++i}";
            string descFn() => $"ServiceUtilityType {i}";

            Defaults(new {
                Type = (Func<string>)typeFn,
                Description = (Func<string>)descFn
            });
        }
    }

    #endregion

    #region SewerOpeningFactory

    public class SewerOpeningFactory : TestDataFactory<SewerOpening>
    {
        #region Fields

        private static int _assetNumberSuffix;

        #endregion

        #region Constructors

        static SewerOpeningFactory()
        {
            Defaults(new {
                Status = typeof(ActiveAssetStatusFactory),
                Coordinate = typeof(CoordinateFactory),
                CreatedBy = typeof(UserFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                Street = typeof(StreetFactory),
                SAPEquipmentId = 123456,
                FunctionalLocation = typeof(FunctionalLocationFactory),
                SewerOpeningType = typeof(NpdesRegulatorSewerOpeningTypeFactory),
                TaskNumber = "task number",
                OldNumber = "003",
                OutfallNumber = "007",
                BodyOfWater = typeof(BodyOfWaterFactory),
                LocationDescription = "description of location",
                OpeningNumber = "MSC-6231",
                WasteWaterSystem = typeof(WasteWaterSystemFactory)
            });
        }

        public SewerOpeningFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private static string GenerateUniqueAssetNumber()
        {
            // This is done up just to give us some results that are similar
            // to what actually is in the live db.

            try
            {
                return string.Format("MAB-{0}", _assetNumberSuffix);
            }
            finally
            {
                _assetNumberSuffix += 1;
            }
        }

        #endregion

        #region Exposed Methods

        public override SewerOpening Build(object overrides = null)
        {
            var asset = base.Build(overrides);

            if (asset.OpeningNumber == null)
            {
                asset.OpeningSuffix = _assetNumberSuffix;
                asset.OpeningNumber = GenerateUniqueAssetNumber();
            }

            return asset;
        }

        #endregion
    }

    #endregion

    #region SewerOpeningConnectionFactory

    public class SewerOpeningConnectionFactory : TestDataFactory<SewerOpeningConnection>
    {
        #region Constructors

        static SewerOpeningConnectionFactory()
        {
            Defaults(new { 
                DownstreamOpening = typeof(SewerOpeningFactory), 
                UpstreamOpening = typeof(SewerOpeningFactory), 
                CreatedAt = Lambdas.GetNow
            });
        }

        public SewerOpeningConnectionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SewerOpeningTypeFactory

    public class SewerOpeningTypeFactory : StaticListEntityLookupFactory<SewerOpeningType, SewerOpeningTypeFactory>
    {
        #region Constructors

        public SewerOpeningTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CatchBasinSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public CatchBasinSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static CatchBasinSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "CATCH BASIN"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.CATCH_BASIN);
        }
    }
    
    public class CleanOutSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public CleanOutSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static CleanOutSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "CLEAN_OUT"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.CLEAN_OUT);
        }
    }

    public class LampholeSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public LampholeSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static LampholeSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "LAMPHOLE"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.LAMPHOLE);
        }
    }

    public class ManholeSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public ManholeSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static ManholeSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "MANHOLE"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.MANHOLE);
        }
    }

    public class OutfallSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public OutfallSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static OutfallSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "OUTFALL"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.OUTFALL);
        }
    }

    public class NpdesRegulatorSewerOpeningTypeFactory
        : SewerOpeningTypeFactory
    {
        public NpdesRegulatorSewerOpeningTypeFactory(IContainer container) : base(container) { }

        static NpdesRegulatorSewerOpeningTypeFactory()
        {
            Defaults(new {
                Description = "NPDES REGULATOR"
            });
            OnSaving((x, _) => x.Id = SewerOpeningType.Indices.NPDES_REGULATOR);
        }
    }

    #endregion

    #region SewerOpeningInspection

    public class SewerOpeningInspectionFactory : TestDataFactory<SewerOpeningInspection>
    {
        static SewerOpeningInspectionFactory()
        {
            Defaults(new {
                SewerOpening = typeof(SewerOpeningFactory),
                DateInspected = Lambdas.GetNow,
                CreatedAt = Lambdas.GetNow,
                PipesIn = "y hallo there",
                PipesOut = "Now I'm not a null Value :)",
                RimHeightAboveBelowGrade = (decimal)12.34,
                RimToWaterLevelDepth = (decimal)23.45,
                InspectedBy = typeof(UserFactory)
            });
        }

        public SewerOpeningInspectionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SewerMainCleaning

    public class SewerMainCleaningFactory : TestDataFactory<SewerMainCleaning>
    {
        #region Constructors

        static SewerMainCleaningFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory),
                InspectionType = typeof(MainCleaningPMSewerMainInspectionTypeFactory),
                Overflow = false,
                Date = Lambdas.GetNow,
                FootageOfMainInspected = 250.0f
            });
        }

        public SewerMainCleaningFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion
    
    #region SewerMainInspectionType

    public class SewerMainInspectionTypeFactoryBase
        : StaticListEntityLookupFactory<SewerMainInspectionType, SewerMainInspectionTypeFactoryBase> 
    { 
        public SewerMainInspectionTypeFactoryBase(IContainer container) : base(container) { }    
    }

    public class AcousticSewerMainInspectionTypeFactory
        : SewerMainInspectionTypeFactoryBase
    {
        public AcousticSewerMainInspectionTypeFactory(IContainer container) : base(container) { }

        static AcousticSewerMainInspectionTypeFactory()
        {
            Defaults(new {
                Description = "ACOUSTIC"
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionType.Indices.ACOUSTIC);
        }
    }

    public class CCTVSewerMainInspectionTypeFactory
        : SewerMainInspectionTypeFactoryBase
    {
        public CCTVSewerMainInspectionTypeFactory(IContainer container) : base(container) { }

        static CCTVSewerMainInspectionTypeFactory()
        {
            Defaults(new {
                Description = "CCTV"
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionType.Indices.CCTV);
        }
    }

    public class MainCleaningPMSewerMainInspectionTypeFactory
        : SewerMainInspectionTypeFactoryBase
    {
        public MainCleaningPMSewerMainInspectionTypeFactory(IContainer container) : base(container) { }

        static MainCleaningPMSewerMainInspectionTypeFactory()
        {
            Defaults(new {
                Description = "MAIN CLEANING PM"
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionType.Indices.MAIN_CLEANING_PM);
        }
    }

    public class SmokeTestSewerMainInspectionTypeFactory
        : SewerMainInspectionTypeFactoryBase
    {
        public SmokeTestSewerMainInspectionTypeFactory(IContainer container) : base(container) { }

        static SmokeTestSewerMainInspectionTypeFactory()
        {
            Defaults(new {
                Description = "SMOKE TEST"
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionType.Indices.SMOKE_TEST);
        }
    }

    #endregion
    
    #region SewerMainInspectionGrade
    
    public class SewerMainInspectionGradeFactoryBase
        : StaticListEntityLookupFactory<SewerMainInspectionGrade, SewerMainInspectionGradeFactoryBase> 
    { 
        public SewerMainInspectionGradeFactoryBase(IContainer container) : base(container) { }    
    }

    public class ExcellentSewerMainInspectionGradeFactory : SewerMainInspectionGradeFactoryBase
    {
        public ExcellentSewerMainInspectionGradeFactory(IContainer container) : base(container) { }

        static ExcellentSewerMainInspectionGradeFactory()
        {
            Defaults(new {
                Description = "EXCELLENT",
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionGrade.Indices.EXCELLENT);
        }
    }

    public class GoodSewerMainInspectionGradeFactory : SewerMainInspectionGradeFactoryBase
    {
        public GoodSewerMainInspectionGradeFactory(IContainer container) : base(container) { }

        static GoodSewerMainInspectionGradeFactory()
        {
            Defaults(new {
                Description = "GOOD",
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionGrade.Indices.GOOD);
        }
    }

    public class FairSewerMainInspectionGradeFactory : SewerMainInspectionGradeFactoryBase
    {
        public FairSewerMainInspectionGradeFactory(IContainer container) : base(container) { }

        static FairSewerMainInspectionGradeFactory()
        {
            Defaults(new {
                Description = "FAIR",
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionGrade.Indices.FAIR);
        }
    }

    public class PoorSewerMainInspectionGradeFactory : SewerMainInspectionGradeFactoryBase
    {
        public PoorSewerMainInspectionGradeFactory(IContainer container) : base(container) { }

        static PoorSewerMainInspectionGradeFactory()
        {
            Defaults(new {
                Description = "POOR",
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionGrade.Indices.POOR);
        }
    }

    public class ImmediateAttentionSewerMainInspectionGradeFactory : SewerMainInspectionGradeFactoryBase
    {
        public ImmediateAttentionSewerMainInspectionGradeFactory(IContainer container) : base(container) { }

        static ImmediateAttentionSewerMainInspectionGradeFactory()
        {
            Defaults(new {
                Description = "IMMEDIATE ATTENTION",
            });
            OnSaving((x, _) => x.Id = SewerMainInspectionGrade.Indices.IMMEDIATE_ATTENTION);
        }
    }

    #endregion

    #region SewerOverflow

    public class SewerOverflowFactory : TestDataFactory<SewerOverflow>
    {
        #region Constructors

        static SewerOverflowFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                Coordinate = typeof(CoordinateFactory),
                Town = typeof(TownFactory),
                IsSystemNewlyAcquired = false,
                IsSystemUnderConsentOrder = false,
                WasteWaterSystem = typeof(WasteWaterSystemFactory),
                IncidentDate = Lambdas.GetNow,
                GallonsOverflowedEstimated = 0,
                SewageRecoveredGallons = 0,
                DischargeLocation = typeof(SewerOverflowDischargeLocationFactory),
                WeatherType = typeof(DischargeWeatherRelatedTypeFactory),
                OverflowType = typeof(SewerOverflowTypeFactory),
                OverflowCause = typeof(SewerOverflowCauseFactory),
                CallReceived = Lambdas.GetNow,
                CrewArrivedOnSite = Lambdas.GetNow,
                SewageContained = Lambdas.GetNow,
                StoppageCleared = Lambdas.GetNow,
                WorkCompleted = Lambdas.GetNow,
                LocationOfStoppage = "test location"
            });
        }

        public SewerOverflowFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SewerOverflowCause

    public class SewerOverflowCauseFactory : StaticListEntityLookupFactory<SewerOverflowCause, SewerOverflowCauseFactory>
    {
        public SewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class PipeFailureSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static PipeFailureSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Pipe Failure"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.PIPE_FAILURE);
        }

        public PipeFailureSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class DebrisSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static DebrisSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Debris (Rags/Paper)"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.DEBRIS);
        }

        public DebrisSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class GreaseSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static GreaseSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Grease"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.GREASE);
        }

        public GreaseSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class RootsSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static RootsSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Roots"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.ROOTS);
        }

        public RootsSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class PowerFailureSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static PowerFailureSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Power Failure"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.POWER_FAILURE);
        }

        public PowerFailureSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class MechanicalFailureSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static MechanicalFailureSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Pump Station / Mechanical Failure"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.MECHANICAL_FAILURE);
        }

        public MechanicalFailureSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class InflowAndInfiltrationSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static InflowAndInfiltrationSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Inflow and Infiltration"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.INFLOW_AND_INFILTRATION);
        }

        public InflowAndInfiltrationSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class VandalismSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static VandalismSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Vandalism"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.VANDALISM);
        }

        public VandalismSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    public class PipeDesignSewerOverflowCauseFactory : SewerOverflowCauseFactory
    {
        static PipeDesignSewerOverflowCauseFactory()
        {
            Defaults(new {
                Description = "Pipe - Capacity/Design"
            });

            OnSaving((x, _) => x.Id = SewerOverflowCause.Indices.PIPE_DESIGN);
        }

        public PipeDesignSewerOverflowCauseFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region SewerOverflowDischargeLocation

    public class SewerOverflowDischargeLocationFactory : StaticListEntityLookupFactory<SewerOverflowDischargeLocation, SewerOverflowDischargeLocationFactory>
    {
        public SewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }
    }

    public class RunsOnGroundSewerOverflowDischargeLocationFactory : SewerOverflowDischargeLocationFactory
    {
        public RunsOnGroundSewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }

        static RunsOnGroundSewerOverflowDischargeLocationFactory()
        {
            Defaults(new {
                Description = "Runs On Ground And Absorbs Into Soil"
            });
            OnSaving((x, _) => x.Id = SewerOverflowDischargeLocation.Indices.RUNS_ON_GROUND);
        }
    }

    public class DitchOrDetentionBasinSewerOverflowDischargeLocationFactory : SewerOverflowDischargeLocationFactory
    {
        public DitchOrDetentionBasinSewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }

        static DitchOrDetentionBasinSewerOverflowDischargeLocationFactory()
        {
            Defaults(new {
                Description = "Ditch Or Detention Basin"
            });
            OnSaving((x, _) => x.Id = SewerOverflowDischargeLocation.Indices.DITCH_OR_DETENTION_BASIN);
        }
    }

    public class StormSewerSewerOverflowDischargeLocationFactory : SewerOverflowDischargeLocationFactory
    {
        public StormSewerSewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }

        static StormSewerSewerOverflowDischargeLocationFactory()
        {
            Defaults(new {
                Description = "Storm Sewer"
            });
            OnSaving((x, _) => x.Id = SewerOverflowDischargeLocation.Indices.STORM_SEWER);
        }
    }

    public class BodyOfWaterSewerOverflowDischargeLocationFactory : SewerOverflowDischargeLocationFactory
    {
        public BodyOfWaterSewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }

        static BodyOfWaterSewerOverflowDischargeLocationFactory()
        {
            Defaults(new {
                Description = "Body Of Water"
            });
            OnSaving((x, _) => x.Id = SewerOverflowDischargeLocation.Indices.BODY_OF_WATER);
        }
    }

    public class OtherSewerOverflowDischargeLocationFactory : SewerOverflowDischargeLocationFactory
    {
        public OtherSewerOverflowDischargeLocationFactory(IContainer container) : base(container) { }

        static OtherSewerOverflowDischargeLocationFactory()
        {
            Defaults(new {
                Description = "Other"
            });
            OnSaving((x, _) => x.Id = SewerOverflowDischargeLocation.Indices.OTHER);
        }
    }

    #endregion

    #region SewerOverflowType

    public class SewerOverflowTypeFactory : StaticListEntityLookupFactory<SewerOverflowType, SewerOverflowTypeFactory>
    {
        public SewerOverflowTypeFactory(IContainer container) : base(container) { }
    }

    public class SSOSewerOverflowTypeFactory : SewerOverflowTypeFactory
    {
        static SSOSewerOverflowTypeFactory()
        {
            Defaults(new {
                Description = "SSO"
            });

            OnSaving((x, _) => x.Id = SewerOverflowType.Indices.SSO);
        }

        public SSOSewerOverflowTypeFactory(IContainer container) : base(container) { }
    }

    public class CSOApprovedLocationSewerOverflowTypeFactory : SewerOverflowTypeFactory
    {
        static CSOApprovedLocationSewerOverflowTypeFactory()
        {
            Defaults(new {
                Description = "CSO - Approved Location"
            });

            OnSaving((x, _) => x.Id = SewerOverflowType.Indices.CSO_APPROVED);
        }

        public CSOApprovedLocationSewerOverflowTypeFactory(IContainer container) : base(container) { }
    }

    public class CSOUnapprovedLocationSewerOverflowTypeFactory : SewerOverflowTypeFactory
    {
        static CSOUnapprovedLocationSewerOverflowTypeFactory()
        {
            Defaults(new {
                Description = "CSO - Unapproved Location"
            });

            OnSaving((x, _) => x.Id = SewerOverflowType.Indices.CSO_UNAPPROVED);
        }

        public CSOUnapprovedLocationSewerOverflowTypeFactory(IContainer container) : base(container) { }
    }
        
    #endregion
    
    #region SewerStoppageType

    public class SewerStoppageTypeFactory : EntityLookupTestDataFactory<SewerStoppageType>
    {
        #region Constructors

        public SewerStoppageTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion
    
    #region ShortCycleCustomerMaterial

    public class ShortCycleCustomerMaterialFactory : TestDataFactory<ShortCycleCustomerMaterial>
    {
        #region Constructors
        
        static ShortCycleCustomerMaterialFactory()
        {
            var i = 0;
            int ShortCycleWorkOrderNumberFn() => ++i;
            Defaults(new {
                Premise = typeof(PremiseFactory),
                ShortCycleWorkOrderNumber = (Func<int>)ShortCycleWorkOrderNumberFn
            });
        }
        
        public ShortCycleCustomerMaterialFactory(IContainer container) : base(container) { }
        
        #endregion
    }
    
    #endregion
    
    #region ShortCycleWorkOrderSafetyBrief

    public class ShortCycleWorkOrderSafetyBriefFactory : TestDataFactory<ShortCycleWorkOrderSafetyBrief>
    {
        #region Constructors

        static ShortCycleWorkOrderSafetyBriefFactory()
        {
            Defaults(new {
                FSR = typeof(EmployeeFactory),
                DateCompleted = Lambdas.GetNow,
                IsPPEInGoodCondition = false,
                HasCompletedDailyStretchingRoutine = false,
                HasPerformedInspectionOnVehicle = false
            });
        }

        public ShortCycleWorkOrderSafetyBriefFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class
        ShortCycleWorkOrderSafetyBriefLocationTypeFactory : UniqueEntityLookupFactory<
            ShortCycleWorkOrderSafetyBriefLocationType>
    {
        public ShortCycleWorkOrderSafetyBriefLocationTypeFactory(IContainer container) : base(container) { }
    }

    public class
        ShortCycleWorkOrderSafetyBriefHazardTypeFactory : UniqueEntityLookupFactory<
            ShortCycleWorkOrderSafetyBriefHazardType>
    {
        public ShortCycleWorkOrderSafetyBriefHazardTypeFactory(IContainer container) : base(container) { }
    }

    public class
        ShortCycleWorkOrderSafetyBriefPPETypeFactory : UniqueEntityLookupFactory<ShortCycleWorkOrderSafetyBriefPPEType>
    {
        public ShortCycleWorkOrderSafetyBriefPPETypeFactory(IContainer container) : base(container) { }
    }

    public class ShortCycleWorkOrderSafetyBriefToolTypeFactory : UniqueEntityLookupFactory<ShortCycleWorkOrderSafetyBriefToolType>
    {
        public ShortCycleWorkOrderSafetyBriefToolTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SiteFactory

    public class SiteFactory : TestDataFactory<Site>
    {
        static SiteFactory()
        {
            Defaults(new {
                Name = "Site",
                Project = typeof(ProjectFactory)
            });
        }

        public SiteFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SkillSet

    public class SkillSetFactory : TestDataFactory<SkillSet>
    {
        #region Constants

        public const string DEFAULT_NAME = "SkillSet",
                            DEFAULT_ABBREVIATION = "SS",
                            DEFAULT_DESCRIPTION = "Description of SkillSet";

        public const bool DEFAULT_IS_ACTIVE = true;

        #endregion

        #region Constructors

        static SkillSetFactory()
        {
            Defaults(new {
                Name = DEFAULT_NAME,
                Abbreviation = DEFAULT_ABBREVIATION,
                IsActive = DEFAULT_IS_ACTIVE,
                Description = DEFAULT_DESCRIPTION
            });
        }

        public SkillSetFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SmartCoverAlert

    public class SmartCoverAlertFactory : TestDataFactory<SmartCoverAlert>
    {
        #region Constructors

        static SmartCoverAlertFactory()
        {
            Defaults(new {
                AlertId = 12345,
                ApplicationDescription = typeof(SmartCoverAlertApplicationDescriptionTypeFactory),
                DateReceived = new DateTime(2022, 01, 09),
                HighAlarmThreshold = (decimal)16.8,
                Latitude = (decimal)39.9,
                Longitude = (decimal)-75.04,
                ManholeDepth = (decimal)160.00,
                PowerPackVoltage = "3.44",
                SensorToBottom = (decimal)20.00,
                SewerOpening = typeof(SewerOpeningFactory),
                SewerOpeningNumber = "67890",
                SignalStrength = "4",
                SignalQuality = "14",
                Temperature = "55.4",
                WaterLevelAboveBottom = "-58.9"
            });
        }

        public SmartCoverAlertFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region SmartCoverAlertApplicationDescriptionType

    public class SmartCoverAlertApplicationDescriptionTypeFactory : TestDataFactory<SmartCoverAlertApplicationDescriptionType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SIN# 9169 SmartRain";

        #endregion

        #region Constructors

        static SmartCoverAlertApplicationDescriptionTypeFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public SmartCoverAlertApplicationDescriptionTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion
    
    #region Spoil

    public class SpoilFactory : TestDataFactory<Spoil>
    {
        static SpoilFactory()
        {
            Defaults(new {
                Quantity = 1m,
                SpoilStorageLocation = typeof(SpoilStorageLocationFactory),
                WorkOrder = typeof(WorkOrderFactory)
            });
        }

        public SpoilFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SpoilRemoval

    public class SpoilRemovalFactory : TestDataFactory<SpoilRemoval>
    {
        static SpoilRemovalFactory()
        {
            Defaults(new {
                RemovedFrom = typeof(SpoilStorageLocationFactory),
                DateRemoved = DateTime.Now,
                FinalDestination = typeof(SpoilFinalProcessingLocationFactory)
            });
        }

        public SpoilRemovalFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SpoilStorageLocation

    public class SpoilStorageLocationFactory : TestDataFactory<SpoilStorageLocation>
    {
        static SpoilStorageLocationFactory()
        {
            var i = 0;
            Func<string> nameFn = () => $"Location {++i}";
            Defaults(new {Name = nameFn, OperatingCenter = typeof(OperatingCenterFactory) });
        }

        public SpoilStorageLocationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SpoilFinalProcessingLocation

    public class SpoilFinalProcessingLocationFactory : TestDataFactory<SpoilFinalProcessingLocation>
    {
        static SpoilFinalProcessingLocationFactory()
        {
            var i = 0;
            Func<string> nameFn = () => $"Location {++i}";
            Defaults(new { Name = nameFn, OperatingCenter = typeof(OperatingCenterFactory) });
        }

        public SpoilFinalProcessingLocationFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region StandardOperatingProcedure

    public class StandardOperatingProcedureFactory : TestDataFactory<StandardOperatingProcedure>
    {
        public StandardOperatingProcedureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region StandardOperatingProcedureQuestion

    public class StandardOperatingProcedureQuestionFactory : TestDataFactory<StandardOperatingProcedureQuestion>
    {
        static StandardOperatingProcedureQuestionFactory()
        {
            Defaults(new {
                StandardOperatingProcedure = typeof(StandardOperatingProcedureFactory),
                Question = "What is love?",
                Answer = "Baby don't hurt me."
            });
        }

        public StandardOperatingProcedureQuestionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region StandardOperatingProcedureReview

    public class StandardOperatingProcedureReviewFactory : TestDataFactory<StandardOperatingProcedureReview>
    {
        static StandardOperatingProcedureReviewFactory()
        {
            Defaults(new {
                AnsweredBy = typeof(UserFactory),
                AnsweredAt = DateTime.Now,
                StandardOperatingProcedure = typeof(StandardOperatingProcedureFactory)
            });
        }

        public StandardOperatingProcedureReviewFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region State Factory

    #region State

    public class StateFactory : TestDataFactory<State>
    {
        #region Constants

        public const string DEFAULT_NAME = "New Jersey",
                            DEFAULT_ABBREVIATION = "NJ";

        #endregion

        #region Fields

        public static int _count;

        #endregion

        #region Constructors

        static StateFactory()
        {
            Func<string> nameCreatorThing = () => {
                _count++;
                return String.Format("{0} {1}", DEFAULT_NAME, _count);
            };
            Defaults(new {
                Name = nameCreatorThing,
                Abbreviation = DEFAULT_ABBREVIATION
            });
        }

        public StateFactory(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        protected override State Save(State entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<State>>();
            var existing = repo.GetAll().SingleOrDefault(x => x.Abbreviation == entity.Abbreviation);
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    #endregion

    #endregion

    #region StateOfMatter

    public class StateOfMatterFactory : UniqueEntityLookupFactory<StateOfMatter>
    {
        public StateOfMatterFactory(IContainer container) : base(container) { }
    }

    public class SolidStateOfMatterFactory : StateOfMatterFactory
    {
        public const string DEFAULT_DESCRIPTION = "Solid";

        public SolidStateOfMatterFactory(IContainer container) : base(container) { }

        static SolidStateOfMatterFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = StateOfMatter.Indices.SOLID);
        }
    }

    public class LiquidStateOfMatterFactory : StateOfMatterFactory
    {
        public const string DEFAULT_DESCRIPTION = "Liquid";

        public LiquidStateOfMatterFactory(IContainer container) : base(container) { }

        static LiquidStateOfMatterFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = StateOfMatter.Indices.LIQUID);
        }
    }

    public class GasStateOfMatterFactory : StateOfMatterFactory
    {
        public const string DEFAULT_DESCRIPTION = "Gas";

        public GasStateOfMatterFactory(IContainer container) : base(container) { }

        static GasStateOfMatterFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = StateOfMatter.Indices.GAS);
        }
    }

    #endregion

    #region StateRegion Factory

    public class StateRegionFactory : TestDataFactory<StateRegion>
    {
        public StateRegionFactory(IContainer container) : base(container) { }

        static StateRegionFactory()
        {
            var i = 0;
            Func<string> regionFn = () => String.Format("Region {0}", ++i);

            Defaults(new {
                Region = regionFn,
                State = typeof(StateFactory)
            });
        }
    }

    #endregion

    #region StockLocation

    public class StockLocationFactory : TestDataFactory<StockLocation>
    {
        #region Fields

        private static int _descId = 1;

        #endregion

        #region Constructors

        static StockLocationFactory()
        {
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
            });
        }

        public StockLocationFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private static string GenerateDescription()
        {
            // This is done up just to give us some results that are similar
            // to what actually is in the live db.
            _descId++;
            return "StockLoc" + _descId;
        }

        #endregion

        #region Public Methods

        public override StockLocation Build(object overrides = null)
        {
            var stock = base.Build(overrides);

            if (stock.Description == null)
            {
                stock.Description = GenerateDescription();
            }

            return stock;
        }

        #endregion
    }

    #endregion

    #region StormWaterAssetFactory

    public class StormWaterAssetFactory : TestDataFactory<StormWaterAsset>
    {
        #region Fields

        private static int _assetNumberPrefix = 1;
        private static int _assetNumberSuffix = 1;

        #endregion

        #region Constructors

        static StormWaterAssetFactory()
        {
            Defaults(new {
                AssetType = typeof(StormWaterAssetTypeFactory),
                Coordinate = typeof(CoordinateFactory),
                CreatedBy = "factory",
                OperatingCenter = typeof(OperatingCenterFactory),
                Town = typeof(TownFactory)
            });
        }

        public StormWaterAssetFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private static string GenerateUniqueAssetNumber()
        {
            // This is done up just to give us some results that are similar
            // to what actually is in the live db.

            try
            {
                return string.Format("{0}-{1}", _assetNumberPrefix, _assetNumberSuffix);
            }
            finally
            {
                if (_assetNumberSuffix >= 10)
                {
                    _assetNumberSuffix = 1;
                    _assetNumberPrefix += 1;
                }

                _assetNumberSuffix += 1;
            }
        }

        #endregion

        #region Public Methods

        public override StormWaterAsset Build(object overrides = null)
        {
            var asset = base.Build(overrides);

            if (asset.AssetNumber == null)
            {
                asset.AssetNumber = GenerateUniqueAssetNumber();
            }

            return asset;
        }

        #endregion
    }

    #endregion

    #region StormWaterAssetTypeFactory

    public class StormWaterAssetTypeFactory : TestDataFactory<StormWaterAssetType>
    {
        #region Constructors

        static StormWaterAssetTypeFactory()
        {
            Defaults(new {
                Description = "Catch Basin"
            });
        }

        public StormWaterAssetTypeFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected override StormWaterAssetType Save(StormWaterAssetType entity)
        {
            // Prevent duplicate asset types from being created. They should all use the same instance.
            var repo = _container.GetInstance<RepositoryBase<StormWaterAssetType>>();
            var existing = repo.GetAll().Where(x => x.Description == entity.Description).SingleOrDefault();
            return existing ?? base.Save(entity);
        }

        #endregion
    }

    #endregion

    #region Street

    public class StreetFactory : TestDataFactory<Street>
    {
        #region Constants

        public const string DEFAULT_SUFFIX = "Street";

        #endregion

        #region Constructors

        static StreetFactory()
        {
            var i = 0;

            Func<string> nameFn = () => $"{(++i).ToString().Ordinalize()}";

            Defaults(new {
                Name = nameFn,
                Prefix = typeof(StreetPrefixFactory),
                Suffix = typeof(StreetSuffixFactory),
                Town = typeof(TownFactory)
            });
        }

        public StreetFactory(IContainer container) : base(container) { }

        #endregion

        public override Street Build(object overrides = null)
        {
            var street = base.Build(overrides);
            street.FullStName = $"{street.Prefix} {street.Name} {street.Suffix}".Trim();
            return street;
        }
    }

    public class StreetPrefixFactory : EntityLookupTestDataFactory<StreetPrefix>
    {
        public StreetPrefixFactory(IContainer container) : base(container) { }
    }

    public class StreetSuffixFactory : EntityLookupTestDataFactory<StreetSuffix>
    {
        public StreetSuffixFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region StreetOpeningPermitFactory

    public class StreetOpeningPermitFactory : TestDataFactory<StreetOpeningPermit>
    {
        static StreetOpeningPermitFactory()
        {
            var i = 0;
            Func<string> sopNumberFn = () => (++i).ToString();
            Defaults(new {
                StreetOpeningPermitNumber = sopNumberFn,
                DateRequested = Lambdas.GetNowDate
            });
        }

        public StreetOpeningPermitFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region SystemDeliveryIgnitionEntry

    public class SystemDeliveryIgnitionEntryFactory : TestDataFactory<SystemDeliveryIgnitionEntry>
    {
        static SystemDeliveryIgnitionEntryFactory()
        {
            Defaults(new {
                FacilityId = 0,
                UnitOfMeasure = "MGD",
                SystemDeliveryType = SystemDeliveryType.Indices.WATER,
                SystemDeliveryEntryType = SystemDeliveryEntryType.Indices.DELIVERED_WATER,
                EntryDate = Lambdas.GetNowDate,
                FacilityName = "Acme Water Co.",
                EntryValue = 3.14M
            });
        }
        
        public SystemDeliveryIgnitionEntryFactory(IContainer container) : base(container) { }
    }
    
    #endregion

    #region SystemDeliveryEntry

    public class SystemDeliveryEntryFactory : TestDataFactory<SystemDeliveryEntry>
    {
        static SystemDeliveryEntryFactory()
        {
            Defaults(new {
                WeekOf = Lambdas.GetNow,
                IsValidated = false,
                EnteredBy = typeof(EmployeeFactory),
                SystemDeliveryType = typeof(SystemDeliveryTypeFactory),
                IsHyperionFileCreated = false
            });
        }

        public SystemDeliveryEntryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SystemDeliveryFacilityEntry

    public class SystemDeliveryFacilityEntryFactory : TestDataFactory<SystemDeliveryFacilityEntry>
    {
        static SystemDeliveryFacilityEntryFactory()
        {
            Defaults(new {
                SystemDeliveryType = typeof(SystemDeliveryTypeFactory),
                SystemDeliveryEntry = typeof(SystemDeliveryEntryFactory),
                SystemDeliveryEntryType = typeof(SystemDeliveryEntryTypeFactory),
                EnteredBy = typeof(EmployeeFactory),
                Facility = typeof(FacilityFactory),
                EntryDate = new DateTime(2020, 1, 4),
                EntryValue = (decimal)2.123,
                IsInjection = false,
                HasBeenAdjusted = false
            });
        }

        public SystemDeliveryFacilityEntryFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region SystemDeliveryFacilityEntryAdjustment

    public class SystemDeliveryFacilityEntryAdjustmentFactory : TestDataFactory<SystemDeliveryFacilityEntryAdjustment>
    {
        static SystemDeliveryFacilityEntryAdjustmentFactory()
        {
            Defaults(new {
                SystemDeliveryFacilityEntry = typeof(SystemDeliveryFacilityEntryFactory),
                SystemDeliveryEntry = typeof(SystemDeliveryEntryFactory),
                Facility = typeof(FacilityFactory),
                EnteredBy = typeof(EmployeeFactory),
                AdjustedDate = Lambdas.GetYesterdayDate,
                AdjustedEntryValue = (decimal)3.145,
                OriginalEntryValue = (decimal)2.125,
                DateTimeEntered = Lambdas.GetNowDate
            });
        }

        public SystemDeliveryFacilityEntryAdjustmentFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SystemDeliveryType

    public class
        SystemDeliveryTypeFactory : StaticListEntityLookupFactory<SystemDeliveryType, SystemDeliveryTypeFactory>
    {
        static SystemDeliveryTypeFactory() { }

        public SystemDeliveryTypeFactory(IContainer container) : base(container) { }
    }

    public class SystemDeliveryTypeWaterFactory : SystemDeliveryTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Water";

        static SystemDeliveryTypeWaterFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
            });
            OnSaving((a, s) => a.Id = SystemDeliveryType.Indices.WATER);
        }

        public SystemDeliveryTypeWaterFactory(IContainer container) : base(container) { }
    }

    public class SystemDeliveryTypeWasteWaterFactory : SystemDeliveryTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Wastewater";

        static SystemDeliveryTypeWasteWaterFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
            });
            OnSaving((a, s) => a.Id = SystemDeliveryType.Indices.WASTE_WATER);
        }

        public SystemDeliveryTypeWasteWaterFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region SystemDeliveryEntryType

    public class SystemDeliveryEntryTypeFactory : TestDataFactory<SystemDeliveryEntryType>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "SystemDeliveryEntryType";

        #endregion

        #region Constructors

        static SystemDeliveryEntryTypeFactory()
        {
            var i = 0;
            Func<string> descriptionFn = () => $"System Delivery Type {++i}";

            Defaults(new {
                Description = descriptionFn,
                SystemDeliveryType = typeof(SystemDeliveryTypeFactory)
            });
        }

        public SystemDeliveryEntryTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region TailgateTalkFactory

    public class TailgateTopicMonthFactory : EntityLookupTestDataFactory<TailgateTopicMonth>
    {
        public TailgateTopicMonthFactory(IContainer container) : base(container) { }
    }

    public class TailgateTopicCategoryFactory : EntityLookupTestDataFactory<TailgateTopicCategory>
    {
        public TailgateTopicCategoryFactory(IContainer container) : base(container) { }
    }

    public class TailgateTalkTopicFactory : TestDataFactory<TailgateTalkTopic>
    {
        public TailgateTalkTopicFactory(IContainer container) : base(container) { }

        static TailgateTalkTopicFactory()
        {
            var i = 0;
            Func<string> topicFn = () => String.Format("idk what do you guys wanna talk about? {0}", ++i);
            Func<string> ormReferenceNumberFn = () => i.ToString().PadLeft(8, '0');

            Defaults(new {
                IsActive = true,
                Topic = topicFn,
                OrmReferenceNumber = ormReferenceNumberFn,
                Category = typeof(TailgateTopicCategoryFactory),
                Month = typeof(TailgateTopicMonthFactory),
                TargetDeliveryDate = Lambdas.GetNow,
                TopicLevel = TopicLevels.Local,
            });
        }
    }

    public class TailgateTalkFactory : TestDataFactory<TailgateTalk>
    {
        public TailgateTalkFactory(IContainer container) : base(container) { }

        static TailgateTalkFactory()
        {
            var i = 0;
            Func<decimal> trainingTimeHoursFn = () => (++i) * 1.1m;

            Defaults(new {
                HeldOn = Lambdas.GetNow,
                Topic = typeof(TailgateTalkTopicFactory),
                PresentedBy = typeof(EmployeeFactory),
                TrainingTimeHours = trainingTimeHoursFn,
            });
        }
    }

    #endregion

    #region TankInspection

    public class TankStructureTypeFactory : EntityLookupTestDataFactory<TankStructureType>
    {
        public TankStructureTypeFactory(IContainer container) : base(container) { }
    }

    public class TankInspectionTypeFactory : EntityLookupTestDataFactory<TankInspectionType>
    {
        public TankInspectionTypeFactory(IContainer container) : base(container) { }
    }

    public class TankInspectionQuestionGroupFactory : EntityLookupTestDataFactory<TankInspectionQuestionGroup>
    {
        public TankInspectionQuestionGroupFactory(IContainer container) : base(container) { }

        static TankInspectionQuestionGroupFactory()
        {
            var i = 0;

            Func<string> descriptionFn = () => $"Bucket Security {++i}";

            Defaults(new {
                Description = descriptionFn
            });
        }
    }

    public class TankInspectionQuestionTypeFactory : TestDataFactory<TankInspectionQuestionType>
    {
        public TankInspectionQuestionTypeFactory(IContainer container) : base(container) { }

        static TankInspectionQuestionTypeFactory()
        {
            var i = 0;

            Func<string> descriptionFn = () => $"Bucket Security {++i}";

            Defaults(new {
                TankInspectionQuestionGroup = typeof(TankInspectionQuestionGroupFactory),
                Description = descriptionFn
            });
        }
    }

    public class TankInspectionQuestionFactory : TestDataFactory<TankInspectionQuestion>
    {
        public TankInspectionQuestionFactory(IContainer container) : base(container) { }

        static TankInspectionQuestionFactory()
        {
            Defaults(new {
                TankInspectionQuestionType = typeof(TankInspectionQuestionTypeFactory)
            });
        }
    }

    public class TankInspectionFactory : TestDataFactory<TankInspection>
    {
        static TankInspectionFactory()
        {
            Defaults(new {
                TankObservedBy = typeof(EmployeeFactory), 
                Town = typeof(TownFactory),
                Facility = typeof(FacilityFactory),
                Equipment = typeof(EquipmentFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                TankInspectionType = typeof(TankInspectionTypeFactory),
                Coordinate = typeof(CoordinateFactory),
                TankCapacity = 1m
            });
        }

        public TankInspectionFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region TapImage

    public class TapImageFactory : TestDataFactory<TapImage>
    {
        #region Constructors

        static TapImageFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                FileName = "some file.tif",
                Directory = "SomeDirectory",
                ServiceType = "Some service type"
            });
        }

        public TapImageFactory(IContainer container) : base(container) { }

        #endregion

        public override TapImage Create(object overrides = null)
        {
            var t = base.Create(overrides);
            if (t.Service != null && !t.Service.TapImages.Contains(t))
            {
                t.Service.TapImages.Add(t);
            }

            return t;
        }
    }

    #endregion

    #region TaskGroupCategory

    public class TaskGroupCategoryFactory : TestDataFactory<TaskGroupCategory>
    {
        #region Constants

        public const string DEFAULT_TYPE = "Category 1";
        public const string DEFAULT_ABBREVIATION = "CAT1";
        public const bool DEFAULT_ISACTIVE = true;
        public const string DEFAULT_DESCRIPTION = "Category 1 Description";

        #endregion  

        static TaskGroupCategoryFactory()
        {
            Defaults(new {
                Type = DEFAULT_TYPE,
                Abbreviation = DEFAULT_ABBREVIATION,
                IsActive = DEFAULT_ISACTIVE,
                Description = DEFAULT_DESCRIPTION
            });
        }

        public TaskGroupCategoryFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TaskGroup

    public class TaskGroupFactory : TestDataFactory<TaskGroup>
    {
        #region Constants

        public const string DEFAULT_TASK_GROUP_ID = "Coconuts";
        public const string DEFAULT_TASK_NAME = "SirRobin";
        public const string DEFAULT_TASK_DETAILS = "Heranawayaway";
        public const string DEFAULT_TASK_DETAILS_SUMMARY = "YoinksAndAway";
        public const decimal DEFAULT_RESOURCES = 1.05M;
        public const decimal DEFAULT_ESTIMATED_HOURS = 23.75M;
        public const decimal DEFAULT_CONTRACTOR_COST = 2;

        #endregion

        static TaskGroupFactory()
        {
            Defaults(new {
                TaskGroupId = DEFAULT_TASK_GROUP_ID,
                TaskGroupName = DEFAULT_TASK_NAME,
                TaskGroupCategory = typeof(TaskGroupCategoryFactory),
                MaintenancePlanTaskType = typeof(MaintenancePlanTaskTypeFactory),
                TaskDetails = DEFAULT_TASK_DETAILS,
                TaskDetailsSummary = DEFAULT_TASK_DETAILS_SUMMARY,
                Caption = $"{DEFAULT_TASK_GROUP_ID} - {DEFAULT_TASK_NAME}"
            });
        }

        public TaskGroupFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MaintenancePlanTaskType

    public class MaintenancePlanTaskTypeFactory : TestDataFactory<MaintenancePlanTaskType>
    {
        #region Constructors

        static MaintenancePlanTaskTypeFactory()
        {
            Defaults(new {
                Description = "Test Task Type",
                Abbreviation = "TTT",
                IsActive = true
            });
        }

        public MaintenancePlanTaskTypeFactory(IContainer container) : base(container) { }

        #endregion
    }
    #endregion

    #region TownFactory

    public class TownFactory : TestDataFactory<Town>
    {
        #region Constants

        public static string SHORT_NAME = "Long Bra";
        public static string FULL_NAME = "Long Branch";
        public static string CRITICAL_MAIN_BREAK_NOTES = "Blah Blah Blah";

        #endregion

        #region Fields

        public static int _townCount;

        #endregion

        static TownFactory()
        {
            Func<string> shortNameCreatorThing = () => {
                _townCount++;
                return $"{SHORT_NAME} {_townCount}";
            };
            Func<string> fullNameCreator = () => $"{FULL_NAME} {_townCount}";
            Defaults(new {
                ShortName = shortNameCreatorThing,
                FullName = fullNameCreator,
                // State = typeof(StateFactory),
                County = typeof(CountyFactory),
                AbbreviationType = typeof(TownAbbreviationTypeFactory),
                CriticalMainBreakNotes = CRITICAL_MAIN_BREAK_NOTES
            });
        }

        public TownFactory(IContainer container) : base(container) { }

        public override Town Create(object overrides = null)
        {
            var town = base.Create(overrides);
            if (!town.County.Towns.Contains(town))
            {
                town.County.Towns.Add(town);
            }

            // Something about setting this leads to an "index is out of range" flush error
            // in nhibernate during regression testing if you specifically create a county,
            // then create two towns that reference that county. 
            if (town.State == null)
            {
                town.State = town.County.State;
            }

            return town;
        }
    }

    #endregion

    #region TownContactFactory

    public class TownContactFactory : TestDataFactory<TownContact>
    {
        public TownContactFactory(IContainer container) : base(container) { }

        static TownContactFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                Contact = typeof(ContactFactory),
                ContactType = typeof(ContactTypeFactory)
            });
        }

        public override TownContact Build(object overrides = null)
        {
            var tc = base.Build(overrides);

            if (!tc.Town.TownContacts.Contains(tc))
            {
                tc.Town.TownContacts.Add(tc);
            }

            if (tc.Contact != null && !tc.Contact.TownContacts.Contains(tc))
            {
                tc.Contact.TownContacts.Add(tc);
            }

            return tc;
        }
    }

    #endregion

    #region TownSection

    public class TownSectionFactory : TestDataFactory<TownSection>
    {
        #region Constructors

        static TownSectionFactory()
        {
            var i = 0;
            Func<string> nameFn = () => $"Section {++i}";
            Func<string> abbrevFn = () => $"TS {i}";
            Defaults(new {
                Name = nameFn,
                Town = typeof(TownFactory),
                Abbreviation = abbrevFn,
                Active = true
            });
        }

        public TownSectionFactory(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        public override TownSection Create(object overrides = null)
        {
            var ts = base.Create(overrides);

            if (!ts.Town.TownSections.Contains(ts))
            {
                ts.Town.TownSections.Add(ts);
            }

            return ts;
        }

        #endregion
    }

    #endregion

    #region TrafficControl

    public class TrafficControlTicketFactory : TestDataFactory<TrafficControlTicket>
    {
        #region Constants

        public const string STREET_NUMBER = "123";

        #endregion

        static TrafficControlTicketFactory()
        {
            var i = 0;
            Func<string> accountingCodeFn = () => string.Format("Accounting{0}", ++i);

            Defaults(new {
                WorkStartDate = Lambdas.GetNow,
                StreetNumber = STREET_NUMBER,
                Street = typeof(StreetFactory),
                Town = typeof(TownFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                AccountingCode = accountingCodeFn,
                Coordinate = typeof(CoordinateFactory),
                DateApproved = Lambdas.GetNow,
                BillingParty = typeof(BillingPartyFactory)
            });
        }

        public TrafficControlTicketFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TrafficControlTicketCheck

    public class TrafficControlTicketCheckFactory : TestDataFactory<TrafficControlTicketCheck>
    {
        #region Constants

        public const decimal DEFAULT_AMOUNT = 250m;

        #endregion

        #region Constructors

        static TrafficControlTicketCheckFactory()
        {
            var i = 0;
            Func<int> checkNumberFn = () => ++i;

            Defaults(new {
                TrafficControlTicket = typeof(TrafficControlTicketFactory),
                CheckNumber = checkNumberFn,
                Amount = DEFAULT_AMOUNT
            });
        }

        public TrafficControlTicketCheckFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region TrainingRecordFactory

    public class TrainingRecordFactory : TestDataFactory<TrainingRecord>
    {
        static TrainingRecordFactory()
        {
            Defaults(new {
                TrainingModule = typeof(TrainingModuleFactory),
                ClassLocation = typeof(ClassLocationFactory) // Nullable, but required field.
            });
        }

        public TrainingRecordFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TrainingRecordAttendedEmployeeFactory

    public class TrainingRecordAttendedEmployeeFactory : TestDataFactory<EmployeeLink>
    {
        public const string TABLE_NAME = TrainingRecordMap.TABLE_NAME,
                            DATA_TYPE_NAME = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED;

        static TrainingRecordAttendedEmployeeFactory()
        {
            Defaults(new {
                DataType = (Func<IContainer, object>)GetDataType,
                Employee = typeof(EmployeeFactory),
                LinkedId = (Func<IContainer, object>)GetLinkedId
            });
        }

        private static DataType GetDataType(IContainer container)
        {
            var dataType = container.GetInstance<DataTypeRepository>()
                                    .GetByTableNameAndDataTypeName(TABLE_NAME, DATA_TYPE_NAME).FirstOrDefault();

            return dataType ?? new DataTypeFactory(container).Create(new {
                TableName = TABLE_NAME,
                Name = DATA_TYPE_NAME
            });
        }

        private static object GetLinkedId(IContainer container)
        {
            return new TrainingRecordFactory(container).Create().Id;
        }

        public TrainingRecordAttendedEmployeeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TrainingRecordScheduledEmployeeFactory

    public class TrainingRecordScheduledEmployeeFactory : TestDataFactory<EmployeeLink>
    {
        public const string TABLE_NAME = TrainingRecordMap.TABLE_NAME,
                            DATA_TYPE_NAME = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED;

        static TrainingRecordScheduledEmployeeFactory()
        {
            Defaults(new {
                DataType = (Func<IContainer, object>)GetDataType,
                Employee = typeof(EmployeeFactory),
                LinkedId = (Func<IContainer, object>)GetLinkedId
            });
        }

        public TrainingRecordScheduledEmployeeFactory(IContainer container) : base(container) { }

        private static DataType GetDataType(IContainer container)
        {
            var dataType = container.GetInstance<DataTypeRepository>()
                                    .GetByTableNameAndDataTypeName(TABLE_NAME, DATA_TYPE_NAME).FirstOrDefault();

            return dataType ?? new DataTypeFactory(container).Create(new {
                TableName = TABLE_NAME,
                Name = DATA_TYPE_NAME
            });
        }

        private static object GetLinkedId(IContainer container)
        {
            return new TrainingRecordFactory(container).Create().Id;
        }
    }

    #endregion

    #region TrainingContactHoursProgramCoordinator

    public class TrainingContactHoursProgramCoordinatorFactory : TestDataFactory<TrainingContactHoursProgramCoordinator>
    {
        #region Constants

        #endregion

        #region Constructors

        static TrainingContactHoursProgramCoordinatorFactory()
        {
            Defaults(new {
                ProgramCoordinator = typeof(EmployeeFactory)
            });
        }

        public TrainingContactHoursProgramCoordinatorFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region TrainingModule

    public class TrainingModuleFactory : TestDataFactory<TrainingModule>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "module description",
                            DEFAULT_TITLE = "Test Training";

        #endregion

        #region Constructors

        static TrainingModuleFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION,
                Title = DEFAULT_TITLE,
                TrainingRequirement = typeof(TrainingRequirementFactory),
                IsActive = true,
                OperatingCenter = typeof(OperatingCenterFactory),
            });
        }

        public TrainingModuleFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region TrainingModuleCategory

    public class TrainingModuleCategoryFactory : TestDataFactory<TrainingModuleCategory>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "TrainingModuleCategory";

        #endregion

        #region Constructors

        static TrainingModuleCategoryFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public TrainingModuleCategoryFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region TrainingModuleRecurrantType

    public class TrainingModuleRecurrantTypeFactory : StaticListEntityLookupFactory<TrainingModuleRecurrantType,
        TrainingModuleRecurrantTypeFactory>
    {
        #region Constructors

        public TrainingModuleRecurrantTypeFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InitialTrainingModuleRecurrantTypeFactory : TrainingModuleRecurrantTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Initial";

        static InitialTrainingModuleRecurrantTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)TrainingModuleRecurrantType.Indices.INITIAL);
        }

        public InitialTrainingModuleRecurrantTypeFactory(IContainer container) : base(container) { }
    }

    public class RecurringTrainingModuleRecurrantTypeFactory : TrainingModuleRecurrantTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Recurring";

        static RecurringTrainingModuleRecurrantTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)TrainingModuleRecurrantType.Indices.RECURRING);
        }

        public RecurringTrainingModuleRecurrantTypeFactory(IContainer container) : base(container) { }
    }

    public class InitialAndRecurringTrainingModuleRecurrantTypeFactory : TrainingModuleRecurrantTypeFactory
    {
        public const string DEFAULT_DESCRIPTION = "Initial and Recurring";

        static InitialAndRecurringTrainingModuleRecurrantTypeFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION});
            OnSaving((a, s) => a.Id = (int)TrainingModuleRecurrantType.Indices.INITIAL_RECURRING);
        }

        public InitialAndRecurringTrainingModuleRecurrantTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TrainingRequirement

    public class TrainingRequirementFactory : TestDataFactory<TrainingRequirement>
    {
        static TrainingRequirementFactory()
        {
            var i = 0;
            Func<string> descriptionFn = (() => String.Format("Training Regulation {0}", ++i));
            Defaults(new {
                Description = descriptionFn,
                Regulation = typeof(RegulationFactory)
            });
        }

        public TrainingRequirementFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region TrainingSession

    public class TrainingSessionFactory : TestDataFactory<TrainingSession>
    {
        static TrainingSessionFactory()
        {
            Defaults(new {
                TrainingRecord = typeof(TrainingRecordFactory),
                StartDateTime = Lambdas.GetYesterday,
                EndDateTime = Lambdas.GetNowDate
            });
        }

        public TrainingSessionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region UnionContractFactory

    public class UnionContractFactory : TestDataFactory<UnionContract>
    {
        public UnionContractFactory(IContainer container) : base(container) { }

        static UnionContractFactory()
        {
            Func<DateTime> startDateFn = () => DateTime.Now;
            Func<DateTime> endDateFn = () => DateTime.Now.AddDays(1);
            Defaults(new {
                OperatingCenter = typeof(OperatingCenterFactory),
                StartDate = startDateFn,
                EndDate = endDateFn
            });
        }
    }

    #endregion

    #region UnionContractProposalFactory

    public class UnionContractProposalFactory : TestDataFactory<UnionContractProposal>
    {
        public UnionContractProposalFactory(IContainer container) : base(container) { }

        static UnionContractProposalFactory()
        {
            Defaults(new {
                Contract = typeof(UnionContractFactory)
            });
        }
    }

    #endregion

    #region UnionFactory

    public class UnionFactory : TestDataFactory<Union>
    {
        public UnionFactory(IContainer container) : base(container) { }

        static UnionFactory()
        {
            var idx = 0;
            Func<string> bargainingUnitFn = () => String.Format("union{0}", ++idx);
            Defaults(new {
                BargainingUnit = bargainingUnitFn
            });
        }
    }

    #endregion

    #region UnitOfMeasure

    public class UnitOfMeasureFactory : UniqueEntityLookupFactory<UnitOfMeasure>
    {
        public UnitOfMeasureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region UnitOfWaterSampleMeasure

    public class UnitOfWaterSampleMeasureFactory : UniqueEntityLookupFactory<UnitOfWaterSampleMeasure>
    {
        public UnitOfWaterSampleMeasureFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region UserFactory

    public class UserFactory : TestDataFactory<User>
    {
        #region Fields

        private static int _userCount;

        #endregion

        #region Constructors

        static UserFactory()
        {
            Func<string> userNameMakeGetterCreatorThing = () => {
                _userCount++;
                return "user" + _userCount;
            };
            Func<string> fullNameMakeGetterCreatorThing = () => { return "Full Name" + _userCount; };
            Defaults(new {
                UserName = userNameMakeGetterCreatorThing,
                FullName = fullNameMakeGetterCreatorThing,
                DefaultOperatingCenter = typeof(OperatingCenterFactory),
                Email = "a@a.com",
                IsAdmin = false,
                HasAccess = true,
                UserType = typeof(InternalUserTypeFactory) // This is the default UserType for most MapCall users.
            });
        }

        public UserFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class AdminUserFactory : UserFactory
    {
        #region Fields

        private static int _userCount;

        #endregion

        #region Constructors

        static AdminUserFactory()
        {
            Func<string> userNameMakeGetterCreatorThing = () => {
                _userCount++;
                return "adminUser" + _userCount;
            };
            Defaults(new {
                UserName = userNameMakeGetterCreatorThing,
                IsAdmin = true,
            });
        }

        public AdminUserFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region UserViewed

    public class UserViewedFactory : TestDataFactory<UserViewed>
    {
        #region Constructors

        static UserViewedFactory()
        {
            Defaults(new {
                User = typeof(UserFactory),
                ViewedAt = Lambdas.GetNow,
                // NOTE: TapImage, ValveImage, and AsBuiltImage are all nullable, 
                // but one of those is required to create a valid record. Do not
                // add one as a default here. Make an inherited factory if you need it.
            });
        }

        public UserViewedFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class UserViewedTapImageFactory : UserViewedFactory
    {
        #region Constructors

        static UserViewedTapImageFactory()
        {
            Defaults(new {
                TapImage = typeof(TapImageFactory)
            });
        }

        public UserViewedTapImageFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region UserTypeFactory

    public class UserTypeFactory : UniqueEntityLookupFactory<UserType>
    {
        public UserTypeFactory(IContainer container) : base(container) { }
    }

    public class InternalUserTypeFactory : UserTypeFactory
    {
        public InternalUserTypeFactory(IContainer container) : base(container) { }

        public override UserType Build(object overrides = null)
        {
            var ut = base.Build(overrides);
            ut.Description = "Internal";
            return ut;
        }
    }

    #endregion

    #region Valve

    public class ValveFactory : TestDataFactory<Valve>
    {
        #region Constants

        public const string DEFAULT_VALNUM = "VAB-100";
        public const int DEFAULT_VALVE_SUFFIX = 100;

        #endregion

        #region Constructors

        static ValveFactory()
        {
            Defaults(new {
                BPUKPI = false,
                Critical = false,
                Coordinate = typeof(CoordinateFactory),
                Traffic = false,
                ValveNumber = DEFAULT_VALNUM,
                ValveSuffix = 100,
                Town = typeof(TownFactory),
                //TownSection = typeof(TownSectionFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                SAPEquipmentId = 1,
                FunctionalLocation = typeof(FunctionalLocationFactory),
                Street = typeof(StreetFactory),
                ValveZone = typeof(ValveZoneFactory),
                Status = typeof(ActiveAssetStatusFactory),
                ValveBilling = typeof(PublicValveBillingFactory),
                ValveControls = typeof(SomeValveControlFactory),
                ValveSize = typeof(TestDataFactory<ValveSize>),
                WorkOrderNumber = "work order number"
            });
        }

        public ValveFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class BlowOffValveFactory : ValveFactory
    {
        #region Constructors

        static BlowOffValveFactory()
        {
            Defaults(new {
                ValveControls = typeof(BlowOffWithFlushingValveControlFactory)
            });
        }

        public BlowOffValveFactory(IContainer container) : base(container) { }

        #endregion

        public override Valve Create(object overrides = null)
        {
            var valve = base.Create(overrides);
            Session.Refresh(valve); // Refresh to make sure formula properties are up to date
            return valve;
        }
    }

    #endregion

    #region ValveControl

    public class ValveControlFactory : StaticListEntityLookupFactory<ValveControl, ValveControlFactory>
    {
        public ValveControlFactory(IContainer container) : base(container) { }
    }

    public class MainValveControlFactory : ValveControlFactory
    {
        static MainValveControlFactory()
        {
            Defaults(new {
                Description = "MAIN"
            });

            OnSaving((vc, s) => vc.Id = ValveControl.Indices.MAIN);
        }

        public MainValveControlFactory(IContainer container) : base(container) { }
    }

    public class HydrantValveControlFactory : ValveControlFactory
    {
        static HydrantValveControlFactory()
        {
            Defaults(new {
                Description = "HYDRANT"
            });
            OnSaving((vc, s) => vc.Id = ValveControl.Indices.HYDRANT);
        }

        public HydrantValveControlFactory(IContainer container) : base(container) { }
    }

    public class SomeValveControlFactory : ValveControlFactory
    {
        static SomeValveControlFactory()
        {
            Defaults(new {
                Description = "SOME VALVE CONTROL"
            });

            OnSaving((vc, s) => vc.Id = 9999);
        }

        public SomeValveControlFactory(IContainer container) : base(container) { }
    }

    public class FooValveControlFactory : ValveControlFactory
    {
        static FooValveControlFactory()
        {
            Defaults(new {
                Description = "Foo"
            });

            OnSaving((vc, s) => vc.Id = 9998);
        }

        public FooValveControlFactory(IContainer container) : base(container) { }
    }

    public class BarValveControlFactory : ValveControlFactory
    {
        static BarValveControlFactory()
        {
            Defaults(new {
                Description = "Bar"
            });

            OnSaving((vc, s) => vc.Id = 9997);
        }

        public BarValveControlFactory(IContainer container) : base(container) { }
    }

    public class BlowOffValveControlFactory : ValveControlFactory
    {
        static BlowOffValveControlFactory()
        {
            Defaults(new {
                Description = ValveControl.BLOW_OFF
            });
        }

        public BlowOffValveControlFactory(IContainer container) : base(container) { }

        protected override ValveControl Save(ValveControl entity)
        {
            entity.Id = ValveControl.Indices.BLOW_OFF;
            return base.Save(entity);
        }
    }

    public class BlowOffWithFlushingValveControlFactory : ValveControlFactory
    {
        static BlowOffWithFlushingValveControlFactory()
        {
            Defaults(new {
                Description = ValveControl.BLOW_OFF_WITH_FLUSHING
            });
            // OnSaving((a, s) => 
            //    a.SetPropertyValueByName("Id", ValveControl.Indices.BLOW_OFF_WITH_FLUSHING)
            //);
        }

        public BlowOffWithFlushingValveControlFactory(IContainer container) : base(container) { }

        protected override ValveControl Save(ValveControl entity)
        {
            entity.Id = ValveControl.Indices.BLOW_OFF_WITH_FLUSHING;
            return base.Save(entity);
        }
    }

    #endregion

    #region ValveImage

    public class ValveImageFactory : TestDataFactory<ValveImage>
    {
        #region Constructors

        static ValveImageFactory()
        {
            Defaults(new {
                Town = typeof(TownFactory),
                OperatingCenter = typeof(OperatingCenterFactory),
                FileName = "some file.tif",
                Directory = "SomeDirectory",
                CrossStreet = "Some street",
                ValveSize = "Some valve size"
            });
        }

        public ValveImageFactory(IContainer container) : base(container) { }

        #endregion

        public override ValveImage Create(object overrides = null)
        {
            var vi = base.Create(overrides);
            if (vi.Valve != null && !vi.Valve.ValveImages.Contains(vi))
            {
                vi.Valve.ValveImages.Add(vi);
            }

            return vi;
        }
    }

    #endregion

    #region ValveInspection

    public class ValveInspectionFactory : TestDataFactory<ValveInspection>
    {
        #region Constructors

        static ValveInspectionFactory()
        {
            Defaults(new {
                Valve = typeof(ValveFactory),
                DateInspected = Lambdas.GetNow,
                InspectedBy = typeof(UserFactory)
            });
        }

        public ValveInspectionFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region ValveNormalPosition

    public class ValveNormalPositionFactory : UniqueEntityLookupFactory<ValveNormalPosition>
    {
        public ValveNormalPositionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ValveOpenDirection

    public class ValveOpenDirectionFactory : UniqueEntityLookupFactory<ValveOpenDirection>
    {
        public ValveOpenDirectionFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ValveBilling

    public class ValveBillingFactory : StaticListEntityLookupFactory<ValveBilling, ValveBillingFactory>
    {
        #region Constructors

        public ValveBillingFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PublicValveBillingFactory : ValveBillingFactory
    {
        static PublicValveBillingFactory()
        {
            Defaults(new {Description = ValveBilling.PUBLIC});
            OnSaving((a, s) => a.Id = (int)ValveBilling.Indices.PUBLIC);
        }

        public PublicValveBillingFactory(IContainer container) : base(container) { }
    }

    public class MunicipalValveBillingFactory : ValveBillingFactory
    {
        static MunicipalValveBillingFactory()
        {
            Defaults(new {Description = ValveBilling.MUNICIPAL});
            OnSaving((a, s) => a.Id = (int)ValveBilling.Indices.MUNICIPAL);
        }

        public MunicipalValveBillingFactory(IContainer container) : base(container) { }
    }

    public class OAndMValveBillingFactory : ValveBillingFactory
    {
        static OAndMValveBillingFactory()
        {
            Defaults(new {Description = ValveBilling.O_AND_M});
            OnSaving((a, s) => a.Id = (int)ValveBilling.Indices.O_AND_M);
        }

        public OAndMValveBillingFactory(IContainer container) : base(container) { }
    }

    public class CompanyValveBillingFactory : ValveBillingFactory
    {
        static CompanyValveBillingFactory()
        {
            Defaults(new {Description = ValveBilling.COMPANY});
            OnSaving((a, s) => a.Id = (int)ValveBilling.Indices.COMPANY);
        }

        public CompanyValveBillingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ValveType

    public class ValveTypeFactory : StaticListEntityLookupFactory<ValveType, ValveTypeFactory>
    {
        public ValveTypeFactory(IContainer container) : base(container) { }
    }

    public class ValveTypeBallFactory : ValveTypeFactory
    {
        static ValveTypeBallFactory()
        {
            Defaults(new {
                Description = ValveType.BALL,
                SAPCode = ValveType.BALL
            });
            OnSaving((a, s) => a.Id = ValveType.Indices.BALL);
        }

        public ValveTypeBallFactory(IContainer container) : base(container) { }
    }

    public class ValveTypeButterflyFactory : ValveTypeFactory
    {
        static ValveTypeButterflyFactory()
        {
            Defaults(new {
                Description = ValveType.BUTTERFLY,
                SAPCode = ValveType.BUTTERFLY
            });
            OnSaving((a, s) => a.Id = ValveType.Indices.BUTTERFLY);
        }

        public ValveTypeButterflyFactory(IContainer container) : base(container) { }
    }

    public class ValveTypeCheckFactory : ValveTypeFactory
    {
        static ValveTypeCheckFactory()
        {
            Defaults(new {
                Description = ValveType.CHECK,
                SAPCode = ValveType.CHECK
            });
            OnSaving((a, s) => a.Id = ValveType.Indices.CHECK);
        }

        public ValveTypeCheckFactory(IContainer container) : base(container) { }
    }

    public class ValveTypeGateFactory : ValveTypeFactory
    {
        static ValveTypeGateFactory()
        {
            Defaults(new {
                Description = ValveType.GATE,
                SAPCode = ValveType.GATE
            });
            OnSaving((a, s) => a.Id = ValveType.Indices.GATE);
        }

        public ValveTypeGateFactory(IContainer container) : base(container) { }
    }

    public class ValveTypeTappingFactory : ValveTypeFactory
    {
        static ValveTypeTappingFactory()
        {
            Defaults(new {
                Description = ValveType.TAPPING,
                SAPCode = ValveType.TAPPING
            });
            OnSaving((a, s) => a.Id = ValveType.Indices.TAPPING);
        }

        public ValveTypeTappingFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ValveWorkOrderRequest

    public class ValveWorkOrderRequestFactory : UniqueEntityLookupFactory<ValveWorkOrderRequest>
    {
        public ValveWorkOrderRequestFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ValveZone

    public class ValveZoneFactory : UniqueEntityLookupFactory<ValveZone>
    {
        #region Constructors

        public ValveZoneFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Vehicle

    public class VehicleFactory : TestDataFactory<Vehicle>
    {
        static VehicleFactory()
        {
            Defaults(new {
                VehicleIdentificationNumber = "ABCDEFGHJIKLMNOP",
                PlateNumber = "ABC-123",
                ModelYear = "1991",
                Make = "Powell",
                Model = "The Homer",
                CreatedAt = Lambdas.GetNow
            });
        }

        public VehicleFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region Video

    public class VideoFactory : TestDataFactory<Video>
    {
        static VideoFactory()
        {
            Defaults(new {
                SproutVideoId = "some id",
                Title = "some title",
            });
        }

        public VideoFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region ViolationCertificate

    public class ViolationCertificateFactory : TestDataFactory<ViolationCertificate>
    {
        #region Constants

        #endregion

        #region Constructors

        static ViolationCertificateFactory()
        {
            Defaults(new {
                Employee = typeof(EmployeeFactory),
                CertificateDate = Lambdas.GetLastYear
            });
        }

        public ViolationCertificateFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WasteWaterSystemOwnership

    public class WasteWaterSystemOwnershipFactory : UniqueEntityLookupFactory<WasteWaterSystemOwnership>
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "AW Contract";

        #endregion

        #region Constructors

        static WasteWaterSystemOwnershipFactory()
        {
            Defaults(new {
                Description = DEFAULT_DESCRIPTION
            });
        }

        public WasteWaterSystemOwnershipFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WasteWaterSystemStatus

    public class WasteWaterSystemStatusFactory : StaticListEntityLookupFactory<WasteWaterSystemStatus, WasteWaterSystemStatusFactory>
    {
        public WasteWaterSystemStatusFactory(IContainer container) : base(container) { }
    }

    public class ActiveWasteWaterSystemStatusFactory : WasteWaterSystemStatusFactory
    {
        #region Constructors

        static ActiveWasteWaterSystemStatusFactory()
        {
            Defaults(new { Description = "Active" });
            OnSaving((a, s) => a.Id = WasteWaterSystemStatus.Indices.ACTIVE);
        }

        public ActiveWasteWaterSystemStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PendingWasteWaterSystemStatusFactory : WasteWaterSystemStatusFactory
    {
        #region Constructors

        static PendingWasteWaterSystemStatusFactory()
        {
            Defaults(new { Description = "Pending" });
            OnSaving((a, s) => a.Id = WasteWaterSystemStatus.Indices.PENDING);
        }

        public PendingWasteWaterSystemStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PendingMergerWasteWaterSystemStatusFactory : WasteWaterSystemStatusFactory
    {
        #region Constructors

        static PendingMergerWasteWaterSystemStatusFactory()
        {
            Defaults(new { Description = "Pending Merger" });
            OnSaving((a, s) => a.Id = WasteWaterSystemStatus.Indices.PENDING_MERGER);
        }

        public PendingMergerWasteWaterSystemStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InactiveWasteWaterSystemStatusFactory : WasteWaterSystemStatusFactory
    {
        #region Constructors

        static InactiveWasteWaterSystemStatusFactory()
        {
            Defaults(new { Description = "Inactive" });
            OnSaving((a, s) => a.Id = WasteWaterSystemStatus.Indices.INACTIVE);
        }

        public InactiveWasteWaterSystemStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class InactiveSeeNoteWasteWaterSystemStatusFactory : WasteWaterSystemStatusFactory
    {
        #region Constructors

        static InactiveSeeNoteWasteWaterSystemStatusFactory()
        {
            Defaults(new { Description = "Inactive-see note" });
            OnSaving((a, s) => a.Id = WasteWaterSystemStatus.Indices.INACTIVE_SEE_NOTE);
        }

        public InactiveSeeNoteWasteWaterSystemStatusFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WaterConstituent

    public class WaterConstituentFactory : TestDataFactory<WaterConstituent>
    {
        public WaterConstituentFactory(IContainer container) : base(container) { }

        static WaterConstituentFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"Constituent {++i}";
            Defaults(new {
                Description = descFn
            });
        }
    }

    #endregion

    #region WaterConstituentStateLimit

    public class WaterConstituentStateLimitFactory : TestDataFactory<WaterConstituentStateLimit>
    {
        public WaterConstituentStateLimitFactory(IContainer container) : base(container) { }

        static WaterConstituentStateLimitFactory()
        {
            Defaults(new {
                WaterConstituent = typeof(WaterConstituentFactory)
            });
        }
    }

    #endregion

    #region WaterSample

    public class WaterSampleFactory : TestDataFactory<WaterSample>
    {
        public WaterSampleFactory(IContainer container) : base(container) { }

        static WaterSampleFactory()
        {
            Defaults(new {
                AnalysisPerformedBy = "Jeanne Poisson"
            });
        }
    }

    #endregion

    #region WaterSampleComplianceForm

    public class WaterSampleComplianceFormFactory : TestDataFactory<WaterSampleComplianceForm>
    {
        public WaterSampleComplianceFormFactory(IContainer container) : base(container) { }

        static WaterSampleComplianceFormFactory()
        {
            Defaults(new {
                PublicWaterSupply = typeof(PublicWaterSupplyFactory),
                CertifiedBy = typeof(UserFactory),
                CertifiedMonth = 1,
                CertifiedYear = 2018,
                DateCertified = new DateTime(2018, 1, 15)
            });
        }

        public override WaterSampleComplianceForm Build(object overrides = null)
        {
            var built = base.Build(overrides);

            built.PublicWaterSupply.WaterSampleComplianceForms.Add(built);

            return built;
        }
    }

    #endregion

    #region WaterSampleComplianceFormAnswerType

    public class WaterSampleComplianceFormAnswerTypeFactory : StaticListEntityLookupFactory<
        WaterSampleComplianceFormAnswerType, WaterSampleComplianceFormAnswerTypeFactory>
    {
        static WaterSampleComplianceFormAnswerTypeFactory() { }

        public WaterSampleComplianceFormAnswerTypeFactory(IContainer container) : base(container) { }
    }

    public class NoWaterSampleComplianceFormAnswerTypeFactory : WaterSampleComplianceFormAnswerTypeFactory
    {
        public NoWaterSampleComplianceFormAnswerTypeFactory(IContainer container) : base(container) { }

        static NoWaterSampleComplianceFormAnswerTypeFactory()
        {
            Defaults(new {Description = "No"});
            OnSaving((a, s) => a.Id = (int)WaterSampleComplianceFormAnswerType.Indices.NO);
        }
    }

    public class NotAvailableWaterSampleComplianceFormAnswerTypeFactory : WaterSampleComplianceFormAnswerTypeFactory
    {
        public NotAvailableWaterSampleComplianceFormAnswerTypeFactory(IContainer container) : base(container) { }

        static NotAvailableWaterSampleComplianceFormAnswerTypeFactory()
        {
            Defaults(new {Description = "N/A"});
            OnSaving((a, s) => a.Id = (int)WaterSampleComplianceFormAnswerType.Indices.NOT_AVAILABLE);
        }
    }

    public class YesWaterSampleComplianceFormAnswerTypeFactory : WaterSampleComplianceFormAnswerTypeFactory
    {
        public YesWaterSampleComplianceFormAnswerTypeFactory(IContainer container) : base(container) { }

        static YesWaterSampleComplianceFormAnswerTypeFactory()
        {
            Defaults(new {Description = "Yes"});
            OnSaving((a, s) => a.Id = (int)WaterSampleComplianceFormAnswerType.Indices.YES);
        }
    }

    #endregion

    #region Water System

    public class WaterSystemFactory : TestDataFactory<WaterSystem>
    {
        #region Constants

        public const string DEFAULT_WATER_SYSTEM_DESCRIPTION = "HAB-1";

        #endregion

        #region Constructors

        static WaterSystemFactory()
        {
            Defaults(new {
                Description = DEFAULT_WATER_SYSTEM_DESCRIPTION
            });
        }

        public WaterSystemFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WaterType

    public class WaterTypeFactory : UniqueEntityLookupFactory<WaterType>
    {
        public WaterTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region WaterQualityComplaintSampleResult

    public class WaterQualityComplaintSampleResultFactory : TestDataFactory<WaterQualityComplaintSampleResult>
    {
        public WaterQualityComplaintSampleResultFactory(IContainer container) : base(container) { }

        static WaterQualityComplaintSampleResultFactory()
        {
            var i = 0;
            Func<string> valueFn = () => $"Result {++i}";
            Func<string> analysisFunc = () => $"Performed By {i}";
            Defaults(new {
                SampleValue = valueFn,
                AnalysisPerformedBy = analysisFunc,
                Complaint = typeof(TestDataFactory<WaterQualityComplaint>),
                SampleDate = Lambdas.GetNowDate
            });
        }
    }

    #endregion

    #region WeatherConditionFactory

    public class WeatherConditionFactory : UniqueEntityLookupFactory<WeatherCondition>
    {
        public WeatherConditionFactory(IContainer container) : base(container) { }
    }

    public class WeatherConditionDryFactory : WeatherConditionFactory
    {
        static WeatherConditionDryFactory()
        {
            Defaults(new {
                Description = WeatherCondition.DRY
            });
            OnSaving((a, s) => a.Id = WeatherCondition.Indices.DRY);
        }

        public WeatherConditionDryFactory(IContainer container) : base(container) { }
    }

    public class WeatherConditionRainingFactory : WeatherConditionFactory
    {
        static WeatherConditionRainingFactory()
        {
            Defaults(new {
                Description = WeatherCondition.RAINING
            });
            OnSaving((a, s) => a.Id = WeatherCondition.Indices.RAINING);
        }

        public WeatherConditionRainingFactory(IContainer container) : base(container) { }
    }

    public class WeatherConditionSnowingFactory : WeatherConditionFactory
    {
        static WeatherConditionSnowingFactory()
        {
            Defaults(new {
                Description = WeatherCondition.SNOWING
            });
            OnSaving((a, s) => a.Id = WeatherCondition.Indices.SNOWING);
        }

        public WeatherConditionSnowingFactory(IContainer container) : base(container) { }
    }

    #endregion
    
    #region WellTest

    public class WellTestFactory : TestDataFactory<WellTest>
    {
        public WellTestFactory(IContainer container) : base(container) { }

        static WellTestFactory()
        {
            Defaults(new {
                ProductionWorkOrder = typeof(ProductionWorkOrderFactory),
                Equipment = typeof(EquipmentFactory),
                Employee = typeof(EmployeeFactory),
                GradeType = typeof(WellTestGradeTypeFactory),
                DateOfTest = Lambdas.GetNow,
                PumpingRate = 1000,
                MeasurementPoint = 12.20M,
                StaticWaterLevel = 213.23M,
                PumpingWaterLevel = 391.23M
            });
        }
    }

    #endregion

    #region WellTestGradeType

    public class WellTestGradeTypeFactory : UniqueEntityLookupFactory<WellTestGradeType>
    {
        public WellTestGradeTypeFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region WBSNumber

    public class WBSNumberFactory : TestDataFactory<WBSNumber>
    {
        public WBSNumberFactory(IContainer container) : base(container) { }

        static WBSNumberFactory()
        {
            var i = 0;
            Func<string> descFn = () => $"1-{++i}";
            Defaults(new {
                Description = descFn
            });
        }
    }

    #endregion

    #region WorkDescription

    public class WorkDescriptionFactory : StaticListEntityLookupFactory<WorkDescription, WorkDescriptionFactory>
    {
        static WorkDescriptionFactory()
        {
            Defaults(new {
                PlantMaintenanceActivityType = typeof(PlantMaintenanceActivityTypeFactory),
                IsActive = true,
                FirstRestorationAccountingCode = typeof(RestorationAccountingCodeFactory),
                FirstRestorationProductCode = typeof(EntityLookupTestDataFactory<RestorationProductCode>),
            });
        }

        public WorkDescriptionFactory(IContainer container) : base(container) { }
    }

    public class WaterMainBleedersWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainBleedersWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainBleedersWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN BLEEDERS", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_BLEEDERS);
        }
    }

    public class ChangeBurstMeterWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ChangeBurstMeterWorkDescriptionFactory(IContainer container) : base(container) { }

        static ChangeBurstMeterWorkDescriptionFactory()
        {
            Defaults(new {Description = "CHANGE BURST METER", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CHANGE_BURST_METER);
        }
    }

    public class CheckNoWaterWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CheckNoWaterWorkDescriptionFactory(IContainer container) : base(container) { }

        static CheckNoWaterWorkDescriptionFactory()
        {
            Defaults(new {Description = "CHECK NO WATER", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CHECK_NO_WATER);
        }
    }

    public class CurbBoxRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CurbBoxRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static CurbBoxRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "CURB BOX REPAIR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CURB_BOX_REPAIR);
        }
    }

    public class BallCurbStopRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public BallCurbStopRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static BallCurbStopRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "BALL/CURB STOP REPAIR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.BALL_CURB_STOP_REPAIR);
        }
    }

    public class ExcavateMeterBoxSetterWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ExcavateMeterBoxSetterWorkDescriptionFactory(IContainer container) : base(container) { }

        static ExcavateMeterBoxSetterWorkDescriptionFactory()
        {
            Defaults(new {Description = "EXCAVATE METER BOX/SETTER", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.EXCAVATE_METER_BOX_SETTER);
        }
    }

    public class ServiceLineFlowTestWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineFlowTestWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineFlowTestWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE FLOW TEST", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_FLOW_TEST);
        }
    }

    public class HydrantFrozenWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantFrozenWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantFrozenWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT FROZEN", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_FROZEN);
        }
    }

    public class FrozenMeterSetWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FrozenMeterSetWorkDescriptionFactory(IContainer container) : base(container) { }

        static FrozenMeterSetWorkDescriptionFactory()
        {
            Defaults(new {Description = "FROZEN METER SET", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FROZEN_METER_SET);
        }
    }

    public class FrozenServiceLineCompanySideWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FrozenServiceLineCompanySideWorkDescriptionFactory(IContainer container) : base(container) { }

        static FrozenServiceLineCompanySideWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "FROZEN SERVICE LINE COMPANY SIDE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FROZEN_SERVICE_LINE_COMPANY_SIDE);
        }
    }

    public class FrozenServiceLineCustSideWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FrozenServiceLineCustSideWorkDescriptionFactory(IContainer container) : base(container) { }

        static FrozenServiceLineCustSideWorkDescriptionFactory()
        {
            Defaults(new {Description = "FROZEN SERVICE LINE CUST. SIDE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FROZEN_SERVICE_LINE_CUST_SIDE);
        }
    }

    public class GroundWaterServiceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public GroundWaterServiceWorkDescriptionFactory(IContainer container) : base(container) { }

        static GroundWaterServiceWorkDescriptionFactory()
        {
            Defaults(new {Description = "GROUND WATER-SERVICE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.GROUND_WATER_SERVICE);
        }
    }

    public class HydrantFlushingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantFlushingWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantFlushingWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT FLUSHING", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_FLUSHING);
        }
    }

    public class HydrantInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantInvestigationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "HYDRANT INVESTIGATION", AssetType = typeof(HydrantAssetTypeFactory), TimeToComplete = 0m
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_INVESTIGATION);
        }
    }

    public class HydrantInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantInstallationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "HYDRANT INSTALLATION", AssetType = typeof(HydrantAssetTypeFactory), TimeToComplete = 3.5m
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_INSTALLATION);
        }
    }

    public class HydrantLeakingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantLeakingWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantLeakingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "HYDRANT LEAKING", AssetType = typeof(HydrantAssetTypeFactory), TimeToComplete = 6m
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_LEAKING);
        }
    }

    public class HydrantNoDripWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantNoDripWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantNoDripWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT NO DRIP", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_NO_DRIP);
        }
    }

    public class HydrantRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT REPAIR", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_REPAIR);
        }
    }

    public class HydrantReplacementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantReplacementWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantReplacementWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT REPLACEMENT", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_REPLACEMENT);
        }
    }

    public class HydrantRetirementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantRetirementWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantRetirementWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT RETIREMENT", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_RETIREMENT);
        }
    }

    public class InactiveAccountWorkDescriptionFactory : WorkDescriptionFactory
    {
        public InactiveAccountWorkDescriptionFactory(IContainer container) : base(container) { }

        static InactiveAccountWorkDescriptionFactory()
        {
            Defaults(new {Description = "INACTIVE ACCOUNT", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.INACTIVE_ACCOUNT);
        }
    }

    public class ValveBlowOffInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBlowOffInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBlowOffInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BLOW OFF INSTALLATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BLOW_OFF_INSTALLATION);
        }
    }

    public class FireServiceInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FireServiceInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static FireServiceInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "FIRE SERVICE INSTALLATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FIRE_SERVICE_INSTALLATION);
        }
    }

    public class InstallLineStopperWorkDescriptionFactory : WorkDescriptionFactory
    {
        public InstallLineStopperWorkDescriptionFactory(IContainer container) : base(container) { }

        static InstallLineStopperWorkDescriptionFactory()
        {
            Defaults(new {Description = "INSTALL LINE STOPPER", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.INSTALL_LINE_STOPPER);
        }
    }

    public class InstallMeterWorkDescriptionFactory : WorkDescriptionFactory
    {
        public InstallMeterWorkDescriptionFactory(IContainer container) : base(container) { }

        static InstallMeterWorkDescriptionFactory()
        {
            Defaults(new {Description = "INSTALL METER", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.INSTALL_METER);
        }
    }

    public class InteriorSettingRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public InteriorSettingRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static InteriorSettingRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "INTERIOR SETTING REPAIR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.INTERIOR_SETTING_REPAIR);
        }
    }

    public class ServiceInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE INVESTIGATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_INVESTIGATION);
        }
    }

    public class MainInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "MAIN INVESTIGATION", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_INVESTIGATION);
        }
    }

    public class LeakInMeterBoxInletWorkDescriptionFactory : WorkDescriptionFactory
    {
        public LeakInMeterBoxInletWorkDescriptionFactory(IContainer container) : base(container) { }

        static LeakInMeterBoxInletWorkDescriptionFactory()
        {
            Defaults(new {Description = "LEAK IN METER BOX, INLET", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.LEAK_IN_METER_BOX_INLET);
        }
    }

    public class LeakInMeterBoxOutletWorkDescriptionFactory : WorkDescriptionFactory
    {
        public LeakInMeterBoxOutletWorkDescriptionFactory(IContainer container) : base(container) { }

        static LeakInMeterBoxOutletWorkDescriptionFactory()
        {
            Defaults(new {Description = "LEAK IN METER BOX, OUTLET", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.LEAK_IN_METER_BOX_OUTLET);
        }
    }

    public class LeakSurveyWorkDescriptionFactory : WorkDescriptionFactory
    {
        public LeakSurveyWorkDescriptionFactory(IContainer container) : base(container) { }

        static LeakSurveyWorkDescriptionFactory()
        {
            Defaults(new {Description = "LEAK SURVEY", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.LEAK_SURVEY);
        }
    }

    public class MainBreakRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        #region Constants

        public const string DEFAULT_DESCRIPTION = "Water Main Break Repair";

        #endregion

        #region Constructors

        static MainBreakRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = DEFAULT_DESCRIPTION, AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
        }

        public MainBreakRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MeterBoxSetterInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterBoxSetterInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterBoxSetterInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER BOX/SETTER INSTALLATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_BOX_SETTER_INSTALLATION);
        }
    }

    public class MeterChangeWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterChangeWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterChangeWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER CHANGE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_CHANGE);
        }
    }

    public class MeterBoxAdjustmentResetterWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterBoxAdjustmentResetterWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterBoxAdjustmentResetterWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER BOX ADJUSTMENT/RESETTER", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_BOX_ADJUSTMENT_RESETTER);
        }
    }

    public class NewMainFlushingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public NewMainFlushingWorkDescriptionFactory(IContainer container) : base(container) { }

        static NewMainFlushingWorkDescriptionFactory()
        {
            Defaults(new {Description = "NEW MAIN FLUSHING", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.NEW_MAIN_FLUSHING);
        }
    }

    public class ServiceLineInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE INSTALLATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION);
        }
    }

    public class ServiceLineLeakCustSideWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineLeakCustSideWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineLeakCustSideWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE LEAK, CUST. SIDE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_LEAK_CUST_SIDE);
        }
    }

    public class ServiceLineRenewalWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRenewalWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRenewalWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SERVICE LINE RENEWAL",
                AssetType = typeof(ServiceAssetTypeFactory),
                // this is needed by WorkOrderPage.feature Scenario
                // "Digital as-built requirement is driven by work description on create"
                // if this needs to change, please update that test as well 
                DigitalAsBuiltRequired = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL);
        }
    }

    public class ServiceLineRetireWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRetireWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRetireWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE RETIRE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RETIRE);
        }
    }

    public class SumpPumpWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SumpPumpWorkDescriptionFactory(IContainer container) : base(container) { }

        static SumpPumpWorkDescriptionFactory()
        {
            Defaults(new {Description = "SUMP PUMP", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SUMP_PUMP);
        }
    }

    public class TestShutDownWorkDescriptionFactory : WorkDescriptionFactory
    {
        public TestShutDownWorkDescriptionFactory(IContainer container) : base(container) { }

        static TestShutDownWorkDescriptionFactory()
        {
            Defaults(new {Description = "TEST SHUT DOWN", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.TEST_SHUT_DOWN);
        }
    }

    public class ValveBoxRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBoxRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBoxRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BOX REPAIR", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BOX_REPAIR);
        }
    }

    public class ValveBoxBlowOffRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBoxBlowOffRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBoxBlowOffRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BOX BLOW OFF REPAIR", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BOX_BLOW_OFF_REPAIR);
        }
    }

    public class ServiceLineValveBoxRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineValveBoxRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineValveBoxRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE/VALVE BOX REPAIR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_VALVE_BOX_REPAIR);
        }
    }

    public class ValveInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE INVESTIGATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_INVESTIGATION);
        }
    }

    public class ValveLeakingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveLeakingWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveLeakingWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE LEAKING", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_LEAKING);
        }
    }

    public class ValveRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE REPAIR", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_REPAIR);
        }
    }

    public class ValveBlowOffRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBlowOffRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBlowOffRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BLOW OFF REPAIR", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BLOW_OFF_REPAIR);
        }
    }

    public class ValveReplacementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveReplacementWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveReplacementWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE REPLACEMENT", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_REPLACEMENT);
        }
    }

    public class ValveRetirementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveRetirementWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveRetirementWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE RETIREMENT", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_RETIREMENT);
        }
    }

    public class WaterBanRestrictionViolatorWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterBanRestrictionViolatorWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterBanRestrictionViolatorWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER BAN/RESTRICTION VIOLATOR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_BAN_RESTRICTION_VIOLATOR);
        }
    }

    public class WaterMainBreakRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainBreakRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainBreakRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN BREAK REPAIR", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR);
        }
    }

    public class WaterMainInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN INSTALLATION", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_INSTALLATION);
        }
    }

    public class WaterMainRetirementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainRetirementWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainRetirementWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN RETIREMENT", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_RETIREMENT);
        }
    }

    public class FlushingServiceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FlushingServiceWorkDescriptionFactory(IContainer container) : base(container) { }

        static FlushingServiceWorkDescriptionFactory()
        {
            Defaults(new {Description = "FLUSHING-SERVICE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FLUSHING_SERVICE);
        }
    }

    public class WaterMainBreakReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainBreakReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainBreakReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN BREAK REPLACE", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE);
        }
    }

    public class MeterBoxSetterReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterBoxSetterReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterBoxSetterReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER BOX/SETTER REPLACE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_BOX_SETTER_REPLACE);
        }
    }

    public class SewerMainBreakRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainBreakRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainBreakRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN BREAK-REPAIR", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_BREAK_REPAIR);
        }
    }

    public class SewerMainBreakReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainBreakReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainBreakReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN BREAK-REPLACE", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_BREAK_REPLACE);
        }
    }

    public class SewerMainRetirementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainRetirementWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainRetirementWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN RETIREMENT", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_RETIREMENT);
        }
    }

    public class SewerMainInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN INSTALLATION", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_INSTALLATION);
        }
    }

    public class SewerMainCleaningWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainCleaningWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainCleaningWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN CLEANING", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_CLEANING);
        }
    }

    public class SewerLateralInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralInstallationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER LATERAL-INSTALLATION",
                AssetType = typeof(SewerLateralAssetTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_INSTALLATION);
        }
    }

    public class SewerLateralRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER LATERAL-REPAIR", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_REPAIR);
        }
    }

    public class SewerLateralReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER LATERAL-REPLACE", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_REPLACE);
        }
    }

    public class SewerLateralRetireWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralRetireWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralRetireWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER LATERAL-RETIRE", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_RETIRE);
        }
    }

    public class SewerLateralCustomerSideWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralCustomerSideWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralCustomerSideWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SEWER LATERAL-CUSTOMER SIDE", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_CUSTOMER_SIDE);
        }
    }

    public class SewerOpeningRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER OPENING REPAIR", AssetType = typeof(SewerOpeningAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_REPAIR);
        }
    }

    public class SewerOpeningReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER OPENING REPLACE", AssetType = typeof(SewerOpeningAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_REPLACE);
        }
    }

    public class SewerOpeningInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningInstallationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER OPENING INSTALLATION",
                AssetType = typeof(SewerOpeningAssetTypeFactory)
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_INSTALLATION);
        }
    }

    public class SewerMainOverflowWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainOverflowWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainOverflowWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN OVERFLOW", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW);
        }
    }

    public class SewerBackupCompanySideWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerBackupCompanySideWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerBackupCompanySideWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER BACKUP-COMPANY SIDE", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_BACKUP_COMPANY_SIDE);
        }
    }

    public class HydraulicFlowTestWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydraulicFlowTestWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydraulicFlowTestWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRAULIC FLOW TEST", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRAULIC_FLOW_TEST);
        }
    }

    public class MarkoutCrewWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MarkoutCrewWorkDescriptionFactory(IContainer container) : base(container) { }

        static MarkoutCrewWorkDescriptionFactory()
        {
            Defaults(new {Description = "MARKOUT-CREW", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MARKOUT_CREW);
        }
    }

    public class ValveBoxReplacementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBoxReplacementWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBoxReplacementWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "VALVE BOX REPLACEMENT",
                AssetType = typeof(ValveAssetTypeFactory),
                // this is needed by WorkOrderPage.feature Scenario
                // "Digital as-built requirement is driven by work description on update"
                // if this needs to change, please update that test as well
                DigitalAsBuiltRequired = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BOX_REPLACEMENT);
        }
    }

    public class SiteInspectionSurveyNewServiceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SiteInspectionSurveyNewServiceWorkDescriptionFactory(IContainer container) : base(container) { }

        static SiteInspectionSurveyNewServiceWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SITE INSPECTION/SURVEY NEW SERVICE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SITE_INSPECTION_SURVEY_NEW_SERVICE);
        }
    }

    public class SiteInspectionSurveyServiceRenewalWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SiteInspectionSurveyServiceRenewalWorkDescriptionFactory(IContainer container) : base(container) { }

        static SiteInspectionSurveyServiceRenewalWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SITE INSPECTION/SURVEY SERVICE RENEWAL",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SITE_INSPECTION_SURVEY_SERVICE_RENEWAL);
        }
    }

    public class ServiceLineRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE REPAIR", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_REPAIR);
        }
    }

    public class SewerCleanOutInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerCleanOutInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerCleanOutInstallationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SEWER CLEAN OUT INSTALLATION", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_CLEAN_OUT_INSTALLATION);
        }
    }

    public class SewerCleanOutRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerCleanOutRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerCleanOutRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER CLEAN OUT REPAIR", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_CLEAN_OUT_REPAIR);
        }
    }

    public class SewerCameraServiceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerCameraServiceWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerCameraServiceWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER CAMERA SERVICE", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_CAMERA_SERVICE);
        }
    }

    public class SewerCameraMainWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerCameraMainWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerCameraMainWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER CAMERA MAIN", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_CAMERA_MAIN);
        }
    }

    public class SewerDemolitionInspectionWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerDemolitionInspectionWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerDemolitionInspectionWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SEWER DEMOLITION INSPECTION", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_DEMOLITION_INSPECTION);
        }
    }

    public class SewerMainTestHolesWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainTestHolesWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainTestHolesWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER MAIN TEST HOLES", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_TEST_HOLES);
        }
    }

    public class WaterMainTestHolesWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainTestHolesWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainTestHolesWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN TEST HOLES", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_TEST_HOLES);
        }
    }

    public class ValveBrokenWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBrokenWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBrokenWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BROKEN", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BROKEN);
        }
    }

    public class GroundWaterMainWorkDescriptionFactory : WorkDescriptionFactory
    {
        public GroundWaterMainWorkDescriptionFactory(IContainer container) : base(container) { }

        static GroundWaterMainWorkDescriptionFactory()
        {
            Defaults(new {Description = "GROUND WATER-MAIN", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.GROUND_WATER_MAIN);
        }
    }

    public class ServiceTurnonWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceTurnonWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceTurnonWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE-TURN ON", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_TURN_ON);
        }
    }

    public class ServiceTurnOffWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceTurnOffWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceTurnOffWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE-TURN OFF", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_TURN_OFF);
        }
    }

    public class MeterObtainReadWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterObtainReadWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterObtainReadWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER-OBTAIN READ", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_OBTAIN_READ);
        }
    }

    public class MeterFinalStartReadWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterFinalStartReadWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterFinalStartReadWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER-FINAL/START READ", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_FINAL_START_READ);
        }
    }

    public class MeterRepairTouchPadWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterRepairTouchPadWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterRepairTouchPadWorkDescriptionFactory()
        {
            Defaults(new {Description = "METER-REPAIR TOUCH PAD", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_REPAIR_TOUCH_PAD);
        }
    }

    public class ValveInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE INSTALLATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_INSTALLATION);
        }
    }

    public class ValveBlowOffReplacementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBlowOffReplacementWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBlowOffReplacementWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BLOW OFF REPLACEMENT", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BLOW_OFF_REPLACEMENT);
        }
    }

    public class HydrantPaintWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantPaintWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantPaintWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT PAINT", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_PAINT);
        }
    }

    public class BallCurbStopReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public BallCurbStopReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static BallCurbStopReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "BALL/CURB STOP REPLACE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.BALL_CURB_STOP_REPLACE);
        }
    }

    public class ValveBlowOffRetirementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBlowOffRetirementWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBlowOffRetirementWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BLOW OFF RETIREMENT", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BLOW_OFF_RETIREMENT);
        }
    }

    public class ValveBlowOffBrokenWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveBlowOffBrokenWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveBlowOffBrokenWorkDescriptionFactory()
        {
            Defaults(new {Description = "VALVE BLOW OFF BROKEN", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_BLOW_OFF_BROKEN);
        }
    }

    public class WaterMainRelocationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainRelocationWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainRelocationWorkDescriptionFactory()
        {
            Defaults(new {Description = "WATER MAIN RELOCATION", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_RELOCATION);
        }
    }

    public class HydrantRelocationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantRelocationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantRelocationWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT RELOCATION", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_RELOCATION);
        }
    }

    public class ServiceRelocationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceRelocationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceRelocationWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE RELOCATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_RELOCATION);
        }
    }

    public class SewerInvestigationMainWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerInvestigationMainWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerInvestigationMainWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER INVESTIGATION-MAIN", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_INVESTIGATION_MAIN);
        }
    }

    public class SewerServiceOverflowWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerServiceOverflowWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerServiceOverflowWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER SERVICE OVERFLOW", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_SERVICE_OVERFLOW);
        }
    }

    public class SewerInvestigationLateralWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerInvestigationLateralWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerInvestigationLateralWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SEWER INVESTIGATION-LATERAL", AssetType = typeof(SewerLateralAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_INVESTIGATION_LATERAL);
        }
    }

    public class SewerInvestigationOpeningWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerInvestigationOpeningWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerInvestigationOpeningWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "SEWER INVESTIGATION-OPENING", AssetType = typeof(SewerOpeningAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_INVESTIGATION_OPENING);
        }
    }

    public class SewerLiftStationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLiftStationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLiftStationRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "SEWER LIFT STATION REPAIR", AssetType = typeof(SewerMainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LIFT_STATION_REPAIR);
        }
    }

    public class CurbBoxReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CurbBoxReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static CurbBoxReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "CURB BOX REPLACE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CURB_BOX_REPLACE);
        }
    }

    public class ServiceLineValveBoxReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineValveBoxReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineValveBoxReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "SERVICE LINE/VALVE BOX REPLACE", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_VALVE_BOX_REPLACE);
        }
    }

    public class StormCatchRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "STORM/CATCH REPAIR", AssetType = typeof(StormCatchAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_REPAIR);
        }
    }

    public class StormCatchReplaceWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchReplaceWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchReplaceWorkDescriptionFactory()
        {
            Defaults(new {Description = "STORM/CATCH REPLACE", AssetType = typeof(StormCatchAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_REPLACE);
        }
    }

    public class StormCatchInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchInstallationWorkDescriptionFactory()
        {
            Defaults(new {Description = "STORM/CATCH INSTALLATION", AssetType = typeof(StormCatchAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_INSTALLATION);
        }
    }

    public class StormCatchInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "STORM/CATCH INVESTIGATION", AssetType = typeof(StormCatchAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_INVESTIGATION);
        }
    }

    public class HydrantLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "HYDRANT LANDSCAPING", AssetType = typeof(HydrantAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_LANDSCAPING);
        }
    }

    public class HydrantRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "HYDRANT RESTORATION INVESTIGATION", AssetType = typeof(HydrantAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_RESTORATION_INVESTIGATION);
        }
    }

    public class HydrantRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantRestorationRepairWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "HYDRANT RESTORATION REPAIR", AssetType = typeof(HydrantAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_RESTORATION_REPAIR);
        }
    }

    public class MainLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainLandscapingWorkDescriptionFactory()
        {
            Defaults(new {Description = "MAIN LANDSCAPING", AssetType = typeof(MainAssetTypeFactory), Revisit = true});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_LANDSCAPING);
        }
    }

    public class MainRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "MAIN RESTORATION INVESTIGATION", AssetType = typeof(MainAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_RESTORATION_INVESTIGATION);
        }
    }

    public class MainRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainRestorationRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "MAIN RESTORATION REPAIR", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_RESTORATION_REPAIR);
        }
    }

    public class ServiceLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SERVICE LANDSCAPING", AssetType = typeof(ServiceAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LANDSCAPING);
        }
    }

    public class ServiceRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE RESTORATION INVESTIGATION", AssetType = typeof(ServiceAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_RESTORATION_INVESTIGATION);
        }
    }

    public class ServiceRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceRestorationRepairWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SERVICE RESTORATION REPAIR", AssetType = typeof(ServiceAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_RESTORATION_REPAIR);
        }
    }

    public class SewerLateralLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER LATERAL LANDSCAPING", AssetType = typeof(SewerLateralAssetTypeFactory),
                Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_LANDSCAPING);
        }
    }

    public class SewerLateralRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SEWER LATERAL RESTORATION INVESTIGATION",
                    AssetType = typeof(SewerLateralAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_RESTORATION_INVESTIGATION);
        }
    }

    public class SewerLateralRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerLateralRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerLateralRestorationRepairWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SEWER LATERAL RESTORATION REPAIR",
                    AssetType = typeof(SewerLateralAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_LATERAL_RESTORATION_REPAIR);
        }
    }

    public class SewerMainLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER MAIN LANDSCAPING", AssetType = typeof(SewerMainAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_LANDSCAPING);
        }
    }

    public class SewerMainRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SEWER MAIN RESTORATION INVESTIGATION",
                    AssetType = typeof(SewerMainAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_RESTORATION_INVESTIGATION);
        }
    }

    public class SewerMainRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerMainRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerMainRestorationRepairWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER MAIN RESTORATION REPAIR",
                AssetType = typeof(SewerMainAssetTypeFactory),
                Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_MAIN_RESTORATION_REPAIR);
        }
    }

    public class SewerOpeningLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "SEWER OPENING LANDSCAPING", AssetType = typeof(SewerOpeningAssetTypeFactory),
                Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_LANDSCAPING);
        }
    }

    public class SewerOpeningRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SEWER OPENING RESTORATION INVESTIGATION",
                    AssetType = typeof(SewerOpeningAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_RESTORATION_INVESTIGATION);
        }
    }

    public class SewerOpeningRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SewerOpeningRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SewerOpeningRestorationRepairWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SEWER OPENING RESTORATION REPAIR",
                    AssetType = typeof(SewerOpeningAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SEWER_OPENING_RESTORATION_REPAIR);
        }
    }

    public class ValveLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "VALVE LANDSCAPING", AssetType = typeof(ValveAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_LANDSCAPING);
        }
    }

    public class ValveRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "VALVE RESTORATION INVESTIGATION", AssetType = typeof(ValveAssetTypeFactory),
                Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_RESTORATION_INVESTIGATION);
        }
    }

    public class ValveRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveRestorationRepairWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "VALVE RESTORATION REPAIR", AssetType = typeof(ValveAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_RESTORATION_REPAIR);
        }
    }

    public class StormCatchLandscapingWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchLandscapingWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchLandscapingWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "STORM/CATCH LANDSCAPING", AssetType = typeof(StormCatchAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_LANDSCAPING);
        }
    }

    public class StormCatchRestorationInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchRestorationInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchRestorationInvestigationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "STORM/CATCH RESTORATION INVESTIGATION",
                    AssetType = typeof(StormCatchAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_RESTORATION_INVESTIGATION);
        }
    }

    public class StormCatchRestorationRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public StormCatchRestorationRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static StormCatchRestorationRepairWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "STORM/CATCH RESTORATION REPAIR", AssetType = typeof(StormCatchAssetTypeFactory),
                    Revisit = true
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.STORM_CATCH_RESTORATION_REPAIR);
        }
    }

    public class RstrnRestorationInquiryWorkDescriptionFactory : WorkDescriptionFactory
    {
        public RstrnRestorationInquiryWorkDescriptionFactory(IContainer container) : base(container) { }

        static RstrnRestorationInquiryWorkDescriptionFactory()
        {
            Defaults(new {
                Description = "RSTRN-RESTORATION INQUIRY", AssetType = typeof(MainAssetTypeFactory), Revisit = true
            });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.RSTRN_RESTORATION_INQUIRY);
        }
    }

    public class ServiceOffAtMainStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceOffAtMainStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceOffAtMainStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE OFF AT MAIN-STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_OFF_AT_MAIN_STORM_RESTORATION);
        }
    }

    public class ServiceOffAtCurbStopStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceOffAtCurbStopStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceOffAtCurbStopStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE OFF AT CURB STOP-STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_OFF_AT_CURB_STOP_STORM_RESTORATION);
        }
    }

    public class ServiceOffAtMeterPitStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceOffAtMeterPitStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceOffAtMeterPitStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE OFF AT METER PIT-STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_OFF_AT_METER_PIT_STORM_RESTORATION);
        }
    }

    public class ValveTurnedOffStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveTurnedOffStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveTurnedOffStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "VALVE TURNED OFF STORM RESTORATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_TURNED_OFF_STORM_RESTORATION);
        }
    }

    public class MainRepairStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainRepairStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainRepairStormRestorationWorkDescriptionFactory()
        {
            Defaults(new {Description = "MAIN REPAIR-STORM RESTORATION", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_REPAIR_STORM_RESTORATION);
        }
    }

    public class MainReplaceStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MainReplaceStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MainReplaceStormRestorationWorkDescriptionFactory()
        {
            Defaults(new {Description = "MAIN REPLACE - STORM RESTORATION", AssetType = typeof(MainAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MAIN_REPLACE_STORM_RESTORATION);
        }
    }

    public class HydrantTurnedOffStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantTurnedOffStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantTurnedOffStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "HYDRANT TURNED OFF - STORM RESTORATION",
                    AssetType = typeof(HydrantAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_TURNED_OFF_STORM_RESTORATION);
        }
    }

    public class HydrantReplaceStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantReplaceStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantReplaceStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "HYDRANT REPLACE - STORM RESTORATION", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_REPLACE_STORM_RESTORATION);
        }
    }

    public class ValveInstallationStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveInstallationStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveInstallationStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "VALVE INSTALLATION - STORM RESTORATION",
                    AssetType = typeof(ValveAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_INSTALLATION_STORM_RESTORATION);
        }
    }

    public class ValveReplacementStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveReplacementStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveReplacementStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "VALVE REPLACEMENT - STORM RESTORATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_REPLACEMENT_STORM_RESTORATION);
        }
    }

    public class CurbBoxLocateStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CurbBoxLocateStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static CurbBoxLocateStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "CURB BOX LOCATE - STORM RESTORATION", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CURB_BOX_LOCATE_STORM_RESTORATION);
        }
    }

    public class MeterPitLocateStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MeterPitLocateStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static MeterPitLocateStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "METER PIT LOCATE - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.METER_PIT_LOCATE_STORM_RESTORATION);
        }
    }

    public class ValveRetirementStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ValveRetirementStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ValveRetirementStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {Description = "VALVE RETIREMENT - STORM RESTORATION", AssetType = typeof(ValveAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VALVE_RETIREMENT_STORM_RESTORATION);
        }
    }

    public class ExcavateMeterPitStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ExcavateMeterPitStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ExcavateMeterPitStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "EXCAVATE METER PIT- STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.EXCAVATE_METER_PIT__STORM_RESTORATION);
        }
    }

    public class ServiceLineRenewalStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRenewalStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRenewalStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE LINE RENEWAL - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_STORM_RESTORATION);
        }
    }

    public class CurbBoxReplacementStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CurbBoxReplacementStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static CurbBoxReplacementStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "CURB BOX REPLACEMENT - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CURB_BOX_REPLACEMENT_STORM_RESTORATION);
        }
    }

    public class WaterMainRetirementStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public WaterMainRetirementStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static WaterMainRetirementStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "WATER MAIN RETIREMENT - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.WATER_MAIN_RETIREMENT_STORM_RESTORATION);
        }
    }

    public class ServiceLineRetirementStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRetirementStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRetirementStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "SERVICE LINE RETIREMENT - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RETIREMENT_STORM_RESTORATION);
        }
    }

    public class FrameAndCoverReplaceStormRestorationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public FrameAndCoverReplaceStormRestorationWorkDescriptionFactory(IContainer container) : base(container) { }

        static FrameAndCoverReplaceStormRestorationWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "FRAME AND COVER REPLACE - STORM RESTORATION",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.FRAME_AND_COVER_REPLACE_STORM_RESTORATION);
        }
    }

    public class PumpRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public PumpRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static PumpRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "Pump Repair", AssetType = typeof(EquipmentAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.PUMP_REPAIR);
        }
    }

    public class LineStopRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public LineStopRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static LineStopRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "Line Stop Repair", AssetType = typeof(EquipmentAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.LINE_STOP_REPAIR);
        }
    }

    public class SawRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public SawRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static SawRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "Saw Repair", AssetType = typeof(EquipmentAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SAW_REPAIR);
        }
    }

    public class VehicleRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public VehicleRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static VehicleRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "Vehicle Repair", AssetType = typeof(EquipmentAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.VEHICLE_REPAIR);
        }
    }

    public class MiscRepairWorkDescriptionFactory : WorkDescriptionFactory
    {
        public MiscRepairWorkDescriptionFactory(IContainer container) : base(container) { }

        static MiscRepairWorkDescriptionFactory()
        {
            Defaults(new {Description = "Misc repair", AssetType = typeof(EquipmentAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.MISC_REPAIR);
        }
    }

    public class ZLwcEw43ConsecutiveMthsOf0UsageZeroWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw43ConsecutiveMthsOf0UsageZeroWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw43ConsecutiveMthsOf0UsageZeroWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - 3 CONSECUTIVE MTHS OF 0 USAGE [ZERO]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_3_CONSECUTIVE_MTHS_OF_0_USAGE_ZERO);
        }
    }

    public class ZLwcEw4CheckMeterNonEmergencyCkmtrWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4CheckMeterNonEmergencyCkmtrWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4CheckMeterNonEmergencyCkmtrWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - CHECK METER NON-EMERGENCY [CKMTR]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_CHECK_METER_NON_EMERGENCY_CKMTR);
        }
    }

    public class ZLwcEw4DemolitionClosedAccountDemocWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4DemolitionClosedAccountDemocWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4DemolitionClosedAccountDemocWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - DEMOLITION CLOSED ACCOUNT [DEMOC]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_DEMOLITION_CLOSED_ACCOUNT_DEMOC);
        }
    }

    public class ZLwcEw4MeterChangeOutMtrchWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4MeterChangeOutMtrchWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4MeterChangeOutMtrchWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - METER CHANGE-OUT [MTRCH]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_METER_CHANGE_OUT_MTRCH);
        }
    }

    public class ZLwcEw4ReadMrEditLocalOpsOnlyMredtWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4ReadMrEditLocalOpsOnlyMredtWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4ReadMrEditLocalOpsOnlyMredtWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - READ MR EDIT - LOCAL OPS ONLY [MREDT]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_READ_MR_EDIT_LOCAL_OPS_ONLY_MREDT);
        }
    }

    public class ZLwcEw4ReadToStopEstimateEstWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4ReadToStopEstimateEstWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4ReadToStopEstimateEstWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - READ TO STOP ESTIMATE [EST]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_READ_TO_STOP_ESTIMATE_EST);
        }
    }

    public class ZLwcEw4RepairInstallReadingDeviceRemWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4RepairInstallReadingDeviceRemWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4RepairInstallReadingDeviceRemWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - REPAIR/INSTALL READING DEVICE [REM]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_REPAIR_INSTALL_READING_DEVICE_REM);
        }
    }

    public class ZLwcEw4RereadAndOrInspectForLeakHilowWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4RereadAndOrInspectForLeakHilowWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4RereadAndOrInspectForLeakHilowWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - REREAD AND/OR INSPECT FOR LEAK [HILOW]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_REREAD_AND_OR_INSPECT_FOR_LEAK_HILOW);
        }
    }

    public class ZLwcEw4SetMeterTurnOnAndReadOnsetWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4SetMeterTurnOnAndReadOnsetWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4SetMeterTurnOnAndReadOnsetWorkDescriptionFactory()
        {
            Defaults(
                new {
                    Description = "Z-LWC/EW4 - SET METER/TURN ON & READ [ONSET]",
                    AssetType = typeof(ServiceAssetTypeFactory)
                });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_SET_METER_TURN_ON_AND_READ_ONSET);
        }
    }

    public class ZLwcEw4TurnOnWateronWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ZLwcEw4TurnOnWateronWorkDescriptionFactory(IContainer container) : base(container) { }

        static ZLwcEw4TurnOnWateronWorkDescriptionFactory()
        {
            Defaults(new {Description = "Z-LWC/EW4 - TURN ON WATER [ON]", AssetType = typeof(ServiceAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.Z_LWC_EW4_TURN_ON_WATER_ON);
        }
    }

    public class HydrantNozzleReplacementWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantNozzleReplacementWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantNozzleReplacementWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT NOZZLE REPLACEMENT", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_NOZZLE_REPLACEMENT);
        }
    }

    public class HydrantNozzleInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public HydrantNozzleInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static HydrantNozzleInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "HYDRANT NOZZLE INVESTIGATION", AssetType = typeof(HydrantAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.HYDRANT_NOZZLE_INVESTIGATION);
        }
    }

    public class CrossingInvestigationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public CrossingInvestigationWorkDescriptionFactory(IContainer container) : base(container) { }

        static CrossingInvestigationWorkDescriptionFactory()
        {
            Defaults(new {Description = "Crossing Investigation", AssetType = typeof(MainCrossingAssetTypeFactory)});
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.CROSSING_INVESTIGATION);
        }
    }

    public class ServiceLineRenewalLeadWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineRenewalLeadWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineRenewalLeadWorkDescriptionFactory()
        {
            Defaults(new { Description = "SERVICE LINE RENEWAL-LEAD", AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_LEAD);
        }
    }

    public class ServiceLineInstallationPartialWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineInstallationPartialWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineInstallationPartialWorkDescriptionFactory()
        {
            Defaults(new { Description = "SERVICE LINE INSTALLATION PARTIAL", AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_PARTIAL);
        }
    }

    public class ServiceLineInstallationCompletePartialWorkDescriptionFactory : WorkDescriptionFactory
    {
        public ServiceLineInstallationCompletePartialWorkDescriptionFactory(IContainer container) : base(container) { }

        static ServiceLineInstallationCompletePartialWorkDescriptionFactory()
        {
            Defaults(new { Description = "SERVICE LINE INSTALLATION COMPLETE PARTIAL", AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL);
        }
    }

    public class IrrigationInstallationWorkDescriptionFactory : WorkDescriptionFactory
    {
        public IrrigationInstallationWorkDescriptionFactory(IContainer container) : base(container) { }

        static IrrigationInstallationWorkDescriptionFactory()
        {
            Defaults(new { Description = "IRRIGATION INSTALLATION", AssetType = typeof(ServiceAssetTypeFactory) });
            OnSaving((a, s) => a.Id = (int)WorkDescription.Indices.IRRIGATION_INSTALLATION);
        }
    }

    #endregion

    #region WorkOrder

    // WorkOrder
    public class WorkOrderFactory : TestDataFactory<WorkOrder>
    {
        #region Constants

        public const string DEFAULT_STREET_NUMBER = "1234",
                            DEFAULT_ACCOUNT_CHARGED = "123456789",
                            DEFAULT_PREMISE_NUMBER = "13243546",
                            DEFAULT_PHONE_NUMBER = "867-5309",
                            DEFAULT_BUSINESS_UNIT = "123456",
                            DEFAULT_ORCOM_SERVICE_ORDER_NUMBER = "uh",
                            DEFAULT_NOTES = "hey this is a note",
                            DEFAULT_SPECIALINSTRUCTIONS = "hey this is an instruction";

        public const int DEFAULT_LOST_WATER = 5;

        public const decimal DEFAULT_LATITUDE = 27.134m,
                             DEFAULT_LONGITUDE = 32.3513m;

        public static readonly DateTime DEFAULT_EXCAVATION_DATE = new DateTime(1921, 5, 1);

        public const bool DEFAULT_TRAFFIC_CONTROL_REQUIRED = false,
                          DEFAULT_STREET_OPENING_PERMIT_REQUIRED = false;

        #endregion

        #region Constructors

        static WorkOrderFactory()
        {
            Defaults(new {
                AccountCharged = DEFAULT_ACCOUNT_CHARGED,
                AssetType = typeof(ValveAssetTypeFactory),
                CreatedBy = typeof(UserFactory),
                CreatedAt = Lambdas.GetNow,
                DateReceived = Lambdas.GetNow,
                ExcavationDate = DEFAULT_EXCAVATION_DATE,
                Latitude = DEFAULT_LATITUDE,
                Longitude = DEFAULT_LONGITUDE,
                LostWater = DEFAULT_LOST_WATER,
                MarkoutRequirement = typeof(NoneMarkoutRequirementFactory),
                ORCOMServiceOrderNumber = DEFAULT_ORCOM_SERVICE_ORDER_NUMBER,
                NearestCrossStreet = typeof(StreetFactory),
                Notes = DEFAULT_NOTES,
                PhoneNumber = DEFAULT_PHONE_NUMBER,
                PlantMaintenanceActivityTypeOverride = typeof(PlantMaintenanceActivityTypeFactory),
                PremiseNumber = DEFAULT_PREMISE_NUMBER,
                Priority = typeof(RoutineWorkOrderPriorityFactory),
                Purpose = typeof(RevenueAbove1000WorkOrderPurposeFactory),
                RequestedBy = typeof(WorkOrderRequesterFactory),
                Street = typeof(StreetFactory),
                StreetNumber = DEFAULT_STREET_NUMBER,
                StreetOpeningPermitRequired =
                    DEFAULT_STREET_OPENING_PERMIT_REQUIRED,
                Town = typeof(TownFactory),
                TrafficControlRequired = DEFAULT_TRAFFIC_CONTROL_REQUIRED,
                WorkDescription = typeof(ValveBoxRepairWorkDescriptionFactory),
                OperatingCenter = typeof(UniqueOperatingCenterFactory),
                Valve = typeof(ValveFactory),
                BusinessUnit = DEFAULT_BUSINESS_UNIT,
                SpecialInstructions = DEFAULT_SPECIALINSTRUCTIONS
            });
        }

        public WorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class CompletedWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static CompletedWorkOrderFactory()
        {
            Defaults(new {
                DateCompleted = Lambdas.GetNow
            });
        }

        public CompletedWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class PlanningWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static PlanningWorkOrderFactory()
        {
            Defaults(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory)
            });
        }

        public PlanningWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SchedulingWorkOrderFactory : PlanningWorkOrderFactory
    {
        #region Constructors

        static SchedulingWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var ready = DateTime.Today.AddDays(-1);
                var call = WorkOrdersWorkDayEngine
                   .GetCallDate(ready, MarkoutRequirementEnum.Routine);
                var expiration = WorkOrdersWorkDayEngine
                   .GetExpirationDate(call, MarkoutRequirementEnum.Routine);
                var mo =
                    new MarkoutFactory(s).Create(
                        new {
                            WorkOrder = wo,
                            DateOfRequest = call,
                            ReadyDate = ready,
                            ExpirationDate = expiration
                        });
                wo.Markouts.Add(mo);
            });
        }

        public SchedulingWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class FinalizationWorkOrderFactory : SchedulingWorkOrderFactory
    {
        #region Constructors

        static FinalizationWorkOrderFactory()
        {
            OnSaving((wo, s) => {
                if (wo.CrewAssignments.Count == 0)
                {
                    var crew = new CrewFactory(s).Create(new {
                        Contractor = wo.AssignedContractor
                    });
                    var ca = new CrewAssignmentFactory(s).Build(new {
                        Crew = crew,
                        WorkOrder = wo
                    });
                    //s.Save(ca);
                    wo.CrewAssignments.Add(ca);
                }
            });
        }

        public FinalizationWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class StreetOpeningPermitRequiredWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static StreetOpeningPermitRequiredWorkOrderFactory()
        {
            Defaults(new {
                StreetOpeningPermitRequired = true
            });
        }

        public StreetOpeningPermitRequiredWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class
        StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory : StreetOpeningPermitRequiredWorkOrderFactory
    {
        #region Constructors

        static StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory()
        {
            OnSaved((wo, c) => {
                var permit = c.GetInstance<StreetOpeningPermitFactory>().Create(new {
                    DateIssued = DateTime.Now.AddDays(-2),
                    ExpirationDate = DateTime.Now.AddDays(-1),
                    WorkOrder = wo
                });
                wo.StreetOpeningPermits.Add(permit);
            });
        }

        public StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class
        StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory :
            StreetOpeningPermitRequiredWorkOrderFactory
    {
        #region Constructors

        static StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory()
        {
            Defaults(new {
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });
        }

        public StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory(
            IContainer container) : base(container) { }

        #endregion
    }

    public class
        StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory :
            StreetOpeningPermitRequiredWorkOrderFactory
    {
        #region Constructors

        static StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var permit = new StreetOpeningPermitFactory(s).Create(new {
                    DateIssued = DateTime.Now.AddDays(-1),
                    ExpirationDate = DateTime.Now.AddDays(1),
                    WorkOrder = wo
                });
                wo.StreetOpeningPermits.Add(permit);
            });
        }

        public StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory(IContainer container) :
            base(container) { }

        #endregion
    }

    public class
        StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory :
            StreetOpeningPermitRequiredWorkOrderFactory
    {
        #region Constructors

        static StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var permit = new StreetOpeningPermitFactory(s).Create(new {
                    DateIssued = DateTime.Today.AddDays(1),
                    ExpirationDate = DateTime.Today.AddDays(4),
                    WorkOrder = wo
                });
                wo.StreetOpeningPermits.Add(permit);
            });
        }

        public StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory(IContainer container) :
            base(container) { }

        #endregion
    }

    public class MarkoutRequirementRoutineWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementRoutineWorkOrderFactory()
        {
            Defaults(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory)
            });
        }

        public MarkoutRequirementRoutineWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutRequirementEmergencyWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementEmergencyWorkOrderFactory()
        {
            Defaults(new {
                MarkoutRequirement = typeof(EmergencyMarkoutRequirementFactory)
            });
        }

        public MarkoutRequirementEmergencyWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory : MarkoutRequirementEmergencyWorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory()
        {
            Defaults(new {
                StreetOpeningPermitRequired = true
            });
        }

        public MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class
        MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory : MarkoutRequirementRoutineWorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var markout = new MarkoutFactory(s).Create(new {
                    WorkOrder = wo,
                    ReadyDate = DateTime.Now.AddDays(1),
                    ExpirationDate = DateTime.Now.AddDays(10)
                });
            });
        }

        public MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory(IContainer container) :
            base(container) { }

        #endregion
    }

    public class
        MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory : MarkoutRequirementRoutineWorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var markout = new MarkoutFactory(s).Create(new {
                    WorkOrder = wo,
                    ReadyDate = DateTime.Now.AddDays(-10),
                    ExpirationDate = DateTime.Now.AddDays(-1)
                });
            });
        }

        public MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory : MarkoutRequirementRoutineWorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var markout = new MarkoutFactory(s).Create(new {
                    WorkOrder = wo,
                    ReadyDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(10)
                });
            });
        }

        public MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory : WorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var markout = new MarkoutFactory(s).Create(new {
                    WorkOrder = wo,
                    ReadyDate = DateTime.Now.AddDays(-14),
                    ExpirationDate = DateTime.Now.AddDays(-10)
                });
            });
        }

        public MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class
        MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory : MarkoutRequirementEmergencyWorkOrderFactory
    {
        #region Constructors

        static MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory()
        {
            OnSaved((wo, s) => {
                var markout = new MarkoutFactory(s).Create(new {
                    WorkOrder = wo,
                    ReadyDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(10)
                });
            });
        }

        public MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WorkOrderCancellationReason

    public class WorkOrderCancellationReasonFactory : EntityLookupTestDataFactory<WorkOrderCancellationReason>
    {
        public WorkOrderCancellationReasonFactory(IContainer container) : base(container) { }

        static WorkOrderCancellationReasonFactory()
        {
            var i = 0;

            string descFn() => $"WorkOrderCancellationReason {++i}";
            string statusFn() => $"Status {i}";

            Defaults(new {
                Description = (Func<string>)descFn,
                Status = (Func<string>)statusFn
            });
        }
    }

    #endregion

    #region WorkOrderPriority

    public class WorkOrderPriorityFactory : StaticListEntityLookupFactory<WorkOrderPriority, WorkOrderPriorityFactory>
    {
        public WorkOrderPriorityFactory(IContainer container) : base(container) { }
    }

    public class EmergencyWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        public EmergencyWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static EmergencyWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Emergency"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPriority.Indices.EMERGENCY);
        }
    }

    public class HighPriorityWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        public HighPriorityWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static HighPriorityWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "High Priority"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPriority.Indices.HIGH_PRIORITY);
        }
    }

    public class RoutineWorkOrderPriorityFactory : WorkOrderPriorityFactory
    {
        public RoutineWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static RoutineWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Routine"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPriority.Indices.ROUTINE);
        }
    }

    #endregion

    #region ProductionWorkOrderPriority

    public class ProductionWorkOrderPriorityFactory : StaticListEntityLookupFactory<ProductionWorkOrderPriority,
        ProductionWorkOrderPriorityFactory>
    {
        public ProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }
    }

    public class EmergencyProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public EmergencyProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static EmergencyProductionWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Emergency"});
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.EMERGENCY);
        }
    }

    public class HighProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public HighProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static HighProductionWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "High"});
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.HIGH);
        }
    }

    public class MediumProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public MediumProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static MediumProductionWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Medium"});
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.MEDIUM);
        }
    }

    public class LowProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public LowProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static LowProductionWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Low"});
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.LOW);
        }
    }

    public class RoutineProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public RoutineProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static RoutineProductionWorkOrderPriorityFactory()
        {
            Defaults(new {Description = "Routine"});
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE);
        }
    }

    public class RoutineOffScheduledProductionWorkOrderPriorityFactory : ProductionWorkOrderPriorityFactory
    {
        public RoutineOffScheduledProductionWorkOrderPriorityFactory(IContainer container) : base(container) { }

        static RoutineOffScheduledProductionWorkOrderPriorityFactory()
        {
            Defaults(new { Description = "Routine - Off Scheduled" });
            OnSaving((a, s) => a.Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED);
        }
    }

    #endregion

    #region WorkOrderPurpose

    public class WorkOrderPurposeFactory : StaticListEntityLookupFactory<WorkOrderPurpose, WorkOrderPurposeFactory>
    {
        public WorkOrderPurposeFactory(IContainer container) : base(container) { }
    }

    public class CustomerWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public CustomerWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static CustomerWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Customer"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.CUSTOMER);
        }
    }

    public class ComplianceWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public ComplianceWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static ComplianceWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Compliance"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.COMPLIANCE);
        }
    }

    public class SafetyWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public SafetyWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static SafetyWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Safety"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.SAFETY);
        }
    }

    public class LeakDetectionWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public LeakDetectionWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static LeakDetectionWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Leak Detection"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.LEAK_DETECTION);
        }
    }

    public class Revenue150To500WorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public Revenue150To500WorkOrderPurposeFactory(IContainer container) : base(container) { }

        static Revenue150To500WorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Revenue 150-500"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.REVENUE_150_TO_500);
        }
    }

    public class Revenue500To1000WorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public Revenue500To1000WorkOrderPurposeFactory(IContainer container) : base(container) { }

        static Revenue500To1000WorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Revenue 500-1000"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.REVENUE_500_TO_1000);
        }
    }

    public class RevenueAbove1000WorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public RevenueAbove1000WorkOrderPurposeFactory(IContainer container) : base(container) { }

        static RevenueAbove1000WorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Revenue >1000"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.REVENUE_ABOVE_1000);
        }
    }

    public class DamagedBillableWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public DamagedBillableWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static DamagedBillableWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Damaged/Billable"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.DAMAGED_BILLABLE);
        }
    }

    public class EstimatesWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public EstimatesWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static EstimatesWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Estimates"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.ESTIMATES);
        }
    }

    public class WaterQualityWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public WaterQualityWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static WaterQualityWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Water Quality"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.WATER_QUALITY);
        }
    }

    public class AssetRecordControlWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public AssetRecordControlWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static AssetRecordControlWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Asset Record Control"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.ASSET_RECORD_CONTROL);
        }
    }

    public class SeasonalWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public SeasonalWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static SeasonalWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Seasonal"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.SEASONAL);
        }
    }

    public class DemolitionWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public DemolitionWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static DemolitionWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Demolition"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.DEMOLITION);
        }
    }

    public class BPUWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public BPUWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static BPUWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "BPU"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.BPU);
        }
    }

    public class HurricaneSandyWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public HurricaneSandyWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static HurricaneSandyWorkOrderPurposeFactory()
        {
            Defaults(new {Description = "Hurricane Sandy"});
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.HURRICANE_SANDY);
        }
    }

    public class EquipReliabilityWorkOrderPurposeFactory : WorkOrderPurposeFactory
    {
        public EquipReliabilityWorkOrderPurposeFactory(IContainer container) : base(container) { }

        static EquipReliabilityWorkOrderPurposeFactory()
        {
            Defaults(new { Description = "Equip Reliability" });
            OnSaving((a, s) => a.Id = (int)WorkOrderPurpose.Indices.EQUIP_RELIABILITY);
        }
    }

    #endregion

    #region WorkOrderRequester

    public class WorkOrderRequesterFactory :
        StaticListEntityLookupFactory<WorkOrderRequester, WorkOrderRequesterFactory>
    {
        public WorkOrderRequesterFactory(IContainer container) : base(container) { }
    }

    public class CustomerWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public CustomerWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static CustomerWorkOrderRequesterFactory()
        {
            Defaults(new {Description = "Customer"});
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.CUSTOMER);
        }
    }

    public class EmployeeWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public EmployeeWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static EmployeeWorkOrderRequesterFactory()
        {
            Defaults(new {Description = "Employee"});
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.EMPLOYEE);
        }
    }

    public class LocalGovernmentWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public LocalGovernmentWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static LocalGovernmentWorkOrderRequesterFactory()
        {
            Defaults(new {Description = "Local Government"});
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.LOCAL_GOVERNMENT);
        }
    }

    public class CallCenterWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public CallCenterWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static CallCenterWorkOrderRequesterFactory()
        {
            Defaults(new {Description = "Call Center"});
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.CALL_CENTER);
        }
    }

    public class FRCCWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public FRCCWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static FRCCWorkOrderRequesterFactory()
        {
            Defaults(new {Description = "FRCC"});
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.FRCC);
        }
    }

    public class AcousticMonitoringWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public AcousticMonitoringWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static AcousticMonitoringWorkOrderRequesterFactory()
        {
            Defaults(new { Description = "Acoustic Monitoring" });
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.ACOUSTIC_MONITORING);
        }
    }

    public class NSIWorkOrderRequesterFactory : WorkOrderRequesterFactory
    {
        public NSIWorkOrderRequesterFactory(IContainer container) : base(container) { }

        static NSIWorkOrderRequesterFactory()
        {
            Defaults(new { Description = "NSI" });
            OnSaving((a, s) => a.Id = (int)WorkOrderRequester.Indices.NSI);
        }
    }

    #endregion

    #region WorkOrderInvoice

    public class WorkOrderInvoiceFactory : TestDataFactory<WorkOrderInvoice>
    {
        #region Constructors

        static WorkOrderInvoiceFactory()
        {
            Defaults(new {
                ScheduleOfValueType = typeof(ScheduleOfValueTypeFactory),
                InvoiceDate = Lambdas.GetNowDate
            });
        }

        public WorkOrderInvoiceFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region WorkOrderRequest

    public class WorkOrderRequestFactory : UniqueEntityLookupFactory<WorkOrderRequest>
    {
        public WorkOrderRequestFactory(IContainer container) : base(container) { }
    }

    #endregion

    #region MeterLocation

    public class MeterLocationFactory : UniqueEntityLookupFactory<MeterLocation>
    {
        public MeterLocationFactory(IContainer container) : base(container) { }
    }

    public class InsideMeterLocationFactory : MeterLocationFactory
    {
        #region Constructors

        static InsideMeterLocationFactory()
        {
            Defaults(new {
                Description = "Inside"
            });
            OnSaving((a, s) => a.Id = (int)MeterLocation.Indices.INSIDE);
        }

        public InsideMeterLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class UnknownMeterLocationFactory : MeterLocationFactory
    {
        #region Constructors

        static UnknownMeterLocationFactory()
        {
            Defaults(new {
                Description = "Unknown"
            });
            OnSaving((a, s) => a.Id = (int)MeterLocation.Indices.UNKNOWN);
        }

        public UnknownMeterLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region MeterSupplementalLocation

    public class MeterSupplementalLocationFactory : UniqueEntityLookupFactory<MeterSupplementalLocation>
    {
        public MeterSupplementalLocationFactory(IContainer container) : base(container) { }
    }

    public class InsideMeterSupplementalLocationFactory : MeterSupplementalLocationFactory
    {
        #region Constructors

        static InsideMeterSupplementalLocationFactory()
        {
            Defaults(new {
                Description = "Inside"
            });
            OnSaving((a, s) => a.Id = (int)MeterSupplementalLocation.Indices.INSIDE);
        }

        public InsideMeterSupplementalLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    public class SecureAccessMeterSupplementalLocationFactory : MeterSupplementalLocationFactory
    {
        #region Constructors

        static SecureAccessMeterSupplementalLocationFactory()
        {
            Defaults(new {
                Description = "SecureAccess"
            });
            OnSaving((a, s) => a.Id = (int)MeterSupplementalLocation.Indices.SECURE_ACCESS);
        }

        public SecureAccessMeterSupplementalLocationFactory(IContainer container) : base(container) { }

        #endregion
    }

    #endregion

    #region Common

    public struct Lambdas
    {
        public static readonly Func<DateTime> GetNow = () => DateTime.Now;
        public static readonly Func<DateTime> GetNowDate = () => DateTime.Now.Date;
        public static readonly Func<DateTime> GetYesterday = () => DateTime.Now.Subtract(new TimeSpan(24, 0, 0));

        public static readonly Func<DateTime> GetYesterdayDate =
            () => DateTime.Now.Subtract(new TimeSpan(24, 0, 0)).Date;

        public static readonly Func<DateTime> GetLastYear = () => DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0));
        public static readonly Func<DateTime> GetBeginningOfWeek = () =>
            DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).Date;
    }

    #endregion
}