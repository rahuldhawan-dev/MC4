using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class SampleSiteCollectionType
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        #endregion
    }
}