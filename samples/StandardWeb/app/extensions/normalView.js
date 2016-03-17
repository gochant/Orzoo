define([
], function () {
    return function (app) {
        var $ = app.core.$;
        var _ = app.core._;

        app.widgetsPattern = {
            "normalListIndexView": [/-index/],
            "normalEditView": [/-edit/]
        }

        app.widget.register('normalListIndexView', {
            defaults: {
                autoAction: true,
                url: {
                    template: '{controller}/ListTemplate',
                    read: '{controller}/List',
                    remove: '{controller}/DeleteSave'
                }
            },
            mixins: function (app) {
                return [app.serverTplViewHelper, app.listViewHelper];
            },
            staticModel: function (app) {
                return {
                    source: app.data.remoteComplexSource(this.url('read'))
                }
            },
            initAttr: function (app) {
            },
            read: function () {
                this.model('source').filter([]);
            },
            modelBound: function (app) {
                this.read();
            },
            listen: function () {
                this.listenTo([['edit', 'saved'], [this, 'removed']], function () {
                    this.read();
                });
            },
            _editView: function (data) {
                this.viewWindow('edit', 'normalEditView', data, { footer: true });
            },
            // #region Handler
            createHandler: function (e, app) {
                this._editView({ data: {} });
            },
            refreshHandler: function () {
                this.read();
            },
            detailHandler: function (e) {
                this.confirmSelected(function (data, id) {
                    this._editView({ id: id, state: 'detail' });
                }, $(e.currentTarget), 'row');
            },
            modifyHandler: function (e, app) {
                this.confirmSelected(function (data, id) {
                    this._editView({ id: id });
                }, $(e.currentTarget), 'row');
            },
            removeHandler: function (e, app) {
                this.removeItem($(e.currentTarget), 'row');
            }
        });

        app.widget.register('normalEditView', {
            defaults: {
                autoAction: true,
                keyElement: 'form',
                url: {
                    'template': '{controller}/EditTemplate',
                    'entity': '{controller}/Entity',
                    'create': '{controller}/CreateSave',
                    'modify': '{controller}/ModifySave'
                }
            },
            mixins: function (app) {
                return [app.serverTplViewHelper, app.editViewHelper];
            },
            init: function (app, _) {
                this._invoke('initDlgAutoAdaptive');
            },
            rendered: function () {
                this.loadData();
            },
            saveHandler: function () {
                this.save();
            },
            editHandler: function (e, app) {
                this.changeState('edit');
            }
        });
    };

});