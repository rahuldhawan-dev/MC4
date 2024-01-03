using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    public static class AssetTestExtensions
    {
        private static AssetStatus EnsureStatus(ISession session, int statusId)
        {
            var status = session.Get<AssetStatus>(statusId);

            if (status != null)
            {
                return status;
            }

            status = new AssetStatus { Id = statusId, Description = $"Status {statusId}" };
            session.Save(status, statusId);
            return status;
        }


        public static void TestNumberDuplicationTruthTable<TEntity>(this MapCallMvcInMemoryDatabaseTestBase<TEntity> that, TEntity entity, ViewModel<TEntity> viewModel, string suffixProp, string errorMessage, Func<AssetStatus, object> factoryOverrides)
            where TEntity : class, IThingWithAssetStatus, new()
        {
            foreach (var (assetAStatus, assetBStatus, expectedResult) in AssetStatusNumberDuplicationValidator.TRUTH_TABLE)
            {
                var statusA = EnsureStatus(that.Session, assetAStatus);
                var statusB = EnsureStatus(that.Session, assetBStatus);

                var existing = that.GetEntityFactory<TEntity>().Create(factoryOverrides(statusB));

                viewModel.SetPropertyValueByName("Status", assetAStatus);

                if (expectedResult)
                {
                    try
                    {
                        ValidationAssert.ModelStateIsValid(viewModel, suffixProp);
                    }
                    catch (AssertFailedException e)
                    {
                        throw new AssertFailedException(
                            $"Status {statusA.Id}:'{statusA.Description}' did not allow a duplicate with existing {statusB.Id}:'{statusB.Description}' as expected.",
                            e);
                    }
                }
                else
                {
                    try
                    {
                        ValidationAssert.ModelStateHasError(viewModel, suffixProp,
                            errorMessage);

                    }
                    catch (AssertFailedException e)
                    {
                        throw new AssertFailedException(
                            $"Status {statusA.Id}:'{statusA.Description}' unexpectedly allowed a duplicate with existing {statusB.Id}:'{statusB.Description}'.",
                            e);
                    }
                }

                that.Session.Delete(existing);
                that.Session.Flush();
                that.Session.Clear();
            }
        }
    }
}