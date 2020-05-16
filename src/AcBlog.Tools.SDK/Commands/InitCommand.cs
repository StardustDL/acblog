using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override string Name => "init";

        public override string Description => "Initialize AcBlog.";

        public override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public class CArgument
        {

        }
    }
}
