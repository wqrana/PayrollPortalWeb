//Common JS file for whole application
var toastMsgOptions = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-top-center",
    "onclick": null,
    "showDuration": "1000",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "preventDuplicates": true
}

$(document).ready(function () {
    // Bootstrap date-picker for datetime field
    $('input[id*=Date]').each(function () {
        $(this).datetimepicker({ format: 'MM/DD/YYYY' });

    });
    $('#tblIndexList').dataTable({
        "searching": true
    });

    $('#alertDismiss').on('click', function () {
        debugger;
        $(this).parent().hide();
    });

    //Overcome the issue of multiple modal, if one is closed then overcome the scroll issue,
    // uncomment below
    $(document)
        .on('hidden.bs.modal', '.modal', function () {
            debugger;
            /**
             * Check if there are still modals open
             * Body still needs class modal-open
             */
            if ($('body').find('.modal.show').length) {
                $('body').addClass('modal-open');
            }
        });
});
//Open popup For Master Data


function showAlertAutoHide(id, type, message) {
    //$(id).children("span").text(message);
    if (type == "Error") {
     //   $(id).removeClass('alert-success').addClass('alert-danger').show();
        displayToastErrorMessage(message);
    }
    else {
      //  $(id).removeClass('alert-danger').addClass('alert-success').show();
        displayToastSuccessMessage(message);
    }
    //setTimeout(function () { $(id).hide('slow', 'swing'); }, 4000);

}
function showAlertDismissable(id, type, message) {
    $(id).children("span").text(message);
    if (type == "Error") {
        $(id).removeClass('alert-success').addClass('alert-danger').show();
    }
    else {
        $(id).removeClass('alert-danger').addClass('alert-success').show();
    }
   

}
function displayToastErrorMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.error(message, "Error!");

}

function displayToastWarningMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.warning(message, "Warning!");

}
function displayToastSuccessMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.success(message, "Success!");
}