define([
    './extensions/core',
    './extensions/kendo',
    './extensions/viewHelper',
    './extensions/subpages',
    './extensions/normalView'
], function (core, kendo, viewHelper, subpages, normalView) {

    return function (app) {
        app.use(core);
        app.use(kendo);
        app.use(viewHelper);
        app.use(subpages);
        app.use(normalView);
    };

});