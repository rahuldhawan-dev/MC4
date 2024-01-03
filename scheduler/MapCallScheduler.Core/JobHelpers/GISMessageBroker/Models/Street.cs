using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class Street
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        public StreetPrefix Prefix { get; set; }
        public StreetSuffix Suffix { get; set; }
        [Required]
        public string Name { get; set; }

        #endregion
    }
}