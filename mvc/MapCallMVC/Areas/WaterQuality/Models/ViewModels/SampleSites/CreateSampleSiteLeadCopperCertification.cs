using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class CreateSampleSiteLeadCopperCertification : ViewModel<SampleSite>
    {
        #region Properties

        [Required, View("Monitoring Period")] // ViewAttribute is not being seen here for some reason.
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(SamplePlan))]
        public int? SamplePlan { get; set; }

        #endregion

        #region Constructors

        public CreateSampleSiteLeadCopperCertification(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            var original = _container.GetInstance<IRepository<SampleSite>>().Find(Id);
            if (original != null)
            {
                // Basically, we want to autoselect the most recent monitoring period.
                SamplePlan = original.SamplePlans.OrderByDescending(x => x.MonitoringPeriodTo).FirstOrDefault()
                                     .Id;
            }
        }

        #endregion
    }
}