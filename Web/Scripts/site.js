// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function DeleteConfirm() {
    if (confirm($('#ConfEliminar').text()))
        return true;
    else
        return false;
}

function CerrarConfirm() {
    if (confirm($('#ConfCerrar').text()))
        return true;
    else
        return false;
}
/*
$(document).ready(function () {
    var message = $('#Message').text();
    var tituloTr = $('#TituloTR').text();
    if (message !== '') {
        toastr.success(message, tituloTr, {
            "closeButton": true,
            "hideDuration": "1000",
            "timeOut": "4000",
            "preventDuplicates": false,
            "progressBar": true,
            "positionClass": "toast-top-center"
        });
    }
});

$(document).ready(function () {
    function disableBack() { window.history.forward() }
    window.onload = disableBack();
    window.onpageshow = function (evt) { if (evt.persisted) disableBack() }
});

 */