"use strict";

let date = {
    format: "dd.MM.yyyy",
    toStr: function (d) {
        return kendo.toString(d, this.format);
    }
}

function get(data, id) {
    if (!id) {
        return data;
    } else {
        for (var i = 0; i < data.length; i++) {
            if (data[i].id == id) {
                return data[i].items;
            } else if (data[i].items) {
                var result = get(data[i].items, id);
                if (result) return result;
            }
        }
    }
}

let localData = [
    {
        expanded: true,
        id: 1,
        name: "Node 1",
        hasChildren: true,
        creationDate: new Date(),
        items: [
            {
                id: 101,
                name: "Node 1.1",
                hasChildren: true,
                creationDate: new Date(),
                items: [
                    { id: 10101, creationDate: new Date(), name: "Node 1.1.1" }
                ]
            }
        ]
    },
    { id: 2, hasChildren: true, creationDate: new Date(), name: "Node 2" },
    { id: 3, hasChildren: true, creationDate: new Date(), name: "Node 3" }
];
let controls = {
    treeview: {}, searchDate: {}, searchName: {}, searchBtn: {},
    editDate: {}, editName: {}, editBtn: {}, addBtn: {}, removeBtn: {}, saveBtn: {}, cancelBtn : {},
    init: function () {
        let initInput = (input) => $(input).addClass("k-textbox");
        let initBtn = (btn) => $(btn).kendoButton().data("kendoButton");
        let initPicker = (picker) => $(picker).kendoDatePicker({ format: date.format}).data("kendoDatePicker");
        
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
        initTree();
        initSearch();
        initEdit();
    }
}

let actionModes =
    {
        get none() { return 0; },
        get select() { return 1; },
        get add() { return 2; },
        get edit() { return 3; },
        get remove() { return 4; }
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
            return this.current;
        },
        set mode(m) {
            switch (m) {
                case actionModes.none:
                    controls.editBtn.enable(false);
                    controls.addBtn.enable(false);
                    controls.removeBtn.enable(false);
                    controls.editDate.enable(false);
                    controls.editName.prop("disabled", true);
                    controls.saveBtn.enable(false);
                    controls.cancelBtn.enable(false);
                    break;
                case actionModes.select:
                    controls.editBtn.enable();
                    controls.addBtn.enable();
                    controls.removeBtn.enable();
                    controls.editDate.enable(false);
                    controls.editName.prop("disabled", true);
                    controls.saveBtn.enable(false);
                    controls.cancelBtn.enable(false);
                    scope.edit.date = scope.selectedNodeItem.creationDate;
                    scope.edit.name = scope.selectedNodeItem.name;
                    break;
                case actionModes.add:
                    controls.editBtn.enable(false);
                    controls.addBtn.enable(true);
                    controls.removeBtn.enable(false);
                    controls.editDate.enable(true);
                    controls.editName.prop("disabled", false);
                    scope.edit.date = new Date();
                    scope.edit.name = "";
                    controls.saveBtn.enable();
                    controls.cancelBtn.enable();
                    break;
                case actionModes.edit:
                    controls.editBtn.enable(true);
                    controls.addBtn.enable(false);
                    controls.removeBtn.enable(false);
                    controls.editDate.enable(true);
                    controls.editName.prop("disabled", false);
                    controls.saveBtn.enable();
                    controls.cancelBtn.enable();
                    break;
                case actionModes.remove:
                    controls.editBtn.enable(false);
                    controls.addBtn.enable(true);
                    controls.removeBtn.enable(true);
                    controls.editDate.enable(false);
                    controls.editName.prop("disabled", true);
                    controls.saveBtn.enable(false);
                    controls.cancelBtn.enable(false);
                    break;
                default:
                    throw "invalid actionMode";
            }
            this._current = m;
        }
    },
    selectedNode: undefined,
    selectedNodeItem: undefined
}

$(document).ready(() => {
    controls.init();
    scope.action.mode = actionModes.none;
});

function select(e) {
    scope.selectedNode = e.node;
    scope.selectedNodeItem = controls.treeview.dataItem(scope.selectedNode);
    scope.action.mode = actionModes.select;
}

function add() {
    scope.action.mode = actionModes.add;
}

function edit() {
    scope.action.mode = actionModes.edit;
}

function remove() {
    controls.treeview.remove(scope.selectedNode);
    scope.action.mode = actionModes.remove;
    scope.action.mode = actionModes.none;

    $.ajax({
        type: "DELETE",
        url: "api/department/" + 1,
        data: {
            //name: $("#name").val()
        }
    });
}

function search() {

}

function save() {
    
}

function cancel() {
    scope.action.mode = actionModes.select;
}

function createTreeViewSource() {
    return new kendo.data.HierarchicalDataSource({
        transport: {
            read: function (options) {
                var id = options.data.id;
                var data = get(localData, id);
                if (data) {
                    options.success(data);
                } else {
                    $.ajax({
                        type: "GET",
                        url: "api/department/" + id,
                        data: {
                            name: $("#name").val()
                        }
                    }).done(function (data) {
                        data.forEach(x => x.creationDate = new Date(x.creationDate));
                        options.success(data);
                    });
                }
            }
        },
        schema: {
            model: {
                id: "id"
            }
        }
    });
}