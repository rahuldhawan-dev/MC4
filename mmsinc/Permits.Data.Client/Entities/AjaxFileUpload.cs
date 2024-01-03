namespace Permits.Data.Client.Entities
{
    // Mimics the MMSINC.Core.Mvc AjaxFileUpload class so we don't have to reference the whole dll.
    public class AjaxFileUpload
    {
        #region Properties

        public byte[] BinaryData { get; set; }
        public string FileName { get; set; }

        #endregion
    }
}
