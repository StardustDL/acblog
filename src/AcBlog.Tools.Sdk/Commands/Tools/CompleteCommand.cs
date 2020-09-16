using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.Tools
{
    public class CompleteCommand : BaseCommand<CompleteCommand.CArgument>
    {
        public override string Name => "complete";

        public override string Description => "Complete post metadata and format.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Force).ToLowerInvariant()}", "Override ID, Time, Category"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            ILogger<CompleteCommand> logger = host.Services.GetRequiredService<ILogger<CompleteCommand>>();

            await foreach(var id in workspace.Local.PostService.All(cancellationToken))
            {
                var post = await workspace.Local.PostService.Get(id, cancellationToken);
                if (post is null)
                    continue;
                if (argument.Force)
                {
                    var meta = workspace.Local.InnerPostService.InnerRepository.GetDefaultMetadata(id);
                    meta.ApplyTo(post);
                }
                await workspace.Local.PostService.Update(post, cancellationToken);
                logger.LogInformation($"Complete {post.Title}.");
            }

            return 0;
        }

        public class CArgument
        {
            public bool Force { get; set; } = false;
        }
    }
}
