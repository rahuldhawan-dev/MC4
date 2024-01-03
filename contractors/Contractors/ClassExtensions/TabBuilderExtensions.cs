using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Helpers;

namespace Contractors.ClassExtensions
{
    public static class TabBuilderExtensions
    {
        public static TabBuilder WithDocuments(this TabBuilder builder, object partialModel)
        {
            int? docCount = null;
            if (partialModel is IThingWithDocuments thingWithDocs)
            {
                docCount = thingWithDocs.LinkedDocuments.Count;
            }
            return builder.WithTab("Documents", "~/Views/Shared/Document/Index.cshtml", partialModel, itemCount: docCount);
        }

        public static TabBuilder WithDocuments(this TabBuilder builder)
        {
            int? docCount = null;
            if (builder.HtmlHelper.ViewData.Model is IThingWithDocuments thingWithDocs)
            {
                docCount = thingWithDocs.LinkedDocuments.Count;
            }
            return builder.WithTab("Documents", "~/Views/Shared/Document/Index.cshtml", itemCount: docCount);
        }

        public static TabBuilder WithNotes(this TabBuilder builder)
        {
            int? noteCount = null;
            if (builder.HtmlHelper.ViewData.Model is IThingWithNotes
                thingWithNotes)
            {
                noteCount = thingWithNotes.LinkedNotes.Count;
            }
            return builder.WithTab("Notes", "~/Views/Shared/Note/Index.cshtml", itemCount:noteCount);
        }

    }
}