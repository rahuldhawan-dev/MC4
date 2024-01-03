using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MMSINC.Testing.Utilities;

namespace MMSINC.Testing
{
    public class FakeFilterProviders : IStaticPropertyReplacer
    {
        #region Fields

        private readonly StaticPropertyReplacer<FilterProviderCollection> _filterProvidersReplacer;
        private readonly StaticPropertyReplacer<GlobalFilterCollection> _globalFiltersReplacer;
        private bool _isInit;

        #endregion

        #region Properties

        public GlobalFilterCollection GlobalFilters
        {
            get { return _globalFiltersReplacer.ReplacementInstance; }
        }

        #endregion

        #region Constructor

        public FakeFilterProviders()
        {
            var um = FilterProviders.Providers;

            _filterProvidersReplacer = new StaticPropertyReplacer<FilterProviderCollection>(typeof(FilterProviders),
                "Providers");
            _globalFiltersReplacer =
                new StaticPropertyReplacer<GlobalFilterCollection>(typeof(GlobalFilters), "Filters");
        }

        #endregion

        public void Dispose()
        {
            _filterProvidersReplacer.Dispose();
            _globalFiltersReplacer.Dispose();
        }

        public void Init()
        {
            if (!_isInit)
            {
                // The static constructor for FilterProviders automatically adds in the existing
                // instance of GlobalFilter.Filters and a few other things. We wanna swap out the
                // existing GlobalFilter.Filters with our own and keep whatever else is in there.
                // For this to work properly, we need to init FilterProvider BEFORE GlobalFilters.
                _filterProvidersReplacer.Init();
                _globalFiltersReplacer.Init();

                foreach (var f in _filterProvidersReplacer.PreviousInstance.ToArray())
                {
                    if (f == _globalFiltersReplacer.PreviousInstance)
                    {
                        _filterProvidersReplacer.ReplacementInstance.Add(_globalFiltersReplacer.ReplacementInstance);
                    }
                    else
                    {
                        _filterProvidersReplacer.ReplacementInstance.Add(f);
                    }
                }

                _isInit = true;
            }
        }
    }
}
