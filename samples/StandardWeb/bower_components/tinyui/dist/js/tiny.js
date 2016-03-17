; (function ($, window, document, undefined) {

    "use strict";

    var pluginName = "submenu";
    var defaults = {};
    var MIN_CLASS = 'sidebar-min';

    function Plugin(element, options) {
        this.element = element;
        this.minimized = $(this.element).hasClass(MIN_CLASS);
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.toggleIcons = ['fa-angle-double-left', 'fa-angle-double-right'];
        this.arrowIcons = ['fa-angle-left', 'fa-angle-down'];
        this.init();
    }

    $.extend(Plugin.prototype, {
        init: function () {
            var $navlist = $(this.element).find('.nav-list');
            var $toggle = $(this.element).find('.sidebar-toggle');
            var me = this;

            $.each($navlist.find('li'), function (i, li) {
                var $li = $(li);
                var $submenu = $li.children('.submenu');
                var $ddtoggle = $li.children('.dropdown-toggle');
                var arrowCls = $li.hasClass('open') ? me.arrowIcons[1] : me.arrowIcons[0];
                if ($submenu.length > 0) {
                    if ($li.children('.arrow').length === 0) {
                        $submenu.before('<b class="arrow"></b>');
                    }
                    if ($ddtoggle.children('.arrow').length === 0) {
                        $ddtoggle.append('<b class="arrow fa ' + arrowCls + '"></b>');
                    }
                }
            });

            $navlist.on('click', 'a', function (e) {
                var $link = $(e.currentTarget);
                var $li = $link.parent();

                if ($li.children('.submenu').length === 0) {
                    me.active($li);
                }

                if ($link.hasClass('dropdown-toggle')) {
                    if ($li.parent().hasClass('nav-list') && me.minimized || $li.hasClass('hover')) {
                        return;
                    }
                    me.toggleDisplay($li);
                }

            });
            $toggle.on('click', function (e) {
                e.preventDefault();
                if (me.minimized) {
                    $(me.element).removeClass(MIN_CLASS);
                    $toggle.children().removeClass(me.toggleIcons[1]).addClass(me.toggleIcons[0]);
                } else {
                    $(me.element).addClass(MIN_CLASS);
                    $toggle.children().removeClass(me.toggleIcons[0]).addClass(me.toggleIcons[1]);
                    // 隐藏所有submenu
                    //$(me.element).find('.submenu').hide();
                }
                me.minimized = !me.minimized;
            });
        },
        toggleDisplay: function ($li) {
            if ($li.hasClass('open')) {
                this._toggleSubmenu($li, 'hide');
            } else {
                this._toggleSubmenu($li, 'show');
                this._toggleSubmenu($li.siblings('.open'), 'hide');
            }
        },
        _toggleSubmenu: function ($item, mode, callback) {
            var me = this;
            var animate = 'slideDown';
            var rmvClsIdx = 0;
            var addClsIdx = 1;
            var openHd = 'addClass';
            callback || (callback = function () { });

            if (mode === 'hide') {
                animate = 'slideUp';
                rmvClsIdx = 1;
                addClsIdx = 0;
                openHd = 'removeClass';
            }

            $item.children('.submenu')[animate]('fast', function () {
                $item[openHd]('open');
                $item.children('.dropdown-toggle').children('.arrow')
                    .removeClass(me.arrowIcons[rmvClsIdx])
                    .addClass(me.arrowIcons[addClsIdx]);
                callback();
            });
            return this;
        },
        active: function (selector) {
            $(this.element).find('.active').removeClass('active');
            $(selector).addClass('active')
                .parentsUntil(this.element, 'li').addClass('active');
        }
    });

    $.fn[pluginName] = function (options) {
        return this.each(function () {

            var $this = $(this);
            var data = $this.data('plugin_' + pluginName);

            if (!data) {
                $this.data('plugin_' + pluginName, (data = new Plugin(this, options)))
            }

            if (typeof options == 'string') {
                data[options]();
            }

        });
    };

})(jQuery, window, document);

; (function ($, window, document, undefined) {

    "use strict";

    // Box class IE bugfix
    var pluginName = "boxBugfix";

    // isIE('lte IE 9')
    var isIE = function (condition) {
        var b = document.createElement('b');
        b.innerHTML = '<!--[if ' + condition + ']><i></i><![endif]-->';
        return b.getElementsByTagName('i').length === 1;
    }

    var now = Date.now || function () {
        return new Date().getTime();
    };

    // thx: https://github.com/jashkenas/underscore
    var debounce = function (func, wait, immediate) {
        var timeout, args, context, timestamp, result;

        var later = function () {
            var last = now() - timestamp;

            if (last < wait && last >= 0) {
                timeout = setTimeout(later, wait - last);
            } else {
                timeout = null;
                if (!immediate) {
                    result = func.apply(context, args);
                    if (!timeout) context = args = null;
                }
            }
        };

        return function () {
            context = this;
            args = arguments;
            timestamp = now();
            var callNow = immediate && !timeout;
            if (!timeout) timeout = setTimeout(later, wait);
            if (callNow) {
                result = func.apply(context, args);
                context = args = null;
            }

            return result;
        };
    };

    var relayout = function ($el) {
        $el.length || ($el = $('body'));
        $.each($el.find('.box-cell'), function (i, el) {
            $(el).height(0);
            setTimeout(function () {  // IE8 需等JS队列执行完 reflow 后才能再计算高度
                $(el).height($(el).parent().height());
            }, 0);
        });
    };

    $.fn[pluginName] = function () { };

    /*@cc_on $('body').addClass('oldie'); @*/

    if ($('body').hasClass('oldie')) {
        $.fn[pluginName] = function () {
            relayout($(this));
        };

        $(window).resize(debounce(relayout, 300));
    }
    setTimeout(function () {
        $('body')[pluginName]();
    }, 0);

})(jQuery, window, document);