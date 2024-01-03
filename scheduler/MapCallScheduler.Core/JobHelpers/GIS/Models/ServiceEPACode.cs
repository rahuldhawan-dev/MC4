using MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class ServiceEPACode
    {
        #region Properties

        public EPACode EPACode { get; set; }

        #endregion

        #region Exposed Methods

        public static ServiceEPACode FromDbRecord(MapCall.Common.Model.Entities.EPACode epaCode)
        {
            return epaCode == null ? null : new ServiceEPACode { EPACode = EPACode.FromDbRecord(epaCode)};
        }

        #endregion
    }
}

