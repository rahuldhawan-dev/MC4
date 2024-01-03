using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InvestmentProject : IEntity, IThingWithCoordinate, IThingWithOperatingCenter, IThingWithNotes,
        IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int PROJECT_NUMBER = 20,
                             PP_WORKORDER = 20,
                             CONTRACTED_INSPECTOR = 30,
                             STREET_NAME = 50,
                             CREATED_BY = 20,
                             CPS_PRIORITY_NUMBER = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string ProjectNumber { get; set; }
        public virtual string PPWorkOrder { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }

        [Multiline]
        public virtual string ProjectDescription { get; set; }

        [Multiline]
        public virtual string ProjectObstacles { get; set; }

        [Multiline]
        public virtual string ProjectRisks { get; set; }

        [Multiline]
        public virtual string ProjectApproach { get; set; }

        public virtual int? ProjectDurationMonths { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? EstimatedProjectCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? FinalProjectCost { get; set; }

        public virtual Employee AssetOwner { get; set; }
        public virtual Employee ProjectManager { get; set; }
        public virtual Employee ConstructionManager { get; set; }
        public virtual Employee CompanyInspector { get; set; }
        public virtual string ContractedInspector { get; set; }
        public virtual Contractor EngineeringContractor { get; set; }
        public virtual Contractor ConstructionContractor { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual string StreetName { get; set; }
        public virtual Town Town { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        public virtual string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? CIMDate { get; set; }

        public virtual bool? ProjectFlagged { get; set; }
        public virtual bool? CurrentYearActive { get; set; }
        public virtual bool? BulkSale { get; set; }
        public virtual bool? RateCase { get; set; }
        public virtual bool? MISDates { get; set; }
        public virtual bool? COE { get; set; }
        public virtual bool? Geography { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ForecastedInServiceDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ControlDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? PPDate { get; set; }

        public virtual int? PPScore { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? InServiceDate { get; set; }

        public virtual int? CPSReferenceYear { get; set; }
        public virtual string CPSPriorityNumber { get; set; }
        public virtual int? DurationLandAcquisitionInMonths { get; set; }
        public virtual int? DurationPermitDesignInMonths { get; set; }
        public virtual int? DurationConstructionInMonths { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? TargetStartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? TargetEndDate { get; set; }

        public virtual InvestmentProjectPhase Phase { get; set; }
        public virtual InvestmentProjectCategory ProjectCategory { get; set; }
        public virtual InvestmentProjectAssetCategory AssetCategory { get; set; }
        public virtual InvestmentProjectApprovalStatus ApprovalStatus { get; set; }
        public virtual InvestmentProjectStatus ProjectStatus { get; set; }

        #region Notes/Docs

        public virtual IList<InvestmentProjectDocument> InvestmentProjectDocuments { get; set; }
        public virtual IList<InvestmentProjectNote> InvestmentProjectNotes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments =>
            InvestmentProjectDocuments.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(InvestmentProject) + "s";

        public virtual IList<INoteLink> LinkedNotes => InvestmentProjectNotes.Cast<INoteLink>().ToList();

        #endregion

        #endregion

        #region Constructor

        public InvestmentProject()
        {
            InvestmentProjectNotes = new List<InvestmentProjectNote>();
            InvestmentProjectDocuments = new List<InvestmentProjectDocument>();
        }

        #endregion
    }

    [Serializable]
    public class InvestmentProjectPhase : EntityLookup { }

    [Serializable]
    public class InvestmentProjectCategory : EntityLookup { }

    [Serializable]
    public class InvestmentProjectAssetCategory : EntityLookup { }

    [Serializable]
    public class InvestmentProjectApprovalStatus : EntityLookup { }

    [Serializable]
    public class InvestmentProjectStatus : EntityLookup { }
}
