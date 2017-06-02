var BBP =
    {
        $: jQuery
        , BBPPath: ""
        , init: function (BBPPath) {
            this.BBPPath = BBPPath;
            BBP.Ready.Execute();
        }
        , Ready: {
            inits: []
            , Add: function (method) {
                this.inits.push(method);
            }
            , Execute: function () {

                var itens = this.inits.length;

                for (var i = 0; i < itens; i++)
                    this.inits[i]();

                this.inits = null;
            }

        }
        , LoadLibrary: function (name) {
            this.LoadScript(name);
            if (BBP[name].init)
                BBP[name].init();
        }

        , LoadScript: function (strScriptPath) {

            var script = document.createElement('script');
            script.setAttribute('type', 'text/javascript');
            script.setAttribute('src', this.BBPPath + strScriptPath + '.js');
            document.body.appendChild(script);

        }

        , Utils: LoadLibrary("Utils")
        , Validate: LoadLibrary("Validate")
        , Ajax: LoadLibrary("Ajax")
        , DateTime: LoadLibrary("DateTime")
        , JSON: LoadLibrary("JSON")
        , Message: LoadLibrary("Message")
        , Componentes: {
            DropDowns: LoadLibrary("Components/DropDowns")
        }
    };