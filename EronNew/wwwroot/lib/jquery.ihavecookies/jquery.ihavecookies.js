/*!
 * ihavecookies - jQuery plugin for displaying cookie/privacy message
 * v0.3.2
 *
 * Copyright (c) 2018 Ketan Mistry (https://iamketan.com.au)
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/mit-license.php
 *
 */
(function (factory) {
    "use strict";

    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery'], function ($) {
            return factory($, window, document);
        });
    }
    else if (typeof exports === 'object') {
        // CommonJS
        module.exports = function (root, $) {
            if (!root) {
                // CommonJS environments without a window global must pass a
                // root. This will give an error otherwise
                root = window;
            }

            if (!$) {
                $ = typeof window !== 'undefined' ? // jQuery's factory checks for a global window
                    require('jquery') :
                    require('jquery')(root);
            }

            return factory($, root, root.document);
        };
    }
    else {
        // Browser
        factory(jQuery, window, document);
    }
}
(function ($) {

    /*
    |--------------------------------------------------------------------------
    | Cookie Message
    |--------------------------------------------------------------------------
    |
    | Displays the cookie message on first visit or 30 days after their
    | last visit.
    |
    | @param event - 'reinit' to reopen the cookie message
    |
    */
    $.fn.ihavecookies = function (options, event) {

        var $element = $(this);

        // Set defaults
        var settings = $.extend({
            cookieTypes: [
                {
                    type: 'Site Preferences',
                    value: 'preferences',
                    description: 'These are cookies that are related to your site preferences, e.g. remembering your username, site colours, etc.'
                },
                {
                    type: 'Analytics',
                    value: 'analytics',
                    description: 'Cookies related to site visits, browser types, etc.'
                },
                {
                    type: 'Marketing',
                    value: 'marketing',
                    description: 'Cookies related to marketing, e.g. newsletters, social media, etc'
                }
            ],
            elementId: 'gdpr-cookie-message',
            title: 'Cookies & Privacy',
            message: 'Cookies enable you to use shopping carts and to personalize your experience on our sites, tell us which parts of our websites people have visited, help us measure the effectiveness of ads and web searches, and give us insights into user behavior so we can improve our communications and products.',
            link: '/privacy-policy',
            delay: 2000,
            expires: 30,
            moreInfoLabel: 'More information',
            acceptBtnLabel: 'Accept All',
            acceptSelectedBtnLabel: 'Accept Selected',
            acceptRecommendedBtnLabel: 'Accept Recommended',
            advancedBtnLabel: 'Customise',
            cookieTypesTitle: 'Select cookies to accept',
            fixedCookieTypeLabel: 'Necessary',
            fixedCookieTypeDesc: 'These are cookies that are essential for the website to work correctly.',
            onAccept: function () { },
            uncheckBoxes: false
        }, options);

        var myCookie = getCookie('cookieControl');
        var myCookiePrefs = getCookie('cookieControlPrefs');
        if (!myCookie || !myCookiePrefs || event == 'reinit') {
            // Remove all instances of the cookie message so it's not duplicated
            $('#' + settings.elementId).remove();

            // Set the 'necessary' cookie type checkbox which can not be unchecked
            var cookieTypes = '<fieldset class="form-check col-md-6 col-sm-12 text-white"><input type="checkbox" class="form-check-input" name="gdpr[]" value="necessary" checked="checked" disabled="disabled"> <label class="form-check-label text-white" title="' + settings.fixedCookieTypeDesc + '">' + settings.fixedCookieTypeLabel + '</label></fieldset>';

            // Generate list of cookie type checkboxes
            preferences = JSON.parse(myCookiePrefs);
            $.each(settings.cookieTypes, function (index, field) {
                if (field.type !== '' && field.value !== '') {
                    var cookieTypeDescription = '';
                    if (field.description !== false) {
                        cookieTypeDescription = ' title="' + field.description + '"';
                    }
                    cookieTypes += '<fieldset class="form-check col-md-6 col-sm-12 text-white"><input type="checkbox" class="form-check-input" id="gdpr-cookietype-' + field.value + '" name="gdpr[]" value="' + field.value + '" data-auto="on"> <label class="form-check-label" for="gdpr-cookietype-' + field.value + '"' + cookieTypeDescription + '>' + field.type + '</label></fieldset>';
                }
            });

            // Display cookie message on page
            var cookieMessage = `
<div class="modal fade right" id="${settings.elementId}" tabindex="-1" role="dialog" aria-labelledby="cookieLabel" aria-hidden="true" data-backdrop="false">
    <div class="modal-dialog modal-side modal-bottom-right modal-notify modal-info" role="document">
         <!--Content-->
        <div class="modal-content" style="background-color: #181C30;">
            <!--Header-->
            <div id="modalHeader" class="modal-header">
                <p class="heading">${settings.title}</p>

                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true" class="white-text">&times;</span>
                </button>
            </div>
            <!--Header-->
            <!--Body-->
            <div class="modal-body">
                <div class="row">
                    <div class="col-12">
                        <p class="text-white" >${settings.message}
                        <a href="${settings.link}">${settings.moreInfoLabel}</a>
                        </p>
                        <div id="gdpr-cookie-types" style="display:none;">
                            <h6>${settings.cookieTypesTitle}</h6>
                            <div class="row rounded-lg p-3">
                                ${cookieTypes}
                            </div>
                            <a type="button" class="btn btn-sm btn-primary waves-effect col-5 ml-3" id="gdpr-cookie-accept" data-dismiss="modal" >${settings.acceptSelectedBtnLabel}</a>
                            <a type="button" class="btn btn-sm btn-primary waves-effect col-5 ml-3" id="gdpr-cookie-accept-recommend" data-dismiss="modal" >${settings.acceptRecommendedBtnLabel}</a>
                        </div>
                        <div class="row">
                            <a type="button" class="btn btn-sm btn-primary waves-effect col-5 ml-3" id="gdpr-cookie-accept-all" data-dismiss="modal" >${settings.acceptBtnLabel}</a>
                            <a type="button" class="btn btn-sm btn-outline-primary waves-effect col-5 ml-4" id="gdpr-cookie-advanced" >${settings.advancedBtnLabel}</a>
                        </div>

                    </div>
                </div>
            </div>
            <!--Body-->
        </div>
        <!--Content-->
    </div>
</div>`;
            setTimeout(function () {
                $($element).append(cookieMessage);
                $('#' + settings.elementId).modal('show');
                $('#' + settings.elementId).on('show.bs.modal', function () {
                    // If reinit'ing, open the advanced section of message
                    // and re-check all previously selected options.
                    if (event == 'reinit') {
                        $('#gdpr-cookie-advanced').trigger('click');
                        $.each(preferences, function (index, field) {
                            $('input#gdpr-cookietype-' + field).prop('checked', true);
                        });
                    }
                });
            }, settings.delay);

            // When accept button is clicked drop cookie
            $('body').on('click', '#gdpr-cookie-accept', function () {
                // Set cookie
                dropModal(true, settings);

                // If 'data-auto' is set to ON, tick all checkboxes because
                // the user hasn't clicked the customise cookies button
                $('input[name="gdpr[]"][data-auto="on"]').prop('checked', true);

                // Save users cookie preferences (in a cookie!)
                var prefs = [];
                $.each($('input[name="gdpr[]"]').serializeArray(), function (i, field) {
                    prefs.push(field.value);
                });
                setCookie('cookieControlPrefs', encodeURIComponent(JSON.stringify(prefs)), 365);

                // Run callback function
                settings.onAccept.call(this);
            });

            
            $('body').on('click', '#gdpr-cookie-accept-all', function () {
                // Set cookie
                dropModal(true, settings);

                // If 'data-auto' is set to ON, tick all checkboxes because
                // the user hasn't clicked the customise cookies button
                $('input[name="gdpr[]"]').prop('checked', true);

                // Save users cookie preferences (in a cookie!)
                var prefs = [];
                $.each($('input[name="gdpr[]"]').serializeArray(), function (i, field) {
                    prefs.push(field.value);
                });
                setCookie('cookieControlPrefs', encodeURIComponent(JSON.stringify(prefs)), 365);

                // Run callback function
                settings.onAccept.call(this);
            });
            
            $('body').on('click', '#gdpr-cookie-accept-recommend', function () {
                // Set cookie
                dropModal(true, settings);

                // If 'data-auto' is set to ON, tick all checkboxes because
                // the user hasn't clicked the customise cookies button
                $('input[name="gdpr[]"][value="preferences"]').prop('checked', true);
                $('input[name="gdpr[]"][value="analytics"]').prop('checked', true);
                // Save users cookie preferences (in a cookie!)
                var prefs = [];
                $.each($('input[name="gdpr[]"]').serializeArray(), function (i, field) {
                    prefs.push(field.value);
                });
                setCookie('cookieControlPrefs', encodeURIComponent(JSON.stringify(prefs)), 365);

                // Run callback function
                settings.onAccept.call(this);
            });

            // Toggle advanced cookie options
            $('body').on('click', '#gdpr-cookie-advanced', function () {
                // Uncheck all checkboxes except for the disabled 'necessary'
                // one and set 'data-auto' to OFF for all. The user can now
                // select the cookies they want to accept.
                $('input[name="gdpr[]"]:not(:disabled)').attr('data-auto', 'off').prop('checked', false);
                $('#gdpr-cookie-types').slideDown('fast', function () {
                    $('#gdpr-cookie-advanced').prop('disabled', true);
                });
            });

        } else {
            var cookieVal = true;
            if (myCookie == 'false') {
                cookieVal = false;
            }
            dropModal(cookieVal, settings);
        }

        // Uncheck any checkboxes on page load
        if (settings.uncheckBoxes === true) {
            $('input[type="checkbox"].ihavecookies').prop('checked', false);
        }

    };

    // Method to get cookie value
    $.fn.ihavecookies.cookie = function () {
        var preferences = getCookie('cookieControlPrefs');
        return JSON.parse(preferences);
    };

    // Method to check if user cookie preference exists
    $.fn.ihavecookies.preference = function (cookieTypeValue) {
        var control = getCookie('cookieControl');
        var preferences = getCookie('cookieControlPrefs');
        preferences = JSON.parse(preferences);
        if (control === false) {
            return false;
        }
        if (preferences === false || preferences.indexOf(cookieTypeValue) === -1) {
            return false;
        }
        return true;
    };

    /*
    |--------------------------------------------------------------------------
    | Drop Cookie
    |--------------------------------------------------------------------------
    |
    | Function to drop the cookie with a boolean value of true.
    |
    */
    var dropModal = function (value, settings) {
        setCookie('cookieControl', value, settings.expires);
        $('#' + settings.elementId).modal('hide');
        $('#' + settings.elementId).on('hidden.bs.modal', function () {

            $('#' + settings.elementId).modal('dispose');
        });
    };

    /*
    |--------------------------------------------------------------------------
    | Set Cookie
    |--------------------------------------------------------------------------
    |
    | Sets cookie with 'name' and value of 'value' for 'expiry_days'.
    |
    */
    var setCookie = function (name, value, expiry_days) {
        var d = new Date();
        d.setTime(d.getTime() + (expiry_days*24*60*60*1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = name + "=" + value + ";" + expires + ";path=/";
        return getCookie(name);
    };

    /*
    |--------------------------------------------------------------------------
    | Get Cookie
    |--------------------------------------------------------------------------
    |
    | Gets cookie called 'name'.
    |
    */
    var getCookie = function (name) {
        var cookie_name = name + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(cookie_name) === 0) {
                return c.substring(cookie_name.length, c.length);
            }
        }
        return false;
    };

}));
