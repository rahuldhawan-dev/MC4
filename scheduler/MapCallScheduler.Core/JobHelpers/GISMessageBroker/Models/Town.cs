using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class Town
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }

        #endregion
    }
}