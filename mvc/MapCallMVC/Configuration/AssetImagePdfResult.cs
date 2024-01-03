using MapCall.Common.Model.Entities;
using MMSINC.Results;

namespace MapCallMVC.Configuration
{
    public class AssetImagePdfResult : PdfResult
    {
        #region Properties

        public IAssetImage Entity { get; set; }

        #endregion

        #region Constructor
        
        public AssetImagePdfResult(byte[] renderedPdf, IAssetImage entity) : base(renderedPdf)
        {
            Entity = entity;   
        }

        #endregion
    }
}