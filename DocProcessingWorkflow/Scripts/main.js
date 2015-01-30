$(document).ready(function($) {
    $('[data-toggle="tooltip"]').tooltip({ 'placement': 'top' });

    // Password Strength
    "use strict";
    var options = {};
    options.ui = {
        container: "#pwd-container",
        showVerdictsInsideProgressBar: true,
        viewports: {
            progress: ".pwstrength_viewport_progress"
        }
    };
    options.common = {
        debug: true,
        onLoad: function () {
            $('#messages').text('Start typing password');
        }
    };
    $('.passwordstrength').pwstrength(options); 

    $('[placeholder]').focus(function () {
        var input = $(this);
        if (input.val() == input.attr('placeholder')) {
            input.val('');
            input.removeClass('placeholder');
        }
    }).blur(function () {
        var input = $(this);
        if (input.val() == '' || input.val() == input.attr('placeholder')) {
            input.addClass('placeholder');
            input.val(input.attr('placeholder'));
        }
    }).blur();

    $('[placeholder]').parents('form').submit(function () {
        $(this).find('[placeholder]').each(function () {
            var input = $(this);
            if (input.val() == input.attr('placeholder')) {
                input.val('');
            }
        })
    });

});