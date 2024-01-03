using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DataPageRouteHelperTest
    /// </summary>
    [TestClass]
    public class DataPageRouteHelperTest
    {
        // TODO: TEST ME!
        // This is code frmo DataPageBaseTest that is no longe rin DataPageBaseTest!

        //[TestMethod]
        //public void TestParseQueryStringContainsSearchWithValidGuidSetsPageModeToResults()
        //{
        //    var target = InitializeBuilderForParseQueryString().Build();

        //    var search = DataPageUtility.QUERY.SEARCH;
        //    var searchGuid = Guid.NewGuid();
        //    var query = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        //    query.Add(search, searchGuid.ToString());

        //    using (_mocks.Record())
        //    {
        //        var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?" + search + "=" + searchGuid.ToString());

        //        SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());

        //        var qs = new QueryStringWrapper(query);

        //        SetupResult.For(_mockedRequest.IQueryString).Return(qs);
        //        SetupResult.For(_mockPermissions.IsAuthorizedToAccessPage).Return(true);

        //    }

        //    using (_mocks.Playback())
        //    {
        //        target.InitializeRouteTest();
        //        Assert.AreEqual(target.PageModeTest, PageModes.Results);
        //        Assert.AreEqual(target.CachedFilterKey, searchGuid);
        //    }

        //}

        //[TestMethod]
        //public void TestParseQueryStringContainsSearchWithMalformedGuidRedirectsToSearchPage()
        //{
        //    var target = InitializeBuilderForParseQueryString().Build();

        //    var search = DataPageUtility.QUERY.SEARCH;
        //    var malformedSearchGuid = "agepgeapgae";
        //    var query = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        //    query.Add(search, malformedSearchGuid);

        //    using (_mocks.Record())
        //    {
        //        var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?" + search + "=" + malformedSearchGuid);

        //        SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());

        //        var qs = new QueryStringWrapper(query);

        //        SetupResult.For(_mockedRequest.IQueryString).Return(qs);
        //        SetupResult.For(_mockPermissions.IsAuthorizedToAccessPage).Return(true);

        //        _mockedResponse.Redirect("http://www.somewebsite.com/Neato.aspx", true);
        //    }

        //    using (_mocks.Playback())
        //    {
        //        target.InitializeRouteTest();
        //        Assert.AreEqual(target.PageModeTest, PageModes.Search);
        //        Assert.AreNotEqual(target.CachedFilterKey, malformedSearchGuid);
        //    }

        //}

        //[TestMethod]
        //public void TestParseQueryStringContainsSearchAndViewSetsCachedFilterKeyButDoesNotChangePageModeToResults()
        //{
        //    var target = InitializeBuilderForParseQueryString().Build();

        //    var view = DataPageUtility.QUERY.VIEW;
        //    var recordId = 153;
        //    var search = DataPageUtility.QUERY.SEARCH;
        //    var searchGuid = Guid.NewGuid();
        //    var query = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        //    query.Add(search, searchGuid.ToString());
        //    query.Add(view, recordId.ToString());

        //    using (_mocks.Record())
        //    {
        //        var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?" + view + "=" + recordId + "&" + search + "=" + searchGuid.ToString());

        //        SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());

        //        var qs = new QueryStringWrapper(query);

        //        SetupResult.For(_mockedRequest.IQueryString).Return(qs);
        //        SetupResult.For(_mockPermissions.IsAuthorizedToAccessPage).Return(true);

        //    }

        //    using (_mocks.Playback())
        //    {
        //        target.InitializeRouteTest();
        //        Assert.AreEqual(target.PageModeTest, PageModes.RecordReadOnly);
        //        Assert.AreEqual(target.CachedFilterKey, searchGuid);
        //    }

        //}

        //[TestMethod]
        //public void TestParseQueryStringContainsExportWithValidGuidSetsPageModeToResults()
        //{
        //    var target = InitializeBuilderForParseQueryString()
        //        .WithCache(_iCache)
        //        .Build();

        //    target.UseMockRenderResultsGridViewToExcel = true;
        //    target.MockedExcelString = "some string";

        //    var export = DataPageUtility.QUERY.EXPORT;
        //    var exportGuid = Guid.NewGuid();
        //    var query = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        //    query.Add(export, exportGuid.ToString());

        //    using (_mocks.Record())
        //    {
        //        var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?" + export + "=" + exportGuid.ToString());

        //        SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());

        //        var qs = new QueryStringWrapper(query);

        //        SetupResult.For(_mockedRequest.IQueryString).Return(qs);
        //        SetupResult.For(_mockPermissions.IsAuthorizedToAccessPage).Return(true);
        //    }

        //    using (_mocks.Playback())
        //    {
        //        target.InitializeRouteTest();
        //        Assert.AreEqual(target.PageModeTest, PageModes.Results);
        //        Assert.AreEqual(target.CachedFilterKey, exportGuid);
        //    }

        //}

        //[TestMethod]
        //public void TestParseQueryStringContainsExportWritesToResponse()
        //{
        //    var target = InitializeBuilderForParseQueryString()
        //        .WithCache(_iCache)
        //        .Build();

        //    target.UseMockRenderResultsGridViewToExcel = true;
        //    target.MockedExcelString = "some string";

        //    var export = DataPageUtility.QUERY.EXPORT;
        //    var exportGuid = Guid.NewGuid();
        //    var query = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        //    query.Add(export, exportGuid.ToString());

        //    using (_mocks.Record())
        //    {
        //        var testUrl = new Uri("http://www.somewebsite.com/Neato.aspx?" + export + "=" + exportGuid.ToString());

        //        SetupResult.For(_mockedRequest.Url).Return(testUrl.GetAbsoluteUriWithoutQuery());

        //        var qs = new QueryStringWrapper(query);

        //        SetupResult.For(_mockedRequest.IQueryString).Return(qs);
        //        SetupResult.For(_mockPermissions.IsAuthorizedToAccessPage).Return(true);

        //        _mockedResponse.Clear();
        //        _mockedResponse.AddHeader("content-disposition", "attachment;filename=Data.xls");
        //        _mockedResponse.Write(target.MockedExcelString);
        //        _mockedResponse.End();

        //    }

        //    using (_mocks.Playback())
        //    {
        //        target.InitializeRouteTest();
        //    }
        //}
    }
}
