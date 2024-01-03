using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string EmployeeId { get; set; }
        public virtual string EmailAddress { get; set; }
    }
}
