using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IObjectDataSource : IControl
    {
        #region Properties

        ParameterCollection SelectParameters { get; }
        ParameterCollection UpdateParameters { get; }

        #endregion

        #region Methods

        int Insert();
        int Update();

        void SetDefaultSelectParameterValue(string parameterName,
            string defaultValue);

        #endregion
    }
}
