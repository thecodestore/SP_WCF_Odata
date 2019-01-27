function ajaxLogin() {
    this.getValidationSummaryErrors = function ($form) {
        var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
        return errorSummary;
    };
    

    this.displayErrors = function ($form, errors) {
        var errorSummary = this.getValidationSummaryErrors($form)
            .removeClass('validation-summary-valid')
            .addClass('validation-summary-errors');

        var items = $.map(errors, function (error) {
            console.log(error);
            return '<li>' + error + '</li>';
        }).join('');

        errorSummary.find('ul').empty().append(items);
    };
}


this.formSubmitHandler = function (e) {
    //

    $(e).parent("div").addClass('spn');
    $(e).hide();
    var token = $('[name=__RequestVerificationToken]').val();
   var ajaxLogin1 = new ajaxLogin();
   var $form = $("#" + $(e).parents("form").attr("id"));// 
  // alert($(e).parents("form").attr("id"));
   //var $form = $("#loginForm");
   var data = $form.serializeFormJSON();
   if (!$form.valid || $form.valid()) {
       console.log("Register gform is valid");
       var jx = $.ajax({
           dataType: "json",
           contentType: "application/json",
           type: 'POST',
           headers: { "__RequestVerificationToken": token },

           url: $form.attr("action"),
           data: JSON.stringify(data)//$form.serializeArray())
       }).done((function (json) {

           json = json || {};
           $(e).parent("div").removeClass('spn');
           $(e).show();
           if (json.success) {
               window.location = json.redirect || location.href;
           }
           else if (json.errors) {
               //ajaxLogin1.displayErrors($form, json.errors);
           }
       })).error(function () {
           $(e).parent("div").removeClass('spn');
           $(e).show();
           this.displayErrors($form, ['An unknown error happened.']);
       });
   }
   else
   {
       console.log("register form is incomplete");
       $(e).parent("div").removeClass('spn');
       $(e).show();
       this.displayErrors($form, ['An unknown error happened.']);
   }
    //e.preventDefault();
    return false;
};

$("#showRegister").click(function () {
    $("#loginPanel").hide("slide", function () {
        $("#registerPanel").show("slide", function () {
            $("#registerName").focus();
        });
    });
});

$(".showLogin").click(function () {
    $("#registerPanel").hide("slide", function () {
        $("#loginPanel").show("slide", function () {
            $("#loginName").focus();
        });
    });
});

