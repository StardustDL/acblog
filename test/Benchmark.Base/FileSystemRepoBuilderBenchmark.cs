using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using System.Reflection;
using System.IO;
using AcBlog.Data.Repositories.FileSystem;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using Test.Data;
using AcBlog.Data.Repositories.FileSystem.Builders;

namespace Benchmark.Base
{
    [BenchmarkCategory("FileSystemRepoBuilder")]
    public class FileSystemRepoBuilderBenchmark
    {
        [Params(100, 1000)]
        public int Length { get; set; }

        string RootPath { get; set; }

        IList<Post> PostData { get; set; }

        IList<AcBlog.Data.Models.File> FileData { get; set; }

        IList<Layout> LayoutData { get; set; }

        IList<Page> PageData { get; set; }

        BlogBuilder Builder { get; set; }

        [IterationSetup]
        public void Setup()
        {
            RootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid().ToString());
            FSStaticBuilder.EnsureDirectoryExists(RootPath);

            PostData = Enumerable.Range(0, Length).Select(x => Generator.GetPost()).ToArray();
            LayoutData = Enumerable.Range(0, Length).Select(x => Generator.GetLayout()).ToArray();
            PageData = Enumerable.Range(0, Length).Select(x => Generator.GetPage()).ToArray();
            FileData = Enumerable.Range(0, Length).Select(x => Generator.GetFile()).ToArray();

            Builder = new BlogBuilder(new BlogOptions(), RootPath);
        }

        [IterationCleanup]
        public void Cleanup()
        {
            FSStaticBuilder.EnsureDirectoryExists(RootPath, false);
        }

        [Benchmark]
        public async Task Post() => await Builder.BuildPosts(PostData);

        [Benchmark]
        public async Task Layout() => await Builder.BuildLayouts(LayoutData);

        [Benchmark]
        public async Task Page() => await Builder.BuildPages(PageData);

        [Benchmark]
        public async Task File() => await Builder.BuildFiles(FileData);
    }
}
