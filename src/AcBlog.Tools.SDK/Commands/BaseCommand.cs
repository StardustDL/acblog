using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public abstract class BaseCommand<T>
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        protected virtual bool DisableHandler { get; } = false;

        public virtual Command Configure()
        {
            var result = new Command(Name, Description);
            return result;
        }

        public virtual Task<int> Handle(T argument, IConsole console, InvocationContext context, CancellationToken cancellationToken) => Task.FromResult(0);

        public virtual Command Build()
        {
            Command command = Configure();
            if (!DisableHandler)
                command.Handler = CommandHandler.Create<T, IConsole, InvocationContext, CancellationToken>(Handle);
            return command;
        }
    }
}
