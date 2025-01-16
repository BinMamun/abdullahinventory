$(function () {
    $("#stockTransferTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/StockTransfer/GetStockTransferJsonData",
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
                targets: 5,
                render: function (data, type, row) {
                    return `
                                <div class="d-flex justify-content-center">
                                <div class="dropdown ms-auto">
                                    <i class="bi bi-three-dots-vertical" data-bs-toggle="dropdown"
                                        aria-expanded="false" role="button" title="Action">
                                    </i>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <button type="submit"
                                            class="dropdown-item show-bs-modal"
                                            data-toggle="modal"
                                            data-target="#transferDetailsModal"
                                            value='${data}' data-id='${data}'>
                                            <i class="default-color bi bi-table">
                                            </i>Transfer Details</button>
                                        </li>
                                        <li>
                                            <button type="submit" class="dropdown-item
                                            show-bs-modal" data-id='${data}' value='${data}'
                                            data-toggle="modal" data-target="#delete-modal"
                                            title="Delete">
                                            <i class="red-color bi bi-trash"></i>
                                            Delete</button>
                                        </li>
                                    </ul>
                                </div>
                                </div>`;
                }
            }
        ]
    });

    $('#stockTransferTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        $("#deleteId").val(id);
        $("#delete-form").attr("action", "/Admin/StockTransfer/Delete");
    });

    $('#stockTransferTable').on('click', '.show-bs-modal', function (event) {

        $('#trsnsferDetailsTable tbody').empty();

        let id = $(this).data("id");

        $.ajax({
            url: '/Admin/StockTransfer/GetStockTransferItemsJsonData',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: function (response) {
                var trsnsferDetailsTableBody = $('#trsnsferDetailsTable tbody');
                $.each(response.data, function (index, item) {
                    var row = `
                                <tr>
                                    <td>${item[0]}</td>
                                    <td class="text-center">${item[1]}</td>
                                </tr>`;
                    trsnsferDetailsTableBody.append(row);
                });
            }
        });
    });

});