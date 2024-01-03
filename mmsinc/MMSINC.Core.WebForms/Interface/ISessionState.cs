namespace MMSINC.Interface
{
    public interface ISessionState
    {
        #region Properties

        object this[int val] { get; set; }
        object this[string val] { get; set; }

        #endregion

        #region Exposed Methods

        void Remove(string s);

        #endregion
    }
}
