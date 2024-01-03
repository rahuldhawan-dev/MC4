using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace WorkOrders.Model
{
    public interface IContractorRepository : IRepository<Contractor>
    {

    }

    public class ContractorRepository : WorkOrdersRepository<Contractor>, IContractorRepository
    {
        public static IEnumerable<Contractor> GetContractorsByOperatingCenterID(int operatingCenterID)
        {
            var opCode =
                OperatingCenterRepository.GetOpCodeByOperatingCenterID(
                    operatingCenterID);
            return GetContractorsByOperatingCenterText(opCode);
        }

        public static IEnumerable<Contractor> GetContractorsByOperatingCenterText(string opCode)
        {
            return GetContractorsByOperatingCenter(
                OperatingCenterRepository.GetOperatingCenterByOpCntr(opCode));
        }

        private static IEnumerable<Contractor> GetContractorsByOperatingCenter(OperatingCenter center)
        {
            return center.Contractors;
        }

        public static IEnumerable<Contractor> SelectAllSorted()
        {
            return (from c in DataTable
                    where c.ContractorsOperatingCenters.Any(x => SecurityService.UserOperatingCenters.Contains(x.OperatingCenter)) 
                    orderby c.Name
                    select c);
        }
    }
}
