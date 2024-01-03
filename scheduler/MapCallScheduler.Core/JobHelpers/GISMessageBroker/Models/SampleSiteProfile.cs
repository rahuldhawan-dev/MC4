using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class SampleSiteProfile
    {
        #region Properties

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public SampleSiteProfileAnalysisType SampleSiteProfileAnalysisType { get; set; }

        [Required]
        public PublicWaterSupply PublicWaterSupply { get; set; }

        #endregion
    }
}