namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class ValveSize
    {
        #region Properties

        public int Id { get; set; }
        public decimal Size { get; set; }

        #endregion

        #region Exposed Methods

        public static ValveSize FromDbRecord(MapCall.Common.Model.Entities.ValveSize size)
        {
            return size == null ? null : new ValveSize {Id = size.Id, Size = size.Size};
        }

        #endregion
    }
}