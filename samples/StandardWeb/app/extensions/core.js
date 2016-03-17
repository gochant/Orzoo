define([
    'tiny',
    'jquery.scrollbar'
], function () {
    return function (app) {
        var $ = app.core.$;
        var _ = app.core._;

        // 禁用全局缓存
        $.ajaxSetup({ cache: false });

        app.use(function (app) {


            // 框架扩展

            app.view.base.linkHandler = function (e, app) {
                e.preventDefault();
                var $ = app.core.$;
                var _ = app.core._;
                var $target = $(e.currentTarget);
               

                var data = $target.data();
                delete data.bind;
                delete data.action;

                var href = $target.attr('href');

                href = _.isEmpty(data) ? href : href + '?' + $.param(data);

                app.router.navigate(href, { trigger: true });
            }

            app.ui.dialog.defaults.backdropOpacity = 0.3;

            app.view.base.viewWindow = function (viewName, viewInitializer, options, dlgOptions) {
                if (_.isObject(dlgOptions)) {
                    dlgOptions.options = _.extend({}, dlgOptions.options, { modal: true });
                }
                return this.window($.extend({
                    name: 'wnd_' + viewName,
                    children: [{
                        type: 'view',
                        name: viewName,
                        initializer: viewInitializer,
                        options: options
                    }]
                }, dlgOptions));
            }

            var baseActiveUI = app.view.base._activeUI;
            app.view.base._activeUI = function () {

                baseActiveUI();

                // 验证
                app.formUtil.enableValidate(this.$('form'));

                // 替换框
                app.formUtil.enablePlaceholder(this.$('form'));

                // 日期
                this.$('input.date').datetimepicker({
                    format: 'yyyy/mm/dd',
                    todayBtn: 'linked',
                    startView: 'month',
                    language: 'zh-CN',
                    minView: 2,
                    autoclose: true
                });

                // TODO: 日期时间

                // Ajax 表单
                this.submitted = this.submitted || $.noop;
                var boundSubmitted = _.bind(this.submitted, this, app);
                app.formUtil.enableAjaxSubmit(this.$('form.ajaxform'), boundSubmitted);

            }

            // 定义分组source
            if (app.data) {
                app.data.remoteComplexGroupSource = function (url,groupfield) {
                    return app.data.source({
                        pageSize: 20,
                        page: 1,
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        schema: {
                            //type: 'json',
                            model: app.data.model(),
                            data: 'data',
                            total: 'total'
                        },
                        transport: {
                            read: {
                                url: url,
                                type: 'POST',
                                dataType: "json",
                                contentType: "application/json; charset=utf-8"
                            },
                            parameterMap: function (data, type) {
                                if (type === "read") {
                                    return JSON.stringify(data);
                                }
                                return data;
                            }
                        },
                        group: { field: (groupfield || 'AreaDisplay') }
                    });
                };
            }

        });


        if ($.validator) {

            $.validator.unobtrusive.adapters.addSingleVal("greaterthan", "other");

            $.validator.addMethod("greaterthan",
                function (val, element, other) {
                    var modelPrefix = element.name.substr(0, element.name.lastIndexOf(".") + 1);
                    var otherVal = $("[name=" + modelPrefix + other + "]").val();
                    if (val && otherVal) {
                        if (val < otherVal) {
                            return false;
                        }
                    }
                    return true;
                }
            );
        }


        // 自动监听事件

        app.notify.autoReport = function () {
            $(document).ajaxSuccess(function (e, xhr, f, resp) {
                var respType = ["", "info", "success", "warn", "error"];
                if (resp && resp.type != null) {

                    var msg = resp.msg;  // 后台返回信息

                    // 未验证或授权
                    if (!resp.success) {
                        // 用户信息丢失，强制刷新
                        if (resp.data === 'UserNotFound') {
                            app.notify.warn('会话过期，请重新登录');
                            setTimeout(function () {
                                window.location.reload(true);
                            }, 1000);
                        }
                        if (resp.data === 'Unauthorized') {
                            app.notify.warn('对不起，您没有访问该功能的权限！');
                        }

                        // 将错误组织成信息
                        if (_.isArray(resp.data)) {
                            msg += '：' + _.map(resp.data, function (error) {
                                return error.Message;
                            }).join('<br />');
                        }

                        console.log(resp.data);
                    }

                    if (resp.type !== 0) {
                        app.notify[respType[resp.type]](msg);
                    }
                }
            }).ajaxError(function (e, xhr, f, resp) {
                // app.notify.error('数据异常');
                console.error(xhr.responseTex);
            });
        }

        // 页面加载监听

        var pageLoading = function (a) {
            var b = $("#glb_loading");
            "hide" === a ? b.hide() : b.show().html(a);
        }

        app.sandbox.on('pageLoading', function () {
            pageLoading();
        });
        app.sandbox.on('pageLoaded', function () {
            pageLoading('hide');
            $.noty.closeAll();
        });



        app.ui.initSidebar = function () {
            $('#sidebar-mini').submenu();

            app.sandbox.on('pageLoaded', function (pageName) {
                // 切换菜单显示
                var $sidebar = $('#sidebar-mini');
                var $active = $sidebar.find('a.active');

                if ($active.attr('href') !== '#' + pageName) {
                    $active.removeClass('active');
                    $sidebar.find('[href$="' + pageName + '"]').parent('li').addClass('active').parents('li').addClass('active open');
                };
            });
        }

        app.core.logger.write = function (output, args) {
            var parameters = Array.prototype.slice.call(args);
            parameters.unshift(this.name + ":");

            if (_.isFunction(output)) {
                output.apply(console, parameters);
            } else {
                output(parameters.join(' '));
            }

        }

        $.extend($.validator.messages, {
            required: "这是必填字段",
            remote: "请修正此字段",
            email: "请输入有效的电子邮件地址",
            url: "请输入有效的网址",
            date: "请输入有效的日期",
            dateISO: "请输入有效的日期 (YYYY-MM-DD)",
            number: "请输入有效的数字",
            digits: "只能输入数字",
            creditcard: "请输入有效的信用卡号码",
            equalTo: "你的输入不相同",
            extension: "请输入有效的后缀",
            maxlength: $.validator.format("最多可以输入 {0} 个字符"),
            minlength: $.validator.format("最少要输入 {0} 个字符"),
            rangelength: $.validator.format("请输入长度在 {0} 到 {1} 之间的字符串"),
            range: $.validator.format("请输入范围在 {0} 到 {1} 之间的数值"),
            max: $.validator.format("请输入不大于 {0} 的数值"),
            min: $.validator.format("请输入不小于 {0} 的数值")
        });

        $.fn.tooltip.Constructor.DEFAULTS.placement = 'bottom';
        $.extend(true, $.validator, {
            prototype: {
                applyTooltipOptions: function (element, message) {
                    var options = {
                        animation: $(element).data('animation') || true,
                        html: $(element).data('html') || false,
                        placement: $(element).data('placement') || 'bottom',
                        selector: $(element).data('selector') || false,
                        title: $(element).attr('title') || message,
                        trigger: $.trim('manual ' + ($(element).data('trigger') || '')),
                        delay: $(element).data('delay') || 0,
                        container: $(element).data('container') || false
                    };

                    if (this.settings.tooltip_options && this.settings.tooltip_options[element.name]) {
                        $.extend(options, this.settings.tooltip_options[element.name]);
                    }
                    /* jshint ignore:start */
                    if (this.settings.tooltip_options && this.settings.tooltip_options['_all_']) {
                        $.extend(options, this.settings.tooltip_options['_all_']);
                    }
                    /* jshint ignore:end */
                    return options;
                }

            }
        });

        app.notify.autoReport();

    };

});