namespace MMSINC.DataPages
{
    public interface IControlState
    {
        #region Methods

        void Add<T>(string key, T value, T defaultValue);
        T Get<T>(string key);
        object GetControlStateObject();
        void LoadControlState(object state);

        #endregion
    }
}
