using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public interface IParser<TEntity>
    {
        #region Abstract Methods

        TEntity Parse(IMailMessage message);

        #endregion
    }
}
