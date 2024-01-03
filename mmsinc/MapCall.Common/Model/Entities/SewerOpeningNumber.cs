namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// This is a dumb data class needed in the SewerOpeningRepository when it generates 
    /// a opening number. This is not an entity.
    /// </summary>
    public class SewerOpeningNumber
    {
        public string FormattedNumber => string.Format("{0}-{1}", Prefix, Suffix);
        public int Suffix { get; set; }
        public string Prefix { get; set; }
    }
}
