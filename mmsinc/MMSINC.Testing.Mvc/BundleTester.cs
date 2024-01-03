using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Optimization;
using MMSINC.Bundling;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Testing
{
    public class BundleTester<TApp> where TApp : MvcApplication, new()
    {
        #region Fields

        private MvcApplicationTester<TApp> _appTester;
        private StaticVirtualPathProvider _staticProvider;
        private EmbeddedVirtualPathProvider _embedVpp;

        #endregion

        #region Properties

        public string StaticFileAbsoluteRoot
        {
            get { return _staticProvider.AbsoluteRoot; }
            set { _staticProvider.AbsoluteRoot = value; }
        }

        #endregion

        #region Constructor

        public BundleTester(MvcApplicationTester<TApp> appTester)
        {
            _appTester = appTester;
            _staticProvider = new StaticVirtualPathProvider();
            _embedVpp = new EmbeddedVirtualPathProvider();

            var composite = new CompositeVirtualPathProvider();
            composite.Providers.Add(_staticProvider);
            composite.Providers.Add(_embedVpp);

            BundleTable.VirtualPathProvider = composite;
            BundleTable.EnableOptimizations = true;
            CssUrlTransform.UseVirtualPathUtility = false;
        }

        #endregion

        #region Private Methods

        private void TestBundle(Bundle bundle, BundleCollection bundles)
        {
            var request = _appTester.CreateRequestHandler();
            var bundleContext = new BundleContext(request.HttpContext.Object, bundles, bundle.Path);
            var bundleFiles = bundle.EnumerateFiles(bundleContext).ToArray();

            // TODO: figure out how to also do this for StyleBundle
            if (bundle is ContentBundle)
            {
                BundleHasAllIncludedFiles((ContentBundle)bundle, bundleFiles);
            }

            var bundleContent = bundle.Builder.BuildBundleContent(bundle, bundleContext, bundleFiles);

            // We get to call each transform manually because Bundle's ApplyTransforms method always
            // calls DefaultTransform.Instance.Process(which isn't in the Transforms property) which
            // throws lots of stupid VirtualPath errors.

            var bundleResponse = new BundleResponse(bundleContent, bundleFiles);

            foreach (var tform in bundle.Transforms)
            {
                tform.Process(bundleContext, bundleResponse);
            }

            Assert.AreNotEqual(string.Empty, bundleResponse.Content);
        }

        private void BundleHasAllIncludedFiles(ContentBundle bundle, IEnumerable<BundleFile> bundleFiles)
        {
            var expectedFiles = bundle.IncludedFiles.Select(s => s.ToLower()).ToArray();
            var resultFiles = bundleFiles.Select(x => x.IncludedVirtualPath.ToLower()).ToArray();

            foreach (var file in expectedFiles)
            {
                Assert.IsTrue(resultFiles.Contains(file),
                    "Unable to find \"{0}\" in the enumerated files for the bundle \"{1}\" using root path \"{2}\", in path \"{3}\".",
                    file, bundle.Path, StaticFileAbsoluteRoot,
                    Directory.GetCurrentDirectory());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Asserts that the bundles created by MvcApplication.RegisterBundles are all valid.
        /// </summary>
        public void BundlesAreValidForApplication()
        {
            // Neeed to do this because the app tester doesn't mock out the BundleTable. 
            var bundles = BundleTable.Bundles;
            bundles.Clear();
            _appTester.ApplicationInstance.RegisterBundles(bundles, _embedVpp);

            if (!bundles.Any())
            {
                Assert.Fail("There aren't any registered bundles.");
            }

            foreach (var b in bundles)
            {
                if (b is ContentBundle || b is StyleBundle)
                {
                    TestBundle(b, bundles);
                }
                else
                {
                    Assert.Fail(
                        "The bundle for \"{0}\" must inherit from ContentBundle or StyleBundle in order for this test to pass.",
                        b.Path);
                }
            }
        }

        #endregion
    }
}
