using MMSINC.Testing;

namespace MapCallMVC.Tests
{
    public class MapCallMvcViewEngine : FilesystemViewEngineBase
    {
        protected override string ProjectDirName => "mvc";
        protected override string WebProjectPath => "MapCallMvc";
    }
}
