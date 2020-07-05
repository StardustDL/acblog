using System.CommandLine;

namespace AcBlog.Tools.Sdk.Commands
{
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
