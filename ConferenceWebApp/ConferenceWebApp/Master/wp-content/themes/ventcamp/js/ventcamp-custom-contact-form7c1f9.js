// Custom Contact Forms Script //
(function ($) {
    $(window).on('load', function() {

        // Contact Forms 7 extra data
        $('.contact-form-7-data').each( function() {

            var $div = $(this),
                token = $div.data('token'),
                settings = window['vsc_custom_contact_form_7_' + token],
                $form = $div.find('form');


            if (settings._wpcf7_vsc_use_mailchimp) {
                $form.append( '<input type="hidden" name="_wpcf7_vsc_use_mailchimp" value="true" />' );
                $form.append( '<input type="hidden" name="_wpcf7_vsc_mailchimp_list_id" value="'+ settings._wpcf7_vsc_mailchimp_list_id + '" />' );
                $form.append( '<input type="hidden" name="_wpcf7_vsc_double_opt" value="'+ settings._wpcf7_vsc_double_opt + '" />' );
            }

            if (settings._wpcf7_vsc_redirect_after_send) {
                $form.append( '<input type="hidden" name="_wpcf7_vsc_redirect_after_send" value="true" />' );
                $form.append( '<input type="hidden" name="_wpcf7_vsc_redirect_url" value="'+ settings._wpcf7_vsc_redirect_url + '" />' );
            }

            if (settings._wpcf7_vsc_hide_after_send) {
                $form.append( '<input type="hidden" name="_wpcf7_vsc_hide_after_send" value="true" />' );
            }
        });
    });

    var oldFormInit = $.fn.wpcf7InitForm;

    $.fn.wpcf7InitForm = function () {
        oldFormInit.apply(this);

        this.ajaxForm({
            beforeSubmit: function(arr, $form, options) {
                $form.wpcf7ClearResponseOutput();
                $form.find('[aria-invalid]').attr('aria-invalid', 'false');
                $form.find('[type="submit"]').addClass('loading');
                return true;
            },
            beforeSerialize: function($form, options) {
                $form.find('[placeholder].placeheld').each(function(i, n) {
                    $(n).val('');
                });
                return true;
            },
            data: { '_wpcf7_is_ajax_call': 1 },
            dataType: 'json',
            success: $.wpcf7AjaxSuccess,
            error: function(xhr, status, error, $form) {
                var e = $('<div class="ajax-error"></div>').text(error.message);
                $form.after(e);
            }
        });
    }

    $.fn.wpcf7AjaxLoader = function () {
        return this.each(function() {
            var $input = $(this),
                classAttr = $input.attr('class'),
                valueAttr = $input.attr('value'),
                $loader = $('<span class="fa fa-refresh wpcf-loader"></span>'),
                $button = $('<button type="submit" class="' + classAttr + '">' + valueAttr + '</button>');

            $input.after($button);
            $button.append($loader);
            $input.remove();
        });
    };

    $.fn.wpcf7ClearResponseOutput = function() {
        return this.each(function() {
            $(this).find('div.wpcf7-response-output').hide().empty().removeClass('wpcf7-mail-sent-ok wpcf7-mail-sent-ng wpcf7-validation-errors wpcf7-spam-blocked').removeAttr('role');
            $(this).find('span.wpcf7-not-valid-tip').remove();
            $(this).find('[type="submit"]').removeClass('loading');
        });
    };
})(jQuery);
