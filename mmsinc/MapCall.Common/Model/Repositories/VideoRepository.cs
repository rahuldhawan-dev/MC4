using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    // NOTE: This repository works with both the database AND the API calls to Sprout.
    // NOTE 2: API only allows for 100 queries per minute! Do not hammer it with multiple calls per request!

    public interface IVideoRepository : IRepository<Video>
    {
        SproutVideo FindSproutVideo(string sproutVideoId);
        SproutVideo FindSproutVideo(int id);
        IEnumerable<SproutVideo> GetAllSproutVideos();
        IEnumerable<SproutTag> GetAllTags();
    }

    public class VideoRepository : SecuredRepositoryBase<Video, User>, IVideoRepository
    {
        #region Consts

        private const string SPROUT_API_KEY = "80e12ea73c81042e797af7c76d17abb9",
                             SERVICE_URL_ROOT = "https://api.sproutvideo.com/v1/";

        #endregion

        #region Constructor

        public VideoRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, authenticationService, container) { }

        #endregion

        #region Private Methods

        // This is protected primarily for unit testing.
        private static dynamic GetSproutApiResponse(string url)
        {
            url = SERVICE_URL_ROOT + url;
            using (var web = new WebClient())
            {
                web.Headers.Add("SproutVideo-Api-Key", SPROUT_API_KEY);
                try
                {
                    var json = web.DownloadString(url);
                    return Json.Decode(json);
                }
                catch (WebException ex)
                {
                    // TODO: Figure out how to catch 404s and then return null for those.
                    if (ex.Status == WebExceptionStatus.ProtocolError &&
                        ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    // else
                    throw;
                }
            }
        }

        internal static string FixSproutEmbedCode(User user, string original)
        {
            /* Sprout, for whatever reason, does not let you pass additional user information to the videos
             * api. This means we have to manually edit their bad iframe html to include the proper url parameters.
             * It's stupid. It makes me angry. */

            //  <iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040' width='630' height='473' frameborder='0' allowfullscreen></iframe>

            var email = user.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                return original;
            }

            const string QUERY = "?";

            if (original.Contains(QUERY))
            {
                var index = original.IndexOf(QUERY, StringComparison.InvariantCulture) + QUERY.Length;
                return original.Insert(index, $"vemail={user.Email}&");
            }

            const string SRC = "src='";
            if (original.Contains(SRC))
            {
                var srcIndex = original.IndexOf(SRC, StringComparison.InvariantCulture) + SRC.Length;
                var srcEndIndex = original.IndexOf("'", srcIndex, StringComparison.InvariantCulture);
                return original.Insert(srcEndIndex, $"?vemail={user.Email}");
            }

            throw new NotSupportedException("Unable to parse this Sprout embed: " + original);
        }

        internal SproutVideo ConvertDynamidJsonToConcreteVideo(dynamic jsonVideo)
        {
            var model = new SproutVideo {
                Id = jsonVideo.id,
                EmbedCode = FixSproutEmbedCode(CurrentUser, jsonVideo.embed_code),
                Title = jsonVideo.title,
                CreatedAt = DateTime.Parse(jsonVideo.created_at).ToString()
            };

            model.Tags = ((DynamicJsonArray)jsonVideo.tags).Cast<string>().ToArray();

            var thumbs = jsonVideo.assets.thumbnails;
            var length = (int)thumbs.Length;
            if (length > 0)
            {
                model.Thumbnail = thumbs[length - 1];
                // The api uses https, as do the videos, but for some reason Sprout doesn't do https with the thumbnails.
                // You can still use https though, it's just not what they send back.
                model.Thumbnail =
                    model.Thumbnail.Replace("http:",
                        string.Empty); // Use the // url method to match http/https based on host of the current page.
            }

            return model;
        }

        #endregion

        #region Public Methods

        public SproutVideo FindSproutVideo(string sproutVideoId)
        {
            var jsonResp = GetSproutApiResponse("videos/" + sproutVideoId);

            if (jsonResp == null)
            {
                return null;
            }

            return ConvertDynamidJsonToConcreteVideo(jsonResp);
        }

        public SproutVideo FindSproutVideo(int id)
        {
            var record = Find(id);
            if (record == null)
            {
                return null;
            }

            return FindSproutVideo(record.SproutVideoId);
        }

        public IEnumerable<SproutVideo> GetAllSproutVideos()
        {
            var vids = new List<SproutVideo>();
            var resp = GetSproutApiResponse("videos?order_dir=desc&order_by=created_at");
            foreach (var vid in resp.videos)
            {
                vids.Add(ConvertDynamidJsonToConcreteVideo(vid));
            }

            return vids;
        }

        public IEnumerable<SproutTag> GetAllTags()
        {
            var tags = new List<SproutTag>();
            var resp = GetSproutApiResponse("tags");

            foreach (var tag in resp.tags)
            {
                tags.Add(new SproutTag {
                    TagId = tag.id,
                    Description = tag.name
                });
            }

            return tags.OrderBy(x => x.Description).ToList();
        }

        #endregion
    }
}
