using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class SewerOpening
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        public string OpeningNumber { get; set; }

        #endregion
    }
}
