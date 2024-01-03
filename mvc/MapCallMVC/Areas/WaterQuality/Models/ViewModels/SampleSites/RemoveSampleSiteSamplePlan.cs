using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class RemoveSampleSiteSamplePlan : ViewModel<SampleSite>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped.")]
        [Required, EntityMustExist(typeof(SamplePlan))]
        public int? SamplePlan { get; set; }

        #endregion

        #region Constructors

        public RemoveSampleSiteSamplePlan(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override SampleSite MapToEntity(SampleSite entity)
        {
            var samplePlan = entity.SamplePlans.SingleOrDefault(x => x.Id == SamplePlan.Value);
            entity.SamplePlans.Remove(samplePlan);
            return entity;
        }

        #endregion
    }
}