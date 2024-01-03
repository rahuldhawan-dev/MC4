using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MMSINC.Testing.DesignPatterns;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.Common.WebFormsTest.Utility
{
    /// <summary>
    /// Summary description for ResourceVirtualDirectoryTest
    /// </summary>
    [TestClass]
    public class ResourceVirtualDirectoryTest
    {
        #region Fields

        private Mock<IResourceManager> _mockResourceManager = new Mock<IResourceManager>();

        #endregion

        [TestInitialize]
        public void ResourceVirtualDirectoryTestInitialize()
        {
            // Override needed so HostingEnvironment.ApplicationVirtualPath doesn't throw
            // an exception since this test isn't running in a web app. 
            //  ResourceManager.ApplicationVirtualPathRoot = "/";

            _mockResourceManager.Setup(x => x.ApplicationVirtualPathRoot).Returns("/");

            var listOfResourceNames = new List<string>();
            listOfResourceNames.Add("MapCall.Common.Resources.scripts.jquery.js");
            listOfResourceNames.Add("MapCall.Common.Resources.scripts.jquery.min.js");
            listOfResourceNames.Add("MapCall.Common.Resources.scripts.choochoo.train");

            _mockResourceManager.Setup(x => x.ResourceNames).Returns(listOfResourceNames);

            _mockResourceManager.Setup(x =>
                                     x.ConvertPathToResourceName("/resources/"))
                                .Returns("MapCall.Common.Resources");

            _mockResourceManager.Setup(x =>
                                     x.ConvertPathToResourceName("/resources/scripts/"))
                                .Returns("MapCall.Common.Resources.scripts");
        }

        private TestResourceVirtualDirectoryBuilder InitializeBuilder()
        {
            var xmlString = "<dir name=\"Resources\"><dir name=\"scripts\" /></dir>";
            var node = CreateNode(xmlString);

            return new TestResourceVirtualDirectoryBuilder()
                  .WithResourceManager(_mockResourceManager.Object)
                  .WithNode(node)
                  .WithVirtualPath("/resources/");
        }

        private static XmlNode CreateNode(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.FirstChild;
        }

        #region Initialization tests

        [TestMethod]
        public void TestInitializeChildrenAddsDirectories()
        {
            var target = InitializeBuilder()
                        .WithVirtualPath("/resources")
                        .WithTestInitializeFilesCall(true)
                        .Build();

            target.InitializeForReal();

            var dirs = target.DirectoriesTest;
            Assert.IsTrue(dirs.Count() == 1);

            var scriptDir = dirs.First();
            Assert.IsNotNull(scriptDir, "How did a null item slip in to the Directories list?");
            Assert.IsTrue(scriptDir.VirtualPath == "/resources/scripts/");

            Assert.IsTrue(target.ChildrenTest.Contains(scriptDir));
        }

        [TestMethod]
        public void TestInitializeFilesAddsFiles()
        {
            var target = InitializeBuilder().Build();

            target.InitializeForReal();

            Assert.IsTrue(target.FilesTest.Any());

            foreach (var file in target.FilesTest)
            {
                Assert.IsNotNull(file, "How did a null file get in there?");
                Assert.IsTrue(target.ChildrenTest.Contains(file), "Children collection is missing file.");
            }
        }

        #endregion

        #region Method tests

        [TestMethod]
        public void TestGetAllDirectoriesAddsAllOfItsChildDirectoriesToList()
        {
            var target = InitializeBuilder().Build();

            target.InitializeForReal();

            var expectedDirs = new List<ResourceVirtualDirectory>();
            GetAllChildDirsRecursive(expectedDirs, target);

            var foundDirs = new List<ResourceVirtualDirectory>();

            target.GetAllChildDirectories(foundDirs);

            Assert.IsTrue(foundDirs.Any(), "No child dirs were added.");
            Assert.IsFalse(foundDirs.Except(expectedDirs).Any());
        }

        private static void GetAllChildDirsRecursive(IList<ResourceVirtualDirectory> dirs, ResourceVirtualDirectory dir)
        {
            foreach (ResourceVirtualDirectory childDir in dir.Directories)
            {
                GetAllChildDirsRecursive(dirs, childDir);
                dirs.Add(childDir);
            }
        }

        [TestMethod]
        public void TestGetAllFilesAddsAllOfItsChildFilesToList()
        {
            var target = InitializeBuilder().Build();

            target.InitializeForReal();

            var expectedFiles = new List<ResourceVirtualFile>();
            GetAllChildFilesRecursive(expectedFiles, target);

            var foundFiles = new List<ResourceVirtualFile>();

            target.GetAllFiles(foundFiles);

            Assert.IsTrue(foundFiles.Any(), "No child files were added.");
            Assert.IsFalse(foundFiles.Except(expectedFiles).Any());
        }

        private static void GetAllChildFilesRecursive(IList<ResourceVirtualFile> files, ResourceVirtualDirectory dir)
        {
            foreach (ResourceVirtualDirectory childDir in dir.Directories)
            {
                GetAllChildFilesRecursive(files, childDir);
            }

            foreach (ResourceVirtualFile file in dir.Files)
            {
                files.Add(file);
            }
        }

        [TestMethod]
        public void FindFileReturnsFileForEveryFileTheDirectoryOrItsChildrenContains()
        {
            var target = InitializeBuilder().Build();

            target.InitializeForReal();

            var expectedFiles = new List<ResourceVirtualFile>();
            GetAllChildFilesRecursive(expectedFiles, target);

            Assert.IsTrue(expectedFiles.Any(), "No files returned for setup.");

            foreach (var file in expectedFiles)
            {
                Assert.IsNotNull(target.FindFile(file.VirtualPath));
            }
        }

        #endregion

        #region Property tests

        [TestMethod]
        public void TestDirectoriesPropertyDoesNotReturnNull()
        {
            Assert.IsNotNull(InitializeBuilder().Build().Directories);
        }

        [TestMethod]
        public void TestFilesPropertyDoesNotReturnNull()
        {
            Assert.IsNotNull(InitializeBuilder().Build().Files);
        }

        [TestMethod]
        public void TestChildrenPropertyDoesNotReturnNull()
        {
            Assert.IsNotNull(InitializeBuilder().Build().Children);
        }

        [TestMethod]
        public void TestChildrenPropertyOnlyContainsValuesFromFilesAndDirectoriesProperty()
        {
            var target = InitializeBuilder()
                        .WithTestInitializeFilesCall(true)
                        .Build();

            target.InitializeForReal();

            // Adding the Files and Directories into one collection, which is what the 
            // Children property should contain. 
            var expectedChildren =
                target.FilesTest.Cast<object>().Union(target.DirectoriesTest.Cast<object>());

            var difference = target.ChildrenTest.Except(expectedChildren);

            Assert.IsFalse(difference.Any(),
                "Children property contains an item not found in either Directories or Files properties.");
        }

        #endregion
    }

    public class TestResourceVirtualDirectory : ResourceVirtualDirectory
    {
        #region Fields

        private XmlNode _node;

        #endregion

        #region Properties

        public bool UseTestInitializeCall { get; set; }
        public bool UseTestInitChildrenCall { get; set; }
        public bool UseTestInitFilesCall { get; set; }

        // These are here to simplify having to cast the base properties
        // since they only return IEnumerable. 

        public IEnumerable<ResourceVirtualDirectory> DirectoriesTest
        {
            get { return Directories.Cast<ResourceVirtualDirectory>(); }
        }

        public IEnumerable<ResourceVirtualFile> FilesTest
        {
            get { return Files.Cast<ResourceVirtualFile>(); }
        }

        public IEnumerable<object> ChildrenTest
        {
            get { return Children.Cast<object>(); }
        }

        #endregion

        #region Constructors

        public TestResourceVirtualDirectory(string virtualPath, XmlNode node, IResourceManager ownerManager)
            : base(virtualPath, node, ownerManager)
        {
            _node = node;
        }

        #endregion

        #region Private methods

        protected override void Initialize(XmlNode node)
        {
            // Do nothing here. 
        }

        public void InitializeForReal()
        {
            base.Initialize(_node);
        }

        protected override void InitializeChildren(XmlNode node)
        {
            if (!UseTestInitChildrenCall)
            {
                base.InitializeChildren(node);
            }
        }

        protected override void InitializeFiles()
        {
            if (!UseTestInitFilesCall)
            {
                base.InitializeFiles();
            }
        }

        #endregion
    }

    internal class TestResourceVirtualDirectoryBuilder : TestDataBuilder<TestResourceVirtualDirectory>
    {
        #region Private Members

        private XmlNode _node;
        private string _virtualPath;
        private bool _withTestInitChildrenCall;
        private bool _withTestInitFilesCall;
        private IResourceManager _withOwningManager;

        #endregion

        #region Exposed Methods

        public TestResourceVirtualDirectoryBuilder WithResourceManager(IResourceManager man)
        {
            _withOwningManager = man;
            return this;
        }

        public TestResourceVirtualDirectoryBuilder WithTestInitializeChildrenCall(bool b)
        {
            _withTestInitChildrenCall = b;
            return this;
        }

        public TestResourceVirtualDirectoryBuilder WithTestInitializeFilesCall(bool b)
        {
            _withTestInitFilesCall = b;
            return this;
        }

        public TestResourceVirtualDirectoryBuilder WithVirtualPath(string vp)
        {
            _virtualPath = vp;
            return this;
        }

        public TestResourceVirtualDirectoryBuilder WithNode(XmlNode node)
        {
            _node = node;
            return this;
        }

        public override TestResourceVirtualDirectory Build()
        {
            var obj = new TestResourceVirtualDirectory(_virtualPath, _node, _withOwningManager);
            obj.UseTestInitChildrenCall = _withTestInitChildrenCall;
            obj.UseTestInitFilesCall = _withTestInitFilesCall;
            return obj;
        }

        #endregion
    }
}
