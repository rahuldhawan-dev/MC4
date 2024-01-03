namespace MMSINC.Metadata
{
    public static class ModelFormatterProviders
    {
        #region Fields

        private static ModelFormatterProvider _current = new ModelFormatterProvider();

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the current ModelFormatterProvider for an app.
        /// </summary>
        public static ModelFormatterProvider Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ModelFormatterProvider();
                }

                return _current;
            }
            set { _current = value; }
        }

        #endregion
    }
}
