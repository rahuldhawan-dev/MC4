using System;

namespace MMSINC.Data
{
    [Serializable]
    public abstract class DisplayItem<TEntity> : IDisplayItem<TEntity>
    {
        #region Properties

        public virtual int Id { get; set; }

        #endregion

        #region Abstract Properties

        public abstract string Display { get; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    public interface IDisplayItem
    {
        #region Abstract Properties

        int Id { get; set; }
        string Display { get; }

        #endregion
    }

    public interface IDisplayItem<TEntity> : IDisplayItem { }
}
