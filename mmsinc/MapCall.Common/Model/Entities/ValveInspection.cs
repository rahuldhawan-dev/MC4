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
    public class ValveInspection
        : IEntityWithCreationTimeTracking, IValidatableObject, IThingWithOperatingCenter, ISAPInspection
    {
        #region Constants

        public struct Display
        {
            public const string TURNS = "Number of Turns Completed",
                                TURNS_NOT_COMPLETED = "Accept even if Min Req Turns not completed";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Valve Valve { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime DateInspected { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS)]
        public virtual decimal? MinimumRequiredTurns { get; set; }

        public virtual bool Inspected { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string Remarks { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS),
         View(Display.TURNS)]
        public virtual decimal? Turns { get; set; }

        [View(Display.TURNS_NOT_COMPLETED)]
        public virtual bool TurnsNotCompleted { get; set; }

        public virtual User InspectedBy { get; set; }
        public virtual ValveNormalPosition NormalPosition { get; set; }
        public virtual ValveNormalPosition PositionFound { get; set; }
        public virtual ValveNormalPosition PositionLeft { get; set; }

        public virtual ValveWorkOrderRequest WorkOrderRequestOne { get; set; }
        public virtual ValveWorkOrderRequest WorkOrderRequestTwo { get; set; }
        public virtual ValveWorkOrderRequest WorkOrderRequestThree { get; set; }

        /// <summary>
        /// For some reason this can't be => Valve.OperatingCenter. Suspect
        /// it screws up some reports. We set through MapToEntity so that notifications
        /// can be routed properly.
        /// </summary>        [AutoMap(MapDirections.None)]
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual string SAPErrorCode { get; set; }
        public virtual string SAPNotificationNumber { get; set; }

        [DoesNotExport]
        public virtual bool SendToSAP =>
            Valve != null && Valve.OperatingCenter.SAPEnabled
                          && !Valve.OperatingCenter.IsContractedOperations
                          && string.IsNullOrEmpty(SAPNotificationNumber);

        //[AutoMap(MapDirections.None)]
        //public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
