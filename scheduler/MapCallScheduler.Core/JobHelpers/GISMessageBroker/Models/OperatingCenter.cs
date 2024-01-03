using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class OperatingCenter
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string OperatingCenterCode { get; set; }
        [Required]
        public string OperatingCenterName { get; set; }

        #endregion
    }
}