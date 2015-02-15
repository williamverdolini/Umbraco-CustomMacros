using System.Web.Mvc;

namespace CustomMacros.Areas.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : Command
    {
        ActionResult Handle(T command);
    }
}
