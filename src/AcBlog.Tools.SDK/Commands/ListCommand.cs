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

    public class NewCommand : BaseCommand<NewCommand.CArgument>
    {
        public override string Name => "new";

        public override string Description => "Create new contents.";

        protected override bool DisableHandler => true;

        public override Command Configure()
        {
            var result = base.Configure();
            return result;
        }

        public class CArgument
        {
        }
    }
}
