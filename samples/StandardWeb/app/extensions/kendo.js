define([
    './datatable',
    '../assets/kendo.messages.zh-CN'
], function (datatable) {
    return function (app) {
        var $ = app.core.$;
        var _ = app.core._;
        var kendo = window.kendo;

        kendo.cultures["zh-CN"] = {
            name: "zh-CN",
            numberFormat: {
                pattern: ["-n"],
                decimals: 2,
                ",": ",",
                ".": ".",
                groupSize: [3],
                percent: {
                    pattern: ["-n%", "n%"],
                    decimals: 2,
                    ",": ",",
                    ".": ".",
                    groupSize: [3],
                    symbol: "%"
                },
                currency: {
                    pattern: ["$-n", "$n"],
                    decimals: 2,
                    ",": ",",
                    ".": ".",
                    groupSize: [3],
                    symbol: "¥"
                }
            },
            calendars: {
                standard: {
                    days: {
                        names: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
                        namesAbbr: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
                        namesShort: ["日", "一", "二", "三", "四", "五", "六"]
                    },
                    months: {
                        names: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                        namesAbbr: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
                    },
                    AM: ["上午", "上午", "上午"],
                    PM: ["下午", "下午", "下午"],
                    patterns: {
                        d: "yyyy/M/d",
                        D: "yyyy'年'M'月'd'日'",
                        F: "yyyy'年'M'月'd'日' H:mm:ss",
                        g: "yyyy/M/d H:mm",
                        G: "yyyy/M/d H:mm:ss",
                        m: "M'月'd'日'",
                        M: "M'月'd'日'",
                        s: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
                        t: "H:mm",
                        T: "H:mm:ss",
                        u: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
                        y: "yyyy'年'M'月'",
                        Y: "yyyy'年'M'月'"
                    },
                    "/": "/",
                    ":": ":",
                    firstDay: 1
                }
            }
        }
        kendo.culture("zh-CN");

        // date 绑定
        kendo.data.binders.date = kendo.data.Binder.extend({
            init: function (element, bindings, options) {
                kendo.data.Binder.fn.init.call(this, element, bindings, options);

                this._change = $.proxy(this.change, this);

                $(this.element).on('change', this._change);

            },

            refresh: function () {

                var date = this.bindings.date.get();
                var dateTxt;
                if (!date) {
                    dateTxt = "";
                } else {
                    if (_.isString(date)) {
                        date = kendo.parseDate(date);
                    }
                    var format = $(this.element).data('format') ||
                     kendo._extractFormat('yyyy/MM/dd');
                    dateTxt = kendo.toString(date, format);
                }
                if ('value' in this.element) {
                    this.element.value = dateTxt;
                } else {
                    this.element.innerHTML = dateTxt;
                }
            },
            change: function () {
                var value = this.element.value;
                this.bindings.date.set(value);
            }
        });


        app.use(function (app) {

            app.ext || (app.ext = {});

            app.ext.url = function (url) {
                return siteRoot + url;
            }

            app.ext.nameCheckNullFor = function (item) {
                if (item == null)
                    return "-";
                return item;
            }

            // 下载文件
            var isChromeFrame = function () {
                var ua = navigator.userAgent.toLowerCase();
                return ua.indexOf('chrome') >= 0 && window.externalHost;
            };
            app.ext.download = function (settings) {
                settings || (settings = {}); //eg: { url: '', data: [object] }
                if (settings.url == undefined) {
                    return;
                }
                if (!_.isString(settings.data)) {
                    settings.data = $.param(settings.data, true);
                }
                if (!isChromeFrame()) {  //当使用ChromeFrame时，采用新窗口打开
                    if ($('#global-download-iframe').length === 0) {
                        $('<iframe id="global-download-iframe" src="" style="width:0;height:0;display: inherit;border:0;" \>').appendTo(document.body);
                    }
                    $('#global-download-iframe').attr('src', settings.url + '?' + settings.data);
                } else {
                    window.open(settings.url + '?' + settings.data, "newwindow");
                }
            };
        });

      //  datatable(app);
    };
});