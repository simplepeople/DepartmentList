"use strict";

let date = {
    format: "dd.MM.yyyy",
    toStr: function (d) {
        return kendo.toString(d, this.format);
    }
}

let settings = {
    url: "api/department/"
}

function createTreeViewSource() {
    return new kendo.data.HierarchicalDataSource({
        transport: {
            read: get
        },
        schema: {
            model: {
                id: "id"
            }
        }
    });
}
let controls = {
    treeview: {}, searchDate: {}, searchName: {}, searchBtn: {},
    editDate: {}, editName: {}, editBtn: {}, addBtn: {}, removeBtn: {}, saveBtn: {}, cancelBtn: {},
    showDate: {}, showName: {},
    init: function () {
        let initInput = (input) => $(input).addClass("k-textbox");
        let initBtn = (btn) => $(btn).kendoButton().data("kendoButton");
        let initPicker = (picker) => $(picker).kendoDatePicker({ format: date.format}).data("kendoDatePicker");
        let initCheckBox = (checkBox) => $(checkBox);

        let initTree = () => {
            this.treeview = $("#treeview").kendoTreeView({
                dataSource: createTreeViewSource(),
                template: kendo.template($("#treeview-template").html()),
                select: select
            }).data("kendoTreeView");
        }
        let initSearch = () => {
            this.searchDate = initPicker("#searchDatepicker");
            this.searchName = initInput("#searchName");
            this.searchBtn = initBtn("#searchBtn");
        }
        let initEdit = () => {
            this.editDate = initPicker("#editDatepicker");
            this.editName = initInput("#editName");
            this.editBtn = initBtn("#editBtn");
            this.addBtn = initBtn("#addBtn");
            this.removeBtn = initBtn("#removeBtn");
            this.saveBtn = initBtn("#saveBtn");
            this.cancelBtn = initBtn("#cancelBtn");
        }
        let initShow = () => {
            this.showDate = initCheckBox("#showDate");
            this.showName = initCheckBox("#showName");
        }
        initTree();
        initSearch();
        initEdit();
        initShow();
    }
}

//бойлерплейт
let actionModes =
    {
        get none() { return 0; },
        noneAction: function() {
            controls.editBtn.enable(false);
            controls.addBtn.enable(true);
            controls.removeBtn.enable(false);
            controls.editDate.enable(false);
            controls.editName.prop("disabled", true);
            controls.saveBtn.enable(false);
            controls.cancelBtn.enable(false);
            controls.searchDate.enable(true);
            controls.searchName.prop("disabled", false);
            controls.searchBtn.enable(true);
            if (scope.selectedNodeItem != null) {
                scope.edit.date = scope.selectedNodeItem.creationDate;
                scope.edit.name = scope.selectedNodeItem.name;
            } else {
                scope.edit.date = "";
                scope.edit.name = "";
            }
        },
        get select() { return 1; },
        selectAction: function() {
            controls.editBtn.enable();
            controls.addBtn.enable();
            controls.removeBtn.enable();
            controls.editDate.enable(false);
            controls.editName.prop("disabled", true);
            controls.saveBtn.enable(false);
            controls.cancelBtn.enable(false);
            controls.searchDate.enable(true);
            controls.searchName.prop("disabled", false);
            controls.searchBtn.enable(true);
            if (scope.selectedNodeItem != null) {
                scope.edit.date = scope.selectedNodeItem.creationDate;
                scope.edit.name = scope.selectedNodeItem.name;
            } else {
                scope.edit.date = "";
                scope.edit.name = "";
            }
        },
        get add() { return 2; },
        addAction: function() {
            controls.editBtn.enable(false);
            controls.addBtn.enable(true);
            controls.removeBtn.enable(false);
            controls.editDate.enable(true);
            controls.editName.prop("disabled", false);
            scope.edit.date = new Date();
            scope.edit.name = "";
            controls.saveBtn.enable();
            controls.cancelBtn.enable();
            controls.searchDate.enable(false);
            controls.searchName.prop("disabled", true);
            controls.searchBtn.enable(false);
        },
        get edit() { return 3; },
        editAction: function() {
            controls.editBtn.enable(true);
            controls.addBtn.enable(false);
            controls.removeBtn.enable(false);
            controls.editDate.enable(true);
            controls.editName.prop("disabled", false);
            controls.saveBtn.enable();
            controls.cancelBtn.enable();
            controls.searchDate.enable(false);
            controls.searchName.prop("disabled", true);
            controls.searchBtn.enable(false);
        },
        get remove() { return 4; },
        removeAction: function() {
            controls.editBtn.enable(false);
            controls.addBtn.enable(true);
            controls.removeBtn.enable(true);
            controls.editDate.enable(false);
            controls.editName.prop("disabled", true);
            controls.saveBtn.enable(false);
            controls.cancelBtn.enable(false);
            controls.searchDate.enable(false);
            controls.searchName.prop("disabled", true);
            controls.searchBtn.enable(false);
        }
    }

