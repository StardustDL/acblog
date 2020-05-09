using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AcBlog.Tools.StaticGenerator
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<DirectoryInfo>(
                    new string[]{
                        "--output",
                        "-o"
                    },
                    getDefaultValue: () => new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "dist")),
                    description: "Output directory")
            };

            rootCommand.Description = "AcBlog Static Backend Generator";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<DirectoryInfo>(Work);

            // Parse the incoming args and invoke the handler
            return await rootCommand.InvokeAsync(args);
        }

        static async Task Work(DirectoryInfo output)
        {
            if (!output.Exists)
                output.Create();

            DirectoryInfo postDist = output.CreateSubdirectory("posts");

            var loader = new PostsLoader(new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "posts")));

            var ls = await loader.LoadAll();

            var postBuilder = new PostRepositoryBuilder(ls, postDist)
            {
                Protector = new PostProtector(),
                CountPerPage = 10,
            };

            await postBuilder.Build();

            {
                KeywordRepositoryBuilder builder = new KeywordRepositoryBuilder(postBuilder.Keywords, output.CreateSubdirectory("keywords"));
                await builder.Build();
            }

            {
                CategoryRepositoryBuilder builder = new CategoryRepositoryBuilder(postBuilder.Categories, output.CreateSubdirectory("categories"));
                await builder.Build();
            }
        }
    }
}
