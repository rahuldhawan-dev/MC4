using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MMSINC.Testing.Utilities;

namespace MMSINC.Testing
{
    public class FakeModelMetadataProviders : IStaticPropertyReplacer
    {
        #region Fields

        private ModelMetadataProvider _previousInstance;
        private ModelMetadataProvider _replacementInstance;

        private bool _isInit;

        #endregion

        #region Properties

        public ModelMetadataProvider ReplacementInstance
        {
            get { return _replacementInstance; }
            set
            {
                if (_isInit)
                {
                    throw new Exception("Can not set replacement instance after replacement has been initialized");
                }

                _replacementInstance = value;
            }
        }

        #endregion

        public void Dispose()
        {
            if (_isInit)
            {
                ModelMetadataProviders.Current = _previousInstance;
                _isInit = false;
            }
        }

        public void Init()
        {
            if (!_isInit)
            {
                _previousInstance = ModelMetadataProviders.Current;
                ModelMetadataProviders.Current = ReplacementInstance;
                _isInit = true;
            }
        }
    }
}
