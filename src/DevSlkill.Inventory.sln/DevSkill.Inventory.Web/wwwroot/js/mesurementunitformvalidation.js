$(function () {
    $('#create-form').validate({
        rules: {
            name: {
                required: true,
            },
            symbol: {
                required: true,
            }
        },
        messages: {
            name: {
                required: "Name is required",
            },
            symbol: {
                required: "Symbol is required",
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

    $('#update-form').validate({
        rules: {
            name: {
                required: true,
            },
            symbol: {
                required: true,
            }
        },
        messages: {
            name: {
                required: "Name is required",
            },
            symbol: {
                required: "Symbol is required",
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

    $('#createMeasurementUnitModal').on('hide.bs.modal', function () {
        window.location.reload();
    });

    $('#updateMeasurementUnit').on('hide.bs.modal', function () {
        window.location.reload();
    });
});