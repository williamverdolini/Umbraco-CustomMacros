using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using CustomMacros.Areas.Framework.Commons;
using CustomMacros.Areas.Framework.Services.Application;
using CustomMacros.Areas.Infrastructure.Commons;

namespace CustomMacros.Areas.Infrastructure.Commands
{
    public class CommandFactory : ICommandFactory
    {
        #region Private Constants
        private const string COMMAND_TYPE = "CommandType";
        private const string COMMAND_PROPERTIES = "CommandProps";
        private const string COMMAND_FORWARDPAGE = "ForwardPage";
        private const string COMMAND_POPUP_OPTIONS = "PopupOptions";
        #endregion

        public string MacroName { get; set; }

        private readonly IRequestService requestService;
        private IDictionary commandTypes { get; set; }
        private IList<ForwardableCommand> forwardedCommands { get; set; }
        
        public CommandFactory(IRequestService requestService)
        {
            Contract.Requires<ArgumentNullException>(requestService != null, "requestService");
            this.requestService = requestService;
            //Initializations
            MacroName = string.Empty;
            commandTypes = new Hashtable();
            forwardedCommands = new List<ForwardableCommand>();
        }

        public void Register(Type commandType)
        {
            if (commandType.Equals(typeof(Command)) || commandType.IsSubclassOf(typeof(Command)))
            {
                commandTypes.Add(commandType.Name, commandType);
            }
            else throw new Exception("The Type registered in module " + MacroName + " is NOT a Command type");
        }

        public bool Contains(Type commandType)
        {
            return commandTypes.Contains(commandType.Name);
        }

        public T Create<T>() where T : Command
        {
            Type commandType = (Type)commandTypes[typeof(T).Name];
            if (commandType == null)
                throw new Exception("The Command " + typeof(T).Name + " created in module " + MacroName + " was NOT registered");
            T retCommand = (T)Activator.CreateInstance(commandType);

            if (retCommand is ForwardableCommand && forwardedCommands.Any(fe => fe.Name.Equals(retCommand.Name)))
            {
                ForwardableCommand fcommand = forwardedCommands.First(fe => fe.Name.Equals(retCommand.Name));
                if (fcommand != null)
                {
                    retCommand.SetProperty(COMMAND_FORWARDPAGE, fcommand.ForwardPage);
                    retCommand.SetProperty(COMMAND_POPUP_OPTIONS, fcommand.PopupOptions);
                }
            }
            return retCommand;
        }

        public T Clone<T>(T command) where T : Command
        {
            Type commandType = (Type)commandTypes[typeof(T).Name];
            if (commandType == null)
                throw new Exception("The Command " + typeof(T).Name + " created in module " + MacroName + " was NOT registered");
            T retCommand = (T)Activator.CreateInstance(commandType);
            foreach (var prop in command.GetProperties())
                retCommand.SetProperty(prop.Key, prop.Value);

            return retCommand;
        }

        public void ConfigForwardedCommands(string forwardPageConfigurationStrings) 
        {
            if (!string.IsNullOrEmpty(forwardPageConfigurationStrings))
            {
                // NOTE: Unescaping, we can clean string for '\\' and unicode codes
                JavaScriptSerializer ser = new JavaScriptSerializer();
                List<Dictionary<string, object>> jSonProps = ser.Deserialize<List<Dictionary<string, object>>>(System.Text.RegularExpressions.Regex.Unescape(forwardPageConfigurationStrings));
                if (jSonProps != null)
                {
                    foreach (Dictionary<string, object> forwardCommand in jSonProps)
                    {
                        // Get command's class type by Reflection
                        string commandTypeName = (forwardCommand[COMMAND_TYPE] ?? string.Empty).ToString();
                        Type commandType = Utilities.GetTypeFromName(commandTypeName);

                        if (commandType != null)
                        {
                            if (typeof(ForwardableCommand).IsAssignableFrom(commandType))
                            {
                                // Create command by Reflection
                                ForwardableCommand commandObj = (ForwardableCommand)Activator.CreateInstance(commandType);
                                commandObj.SetProperties((Dictionary<string, object>)forwardCommand[COMMAND_PROPERTIES]);
                                forwardedCommands.Add((ForwardableCommand)commandObj);
                            }
                            else
                                throw new Exception("The Command " + commandType + " configured in module " + MacroName + " is NOT a ForwardableEvent");
                        }
                        else
                            throw new Exception("The Command Name " + commandTypeName + " configured in module " + MacroName + " is NOT CORRECT");
                    }
                }
            }

        }

        public Command CreateFromRequest()
        {
            string commandTypeName = requestService.GetRequest(Constants.PARAM_COMMAND_TYPENAME) ?? string.Empty;
            if (string.IsNullOrEmpty(commandTypeName)) return null;
            Type commandType = Utilities.GetTypeFromName(commandTypeName);
            if (commandType != null)
            {
                Command retCommand = (Command)Activator.CreateInstance(commandType);
                foreach (PropertyInfo prop in commandType.GetProperties())
                {
                    if (prop.CanWrite)
                        prop.SetValue(retCommand, requestService.GetRequest(prop.Name), null);
                }
                return retCommand;
            }
            return null;
        }

    }
}