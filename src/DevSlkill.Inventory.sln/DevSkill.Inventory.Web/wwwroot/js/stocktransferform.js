$(function () {

    $('#create-form').validate({
        rules: {
            fromWarehouseId: {
                required: true,
            },
            toWarehouseId: {
                required: true
            },
            itemSearch: {
                required: true
            }
        },
        messages: {
            fromWarehouseId: {
                required: "From Warehouse is required",
            },
            toWarehouseId: {
                required: "To Warehouse is required",
            },
            itemSearch: {
                required: "Item name is required",
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

    $('#fromWarehouseId').change(function () {
        var selectedWarehouse = $(this).val();

        if (selectedWarehouse) {
            $('#itemSearchTable').removeClass('d-none');

            if ($('#stockTransferBody').find('tr').length === 0) {
                addItemRow();
            }
        } else if (!selectedWarehouse) {
            $('#itemSearchTable').addClass('d-none');
        }
    });

    let rowIndex = 0;
    function addItemRow(itemId = '', itemName = '', stockQuantity = '') {
        const newRow =
            `<tr>
                        <td>
                            <input type="text" class="form-control itemSearch"
                                value="${itemName}" placeholder="Search item" name="itemSearch"/>
                            <ul class="list-group itemList" style="display:none;"></ul>
                            <input type="hidden" 
                                name="stockTransferItems[${rowIndex}].ItemId"
                                value="${itemId}" />
                        </td>

                        <td style="font-size: 14px; padding-top:20px;"
                            class="stockQuantity d-flex justify-content-end">${stockQuantity}
                        </td>

                        <td>
                            <input type="text" class="form-control transferQuantity" min="1"
                                name="stockTransferItems[${rowIndex}].TransferQuantity" required />
                        </td>

                        <td class="text-center">
                            <button type="button" class="btn removeItem">
                                <i class="bi bi-trash text-danger"></i>
                            </button>
                        </td>
                    </tr>`;
        $('#stockTransferBody').append(newRow);
        rowIndex++;

        calculateTotalTransferredQuantity();
        $('.transferQuantity').off('input').on('input', function () {
            calculateTotalTransferredQuantity();
        });

        $('.removeItem').off('click').on('click', function () {
            $(this).closest('tr').remove();
            calculateTotalTransferredQuantity();
        });
    }

    function calculateTotalTransferredQuantity() {
        let totalQuantity = 0;
        $('.transferQuantity').each(function () {
            const quantity = parseFloat($(this).val()) || 0;
            totalQuantity += quantity;
        });

        $('.totalTransferredQuantity strong').text(totalQuantity.toFixed(2));
    }

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
    }

    function searchItems(input) {

        var itemId = $(input).val();
        var warehouseId = $('#fromWarehouseId').val();

        var itemList = $(input).siblings('.itemList');

        itemList.empty();

        if (itemId.length > 1) {
            $.ajax({
                url: "/Admin/StockTransfer/GetSearchedItemJsonData",
                type: 'POST',
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({ searchItem: itemId, warehouseId: warehouseId }),
                success: function (data) {
                    itemList.empty();
                    $.each(data, function (index, item) {
                        item.stockQuantity = item.stockQuantity === null ?
                            0 : item.stockQuantity
                        itemList.append('<li style="cursor:pointer;" class="list-group-item" data-item-id="' + item.itemId + '" data-stock="' + item.stockQuantity + '">' + item.itemName + '</li>');
                    });
                    itemList.show();
                }
            });
        } else {
            itemList.hide();
        }
    }

    $(document).on('keyup', '.itemSearch', debounce(function () {
        searchItems(this);
    }, 500));

    $(document).on('click', '.itemList li', function () {
        var selectedItem = $(this).text();
        var stockQuantity = $(this).data('stock');
        var itemId = $(this).data('item-id');
        var inputField = $(this).closest('td').find('.itemSearch');
        var stockDisplay = $(this).closest('tr').find('.stockQuantity');
        var itemIdInput = $(this).closest('tr').find('input[name*="ItemId"]');

        inputField.val(selectedItem);
        stockDisplay.text(stockQuantity);
        itemIdInput.val(itemId);
        $(this).parent().empty();
    });

    $(document).on('click', '#addRow', function () {
        addItemRow();
    });

    $(document).on('click', '.removeItem', function () {
        $(this).closest('tr').remove();
    });
});