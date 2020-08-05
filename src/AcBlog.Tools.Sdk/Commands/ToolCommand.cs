using System.CommandLine;

namespace AcBlog.Tools.Sdk.Commands
{
    public class ToolCommand : BaseCommand<ToolCommand.CArgument>
    {
        public override string Name => "tool";

        public override string Description => "Tools";

        protected override bool DisableHandler => true;

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddCommand(new Tools.CompleteCommand().Build());
            return result;
        }

        public class CArgument
        {
        }
    }
}
