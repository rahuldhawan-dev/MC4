using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Interface;
using MapCall.Common.Utility;
using StructureMap;
using HttpContextWrapper = MMSINC.Common.HttpContextWrapper;

//using MapCall.Common.Utility;

namespace MapCall.Common.Web
{
    // Required for MapCall:
    // To make this work locally, each file extension needs to be registered with IIS on the dev machine.
    // Just adding it to web.config doesn't work. 
    // 1. Open IIS and go to the MapCall directory.
    // 2. Right click and go to properties
    // 3. Click "Configure" in Virtual Directory tab.
    // 4. Add each extension:
    //      a. Executable: C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll
    //      b. Extension name is just ".js"(or whatever). No wildcards.
    //      c. Uncheck "Check that file exists"

    // NOTE: ResourceHandler's ResourceManager instance needs to be static for performance reasons. ResourceManager
    // creation is somewhat expensive in that it has to parse a whole ton of crap every time it's created. Having to
    // create a new ResourceManager for every single request will be a big sad-face. 

    public class ResourceHandler : IHttpHandler
    {
        #region Helper classes

        public enum StreamReaderType
        {
            StreamReader,
            BinaryReader,
        }

        public sealed class ContentHelper
        {
            public readonly string ContentType;
            public readonly StreamReaderType StreamReaderType;

            public ContentHelper(string contentType, StreamReaderType srt)
            {
                ContentType = contentType;
                StreamReaderType = srt;
            }
        }

        #endregion

        #region Fields

        private static readonly IDictionary<string, ContentHelper> _extensionContentTypes =
            new Dictionary<string, ContentHelper>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ResourceManager used by all ResourceHandler instances. 
        /// </summary>
        public static IResourceManager ResourceManager { get; private set; }

        // IHttpHandler
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        #region Constructors

        static ResourceHandler()
        {
            _extensionContentTypes[".css"] = new ContentHelper("text/css", StreamReaderType.StreamReader);
            _extensionContentTypes[".ico"] = new ContentHelper("image/x-icon", StreamReaderType.BinaryReader);
            _extensionContentTypes[".js"] =
                new ContentHelper("application/x-javascript", StreamReaderType.StreamReader);
            _extensionContentTypes[".png"] =
                new ContentHelper("application/octet-stream", StreamReaderType.BinaryReader);
            _extensionContentTypes[".gif"] =
                new ContentHelper("application/octet-stream", StreamReaderType.BinaryReader);
        }

        #endregion

        #region Private Methods

        internal static void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException("resourceManager");
            }

            ResourceManager = resourceManager;
        }

        internal virtual IHttpContext GetIHttpContext(HttpContext context)
        {
            return new HttpContextWrapper(DependencyResolver.Current.GetService<IContainer>(), context);
        }

        internal virtual void ProcessEmbeddedResourceRequest(IHttpContext icontext, string path)
        {
            //  System.Diagnostics.Debug.Print("Path: " + path);
            var ext = Path.GetExtension(path);
            var helper = _extensionContentTypes[ext];
            WriteResourceToResponse(helper, icontext.Response, path);

            icontext.Response.ContentType = helper.ContentType;
        }

        internal virtual void DeferRequestToDefaultHandler(HttpContext context)
        {
            // This is needed to process the request normally if the path refers to a
            // non-embedded resource. However, I don't like it. However, again, I don't
            // want to deal with handling all the internal file not found and whatever
            // other errors that can popup myself.

            // NOTE: Do not try/catch this. It needs to throw exceptions so everything
            // else works as expected. IE: FileNotFoundException gets handled elsewhere
            // and properly returns a 404. 

            var def = new DefaultHttpHandler();
            context.Handler = def; // Dunno if this is needed ever.
            try
            {
                def.EndProcessRequest(def.BeginProcessRequest(context, null, null));
            }
            catch (HttpException ex)
            {
                throw new HttpException(ex.Message + String.Format(" {0}", context.Request.Url), ex);
            }
        }

        internal static object GetResource(StreamReaderType srt, string resourcePath)
        {
            var appCache = HttpRuntime.Cache;
            var cachedResource = appCache[resourcePath];

            if (cachedResource == null)
            {
                // It's assumed that the resource exists by the time this call is made.

                using (var s = ResourceManager.GetStreamByVirtualPath(resourcePath))
                {
                    switch (srt)
                    {
                        case StreamReaderType.BinaryReader:
                            using (var br = new BinaryReader(s))
                            {
                                // Not worrying about this int conversion unless
                                // we start embedding resources that are over 2MB in size.
                                cachedResource = br.ReadBytes((int)s.Length);
                            }

                            break;

                        case StreamReaderType.StreamReader:
                            using (var sr = new StreamReader(s))
                            {
                                cachedResource = sr.ReadToEnd();
                            }

                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }

                appCache.Add(resourcePath, cachedResource, null, DateTime.Now.AddDays(1.0), TimeSpan.Zero,
                    CacheItemPriority.Default, null);
            }

            return cachedResource;
        }

        internal virtual void WriteResourceToResponse(ContentHelper helper, IResponse response, string resourcePath)
        {
            var resource = GetResource(helper.StreamReaderType, resourcePath);

            switch (helper.StreamReaderType)
            {
                case StreamReaderType.BinaryReader:

                    response.BinaryWrite((byte[])resource);
                    break;

                case StreamReaderType.StreamReader:
                    response.Write((string)resource);
                    break;
            }
        }

        #endregion

        #region Public Methods

        // IHttpHandler
        public void ProcessRequest(HttpContext context)
        {
            var icontext = GetIHttpContext(context);

            var path = icontext.Request.Uri.AbsolutePath;

            if (ResourceManager.HasResource(path))
            {
                ProcessEmbeddedResourceRequest(icontext, path);

                icontext.Response.Cache.SetExpires(DateTime.Now.AddHours(1));

                //   context.Response.Cache.SetExpires(DateTime.Now.AddHours(1));
            }
            else
            {
                DeferRequestToDefaultHandler(context);
            }
        }

        #endregion
    }
}
