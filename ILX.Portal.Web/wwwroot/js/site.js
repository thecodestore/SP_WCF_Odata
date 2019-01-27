var verifyCallback = function (response) {
    alert(response);
};
var widgetId1;
var widgetId2;
var onloadCallback = function () {
    // Renders the HTML element with id 'example1' as a reCAPTCHA widget.
    // The id of the reCAPTCHA widget is assigned to 'widgetId1'.
    widgetId1 = grecaptcha.render('example1', {
        'sitekey': 'your_site_key',
        'theme': 'light'
    });
    widgetId2 = grecaptcha.render(document.getElementById('example2'), {
        'sitekey': 'your_site_key'
    });
    grecaptcha.render('example3', {
        'sitekey': 'your_site_key',
        'callback': verifyCallback,
        'theme': 'dark'
    });
};

      $(document).ready(function () {
         $("form").submit(function (e) {                
            var recaptcha = $("#g-recaptcha-response").val();
            if (recaptcha === "") {
               alert("Please check the recaptcha");
            } else {
               $.ajax({
                  async:false,
                  url: "/api/captcha/verify",
                  method: "POST",
                  data: { "captchaResponse" : recaptcha },

                  success: function (response) {
                     if (response.success === true) {
                        alert("captcha verification was successful\n" + JSON.stringify(response));
                     } else {
                        alert("captcha verification NOT successful \n" + JSON.stringify(response));
                        e.preventDefault();
                     }
                  },

                  error: function (jqXHR, textStatus, errorThrown) {
                     alert("captcha verification NOT successful");
                     e.preventDefault();
                  }
               });
            }                
         });
     });
