using System;
using System.Web.Mvc;

namespace MapCallMVC.Configuration
{
    /// <summary>
    /// This filter's purpose is to log in ReportViewing that a user ran a report
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AuditReportAttribute : ActionFilterAttribute
    {
        #region Properties

        public string ReportName { get; set; }

        #endregion

        #region Constructors

        public AuditReportAttribute(string reportName)
        {
            ReportName = reportName;
        }

        #endregion
    }
}
