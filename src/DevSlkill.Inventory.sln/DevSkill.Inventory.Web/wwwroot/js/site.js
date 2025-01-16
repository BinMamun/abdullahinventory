$(document).ready(function () {
    let toastEl = document.getElementById('responseToast');
    if (toastEl) {
        var toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
});


$("#select-date").datepicker({
    format: 'dd-mm-yyyy',
    autoclose: true,
    todayHighlight: true
}).datepicker("setDate", new Date());
$('#calendar-icon').on('click', function () {
    $('#select-date').datepicker('show');
});