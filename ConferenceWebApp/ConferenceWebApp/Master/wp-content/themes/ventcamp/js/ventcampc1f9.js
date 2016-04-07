/*
*
* Custom js snippets for Ventcamp v2.0
* by Vivaco
*
*/

var Ventcamp;

;(function($){

$(document).on('ready', function () {
    if ( window.ventcampThemeOptions ) {
        Ventcamp.init(ventcampThemeOptions);

    }else {
        Ventcamp.init();

    }
});

// Main theme functions start
Ventcamp = {
    defaults: {
        log: false,
        styleSwitcher: false,
        animations: true,
        headerRelativePosition: false,
        onePageNav: true,
        onePageNavHashChange: true,
        alwaysMobileMenuMode: false,
        mobileMenuMaxWidth: 768,
        smoothScroll: false,
        smoothScrollSpeed: 800,
        pseudoSelect: true,
        ajaxedForm: true,
        ajaxedFormSuccessMsg: 'Success',
        ajaxedFormErrorMsg: 'An error occured. Please try again later.',
        toastrPositionClass: 'toast-top-full-width'
    },

    mobileDevice: false,

    log: function (msg) {
        if ( this.options.log ) console.log('%Ventcamp Log: ' + msg, 'color: #fe4918');
    },

    // buildStyleSwitcher: function () {
    //     var template = '<div class="style-switcher"><a href="#" class="style-toggle"><i class="fa fa-cog"></i></a><h6>Color palette</h6><ul class="template-set-color"><li><a data-color="Blue" class="color blue" href="#" title="style"></a></li><li><a data-color="Pink" class="color pink" href="#" title="main-pink"></a></li><li><a data-color="Green" class="color green" href="#" title="main-green"></a></li><li><a data-color="Berry" class="color berry" href="#" title="main-berry"></a></li><li><a data-color="Orange" class="color orange" href="#" title="main-orange"></a></li></ul><div class="template-animations-switch"><h6>Animations</h6><input id="animations_switch" type="checkbox" checked="checked" /><label for="animations_switch"><i></i></label></div></div>'

    //     $('body').append(template);
    // },

    // check if site is laoded from mobile device
    checkMobile: function () {
        mobileDeviceOld = this.mobileDevice

        if ( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) || $(window).width() < this.options.mobileMenuMaxWidth ) {
            this.mobileDevice = true;

            this.log('Mobile device');
        }else {
            this.log('Desktop')
        }

        if ( this.mobileDevice ) {
            return true;
        }else {
            return false;
        }
    },

    // init counters on page
    initCounters: function () {
        var _this = this;

        $('.counter-block.no-animation .counter-content > .count').each(function () {
            $(this).text($(this).data('to'));
        });

        if ( this.mobileDevice || !this.options.animations ) {
            $('.counter-block .counter-content > .count').each(function () {
                $(this).text($(this).data('to'));
            });

        }else if ( typeof $.fn.appear == 'function' ) {
            this.log( 'Init animations' );

            $('.counter-block').not('.no-animation').appear(function () {
                if ( typeof $.fn.countTo == 'function' ) {
                    $(this).find('.counter-content > .count').countTo();
                } else {
                    _this.log( 'Can\'t find jQuery.countTo function' );
                }
            }, {accY: -150});

        }else {
            this.log( 'Can\'t find jQuery.appear function' );
            this.log( 'Remove animations' );
        }
    },

    calculateMenuSizes: function () {
        var _this = this,
            $header = $('.header'),
            $dropdownItem = $('.navigation-item.dropdown'),
            $navbarCollapse = $('.navbar-collapse'),
            winHeight = window.innerHeight;

        if ( $dropdownItem.length ) {
            $dropdownItem.each( function () {
                var $this = $(this),
                    $dropdown = $this.find('.dropdown-menu'),
                    headerHeight = $dropdownItem.closest('header').outerHeight();

                if ( !_this.mobileDevice ) {
                    $dropdown.css( 'max-height', winHeight - headerHeight - 50 );

                }else {
                    $dropdown.css( 'max-height', '' );

                }
            });
        }

        if ( $navbarCollapse.length ) {
            $navbarCollapse.each( function () {
                if ( _this.mobileDevice ) {
                    var headerHeight = $(this).closest('header').outerHeight();

                    $(this).css('max-height', winHeight - headerHeight);
                }
            });
        }

        if ( _this.mobileDevice ) {
            $('body').css( 'padding-top', $header.height() );
        }else {
            $('body').css( 'padding-top', '' );
        }

    },

    windowHeightBlock: function () {
        var height = window.innerHeight - $('.header-wrapper').outerHeight();

        $('.hero-section.window-height').css('min-height', window.innerHeight);

        $('.vc_row-fluid.window-height').each(function () {
            if ( !$(this).is('.vsc_inner') ) {
                $(this).css('min-height', height);
            }
        });

        $('.wpb_column.full-height').each(function () {
            if ( $(this).css('float') == 'none' ) {
                $(this).height('auto');

            } else {
                var $closestRow = $(this).closest('.vc_row'),
                    paddingTop = $closestRow.css('padding-top'),
                    paddingBottom = $closestRow.css('padding-bottom');

                $(this).height('auto');
                $(this).outerHeight($closestRow.outerHeight());
                $(this).css({
                    'margin-top': '-' + paddingTop,
                    'margin-bottom': '-' + paddingBottom
                })

            }
        });

        $('.vc_row-fluid.window-height.vsc_inner').each(function () {
            $(this).css('min-height', $(this).closest('.wpb_column').height() );
        });

        this.log( 'Init window height blocks');
    },

    centeredBlock: function() {
        $('.vc_row-fluid.centered-content').each(function () {
            var rowHeight = $(this).outerHeight(),
                $container = $(this).find('>.container'),
                containerHeight = 0,
                padding = 0;

            if ( !$container.length ) {
                $container = $(this).find('.column_container').first();
            }

            containerHeight = $container.outerHeight();
            padding = (rowHeight - containerHeight) / 2;

            $(this).css({ 'padding-top': padding, 'padding-bottom': padding });
        });

        if ( $('.centered-block').length ) {
            $('.centered-block').each( function () {
                var $el = $(this),
                    $parent = $el.parent(),
                    elHeight = $el.outerHeight(),
                    parentHeight = $parent.outerHeight(),
                    padding = (parentHeight - elHeight) / 2;

                if ( padding >= 0 ) {
                    $parent.css({'padding-top': padding, 'padding-bottom': 0 });
                }
            });
        }

        this.log( 'Init centered blocks');
    },

    videoBackgroundInit: function () {
        var _this = this,
            height = window.innerHeight - $('.header-wrapper').outerHeight();

        $('.ytpb-row').each( function() {
            var $div = $(this),
                paddingTop = parseInt($div.css('padding-top')),
                paddingBottom = parseInt($div.css('padding-bottom')),
                videoUrl = $div.data('videoUrl'),
                vid,
                $player,
                controlsTempalte;

            if ( !(videoUrl && this.id) ) {
                return false;
            }

            $div.css('min-height', $div.outerHeight());
            $div.children().not('.row-overlay').first().css({ 'padding-top': paddingTop, 'padding-bottom': paddingBottom });
            $div.style('padding-top', '0', 'important');
            $div.style('padding-bottom', '0', 'important');

            /(?:youtu\.be\/|youtube\.com\/(?:watch\?(?:.*&)?v=|(?:embed|v)\/))([^\?&"'>]+)/.test(videoUrl);

            vid = RegExp.$1;

            if ( vid ) {
                $div.css({
                    'background-image': 'url("http://img.youtube.com/vi/' + vid + '/maxresdefault.jpg")',
                    'background-size': 'cover',
                    '-webkit-background-size': 'cover'
                });
            }

            if ( !_this.checkMobile() ) {
                $player = $div.mb_YTPlayer();

                if ( $div.attr('data-controls') != 'none' ) {
                    controlsTempalte = '<div class="video-conrols"><i class="yt-play-btn-big"' + ( ( $div.attr('data-autoplay') == 'true' ) ? ' style="display: none"' : '' ) + '></i><div class="bottom"><div class="controls-container ' + $div.attr('data-controls') + '"><i class="yt-play-toggle' + ( ( $div.attr('data-autoplay') == 'false' ) ? ' active' : '' ) + '"></i><i class="yt-mute-toggle ' + ( ( $div.attr('data-mute') == 'true' ) ? ' active' : '' ) + '"></i><div class="yt-volume-slider"></div></div></div><div>'

                    $div.append(controlsTempalte);

                    var $playBtn = $div.find('.yt-play-btn-big'),
                        $playToggle = $div.find('.yt-play-toggle'),
                        $muteToggle = $div.find('.yt-mute-toggle'),
                        $volumeSlider = $div.find('.yt-volume-slider');

                    $volumeSlider.slider({
                        range: 'min',
                        min: 0,
                        max: 100,
                        step: 5,
                        value: 50,
                        slide: function ( event, ui ) {
                            $player.setYTPVolume(ui.value);
                        }
                    });

                    $playBtn.on('click', function () {
                        $player.YTPPlay();
                    });

                    $playToggle.on('click', function () {
                        if ( $(this).is('.active') ) {
                            $player.YTPPlay();
                        }else {
                            $player.YTPPause();
                        }
                    });

                    $muteToggle.on('click', function () {
                        if ( $(this).is('.active') ) {
                            $muteToggle.removeClass('active');
                        }else {
                            $muteToggle.addClass('active');
                        }

                        $player.toggleVolume();
                    });

                    $player.on("YTPStart", function (e) {
                        $playBtn.fadeOut(300);
                        $playToggle.removeClass('active');
                    });

                    $player.on("YTPPause", function (e) {
                        $playBtn.fadeIn(200);
                        $playToggle.addClass('active');
                    });
                }

            } else {
                $div.css('min-height', height).addClass('no-video-bg');

            }
        });

        this.log( 'Init row video background');
    },

    initPseudoSelect: function () {
        var $select = $('select');

        if ( $select.length ) {
            $select.each(this.makePseudoSelect);
        }
    },

    makePseudoSelect: function (i, el) {
        var $el = $(el),
            $options = $el.find('option'),
            $pseudoSelect = $('<div class="pseudo-select"></div>'),
            $input = $('<input type="text" />'),
            $field = $('<span class="pseudo-select-field"></span>'),
            $dropdown = $('<ul class="pseudo-select-dropdown"></ul>');

        $options.each(function () {
            $li = $('<li class="pseudo-select-dropdown-item"></li>');
            $li.data('value', this.value);
            $li.text(this.text);

            if ( this.disabled ) $li.addClass('disabled');
            if ( this.selected ) {
                $li.addClass('selected');
                $field.text(this.text);
                $input.attr('value', this.value);
            }

            $dropdown.append($li);
        });

        $el.after($pseudoSelect);
        $input.attr({ id: el.id, name: el.name });
        $pseudoSelect.append($input).append($field).append($dropdown);

        $el.remove();

        var closePseudoSelect = function () {
                $dropdown.stop(true, true).slideUp(150);
                setTimeout(function () { $el.removeClass('open'); }, 150);
            },
            openPseudoSelect = function () {
                $el.addClass('open');
                $dropdown.stop(true, true).slideDown(250);
            },
            selectItem = function ($li) {
                var value = $li.data('value'),
                    text = $li.text(),
                    dropdownHeight = $dropdown.outerHeight(),
                    elHeight = $li.outerHeight(),
                    scrollTop = $dropdown.scrollTop(),
                    elemPosition = $li.position().top;

                if ( elemPosition + elHeight > dropdownHeight ) {
                    $dropdown.scrollTop(elHeight + elemPosition - dropdownHeight + scrollTop);
                }else if ( elemPosition < 0 ) {
                    $dropdown.scrollTop(scrollTop + elemPosition);
                }

                $li.addClass('selected').siblings('li').removeClass('selected');
                $input.val(value);
                $field.text(text);
                $input.trigger('change');
            }

        $input.on('focus.pseudoSelect', openPseudoSelect);
        $input.on('blur.pseudoSelect', closePseudoSelect);

        $input.on('keydown', function (event) {
            var $li = $dropdown.find('li').not('.disabled'),
                $liSelected = $dropdown.find('li.selected').not('.disabled'),
                index = $.map($li, function (el, i) { if ( $(el).is('.selected')) { return i; } })[0],
                nextIndex = (index < $li.length - 1) ? index + 1 : 0,
                prevIndex = index - 1,
                $prev = $li.eq(prevIndex),
                $next = $li.eq(nextIndex);

            if ( event.keyCode == 38 ) {
                if ( $liSelected.length ) {
                    selectItem($prev);
                }else {
                    selectItem($li.last());
                }
            }

            if ( event.keyCode == 40 ) {
                if ( $liSelected.length ) {
                    selectItem($next);
                }else {
                    selectItem($li.first());
                }
            }

            if ( event.keyCode == 32 ) {
                if ( $el.is('.open') ) closePseudoSelect();
                else openPseudoSelect();
            }

            if ( event.keyCode != 13 && event.keyCode != 9 ) {
                return false;
            }
        });

        $field.on('click.pseudoSelect', function (event) {
            event.preventDefault();

            if ( !$el.is('.open') ) $input.trigger('focus');
        });

        $('body').on('click.pseudoSelect', function (event) {
            if ( !$(event.target).closest($field).length && $el.is('.open') ) closePseudoSelect();
        });

        $dropdown.on('mousedown.pseudoSelect click.pseudoSelect', 'li', function (event) {
            event.preventDefault();

            selectItem($(this));
            closePseudoSelect();
        });

        return $input;
    },

    // Tabs view
    tabNavToSelect: function() {
        $nav = $('.nav.nav-schedule');

        var _this = this;

        $nav.each(function () {
            var $this = $(this),
                $active = $this.find('li.active > a'),
                $field = $('<span class="nav-current">' + $active.html() + '</span>');

            if ( $this.find('> li').length <= 1 ) {
                return;
            }

            $this.wrapAll('<div class="nav-wrapper"></div>')

            $this.before($field);

            $field.on('click', function () {
                if ( !$this.is('.open') ) {
                    $this.stop(true, true).slideDown(250).addClass('open');

                }else {
                    $this.stop(true, true).slideUp(150).removeClass('open');

                }
            });

            $this.on('click', 'a', function () {
                $field.html($(this).html());
            });

            $('body').on('click', function (event) {
                $target = $(event.target);

                if ( !$target.closest($field).length && $this.is('.open') ) {
                    $this.stop(true, true).slideUp(150).removeClass('open');
                }
            });
        });
    },

    // Google map
    initGoogleMap: function() {
        var _this = this;

        if ( $('.vncp-map').length ) {
            $('.vncp-map').each(function () {
                var mapEl = this;

                var map, marker, geocoder, service;

                var icon = '/wp-content/themes/ventcamp/img/marker-46x46.png',
                    address,
                    markerLatLng,
                    balloons,
                    offsetX,
                    offsetY,
                    relativeOffset,
                    mapOptions = {
                        zoom: 14,
                        scrollwheel: false,
                        mapTypeControl: false
                    };

                function createMap () {
                    mapOptions.center = markerLatLng

                    map = new google.maps.Map(mapEl, mapOptions);

                    marker = new google.maps.Marker({
                        map: map,
                        icon: icon,
                        position: markerLatLng
                    });

                    map.addListener('projection_changed', function() {
                        centerMap(map, offsetX, offsetY, relativeOffset);
                    });

                    createBalloons();
                }

                function createBalloons () {
                    service = new google.maps.places.PlacesService(map);

                    if ( balloons ) {
                        for (var i = 0; i < balloons.length; i++) {
                            var balloon = balloons[i];

                            if ( typeof balloon == 'string' ) {
                                service.textSearch({
                                    location: markerLatLng,
                                    radius: 5000,
                                    query: balloon
                                }, function(results, status) {
                                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                                        for (var i = 0, place; place = results[i]; i++) {
                                            service.getDetails({placeId: place.place_id}, createMarkers);
                                        };
                                    }
                                });
                            }
                        };
                    }
                }

                function centerMap(map, offsetX, offsetY, relative) {
                    var offsetX = (typeof offsetX == 'number' ? offsetX : 0),
                        offsetY = (typeof offsetY == 'number' ? offsetY : 0),
                        zoom = map.getZoom(),
                        scale = Math.pow( 2, zoom ),
                        northEast = map.getBounds().getNorthEast(),
                        southWest = map.getBounds().getSouthWest(),
                        width = Math.abs( northEast.lng() - southWest.lng() ),
                        height = Math.abs( northEast.lat() - southWest.lat() ),
                        point1 = map.getProjection().fromLatLngToPoint( map.getCenter() ),
                        point2 = new google.maps.Point(
                            offsetX / scale,
                            offsetY / scale
                        ),
                        centerPoint = new google.maps.Point(
                            point1.x - point2.x,
                            point1.y - point2.y
                        ),
                        center = map.getProjection().fromPointToLatLng( centerPoint );

                    if ( relative ) {
                        center = new google.maps.LatLng(
                            map.getCenter().lat() + height * offsetY / 100,
                            map.getCenter().lng() - width * offsetX / 100
                        );
                    }

                    map.setCenter( center );
                }

                function createMarkers(place, status) {
                    if (status == google.maps.places.PlacesServiceStatus.OK) {
                        var innerContent;

                        innerContent = '<div class="balloon"><strong class="name">' + place.name + '</strong>';

                        if ( place.rating ) {
                            innerContent += '<ul class="list-inline rating">';

                            for (var i = 0; i < Math.floor(place.rating); i++) {
                                innerContent += '<li><span class="fa fa-star"></span></li>'
                            };

                            innerContent += '</ul>';
                        }

                        if ( place.adr_address ) {
                            innerContent += '<p class="address">' + place.adr_address + '</p>';
                        }

                        if ( place.international_phone_number ) {
                            innerContent += '<p class="phone">' + place.international_phone_number + '</p>';
                        }

                        innerContent += '</div>';

                        var infowindow = new google.maps.InfoWindow({
                            position: place.geometry.location,
                            maxWidth: 200,
                            content: innerContent,
                            disableAutoPan: true
                        });

                        infowindow.open(map);
                    }
                }

                function successCallback (data) {
                    if ( typeof data == 'object' ) {
                        offsetX = data.offsetX || 0;
                        offsetY = data.offsetY || 0;

                        if ( data.zoom ) mapOptions.zoom = data.zoom;
                        if ( data.icon && typeof data.icon == 'string' ) icon = data.icon;
                        if ( data.balloons ) balloons = data.balloons.split('|');

                        if ( data.relativeOffset ) relativeOffset = true;
                        else relativeOffset = false;

                        if ( data.address && typeof data.address == 'string' ) address = data.address;
                        else if ( data.latitude && data.longitude ) markerLatLng = new google.maps.LatLng(data.latitude, data.longitude);
                    }

                    geocoder = new google.maps.Geocoder();

                    if ( typeof markerLatLng == 'object' ) {
                        createMap();

                    }else if ( typeof address == 'string' ) {
                        geocoder.geocode({ 'address': address }, function(results, status) {
                            if ( results.length ) {
                                markerLatLng = results[0].geometry.location;
                                createMap();
                            }else {
                                _this.log('Can\'t find address');
                            }
                        });
                    }
                }

                function failCallback () {
                    _this.log("Can't parse map settings!");

                    new google.maps.Map(mapEl, {
                        center: new google.maps.LatLng(0, 0),
                        zoom: 2
                    });
                }

                if ( $(this).data('fillRow') ) {
                    var $row = $(this).closest('.vc_row'),
                        $el = $(this).detach();

                    $el.prependTo($row);
                    $el.css({
                        'position': 'absolute',
                        'top': '0',
                        'right': '0',
                        'bottom': '0',
                        'left': '0',
                        'width': 'auto',
                        'height': 'auto'
                    })
                }

                if ( ( $(this).data('latitude') && $(this).data('longitude') ) || $(this).data('address') ) {
                    successCallback($(this).data());

                }else {
                    failCallback();
                }
            });
        }
    },

    // count down timer
    countdownInit: function () {
        if ( $('.countdown').length ) {
            if ( typeof $.fn.countdown == 'function' ) {
                $('.countdown').each(function () {
                    var $el = $(this),
                        date = new Date($el.data('date')) || new Date().getDate() + 10,
                        direction = $el.data('direction') || 'down',
                        format = $el.data('format') || 'DHMS',
                        layout = $el.html(),
                        options = {
                            format: format,
                            padZeroes: true
                        };

                    if ( direction == 'up' ) {
                        $.extend(options, { since: date });

                    }else {
                        $.extend(options, { until: date });

                    }

                    if ( $el.not('.inline-layout') ) {
                        $el.addClass('format-length-' + format.length);
                        $.extend(options, { layout: layout });

                    }else {
                        $.extend(options, { compact: true });

                    }

                    $el.countdown(options);
                });

            }else {
                this.log( 'Can\'t find jQuery.coundown function' );

            }
        }
    },

    masonryInit: function () {
        var _this = this;

        if ( $('.masonry').length && typeof $.fn.masonry == 'function' ) {
            $('.masonry').each(function () {
                var $container = $(this),
                    $masonry,
                    masonryResizeCallback = function ($container) {
                        var containerWidth = $container.width();

                        if ( containerWidth > 940 ) {
                            $container.removeClass('width940').removeClass('width720').removeClass('width620').removeClass('width380');

                        }else if ( containerWidth > 720 ) {
                            $container.addClass('width940').removeClass('width720').removeClass('width620').removeClass('width380');

                        }else if ( containerWidth > 620 ) {
                            $container.addClass('width720').removeClass('width940').removeClass('width620').removeClass('width380');

                        }else if ( containerWidth > 380 ) {
                            $container.addClass('width620').removeClass('width940').removeClass('width720').removeClass('width380');

                        }else {
                            $container.addClass('width380').removeClass('width940').removeClass('width720').removeClass('width620');
                        }
                    }

                masonryResizeCallback($container);

                $masonry = $container.masonry({
                    itemSelector: '.masonry-item',
                    isAnimated: true,
                    percentPosition: true,
                    columnWidth: '.masonry-item'
                });

                $(window).on('resize.masonry', function () {
                    masonryResizeCallback($container);
                });
            });
        }
    },

    //sticky menu initialization
    stickMenu: function () {
        var $header = $('header.header');

        $header.css('min-height', $header.height());
        $header.addClass('fixed');
        $('.fade-in-on-stick').fadeIn();
    },

    unstickMenu: function () {
        var $header = $('header');

        $header.removeClass('fixed');
        $header.css('min-height', '');
        $('.fade-in-on-stick').fadeOut();
    },

    vcTabsFix: function () {
        var $tabsList = $('.vc_tta-tabs-list'), $elems;

        if ( $tabsList.length ) {
            $elems = $tabsList.find('> li');

            $elems.each(function() {
                if ( $(this).next().length ) {
                    $(this).after(" ");
                }
            });
        }
    },

    headerMenuFix: function () {
        var _this = this,
            $menuItemHasChildren = $('.header .menu-item.menu-item-has-children');

        if ( $menuItemHasChildren.length ) {
            $menuItemHasChildren.each(function () {
                var $el = $(this),
                    $subMenu = $el.find('.sub-menu'),
                    $subMenuItem = $subMenu.find('> li');

                if ( $subMenu.length && $subMenuItem.length > 10 ) {
                    $el.addClass('too-many-items');
                }

            });
        }
    },

    //one page menu navigation
    onePageNavInit: function () {
        var _this = this;

        if ( typeof $.fn.waypoint != 'undefined' ) {
            var $menuLinks = $('.navigation-list a').not('[href="#"]').filter(function () {
                    return /#\w+/.test(this.href);
                }),
                hashChange = _this.options.onePageNavHashChange,
                pushState = history.pushState,
                enterHandler = function( that, direction ) {
                    var id = that.id,
                        $item = $('.navigation-list a').filter(function () {
                            return this.href.indexOf('#' + that.id) > -1;
                        });

                    $('.navigation-list .active').removeClass('active');
                    $item.addClass('active');

                    // push anchor to browser URL
                    if ( hashChange ){
                        if ( pushState ) {
                            history.pushState(null, null, '#' + id);
                        }else {
                            _this.log('Browser don\'t support history API');
                        }
                    }
                },
                leaveHandler = function ( that, direction ) {
                    var $item = $('.navigation-list a').filter(function () {
                            return this.href.indexOf('#' + that.id) > -1;
                        });

                    $item.removeClass('active');

                    if ( hashChange ){
                        if ( pushState && window.location.hash == '#' + that.id ) {
                            history.pushState('', document.title, window.location.pathname);
                        }else {
                            _this.log('Browser don\'t support history API');
                        }
                    }
                };

            $menuLinks.each(function (index) {
                var href = this.href,
                    anchorId = href.substring(href.indexOf('#'), href.length),
                    $block = $(anchorId);

                if ( $block.length ) {
                    $block.waypoint(function (direction) {
                        if ( direction == 'down' ) {
                            enterHandler( this, direction );
                        }

                    }, { offset: 0 });

                    $block.waypoint(function (direction) {
                        if ( direction == 'down' ) {
                            leaveHandler( this, direction );

                        }else {
                            enterHandler( this, direction );
                        }

                    }, { offset: -$block.outerHeight() });

                    $block.waypoint(function (direction) {
                        if ( direction == 'up' ) {
                            leaveHandler( this, direction );
                        }

                    }, { offset: '100%' });
                }
            });

            $('body').waypoint(function () {
                var id = 'hero',
                    $item = $('.navigation-list a[href="#' + id + '"]');

                $('.navigation-list .active').removeClass('active');

                if ( $item.length ) {
                    $item.addClass('active');

                    if ( _this.options.onePageNavHashChange ){
                        if ( history.pushState ) {
                            history.pushState(null, null, '#' + id);
                        }else {
                            _this.log('Browser don\'t support history API');
                        }
                    }
                }
            }, { offset: -100 });

            $('body').on( 'click', 'a[href*="#"]', function (event) {
                var href = $(this).attr('href'),
                    anchorId = href.substring(href.indexOf('#'), href.length);

                if ( typeof $(this).attr('data-vc-accordion') != 'undefined' ||
                     typeof $(this).attr('data-vc-tours') != 'undefined' ||
                     typeof $(this).attr('data-vc-tabs') != 'undefined' ||
                     $(this).hasClass('schedule-item-toggle') ||
                     $(this).attr('data-toggle') == 'tab' ) {
                    return;
                }

                if ( $(anchorId).length ) {
                    _this.anchorClickHandler(anchorId);

                    return false;
                }
            });

        }else {
            this.log( 'Can\'t find jQuery.waypoint function' );

        }
    },

    //custom smooth scrolling for all onpage anchors
    anchorClickHandler: function(anchorId) {
        var _this = this,
            $nav = $('.navigation-list'),
            $elems = $nav.find('a[href="' + anchorId + '"]'),
            offsetTop;

        if ( !$(anchorId).length ) {
            return false;
        }

        offsetTop = $(anchorId).offset().top - $('.header').height()

        $('body, html').animate({
            scrollTop: offsetTop

        }, 450, function () {
            if ( _this.options.onePageNavHashChange ) {
                if ( history.pushState ) {
                    history.pushState(null, null, anchorId);

                }else {
                    window.location.hash = anchorId;

                }
            }

            $nav.find('.active').removeClass('active');
            $elems.addClass('active');
        });
    },

    checkHeaderStatus: function () {
        if ( $('header').length && !this.options.headerRelativePosition ) {
            var $header = $('header'),
                scrollTop = $(window).scrollTop(),
                headerTop = $header.offset().top;

            if ( scrollTop >= headerTop ) {
                this.stickMenu();

            }else {
                this.unstickMenu();
            }

            if ( $('body').is('.admin-bar') ) {
                $('html').style('margin-top', '0', 'important');
            }
        }
    },

    //onload handler
    windowLoadHeandler: function (event) {
        this.log('Window load handler');

        if ( this.options.onePageNav ) {
            if ( window.location.hash.length ) {
                this.anchorClickHandler(window.location.hash);
            }

            this.onePageNavInit();
        }

        this.checkHeaderStatus();
    },

    //on resize handler
    windowResizeHandler: function (event) {
        this.checkMobile();

        this.windowHeightBlock();

        this.centeredBlock();

        this.calculateMenuSizes();
    },

    //on scroll handler
    windowScrollHandler: function (event) {
        this.checkHeaderStatus();
    },

    //small back-to-top link function
    backToTopHandler: function () {
        $('html, body').animate({
            scrollTop: 0,
            easing: 'swing'
        }, 750);
    },

    setEventHandlers: function () {
        var _this = this;

        $(window).on('load', function (event) {
            _this.windowLoadHeandler(event);
        });

        $(window).on('resize', function (event) {
            _this.windowResizeHandler(event);
        });

        $(window).on('scroll', function (event) {
            _this.windowScrollHandler(event);
        });

        $('.back-to-top').on('click', function (event) {
            event.preventDefault();

            _this.backToTopHandler();
        });

        $('.navbar-collapse a').on('click', function (event) {
            if ( !$(this).closest('.menu-item-has-children').length && _this.mobileDevice ) {
                $(this).closest('header').find('.navbar-toggle').trigger('click');
            }
        });

        $('.navbar-collapse .menu-item-has-children > a').on('click', function (event) {
            event.preventDefault();

            var $subMenu = $(this).siblings('.sub-menu');

            if ( $subMenu.length ) $subMenu.toggle();
        });

        $('.navbar-collapse').on( 'mousewheel DOMMouseScroll', function (event) {
            if ( _this.mobileDevice ) {
                var e0 = event.originalEvent,
                    delta = e0.wheelDelta || -e0.detail;

                this.scrollTop -= delta;
                event.preventDefault();
            }
        });

        this.log('Set event hendlers');
    },

    hidePreloader: function (callback) {
        var _this = this;

        $('.preloader-mask').delay(500).fadeOut(600);

        setTimeout(function() {
            _this.initCounters();

        }, 1000);

        if ( callback ) {
            callback();
        }
    },

    init: function (options) {
        this.options = $.extend(this.defaults, options, $('body').data());

        this.log('Init');

        this.checkMobile();

        this.tabNavToSelect();

        if ( this.options.smoothScroll ) this.smoothScrollInit();

        if ( this.options.pseudoSelect ) this.initPseudoSelect();

        if ( typeof google != 'undefined') this.initGoogleMap();

        this.calculateMenuSizes();

        this.windowHeightBlock();

        this.videoBackgroundInit();

        this.centeredBlock();

        this.vcTabsFix();

        this.headerMenuFix();

        this.countdownInit();

        this.masonryInit();

        this.setEventHandlers();

        this.hidePreloader();
    }
}

//jQuery style helper
if (!$.fn.style) {
    var e = function (t) {
        return t.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
    },
    r = !!CSSStyleDeclaration.prototype.getPropertyValue;

    r || (
        CSSStyleDeclaration.prototype.getPropertyValue = function (t) {
            return this.getAttribute(t);
        },

        CSSStyleDeclaration.prototype.setProperty = function (t, r, n) {
            this.setAttribute(t, r);
            var n = "undefined" != typeof n ? n : "";

            if ("" != n) {
                var i = new RegExp(e(t) + "\\s*:\\s*" + e(r) + "(\\s*;)?", "gmi");
                this.cssText = this.cssText.replace(i, t + ": " + r + " !" + n + ";");
            }
        },

        CSSStyleDeclaration.prototype.removeProperty = function (t) {
            return this.removeAttribute(t);
        },

        CSSStyleDeclaration.prototype.getPropertyPriority = function (t) {
            var r = new RegExp(e(t) + "\\s*:\\s*[^\\s]*\\s*!important(\\s*;)?", "gmi");

            return r.test(this.cssText) ? "important" : "";
        }
    ),

    $.fn.style = function (t, e, r) {
        var n = this.get(0);

        if ("undefined" == typeof n) return this;

        var i = this.get(0).style;

        return "undefined" != typeof t ? "undefined" != typeof e ? (r = "undefined" != typeof r ? r : "", i.setProperty(t, e, r), this) : i.getPropertyValue(t) : i
    }
}

})( jQuery );
