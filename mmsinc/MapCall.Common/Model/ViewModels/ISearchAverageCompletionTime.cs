using System;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    /// <summary>
    /// None of these properties are searchable by default, they need to be manually wired in
    /// in the repository method.
    /// </summary>
    public interface ISearchAverageCompletionTime : ISearchSet<AverageCompletionTime>
    {
        [Search(CanMap = false)]
        DateTime? StartDate { get; set; }
        [Search(CanMap = false)]
        DateTime? EndDate { get; set; }
        [Search(CanMap = false)]
        int? OperatingCenter { get; set; }
    }
}
