using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Testing
{
    public abstract class FilesystemViewEngineBase : ViewEngineBase
    {
        protected string _basePath, _projectPath;

        /// <summary>
        /// Override in inheriting classes to provide the name of the solution folder,
        /// e.g. "mapcall_mvc".
        /// </summary>
        protected abstract string ProjectDirName { get; }

        /// <summary>
        /// Override in inheriting classes to provide the path from the solution folder
        /// to the web project, e.g. "MapCallMvc" or "src/SomeWebProject".
        /// </summary>
        protected abstract string WebProjectPath { get; }

        protected string BasePath => _basePath ?? (_basePath = GetBasePath());
        protected string ProjectPath => _projectPath ?? (_projectPath = Path.Combine(BasePath, WebProjectPath));

        protected string GetViewPath(ControllerContext context)
        {
            var resourceName = context.RouteData.Values["controller"].ToString();

            return context.RouteData.Values.ContainsKey("area")
                ? Path.Combine(ProjectPath, "Areas", context.RouteData.Values["area"].ToString(), "Views", resourceName)
                : Path.Combine(ProjectPath, "Views", resourceName);
        }

        protected string GetBasePath()
        {
            var path =
                Directory.GetCurrentDirectory().Split(new[] {ProjectDirName},
                    StringSplitOptions.None)[0];

            if (path.Contains("TestResults"))
            {
                path = Path.GetFullPath(Path.Combine(path, "..", "..", ".."));
            }
            if (path.Contains("everything"))
            {
                path = Path.GetFullPath(Path.Combine(path, ".."));
            }

            return Path.Combine(path, ProjectDirName);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName,
            bool useCache)
        {
            var path = GetViewPath(controllerContext);
            foreach (var ext in new[] {"", ".cshtml"})
            {
                var viewPath = Path.Combine(path, $"{partialViewName}{ext}");
                if (File.Exists(viewPath))
                {
                    return new ViewEngineResult(new RazorView(controllerContext, partialViewName, null, false, null),
                        this);
                }
            }

            if (ThrowIfViewIsNotRegistered)
            {
                throw new Exception($"Could not find partial view named {partialViewName} at path '{path}'.");
            }

            return new ViewEngineResult(Enumerable.Empty<string>());
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
            string masterName, bool useCache)
        {
            var path = GetViewPath(controllerContext);
            foreach (var ext in new[] {"", ".cshtml"})
            {
                var viewPath = Path.Combine(path, $"{viewName}{ext}");
                if (File.Exists(viewPath))
                {
                    return new ViewEngineResult(new RazorView(controllerContext, viewName, null, false, null), this);
                }
            }

            if (ThrowIfViewIsNotRegistered)
            {
                throw new Exception($"Could not find view named {viewName} at path '{path}'.");
            }

            return new ViewEngineResult(Enumerable.Empty<string>());
        }

        public override void ReleaseView(ControllerContext controllerContext, IView view) { }
    }
}
