namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class EPACode
    {
        #region Properties

        public int Id { get; set; }
        public string Description { get; set; }

        #endregion

        #region Exposed Methods

        public static EPACode FromDbRecord(MapCall.Common.Model.Entities.EPACode epaCode)
        {
            return epaCode == null ? null : new EPACode { Id = epaCode.Id, Description = epaCode.Description };
        }

        #endregion
    }
}

