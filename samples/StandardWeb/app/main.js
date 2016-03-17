
require(['./require-conf'], function (config) {

    var devPath = '../bower_components';  // 开发时路径
    var releasePath = './vendor';  // 发布时路径
    require.config(config(devPath, releasePath));  // 进行 requirejs 配置

    require([
        'veronica'
    ], function (veronica) {

        // 创建应用程序
        var app = veronica.createApp({
            global: true,
            extensions: ['veronica-ui'],
            modules: [{
                name: 'basic',
                source: './modules',
                widgetPath: '',
                multilevel: true,
                hasEntry: false
            }],
            homePage: 'basic/home/index',
            autoParseWidgetName: false,
            autoBuildPage: true
        });

        app.launch().done(function () {

            require(['extension'], function (extension) {
                app.use(extension);
                app.ui.initSidebar();

                app.request.getJSON('/Home/GetModules').done(function(resp) {
                    app.existWidgets = resp;
                    app.page.start();
                });
            });

        });
    });
});

