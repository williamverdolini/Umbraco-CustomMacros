using System;
using System.Linq;
using System.Web.Mvc;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Infrastructure.Filters
{
    public class SubscribeCommandsToProvideAttribute : ActionFilterAttribute
    {
        private MacroController controller;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = filterContext.Controller as MacroController;
            SubscribeCommandsToProvide();
        }

        private void SubscribeCommandsToProvide()
        {
            var commandHandlers = controller.GetType().GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandProvider<>));

            foreach (Type commandInterface in commandHandlers)
                SubscribeCommandToProvide(commandInterface.GetGenericArguments()[0]);
        }

        private void SubscribeCommandToProvide(Type @eventType)
        {
            controller.CommandsProvider.Register(@eventType);
        }

    }

    public class SubscribeCommandsToHandleAttribute : ActionFilterAttribute
    {
        private MacroController controller;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = filterContext.Controller as MacroController;
            SubscribeCommandsToHandle();
        }

        private void SubscribeCommandsToHandle()
        {
            var commandHandlers = controller.GetType().GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            foreach (Type commandInterface in commandHandlers)
                SubscribeCommandToHandle(commandInterface.GetGenericArguments()[0]);
        }

        private void SubscribeCommandToHandle(Type commandType)
        {
            Command retCommand = (Command)Activator.CreateInstance(commandType);
            controller.CommandsConsumer.Add(retCommand);
        }

    }
}