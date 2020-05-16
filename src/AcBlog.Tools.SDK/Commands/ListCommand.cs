using System.CommandLine;

namespace AcBlog.Tools.SDK.Commands
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override string Name => "list";

        public override string Description => "List blog contents.";

        protected override bool DisableHandler => true;

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddCommand(new ListPostCommand().Build());
            result.AddCommand(new ListCategoryCommand().Build());
            result.AddCommand(new ListKeywordCommand().Build());
            return result;
        }

        public class CArgument
        {
        }
    }
}
