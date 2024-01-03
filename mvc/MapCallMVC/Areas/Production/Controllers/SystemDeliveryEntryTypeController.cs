using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class SystemDeliveryEntryTypeController : ControllerBaseWithPersistence<IRepository<SystemDeliveryEntryType>, SystemDeliveryEntryType, User>
    {
        [HttpGet]
        public ActionResult BySystemDeliveryTypeId(int systemDeliveryTypeId)
        {
            return new CascadingActionResult(Repository.Where(x => x.SystemDeliveryType.Id == systemDeliveryTypeId), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByFacilitiesSystemDeliveryTypeId(int[] facId)
        {
            var repo = _container.GetInstance<IFacilityRepository>();
            List<int> deliveryTypeList = new List<int>();

            foreach (int i in facId)
            {
                var facility = repo.Find(i);
                var facilitySystemDeliveryType = facility.SystemDeliveryType?.Id; 
                if (facilitySystemDeliveryType != null && !deliveryTypeList.Contains(facility.SystemDeliveryType.Id))
                {
                    deliveryTypeList.Add(facility.SystemDeliveryType.Id);
                }
            }

            var result = Repository.Where(x => deliveryTypeList.Contains(x.SystemDeliveryType.Id));
            return new CascadingActionResult(result, "Description", "Id");
        }

        public SystemDeliveryEntryTypeController(ControllerBaseWithPersistenceArguments<IRepository<SystemDeliveryEntryType>, SystemDeliveryEntryType, User> args) : base(args) { }
    }
}
