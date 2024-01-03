using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class UnitOfWaterSampleMeasureController : ControllerBaseWithPersistence<UnitOfWaterSampleMeasure, User>
    {
        public UnitOfWaterSampleMeasureController(ControllerBaseWithPersistenceArguments<IRepository<UnitOfWaterSampleMeasure>, UnitOfWaterSampleMeasure, User> args) : base(args) {}

        [HttpGet]
        public ActionResult ForSampleIdMatrix(int sampleIdMatrix)
        {
            var matrix = _container.GetInstance<IRepository<SampleIdMatrix>>().Find(sampleIdMatrix);

            return new CascadingActionResult(Repository.GetAll(), "Description", "Id") {
                SelectedValue = matrix?.WaterConstituent?.UnitOfMeasure?.Id
            };
        }
    }
}