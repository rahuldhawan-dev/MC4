using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Data;
using MMSINC.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Testing
{
    public static class MvcAssert
    {
        #region IsForbidden

        public static void IsForbidden(ActionResult result)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/Error/Forbidden", ((RedirectResult)result).Url);
        }

        #endregion

        #region IsNotFound

        /// <summary>
        /// Asserts that a ViewResult or PartialViewResult is a 404 Not Found result.
        /// </summary>
        /// <param name="result">The ActionResult to test.</param>
        /// <param name="expectedStatusDescription">Expect status description.</param>
        /// <param name="errorMessage">Error message to display if the test fails.</param>
        public static void IsNotFound(ActionResult result, string expectedStatusDescription = null,
            string errorMessage = null)
        {
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult), errorMessage);
            if (!String.IsNullOrWhiteSpace(expectedStatusDescription))
            {
                Assert.AreEqual(expectedStatusDescription, ((HttpNotFoundResult)result).StatusDescription,
                    errorMessage);
            }
        }

        #endregion

        #region IsPartialView

        /// <summary>
        /// Asserts that an ActionResult is a PartialViewResult. Mainly just shortcut for Assert.IsInstancef(PartialViewResult).
        /// </summary>
        public static void IsPartialView(ActionResult result, string errorMessage = null)
        {
            Assert.IsInstanceOfType(result, typeof(PartialViewResult), errorMessage);
        }

        /// <summary>
        /// Asserts that an ActionResult is a PartialViewResult. Mainly just shortcut for Assert.IsInstancef(PartialViewResult).
        /// </summary>
        public static void IsNotPartialView(ActionResult result, string errorMessage = null)
        {
            Assert.IsNotInstanceOfType(result, typeof(PartialViewResult), errorMessage);
        }

        #endregion

        #region IsStatusCode

        /// <summary>
        /// Asserts that an ActionResult is an HttpStatusCodeResult with the given values. Leave the status description null if it doesn't need to be
        /// checked. 
        /// </summary>
        public static void IsStatusCode(int expectedStatusCode, string expectedStatusDescription, ActionResult result,
            string message = null)
        {
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult), message);
            var hscr = (HttpStatusCodeResult)result;
            Assert.AreEqual(expectedStatusCode, hscr.StatusCode, message);
            if (expectedStatusDescription != null)
            {
                Assert.AreEqual(expectedStatusDescription, hscr.StatusDescription, message);
            }
        }

        /// <summary>
        /// Asserts that an ActionResult is an HttpStatusCodeResult with the given values. This overload does not
        /// check for the StatusDescription message!
        /// </summary>
        public static void IsStatusCode(int expectedStatusCode, ActionResult result, string message = null)
        {
            IsStatusCode(expectedStatusCode, null, result, message);
        }

        #endregion

        #region IsViewNamed

        public static void IsViewNamed(ActionResult result, string expectedName, string errorMessage = null)
        {
            // The null check is here to make to avoid figuring out where
            // nullref exceptions are if a null result gets passed. Also
            // it makes tests that use this a bit shorter since they
            // won't need to perform their own null check.
            Assert.IsNotNull(result, $"{errorMessage} ViewResult can't be null");
            Assert.IsInstanceOfType(result, typeof(ViewResultBase), errorMessage);

            var view = (ViewResultBase)result;

            // If result.ViewName is null/empty, that means the controller 
            // returned a view by just going View() or View(model).
            // If it does have a ViewName, then the controller returned
            // a view by going View("ViewName") or View("ViewName", model).
            // HOWEVER, at the moment we don't have controller tests that
            // simulate the whole request, so trying to check for ViewData["action"]
            // will always result in null unless we manually set it ourselves, which
            // would defeat the purpose of the test.
            // -Ross 5/2/2012
            //
            if (!string.IsNullOrWhiteSpace(view.ViewName))
            {
                Assert.AreEqual(expectedName,
                    view.ViewName,
                    $"{errorMessage} result.ViewName should be the name of the expected view if a view with a different name than the controller action is used.");
            }
            else
            {
                var viewName = (string)view.ViewData["action"];
                // NOTE: If/when we get a solid controller base test that goes
                //       through the whole request pipeline, we'll need to remove
                //       this null check as the ViewData["action"] should then be
                //       getting properly set.
                if (viewName != null)
                {
                    Assert.AreEqual(expectedName,
                        viewName,
                        $"{errorMessage} result.ViewData[\"action\"] should be the name of the expected view if a view with a different name than the controller action is used.");
                }
            }
        }

        #endregion

        #region IsViewWithModel

        /// <summary>
        /// Asserts that a ViewResultBase-derived result has the expected view model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expectedModel"></param>
        public static void IsViewWithModel(ActionResult result, object expectedModel, string errorMessage = null)
        {
            var viewModel = ((ViewResultBase)result).Model;
            if (expectedModel == null)
            {
                Assert.IsNull(viewModel, errorMessage);
            }
            else
            {
                Assert.AreSame(expectedModel, viewModel, $"{errorMessage} Expected model was not passed to the view.");
            }
        }

        /// <summary>
        /// Asserts that all the expected objects in an IEnumerable model exist.
        /// </summary>
        public static void IsViewWithEnumerableModel<T>(ActionResult result, IEnumerable<T> expectedModel)
        {
            var viewModel = ((ViewResultBase)result).Model;
            Assert.IsInstanceOfType(viewModel, typeof(IEnumerable<T>));
            var enumerableModel = (IEnumerable<T>)viewModel;
            Assert.AreEqual(expectedModel.Count(), enumerableModel.Count());

            var missing = expectedModel.Except(enumerableModel).Count();
            if (missing > 0)
            {
                Assert.Fail("{0}/{1} expected items are not contained in the result model", missing,
                    expectedModel.Count());
            }
        }

        #endregion

        #region IsViewWithNameAndModel

        /// <summary>
        /// Asserts that an ActionResult is a view or partial view result for the given view name and 
        /// includes the expected model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="viewName"></param>
        /// <param name="expectedModel"></param>
        public static void IsViewWithNameAndModel(ActionResult result, string viewName, object expectedModel,
            string errorMessage = null)
        {
            IsViewNamed(result, viewName, errorMessage);
            IsViewWithModel(result, expectedModel, errorMessage);
        }

        #endregion

        #region IsViewWithNameAndViewModelFor

        /// <summary>
        /// Asserts that an ActionResult is a view or a partial view result for the given view name and
        /// includes a ViewModelT for the given entity.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        public static void IsViewWithNameAndViewModelFor(ActionResult result, string viewName, object model)
        {
            IsViewNamed(result, viewName);
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));

            var viewModel = ((ViewResultBase)result).Model;
            var viewModelType = typeof(ViewModel<>).MakeGenericType(model.GetType());

            Assert.IsTrue(viewModelType.IsInstanceOfType(viewModel),
                "The view model '{0}' is not an instance of a type that inherits from 'ViewModel<{1}>'.", viewModel,
                model.GetType());
        }

        #endregion

        #region RedirectsTo

        /// <summary>
        /// Asserts that a RedirectResult was returned for a given url. Only use this
        /// if you're redirecting a url string rather than redirecting to a route.
        /// </summary>
        public static void RedirectsToUrl(ActionResult result, string expectedUrl, string errorMessage = null)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectResult), errorMessage);
            var redir = (RedirectResult)result;
            Assert.AreEqual(expectedUrl, redir.Url, errorMessage);
        }

        /// <inheritdoc cref="RedirectsToUrl(System.Web.Mvc.ActionResult,string,string)" />
        /// <remarks>
        /// This implementation allows the expected query parameters to be passed separately as a
        /// <see cref="NameValueCollection"/> rather than being tacked onto the end of the expected url.
        /// This means the assertion won't fail because of the ordering of query string parameters which
        /// can vary.
        /// </remarks>
        public static void RedirectsToUrl(
            ActionResult result,
            string expectedBaseUrl,
            NameValueCollection expectedQueryParams,
            string errorMessage = null)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectResult), errorMessage);

            var redir = (RedirectResult)result;
            var splitUrl = redir.Url.Split('?');
            
            var actualBaseUrl = splitUrl[0];
            Assert.AreEqual(expectedBaseUrl, actualBaseUrl);

            var actualQueryParams = HttpUtility.ParseQueryString(splitUrl[1]);

            foreach (var param in expectedQueryParams.AllKeys)
            {
                Assert.AreEqual(
                    expectedQueryParams[param],
                    actualQueryParams[param],
                    $"Value for parameter '{param}' did not match");
            }
        }

        public static void RedirectsToRoute(ActionResult result, object routeParams = null, string errorMessage = null)
        {
            RedirectsToRoute(result, null, null, routeParams, errorMessage);
        }

        public static void RedirectsToRoute(ActionResult result, string action, object routeParams = null,
            string errorMessage = null)
        {
            RedirectsToRoute(result, null, action, routeParams, errorMessage);
        }

        public static void RedirectsToRoute(ActionResult result, string controller, string action,
            object routeParams = null, string errorMessage = null)
        {
            var dictArgs = new Dictionary<string, object>();

            if (routeParams != null)
            {
                var props =
                    routeParams.GetType()
                               .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var p in props)
                {
                    dictArgs.Add(p.Name, p.GetValue(routeParams, null));
                }
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                dictArgs["action"] = action;
            }

            if (!string.IsNullOrWhiteSpace(controller))
            {
                dictArgs["controller"] = controller;
            }

            RedirectsToRoute(result, dictArgs, errorMessage);
        }

        private static void RedirectsToRoute(ActionResult result, Dictionary<string, object> routeParams,
            string errorMessage)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), errorMessage);
            var redirectResult = (RedirectToRouteResult)result;

            foreach (var routeValue in routeParams)
            {
                Assert.IsTrue(redirectResult.RouteValues.ContainsKey(routeValue.Key),
                    $"{errorMessage}. RedirectToRouteResult does not contain key '{routeValue.Key}'.");
                var expected = routeValue.Value;
                var actual = redirectResult.RouteValues[routeValue.Key];
                Assert.AreEqual(expected, actual,
                    $"{errorMessage}. Errored on key '{routeValue.Key}'. Expected \"{expected}\" but got \"{actual}\".");
            }
        }

        #endregion

        #region RespondsWith

        public static void RespondsWithExcelResult(ActionResult result)
        {
            Assert.IsInstanceOfType(result, typeof(ExcelResult));
        }

        #endregion

        #region AssertHasDropDownData<TData>(ControllerBase)

        public static void AssertHasDropDownData<TData>(this Controllers.ControllerBase target,
            IEnumerable<TData> expected, Func<TData, object> keyGetter, Func<TData, object> valueGetter,
            string key = null)
        {
            var typeName = typeof(TData).Name;
            key = key ?? typeName;
            var expectedColl = SelectListItemConverter.Convert(expected, keyGetter, valueGetter);

            Assert.IsNotNull(target.ViewData[key], "ViewData key {0} was not found.", key);
            var actual = (IEnumerable<SelectListItem>)target.ViewData[key];

            foreach (var item in expectedColl)
            {
                Assert.IsTrue(actual.Any(i => i.Value == item.Value && i.Text == item.Text),
                    "Drop down data for key '{0}' did not contain the expected item.", key);
            }
        }

        public static void AssertDoesNotHaveDropDownData<TData>(this Controllers.ControllerBase target,
            IEnumerable<TData> expected, Func<TData, object> keyGetter, Func<TData, object> valueGetter,
            string key = null)
        {
            var typeName = typeof(TData).Name;
            key = key ?? typeName;
            var unExpectedColl = SelectListItemConverter.Convert(expected, keyGetter, valueGetter);
           
            Assert.IsNotNull(target.ViewData[key], "ViewData key {0} was not found.", key);
            var actual = (IEnumerable<SelectListItem>)target.ViewData[key];

            foreach (var item in unExpectedColl)
            {
                Assert.IsFalse(actual.Contains(item), "Drop down data for key '{0}' contained the unexpected item.",
                    key);
            }
        }

        #endregion

        #region Assert Temp Data

        public static void AssertTempDataContainsMessage(this Controller result, string message, string messageKey)
        {
            Assert.IsTrue(
                ((IList<string>)result.TempData[messageKey]).Any(x => x == message),
                $"The message: \"{message}\" was not found in the view result collection for the message key: {messageKey}");
        }

        #endregion
    }

    public static class MvcApplicationExtensions
    {
        #region TestRoute

        public static MvcApplication TestRoute(this MvcApplication application, string url, dynamic expectedRouteValues,
            string message = null)
        {
            var routeData = RegisterAndGetRouteDataForUrl(application, url);

            try
            {
                Assert.IsNotNull(routeData, "Failure getting route data.");

                var props = (IEnumerable<PropertyInfo>)
                    ObjectExtensions.GetAllPublicGetters(expectedRouteValues);

                if (!props.Any())
                {
                    Assert.AreEqual(0, routeData.Values.Count);
                }
                else
                {
                    props.Each(prop =>
                        Assert.AreEqual(
                            prop.GetValue(expectedRouteValues, null),
                            routeData.Values[prop.Name]));
                }
            }
            catch (AssertFailedException e)
            {
                throw (message == null)
                    ? e
                    : new AssertFailedException(message, e);
            }

            return application;
        }

        public static MvcApplication TestRouteInvalid(this MvcApplication application, string url,
            string message = null)
        {
            try
            {
                Assert.IsNull(RegisterAndGetRouteDataForUrl(application, url));
            }
            catch (AssertFailedException e)
            {
                throw (message == null)
                    ? e
                    : new AssertFailedException(message, e);
            }

            return application;
        }

        public static MvcApplication TestRouteIgnored(this MvcApplication application, string url,
            string message = null)
        {
            var routeData = RegisterAndGetRouteDataForUrl(application, url);

            try
            {
                Assert.IsNotNull(routeData);
                Assert.IsInstanceOfType(routeData.RouteHandler,
                    typeof(StopRoutingHandler));
            }
            catch (AssertFailedException e)
            {
                throw (message == null)
                    ? e
                    : new AssertFailedException(message, e);
            }

            return application;
        }

        #region Helper Methods

        private static RouteData RegisterAndGetRouteDataForUrl(MvcApplication application,
            string url)
        {
            var routes = new RouteCollection();
            application.RegisterRoutes(routes);
            return routes.GetRouteData(
                new StubHttpContextForRouting(requestUrl: url));
        }

        #endregion

        #endregion
    }
}
