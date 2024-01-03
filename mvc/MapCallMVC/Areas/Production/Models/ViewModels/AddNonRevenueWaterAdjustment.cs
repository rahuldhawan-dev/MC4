using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddNonRevenueWaterAdjustment : ViewModel<NonRevenueWaterEntry>
    {
        #region Properties

        [Required, DoesNotAutoMap, StringLength(NonRevenueWaterAdjustment.StringLengths.COMMENTS)]
        public string Comments { get; set; }
        
        [DropDown, Required, DoesNotAutoMap]
        public string BusinessUnit { get; set; }

        [Required, DoesNotAutoMap]
        public long TotalGallons { get; set; }

        #endregion

        #region Constructors

        public AddNonRevenueWaterAdjustment(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private void MapNonRevenueWaterEntryAdjustments(NonRevenueWaterEntry entity)
        {
            entity.NonRevenueWaterAdjustments.Add(new NonRevenueWaterAdjustment() {
                TotalGallons = TotalGallons,
                Comments = Comments,
                BusinessUnit = BusinessUnit,
                NonRevenueWaterEntry = entity
            });
        }

        #endregion

        #region Exposed Methods

        public override NonRevenueWaterEntry MapToEntity(NonRevenueWaterEntry entity)
        {
            base.MapToEntity(entity);

            MapNonRevenueWaterEntryAdjustments(entity);

            return entity;
        }

        #endregion
    }
}
