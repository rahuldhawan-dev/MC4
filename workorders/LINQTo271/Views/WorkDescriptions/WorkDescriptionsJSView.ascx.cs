using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.UI;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkDescriptions
{
    public partial class WorkDescriptionsJSView : UserControl
    {
        protected string Descriptions => GetDescriptions();

        private static string GetDescriptions()
        {
            var descriptions = WorkDescriptionRepository
                              .SelectAllAsList()
                              .Select(WorkDescription.WorkDescriptionToJson)
                              .ToArray();

            return Json.Encode(descriptions);
        }
    }
}

