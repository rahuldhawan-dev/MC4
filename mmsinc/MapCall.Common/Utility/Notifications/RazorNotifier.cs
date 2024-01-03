using MapCall.Common.Model.Entities;
using StructureMap;
using System.IO;
using System.Reflection;
using RazorEngine.Templating;

namespace MapCall.Common.Utility.Notifications
{
    public class RazorNotifier : NotifierBase
    {
        #region Constants

        public const string BASE_TEMPLATE_PATH = "MapCall.Common.Resources.NotificationTemplates",
                            CONTROL_PATH_FORMAT = BASE_TEMPLATE_PATH + ".{0}.{1}.cshtml",
                            MAIL_SUBJECT_FORMAT = "MapCall Notification - {0}",
                            CACHE_NAME = "MapCall Notifications";

        #endregion

        #region Private Members

        private Assembly _assembly;
        protected readonly ITemplateService _templateService;

        #endregion

        #region Properties

        public virtual Assembly Assembly => _assembly ?? (_assembly = GetType().Assembly);

        #endregion

        #region Constructors

        public RazorNotifier(IContainer container, ITemplateService templateService) : base(container)
        {
            _templateService = templateService;
        }

        #endregion

        #region Private Methods

        protected override string LoadNotification(RoleApplications application, RoleModules module, string templateName,
            object data)
        {
            string template;
            var path = BuildControlPath(application, module, templateName);

            using (var stream = Assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                template = reader.ReadToEnd();
            }

            return _templateService.Parse(template, data, null, templateName);
        }

        protected virtual string BuildControlPath(RoleApplications application, RoleModules module, string templateName)
        {
            return string.Format(CONTROL_PATH_FORMAT,
                module.ToString().Replace(application.ToString(), application + "."),
                templateName.Replace(" ", ""));
        }

        #endregion
    }
}
