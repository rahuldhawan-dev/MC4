using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.Utilities;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Model.Repositories;
using StructureMap;
using Module = MapCall.Common.Model.Entities.Module;

namespace MapCall.Common.Utility.Permissions
{
    internal static class ModulePermissionsHelper
    {
        internal static void InitType(Type classType)
        {
            var appName = GetApplicationNameFromType(classType);
            foreach (var fi in GetIModulePermissionFields(classType))
            {
                var formattedName = FormatName(fi.Name);
                var p = new ModulePermissions(appName, formattedName);
                SetPermissionsOnField(fi, p);
            }
        }

        internal static void SetPermissionsOnField(FieldInfo fi, IModulePermissions perms)
        {
            // If the field already has been initialized(aka it's not null) then don't reset it. 
            // This will make it so we can override fields if need be.
            if (fi.GetValue(null) != null)
            {
                return;
            }

            fi.SetValue(null, perms);
        }

        #region Name Formatting

        internal static string FormatName(string name)
        {
            const string wordSpacer = " ";
            var words = Wordify.GetWordsFromCamelCase(name);
            return string.Join(wordSpacer, words);
        }

        internal static string GetApplicationNameFromType(Type classType)
        {
            var attr = (from a in classType.GetCustomAttributes(typeof(RoleApplicationNameAttribute), false)
                        select a).SingleOrDefault();

            if (attr != null)
            {
                return ((RoleApplicationNameAttribute)attr).Name;
            }

            return FormatName(classType.Name);
        }

        #endregion

        #region Reflection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        internal static IEnumerable<FieldInfo> GetIModulePermissionFields(Type classType)
        {
            var fields =
                classType.GetFields(BindingFlags.Static | BindingFlags.Public);

            // In case any of these static classes get public fields that aren't
            // IModulePermissions, we want to ignore them.
            return (from f in fields
                    where f.FieldType == typeof(IModulePermissions)
                    select f);
        }

        internal static Type GetTypeByName(string fullyQualifiedTypeName)
        {
            var t = Type.GetType(fullyQualifiedTypeName);
            if (t == null)
            {
                throw new TypeLoadException("Type not found: " + fullyQualifiedTypeName);
            }

            return t;
        }

        #endregion

        /// <summary>
        /// Returns the IModulePermissions object that matches the passed in name. The name must be in 
        /// the format of Application.Module(ApplicationType.ModPermissionFieldName).
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        internal static IModulePermissions GetModulePermissionsByName(string fullName)
        {
            var names = GetApplicationAndModuleNames(fullName);
            var appTypeName = names.Item1;
            var modFieldName = names.Item2;
            var fullTypeName = "MapCall.Common.Utility.Permissions.Modules." + appTypeName;

            var t = GetTypeByName(fullTypeName);

            var fields = GetIModulePermissionFields(t);

            foreach (var f in fields)
            {
                if (f.Name == modFieldName)
                {
                    return (IModulePermissions)f.GetValue(null);
                }
            }

            throw new InvalidOperationException(
                "FindModulePermissionsByName could not find the field('" +
                modFieldName + "') in type '" + fullTypeName + "'");
        }

        internal static Tuple<string, string> GetApplicationAndModuleNames(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new NullReferenceException(
                    "GetApplicationAndModuleNames must be passed a non-null or empty value.");
            }

            var spliterino = fullName.Split('.');
            const int EXPECTED_SPLIT_LENGTH = 2;

            if (spliterino.Length != EXPECTED_SPLIT_LENGTH)
            {
                throw new InvalidOperationException(
                    "GetApplicationAndModuleNames must be given a fullName in the format of Application.Module(ApplicationType.ModPermissionFieldName).");
            }

            var appTypeName = spliterino[0];
            var modFieldname = spliterino[1];
            return new Tuple<string, string>(appTypeName, modFieldname);
        }
    }

    /// <summary>
    /// Optional attribute to throw on top of the various permissions static classes if the 
    /// name of the RoleApplication can't be generated automatically. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RoleApplicationNameAttribute : Attribute
    {
        #region Properties

        public string Name { get; set; }

        #endregion

        #region Constructors

        public RoleApplicationNameAttribute(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new NullReferenceException(
                    "RoleApplicationNameAttribute constructor requires a non-null or empty name");
            }

            Name = appName;
        }

        #endregion
    }

    public static class IModulePermissionsExtensions
    {
        #region Extension Methods

        public static Module ToModule(this IModulePermissions modulePermissions)
        {
            return DependencyResolver.Current.GetService<IModuleRepository>()
                                     .FindByApplicationAndModuleName(
                                          modulePermissions.Application, modulePermissions.Module);
        }

        #endregion
    }
}
