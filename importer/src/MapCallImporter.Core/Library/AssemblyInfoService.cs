using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MapCallImporter.Library
{
    public class AssemblyInfoService : IAssemblyInfoService
    {
        #region Properties

        public virtual Assembly Assembly { get; }

        public virtual string Title => GetThingFromMatchingAttribute<AssemblyTitleAttribute>(
            attr => attr.Title,
            attr => !string.IsNullOrWhiteSpace(attr.Title),
            () => Path.GetFileNameWithoutExtension(Assembly.CodeBase));

        public virtual string Version => FileVersionInfo.GetVersionInfo(Assembly.Location).ProductVersion;

        public virtual string Description =>
            GetThingFromMatchingAttribute<AssemblyDescriptionAttribute>(attr => attr.Description);

        public virtual string Product => GetThingFromMatchingAttribute<AssemblyProductAttribute>(attr => attr.Product);

        public virtual string Copyright =>
            GetThingFromMatchingAttribute<AssemblyCopyrightAttribute>(attr => attr.Copyright);

        public virtual string Company => GetThingFromMatchingAttribute<AssemblyCompanyAttribute>(attr => attr.Company);

        #endregion

        #region Constructors

        public AssemblyInfoService(Assembly assembly)
        {
            Assembly = assembly;
        }

        public AssemblyInfoService() : this(Assembly.GetExecutingAssembly()) {}

        #endregion

        #region Private Methods

        protected virtual string GetThingFromMatchingAttribute<TAttribute>(Func<TAttribute, string> thingFn, Func<TAttribute, bool> attributeMatchesP = null, Func<string> elseFn = null)
            where TAttribute : Attribute
        {
            var attributes = Assembly.GetCustomAttributes(typeof(TAttribute), false);
            attributeMatchesP = attributeMatchesP ?? (_ => true);
            elseFn = elseFn ?? (() => string.Empty);

            return attributes.Any() && attributes[0] is TAttribute attr && attributeMatchesP(attr)
                ? thingFn(attr)
                : elseFn();
        }

        #endregion
    }
}
