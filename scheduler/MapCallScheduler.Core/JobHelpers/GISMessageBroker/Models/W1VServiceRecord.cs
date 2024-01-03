using MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class W1VServiceRecord
    {
        #region Properties

        public string PremiseId { get; set; }
        public string FsrId { get; set; }
        public string CompanySideMaterial { get; set; }
        public string CustomerSideMaterial { get; set; }
        public string CustomerSideMaterialExternal { get; set; }
        public string EpaConvertedCompanySideMaterial { get; set; }
        public string EpaConvertedCustomerSideMaterial { get; set; }
        public string EpaConvertedCustomerSideMaterialExternal { get; set; }
        public string EpaConvertedConsolidatedCustomerSideMaterial { get; set; }
        public string SubmitDate { get; set; }
        public string EquipmentId { get; set; }
        public string DeviceLocationId { get; set; }
        public string InstallationId { get; set; }
        public string InstallationType { get; set; }

        #endregion

        #region Exposed Methods

        public static W1VServiceRecord FromDbRecord(Service entity)
        {
            var state = entity.Premise?.OperatingCenter?.State;
            return new W1VServiceRecord {
                PremiseId = entity.PremiseNumber,
                FsrId = entity.UpdatedBy?.Employee?.EmployeeId,
                SubmitDate = entity.UpdatedAt.ToString("d"),
                EquipmentId = entity.Premise?.Equipment,
                DeviceLocationId = entity.Premise?.DeviceLocation,
                InstallationId = entity.Premise?.Installation,
                InstallationType = entity.Premise?.ServiceUtilityType?.Division,

                // Company side materials
                CompanySideMaterial = entity.ServiceMaterial?.Description,
                EpaConvertedCompanySideMaterial =
                    entity.ServiceMaterial?.CompanyEPACodeOverridenOrDefault(state)?.Description,

                //W1V materials
                CustomerSideMaterial = entity.Premise?.MostRecentCustomerMaterial?.CustomerSideMaterial?.Description,
                EpaConvertedCustomerSideMaterial = entity.Premise?.MostRecentCustomerMaterial?.CustomerSideMaterial
                                                        ?.CustomerEPACodeOverridenOrDefault(state)?.Description,

                // MapCall materials
                //Mapcall materials are mapped as external because that is how it shows up in SAP and
                // that is how W1V is pulling the data as well
                CustomerSideMaterialExternal = entity.CustomerSideMaterial?.Description,
                EpaConvertedCustomerSideMaterialExternal = entity.CustomerSideMaterial
                                                                ?.CustomerEPACodeOverridenOrDefault(state)?.Description,
                EpaConvertedConsolidatedCustomerSideMaterial = entity.Premise?.ConsolidatedCustomerSideMaterial?.Description,
            };
        }

        #endregion
    }
}
