using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class AddSampleSiteSamplePlan : ViewModel<SampleSite>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped.")]
        [Required, EntityMustExist(typeof(SamplePlan)), DropDown]
        public int? SamplePlan { get; set; }

        #endregion

        #region Constructors

        public AddSampleSiteSamplePlan(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override SampleSite MapToEntity(SampleSite entity)
        {
            var samplePlan = _container.GetInstance<IRepository<SamplePlan>>().Find(SamplePlan.Value);
            if (!entity.SamplePlans.Contains(samplePlan))
                entity.SamplePlans.Add(samplePlan);
            return entity;
        }

        #endregion
    }
}