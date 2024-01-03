using System.ComponentModel.DataAnnotations;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class State
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        [Required]
        public string Abbreviation { get; set; }

        #endregion
    }
}