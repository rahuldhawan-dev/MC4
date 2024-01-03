using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Utility
{
    /// <summary>
    /// Summary description for ResourceManagerTest
    /// </summary>
    [TestClass]
    public class ResourceManagerTest
    {
        #region Constants

        public const string EXPECTED_BASE_NAMESPACE = "MapCall.Common";

        #endregion

        #region Fields

        private Mock<IResourceConfiguration> _mockConfig = new Mock<IResourceConfiguration>();
        private List<string> _expectedResourceNames = new List<string>();

        #endregion

        [TestInitialize]
        public void TestResourceManagerInitialize()
        {
            _expectedResourceNames.Add(EXPECTED_BASE_NAMESPACE + ".Stuff.For.Things.png");
            _expectedResourceNames.Add(EXPECTED_BASE_NAMESPACE + ".Stuff.For.OtherThings.js");
            _expectedResourceNames.Add(EXPECTED_BASE_NAMESPACE + ".Resources.scripts.jquery.js");
            _expectedResourceNames.Add(EXPECTED_BASE_NAMESPACE + ".Dir_With_A_Space.file.ext");

            _mockConfig.Setup(x => x.ConfigurationResourceName).Returns(EXPECTED_BASE_NAMESPACE + ".Some.Path.xml");
        }

        private TestResourceManagerBuilder Initialize()
        {
            return new TestResourceManagerBuilder()
                  .WithResourceNames(_expectedResourceNames)
                  .WithResourceConfigDocument(CreateFakeResourceConfig())
                  .WithApplicationVirtualPathRoot("/");
        }

        private static XmlDocument CreateFakeResourceConfig()
        {
            var config =
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <gofigure>
                  <virtualDirectories>
                    <dir name=""Stuff"">
                        <dir name=""For"">
                           <dir name=""For"" />
                        </dir>
                    </dir>
                    <dir name=""Resources"">
                      <dir name=""scripts"" />
                    </dir>
                    <dir name=""Dir With A Space"" />
                  </virtualDirectories>
                </gofigure>";

            return CreateXmlDoc(config);
        }

        private static XmlDocument CreateXmlDoc(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        #region Tests

        [TestMethod]
        public void TestAssemblyNameIsCorrect()
        {
            Assert.AreEqual(EXPECTED_BASE_NAMESPACE, Initialize().Build().BaseNamespace);
        }

        [TestMethod]
        public void TestAssemblyPropertyReturnsCorrectAssembly()
        {
            var expectedAssembly = typeof(ResourceManager).Assembly;
            var target = Initialize().Build();

            Assert.AreSame(target.Assembly, expectedAssembly);
        }

        [TestMethod]
        public void TestCreateDictionaryUsesStringComparerOrdinalIgnoreCase()
        {
            var result = ResourceManager.CreateDictionary<object>();
            Assert.AreSame(StringComparer.OrdinalIgnoreCase, result.Comparer);
        }

        [TestMethod]
        public void TestResourceNamesPropertyIncludesValuesFromGetUsableResourceNames()
        {
            var target = Initialize().Build();

            Assert.IsTrue(target.ResourceNames.Any());

            var diff = target.ResourceNames.Except(_expectedResourceNames);
            Assert.IsFalse(diff.Any());
        }

        [TestMethod]
        public void TestGetXmlResourceThrowsIfStreamIsNull()
        {
            var target = Initialize()
               .Build();
            MyAssert.Throws(() => target.GetXmlResource("some.resource"), typeof(FileNotFoundException));
        }

        [TestMethod]
        public void GetResourceConfigDocumentCallsGetXmlResourceForConfigurationResourceName()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            try
            {
                target.BaseGetResourceConfigDocument();
            }
            catch (FileNotFoundException)
            {
                // This is gonna throw because I'm calling the base methods to test these
                // sort of things. But it will throw after having set the LastGetXmlResourceArgument
                // property. 
            }

            var expectedResourceName = target.SiteConfiguration.ConfigurationResourceName;

            Assert.AreEqual(target.LastGetXmlResourceArgument, expectedResourceName);
        }

        [TestMethod]
        public void TestCreateXmlReaderUsesIgnoreCommentsSetting()
        {
            var xml = "<something></something>";
            var xmlStream = new MemoryStream(Encoding.Default.GetBytes(xml));

            var result = ResourceManager.CreateXmlReader(xmlStream);
            Assert.IsTrue(result.Settings.IgnoreComments);
        }

        [TestMethod]
        public void TestAllFilesPropertyHasAllFiles()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            var resultResourceNames = (from f in target.AllFiles
                                       select f.ResourceName);

            var diff = resultResourceNames.Except(_expectedResourceNames);
            Assert.IsFalse(diff.Any());
        }

        #region GetVirtualDirectoryNodes tests

        [TestMethod]
        public void TestGetVirtualDirectoryNodesThrowsIfPassedNullArgument()
        {
            MyAssert.Throws(() => Initialize().Build().GetVirtualDirectoryNodes(null));
        }

        [TestMethod]
        public void TestGetVirtualDirectoryNodesThrowsIfVirtualDirectoriesNodeIsMissing()
        {
            var config = @"<?xml version=""1.0"" encoding=""utf-8"" ?><gofigure></gofigure>";
            var doc = CreateXmlDoc(config);
            var target = Initialize().Build();

            MyAssert.Throws(() => target.GetVirtualDirectoryNodes(doc));
        }

        #endregion

        #region ConvertPathToResourceName tests

        [TestMethod]
        public void TestConvertPathToResourceNameReturnsExpectedResult()
        {
            var target = Initialize().Build();

            var test1 = "~/some/path/file.ext";
            var expected1 = EXPECTED_BASE_NAMESPACE + ".some.path.file.ext";
            var result1 = target.ConvertPathToResourceName(test1);
            Assert.AreEqual(expected1, result1);

            var test2 = "/some/path/file.ext";
            var expected2 = EXPECTED_BASE_NAMESPACE + ".some.path.file.ext";
            var result2 = target.ConvertPathToResourceName(test2);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void TestConvertPathToResourceNameReturnsCaseSensitiveVersionIfExists()
        {
            var target = Initialize().Build();

            var test = "/stuff/for/otherthings.js";
            var expected = EXPECTED_BASE_NAMESPACE + ".Stuff.For.OtherThings.js";
            var result = target.ConvertPathToResourceName(test);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConvertPathToResourceNameDealsWithSpacesInDirectoriesAndFileNamesProperly()
        {
            var target = Initialize().Build();

            var test1 = "~/some/path with a space/file with a space.ext";
            var expected1 = EXPECTED_BASE_NAMESPACE + ".some.path_with_a_space.file with a space.ext";
            var result1 = target.ConvertPathToResourceName(test1);
            Assert.AreEqual(expected1, result1);
        }

        [TestMethod]
        public void TestConvertPathToResourceNameDealsWithSpacesInNestedDirectoriesAndFileNamesProperly()
        {
            var target = Initialize().Build();

            var test1 = "~/some/path with a space/another path/file with a space.ext";
            var expected1 = EXPECTED_BASE_NAMESPACE + ".some.path_with_a_space.another_path.file with a space.ext";
            var result1 = target.ConvertPathToResourceName(test1);
            Assert.AreEqual(expected1, result1);
        }

        #endregion

        #region Initialization tests

        [TestMethod]
        public void TestInitializeConfigurationThrowsIfConfigIsNull()
        {
            var target = Initialize().Build();
            MyAssert.Throws(() => target.InitializeConfiguration(null));
        }

        [TestMethod]
        public void TestInitializeConfigurationSetsSiteConfigurationToGivenConfig()
        {
            var target = Initialize().Build();
            var expectedConfig = _mockConfig.Object;

            target.InitializeConfiguration(expectedConfig);

            Assert.AreSame(target.SiteConfiguration, expectedConfig);
        }

        #endregion

        #region HasResource tests

        [TestMethod]
        public void TestHasResourceReturnsTrueWhenGivenValidResourcePath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            Assert.IsTrue(target.HasResource("/stuff/for/otherthings.js"));
        }

        [TestMethod]
        public void TestHasResourceReturnsFalseWhenGivenInvalidResourcePath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            Assert.IsFalse(target.HasResource("/uh"));
        }

        #endregion

        #region ResourceFileExists tests

        [TestMethod]
        public void TestResourceFileExistsReturnsTrueForValidPath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            Assert.IsTrue(target.ResourceFileExists("/stuff/for/otherthings.js"));
        }

        [TestMethod]
        public void TestResourceFileExistsReturnsFalseForInvalidPath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            Assert.IsFalse(target.ResourceFileExists("/stuff/for/idontexist.js"));
        }

        #endregion

        #region ResourceDirectoryExists tests

        [TestMethod]
        public void TestResourceDirectoryExistsReturnsTrueForValidPath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            Assert.IsTrue(target.ResourceDirectoryExists("/stuff/for/"));
        }

        [TestMethod]
        public void TestResourceDirectoryExistsReturnsFalseForInvalidPath()
        {
            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);
            Assert.IsFalse(target.ResourceDirectoryExists("/i/am/not/real/"));
        }

        #endregion

        #region GetResourceDirectory tests

        [TestMethod]
        public void TestGetResourceDirectoryReturnsResourceVirtualDirectoryIfExists()
        {
            var expectedFilePath = "/Stuff/For/";

            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            var result = target.GetResourceDirectory(expectedFilePath);

            Assert.IsNotNull(result);

            Assert.AreEqual(result.VirtualPath, expectedFilePath);
        }

        [TestMethod]
        public void TestGetResourceDirectoryReturnsNullIfDoesNotExist()
        {
            var expectedFilePath = "/Stuff/NotHere/";

            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            var result = target.GetResourceDirectory(expectedFilePath);

            Assert.IsNull(result);
        }

        #endregion

        #region GetResourceFile tests

        [TestMethod]
        public void TestGetResourceFileReturnsResourceVirtualFileIfExists()
        {
            var expectedFilePath = "/Stuff/For/OtherThings.js";

            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            var result = target.GetResourceFile(expectedFilePath);

            Assert.IsNotNull(result);

            Assert.AreEqual(result.VirtualPath, expectedFilePath);
        }

        [TestMethod]
        public void TestGetResourceFileReturnsNullIfDoesNotExist()
        {
            var expectedFilePath = "/Stuff/For/nothere.js";

            var target = Initialize().Build();
            target.InitializeConfiguration(_mockConfig.Object);

            var result = target.GetResourceFile(expectedFilePath);

            Assert.IsNull(result);
        }

        #endregion

        #region Getting streams

        // Since the whole purpose of this class is to get streams from the embedded
        // files in an assembly, I'm going to go ahead and actually test that while I'm testing
        // these methods. I guess I could make a Mock assembly but that is just going overboard.

        [TestMethod]
        public void TestGetStreamByResourceNameReturnsEmbeddedAssemblyStream()
        {
            var expectedResourceName = EXPECTED_BASE_NAMESPACE + ".Resources.resourceconfig.xml";
            var expectedStream = typeof(ResourceManager).Assembly.GetManifestResourceStream(expectedResourceName);
            Assert.IsNotNull(expectedStream, "resourceconfig.xml not found.");

            var target = Initialize().Build();
            var result = target.GetStreamByResourceName(EXPECTED_BASE_NAMESPACE + ".Resources.resourceconfig.xml");
            Assert.IsNotNull(result, "GetStreamByResourceName returned null.");

            Assert.AreEqual(result.Length, expectedStream.Length,
                "Mismatched streams. Streams have different Lengths.");

            using (var resultReader = new StreamReader(result))
            using (var expectedReader = new StreamReader(expectedStream))
            {
                while (!expectedReader.EndOfStream)
                {
                    Assert.AreEqual(expectedReader.Read(), resultReader.Read());
                }
            }
        }

        [TestMethod]
        public void TestGetStreamByVirtualPathReturnsExpectedStream()
        {
            var expectedResourceName = EXPECTED_BASE_NAMESPACE + ".Resources.resourceconfig.xml";
            var expectedStream = typeof(ResourceManager).Assembly.GetManifestResourceStream(expectedResourceName);
            Assert.IsNotNull(expectedStream, "resourceconfig.xml not found.");

            var target = Initialize().Build();
            var result = target.GetStreamByVirtualPath("~/Resources/resourceconfig.xml");
            Assert.IsNotNull(result, "GetStreamByVirtualPath returned null.");

            Assert.AreEqual(result.Length, expectedStream.Length,
                "Mismatched streams. Streams have different Lengths.");

            using (var resultReader = new StreamReader(result))
            using (var expectedReader = new StreamReader(expectedStream))
            {
                while (!expectedReader.EndOfStream)
                {
                    Assert.AreEqual(expectedReader.Read(), resultReader.Read());
                }
            }
        }

        [TestMethod]
        public void TestGetStreamByResourceFileThrowsIfNullArgument()
        {
            var target = Initialize().Build();
            MyAssert.Throws(() => target.GetStreamByResourceFile(null));
        }

        [TestMethod]
        public void TestGetStreamByResourceFileReturnsExpectedStream()
        {
            var expectedResourceName = EXPECTED_BASE_NAMESPACE + ".Resources.resourceconfig.xml";
            var expectedStream = typeof(ResourceManager).Assembly.GetManifestResourceStream(expectedResourceName);
            Assert.IsNotNull(expectedStream, "resourceconfig.xml not found.");

            var target = Initialize().Build();
            var testFile = new ResourceVirtualFile("/Resources/resourceconfig.xml", target);

            var result = target.GetStreamByResourceFile(testFile);
            Assert.IsNotNull(result, "GetStreamByVirtualPath returned null.");
            Assert.IsTrue(expectedStream.Length > 0);
            Assert.IsTrue(result.Length > 0);
            Assert.AreEqual(result.Length, expectedStream.Length,
                "Mismatched streams. Streams have different Lengths.");

            using (var resultReader = new StreamReader(result))
            using (var expectedReader = new StreamReader(expectedStream))
            {
                while (!expectedReader.EndOfStream)
                {
                    Assert.AreEqual(expectedReader.Read(), resultReader.Read());
                }
            }
        }

        #endregion

        #endregion
    }

    public class TestResourceManager : ResourceManager
    {
        #region Properties

        public IEnumerable<string> TestResourceNames { get; set; }
        public bool UseTestGetStreamByResourceName { get; set; }
        public XmlDocument TestResourceConfigDoc { get; set; }
        public string LastGetXmlResourceArgument { get; set; }

        #endregion

        public override Stream GetStreamByResourceName(string resourceName)
        {
            if (!UseTestGetStreamByResourceName)
            {
                return base.GetStreamByResourceName(resourceName);
            }

            return null;
        }

        protected override IEnumerable<string> GetUsableResourceNames()
        {
            if (TestResourceNames == null)
            {
                return new List<string>();
            }

            return TestResourceNames;
        }

        public override XmlDocument GetResourceConfigDocument()
        {
            return TestResourceConfigDoc;
        }

        public XmlDocument BaseGetResourceConfigDocument()
        {
            return base.GetResourceConfigDocument();
        }

        internal override XmlDocument GetXmlResource(string resourceName)
        {
            LastGetXmlResourceArgument = resourceName;
            return base.GetXmlResource(resourceName);
        }
    }

    internal class TestResourceManagerBuilder : TestDataBuilder<TestResourceManager>
    {
        private string _appRoot;
        private IEnumerable<string> _resourceNames;
        private bool _withTestGetResourceStreamByName;
        private XmlDocument _resourceDoc;

        #region Methods

        public TestResourceManagerBuilder WithApplicationVirtualPathRoot(string path)
        {
            _appRoot = path;
            return this;
        }

        public TestResourceManagerBuilder WithResourceNames(IEnumerable<string> names)
        {
            _resourceNames = names;
            return this;
        }

        public TestResourceManagerBuilder WithResourceConfigDocument(XmlDocument doc)
        {
            _resourceDoc = doc;
            return this;
        }

        public TestResourceManagerBuilder WithTestGetResourceStreamByName(bool b)
        {
            _withTestGetResourceStreamByName = b;
            return this;
        }

        public override TestResourceManager Build()
        {
            var test = new TestResourceManager();
            test.ApplicationVirtualPathRoot = _appRoot;
            test.UseTestGetStreamByResourceName = _withTestGetResourceStreamByName;
            test.TestResourceConfigDoc = _resourceDoc;

            if (_resourceNames != null)
            {
                test.TestResourceNames = _resourceNames;
                test.BuildResourceNamesDictionary();
            }

            return test;
        }

        #endregion
    }
}
