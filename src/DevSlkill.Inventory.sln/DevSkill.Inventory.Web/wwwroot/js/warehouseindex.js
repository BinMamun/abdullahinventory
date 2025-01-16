$(function () {
    $("#warehouseTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/Warehouse/GetWarehouseJsonData",
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
                targets: 1,
                render: function (data, type, row) {
                    return `
                            <button type="submit" class="btn btn-sm show-bs-modal"
                            data-toggle="modal" data-target="#updateWarehouseModal"
                            value='${data}' data-name='${row[0]}' data-id='${data}' title="Edit">
                            <i class="default-color bi bi-pencil-square"></i></button>

                            <button type="submit" class="btn show-bs-modal"
                            data-toggle="modal" data-target="#delete-modal"
                            data-id='${data}' value='${data}' title="Delete">
                            <i class="red-color bi bi-trash"></i></button>`;
                }
            }
        ],
    });

    $('#warehouseTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        let name = $(this).data("name");

        let modal = $(this).data('target') === '#delete-modal' ? $('#delete-modal') : $('#updateWarehouseModal');

        if (modal.is('#delete-modal')) {
            $("#deleteId").val(id);
            $("#delete-form").attr("action", "/Admin/Warehouse/Delete");
        }
        else {
            $("#updateId").val(id);
            $("#updateName").val(name);
            $("#update-form").attr("action", "/Admin/Warehouse/Update/{id}");
        }
    });
});
