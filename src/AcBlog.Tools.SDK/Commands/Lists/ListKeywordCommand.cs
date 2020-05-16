using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class ListKeywordCommand : BaseCommand<ListKeywordCommand.CArgument>
    {
        public override string Name => "keyword";

        public override string Description => "List keywords.";

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Current();
            using var client = new HttpClient();
            await workspace.Connect(client);
            var service = workspace.Remote!.KeywordService;
            var list = await service.All();
            foreach (var id in list)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var item = (await service.Get(id))!;
                console.Out.WriteLine(item.Name);
            }
            return 0;
        }

        public class CArgument
        {
        }
    }
}
