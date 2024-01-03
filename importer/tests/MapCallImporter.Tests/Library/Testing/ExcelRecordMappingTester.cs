using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ExpressionExtensions;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Library.Testing
{
    public class ExcelRecordMappingTester<TEntity, TViewModel, TExcelRecord>
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelRecord : ExcelRecordBase<TEntity, TViewModel, TExcelRecord>
    {
        #region Private Members

        protected IEnumerable<PropertyInfo> _allExcelRecordProperties;
        protected readonly IList<PropertyInfo> _testedExcelRecordProperties;

        #endregion

        #region Properties

        public ExcelRecordTestBase<TEntity, TViewModel, TExcelRecord> Test { get; }
        public TExcelRecord Target { get; }

        public IEnumerable<PropertyInfo> AllExcelRecordProperties =>
            _allExcelRecordProperties ?? (_allExcelRecordProperties = GatherExcelRecordProperties());

        public IEnumerable<PropertyInfo> TestedExcelRecordProperties => _testedExcelRecordProperties;

        #endregion

        #region Constructors

        public ExcelRecordMappingTester(ExcelRecordTestBase<TEntity, TViewModel, TExcelRecord> test, TExcelRecord target)
        {
            Test = test;
            Target = target;
            _testedExcelRecordProperties = new List<PropertyInfo>();
        }

        #endregion

        #region Private Methods

        private IEnumerable<PropertyInfo> GatherExcelRecordProperties()
        {
            return Target.GetAllPublicGetters();
        }

        private PropertyInfo SetValueFromMemberFn<TValue>(TExcelRecord target, Expression<Func<TExcelRecord, TValue>> memberFn, TValue value)
        {
            return _testedExcelRecordProperties.AddIfMissing(memberFn.SetProperty(target, value));
        }

        private int FindInvalidReferenceId<TRef>()
        {
            var index = -1;
            Test.WithUnitOfWork(uow => {
                for (index = 1; !Equals(uow.Find<TRef>(index), default(TRef)); ++index) { }
            });
            return index;
        }

        protected int GetIdFromReference<TRef>(TRef reference)
        {
            return ((dynamic)reference).Id;
        }

        protected void Value<TVal>(Expression<Func<TExcelRecord, TVal>> memberFn, Func<TEntity, TVal> entityMemberFn, TVal testValue)
        {
            var prop = SetValueFromMemberFn(Target, memberFn, testValue);

            Test.WithUnitOfWork(uow => {
                var mapped = Target.MapToEntity(uow, 1, Test.MappingHelper);
                var actual = entityMemberFn(mapped);

                Assert.AreEqual(testValue, actual,
                    $"Property {prop.PropertyType.GetFullName()} {prop.Name} did not map as expected");
            });
        }

        #endregion

        #region Exposed Methods

        public void RequiredString(Expression<Func<TExcelRecord, string>> memberFn,
            Func<TEntity, object> entityMemberFn)
        {
            var originalValue = memberFn.Compile().Invoke(Target);

            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] {string.Empty, " ", null})
                {
                    var prop = SetValueFromMemberFn(Target, memberFn, value);

                    Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper),
                        $"Setting property {prop.PropertyType.GetFullName()} {prop.Name} to '{value ?? "null"}' did not generate mapping failure as expected");
                }

                SetValueFromMemberFn(Target, memberFn, originalValue);

                Assert.AreEqual(originalValue,
                    entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)).ToString());
            });
        }

        public void RequiredInt(Expression<Func<TExcelRecord, int>> memberFn,
            Func<TEntity, int> entityMemberFn)
        {
            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] {-1, 0, 666})
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Assert.AreEqual(value, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
                }
            });
        }

        public void RequiredInt(Expression<Func<TExcelRecord, int>> memberFn, Func<TEntity, int?> entityMemberFn)
        {
            RequiredInt(memberFn, x => entityMemberFn(x).Value);
        }

        public void RequiredGreaterThanZeroInt(Expression<Func<TExcelRecord, int>> memberFn,
            Func<TEntity, int> entityMemberFn)
        {
            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] {-1, 0})
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper));
                }

                SetValueFromMemberFn(Target, memberFn, 1);

                Assert.AreEqual(1, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
            });
        }

        public void RequiredDecimal(Expression<Func<TExcelRecord, decimal?>> memberFn,
            Func<TEntity, decimal?> entityMemberFn)
        {
            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] {-1, 0, 666})
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Assert.AreEqual(value, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
                }

                SetValueFromMemberFn(Target, memberFn, null);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper));
            });
        }

        public void RequiredDateTime(Expression<Func<TExcelRecord, DateTime>> memberFn,
            Func<TEntity, DateTime> entityMemberFn)
        {
            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] {System.DateTime.MinValue, System.DateTime.Now, System.DateTime.MaxValue})
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Assert.AreEqual(value, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
                }
            });
        }

        public void RequiredDateTime(Expression<Func<TExcelRecord, DateTime?>> memberFn,
            Func<TEntity, DateTime?> entityMemberFn)
        {
            Test.WithUnitOfWork(uow => {
                SetValueFromMemberFn(Target, memberFn, null);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper));

                foreach (var value in new[] {System.DateTime.MinValue, System.DateTime.Now, System.DateTime.MaxValue})
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Assert.AreEqual(value, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
                }
            });
        }

        public void RequiredEntityRef<TRef>(Expression<Func<TExcelRecord, int>> memberFn,
            Func<TEntity, TRef> entityMemberFn, int? validId = null)
        {
            var actualValidId = (validId ?? memberFn.Compile()(Target));
            var invalidId = FindInvalidReferenceId<TRef>();

            Test.WithUnitOfWork(uow => {
                SetValueFromMemberFn(Target, memberFn, invalidId);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper));

                SetValueFromMemberFn(Target, memberFn, actualValidId);

                Assert.AreEqual(GetIdFromReference(uow.Find<TRef>(actualValidId)),
                    GetIdFromReference(entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper))));
            });
        }

        public void RequiredEntityRef<TRef>(Expression<Func<TExcelRecord, int?>> memberFn,
            Func<TEntity, TRef> entityMemberFn, int? validId = null)
        {
            var actualValidId = (validId ?? memberFn.Compile()(Target));
            var invalidId = FindInvalidReferenceId<TRef>();
            var memberName = ((MemberExpression)memberFn.Body).Member.Name;

            Test.WithUnitOfWork(uow => {
                SetValueFromMemberFn(Target, memberFn, invalidId);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper),
                    $"Invalid id {invalidId} for property {memberName} did not generate exception as expected.");

                SetValueFromMemberFn(Target, memberFn, null);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper),
                    $"Null value for property {memberName} did not generate an exception as expected.");

                SetValueFromMemberFn(Target, memberFn, actualValidId);

                if (actualValidId.HasValue)
                {
                    Assert.AreEqual(GetIdFromReference(uow.Find<TRef>(actualValidId)),
                        GetIdFromReference(entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper))));
                }
            });
        }

        public void EntityLookup<TEntityLookup>(Expression<Func<TExcelRecord, string>> memberFn, Func<TEntity, TEntityLookup> entityMemberFn, string validDescription)
            where TEntityLookup : ReadOnlyEntityLookup
        {
            var actualValidDescription = (validDescription ?? memberFn.Compile()(Target));
            var invalidDescription = "NOTHING SHOULD EVER BE CALLED THIS";

            Test.WithUnitOfWork(uow => {
                SetValueFromMemberFn(Target, memberFn, invalidDescription);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper),
                    $"Invalid value '{invalidDescription}' for property {((MemberExpression)memberFn.Body).Member.Name} did not generate exception as expected.");

                SetValueFromMemberFn(Target, memberFn, null);

                MyAssert.DoesNotThrow(() => Target.MapToEntity(uow, 1, Test.MappingHelper));

                SetValueFromMemberFn(Target, memberFn, actualValidDescription);

                if (actualValidDescription != null)
                {
                    Assert.AreEqual(actualValidDescription, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)).Description);
                }
            });
        }

        public void EntityRef<TRef>(Expression<Func<TExcelRecord, int?>> memberFn, Func<TEntity, TRef> entityMemberFn, int? validId = null)
        {
            var actualValidId = (validId ?? memberFn.Compile()(Target));
            var invalidId = FindInvalidReferenceId<TRef>();

            Test.WithUnitOfWork(uow => {
                SetValueFromMemberFn(Target, memberFn, invalidId);

                Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper),
                    $"Invalid id {invalidId} for property {((MemberExpression)memberFn.Body).Member.Name} did not generate exception as expected.");

                SetValueFromMemberFn(Target, memberFn, null);

                MyAssert.DoesNotThrow(() => Target.MapToEntity(uow, 1, Test.MappingHelper));

                SetValueFromMemberFn(Target, memberFn, actualValidId);

                if (actualValidId.HasValue)
                {
                    Assert.AreEqual(GetIdFromReference(uow.Find<TRef>(actualValidId)),
                        GetIdFromReference(entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper))));
                }
            });
        }

        public void String(Expression<Func<TExcelRecord, string>> memberFn, Func<TEntity, string> entityMemberFn)
        {
            Value(memberFn, entityMemberFn, memberFn.Compile().Invoke(Target) ?? "foo");
        }

        public void Int(Expression<Func<TExcelRecord, int?>> memberFn, Func<TEntity, int?> entityMemberFn)
        {
            Value(memberFn, entityMemberFn, 1);
        }

        public void Decimal(Expression<Func<TExcelRecord, decimal?>> memberFn, Func<TEntity, decimal?> entityMemberFn)
        {
            Value(memberFn, entityMemberFn, 1);
        }

        public void Long(Expression<Func<TExcelRecord, long?>> memberFn, Func<TEntity, long?> entityMemberFn, long testValue = 1)
        {
            Value(memberFn, entityMemberFn, testValue);
        }

        public void Boolean(Expression<Func<TExcelRecord, bool?>> memberFn, Func<TEntity, bool?> entityMemberFn)
        {
            foreach (var value in new[] {true, false})
            {
                Value(memberFn, entityMemberFn, value);
            }
        }

        public void DateTime(Expression<Func<TExcelRecord, DateTime?>> memberFn, Func<TEntity, DateTime?> entityMemberFn)
        {
            Value(memberFn, entityMemberFn, new DateTime(2018, 2, 22));
        }

        public void AssertCountsMatch()
        {
            var untested = AllExcelRecordProperties.Where(ap =>
                    !TestedExcelRecordProperties.Any(tp => tp.Name == ap.Name && tp.PropertyType == ap.PropertyType))
                .OrderBy(p => p.Name)
                .Select(p => $"{p.PropertyType.GetFullName()} {p.Name}");

            if (untested.Any())
            {
                var props = string.Join("," + Environment.NewLine, untested);

                Assert.Fail(
                    $@"Found {untested.Count()} untested properties on type {typeof(TExcelRecord)}:{Environment.NewLine}{props}");
            }
        }

        public void TestedElsewhere<TVal>(Expression<Func<TExcelRecord, TVal>> memberFn)
        {
            _testedExcelRecordProperties.AddIfMissing(memberFn.GetProperty());
        }

        public void NotMapped<TVal>(Expression<Func<TExcelRecord, TVal>> memberFn)
        {
            TestedElsewhere(memberFn);
        }

        public void IntWithinRange(Expression<Func<TExcelRecord, int?>> memberFn,
            Func<TEntity, int?> entityMemberFn, int minValue, int maxValue)
        {
            Test.WithUnitOfWork(uow => {
                foreach (var value in new[] { minValue - 1, maxValue + 1 })
                {
                    SetValueFromMemberFn(Target, memberFn, value);

                    Test.ExpectMappingFailure(() => Target.MapToEntity(uow, 1, Test.MappingHelper));
                }

                foreach (var value in new[] { minValue, maxValue, minValue + 1, maxValue - 1 })
                {
                    SetValueFromMemberFn(Target, memberFn, maxValue);

                    Assert.AreEqual(maxValue, entityMemberFn(Target.MapToEntity(uow, 1, Test.MappingHelper)));
                }
            });
        }

        #endregion
    }
}