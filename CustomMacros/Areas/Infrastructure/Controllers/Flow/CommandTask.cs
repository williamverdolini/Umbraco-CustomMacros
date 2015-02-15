using System;
using System.Web.Mvc;

namespace CustomMacros.Areas.Infrastructure.Controllers.Flow
{
    public class CommandTask<TModel, TCommand>
    {
        private TModel viewModel { get; set; }
        private TCommand command { get; set; }

        public CommandTask(TModel viewModel, TCommand command)
        {
            this.viewModel = viewModel;
            this.command = command;
        }

        public CommandTask<TModel, TCommand> Then(Action<TModel, TCommand> step)
        {
            step(viewModel, command);
            return this;
        }

        public CommandTask<TModel, TCommand> Then(Action<TModel> step)
        {
            step(viewModel);
            return this;
        }

        public CommandTask<TModel, TCommand> Then(Action<TCommand> step)
        {
            step(command);
            return this;
        }

        public CommandTask<TModel, TCommand> Then(Action step)
        {
            step();
            return this;
        }

        public CommandTask<TModel, TCommand> If(Func<bool> check, Action<TModel, TCommand> success, Action<TModel, TCommand> fail)
        {
            if (check()){
                if(success != null)
                    success(viewModel, command);
            }
            else if(fail!=null)
                fail(viewModel, command);
            return this;
        }

        public CommandTask<TModel, TCommand> If(Func<TModel, TCommand, bool> check, Action<TModel, TCommand> success, Action<TModel, TCommand> fail)
        {
            if (check(viewModel, command)){
                if(success != null)
                    success(viewModel, command);
            }
            else if(fail!=null)
                fail(viewModel, command);
            return this;
        }

        public ActionResult Result(Func<TModel, TCommand, ActionResult> result)
        {
            return result(viewModel, command);
        }

        public ActionResult Result(Func<TModel, ActionResult> result)
        {
            return result(viewModel);
        }

    }

}