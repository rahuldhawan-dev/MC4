using System;

namespace MapCall.Common.Model.ViewModels
{
    public class CrewWorkOrderDetails
    {
        public int Id { get; set; }
        public string WorkDescription { get; set; }
        public DateTime? DateReceived { get; set; }
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string NearestCrossStreet { get; set; }
        public string Town { get; set; }
        public string TownSection { get; set; }
        public bool MarkoutRequired { get; set; }
    }
}
