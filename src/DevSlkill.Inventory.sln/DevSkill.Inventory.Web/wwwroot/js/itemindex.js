$(function () {

    let active = null;
    let belowMinimumStock = false;

    $("#itemTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/Item/GetItemJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.SearchItem = {
                    ItemName: $('#SearchItem_ItemName').val(),
                    Barcode: $('#SearchItem_Barcode').val(),
                    CategoryId: $("#SearchItem_CategoryId").val(),
                    WarehouseId: $('#SearchItem_WarehouseId').val(),
                    PriceFrom: $('#SearchItem_PriceFrom').val() === "" ? null :
                        $('#SearchItem_PriceFrom').val(),
                    PriceTo: $('#SearchItem_PriceTo').val() === "" ? null :
                        $('#SearchItem_PriceTo').val(),
                    IsActive: active,
                    BelowMinimumStock: belowMinimumStock
                };
                return JSON.stringify(d);
            }
        },
        order: [[ 2, 'asc' ]],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                render: function (data, type, row) {

                    return `<input type="checkbox" data-id='${data}' class="mx-2 form-check-input row-checkbox">`;
                }
            },
            {
                orderable: false,
                targets: 1,
                render: function (data) {
                    if (data) {
                        return `
                                <img src="/${data}" alt="${data}" class="image-thumbnail">`;
                    }
                    else {
                        return '<img src="#" class="hidden">'
                    }
                }
            },
            {
                targets: 7,
                visible: false
            },
            {
                orderable: false,
                targets: 10,
                render: function (data, type, row) {
                    return `
                    <div class="d-flex justify-content-center">
                    <div class="dropdown ms-auto">
                                <i class="bi bi-three-dots-vertical" data-bs-toggle="dropdown"
                                aria-expanded="false" role="button" title="Action"></i>
                            <ul class="dropdown-menu">
                                <li>
                                    <button type="submit" class="dropdown-item"
                                    onclick="window.location.href='/admin/item/update/${data}'" value='${data}' title="Edit">
                                    <i class="default-color bi bi-pencil-square"></i>
                                    Edit</button>
                                </li>
                                    <li>
                                        <button type="submit" class="dropdown-item
                                        show-bs-modal" data-id='${data}' value='${data}'
                                        data-toggle="modal" data-target="#delete-modal"
                                        title="Delete">
                                        <i class="red-color bi bi-trash"></i>
                                        Delete
                                        </button>
                                    </li>
                            </ul>
                            </div>
                    </div>`;
                }
            }
        ],
        rowCallback: function (row, data) {
            var minStockQuantity = parseFloat(data[7]);
            var stockQuantity = parseFloat(data[8]);

            if (stockQuantity < minStockQuantity) {
                $(row).addClass('text-danger');
            }
        }
    });

    //Delete Modal
    $('#itemTable').on('click', '.show-bs-modal', function (event) {
        let id = $(this).data("id");
        $("#deleteId").val(id);
        $("#delete-form").attr("action", "/Admin/Item/Delete");
    });

    $('#select-all').on('click', function () {
        let rows = $('#itemTable tbody input.row-checkbox');
        rows.prop('checked', this.checked);
    });
    $('#itemTable tbody').on('change', 'input.row-checkbox', function () {
        if (!this.checked) {
            $('#select-all').prop('checked', false);
        }
        else if ($('#itemTable tbody input.row-checkbox:checked').length === $('#itemTable tbody input.row-checkbox').length) {
            $('#select-all').prop('checked', true);
        }
    });

    $('#delete-button').on('click', function () {
        var selectedItemIds = getSelectedRowIds();
        $('#deleteId').val(selectedItemIds = selectedItemIds.length > 0 ? JSON.stringify(selectedItemIds) : null);
        $("#delete-form").attr("action", "/Admin/Item/DeleteAll");
    });

    function getSelectedRowIds() {
        let selectedIds = [];
        $('input.row-checkbox:checked').each(function () {
            let rowId = $(this).data('id');
            selectedIds.push(rowId);
        });
        return selectedIds;
    };



    $('#SearchItem_ItemName').on('keyup', debounce(function () {
        $("#itemTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_Barcode').on('keyup', debounce(function () {
        $("#itemTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_PriceFrom').on('keyup', debounce(function () {
        console.log("Price from:" + $('#SearchItem_PriceFrom').val());
        $("#itemTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_PriceTo').on('keyup', debounce(function () {
        console.log("Price to:" + $('#SearchItem_PriceTo').val());
        $("#itemTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_CategoryId, #SearchItem_WarehouseId').on('change', function () {
        $("#itemTable").DataTable().ajax.reload(null, false);
    });

    $('#showAll').on('click', function () {
        belowMinimumStock = false;
        active = null;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#active, #inactive').removeClass("btn-info").addClass("btn-outline-info");
        $('#belowMinimumStockButton').removeClass("btn-info").addClass("btn-outline-info");
        $("#itemTable").DataTable().ajax.reload(null, false);
    });


    $('#belowMinimumStockButton').on('click', function () {
        belowMinimumStock = !belowMinimumStock;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#showAll').removeClass("btn-info").addClass("btn-outline-info");
        $("#itemTable").DataTable().ajax.reload(null, false);
    });

    $('#active').on('click', function () {
        active = true;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#inactive, #showAll').removeClass("btn-info").addClass("btn-outline-info");
        $("#itemTable").DataTable().ajax.reload(null, false);
    });

    $('#inactive').on('click', function () {
        active = false;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#active, #showAll').removeClass("btn-info").addClass("btn-outline-info");
        $("#itemTable").DataTable().ajax.reload(null, false);
    });

    function debounce(func, delay) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                func.apply(context, args);
            }, delay);
        };
    };
});
