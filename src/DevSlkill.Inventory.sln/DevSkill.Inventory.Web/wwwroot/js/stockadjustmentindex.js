$(function () {
    $("#stockAdjustmentTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: true,
        autoWidth: true,
        ajax: {
            url: "/Admin/StockAdjustment/GetStockAdjustmentJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.SearchItem = {
                };
                return JSON.stringify(d);
            }
        },
        order: [[0, 'desc']],
        columnDefs: [
            {
                orderable: false,
                targets: 7,
                render: function (data, type, row) {
                    return `
                                    <button type="submit" class="btn btn-sm show-bs-modal"
                                    data-toggle="modal" data-target="#delete-modal"
                                    data-id='${data}' value='${data}' title="Delete">
                                    <i class="red-color bi bi-trash"></i></button>`;
                }
            }
        ],
    });

    $('#stockAdjustmentTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        let modal = $('#delete-modal');
        $("#deleteId").val(id);
        $("#delete-form").attr("action", "/Admin/StockAdjustment/Delete");
    });
});