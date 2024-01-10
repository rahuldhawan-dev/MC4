using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class PublicWaterSupply
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string Identifier { get; set; }

        public PublicWaterSupplyStatus Status { get; set; }
        
        public string System { get; set; }

        #endregion
    }
}