$(function () {
    $("#fechaFinal").datetimepicker({
        format: "dd/MM/yyyy",
        icons: {
            clear: 'fa fa-trash fa-fw',
            close: 'fa fa-times fa-fw',
            date: 'fa fa-calendar fa-fw',
            down: 'fa fa-chevron-down fa-fw',
            next: 'fa fa-chevron-right fa-fw',
            previous: 'fa fa-chevron-left fa-fw',
            time: 'fa fa-clock-o fa-fw',
            up: 'fa fa-chevron-up fa-fw',
            today: 'fa fa-calendar-times-o fa-fw'
        },
        showClear: true,
        showClose: true,
        showTodayButton: true
    });
});

$(function () {
    $('#fechaInicio').datetimepicker({
        format: "dd/MM/yyyy",
        icons: {
            clear: 'fa fa-trash fa-fw',
            close: 'fa fa-times fa-fw',
            date: 'fa fa-calendar fa-fw',
            down: 'fa fa-chevron-down fa-fw',
            next: 'fa fa-chevron-right fa-fw',
            previous: 'fa fa-chevron-left fa-fw',
            time: 'fa fa-clock-o fa-fw',
            up: 'fa fa-chevron-up fa-fw',
            today: 'fa fa-calendar-times-o fa-fw'
        },
        showClear: true,
        showClose: true,
        showTodayButton: true
    });
});