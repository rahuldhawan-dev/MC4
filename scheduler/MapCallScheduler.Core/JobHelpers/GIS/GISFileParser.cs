using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileParser : IGISFileParser
    {
        #region Exposed Methods

        public IEnumerable<ParsedRecord> ParseHydrants(string json)
        {
            var data = JsonConvert.DeserializeObject<ParsedFile>(json);
            return data.Hydrants;
        }

        public IEnumerable<ParsedRecord> ParseValves(string json)
        {
            var data = JsonConvert.DeserializeObject<ParsedFile>(json);
            return data.Valves;
        }

        public IEnumerable<ParsedRecord> ParseSewerOpenings(string json)
        {
            var data = JsonConvert.DeserializeObject<ParsedFile>(json);
            return data.SewerOpenings;
        }

        public IEnumerable<ParsedRecord> ParseServices(string json)
        {
            var data = JsonConvert.DeserializeObject<ParsedFile>(json);
            return data.Services;
        }

        #endregion

        #region Nested Type: ParsedFile

        public class ParsedFile
        {
            #region Properties

            public string DataType { get; set; }
            public string SourceSystem { get; set; }
            public IList<ParsedRecord> Hydrants { get; set; }
            public IList<ParsedRecord> Valves { get; set; }
            public IList<ParsedRecord> SewerOpenings { get; set; }
            public IList<ParsedRecord> Services { get; set; }

            #endregion
        }

        #endregion

        #region Nested Type: ParsedRecord

        public class ParsedRecord
        {
            #region Properties

            public int Id { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }

            #endregion
        }

        #endregion
    }

    public interface IGISFileParser
    {
        #region Abstract Methods

        IEnumerable<GISFileParser.ParsedRecord> ParseHydrants(string json);
        IEnumerable<GISFileParser.ParsedRecord> ParseValves(string json);
        IEnumerable<GISFileParser.ParsedRecord> ParseSewerOpenings(string json);
        IEnumerable<GISFileParser.ParsedRecord> ParseServices(string json);

        #endregion
    }
}