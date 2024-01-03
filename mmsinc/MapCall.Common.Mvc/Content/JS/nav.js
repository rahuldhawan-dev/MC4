var Menu = (function($, window) {
  var navMenuInstance;
  var navMenuParent;
  var siteWrapInstance;

  var NO_MENU_CLASS = 'no-menu';
  var DISABLE_AUTO_MENU_CLASS = 'disable-automatic-menu';
  var SLIM_CLASS = 'slim';

  var MapCall = {
    LOOKUP_STRINGS: {
      siteWrap: 'html',
      mainMenu: '#nav',
      titleDiv: 'div.title',
      contentList: '#nav div.content > ul',
      linkOnlyHeaders: '#nav div.linkOnly a',
      toggleMenuButton: '#toggleMenuButton'
    },

    initialize: function() {
      navMenuInstance = $(MapCall.LOOKUP_STRINGS.mainMenu);
      if (navMenuInstance.length == 0) {
        // No menu is being displayed server-side, so there's nothing to 
        // do here.
        return;
      }

      siteWrapInstance = $(MapCall.LOOKUP_STRINGS.siteWrap);
      navMenuParent = navMenuInstance.parent();
      MapCall.initializeMenu();
    },

    initializeMenu: function() {
      MapCall.setMenuVisibility(settingsCookie.get(true));
      var disableAutomaticMenu = siteWrapInstance.hasClass(DISABLE_AUTO_MENU_CLASS);

      // When initializing alot of these jQuery controls, they should be set to display:none until
      // they've been fully initialized. This allows for faster rendering since the browser
      // won't have to actually draw the controls until they've finished creating secondary
      // styling elements.  This may cause issues with slow connections or huge table loads.

      navMenuInstance.accordion({
        header: MapCall.LOOKUP_STRINGS.titleDiv,
        heightStyle: 'content',
        collapsible: true
      });

      var menuSections = navMenuInstance.find('> div');

	  // NOTE: MapCall proper and 271 parse the url to find links. With MVC we're able to do proper route checking instead.
	  var routeData = Application.routeData;
	  var activeLinkFilter = function () {
		  var thisLink = $(this);
		  if (thisLink.data("area") === routeData.area && thisLink.data("controller") === routeData.controller) {
			  return true;
		  }
		  return false;
      };

      // NOTE: The below code does NOT deal with the treeview aspect of opening
      //       to the correct link!
      var activeSection = menuSections.filter(function() {
        var activeLink = $(this).find('a').filter(activeLinkFilter);
        return (activeLink.length > 0);
      });

      if (!disableAutomaticMenu) {
        navMenuInstance.accordion('option', 'active', activeSection.index());
      }

      // Initializes the treeview part of the site-wide menu.
      // There's custom code added in jquery.treeview.js that were needed
      // so that it worked properly when urls contain a querystring or a #.
      var treeArgs = { collapsed: true, activeLinkFilter: activeLinkFilter };
      if (!disableAutomaticMenu) {
        treeArgs.persist = 'location';
      }
      $(MapCall.LOOKUP_STRINGS.contentList).treeview(treeArgs);

      // This is needed to make link-only accordion headers function properly.
      $(MapCall.LOOKUP_STRINGS.linkOnlyHeaders).click(MapCall.linkOnlyHeader_Click);
      $(MapCall.LOOKUP_STRINGS.toggleMenuButton).click(MapCall.toggleMenu);

      navMenuInstance.css('display', 'block');

      // TODO: Clean this up a bit if we go with this kind of layout.
      $('#menuScrollWrap').scrollless({ scrollChild: '.menu-scroll-child' });
    },

    linkOnlyHeader_Click: function(e) {
      $(window).attr('location', e.currentTarget.href);
    },

    isMenuVisible: function() {
      return !siteWrapInstance.hasClass(NO_MENU_CLASS) && !MapCall.isSlimTheme();
    },

    isSlimTheme: function() {
      return siteWrapInstance.hasClass(SLIM_CLASS);
    },

    toggleMenu: function() {
      MapCall.setMenuVisibility(!MapCall.isMenuVisible());
    },

    setMenuVisibility: function(makeVisible) {

      // This attaches and detaches the menu entirely so we can clear
      // up a lot of dom nodes when the menu isn't displaying. Could
      // speed up regression tests if we have NoMenu set by default.
      if (makeVisible) {
        siteWrapInstance.removeClass(NO_MENU_CLASS);
        navMenuParent.append(navMenuInstance);
        settingsCookie.set(true);
      }
      else {
        siteWrapInstance.addClass(NO_MENU_CLASS);
        navMenuInstance.detach();
        settingsCookie.set(false);
      }
    },

    getLocationHref: function() {
      return location.href;
    },

    fixHref: function(str) {
      if (str.indexOf('?') > -1) {
        return str.split('?')[0];
      }
      else if (str.indexOf('#') > -1) {
        return str.split('#')[0];
      }
      return str;
    }
  };

  // TODO: I can see this becoming something a bit more global if we need
  //       to keep track of other things. So this is separated from the
  //       MapCall object for now.
  var settingsCookie = {
    COOKIE_NAME: 'menuIsVisible',
    set: function(menuIsVisible) {
      // The path needes to be explicitly set to / or else the setting is forgotten
      // on any pages in different directories.
      document.cookie = settingsCookie.COOKIE_NAME + "=" + menuIsVisible + "; path=/";
    },
    get: function(defaultValueIfNotSet) {
      if (document.cookie) {
        var cookies = document.cookie.split('; ');
        for (var i = 0; i < cookies.length; i++) {
          var cur = cookies[i].split('=');
          if (cur[0] == settingsCookie.COOKIE_NAME) {
            return settingsCookie._getSafeValue(cur[1]);
          }
        }
      }

      return defaultValueIfNotSet;
    },
    _getSafeValue: function(val) {
      if (val === 'true') {
        return true;
      }
      else if (val === 'false') {
        return false;
      }
      return val;
    }
  };

  $(document).ready(MapCall.initialize);

  return MapCall;
})(jQuery, window);