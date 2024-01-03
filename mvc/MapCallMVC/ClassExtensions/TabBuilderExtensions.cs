using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Helpers;

namespace MapCallMVC.ClassExtensions
{
    public static class TabBuilderExtensions
    {
        public static TabBuilder WithNotes(this TabBuilder builder)
        {
            int? noteCount = null;
            var thingWithNotes = builder.HtmlHelper.ViewData.Model as IThingWithNotes;
            if (thingWithNotes != null)
            {
                noteCount = thingWithNotes.LinkedNotes.Count;
            }
            return builder.WithTab("Notes", "~/Views/Shared/Note/Index.cshtml", itemCount: noteCount);
        }

        public static TabBuilder WithDocuments(this TabBuilder builder, object partialModel = null)
        {
            var model = new SecureSearchDocumentForSingleEntity();
            int? docCount = null;

            if (partialModel != null)
            {
                if (partialModel is IThingWithDocuments thingWithDocs)
                {
                    docCount = thingWithDocs.LinkedDocuments.Count;
                    model.UserCanEdit = builder.HtmlHelper.CurrentUserCanEdit();
                    model.LinkedId = thingWithDocs.Id;
                    model.TableName = thingWithDocs.TableName;
                }
            }
            else
            {
                // Where are we calling WithDocuments where the model *wouldn't* be IThingWithDocuments? 
                var thingWithDocs = builder.HtmlHelper.ViewData.Model as IThingWithDocuments;
                if (thingWithDocs != null)
                {
                    docCount = thingWithDocs.LinkedDocuments.Count;
                    model.UserCanEdit = builder.HtmlHelper.CurrentUserCanEdit();
                    model.LinkedId = thingWithDocs.Id;
                    model.TableName = thingWithDocs.TableName;
                }
            }

            return builder.WithAjaxTab("Documents", "~/Views/Shared/Document/Index.cshtml", model, itemCount: docCount, updateTargetId: "document-results");
        }

        public static TabBuilder WithEmployees(this TabBuilder builder)
        {
            return builder.WithTab("Employees", "~/Views/Shared/Employee/Index.cshtml");
        }

        public static TabBuilder WithEmployees(this TabBuilder builder, string tabName, string dataTypeName, bool readOnly = false)
        {
            return builder.WithTab(tabName,
                readOnly ? "~/Views/Shared/Employee/ReadOnlyIndex.cshtml" : "~/Views/Shared/Employee/Index.cshtml",
                extraData: new ViewDataDictionary {{"DataTypeName", dataTypeName}});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="tabName"></param>
        /// <param name="dataTypeName"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        /// <param name="opCenter"></param>
        /// <param name="readOnly">If true, no security lookup is done and the read-only view is rendered. Useful for overriding inside the view based on model values.</param>
        /// <returns></returns>
        public static TabBuilder WithEmployees(this TabBuilder builder, IRoleService roleService, string tabName, string dataTypeName, RoleModules module, RoleActions action = RoleActions.Read, OperatingCenter opCenter = null, bool readOnly = false)
        {
            if (!readOnly)
            {
                readOnly = !roleService.CanAccessRole(module, action, opCenter);
            }

            return builder.WithEmployees(tabName, dataTypeName, readOnly);
        }

        /// <summary>
        /// Adds the audit log tab for the current view's model.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TabBuilder WithLog(this TabBuilder builder)
        {
            // NOTE: Do not add role/admin authentication in here. 
            // Call IsVisible after WithLog and supply the logic there.
            var tabModel = new SecureSearchAuditLogEntryForSingleRecord();
            var actualModel = builder.HtmlHelper.ViewData.ModelMetadata.Model;

            // Sometimes entities are returned as nhibernate proxy objects instead. We need to
            // correct the type name in those instances in order to correctly query for them.
            tabModel.EntityTypeName = NHibernate.Proxy.NHibernateProxyHelper.GuessClass(actualModel).Name;
            tabModel.ControllerName = builder.HtmlHelper.ViewContext.RouteData.Values["controller"]?.ToString();

            // Is this throwing an error about not being able to access the Id property even 
            // though the object definitely has an Id property? Make sure the class is public.
            tabModel.EntityId = actualModel.AsDynamic().Id;
            return builder.WithAjaxTab("Log", "~/Views/Shared/AuditLogEntry/Index.cshtml", tabModel);
        }

        // <summary>
        // Adds the action items ta for the current view's model
        //</summary>
        public static TabBuilder WithActionItems(this TabBuilder builder)
        {
            int? actionItemCount = null;
            var thingWithActionItems = builder.HtmlHelper.ViewData.Model as IThingWithActionItems;
            if (thingWithActionItems != null)
            {
                actionItemCount = thingWithActionItems.LinkedActionItems.Count;
            }
            return builder.WithAjaxTab("Action Items", "~/Views/Shared/ActionItem/Index.cshtml", itemCount: actionItemCount);
        }

        /// <summary>
        /// Adds the Notes and Documents tabs to a TabBuilder instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TabBuilder WithNotesAndDocuments(this TabBuilder builder)
        {
            return builder.WithNotes().WithDocuments();
        }

        public static TabBuilder WithNotesDocumentsAndEmployees(this TabBuilder builder)
        {
            return builder.WithNotesAndDocuments().WithEmployees();
        }

        public static TabBuilder WithNotesDocumentsAndActionItems(this TabBuilder builder)
        {
            return builder.WithNotesAndDocuments().WithActionItems();
        }

        public static TabBuilder WithVideos(this TabBuilder builder)
        {
            int? docCount = null;
            var thingWithVids = builder.HtmlHelper.ViewData.Model as IThingWithVideos;
            if (thingWithVids != null)
            {
                docCount = thingWithVids.LinkedVideos.Count();
            }
            return builder.WithTab("Videos", "~/Views/Video/Index.cshtml", itemCount: docCount);
        }

        public static TabBuilder WithWhoHasAccess(this TabBuilder builder)
        {
            return builder.WithTab("Who Has Access", "~/Views/Shared/UsersWithAccess/Index.cshtml");
        }
    }
}