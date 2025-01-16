$(function () {
    $("#serviceTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/Service/GetServiceJsonData",
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
                targets: 3,
                render: function (data, type, row) {
                    return `
                                <button type="submit" class="btn btn-sm show-bs-modal"
                                onclick="window.location.href='/admin/service/update/${data}'"
                                value='${data}' data-id='${data}' title="Edit">
                                <i class="default-color bi bi-pencil-square"></i></button>

                                <button type="submit" class="btn btn-sm show-bs-modal"
                                data-toggle="modal" data-target="#delete-modal"
                                data-id='${data}' value='${data}' title="Delete">
                                <i class="red-color bi bi-trash"></i></button>`;
                }
            }
        ],
    });

    $('#serviceTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        let modal = $('#delete-modal');
        $("#deleteId").val(id);
        $("#delete-form").attr("action", "/Admin/Service/Delete");
    });
});