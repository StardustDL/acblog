using AcBlog.Tools.Sdk.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class ListCategoryCommand : BaseCommand<ListCategoryCommand.CArgument>
    {
        public override string Name => "category";

        public override string Description => "List categories.";

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            using var client = new HttpClient();
            await workspace.Connect(client);
            var service = workspace.Remote!.CategoryService;
            var list = (await service.All(cancellationToken)).ToList();
            console.Out.WriteLine($"Founded {list.Count} categories.");
            var items = await Task.WhenAll(list.Select(id => service.Get(id, cancellationToken)));
            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                console.Out.WriteLine(item!.Name);
            }
            return 0;
        }

        public class CArgument
        {
        }
    }
}
