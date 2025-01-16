$(function () {
    $("#rolesTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/Role/GetRolesJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.SearchItem = {
                };
                return JSON.stringify(d);
            }
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 1,
                className: "text-center",
                render: function (data, type, row) {
                    return `
                            <button type="submit" class="btn btn-sm show-bs-modal"
                            data-toggle="modal" data-target="#updateRoleModal"
                            value='${data}' data-name='${row[0]}'
                            data-id='${data}' title="Edit">
                            <i class="default-color bi bi-pencil-square"></i></button>

                            <button type="submit" class="btn btn-sm show-bs-modal"
                            data-toggle="modal" data-target="#delete-modal"
                            data-id='${data}' value='${data}' title="Delete">
                            <i class="red-color bi bi-trash"></i></button>`;
                }
            }
        ],
    });

    $('#rolesTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        let name = $(this).data("name");

        let modal = $(this).data('target') === '#delete-modal' ? $('#delete-modal') : $('#updateRoleModal');

        if (modal.is('#delete-modal')) {
            $("#deleteId").val(id);
            $("#delete-form").attr("action", "/Admin/Role/Delete");
        }
        else {
            $("#updateId").val(id);
            $("#updateName").val(name);
            $("#update-form").attr("action", "/Admin/Role/Update/{id}");
        }
    });


    $('#create-form').validate({
        rules: {
            name: {
                required: true,
            }
        },
        messages: {
            name: {
                required: "Name is required",
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });

    $('#update-form').validate({
        rules: {
            name: {
                required: true,
            }
        },
        messages: {
            name: {
                required: "Name is required",
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });

    $('#createRoleModal').on('hide.bs.modal', function () {
        window.location.reload();
    });

    $('#updateRoleModal').on('hide.bs.modal', function () {
        window.location.reload();
    });
});