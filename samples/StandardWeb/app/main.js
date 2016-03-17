
require(['./require-conf'], function (config) {

    var devPath = '../bower_components';  // ����ʱ·��
    var releasePath = './vendor';  // ����ʱ·��
    require.config(config(devPath, releasePath));  // ���� requirejs ����

    require([
        'veronica'
    ], function (veronica) {

        // ����Ӧ�ó���
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

