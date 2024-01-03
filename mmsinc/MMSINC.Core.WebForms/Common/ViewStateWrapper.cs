using System.Web.UI;

namespace MMSINC.Common
{
    public class ViewStateWrapper : IViewState
    {
        #region Private Members

        private readonly StateBag _stateBag;

        #endregion

        #region Properties

        public object this[string key]
        {
            get { return _stateBag[key]; }
            set { _stateBag[key] = value; }
        }

        #endregion

        #region Constructors

        public ViewStateWrapper(StateBag stateBag)
        {
            _stateBag = stateBag;
        }

        #endregion

        #region Exposed Methods

        public object GetValue(string key)
        {
            return this[key];
        }

        public void SetValue(string key, object value)
        {
            this[key] = value;
        }

        public bool HasKey(string key)
        {
            return this[key] != null;
        }

        #endregion
    }

    public interface IViewState
    {
        #region Properties

        object this[string key] { get; set; }

        #endregion

        #region Exposed Methods

        object GetValue(string key);

        void SetValue(string key, object value);

        bool HasKey(string key);

        #endregion
    }
}
