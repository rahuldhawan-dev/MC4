using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using DeleporterCore.Client;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using NHibernate;
using StructureMap;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public class SearchPageSteps
    {
        #region Private Methods

        private static void SelectSearchOptions(NameValueCollection nvc, string typeName)
        {
            var townType = typeof(Town);
            var modelAssembly = townType.Assembly;
            var type = modelAssembly.GetType(townType.Namespace + "." + typeName);
            type.ThrowIfNull("type", String.Format("Could not find type '{0}'.", typeName));

            foreach (var key in nvc.AllKeys)
            {
                SelectSearchOption(key, nvc[key], type, modelAssembly);
            }
        }

        private static string GetStringValueForNamedTypeThingy(string typeName, string rawValue)
        {
            if (!TestObjectCache.Instance.ContainsKey(typeName))
            {
                return null;
            }
            dynamic value = TestObjectCache.Instance[typeName][rawValue];
            Deleporter.Run(() => {
                value = DependencyResolver.Current.GetService<ISession>().Load(value.GetType(), value.Id);
                value = ObjectExtensions.HasPublicProperty(value, "Description") ? value.Description : value.ToString();
            });
            return value.ToString();
        }

        private static void SelectSearchOption(string key, string rawValue, Type modelType, Assembly modelAssembly)
        {
            var propName = key.ToPascalCase();
            var propInfo = modelType.GetProperty(propName);
            string value;

            if (propInfo != null)
            {
                if (propInfo.PropertyType.Assembly == modelAssembly)
                {
                    value = GetStringValueForNamedTypeThingy(propInfo.PropertyType.Name.ToLowerSpaceCase(), rawValue);

                    Input.SelectTextInField(propName, value);
                }
                else if (propInfo.PropertyType == typeof(string))
                {
                    Input.TypeValueInField(propName, rawValue);
                }
                return;
            }

            value = GetStringValueForNamedTypeThingy(key, rawValue);

            if (value != null)
            {
                Input.SelectTextInField(propName, value);
            }
        }

        #endregion

        #region Exposed Methods

        [When(@"^I search for ([^\s]+) with (.+) chosen$")]
        [Given(@"^I have searched for ([^\s]+) with (.+) chosen$")]
        public void WhenISearchForBappTeamIdeasWithNoConditionsChosen(string controller, string options)
        {
            var singular = controller.Singularize();
            Navigation.GivenIAmAtAPage(singular + "/Search");

            if (options != "no conditions")
            {
                SelectSearchOptions(MMSINC.Testing.SpecFlow.StepDefinitions.Data.ConvertToNameValueCollection(options), singular);
            }

            Input.WhenIPressAButton("Search");
        }

        #endregion
    }
}
