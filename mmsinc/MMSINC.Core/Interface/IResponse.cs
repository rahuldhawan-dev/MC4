namespace MMSINC.Interface
{
    public interface IResponse
    {
        #region Properties

        IHttpCachePolicy Cache { get; }

        string Charset { get; set; }
        string ContentType { get; set; }
        bool IsClientConnected { get; }
        bool IsRequestBeingRedirected { get; }

        #endregion

        #region Methods

        void Redirect(string url);
        void Redirect(string url, bool endResponse);
        void Clear();
        void AddHeader(string headerName, string headerValue);
        void Write(string text);
        void End();
        void BinaryWrite(byte[] bytes);

        #endregion
    }
}
