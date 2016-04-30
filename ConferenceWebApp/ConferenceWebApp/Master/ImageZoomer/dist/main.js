window.onload = function () {

  'use strict';

  var Viewer = window.Viewer;
  var console = window.console || { log: function () {} };
  var pictures = document.querySelector('.docs-pictures');
  var toggles = document.querySelector('.docs-toggles');
  var buttons = document.querySelector('.docs-buttons');
  var options = {
        // inline: true,
      url: 'data-original',
      rotatable: false,
      scalable: false,
        build: function (e) {
        },
        built:  function (e) {
        },
        show:  function (e) {
        },
        shown:  function (e) {
        },
        hide:  function (e) {
        },
        hidden:  function (e) {
        },
        view:  function (e) {
        }
      //,
        //viewed:  function (e) {
        //  // this.viewer.zoomTo(1).rotateTo(180);
        //}
      };
  var viewer;

  function toggleButtons(mode) {
    var targets;
    var target;
    var length;
    var i;

    if (/modal|inline|none/.test(mode)) {
      targets = buttons.querySelectorAll('button[data-enable]');

      for (i = 0, length = targets.length; i < length; i++) {
        target = targets[i];
        target.disabled = true;

        if (String(target.getAttribute('data-enable')).indexOf(mode) > -1) {
          target.disabled = false;
        }
      }
    }
  }
  viewer = new Viewer(pictures, options);

  toggleButtons(options.inline ? 'inline' : 'modal');

  toggles.onchange = function (event) {
    var e = event || window.event;
    var input = e.target || e.srcElement;
    var name;

    if (viewer) {
      name = input.getAttribute('name');
      options[name] = name === 'inline' ? JSON.parse(input.getAttribute('data-value')) : input.checked;
      viewer.destroy();
      viewer = new Viewer(pictures, options);
      toggleButtons(options.inline ? 'inline' : 'modal');
    }
  };

  buttons.onclick = function (event) {
    var e = event || window.event;
    var button = e.target || e.srcElement;
    var method = button.getAttribute('data-method');
    var target = button.getAttribute('data-target');
    var args = JSON.parse(button.getAttribute('data-arguments')) || [];

    if (viewer && method) {
      if (target) {
        viewer[method](target.value);
      } else {
        viewer[method](args[0], args[1]);
      }

      switch (method) {
        case 'scaleX':
        case 'scaleY':
          args[0] = -args[0];
          break;

        case 'destroy':
          viewer = null;
          toggleButtons('none');
          break;
      }
    }
  };

};
