using System;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class VideoRepositoryTest
    {
        [TestMethod]
        public void TestFixSproutEmbedCodeReturnsSameCodeIfUserEmailIsNullOrEmpty()
        {
            var expected = "Well that's not a valid iframe at all!";
            Assert.AreEqual(expected, VideoRepository.FixSproutEmbedCode(new User(), expected));
            Assert.AreEqual(expected, VideoRepository.FixSproutEmbedCode(new User {Email = string.Empty}, expected));
        }

        [TestMethod]
        public void TestFixSproutEmbedCodeAttachesUserEmailToEmbedCodeUrlWhenAQueryStringExists()
        {
            //  <iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040' width='630' height='473' frameborder='0' allowfullscreen></iframe>
            var user = new User {Email = "some@email.com"};
            var iframe =
                "<iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040?some=query' width='630' height='473' frameborder='0' allowfullscreen></iframe>";
            var expected =
                "<iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040?vemail=some@email.com&some=query' width='630' height='473' frameborder='0' allowfullscreen></iframe>";

            var result = VideoRepository.FixSproutEmbedCode(user, iframe);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFixSproutEmbedCodeAttachesUserEmailToEmbedCodeUrlWhenAQueryStringDoesNotExist()
        {
            //  <iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040' width='630' height='473' frameborder='0' allowfullscreen></iframe>
            var user = new User {Email = "some@email.com"};
            var iframe =
                "<iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040' width='630' height='473' frameborder='0' allowfullscreen></iframe>";
            var expected =
                "<iframe class='sproutvideo-player' src='//videos.sproutvideo.com/embed/d49bdfb71a1fe7c45c/a94a79d421fa2040?vemail=some@email.com' width='630' height='473' frameborder='0' allowfullscreen></iframe>";

            var result = VideoRepository.FixSproutEmbedCode(user, iframe);
            Assert.AreEqual(expected, result);
        }
    }
}
