/* template
 */


(function ($) {
    $(document).ready(function () {
        //pd-search
        $('.btn-search').click(function () {
            $('#searchBox').fadeIn(200);
        });
        $('.closebtn').click(function () {
            $('#searchBox').fadeOut();
        });

        //browing-panel-mobile

        $('.btn-browing, .footer-btn-browing').click(function () {
            $('.browsing-panel').show('slide', {
                direction: "right"
            }, 300);
            $('.browsing-panel-overlay').show();
            $('.btn-browing').hide();
        });
        $('.closebtn').click(function () {
            $('.browsing-panel').hide('slide', {
                direction: "right"
            }, 300);
            $('.browsing-panel-overlay').hide();
            $('.btn-browing').show();
        });
        $('.browsing-panel-overlay').on('click', function () {
            $(this).hide();
            $('.browsing-panel').hide('slide', {
                direction: "right"
            }, 300);
            $('.btn-browing').show();
        });

        //imagefill

        $('.fill').imagefill({target:'.background-image'});

        //twzipcode
        $('.twzipcode').twzipcode();

        //validate
        $("#register-form,.contact-form,.order-form,.buyForm,.login-form,.forget-form").validate();

        //cartbox
        $(document).click(function () {
            $("body").click(function () {
                return;
            });
            $(".i-cart").removeClass("active");
            $(".cartbox").removeClass("open").fadeOut();
            $("#cartbox").removeClass("active");

        })
        $("#cartbox").hover(function () {
            $(".i-cart").addClass("active");
            $(this).addClass("active");
            $(".cartbox").addClass("open").fadeIn();
        });


        //Scroll totop
        //-----------------------------------------------
        $(window).scroll(function () {
            if ($(this).scrollTop() != 0) {
                $(".scrollToTop , .browsing-history").fadeIn();
            } else {
                $(".scrollToTop , .browsing-history").fadeOut();
            }
        });

        $(".scrollToTop").click(function () {
            $("body,html").animate({
                scrollTop: 0
            }, 800);
        });

        //Modal
        //-----------------------------------------------
        if ($(".modal").length > 0) {
            $(".modal").each(function () {
                $(".modal").prependTo("body");
            });
        }

    }); // End document ready

})(this.jQuery);
