using System.CommandLine;

namespace AcBlog.Tools.Sdk.Commands
{
    public class NewCommand : BaseCommand<NewCommand.CArgument>
    {
        public override string Name => "new";

        public override string Description => "Create blog contents.";

        protected override bool DisableHandler => true;

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddCommand(new News.PostCommand().Build());
            return result;
        }

        public class CArgument
        {
        }
    }
}
