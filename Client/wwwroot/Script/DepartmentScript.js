
$(document).ready(function () {
    $('#departmentTable').DataTable({
        "paging": true,
        "responsive": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "ajax": {
            url: 'https://localhost:7145/api/Departments',
            type: 'GET',
            datatype: 'json',
            dataSrc: "data",
            //success: function (data) {
            //    console.log(data);
            //}
        },
        "columns": [
            //{"data": 1++},
            {
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "dept_Id" },
            { "data": "dept_Initial" },
            { "data": "dept_Name" },
            {
                "render": function (data, type, row) {
                   
                    
                    return '<div class="text-center ">'+
                     '<button type="button" class="btn btn-warning p-2 btn-lg w-[50%]" href="#" data-toggle="tooltip" data-placement="top" title="Edit Data" onclick="return getDepartmentById(' + row.dept_Id + ',this)"><i class="fas fa-edit"></i></button> ' +
                     '<button type="button" class="btn btn-danger p-2 btn-lg w-auto" href="#" data-toggle="tooltip" data-placement="top" title="Delete Data" onclick="return deleteDepartment(' + row.dept_Id + ')"><i class="fas fa-trash"></i></button>' +
                    '</div>';
                   
                }
}
        ],
        //"initComplete": function (settings, json) {
        //    // Initialize tooltips after the DataTable is fully rendered
        //    $('[data-toggle="tooltip"]').tooltip();
        //}
    });

});

function handleButtonclick(button) {
    var state = $(button).data('state');
    //console.log(button);
    if (state === "create") {
        var buttonEdit = $('#modalBtnEdit');
        buttonEdit.hide();
        //console.log("create button is clicked")
    }
}
function save() {
    //debugger
    var department = new Object();
    department.initial = $('#departmentInputInitial').val();
    department.name = $('#departmentInputName').val();
    $.ajax({
        url: 'https://localhost:7145/api/Departments',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(department),
        success: function (result) {
            alert(result.message);
            $('#departmentTable').DataTable().ajax.reload();
            $('#departmentModal').modal('hide');
            $('#departmentInputInitial').val('');
            $('#departmentInputName').val('');
        }
    })
}
function getDepartmentById(dept_Id, button) {
    console.log(dept_Id); // Corrected to use dept_Id instead of Id
    var state = $(button).data('state');
    if (state === "edit") {
        $('#departmentModal').modal('show');
        var buttonSave = $('#modalBtnSave');
        buttonSave.hide();
    }
}
$(function () {
    $('[data-toggle-tooltip="tooltip"]').tooltip()
})
$(document).ajaxComplete(function () {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: "hover",
    })
})