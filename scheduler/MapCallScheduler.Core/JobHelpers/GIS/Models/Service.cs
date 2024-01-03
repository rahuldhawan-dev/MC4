using System;
using MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class Service
    {
        #region Properties

        public int Id { get; set; }
        public string PremiseNumber { get; set; }
        public string Installation { get; set; }
        public ServiceMaterial ServiceMaterial { get; set; }
        public ServiceMaterial CustomerSideMaterial { get; set; }
        public ServiceMaterial Work1VCustomerSideMaterial { get; set; }
        public ServiceEPACode EpaCompanySideMaterial { get; set; }
        
        public ServiceEPACode EpaCustomerSideMaterial { get; set; }
        
        public ServiceEPACode EpaCustomerSideMaterialExternal { get; set; }
        
        public ServiceEPACode EpaConsolidatedCustomerSideMaterial { get; set; }
        public DateTime? DateInstalled { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Town Town { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public State State { get; set; }
        public ServiceUtilityType ServiceUtilityType { get; set; }
        
        public User ServiceMaterialSetBy { get; set; }
        public User CustomerMaterialSetBy { get; set; }

        #endregion

        #region Exposed Methods

        public static Service FromDbRecord(
            MapCall.Common.Model.Entities.MostRecentlyInstalledService service)
        {
            return new Service {
                Id = service.Service.Id,
                PremiseNumber = service.Service.PremiseNumber,
                Installation = service.Service.Installation,
                ServiceMaterial = ServiceMaterial.FromDbRecord(service.ServiceMaterial),
                CustomerSideMaterial = ServiceMaterial.FromDbRecord(service.CustomerSideMaterial),
                Work1VCustomerSideMaterial = ServiceMaterial.FromDbRecord(service.Premise?.MostRecentCustomerMaterial?.CustomerSideMaterial),
                EpaCompanySideMaterial = ServiceEPACode.FromDbRecord(service.Premise?.MostRecentServiceCompanyMaterialEPACode),
                EpaCustomerSideMaterial = ServiceEPACode.FromDbRecord(service.Premise?.MostRecentServiceCustomerMaterialEPACode),
                EpaCustomerSideMaterialExternal = ServiceEPACode.FromDbRecord(service.Premise?.MostRecentCustomerMaterialEPACode),
                EpaConsolidatedCustomerSideMaterial = ServiceEPACode.FromDbRecord(service.Premise?.ConsolidatedCustomerSideMaterial),
                DateInstalled = service.Service.DateInstalled.FromDbRecord(),
                LastUpdated = service.Service.UpdatedAt.ToUniversalTime(),
                Town = Town.FromDbRecord(service.Service.Town),
                OperatingCenter = OperatingCenter.FromDbRecord(service.Service.OperatingCenter),
                State = State.FromDbRecord(service.Service),
                ServiceUtilityType = ServiceUtilityType
                   .FromDbRecord(service.Service.ServiceCategory?.ServiceUtilityType),
                ServiceMaterialSetBy = User.FromDbRecord(service.ServiceMaterialSetBy),
                CustomerMaterialSetBy = User.FromDbRecord(service.CustomerMaterialSetBy)
            };
        }

        #endregion
    }
}
