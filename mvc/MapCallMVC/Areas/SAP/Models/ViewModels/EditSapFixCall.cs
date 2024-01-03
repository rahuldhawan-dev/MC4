using System;
using MapCall.SAP.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.SAP.Models.ViewModels
{
    public class EditSapFixCall : ViewModel<SAPFixCall>
    {
        #region Fields

        private DateTime? _correctePlanDate { get; set; }

        #endregion
        
        #region Constructors

        public EditSapFixCall(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap("SAP Entities don't have Ids")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual string MaintenancePlan { get; set; }
        public virtual string CallNumber { get; set; }
        public virtual string PlanDate { get; set; }

        [DoesNotAutoMap("This is manually mapped to to SAPFixCall.PlanDate in the SAPMaintenancePlanController for some reason.")]
        [View(DisplayFormat= CommonStringFormats.DATE)]
        public virtual DateTime? CorrectedPlanDate
        {
            get
            {
                if (!_correctePlanDate.HasValue && !string.IsNullOrWhiteSpace(PlanDate) && PlanDate != "00000000")

                    _correctePlanDate = DateTime.Parse(PlanDate);// DateTime.ParseExact(PlanDate,"yyyyMMdd",CultureInfo.InvariantCulture, DateTimeStyles.None);
                return _correctePlanDate;
            }
            set { _correctePlanDate = value; }
        }
        #endregion
    }
}
