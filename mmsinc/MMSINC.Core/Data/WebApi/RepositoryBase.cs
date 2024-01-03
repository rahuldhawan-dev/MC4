using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Data.WebApi
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    {
        #region Properties

        public abstract Uri BaseAddress { get; }
        public abstract String RsaTokenConfigKey { get; }
        public abstract String UserName { get; }

        public string Url
        {
            get
            {
                return String.Format("api/{0}",
                    typeof(TEntity).Name.ToLowerInvariant());
            }
        }

        public string Token
        {
            get { return ConfigurationManager.AppSettings[RsaTokenConfigKey]; }
        }

        #endregion

        #region Private Methods

        protected void WithClient(Action<IHttpClient> fn)
        {
            WithClient<Object>(client => {
                fn(client);
                return null;
            });
        }

        protected TResult WithClient<TResult>(Func<IHttpClient, TResult> fn)
        {
            using (var client = GetClient())
            {
                client.AcceptMimeType("application/json");
                client.SetUserName(UserName);
                client.SetAuthorizationToken(Token);
                return fn(client);
            }
        }

        protected virtual IHttpClient GetClient()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            return new HttpClient {
                BaseAddress = BaseAddress
            };
        }

        #endregion

        #region Exposed Methods

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            WithClient(c => c.Delete(Url, id));
        }

        public TEntity Save(TEntity entity)
        {
            return WithClient(c => c.Post(Url, entity));
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Save(NameValueCollection entity)
        {
            var controllerContext = new ControllerContext();
            var modelBindingContext = new ModelBindingContext {
                ModelMetadata =
                    ModelMetadataProviders.Current.GetMetadataForType(null,
                        typeof(
                            TEntity
                        )),
                ValueProvider =
                    new NameValueCollectionValueProvider(entity,
                        CultureInfo.CurrentCulture)
            };

            return Save((TEntity)new DefaultModelBinder()
               .BindModel(controllerContext,
                    modelBindingContext));
        }

        public TEntity Find(int id)
        {
            return WithClient(c => c.Get<TEntity>(Url, id));
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public int GetIdentifier(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Search(NameValueCollection searchParams)
        {
            return WithClient(c =>
                c.Search<TEntity>(Url, searchParams));
        }

        public IEnumerable<TEntity> Search(int[] ids)
        {
            var dict = new NameValueCollection();

            ids.Each(id => dict.Add("ids", id.ToString()));

            return Search(dict);
        }

        public string New(NameValueCollection initialParams = null)
        {
            return WithClient(c => c.New(Url + "/new", initialParams));
        }

        #endregion
    }

    public class Repository<TEntity> : RepositoryBase<TEntity>
    {
        #region Constants

        public const string RSA_TOKEN_CONFIG_KEY =
            "MMSINC.Data.WebApi.RepositoryRSAKey";

        #endregion

        #region Private Members

        protected readonly Uri _baseAddress;
        protected readonly string _userName;

        #endregion

        #region Properties

        public override Uri BaseAddress
        {
            get { return _baseAddress; }
        }

        public override string RsaTokenConfigKey
        {
            get { return RSA_TOKEN_CONFIG_KEY; }
        }

        public override string UserName
        {
            get { return _userName; }
        }

        #endregion

        #region Constructors

        public Repository(String userName, Uri baseAddress)
        {
            _userName = userName;
            _baseAddress = baseAddress;
        }

        #endregion
    }

    public interface IRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Abstract Methods

        IEnumerable<TEntity> Search(NameValueCollection searchParams);
        IEnumerable<TEntity> Search(int[] ids);
        void Delete(int id);

        /// <summary>
        /// Returns rendered html that needs to be injected into a webpage.
        /// If there's an error, the repository doesn't know, and you'll inject error messages instead.
        /// </summary>
        /// <param name="initialParams"></param>
        /// <returns></returns>
        string New(NameValueCollection initialParams = null);
        TEntity Save(NameValueCollection entity);

        #endregion
    }
}
