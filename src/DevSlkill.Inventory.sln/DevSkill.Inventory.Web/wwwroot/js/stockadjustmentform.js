$(function () {
    $('#create-form').validate({
        rules: {
            stockAdjustmentReasonId: {
                required: true,
            },
            warehouseId: {
                required: true
            },
            itemSearch: {
                required: true
            }
        },
        messages: {
            stockAdjustmentReasonId: {
                required: "Reason is required",
            },
            warehouseId: {
                required: "Warehouse is required",
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

    $('#warehouseId').change(function () {
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
            `<tr data-stock="${stockQuantity}">
        <td>
            <input type="text" class="form-control itemSearch"
                value="${itemName}" placeholder="Search item" name="itemSearch" />
            <ul class="list-group itemList" style="display:none;"></ul>
            <input type="hidden"
                name="StockAdjustmentItems[${rowIndex}].ItemId" value="${itemId}" />
        </td>

        <td style="font-size: 14px; padding-top:20px;" class="stockQuantity text-right">
            ${stockQuantity}
        </td>

        <td>
            <input type="text" class="form-control text-right newStockInHandQuantity"
                value="${stockQuantity}" id="newStock${rowIndex}" readonly />
        </td>

        <td class="row d-flex align-items-center">
            <div class="col-7">
                <input type="text" class="form-control text-right adjustmentQuantity" min="1"
                    name="StockAdjustmentItems[${rowIndex}].AdjustedQuantity"
                    oninput="updateStock(${rowIndex})" required />
            </div>
            <div class="col-4">
                <div class="row">
                    <span class="pr-2 text-danger text-bold">&#8722;</span>
                    <div class="custom-control custom-switch custom-switch-off-info custom-switch-on-info">
                        <input type="checkbox" checked class="custom-control-input adjustmentSwitch" id="customSwitch${rowIndex}"
                          name="StockAdjustmentItems[${rowIndex}].IsIncrease"
                          onchange="toggleSwitchValue(${rowIndex}); updateStock(${rowIndex})" value="true">
                        <label class="custom-control-label" for="customSwitch${rowIndex}"></label>
                    </div>
                    <span class="text-success text-bold">&plus;</span>
                </div>
            </div>
        </td>

        <td class="text-center">
            <button type="button" class="btn removeItem">
                <i class="bi bi-trash text-danger"></i>
            </button>
        </td>
    </tr>`;

        $('#stockAdjustmentBody').append(newRow);
        rowIndex++;

        calculateTotalAdjustedQuantity();

        $('.adjustmentQuantity').off('input').on('input', function () {
            calculateTotalAdjustedQuantity();
        });

        $('.removeItem').off('click').on('click', function () {
            $(this).closest('tr').remove();
            calculateTotalAdjustedQuantity();
        });
    }
    function calculateTotalAdjustedQuantity() {
        let totalQuantity = 0;
        $('#stockAdjustmentBody tr').each(function () {
            const row = $(this);
            const isIncrease = row.find('.adjustmentSwitch').prop('checked');
            const quantity = parseFloat(row.find('.adjustmentQuantity').val()) || 0;

            if (isIncrease) {
                totalQuantity += quantity;
            } else {
                totalQuantity -= quantity;
            }
        });

        $('.totalAdjustedQuantity strong').text(totalQuantity.toFixed(2));
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
        var warehouseId = $('#warehouseId').val();
        const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

        var itemList = $(input).siblings('.itemList');

        itemList.empty();

        if (itemId.length > 1) {
            $.ajax({
                url: "/Admin/StockAdjustment/GetSearchedItemJsonData",
                type: 'POST',
                contentType: "application/json",
                dataType: "json",
                headers: {
                    'RequestVerificationToken': antiForgeryToken
                },
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
        var newStockField = $(this).closest('tr').find('.newStockInHandQuantity');

        $(this).closest('tr').data('stock', stockQuantity);

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

    $(document).on('change', '.adjustmentSwitch', function () {
        calculateTotalAdjustedQuantity();
    });

    $(document).on('input', '.adjustmentQuantity', function () {
        calculateTotalAdjustedQuantity();
    });


    $("#save-reason-btn").on("click", function (event) {

        const reasonName = $("#reasonName").val();
        const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

        if (reasonName && reasonName.trim() !== "") {
            $.ajax({
                url: "/Admin/StockAdjustment/CreateStockAdjustmentReason",
                type: 'POST',
                contentType: "application/json",
                dataType: "json",
                headers: {
                    'RequestVerificationToken': antiForgeryToken
                },
                data: JSON.stringify({ reasonName: reasonName }),
                success: function (result) {
                    if (result.success) {
                        const reasonSelect = $("#stockAdjustmentReasonId");
                        const newOption = $("<option></option>")
                            .val(result.reasonId)
                            .text(result.reasonName)
                            .prop("selected", true);
                        reasonSelect.append(newOption);
                        reasonSelect.val(result.reasonId);

                        $("#reasonName").val("");
                        $("#createReasonModal").fadeOut(300, function () {
                            $(this).hide();
                            $('.modal-backdrop').remove();
                        });
                    }
                }
            });
        }
        else {
            $('#reason-create-form').validate({
                rules: {
                    reasonName: {
                        required: true,
                    }
                },
                messages: {
                    reasonName: {
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
            }).form();;
        }

    });
});

function updateStock(index) {
    const row = $(`#stockAdjustmentBody tr:nth-child(${index + 1})`);
    const originalStock = parseFloat(row.data('stock'));
    const adjustedQuantity = parseFloat(row.find('.adjustmentQuantity').val());
    const isIncrease = row.find('.adjustmentSwitch').prop('checked');

    if (!isNaN(adjustedQuantity)) {
        let newStock;
        if (isIncrease) {
            newStock = originalStock + adjustedQuantity;
        } else {
            newStock = originalStock - adjustedQuantity;
        }
        row.find('.newStockInHandQuantity').val(newStock);
    }
}
function toggleSwitchValue(index) {
    const checkbox = document.getElementById(`customSwitch${index}`);
    if (checkbox.checked) {
        checkbox.value = "true";
    } else {
        checkbox.value = "false";
    }
}