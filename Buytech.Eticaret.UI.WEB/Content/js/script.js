$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 0,
    responsiveClass: true,
    responsive: {
        0:{
            items: 1,
        },
        768:{
            items: 2,
        },
        1100:{
            items: 3,  
        },
        1400:{
            items: 4,
            loop: false,
        }
    }
});


jQuery(document).ready(function($) {
    "use strict";

    var navclone = function(){
        $('body').on('click','.js-toggle', function(e) {
            var $this = $(this);
            e.preventDefault();

            if($('body').hasClass('off-view')) {
                $('body').removeClass('off-view');
            }
            else {
                $('body').addClass('off-view');
            }
        });

        $(document).mouseup(function(e) {
            var container = $('.mobile-view');
            if(!container.is(e.target) && container.has(e.target).length === 0) {
                if($('body').hasClass('off-view')){
                    $('body').removeClass('off-view');
                }
            }
        });

        $(window).resize(function() {
            var $this = $(this),
            w = $this.width();
            if(w > 1187) {
                if($('body').hasClass('off-view')){
                    $('body').removeClass('off-view');
                }
            }
        });
    }
    navclone();
});