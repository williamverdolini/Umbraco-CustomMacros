using System;

namespace CustomMacros.Areas.Infrastructure.Controllers.Flow
{
    public class CommandTaskFlow
    {
        public static CommandTask<TModel, TCommand> Init<TModel, TCommand>(TModel viewModel, TCommand command)
        {
            CommandTask<TModel, TCommand> task = new CommandTask<TModel, TCommand>(viewModel, command);
            return task;
        }

        public static CommandTask<TModel, TCommand> Init<TModel, TCommand>(Func<TModel> initilizeModel, TCommand command)
        {
            CommandTask<TModel, TCommand> task = new CommandTask<TModel, TCommand>(initilizeModel(), command);
            return task;
        }

        public static CommandTask<TModel, TCommand> Init<TModel, TCommand>(Func<TCommand, TModel> initilizeModel, TCommand command)
        {
            CommandTask<TModel, TCommand> task = new CommandTask<TModel, TCommand>(initilizeModel(command), command);
            return task;
        }
    }
}