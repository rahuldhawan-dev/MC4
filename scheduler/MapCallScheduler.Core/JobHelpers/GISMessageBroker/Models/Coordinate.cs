using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class Coordinate
    {
        #region Properties

        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }

        #endregion
    }
}