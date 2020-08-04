$(function () {

    $("#signupForm input").jqBootstrapValidation({
        preventSubmit: true,
        submitError: function ($form, event, errors) {
            // additional error messages or events
        },
        submitSuccess: function ($form, event) {
            event.preventDefault(); // prevent default submit behaviour
            // get values from FORM
            var email = $("input#email").val();
            var parameters = "{'email':'" + email + "'}";

            $this = $("#sendSignupButton");
            $this.prop("disabled", true); // Disable submit button until AJAX call is complete to prevent duplicate messages
            $.ajax({
                url: "/api/API/SignupNewsletter",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(parameters),
                cache: false,
                success: function () {
                    // Success message
                    $('#signupSuccess').html("<div class='alert alert-success'>");
                    $('#signupSuccess > .alert-success').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                        .append("</button>");
                    $('#signupSuccess > .alert-success')
                        .append("<strong>Deine Nachricht wurde gesendet. </strong>");
                    $('#signupSuccess > .alert-success')
                        .append('</div>');
                    //clear all fields
                    $('#signupForm').trigger("reset");
                },
                error: function () {
                    // Fail message
                    $('#signupSuccess').html("<div class='alert alert-danger'>");
                    $('#signupSuccess > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                        .append("</button>");
                    $('#signupSuccess > .alert-danger').append($("<strong>").text("Sorry " + firstName + ", der Mail Server antwortet gerade nicht. Versuch es sp&auml;ter noch einmal!"));
                    $('#signupSuccess > .alert-danger').append('</div>');
                    //clear all fields
                    $('#signupForm').trigger("reset");
                },
                complete: function () {
                    setTimeout(function () {
                        $this.prop("disabled", false); // Re-enable submit button when AJAX call is complete
                    }, 1000);
                }
            });
        },
        filter: function () {
            return $(this).is(":visible");
        },
    });

    $("a[data-toggle=\"tab\"]").click(function (e) {
        e.preventDefault();
        $(this).tab("show");
    });
});

/*When clicking on Full hide fail/success boxes */
$('#name').focus(function () {
    $('#signupSuccess').html('');
});
