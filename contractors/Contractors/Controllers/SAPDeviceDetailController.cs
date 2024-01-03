using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace Contractors.Controllers
{
    public class SAPDeviceDetailController : ControllerBaseWithValidation<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder>
    {
        #region Constructors

        public SAPDeviceDetailController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) { }
        
        #endregion

        #region Search/Show/Index

        [HttpGet]
        public ActionResult Index(SearchSAPDeviceDetail search)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(search.WorkOrderID);

            //TODO: Fail gracefully
            if (workOrder?.DeviceLocation == null)
            {
                return new JsonResult {
                    Data = new SAPDeviceCollection {
                        EquipmentData = new SAPDeviceDetailResponse[] {
                            new SAPDeviceDetailResponse {
                                ReturnStatuses = new List<SAPStatus>() {
                                    new SAPStatus() {
                                        ReturnStatusDescription = (workOrder == null)
                                            ? "Unable to locate a valid work order. Please ensure it is not cancelled."
                                            : "Must Provide a WorkOrder with a valid Device Location"
                                    }
                                }
                            }
                        }
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            var repo = _container.GetInstance<ISAPDeviceRepository>();
            var result = repo.Search(new SAPDeviceDetailRequest {
                MeterSerialNumber = search.MeterSerialNumber,
                ActionCode = search.ActionCode,
                DeviceLocation = workOrder.DeviceLocation?.ToString(),
                DeviceType = search.DeviceType
            });

            return this.RespondTo((formatter) => {
                formatter.Json(() => {
                    return Json(SetReadType(result), JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        #region Private Methods

        private SAPDeviceCollection SetReadType(SAPDeviceCollection result)
        {
            // SAP is sending code & description with comma separated in ReadType
            // So splitting & getting entity id of mapping table
            if (result.EquipmentData.Any() && result.EquipmentData[0].ReadType != null)
            {
                result.EquipmentData[0].ReadType = _container
                                                  .GetInstance<IRepository<ServiceInstallationReadType>>()
                                                  .Where(x => x.SAPCode == result.EquipmentData[0].ReadType.Split(',')
                                                      .First()).FirstOrDefault()?.Id.ToString();

                if (result.EquipmentData[0].RegisterDetails != null)
                {
                    foreach (var item in result.EquipmentData[0].RegisterDetails)
                    {
                        if (!string.IsNullOrWhiteSpace(item.ReadType))
                        {
                            var serviceInstallationReadType = _container
                                                             .GetInstance<IRepository<ServiceInstallationReadType>>()
                                                             .Where(x => x.SAPCode == item.ReadType.Split(',').First())
                                                             .FirstOrDefault();

                            item.ReadType = serviceInstallationReadType?.Id.ToString();
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}