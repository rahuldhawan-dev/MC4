using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using IContainer = StructureMap.IContainer;
using MMSINC.Utilities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class EditEnvironmentalNonComplianceEvent : EnvironmentalNonComplianceEventViewModel
    {
        #region Properties

        [Secured(AppliesToAdmins = false)]
        public override DateTime? EventDate { get; set; }

        [Secured(AppliesToAdmins = false)]
        public override DateTime? AwarenessDate { get; set; }

        public virtual DateTime? DateOfEnvironmentalLeadershipTeamReview { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime? CreatedAt { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        public virtual User CreatedBy { get; set; }

        [View(EnvironmentalNonComplianceEvent.DisplayNames.CREATED_BY)]
        public virtual string CreatedByFullName => CreatedBy?.FullName;

        #endregion

        #region Constructors

        public EditEnvironmentalNonComplianceEvent(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(EnvironmentalNonComplianceEvent entity)
        {
            base.Map(entity);

            if (entity.PublicWaterSupply != null && entity.WasteWaterSystem != null)
            {
                WaterType = MapCall.Common.Model.Entities.WaterType.Indices.WATERWASTEWATER;
            }
            else if (entity.WasteWaterSystem == null) 
            {
                WaterType = MapCall.Common.Model.Entities.WaterType.Indices.WATER;
            }
            else if (entity.PublicWaterSupply == null)
            {
                WaterType = MapCall.Common.Model.Entities.WaterType.Indices.WASTEWATER;
            }
        }

        #endregion
    }
}
