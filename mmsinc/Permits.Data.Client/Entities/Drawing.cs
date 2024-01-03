namespace Permits.Data.Client.Entities
{
    public class Drawing : IDrawing
    {
        #region Properties

        public int Id { get; set; }
        public int PermitId { get; set; }
        public AjaxFileUpload FileUpload { get; set; }

        #endregion
    }

    public interface IDrawing
    {
        #region Abstract Properties

        int Id { get; set; }
        int PermitId { get; set; }
        AjaxFileUpload FileUpload { get; set; }

        #endregion
    }
}
