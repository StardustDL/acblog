using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.News
{
    public class PostCommand : BaseCommand<PostCommand.CArgument>
    {
        public override string Name => "post";

        public override string Description => "Create post.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<string>($"--{nameof(CArgument.Id).ToLowerInvariant()}", "Id"));
            result.AddOption(new Option<string>($"--{nameof(CArgument.Title).ToLowerInvariant()}", "Title"));
            result.AddOption(new Option<PostType>($"--{nameof(CArgument.Type).ToLowerInvariant()}", "Type"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            ILogger<PostCommand> logger = host.Services.GetRequiredService<ILogger<PostCommand>>();

            Post post = new Post
            {
                Id = argument.Id,
                Title = argument.Title,
                CreationTime = DateTimeOffset.Now,
                ModificationTime = DateTimeOffset.Now,
                Type = argument.Type,
            };
            var id = await workspace.Local.PostService.Create(post, cancellationToken);
            if (string.IsNullOrEmpty(id))
            {
                logger.LogError("Create failed.");
                return 1;
            }
            else
            {
                logger.LogInformation($"Create post: {id}.");
                return 0;
            }
        }

        public class CArgument
        {
            public string Id { get; set; } = string.Empty;

            public string Title { get; set; } = string.Empty;

            public PostType Type { get; set; } = PostType.Article;
        }
    }
}
