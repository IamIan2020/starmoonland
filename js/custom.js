/* custom
 */

(function ($) {
    $(document).ready(function () {
        //lazyload
        
        $('img').lazyload();
        
        var nextPages = [
          'index',
        ];
        
        //-------------------------------------//
            // init Infinte Scroll

        var $container = $('.scroll-box').infiniteScroll({
          path: function() {
            return nextPages + '.php';
          },
          append: '.class-item',
          status: '.scroller-status',
        });
        
        
        //Smooth Scroll
		//-----------------------------------------------
		// Select all links with hashes
        /*$('a[href*="#"]')
          // Remove links that don't actually link to anything
          .not('[href="#"]')
          .not('[href="#0"]')
          .click(function(event) {
            // On-page links
            if (
              location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') 
              && 
              location.hostname == this.hostname
            ) {
              // Figure out element to scroll to
              var target = $(this.hash);
              target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
              // Does a scroll target exist?
              if (target.length) {
                // Only prevent default if animation is actually gonna happen
                event.preventDefault();
                $('html, body').animate({
                  scrollTop: target.offset().top
                }, 1000, function() {
                  // Callback after animation
                  // Must change focus!
                  var $target = $(target);
                  $target.focus();
                  if ($target.is(":focus")) { // Checking if the target was focused
                    return false;
                  } else {
                    $target.attr('tabindex','-1'); // Adding tabindex for elements not focusable
                    $target.focus(); // Set focus again
                  };
                });
              }
            }
          });*/


        //banner

        $(".icarousel").owlCarousel({
            items: 1,
            lazyLoad: true,
            animateOut: 'fadeOut',
            loop: true,
            dots: false,
            nav: false,
            autoplay: false,
            autoplayHoverPause: false,
            /*responsive: {
                767: {
                    items: 1,
                    dots: false,
                }
            }*/
        });
        
        //index-news
        $(".news-carousel").owlCarousel({
            items: 1,
            loop: false,
            dots: true,
            autoplay: true,
            autoplayHoverPause: true,
            responsive: {
                767: {
                    items: 1,
                    dots: true,
                }
            }
        });
        
        
        //dining-slick
        $(".dining-slick").slick({
            dots: false,
            arrows: false,
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            fade: true,
        });
         $('.dining-nav').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.dining-slick',
            dots: true,
            arrow: false,
             fade: true,
            centerMode: false,
            focusOnSelect: true,
            customPaging: function(slick, index) {
                var inrHTML;
                inrHTML = slick.$slides.get(index).innerHTML;
                inrHTML = inrHTML.match(/class="ttl">.*?<\//)[0];
                inrHTML = inrHTML.replace('class="ttl">', '');
                inrHTML = inrHTML.replace('<\/', '');
                return '<button class="tab">' + inrHTML + '</button>';
            }, 
        });
        

        //index album-slick
        
        $('.slider-left').each(function() {
		  $('.slider-left li:last-child').prependTo('.slider-left');
	    })

        $('.slider-right').each(function() {
            $('.slider-right li:first-child').appendTo('.slider-right');
        })


        $('.slider-left').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.slider-center, .slider-right',
            dots: false,
            arrows: false,
            infinite: false,
            focusOnSelect: true,
            autoplay: false,
            autoplaySpeed: 3000
        });
	
        $('.slider-center').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.slider-left, .slider-right',
            prevArrow: '<a href="javascript:;" class="btnPrev"></a>',
            nextArrow: '<a href="javascript:;" class="btnNext"></a>',
            infinite: false,
            focusOnSelect: true,
            autoplay: false,
            autoplaySpeed: 3000,
            dots: false,
            responsive: [
                {
                    breakpoint: 1180,
                    settings: {
                        slidesToShow: 3,
                        arrows: false,
                        dots: true
                    }
                },
                {
                    breakpoint: 767,
                    settings: {
                        slidesToShow: 2,
                        arrows: false,
                        dots: true
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        arrows: false,
                        centerMode: true,
                        dots: true
                    }
                }
            ]
        });
        $('.slider-right').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.slider-center, .slider-left',
            dots: false,
            arrows: false,
            infinite: false,
            focusOnSelect: true,
            autoplay: false,
            autoplaySpeed: 3000
        });
        
        

        //class-slider
        
        $('.slider-for').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            fade: true,
            adaptiveHeight: false,
            asNavFor: '.slider-nav',
            autoplay: true,
            autoplaySpeed: 3000,
            responsive: [
                {
                    breakpoint: 769,
                    settings: {
                        slidesToShow: 1,
                        dots: true,
                    }
                }
            ]
        });
        $('.slider-nav').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.slider-for',
            dots: true,
            arrow: false,
            centerMode: false,
            focusOnSelect: true,
        });
        
        //album show
        
        $('.album-slider-for').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            fade: true,
            adaptiveHeight: false,
            asNavFor: '.slider-nav',
            autoplay: true,
            autoplaySpeed: 3000,
            responsive: [
                {
                    breakpoint: 769,
                    settings: {
                        slidesToShow: 1,
                        dots: true,
                    }
                }
            ]
        });
        $('.album-slider-nav').slick({
            slidesToShow: 5,
            slidesToScroll: 1,
            dots: false,
            arrow: true,
            centerMode: false,
            focusOnSelect: true,
        });
        
        $(".slider-album").slick({
            slide: 'li',
            slidesToShow: 5,
            slidesToScroll: 5,
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 1,
                    }
                }
            ]
        });
        $(".slider-album-show").slick({
            slide: 'li',
            slidesToShow: 2,
            slidesToScroll: 2,
        });
        


    }); // End document ready

})(this.jQuery);
