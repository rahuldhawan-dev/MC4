using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class GridViewRowCollectionWrapper : IGridViewRowCollection
    {
        #region Private Members

        private readonly GridViewRowCollection _innerColl;

        #endregion

        #region Properties

        public IGridViewRow this[int value]
        {
            get { return new GridViewRowWrapper(_innerColl[value]); }
        }

        public int Count
        {
            get { return _innerColl.Count; }
        }

        #endregion

        #region Constructors

        public GridViewRowCollectionWrapper(GridViewRowCollection coll)
        {
            _innerColl = coll;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<TRet> Collect<TRet>(Func<GridViewRow, bool> filter, Func<GridViewRow, TRet> transform)
        {
            foreach (GridViewRow row in _innerColl)
                if (filter(row))
                    yield return transform(row);
        }

        public IEnumerator<IGridViewRow> GetEnumerator()
        {
            return new WrappedGridViewRowEnumerator(this, _innerColl.Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    internal class WrappedGridViewRowEnumerator : IEnumerator<IGridViewRow>
    {
        #region Constants

        private const int BASE_POSITION = -1;

        #endregion

        #region Private Members

        private readonly IGridViewRowCollection _coll;
        private readonly int _collLength;
        private int _position = BASE_POSITION;

        #endregion

        #region Properties

        public IGridViewRow Current
        {
            get { return _coll[_position]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion

        #region Constructors

        internal WrappedGridViewRowEnumerator(IGridViewRowCollection coll, int length)
        {
            _coll = coll;
            _collLength = length;
        }

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            // noop
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _collLength);
        }

        public void Reset()
        {
            _position = BASE_POSITION;
        }

        #endregion
    }

    public interface IGridViewRowCollection : IEnumerable<IGridViewRow>
    {
        #region Properties

        int Count { get; }

        #endregion

        #region Methods

        IEnumerable<TRet> Collect<TRet>(Func<GridViewRow, bool> filter,
            Func<GridViewRow, TRet> transform);

        IGridViewRow this[int value] { get; }

        #endregion
    }
}
