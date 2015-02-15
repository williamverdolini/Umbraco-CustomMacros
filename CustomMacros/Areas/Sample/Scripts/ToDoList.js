// Inherit namespace winic
var CustomMacros = window['CustomMacros'] || {};

// Global Namespace: winic.wizard
(function (CustomMacros) {
    CustomMacros.ToDoList = {
        SortBy: function (command) {
            CustomMacros.commands.fire(command);
        }
    }
})(CustomMacros);