$(function () {
    $("#categoryTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/Category/GetCategoryJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.SearchItem = {
                };
                return JSON.stringify(d);
            }
        },
        columnDefs: [
            {
                orderable: false,
                targets: 2,
                render: function (data, type, row) {
                    return `
                            <button type="submit" class="btn btn-sm show-bs-modal"
                            data-toggle="modal" data-target="#updateCategoryModal"
                            value='${data}' data-name='${row[0]}' 
                            data-description='${row[1]}'
                            data-id='${data}' title="Edit">
                            <i class="default-color bi bi-pencil-square"></i></button>

                            <button type="submit" class="btn show-bs-modal"
                            data-toggle="modal" data-target="#delete-modal"
                            data-id='${data}' value='${data}' title="Delete">
                            <i class="red-color bi bi-trash"></i></button>`;
                }
            }
        ],
    });

    $('#categoryTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        let name = $(this).data("name");
        let description = $(this).data("description");

        let modal = $(this).data('target') === '#delete-modal' ? $('#delete-modal') : $('#updateCategoryModal');

        if (modal.is('#delete-modal')) {
            $("#deleteId").val(id);
            $("#delete-form").attr("action", "/Admin/Category/Delete");
        }
        else {
            $("#updateId").val(id);
            $("#updateName").val(name);
            $("#updateDescription").val(description);
            $("#update-form").attr("action", "/Admin/Category/Update/{id}");
        }
    });
});
