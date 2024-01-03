using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MMSINC.Data.Linq;

namespace WorkOrders.Model
{
    public class StreetOpeningPermitRepository : WorkOrdersRepository<StreetOpeningPermit>, IStreetOpeningPermitRepository
    {
        #region Exposed Static Methods

        public static IEnumerable<StreetOpeningPermit> GetStreetOpeningPermitsByWorkOrder(int workOrderID)
        {
            return (from m in DataTable
                    where m.WorkOrderID == workOrderID
                    orderby m.DateRequested
                    select m);
        }

        public static void InsertStreetOpeningPermit(int workOrderID, string streetOpeningPermitNumber, DateTime dateRequested, DateTime? dateIssued, DateTime? expirationDate, string notes)
        {
            try
            {
                var sop = new StreetOpeningPermit {
                    WorkOrderID = workOrderID,
                    StreetOpeningPermitNumber = streetOpeningPermitNumber,
                    DateRequested = dateRequested,
                    DateIssued = dateIssued,
                    ExpirationDate = expirationDate,
                    Notes = notes
                };
                
                //Its lame this has to be done but apparently ConvertEmptyStringToNull doesn't work on DateTimes
                if(sop.DateIssued == DateTime.MinValue)
                    sop.DateIssued = null;

                if (sop.ExpirationDate == DateTime.MinValue)
                    sop.ExpirationDate = null;

                Insert(sop);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateStreetOpeningPermit(string streetOpeningPermitNumber, DateTime dateRequested, DateTime? dateIssued, DateTime? expirationDate, int streetOpeningPermitID, string notes)
        {
            try
            {
                var sop = GetEntity(streetOpeningPermitID);
                
                sop.StreetOpeningPermitNumber = streetOpeningPermitNumber;
                sop.DateRequested = dateRequested;
                sop.DateIssued = dateIssued == DateTime.MinValue ? null : dateIssued;
                sop.ExpirationDate = expirationDate == DateTime.MinValue ? null : expirationDate;
                sop.Notes = notes;
                
                Update(sop);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void DeleteStreetOpeningPermit(int streetOpeningPermitID)
        {
            var entity = GetEntity(streetOpeningPermitID);
            Delete(entity);
        }
        
        public static void UpdateStreetOpeningPermitDrawingsPayments(int workOrderID, int permitId, bool hasMetDrawingRequirements, bool isPaidFor)
        {
            var sop =
                GetStreetOpeningPermitsByWorkOrder(workOrderID).FirstOrDefault(x => x.PermitId == permitId);
            if (sop != default(StreetOpeningPermit))
            {
                sop.HasMetDrawingRequirement = hasMetDrawingRequirements;
                sop.IsPaidFor = isPaidFor;
                Update(sop);
            }
        }

        #endregion

        public StreetOpeningPermit FindByPermitId(int permitId)
        {
            return
                (from sop in DataTable where sop.PermitId == permitId select sop)
                    .FirstOrDefault();
        }
    }

    public interface IStreetOpeningPermitRepository : IRepository<StreetOpeningPermit>
    {
        StreetOpeningPermit FindByPermitId(int permitId);
    }
}
