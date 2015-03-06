using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Sample.Worker
{
    public interface IToDoListMacroProperties : IMacroProperties
    {
        string IsIdVisible { get; set; } 
    }
}
