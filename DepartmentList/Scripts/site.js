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
    treeview: {},
    searchName: {},
    searchDate: {},
    editDate: {},
    editName: {}
}
let scope = {
    search: {
        get date() { return controls.searchDate.value() },
        set date(v) { controls.searchDate.value(v) },
        get name() { return controls.searchName.val() },
        set name(v) { controls.searchName.val(v) }
    },
    edit: {
        get date() { return controls.editDate.value() },
        set date(v) { controls.editDate.value(date.toStr(v)) },
        get name() { return controls.editName.val() },
        set name(v) { controls.editName.val(v) }
    }
}


function init() {
    let initTree = () => {
        controls.treeview = $("#treeview").kendoTreeView({
            dataSource: createTreeViewSource(),
            template: kendo.template($("#treeview-template").html()),
            select: select
        }).data("kendoTreeView");
    }
    let initSearch = () => {
        controls.searchDate = $("#searchDatepicker").kendoDatePicker({
            format: date.format
        }).data('kendoDatePicker');
        controls.searchName = $("#searchName");
    }
    let initEdit = () => {
        controls.editDate = $("#editDatepicker").kendoDatePicker({
            format: date.format
        }).data('kendoDatePicker');
        controls.editName = $("#editName");
    }
    initSearch();
    initEdit();
    initTree();
}


$(document).ready(() => init());

function select(e) {
    let node = controls.treeview.dataItem(e.node);
    scope.edit.date = node.creationDate;
    scope.edit.name = node.name;
}

function add() {
    controls.treeview.append(
        { name: scope.edit.name, creationDate: scope.edit.date },
        controls.treeview.select());
}

function edit() {

}

function remove() {
    var selectedNode = controls.treeview.select();
    controls.treeview.remove(selectedNode);
}

function expandAll() {
    controls.treeview.expand(".k-item");
}

function collapseAll() {
    controls.treeview.collapse(".k-item");
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