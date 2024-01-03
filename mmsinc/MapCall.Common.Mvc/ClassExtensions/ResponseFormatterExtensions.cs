using System;
using System.Web.Mvc;
using MMSINC.Results;

namespace MapCall.Common.ClassExtensions
{
    /// <summary>
    /// Extension methods for ResponseFormatter that are specific to MapCallMVC
    /// and not worth putting on the base class.
    /// </summary>
    public static class ResponseFormatterExtensions
    {
        #region Consts

        public const string MAP_ROUTE_EXTENSION = "map";

        #endregion

        #region Extensions

        /// <summary>
        /// Sets how a response should be formatted when being requested by the MapController.
        /// </summary>
        public static void Map(this ResponseFormatter formatter, Func<ActionResult> responder)
        {
            formatter.RespondTo(MAP_ROUTE_EXTENSION, responder);
        }

        #endregion
    }
}
