using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    /// <summary>
    /// Base factory for entities that are lookups but where descriptions and indices are significant (AssetType, WorkDescription, etc.)
    /// For when you care about the values, otherwise use UniqueEntityLookupFactory
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TFactory"></typeparam>
    public abstract class StaticListEntityLookupFactory<TEntity, TFactory> : TestDataFactory<TEntity>
        where TEntity : class, new()
        where TFactory : TestDataFactory<TEntity>
    {
        /* NOTE:
         * this will help you generate some of the nonsense that goes into this:

select wd.Description,
	'public class ' + replace(dbo.CapitalizeFirstLetter(lower(replace(replace(replace(replace(replace(replace(replace(wd.Description, '&', 'AND'), '.', ''), '[', ''), ']', ''), ',', ''), '-', ' '), '/', ' '))), ' ', '') + 'WorkDescriptionFactory : WorkDescriptionFactory' + CHAR(13) + CHAR(10) +
	'{' + CHAR(13) + CHAR(10) +
	'    public ' + replace(dbo.CapitalizeFirstLetter(lower(replace(replace(replace(replace(replace(replace(replace(wd.Description, '&', 'AND'), '.', ''), '[', ''), ']', ''), ',', ''), '-', ' '), '/', ' '))), ' ', '') + 'WorkDescriptionFactory(IContainer container) : base(container) {}' + CHAR(13) + CHAR(10) +
	'    static ' + replace(dbo.CapitalizeFirstLetter(lower(replace(replace(replace(replace(replace(replace(replace(wd.Description, '&', 'AND'), '.', ''), '[', ''), ']', ''), ',', ''), '-', ' '), '/', ' '))), ' ', '') + 'WorkDescriptionFactory()' + CHAR(13) + CHAR(10) +
	'    {' + CHAR(13) + CHAR(10) +
	'        Defaults(new {Description = "' + wd.Description + '", AssetType = typeof(' + replace(replace(at.Description, ' ', ''), '/', '') + 'AssetTypeFactory)});' + CHAR(13) + CHAR(10) +
	'        OnSaving((a, s) => a.SetPropertyValueByName("Id", (int)WorkDescription.Indices.' + upper(replace(replace(replace(replace(replace(replace(replace(replace(replace(wd.Description, ',', ''), '&', 'AND'), '[', ''), ']', ''), '.', ''), ' ', '_'), '-', '_'), '/', '_'), '___', '_')) + '));' + CHAR(13) + CHAR(10) +
	'    }' + CHAR(13) + CHAR(10) +
	'}' + CHAR(13) + CHAR(10) as [Classes],
	upper(replace(replace(replace(replace(replace(replace(replace(replace(replace(wd.Description, ',', ''), '&', 'AND'), '[', ''), ']', ''), '.', ''), ' ', '_'), '-', '_'), '/', '_'), '___', '_')) + ' = ' + cast(wd.WorkDescriptionID as varchar) + ',' as [Indices],
	'case "' + lower(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(wd.Description, ',', ''), '&', 'and'), '[', ''), ']', ''), '.', ''), '_', ' '), '-', ' '), '/', ' '), '___', ' '), '   ', ' ')) + '":' + CHAR(13) + CHAR(10) +
	'    return new ' + replace(dbo.CapitalizeFirstLetter(lower(replace(replace(replace(replace(replace(replace(replace(wd.Description, '&', 'and'), '.', ''), '[', ''), ']', ''), ',', ''), '-', ' '), '/', ' '))), ' ', '') + 'WorkDescriptionFactory(session).Create();' as [Switch Cases],
	'createDescription("' + lower(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(wd.Description, ',', ''), '&', 'and'), '[', ''), ']', ''), '.', ''), '_', ' '), '-', ' '), '/', ' '), '___', ' '), '   ', ' ')) + '");' as [FN Call]
from WorkDescriptions wd
inner join AssetTypes at
on at.AssetTypeID = wd.AssetTypeID
order by wd.WorkDescriptionID

CREATE FUNCTION [dbo].[CapitalizeFirstLetter]
(
--string need to format
@string VARCHAR(200)--increase the variable size depending on your needs.
)
RETURNS VARCHAR(200)
AS

BEGIN
--Declare Variables
DECLARE @Index INT,
@ResultString VARCHAR(200)--result string size should equal to the @string variable size
--Initialize the variables
SET @Index = 1
SET @ResultString = ''
--Run the Loop until END of the string

WHILE (@Index <LEN(@string)+1)
BEGIN
IF (@Index = 1)--first letter of the string
BEGIN
--make the first letter capital
SET @ResultString =
@ResultString + UPPER(SUBSTRING(@string, @Index, 1))
SET @Index = @Index+ 1--increase the index
END

-- IF the previous character is space or '-' or next character is '-'

ELSE IF ((SUBSTRING(@string, @Index-1, 1) =' 'or SUBSTRING(@string, @Index-1, 1) ='-' or SUBSTRING(@string, @Index+1, 1) ='-') and @Index+1 <> LEN(@string))
BEGIN
--make the letter capital
SET
@ResultString = @ResultString + UPPER(SUBSTRING(@string,@Index, 1))
SET
@Index = @Index +1--increase the index
END
ELSE-- all others
BEGIN
-- make the letter simple
SET
@ResultString = @ResultString + LOWER(SUBSTRING(@string,@Index, 1))
SET
@Index = @Index +1--incerase the index
END
END--END of the loop

IF (@@ERROR
<> 0)-- any error occur return the sEND string
BEGIN
SET
@ResultString = @string
END
-- IF no error found return the new string
RETURN @ResultString
END
         */

        #region Fields

        private static readonly Type _tFactoryType = typeof(TFactory);

        #endregion

        public StaticListEntityLookupFactory(IContainer container) : base(container) { }

        public override TEntity Build(object overrides = null)
        {
            if (GetType() == _tFactoryType)
            {
                var type = _tFactoryType.Assembly
                                        .GetTypes().First(t =>
                                             t.IsClass && !t.IsAbstract && t.IsSubclassOf(_tFactoryType));

                return ((TFactory)_container.GetInstance(type)).Create(overrides);
            }

            return base.Build(overrides);
        }

        protected override TEntity Save(TEntity entity)
        {
            dynamic dyn = entity;
            TEntity ret = _container.GetInstance<RepositoryBase<TEntity>>().Find(dyn.Id);
            return ret ?? base.Save(entity);
        }

        public virtual IList<TEntity> CreateAll()
        {
            return
                _tFactoryType.Assembly.GetTypes()
                             .Where(t => t.IsSubclassOf(_tFactoryType))
                             .Select(sub => ((TFactory)_container.GetInstance(sub)).Create())
                             .ToList();
        }
    }
}