let scope = {
    search: {
        get date() { return controls.searchDate.value() },
        get name() { return controls.searchName.val() }
    },
    edit: {
        get date() { return controls.editDate.value() },
        set date(v) { controls.editDate.value(date.toStr(v)) },
        get name() { return controls.editName.val() },
        set name(v) { controls.editName.val(v) }
    },
    action: {
        _current: actionModes.none,
        get mode() {
            return scope.action._current;
        },
        set mode(m) {
            switch (m) {
                case actionModes.none:
                    actionModes.noneAction();
                    break;
                case actionModes.select:
                    actionModes.selectAction();
                    break;
                case actionModes.add:
                    actionModes.addAction();
                    break;
                case actionModes.edit:
                    actionModes.editAction();
                    break;
                case actionModes.remove:
                    actionModes.removeAction();
                    break;
                default:
                    throw "invalid actionMode";
            }
            this._current = m;
        }
    },
    selectedNode: undefined,
    selectedNodeItem: undefined,
    get showDate() { return controls.showDate.prop("checked"); },
    get showName() { return controls.showName.prop("checked"); },
}

$(document).ready(() => {
    controls.init();
    scope.action.mode = actionModes.none;
});

function showDate() {
    showHide(".depDate", scope.showDate);
}
function showName() {
    showHide(".depName", scope.showName);
}

function showHide(elemName, v) {
    var elem = $(elemName);
    if (v)
        elem.show();
    else elem.hide();
}

function select(e) {
    scope.selectedNode = e.node;
    scope.selectedNodeItem = controls.treeview.dataItem(scope.selectedNode);
    scope.action.mode = actionModes.select;
}

function searchClick() {
    search();
}

function search() {
    $.ajax({
        type: "GET",
        url: settings.url + "/" + (scope.selectedNode == undefined ? "" : scope.selectedNode.id) +
            "?" + "name=" + scope.search.name
            + "&creationDate=" + (scope.search.date!=null?scope.search.date.toISOString():null)
    }).done(function(data) {
        data.forEach(x => x.creationDate = new Date(x.creationDate));
    });
}



function get (options) {
    var id = options.data.id;
    $.ajax({
        type: "GET",
        url: settings.url + (id == undefined ? "" : id)
    }).done(function (data) {
        data.forEach(x => x.creationDate = new Date(x.creationDate));
        options.success(data);
    });
}

function addClick() {
    scope.action.mode = actionModes.add;
}

function add() {
    $.ajax({
        type: "POST",
        url: settings.url,
        data: {
            name: scope.edit.name,
            creationDate: scope.edit.date.toISOString(),
            parentId: scope.selectedNodeItem == undefined ? null : scope.selectedNodeItem.id
        }
    }).done(data => {
        controls.treeview.append(
            {
                id: data.id,
                hasChildren: data.hasChildren,
                creationDate: new Date(data.creationDate),
                name: data.name,
                parentId: data.parentId,
                template: kendo.template($("#treeview-template").html())
            }, controls.treeview.select());
        controls.treeview.select($());
        scope.action.mode = actionModes.none;
    });
}

function editClick() {
    scope.action.mode = actionModes.edit;
}

function edit() {
    $.ajax({
        type: "PUT",
        url: settings.url,
        data: {
            name: scope.edit.name,
            creationDate: scope.edit.date.toISOString(),
            id: scope.selectedNodeItem.id
        }
    }).done(() => {
        scope.selectedNodeItem.set('name', scope.edit.name);
        scope.selectedNodeItem.set('creationDate', scope.edit.date);
        scope.action.mode = actionModes.select;
    });
}

function removeClick() {
    remove();
}

function remove() {
    $.ajax({
        type: "DELETE",
        url: settings.url + scope.selectedNodeItem.id
    }).done(() => {
        controls.treeview.remove(scope.selectedNode);
        scope.action.mode = actionModes.remove;
        scope.selectedNode = scope.selectedNodeItem = undefined;
        scope.action.mode = actionModes.none;
    });
}

function saveClick() {
    switch (scope.action.mode) {
    case actionModes.add:
        add();
        break;
    case actionModes.edit:
        edit();
        break;
    }

}

function cancelClick() {
    cancel();
}

function cancel() {
    scope.action.mode = actionModes.select;
}