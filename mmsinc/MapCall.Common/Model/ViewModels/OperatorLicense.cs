using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchOperatorLicenseReport : ISearchSet<OperatorLicense>
    {
        [Search(CanMap = false)]
        bool? Expired { get; set; }
    }
}
