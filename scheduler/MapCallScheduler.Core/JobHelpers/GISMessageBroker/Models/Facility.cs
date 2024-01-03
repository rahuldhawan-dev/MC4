using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class Facility
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string FacilityName { get; set; }

        #endregion
    }
}