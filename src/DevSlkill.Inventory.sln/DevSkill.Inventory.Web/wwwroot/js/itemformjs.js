$(function () {
    $('#createForm').validate({
        rules: {
            itemName: {
                required: true,
            },
            measurementUnitId: {
                required: true
            },
            buyingPrice: {
                required: true
            },
            sellingPrice: {
                required: true
            }
        },
        messages: {
            itemName: {
                required: "Item Name is required",
            },
            measurementUnitId: {
                required: "Measurement Unit is required",
            },
            buyingPrice: {
                required: "Buying Price is required",
            },
            sellingPrice: {
                required: "Selling Price is required",
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

    $('#updateForm').validate({
        rules: {
            itemName: {
                required: true,
            },
            measurementUnitId: {
                required: true
            },
            buyingPrice: {
                required: true
            },
            sellingPrice: {
                required: true
            }
        },
        messages: {
            itemName: {
                required: "Item Name is required",
            },
            measurementUnitId: {
                required: "Measurement Unit is required",
            },
            buyingPrice: {
                required: "Buying Price is required",
            },
            sellingPrice: {
                required: "Selling Price is required",
            },

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

    $("#generate-barcode").on('click', function () {
        var randomNumber = Date.now();
        $("#barcode-field").val(randomNumber);
    });

    const preview = document.querySelector('.preview');
    const imageInput = document.querySelector('#image');
    const imagePreview = document.querySelector('#imagePreview');
    const previousImagePath = document.querySelector('#previousImagePath');

    $('#image').on('change', function (e) {
        if (e.target.files && e.target.files[0]) {
            const reader = new FileReader();

            reader.onload = function (e) {
                imagePreview.src = e.target.result;
                preview.classList.remove('hidden');
            };

            reader.readAsDataURL(e.target.files[0]);
        }
        else {
            imagePreview.src = "#";
            preview.classList.add('hidden');
        }
    });
    $('#close-preview').on('click', function () {
        imageInput.value = "";
        imagePreview.src = "#";
        preview.classList.add('hidden');
        previousImagePath.value = "";
    });

    $('#buyingPrice').on('input', function () {
        let value = $(this).val();
        $('#cost-per-unit').val(value);
    });
});

