using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WorkOrders.Model
{
    public class MarkoutRepository : WorkOrdersRepository<Markout>
    {
        #region Exposed Static Methods

        public static IEnumerable<Markout> GetMarkoutsByWorkOrder(int workOrderID)
        {
            return (from m in DataTable
                    where m.WorkOrderID == workOrderID
                    orderby m.DateOfRequest
                    select m);
        }

        public static void DeleteMarkout(int markoutID)
        {
            var entity = GetEntity(markoutID);
            Delete(entity);
        }

        public static void InsertMarkout(int workOrderID, int markoutTypeID, DateTime dateOfRequest, DateTime readyDate, DateTime expirationDate, string markoutNumber, string note, int creatorID)
        {
            try
            {
                var mo = new Markout {
                    WorkOrderID = workOrderID,
                    MarkoutTypeID = markoutTypeID,
                    DateOfRequest = dateOfRequest,
                    ReadyDate = readyDate,
                    ExpirationDate = expirationDate,
                    MarkoutNumber = markoutNumber, 
                    Note = note, 
                    CreatorID = creatorID
                };
                Insert(mo);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateMarkout(int markoutTypeID, DateTime dateOfRequest, DateTime readyDate, DateTime expirationDate, string markoutNumber, int markoutID, string note)
        {
            var mo = GetEntity(markoutID);
            mo.MarkoutTypeID = markoutTypeID;
            mo.DateOfRequest = dateOfRequest;
            mo.MarkoutNumber = markoutNumber;
            mo.ReadyDate = readyDate;
            mo.ExpirationDate = expirationDate;
            mo.Note = note;
            Update(mo);
        }

        #endregion
    }
}
