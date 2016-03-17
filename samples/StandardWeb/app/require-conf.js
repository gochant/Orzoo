// requirejs
(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(function () {
            return factory();
        });
    } else if (typeof exports === 'object') {
        // Node.js
        module.exports = factory();
    } else {
        // Browser globals
        root.RequireConf = factory();
    }
}(this, function () {

    return function (devPath, releasePath) {

        var config = {
            debug: true,
            shim: {
                'underscore': { 'exports': '_' },
                'tiny': { 'deps': ['jquery'] },
                'jquery.scrollbar': { 'deps': ['jquery'] }
            }
        };

        var framePath = config.debug === true ? devPath : releasePath;

        // 第三方库路径
        config.paths = {
            'underscore': framePath + '/underscore/underscore',
            'jquery': framePath + '/jquery/dist/jquery',
            'text': framePath + '/requirejs-text/text',
            'ver': framePath + '/requirejs-ver/ver',
            'css': framePath + '/require-css/css',
            'normalize': framePath + '/require-css/normalize',
            'css-builder': framePath + '/require-css/css-builder',
            'veronica': framePath + '/veronica/dist/veronica',
            'veronica-ui': framePath + '/veronica-ui/dist/veronica-ui',
            'tiny': framePath + '/tinyui/dist/js/tiny',
            'jquery.scrollbar': framePath + '/jquery.scrollbar/jquery.scrollbar'
        }


        return config;
    }

}));
