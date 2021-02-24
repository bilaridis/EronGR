
const domainUrl = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');

moment().locale('el');

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

function gup(name, url) {
    if (!url) url = location.href;
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    return results == null ? "" : results[1];
}
function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1));
    var array = []
    var sURLVariables = sPageURL.split('&');
    for (let i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            array.push(decodeURIComponent(sParameterName[1]));
        }
    }
    return array;
}
function contains(a, obj) {
    for (let i = 0; i < a.length; i++) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        console.log(e)
    }
};

var cookieOptions = {
    elementId: 'gdpr-cookie-message',
    title: '&#x1F36A; Accept Cookies & Privacy Policy?',
    message: 'This site uses cookies – small text files that are placed on your machine to help the site provide a better user experience.',
    delay: 800,
    expires: 1000,
    link: '#privacy',
    onAccept: function () {
        var myPreferences = $.fn.ihavecookies.cookie();
        console.log('Yay! The following preferences were saved...');
        console.log(myPreferences);
    },
    uncheckBoxes: true,
    acceptBtnLabel: 'Accept & Close',
    moreInfoLabel: '<a href="http://www.aboutcookies.org">About Cookies <i class="fas fa-book ml-1"></i></a>',
    cookieTypesTitle: 'Select which cookies you want to accept',
    fixedCookieTypeLabel: 'Essential',
    fixedCookieTypeDesc: 'These are essential for the website to work correctly.'
}

$(function () {
    //onResize();
    if (getCookie(".AspNetCore.Culture") != undefined) {
        var value = unescape(getCookie(".AspNetCore.Culture"));
        var culture = value.split('|')[0].replace('c=', '').toLowerCase();
        $("#langButton").html('<span class="flag-icon flag-icon-' + culture + ' fa-2x"></span>');
    }
    else {
        $("#langButton").html('<span class="flag-icon flag-icon-el-gr fa-2x"></span>');
    }

    toastr.options = {
        "closeButton": true, // true/false
        "debug": false, // true/false
        "newestOnTop": false, // true/false
        "progressBar": false, // true/false
        "positionClass": "md-toast-top-center", // md-toast-top-right / md-toast-top-left / md-toast-bottom-right / md- toast - bottom - left
        "preventDuplicates": false, //true / false
        "onclick": null,
        "showDuration": "300", // in milliseconds
        "hideDuration": "1000", // in milliseconds
        "timeOut": "5000", // in milliseconds
        "extendedTimeOut": "1000", // in milliseconds
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $('[data-toggle="tooltip"]').tooltip({
        template: '<div class="tooltip md-tooltip d-print-none"><div class="tooltip-inner md-inner"></div></div>'
    });
   
    $('body').ihavecookies(cookieOptions);

    if (!window.location.href.includes('Search')) {
        $('#ihavecookiesBtn').on('click', function () {
            $('body').ihavecookies(cookieOptions, 'reinit');
        });
        $(".mdb-select").materialSelect();
    }

    new WOW().init();
});




