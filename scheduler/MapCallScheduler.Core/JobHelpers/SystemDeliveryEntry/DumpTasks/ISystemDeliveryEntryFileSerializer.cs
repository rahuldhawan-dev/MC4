using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.ViewModels;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public interface ISystemDeliveryEntryFileSerializer
    {
        string Serialize(IQueryable<SystemDeliveryEntryFileDumpViewModel> coll);
    }
}
