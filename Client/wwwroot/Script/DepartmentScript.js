
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
                    return '<div class="text-center ">' +
                        '<button type="button" data-state="edit" class="btn btn-warning cursor-pointer p-2 btn-lg" href="#" data-toggle="tooltip" data-placement="top" title="Edit Data" onclick="return getDepartmentById(\'' + row.dept_Id + '\',this)"><i class="fas fa-edit"></i></button> ' +
                        '<button type="button" data-state="delete" class="btn btn-danger p-2 btn-lg w-auto" href="#" data-toggle="tooltip" data-placement="top" title="Delete Data" onclick="return deleteDepartment(\'' + row.dept_Id + '\')"><i class="fas fa-trash"></i></button>' +
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
            Swal.fire({
                icon: "success",
                title: result.message,
                showConfirmButton: false,
                timer: 1500
            });
            $('#departmentTable').DataTable().ajax.reload();
            $('#departmentModal').modal('hide');
            $('#departmentInputInitial').val('');
            $('#departmentInputName').val('');
        }
    })
}
function getDepartmentById(dept_Id, button) {
    //debugger
    var state = $(button).data('state');
    if (state === "edit") {
        $('#departmentModal').modal('show');
        var buttonSave = $('#modalBtnSave');
        buttonSave.hide();
    }
    $.ajax({
        url: 'https://localhost:7145/api/Departments/' + dept_Id,
        type: 'GET',
        success: function (result) {
            $('#departmentInputInitial').val(result.data.dept_Initial);
            $('#departmentInputName').val(result.data.dept_Name);
            var buttonEdit = $('#modalBtnEdit');
            buttonEdit.attr('onclick', 'updateDepartment(\'' + dept_Id + '\')');
        },
        error: function (xhr, status, error) {
            Swal.fire(
                'Error!',
                'An error occurred while deleting the department.',
                'error'
            );
        }
    });
}
function updateDepartment(dept_Id) {
    $.ajax({
        url: 'https://localhost:7145/api/Departments/',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({
            dept_Id: dept_Id,
            dept_Initial: $('#departmentInputInitial').val(),
            dept_Name: $('#departmentInputName').val()
        }),
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: "Department "+dept_Id+" is updated!",
                showConfirmButton: false,
                timer: 1500
            });
            //alert(result.message);
            $('#departmentTable').DataTable().ajax.reload();
            $('#departmentModal').modal('hide');
            $('#departmentInputInitial').val('');
            $('#departmentInputName').val('');
        }
    })

}

function deleteDepartment(dept_Id) {
    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure you want to delete department with ID " + dept_Id + "",
        icon: "warning",
        reverseButtons: true,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: 'https://localhost:7145/api/Departments/' + dept_Id,
                type: 'DELETE',
                contentType: 'application/json',
                success: function (result) {
                    Swal.fire({
                        icon: "success",
                        title: "Department " + dept_Id + " is updated!",
                        showConfirmButton: false,
                        timer: 1500
                    });
                    //alert(result.message);
                    Swal.fire({
                        title: "Deleted!",
                        text: result.message,
                        icon: "success"
                    });
                    $('#departmentTable').DataTable().ajax.reload();
                    $('#departmentInputInitial').val('');
                    $('#departmentInputName').val('');
                }

            });
            
        }
    });

}
$(function () {
    $('[data-toggle-tooltip="tooltip"]').tooltip()
})
$(document).ajaxComplete(function () {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: "hover",
    })
})