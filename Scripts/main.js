/**
 * Created by kola on 8/19/2016.
 */
$(function () {
    scaleVideoContainer();

    initBannerVideoSize('.video-container .poster img');
    initBannerVideoSize('.video-container .filter');
    initBannerVideoSize('.video-container video');

    $(window).on('resize', function () {
        scaleVideoContainer();
        scaleBannerVideoSize('.video-container .poster img');
        scaleBannerVideoSize('.video-container .filter');
        scaleBannerVideoSize('.video-container video');
    });

    $(window).on('scroll', function() {/*
        $('.search-section').css('display', 'block');*/
        $('.navigation').css({background: '#fff', boxShadow: '0 1px 0 0 rgba(0, 0, 0, .05), 0 2px 4px 0 rgba(0, 0, 0, .06)'});
        $('#main-nav').find('a').css({color: '#333'});
        if ($(window).scrollTop() === 0) {
            $('.navigation').css({background: 'transparent', boxShadow: 'none'});
            $('#main-nav').find('a').css({color: '#fff'});
        }
    });

    $('a[href*="#"]:not([href="#"])').click(function () {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 1000);
                return false;
            }
        }
    });

    $('.js-overlay-btn').click(openOverlay);
    $('.close-btn').click(closeOverlay);


    $('.search-overlay-btn').click(openSearchOverlay);
    $('.search-close-btn').click(closeSearchOverlay);

    $('.overlay-logo').click(function () {
        closeOverlay();
        $(window).scrollTop(0);

    });


    $('.search-overlay-logo').click(function () {
        closeSearchOverlay();
        $(window).scrollTop(0);
    });

});

function openOverlay() {
    $("#overlay").css('height', '100%');
}

/* Close when someone clicks on the "x" symbol inside the overlay */
function closeOverlay() {
    $("#overlay").css('height', '0');
}


function openSearchOverlay() {
    $("#search-overlay").css('height', '100%');
}

/* Close when someone clicks on the "x" symbol inside the overlay */
function closeSearchOverlay() {
    $("#search-overlay").css('height', '0');
}

function scaleVideoContainer() {

    var height = $(window).height() + 5;
    var unitHeight = parseInt(height) + 'px';
    $('.homepage-hero-module').css('height', unitHeight);

}

function initBannerVideoSize(element) {

    $(element).each(function () {
        $(this).data('height', $(this).height());
        $(this).data('width', $(this).width());
    });

    scaleBannerVideoSize(element);

}

function scaleBannerVideoSize(element) {

    var windowWidth = $(window).width(),
        windowHeight = $(window).height() + 5,
        videoWidth,
        videoHeight;


    $(element).each(function () {
        var videoAspectRatio = $(this).data('height') / $(this).data('width');

        $(this).width(windowWidth);

        if (windowWidth < 1000) {
            videoHeight = windowHeight;
            videoWidth = videoHeight / videoAspectRatio;
            $(this).css({'margin-top': 0, 'margin-left': -(videoWidth - windowWidth) / 2 + 'px'});

            $(this).width(videoWidth).height(videoHeight);
        }

        $('.homepage-hero-module .video-container video').addClass('fadeIn animated');

    });
}