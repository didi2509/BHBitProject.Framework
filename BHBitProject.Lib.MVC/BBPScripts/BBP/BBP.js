var BBP =
    {
        $: jQuery
        , init: function () {
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
        , LoadScript: function (scriptPath) {
            var script = document.createElement('script');
            script.setAttribute('type', 'text/javascript');
            script.setAttribute('src', scriptPath);
            document.body.appendChild(script);
        }
    };