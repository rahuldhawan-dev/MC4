using System.Collections.Generic;

namespace MapCall.Common.Model.ViewModels
{
    // NOTE: This class is directly serialized to json for VideoPicker and VideoPlayer.
    public class SproutVideo
    {
        public string Id { get; set; }
        public string EmbedCode { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }

        // This is a pre-formatted date string
        public string CreatedAt { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    public class SproutTag
    {
        #region Properties

        public string TagId { get; set; }
        public string Description { get; set; }

        #endregion
    }
}
