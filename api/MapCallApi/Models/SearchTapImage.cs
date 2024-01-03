using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchTapImage : SearchSet<TapImage>
    {
        public string PremiseNumber { get; set; }
        public string StreetNumber { get; set; }
        public string StreetPrefix { get; set; }
        public string Street { get; set; }
        public string StreetSuffix { get; set; }
        [SearchAlias("Town", "T", "ShortName", Required = true)]
        public string Town { get; set; }
        [SearchAlias("T.State", "S", "Abbreviation")]
        public string State { get; set; }
        
        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (!string.IsNullOrWhiteSpace(PremiseNumber))
            {
                mapper.MappedProperties["PremiseNumber"].Value = new SearchString {
                    Value = PremiseNumber, MatchType = SearchStringMatchType.Exact
                };
            }
        }
    }
}
