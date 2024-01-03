using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BlowOffInspection : IEntityWithCreationTimeTracking, IValidatableObject, ISAPInspection
    {
        #region Constants

        public struct StringLengths
        {
            public const int BUSINESS_UNIT = 256;
        }

        #endregion
        
        #region Properties

        public virtual decimal MAX_CHLORINE_LEVEL => 3.2m;
        public virtual decimal MIN_CHLORINE_LEVEL => .2m;

        public virtual int Id { get; set; }
        public virtual Valve Valve { get; set; }
        public virtual HydrantProblem HydrantProblem { get; set; }
        public virtual HydrantTagStatus HydrantTagStatus { get; set; }

        public virtual HydrantInspectionType HydrantInspectionType { get; set; }

        // TODO: Normalize these 4 one day
        public virtual WorkOrderRequest WorkOrderRequestOne { get; set; }
        public virtual WorkOrderRequest WorkOrderRequestTwo { get; set; }
        public virtual WorkOrderRequest WorkOrderRequestThree { get; set; }
        public virtual WorkOrderRequest WorkOrderRequestFour { get; set; }
        public virtual User InspectedBy { get; set; }
        public virtual NoReadReason FreeNoReadReason { get; set; }

        public virtual NoReadReason TotalNoReadReason { get; set; }

        [View("Pre Residual/Free Chlorine")]
        public virtual decimal? PreResidualChlorine { get; set; }

        [View("Post Residual/Free Chlorine")]
        public virtual decimal? ResidualChlorine { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime DateInspected { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? FullFlow { get; set; }
        public virtual int? GallonsFlowed { get; set; }
        public virtual decimal? GPM { get; set; }
        public virtual decimal? MinutesFlowed { get; set; }
        public virtual decimal? StaticPressure { get; set; }
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
            Valve != null && Valve.OperatingCenter.SAPEnabled
                          && !Valve.OperatingCenter.IsContractedOperations
                          && string.IsNullOrEmpty(SAPNotificationNumber);

        public virtual string RecordUrl { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
