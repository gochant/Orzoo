define([
], function () {
    return function (app) {
        var $ = app.core.$;
        var _ = app.core._;
        var kendo = window.kendo;

        // 普通视图扩展

        function removeData(view, data, url, content) {
            url || (url = view.url('remove'));
            content || (content = "确认删除这条数据？");
            app.ui.confirm(content, function () {
                app.request.postComplex(url, data).done(function (resp) {
                    if (resp.success) {
                        view.trigger('removed', data.id || data.Id, data);
                    }
                });
            });
        }

        function notifyWarning(condition, msg) {
            if (condition) {
                app.notify.warn(msg);
            }
            return condition;
        }

        app.viewHelper || (app.viewHelper = {});
        app.viewHelper.handleNull = function (obj) {
            return JSON.parse(JSON.stringify(obj).replace(/null/g, "\"\""));
        }
        app.viewHelper.removeData = function (id) {
            removeData(this, { id: id });
        }


        // 服务器模板视图

        app.serverTplViewHelper = {
            templateUrl: function () {
                var $ = this.options.sandbox.app.core.$;
                return this.url('template') + '?' + $.param({ state: this.options.state });
            }
        };

        // 列表视图扩展

        var listType = {
            grid: {
                getSelect: function (list) {
                    return list.select();
                },
                getData: function (list, select) {
                    return list.dataItem($(select));
                }
            },
            listview: {
                getSelect: function (list) {
                    return list.select();
                },
                getData: function (list, select) {
                    var uid = $(select).data().uid;
                    var source = list.dataSource;
                    if (source.getByUid) {
                        return source.getByUid(uid);
                    } else {
                        return _.find(source, function (item) {
                            return item.uid === uid;
                        });
                    }
                }
            },
            list: {
                getSelect: function (list) {
                    return list.find('.k-state-selected');
                },
                getData: function (list, $selecteds) {
                    return _.map($selecteds, function (el) {
                        return $(el).data();
                    });
                }
            },
            row: {
                getSelect: function (row) {
                    return $(row).closest('tr');
                },
                getData: function (list, $selecteds) {
                    return _.map($selecteds, function (el) {
                        return $(el).data();
                    });
                }
            }
        }

        app.listUtil.getSelectedItem = function (list, type) {

            if (type == null) {
                if (list.dataItem) {
                    type = 'grid';
                } else {
                    if (list.dataSource) {
                        type = 'listview';
                    } else {
                        type = 'list';
                    }
                }
            }

            var items = listType[type].getData(list, listType[type].getSelect(list));

            return items.length === 0 ? null : (items.length === 1 ? items[0] : items);
        };

        app.listViewHelper || (app.listViewHelper = {});

        app.listViewHelper.confirmSelected = function (callback, $list, type) {
            var data = this.getSelectedItem($list, type);

            if (notifyWarning((data == null), "未选择数据")) return;
            if (notifyWarning((data && data.length), "只能选择一条数据进行操作")) return;

            callback.call(this, data, this.getDataId(data));
        }

        app.listViewHelper.getDataId = function (data) {
            return data.id || data.Id || data.ID;
        }

        app.listViewHelper.$list = function () {
            return this.$('.fn-list');
        }

        app.listViewHelper.getSelectedItem = function ($list, type) {
            $list || ($list = this.$list());
            var instance = this.instance($list);
            if (instance == null) {
                instance = $list;
            }

            var selected = app.listUtil.getSelectedItem(instance, type);

            return selected;
        }

        app.listViewHelper.removeItem = function (selector, type) {
            var me = this;
            this.confirmSelected(function (data, id) {

                app.ui.confirm("确认删除这条数据？", function () {
                    app.request.postComplex(me.url('remove'), { id: id }).done(function (resp) {
                        if (resp.success) {
                            me.trigger('removed', id, data);
                        }
                    });
                });
            }, selector, type);
        }

        app.listViewHelper.getRowId = function (el) {
            return $(el).closest('tr').data('id');
        }



        // 编辑视图扩展

        app.editViewHelper || (app.editViewHelper = {});
        app.editViewHelper.STATE = {
            EDIT: 'edit',
            ADD: 'add',
            DETAIL: 'detail'
        };
        app.editViewHelper.loadData = function (request) {
            request || (request = this.loadUrl());
            var me = this;
            return app.request.getJSON(request.url, request.data).done(function (res) {
                if (res && res.success) {
                    me.model({
                        data: res.data
                    });
                }
            });
        }
        app.editViewHelper.loadUrl = function (url) {
            url || (url = this.url('entity'));
            var data = this.options.id ? { id: this.options.id } : this.options.data;
            return { url: url, data: data };
        }
        app.editViewHelper.save = function (params) {
            var isValid = this.$('form').valid();
            if (!isValid) return null;
            var result = this.model('data').toJSON();
            var me = this;
            var url = this.options.id ? (this.options.isforce ? this.url('forceModify') : this.url('modify')) : this.url('create');
            return app.request.postComplex(url, result).done(function (resp) {
                if (resp.success) {
                    me.trigger('saved', resp, params);
                    me.options.parentWnd && me.options.parentWnd.close();
                }
            });
        }
        app.editViewHelper.changeState = function (state) {
            this.options.state = state;
            this.render();
        }

        app.editViewHelper.initDlgAutoAdaptive = function (app, _) {
            var me = this;
            if (me.options.parentWnd && !me.options.windowOptions) {
                me.options.parentWnd.core.addEventListener('show', function () {
                    if (me.state.isRendered) {
                        me._dlgAutoAdaptiveSetOptions(_);
                    }
                    me.on('rendered', function () {
                        me._dlgAutoAdaptiveSetOptions(_);
                    });
                });
            }
        };

        app.editViewHelper._dlgAutoAdaptiveSetOptions = function (_) {
            var me = this;
            _.defer(function () {
                var $keyEl = me.$(me.options.keyElement);
                me.options.parentWnd.setOptions({
                    title: ($keyEl.data('title') || '数据') + me._getTitleSuffix(),
                    width: $keyEl.outerWidth(),
                    height: $keyEl.outerHeight() > 500 ? 500 : $keyEl.outerHeight() + 20
                });
            });
        };

        app.editViewHelper._getTitleSuffix = function () {
            var result = '新增';
            switch (this.options.state) {
                case 'edit':
                    result = '修改';
                    break;
                case 'add':
                    result = '新增';
                    break;
                case 'detail':
                    result = '详情';
                    break;
                default:
                    break;
            }
            return result;
        };

        // 简单列表视图
        app.simpleSelectedViewHelper = {
            selectedHandler: function (e, app) {
                var $target = $(e.currentTarget);
                this.select($target);
            },
            select: function ($target, id) {
                if ($target == null) {
                    if (id == null) {
                        // this.trigger('selected', {});
                        return;
                    }
                    $target = this.$('a[data-id=' + id + ']');
                }
                if ($target.length === 0) {
                    this.trigger('selected', {});
                } else {
                    if (!$target.hasClass('disabled') && !$target.hasClass('k-state-selected')) {
                        this.$('.k-state-selected').removeClass('k-state-selected');
                        $target.addClass('k-state-selected');
                        this.trigger('selected', $target.data());
                    }
                }
            },
            getSelectedId: function () {
                return this.model('selected').id;
            }
        };
        app.simpleSelectedViewHelper.confirmDataExist = function (callback, data) {
            if (data != null) {
                callback.call(this, data);
            } else {
                app.notify.warn("未选择数据");
            }
        }
        app.simpleSelectedViewHelper.removeItem = function (data) {
            var me = this;
            this.confirmDataExist(function (id) {

                app.ui.confirm("确认删除这条数据？", function () {
                    app.request.post(me.url('remove'), { id: id }).done(function (resp) {
                        if (resp.success) {
                            me.trigger('removed', id);
                        }
                    });
                });
            }, data);
        }


    };

});