using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.Data
{
    public class EntityMapperException : Exception
    {
        #region Consts

        internal const string CAN_NOT_INITIALIZE_MORE_THAN_ONCE =
            "Can not initialize a BaseMapAttribute-derived class more than once.";

        #endregion

        #region Constructor

        private EntityMapperException(string message, Exception innerException = null) :
            base(message, innerException) { }

        #endregion

        #region Public Methods

        public static EntityMapperException CanNotFindRepositoryType(string repositoryEntityTypeName)
        {
            const string format =
                "Unable to find a repository registered with StructureMap that implements IRepository<{0}>.";
            return new EntityMapperException(string.Format(format, repositoryEntityTypeName));
        }

        public static EntityMapperException EntityMapRequiresViewModelPropertyToBeIntOrNullableInt(string property,
            string type)
        {
            const string format =
                "EntityMapAttribute requires the ViewModel property be of type int or nullable int. Property: {0}, Type: {1}.";
            return new EntityMapperException(string.Format(format, property, type));
        }

        public static EntityMapperException RepositoryMustImplementIRepository(string fullTypeName)
        {
            const string format = "RepositoryType \"{0}\" must implement IRepository.";
            return new EntityMapperException(string.Format(format, fullTypeName));
        }

        public static EntityMapperException TooManyRepositoryTypeMatches(string repositoryEntityTypeName,
            IEnumerable<Type> registeredTypes)
        {
            const string format =
                "There are multiple types registered with StructureMap that implement IRepository<{0}>. Set the Repository property on the EntityMapAttribute to the specific repository type. Types found: {1}";
            var registered = string.Join(", ", registeredTypes.Select(x => x.FullName));
            return new EntityMapperException(string.Format(format, repositoryEntityTypeName, registered));
        }

        #endregion
    }
}
