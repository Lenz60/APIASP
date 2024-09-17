
$(document).ready(function () {
    //$('#departmentGroupSelect').select2({
    //    ajax: {
    //        url: 'https://localhost:7145/api/Departments',
    //        type: 'GET',
    //        dataType: 'json',
    //        processResults: function (result) {
    //            return {
    //                results: result.data.map(function (department) {
    //                    return {
    //                        id: department.dept_Id,
    //                        text: department.dept_Name
    //                    };
    //                })
    //            };
    //        }
    //        //success: function (result) {
    //        //    //console.log('Data received:', data); // Log the data to see its structure

    //        //    //// Check if data is an array
    //        //    //if (Array.isArray(data)) {
    //        //    //    populateSelect(data);
    //        //    //} else if (data.data && Array.isArray(data.data)) {
    //        //    //    // If data is wrapped in an object with a "data" property
    //        //    //    populateSelect(data.data);
    //        //    //} else {
    //        //    //    console.error('Unexpected data format:', data);
    //        //    //}
    //        //    let select = $('#departmentGroupSelect');
    //        //    result.data.forEach((item) => {
    //        //        let option = document.createElement("option");
    //        //        option.value = item.dept_Id; // Set the value attribute
    //        //        option.innerHTML = item.dept_Name; // Set the display text
    //        //        select.append(option);
    //        //    });
    //        //},
    //        //error: function (jqXHR, textStatus, errorThrown) {
    //        //    console.error('Error fetching departments:', textStatus, errorThrown);
    //        //}
    //    }
    //})
    $.ajax({
        url: 'https://localhost:7145/api/Departments',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            let select = $('#departmentGroupSelect');
            result.data.forEach((item) => {
                let option = document.createElement("option");
                option.value = item.dept_Id; // Set the value attribute
                option.innerHTML = item.dept_Name; // Set the display text
                select.append(option);
            });

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching departments:', textStatus, errorThrown);
        }
    });

    $('#accountTable').DataTable({
        "paging": true,
        "responsive": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "ajax": {
            url: 'https://localhost:7145/api/Account',
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
            { "data": "nik" },
            { "data": "email" },
            { "data": "username" },
            { "data": "fullName" },
            { "data": "departmentName" },
            {
                "render": function (data, type, row) {
                    return '<div class="text-center ">' +
                        '<button type="button" data-state="delete" class="btn btn-danger p-2 btn-lg w-auto" href="#" data-toggle="tooltip" data-placement="top" title="Delete Data" onclick="return deleteAccount(\'' + row.nik + '\')"><i class="fas fa-trash"></i></button>' +
                        '</div>';

                }
            }
        ],
    });

});
//$(document).ready(function () {

//})

function handleButtonclick(button) {
    //debugger
    $('#modalBtnSave').removeAttr('disabled');
    $('#modalBtnSave').show();
    $('#accountEmail').removeClass('is-invalid');
    $('#accountForm').removeClass('was-validated');
    $('#accountForm').addClass('needs-validation');
    $('#accountModal').modal('show');
    clearData();
}

function registerAccount() {
    $('#modalBtnSave').attr('disabled', true);
    //console.log("here")
    var firstName = $('#accountFirstName').val();
    var lastName = $('#accountLastName').val();
    var email = $('#accountEmail').val();
    var password = $('#accountPassword').val();
    var repeatPassword = $('#accountRepeatPassword').val();
    var departmentId = $('#departmentGroupSelect').val();
    //var form = document.getElementById('accountForm');
    //console.log(departmentId);
    if (password !== repeatPassword && repeatPassword != null) {
        Swal.fire({
            icon: 'error',
            title: 'Passwords do not match',
            text: 'Please make sure your passwords match'
        });
        return;
    } else {
        manageAccountAdd(firstName, lastName, email, password, departmentId);
       
    }
}

function manageAccountAdd(firstName, lastName, email, password, departmentId) {
    //debugger

    $.ajax({
        url: 'https://localhost:7145/api/Account',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password,
            dept_Id: departmentId
        }),
        success: function (result) {
            //console.log(result)
            Swal.fire({
                icon: "success",
                title: result.message,
                showConfirmButton: false,
                timer: 1500
            });
            clearData();
            //$('#accountTable').DataTable().ajax.reload();
            
        },
        error: function (xhr, status, error) {
            debugger
            Swal.fire({
                icon: 'error',
                title: xhr.responseJSON.message,
                text: error
            });
            $('#modalBtnSave').removeAttr('disabled');
            if (xhr.responseJSON.message.includes("Email")) {
                $('#accountEmail').addClass('is-invalid');
                $('#invalidFeedbackEmail').text('Email already exists!')
            }
            
        }
    })
}

function deleteAccount(nik) {
    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure you want to delete account with ID " + nik + "",
        icon: "warning",
        reverseButtons: true,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: 'https://localhost:7145/api/Account/' + nik,
                type: 'DELETE',
                contentType: 'application/json',
                success: function (result) {
                    Swal.fire({
                        icon: "success",
                        title: "Department " + nik + " is deleted!",
                        showConfirmButton: false,
                        timer: 1500
                    });
                    //alert(result.message);
                    Swal.fire({
                        title: "Deleted!",
                        text: result.message,
                        icon: "success"
                    });
                    clearData();
                }

            });

        }
    });
}

function clearData() {
    $('#accountFirstName').val('');
    $('#accountLastName').val('');
    $('#accountEmail').val('');
    $('#accountPassword').val('');
    $('#departmentGroupSelect option:first').prop('selected', true);
    $('#accountTable').DataTable().ajax.reload();
    $('#accountModal').modal('hide');
}