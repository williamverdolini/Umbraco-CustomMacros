// Inherit namespace CustomMacros
var CustomMacros = window['CustomMacros'] || {};

// Global Namespace: CustomMacros
(function (CustomMacros) {

    var loader = function (loaderClass, isToShow) {
        if (loaderClass == '') return;
        if (isToShow)
            $('.' + loaderClass).hide();
        else
            $('.' + loaderClass).show();
    }

    CustomMacros.ajax = {
        execute: function (data, ActionUrl) {
            var divLoading = data.divLoading;
            var divResult = data.divResult;
            loader(divLoading, true);
            var eicexhr = $.ajax({
                cache: false,
                type: 'POST',
                url: ActionUrl,
                data: data,
                success: function (data, status) {
                    $('.' + divResult + ':first').html(data).trigger('create');
                    loader(divLoading, false);
                },
                error: function (data, status) {
                    console.log(data);
                    loader(divLoading, false);
                }
            });
        },
        executeInPopup: function (data, ActionUrl, popupTitle, popupOptions) {
            var divLoading = data.divLoading;
            var divResult = data.divResult;
            loader(divLoading, true);
            var eicexhr = $.ajax({
                cache: false,
                type: 'POST',
                url: ActionUrl,
                data: data,
                success: function (data, status) {
                    loader(divLoading, false);
                    var divObj = $('.' + divResult + ':first');
                    var divContent = $(data).html();
                    divObj.html(divContent);
                    divObj.modal('show');
                },
                error: function (data, status) {
                    console.log(data);
                    loader(divLoading, false);
                }
            });
        }
    }

    var CommandManager = function() {
        if (arguments.callee.instance)
            return arguments.callee.instance;
        arguments.callee.instance = this;

        this.CommandType = function (commandName) {
            this.name = commandName;
            this.listeners = [];
            this.setListener = function (moduleName) {
                if (this.listeners[moduleName] == undefined) {
                    this.listeners[moduleName] = new CustomMacros.commands.CommandMng.CommandListener(moduleName);
                }
                return this.listeners[moduleName];
            }
        };
        this.CommandListener = function (moduleName) {
            this.moduleName = moduleName; 
            this.commands = [];
        };
        this.Command = function (commandName, commandParamSet) {
            this.name = commandName;
            this.paramSet = commandParamSet;
        };
        this.CommandTypes = $.extend(
                [],
                {
                    set: function (commandTypeName) {
                        if (this[commandTypeName] == undefined) {
                            this[commandTypeName] = new CustomMacros.commands.CommandMng.CommandType(commandTypeName);
                        }
                        return this[commandTypeName];
                    }
                }
         );
        this.Commands = $.extend(
                [],
                {
                    addElement: function (commandItem) { this.push(commandItem); },
                    cloneCommandToParamSet: function (_index) {
                        return new CustomMacros.commands.CommandMng.Command(this[_index].name, this[_index].paramSet);
                    }
                }
        );
    };

    var getCommandManagerInstance = function () {
        var singletonClass = new CommandManager();
        return singletonClass;
    };

    CommandManager.prototype.fire = function () {
        var hitModules = {};
        var curHitModule = function (currentHashMap, moduleName) {
            if (currentHashMap[moduleName] == undefined) { currentHashMap[moduleName] = []; }
            return currentHashMap[moduleName];
        }

        for (var i = 0; i < this.Commands.length; i++) {
            if (this.CommandTypes[this.Commands[i].name] != null && this.CommandTypes[this.Commands[i].name].listeners != null) {
                var modulesListening = this.CommandTypes[this.Commands[i].name].listeners;
                for (var j in modulesListening) {
                    curHitModule(hitModules, modulesListening[j].moduleName).push(this.Commands.cloneCommandToParamSet(i));
                }
            }
        }
        for (var module in hitModules) {
            for (var i = 0; i < hitModules[module].length; i++) {
                window[module].fireAjax(module, hitModules[module][i].paramSet);
            }
        }
        while (this.Commands.length > 0) {
            this.Commands.pop();
        }
    };

    var fireMethodFwd = CommandManager.prototype.fire;

    CustomMacros.commands = {
        CommandMng: getCommandManagerInstance(),
        fire: function (command) {
            CustomMacros.commands.CommandMng.Commands.addElement(command);
            CustomMacros.commands.CommandMng.fire();
        }
    }

})(CustomMacros)