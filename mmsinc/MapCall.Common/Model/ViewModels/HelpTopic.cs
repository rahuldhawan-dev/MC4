using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchHelpTopicWithDocument : ISearchSet<HelpTopic>
    {
        [Search(CanMap = false)]
        DateRange DocumentUpdated { get; set; }

        [Search(CanMap = false)]
        SearchString DocumentTitle { get; set; }

        [Search(CanMap = false)]
        int? Active { get; set; }

        [Search(CanMap = false)]
        int[] DocumentType { get; set; }
        
        [Search(CanMap = false)]
        DateRange DocumentNextReviewDate { get; set; }
    }
}
