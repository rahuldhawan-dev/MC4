using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using OfficeOpenXml;
using StructureMap;
using StructureMap.TypeRules;

namespace MapCallImporter.Common
{
    public abstract class ExcelFileHandlerFinderServiceBase
    {
        #region Properties

        private static Dictionary<Type, IEnumerable<Type>> _handlerTypes = new Dictionary<Type, IEnumerable<Type>>();

        public IContainer Container { get; }

        protected IEnumerable<Type> HandlerTypes
        {
            get
            {
                var thisType = GetType();
                if (!_handlerTypes.ContainsKey(thisType))
                {
                    _handlerTypes[thisType] = GetHandlerTypes();
                }
                return _handlerTypes[thisType];
            }
        }

        #endregion

        #region Abstract Properties

        public abstract Type HandlerType { get; }

        #endregion

        #region Constructors

        public ExcelFileHandlerFinderServiceBase(IContainer container)
        {
            Container = container;
        }

        #endregion

        #region Private Methods

        protected IEnumerable<Type> GetHandlerTypes()
        {
            foreach (var type in GetType().Assembly.GetClassesByCondition(t =>
                (t.Namespace ?? string.Empty).StartsWith("MapCallImporter.Models") &&
                !t.IsAbstract &&
                !t.IsNested &&
                !t.IsOpenGeneric() &&
                t.IsSubclassOfRawGeneric(typeof(ExcelRecordBase<,,>))))
            {
                Type baseType;

                for (baseType = type.BaseType; baseType.GetGenericTypeDefinition() != typeof(ExcelRecordBase<,,>); baseType = baseType.BaseType) {}

                yield return HandlerType.MakeGenericType(baseType.GenericTypeArguments);
            }
        }

        #endregion

        #region Exposed Methods

        // for validation
        public ExcelFileHandlerBase GetHandler(byte[] fileBytes)
        {
            using (var stream = new MemoryStream(fileBytes))
            using (var package = new ExcelPackage(stream))
            {
                foreach (var type in HandlerTypes)
                {
                    var name = type.GetGenericArguments()[0].Name;
                    var openingResult = default(ExcelFileProcessingResult);
                    var instance = (ExcelFileHandlerBase)Container.GetInstance(type);
                    var canHandle = instance.CanHandle(package, ref openingResult);

                    if ((!canHandle && openingResult != default(ExcelFileProcessingResult)) || canHandle)
                    {
                        // if the validator cannot handle the given file because the file type is invalid or
                        // the file is already open it won't matter which specific validator class we use, they're
                        // all going to return the same result
                        return instance;
                    }
                }

                return null;
            }
        }

        // for importing
        public ExcelFileHandlerBase GetHandler(ExcelFileValidationAndMappingResult lastResult)
        {
            if (lastResult.Entities == null || !lastResult.Entities.Any())
            {
                throw new InvalidOperationException("Last result had no entities, so no handler can be found");
            }

            var handler = HandlerTypes
                .Single(t => t.GenericTypeArguments[0] == lastResult.Entities.GetType().GenericTypeArguments[0] &&
                             t.GenericTypeArguments[2] == lastResult.RecordType);
            return (ExcelFileHandlerBase)Container.GetInstance(handler);
        }

        #endregion
    }
}