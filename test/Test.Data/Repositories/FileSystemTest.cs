using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Builders;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Sdk;
using AcBlog.Sdk.Extensions;
using AcBlog.Sdk.FileSystem;
using DeepEqual.Syntax;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StardustDL.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    [TestClass]
    public class FileSystemTest
    {
        string RootPath { get; set; }

        IList<Post> PostData { get; set; }

        IList<Layout> LayoutData { get; set; }

        IList<Page> PageData { get; set; }

        BlogOptions BlogOptionsData { get; set; }

        FileSystemBlogService BlogService { get; set; }

        [TestInitialize]
        public async Task Setup()
        {
            RootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "fstest");
            FSStaticBuilder.EnsureDirectoryExists(RootPath);

            PostData = Enumerable.Range(0, 100).Select(x => Generator.GetPost()).ToArray();
            LayoutData = Enumerable.Range(0, 100).Select(x => Generator.GetLayout()).ToArray();
            PageData = Enumerable.Range(0, 100).Select(x => Generator.GetPage()).ToArray();
            BlogOptionsData = new BlogOptions();

            BlogBuilder builder = new BlogBuilder(BlogOptionsData, RootPath);
            await builder.Build();
            await builder.BuildPosts(PostData);
            await builder.BuildLayouts(LayoutData);
            await builder.BuildPages(PageData);

            BlogService = new FileSystemBlogService(new PhysicalFileProvider(RootPath).AsFileProvider());
        }

        [TestMethod]
        public async Task Post()
        {
            var repo = BlogService.PostService;

            await repo.TestStatus();

            foreach (var item in PostData)
            {
                await repo.TestGet(item.Id);
            }

            foreach (var id in await repo.TestAll())
            {
                Assert.IsTrue(PostData.Any(x => x.Id == id));
            }

            {
                {
                    var res = await repo.Query(new AcBlog.Data.Models.Actions.PostQueryRequest
                    {
                        Type = PostType.Article
                    });
                    Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                    var filter = await repo.CreateArticleFilter().Filter();
                    filter.ShouldDeepEqual(res);
                }
                {
                    var res = await repo.Query(new AcBlog.Data.Models.Actions.PostQueryRequest
                    {
                        Type = PostType.Note
                    });
                    Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                    var filter = await repo.CreateNoteFilter().Filter();
                    filter.ShouldDeepEqual(res);
                }
                {
                    var res = await repo.Query(new AcBlog.Data.Models.Actions.PostQueryRequest
                    {
                        Type = PostType.Slides
                    });
                    Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                    var filter = await repo.CreateSlidesFilter().Filter();
                    filter.ShouldDeepEqual(res);
                }
            }

            var cate = await repo.GetCategories();
            cate.ShouldDeepEqual(CategoryTreeBuilder.BuildFromPosts(PostData));
            {
                var c = cate.Root.Children.Values.First().Category;
                var res = await repo.Query(new AcBlog.Data.Models.Actions.PostQueryRequest
                {
                    Category = c
                });
                Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                var filter = await repo.CreateCategoryFilter().Filter(c);
                filter.ShouldDeepEqual(res);
            }

            var kwds = await repo.GetKeywords();
            kwds.ShouldDeepEqual(KeywordCollectionBuilder.BuildFromPosts(PostData));
            {
                var c = new Keyword(kwds.Items.First().OneName());
                var res = await repo.Query(new AcBlog.Data.Models.Actions.PostQueryRequest
                {
                    Keywords = c
                });
                Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                var filter = await repo.CreateKeywordFilter().Filter(c);
                filter.ShouldDeepEqual(res);
            }
        }

        [TestMethod]
        public async Task Layout()
        {
            var repo = BlogService.LayoutService;

            await repo.TestStatus();

            foreach (var item in LayoutData)
            {
                await repo.TestGet(item.Id);
            }

            foreach (var id in await repo.TestAll())
            {
                Assert.IsTrue(LayoutData.Any(x => x.Id == id));
            }
        }

        [TestMethod]
        public async Task Page()
        {
            var repo = BlogService.PageService;

            await repo.TestStatus();

            foreach (var item in PageData)
            {
                await repo.TestGet(item.Id);
            }

            foreach (var id in await repo.TestAll())
            {
                Assert.IsTrue(PageData.Any(x => x.Id == id));
            }

            {
                var c = PageData.First().Route;
                var res = await repo.Query(new AcBlog.Data.Models.Actions.PageQueryRequest
                {
                    Route = c
                });
                Assert.IsTrue(res.CurrentPage.TotalCount > 0);

                var filter = await repo.CreateRouteFilter().Filter(c);
                filter.ShouldDeepEqual(res);
            }
        }
    }
}
