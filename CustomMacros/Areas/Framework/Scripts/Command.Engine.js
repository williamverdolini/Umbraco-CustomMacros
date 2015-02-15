// Inherit namespace winic
var CustomMacros = window['CustomMacros'] || {};

// Global Namespace: winic.wizard
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
                    //if (!divObj.is(':data(dialog)')) {
                    //    divObj.remove();
                    //    divObj.dialog(popupOptions ||
                    //                                {
                    //                                    autoResize: true,
                    //                                    height: "auto",
                    //                                    width: "auto",
                    //                                    hide: "fold",
                    //                                    title: popupTitle,
                    //                                    resizable: true,
                    //                                    position: [($(window).width() / 2) - (630 / 2), 150]
                    //                                }
                    //                            );
                    //}
                    //divObj.dialog("open").show();

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
        // Metodo di apertura popup
        this.pageInPopup = function (form, page, postData, popupOptions) {
            var _DivContainer = "CustomMacrosdivContainer";
            var _iframePage = "CustomMacrosiframePage";
            var _divLoading = "CustomMacrosdivLoading";
            var _poJSON = window["eval"]("({" + popupOptions + "})");

            //Creazione DIV container dell'iframe
            var divContainer = $('#' + _DivContainer);
            if (divContainer == null || (divContainer != null && divContainer.length <= 0)) {
                divContainer = $('<div id="' + _DivContainer + '"></div>');
                divContainer.appendTo('body');
            }
            //Creazione dell'iframe ed inserimento nel DIV Container
            var iframePage = $('#' + _iframePage);
            if (iframePage == null || (iframePage != null && iframePage.length <= 0)) {
                iframePage = $('<iframe id="' + _iframePage + '" name="' + _iframePage + '" frameborder="2"></iframe>');
                iframePage.width("100%").height("100%");
                iframePage.load(function () {
                    $(this).show();
                    $('#' + _divLoading).hide();
                });
                // Inserimento nel DIV Container
                iframePage.appendTo($(divContainer));
            }
            iframePage.hide();

            //Creazione del div per il loading ed inserimento nel DIV Container
            var divLoading = $('#' + _divLoading);
            if (divLoading == null || (divLoading != null && divLoading.length <= 0)) { //style="display:none;" 
                divLoading = $('<div id="' + _divLoading + '" class="CustomMacrosloading" ></div>');
                // Inserimento nel DIV Container
                divLoading.appendTo($(divContainer));
            }

            // Apertura in dialog del DIV Container
            divContainer.dialog(
                            $.extend(
                                {
                                    modal: true,
                                    resizable: false,
                                    height: 800,
                                    width: 600,
                                    title: ''
                                },
                                _poJSON
                                )
                            );

            //Inserimento del form-evento in pagina e sottomissione del form verso la pagina caricata nell'iframe
            $(form).appendTo('body').attr('method', 'post').attr('action', page).attr('target', _iframePage).submit();
            divLoading.show();
        }


    };

    var getCommandManagerInstance = function () {
        var singletonClass = new CommandManager();
        return singletonClass;
    };

    /********************************************************************
    MANAGER: metodi "statici" che gestiscono le relazioni tra gli eventi lanciati
    ed i modulin in ascolto di tali eventi. In particolare:
    1) fire: lancia gli eventi raccolti nel contenitore (Events) passandoli
    ai moduli in ascolto degli eventi
    *********************************************************************/
    CommandManager.prototype.fire = function () {
        //hashmap locale dei soli moduli colpiti dagli eventi lanciati
        var hitModules = {};
        //funzione interna (locale) di caricamento comodo dell'hashMap
        var curHitModule = function (currentHashMap, moduleName) {
            if (currentHashMap[moduleName] == undefined) { currentHashMap[moduleName] = []; }
            return currentHashMap[moduleName];
        }

        // scorro gli eventi lanciati
        for (var i = 0; i < this.Commands.length; i++) {
            // per tutti i listener in ascolto dell'evento
            // creo il set di eventi da mandare a server
            // e li aggrego sull'hashmap dei moduli "colpiti"
            if (this.CommandTypes[this.Commands[i].name] != null && this.CommandTypes[this.Commands[i].name].listeners != null) {
                var modulesListening = this.CommandTypes[this.Commands[i].name].listeners;
                for (var j in modulesListening) {
                    curHitModule(hitModules, modulesListening[j].moduleName).push(this.Commands.cloneCommandToParamSet(i));
                }
            }
        }
        // l'hashmap dei moduli colpiti dagli eventi è popolato
        // a questo punto è possibile chiamare una funzione ajax presente su tutti i moduli
        // a cui passare l'insieme degli eventi d'interesse
        for (var module in hitModules) {
            for (var i = 0; i < hitModules[module].length; i++) {
                window[module].fireAjax(module, hitModules[module][i].paramSet);
            }
        }

        // Svuoto il vettore di eventi caricato.    
        while (this.Commands.length > 0) {
            this.Commands.pop();
        }
    };

    var fireMethodFwd = CommandManager.prototype.fire;
    CommandManager.prototype.fire = function () {
        // scorro gli eventi lanciati
        var trovato = false;
        for (var i = 0; i < this.Commands.length; i++) {
            // Se è presente la proprietà ForwardPage, vengono inviati in POST tutti i parametri dell'evento
            // all'indirizzo indicato nella proprietà.
            // L'invio è realizzato tramite la creazione run-time di una form di hidden fields con i dati
            // dell'evento inviato
            var fp = this.Commands[i].paramSet.ForwardPage;
            if (fp != undefined) {
                var form = $('<form></form>');
                $(form).hide();
                for (j in this.Commands[i].paramSet) {
                    var input = $('<input type="hidden" />').attr('name', j).val(this.Commands[i].paramSet[j]);
                    $(form).append(input);
                }

                // Nel caso di ForwardPage incapsulato in parantesi quadre viene delegato al gestione della pagina in Popup
                if (fp[0] == "[" && fp[fp.length - 1] == "]") {
                    fp = fp.substring(1, fp.length - 1);
                    $EiceEventMng.pageInPopup(form, fp, this.Commands[i].paramSet, this.Commands[i].paramSet.PopupOptions);
                }
                else
                    //'body' erica 04/12/2014 per fare in modo che quando sono in popup l'evento venga forwardato alla pagina fuori dalla popup
                    $(form).appendTo(parent.document.body).attr('method', 'post').attr('action', fp).submit();

                // Svuoto il vettore di eventi caricato.    
                while (this.Commands.length > 0) {
                    this.Commands.pop();
                }

                trovato = true;
                return;
            }
        }
        if (!trovato)
            return $.proxy(fireMethodFwd, this)();
    };

    CustomMacros.commands = {
        CommandMng: getCommandManagerInstance(),
        fire: function (command) {
            CustomMacros.commands.CommandMng.Commands.addElement(command);
            CustomMacros.commands.CommandMng.fire();
        }
    }

})(CustomMacros)