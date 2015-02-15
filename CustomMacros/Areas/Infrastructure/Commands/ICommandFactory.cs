using System;

namespace CustomMacros.Areas.Infrastructure.Commands
{
    public interface ICommandFactory
    {
        string MacroName { get; set; }

        void Register(Type commandType);
        bool Contains(Type commandType);
        T Create<T>() where T : Command;
        T Clone<T>(T command) where T : Command;
        Command CreateFromRequest();
        void ConfigForwardedCommands(string fPs);
    }
}
