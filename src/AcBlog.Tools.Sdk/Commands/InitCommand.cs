using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override string Name => "init";

        public override string Description => "Initialize AcBlog.";

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            await workspace.Initialize();
            return 0;
        }

        public class CArgument
        {

        }
    }
}
