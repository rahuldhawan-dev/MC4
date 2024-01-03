using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HydrantInspection
        : IEntityWithCreationTimeTracking, IValidatableObject, ISAPInspection, IThingWithOperatingCenter
    {
        #region Constants

        public virtual decimal MAX_CHLORINE_LEVEL => 3.2m;
        public virtual decimal MIN_CHLORINE_LEVEL => .2m;

        public struct StringLengths
        {
            public const int BUSINESS_UNIT = 256;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Hydrant Hydrant { get; set; }

        [DisplayName("Problem")]
        public virtual HydrantProblem HydrantProblem { get; set; }

        public virtual HydrantTagStatus HydrantTagStatus { get; set; }

        [DisplayName("Inspection Type")]
        public virtual HydrantInspectionType HydrantInspectionType { get; set; }

        // TODO: Normalize these 4 one day
        [DisplayName("Work Order Request #1")]
        public virtual WorkOrderRequest WorkOrderRequestOne { get; set; }

        [DisplayName("Work Order Request #2")]
        public virtual WorkOrderRequest WorkOrderRequestTwo { get; set; }

        [DisplayName("Work Order Request #3")]
        public virtual WorkOrderRequest WorkOrderRequestThree { get; set; }

        [DisplayName("Work Order Request #4")]
        public virtual WorkOrderRequest WorkOrderRequestFour { get; set; }

        public virtual User InspectedBy { get; set; }

        [View("Pre Residual/Free Chlorine")]
        public virtual decimal? PreResidualChlorine { get; set; }

        [View("Post Residual/Free Chlorine")]
        public virtual decimal? ResidualChlorine { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime DateInspected { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? FullFlow { get; set; }

        //formula field
        public virtual int? GallonsFlowed { get; set; }

        public virtual NoReadReason FreeNoReadReason { get; set; }
        public virtual NoReadReason TotalNoReadReason { get; set; }

        public virtual decimal? GPM { get; set; }
        public virtual decimal? MinutesFlowed { get; set; }
        public virtual decimal? StaticPressure { get; set; }

        [Multiline]
        public virtual string Remarks { get; set; }

        [View("Pre Total Chlorine")]
        public virtual decimal? PreTotalChlorine { get; set; }

        [View("Post Total Chlorine")]
        public virtual decimal? TotalChlorine { get; set; }

        public virtual string SAPErrorCode { get; set; }
        public virtual string SAPNotificationNumber { get; set; }
        
        public virtual string BusinessUnit { get; set; }

        [DoesNotExport]
        public virtual bool SendToSAP =>
            Hydrant != null && Hydrant.OperatingCenter.SAPEnabled
                            && !Hydrant.OperatingCenter.IsContractedOperations
                            && string.IsNullOrEmpty(SAPNotificationNumber);

        /// <summary>
        /// For some reason this can't be => Hydrant.OperatingCenter. Suspect
        /// it screws up some reports. We set through MapToEntity so that notifications
        /// can be routed properly.
        /// </summary>
        [AutoMap(MapDirections.None)]
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual string RecordUrl { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class HydrantProblem : EntityLookup { }

    [Serializable]
    public class HydrantInspectionType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int FLUSH = 1, INSPECT = 2, INSPECT_FLUSH = 3, WATER_QUALITY = 4;
        }
    }

    [Serializable]
    public class WorkOrderRequest : EntityLookup { }
}
