$(function () {

    let stockGreaterThanZero = true;
    let belowMinimumStock = false;

    $("#stockListTable").DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        filter: false,
        autoWidth: true,
        ajax: {
            url: "/Admin/StockList/GetStockListJsonData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                d.SearchItem = {
                    ItemName: $('#SearchItem_ItemName').val(),
                    Barcode: $('#SearchItem_Barcode').val(),
                    CategoryId: $("#SearchItem_CategoryId").val(),
                    WarehouseId: $('#SearchItem_WarehouseId').val(),
                    StockGreaterThanZero: stockGreaterThanZero,
                    BelowMinimumStock: belowMinimumStock
                };
                return JSON.stringify(d);
            }
        },
        columnDefs: [
            {
                targets: [5, 6],
                className: "text-right"
            },
            {
                targets: 4,
                visible: false
            }
        ],
        rowCallback: function (row, data) {
            var minStockQuantity = parseFloat(data[4]);
            var stockQuantity = parseFloat(data[5]);

            if (stockQuantity < minStockQuantity) {
                $(row).addClass('text-danger');
            }
        }
    });

    $('#SearchItem_ItemName').on('keyup', debounce(function () {
        $("#stockListTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_Barcode').on('keyup', debounce(function () {
        $("#stockListTable").DataTable().ajax.reload(null, false);
    }, 500));

    $('#SearchItem_CategoryId, #SearchItem_WarehouseId').on('change', function () {
        $("#stockListTable").DataTable().ajax.reload(null, false);
    });

    $('#showAll').on('click', function () {
        belowMinimumStock = false;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#belowMinimumStockButton').removeClass("btn-info").addClass("btn-outline-info");
        $("#stockListTable").DataTable().ajax.reload(null, false);
    });

    $('#belowMinimumStockButton').on('click', function () {
        belowMinimumStock = true;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#showAll').removeClass("btn-info").addClass("btn-outline-info");
        $("#stockListTable").DataTable().ajax.reload(null, false);
    });

    $('#stockGreaterThanZeroButton').on('click', function () {
        stockGreaterThanZero = true;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#includeZeroStockButton').removeClass("btn-info").addClass("btn-outline-info");
        $("#stockListTable").DataTable().ajax.reload(null, false);
    });

    $('#includeZeroStockButton').on('click', function () {
        stockGreaterThanZero = false;

        $(this).toggleClass("btn-outline-info btn-info");
        $('#stockGreaterThanZeroButton').removeClass("btn-info").addClass("btn-outline-info");
        $("#stockListTable").DataTable().ajax.reload(null, false);
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