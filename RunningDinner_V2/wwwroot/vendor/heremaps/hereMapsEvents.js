﻿
/**
 * The code below uses open source software. Please visit the URL below for an overview of the licenses:
 * http://js.api.here.com/v3/3.1.11.0/HERE_NOTICE
 */

H.util.eval("function fo(a){var b=a.ownerDocument;b=b.documentElement||b.body.parentNode||b.body;try{var c=a.getBoundingClientRect()}catch(d){c={top:0,right:0,bottom:0,left:0,height:0,width:0}}return{x:c.left+(\"number\"===typeof window.pageXOffset?window.pageXOffset:b.scrollLeft),y:c.top+(\"number\"===typeof window.pageYOffset?window.pageYOffset:b.scrollTop)}}var go=Function(\"return this\")();function ho(a,b,c,d,e,f,g){ho.l.constructor.call(this,a);this.pointers=b;this.changedPointers=c;this.targetPointers=d;this.currentPointer=e;this.originalEvent=g;this.target=f}u(ho,ld);t(\"H.mapevents.Event\",ho);function io(a,b,c,d,e,f){if(isNaN(a))throw Error(\"x needs to be a number\");if(isNaN(b))throw Error(\"y needs to be a number\");if(isNaN(c))throw Error(\"pointer must have an id\");this.viewportX=a;this.viewportY=b;this.target=null;this.id=c;this.type=d;this.dragTarget=null;this.a=this.button=nc(e)?e:-1;this.buttons=nc(f)?f:0}t(\"H.mapevents.Pointer\",io);\nfunction jo(a,b,c){if(isNaN(b))throw Error(\"x needs to be a number\");if(isNaN(c))throw Error(\"y needs to be a number\");a.viewportX=b;a.viewportY=c}io.prototype.nm=function(){return this.a};io.prototype.getLastChangedButton=io.prototype.nm;function ko(a,b){a.a=b;a.buttons|=io.prototype.b[+b]||0}function lo(a,b){a.a=b;a.buttons&=~(io.prototype.b[+b]||0)}io.prototype.b=[1,4,2];var mo={NONE:-1,LEFT:0,MIDDLE:1,RIGHT:2};io.Button=mo;function no(a){this.a=a instanceof Array?a.slice(0):[]}n=no.prototype;n.clear=function(){this.a.splice(0,this.a.length)};n.length=function(){return this.a.length};n.indexOf=function(a){for(var b=this.a.length;b--;)if(this.a[b].id===a)return b;return-1};function oo(a,b){b=a.indexOf(b);return-1!==b?a.a[b]:null}n.remove=function(a){a=this.indexOf(a);return-1!==a?this.a.splice(a,1)[0]:null};function po(a,b){for(var c=a.a.length,d=[];c--;)a.a[c].type!==b&&d.push(a.a[c]);a.a=d}\nfunction qo(a,b){for(var c=a.a.length;c--;)if(a.a[c].dragTarget===b)return!0;return!1}n.push=function(a){if(a instanceof io)return this.a.push(a);throw Error(\"list needs a pointer\");};n.Pa=function(){return this.a};n.clone=function(){return new no(this.a)};function ro(a,b,c){c=c||{};if(!(a instanceof T))throw Error(\"events: map instance required\");if(!(b instanceof Array))throw Error(\"events: map array required\");hd.call(this);this.Wg=c.Wg||300;this.gj=c.gj||50;this.Wk=c.Wk||50;this.Xk=c.Xk||500;this.Jh=c.Jh||900;this.Ih=c.Ih||8;this.map=a;this.o=this.map.Da;this.i=this.o.element;this.D=b;this.a=new no;this.b=new no;this.j={};this.c=null;this.fa=!0;this.v={};this.f={};this.m=null;this.ve=z(this.ve,this);this.K={pointerdown:this.Al,pointermove:this.Bl,\npointerup:this.Cl,pointercancel:this.zl};so(this)}u(ro,hd);function so(a,b){var c,d=a.D.length;for(c=0;c<d;c++){var e=a.D[c];var f=e.listener;\"function\"===typeof f&&(b?(e.target||a.i).removeEventListener(e.Ta,f):(e.target||a.i).addEventListener(e.Ta,f))}}function to(a,b,c){var d;if(\"function\"===typeof a.K[b]){\"pointermove\"!==b&&(a.fa=!0);var e=0;for(d=a.b.length();e<d;e++){var f=a.b.a[e];a.i.contains(c.target)?uo(a,f,a.mj.bind(a,c,b,f)):a.mj(c,b,f,null)}}a.b.clear()}n=ro.prototype;\nn.mj=function(a,b,c,d){vo(c.id,this.v);this.K[b].call(this,c,d,a)};function uo(a,b,c){if(a.c===b)c(b.target);else{var d=a.o;var e=b.viewportX;b=b.viewportY;if(0>e||0>b||e>=d.width||b>=d.height)c(y);else{var f=a.map;f.Qd(e,b,function(g){c(g||f)})}}}\nn.Cl=function(a,b,c){var d=a.id;a.target=b;wo(this,a,c);xo(this,b,\"pointerup\",c,a);\"mouse\"!==a.type&&xo(this,b,\"pointerleave\",c,a);b=this.j[a.id];var e={x:a.viewportX,y:a.viewportY},f=c.timeStamp,g=a.target,h=this.m;b&&b.target===g&&b.qe.Ya(e)<this.Wk&&f-b.Li<this.Xk?(xo(this,g,\"tap\",c,a),h&&h.target===g&&f-h.Li<this.Wg?h.qe.Ya({x:a.viewportX,y:a.viewportY})<this.gj&&(xo(this,g,\"dbltap\",c,a),this.m=null):this.m={target:g,qe:new I(a.viewportX,a.viewportY),Li:c.timeStamp}):this.m=null;this.j={};vo(d,\nthis.f)};function wo(a,b,c){b===a.c&&(xo(a,b.dragTarget,\"dragend\",c,b),a.c=null,vo(b.id,a.v));b.dragTarget=null}n.ve=function(a,b){var c=this;xo(this,a.dragTarget,\"drag\",b,a);vo(a.id,this.v);this.v[a.id]=setTimeout(function(){c.ve(a,b)},150)};function vo(a,b){b[a]&&(b[a].timeout?clearTimeout(b[a].timeout):clearTimeout(b[a]),delete b[a])}\nfunction yo(a,b,c){var d=b.target,e=new I(b.viewportX,b.viewportY),f=b.id;vo(f,a.f);var g=setTimeout(function(){d&&d===b.target&&e.Ya({x:b.viewportX,y:b.viewportY})<a.Ih&&(xo(a,d,\"longpress\",c,b),delete a.j[b.id])},a.Jh);a.f[f]={timeout:g,qe:e}}\nn.Bl=function(a,b,c){var d=a.dragTarget,e=a.id;var f=a.target;a.target=b;f!==b&&(xo(this,f,\"pointerleave\",c,a),xo(this,b,\"pointerenter\",c,a));d?this.c?(this.ve(a,c),this.f[e]&&this.f[e].qe.Ya({x:a.viewportX,y:a.viewportY})>this.Ih&&vo(e,this.f)):this.fa?this.fa=!1:(this.c=a,xo(this,d,\"dragstart\",c,a),this.ve(a,c),delete this.j[e],this.fa=!0):(!this.c||this.c&&this.c.dragTarget!==b&&this.c.dragTarget!==this.map)&&xo(this,b,\"pointermove\",c,a)};\nn.Al=function(a,b,c){var d=!(/^(?:mouse|pen)$/.test(a.type)&&0!==c.button);if(b){a.target=b;this.j[a.id]={qe:new I(a.viewportX,a.viewportY),target:a.target,Li:c.timeStamp};\"mouse\"!==a.type&&xo(this,b,\"pointerenter\",c,a);var e=xo(this,b,\"pointerdown\",c,a);!this.c&&d&&(b.draggable&&!qo(this.a,b)?a.dragTarget=b:!this.map.draggable||e.defaultPrevented||qo(this.a,this.map)||(a.dragTarget=this.map));yo(this,a,c)}};\nn.zl=function(a,b,c){var d=a.id;a.target=null;b?(xo(this,b,\"pointerleave\",c,a),xo(this,b,\"pointercancel\",c,a)):xo(this,this.map,\"pointercancel\",c,a);wo(this,a,c);this.j={};vo(d,this.f)};function xo(a,b,c,d,e){if(b&&\"function\"===typeof b.dispatchEvent){var f=ho;var g=a.a.Pa(),h=a.b.Pa();a=a.a;var k,l=a.a.length,m=[];for(k=0;k<l;k++)a.a[k].target===b&&m.push(a.a[k]);f=new f(c,g,h,m,e,b,d);e.button=/^(?:longpress|(?:dbl)?tap|pointer(?:down|up))$/.test(c)?e.a:mo.NONE;b.dispatchEvent(f)}return f}\nn.u=function(){so(this,!0);this.a.clear();this.b.clear();var a=this.v,b;for(b in a)vo(b,a);a=this.f;for(var c in a)vo(c,a);this.c=this.j=this.m=this.map=this.b=this.a=this.D=this.O=null;hd.prototype.u.call(this)};function zo(a){this.g=z(this.g,this);ro.call(this,a,[{Ta:\"touchstart\",listener:this.g},{Ta:\"touchmove\",listener:this.g},{Ta:\"touchend\",listener:this.g},{Ta:\"touchcancel\",listener:this.g}]);this.F={touchstart:\"pointerdown\",touchmove:\"pointermove\",touchend:\"pointerup\",touchcancel:\"pointercancel\"};this.s=(a=(a=a.m)?a.J():null)?Array.prototype.slice.call(a.querySelectorAll(\"a\"),0):[]}u(zo,ro);\nzo.prototype.g=function(a){var b=a.touches,c=this.a.length(),d;if(\"touchstart\"===a.type&&c>=b.length){c=this.a.clone();for(d=b.length;d--;)c.remove(b[d].identifier);for(d=c.length();d--;)this.a.remove(c.a[d].id);this.b=c;to(this,\"pointercancel\",a);this.b.clear()}if(this.F[a.type]){b=fo(this.o.element);c=a.type;d=a.changedTouches;var e=d.length,f;this.b.clear();for(f=0;f<e;f++){var g=d[f];var h=oo(this.a,g.identifier);var k=g.pageX-b.x;var l=g.pageY-b.y;if(h)if(\"touchmove\"===c){g=Math.abs(h.viewportX-\nk);var m=Math.abs(h.viewportY-l);if(1<g||1<m||1===g&&1===m)jo(h,k,l),this.b.push(h)}else\"touchend\"===c&&(this.a.remove(h.id),this.b.push(h),lo(h,mo.LEFT));else h=new io(k,l,g.identifier,\"touch\",mo.LEFT,1),this.a.push(h),this.b.push(h)}to(this,this.F[a.type],a);-1===this.s.indexOf(a.target)&&a.preventDefault()}};zo.prototype.u=function(){this.s=null;ro.prototype.u.call(this)};function Ao(a){var b=Bo(this);(window.PointerEvent||window.MSPointerEvent)&&b.push({Ta:\"MSHoldVisual\",listener:\"prevent\"});ro.call(this,a,b)}u(Ao,ro);function Bo(a){var b=!!window.PointerEvent,c,d,e=[];a.g=z(a.g,a);\"MSPointerDown MSPointerMove MSPointerUp MSPointerCancel MSPointerOut MSPointerOver\".split(\" \").forEach(function(f){c=f.toLowerCase().replace(/ms/g,\"\");d=b?c:f;e.push({Ta:d,listener:a.g,target:\"MSPointerUp\"===f||\"MSPointerMove\"===f?window:null})});return e}var Co={2:\"touch\",3:\"pen\",4:\"mouse\"};\nAo.prototype.g=function(a){var b=window.PointerEvent?a.type:a.type.toLowerCase().replace(/ms/g,\"\"),c=fo(this.i),d=oo(this.a,a.pointerId),e=a.pageX-c.x;c=a.pageY-c.y;var f=Co[a.pointerType]||a.pointerType;Uc&&\"rtl\"===x.getComputedStyle(this.o.element).direction&&(e-=(x.devicePixelRatio-1)*this.o.width);if(!(d||b in{pointerup:1,pointerout:1,pointercancel:1}||\"touch\"===f&&\"pointerdown\"!==b)){d={x:e,y:c};var g=a.pointerType;\"number\"===typeof g&&(g=Co[g]);d=new io(d.x,d.y,a.pointerId,g,a.button,a.buttons);\nthis.a.push(d)}d&&(b in{pointerup:1,pointercancel:1}?(\"touch\"===f&&this.a.remove(d.id),lo(d,a.button)):\"pointerdown\"===b&&(\"touch\"===a.pointerType&&(po(this.a,\"mouse\"),po(this.a,\"pen\")),ko(d,a.button)),this.b.push(d),\"pointermove\"!==b?(jo(d,e,c),to(this,\"pointerout\"===b||\"pointerover\"===b?\"pointermove\":b,a)):d.viewportX===e&&d.viewportY===c||a.target===document.documentElement||(jo(d,e,c),to(this,b,a)));this.b.clear()};function Do(a,b,c,d){Do.l.constructor.call(this,\"contextmenu\");this.items=[];this.viewportX=a;this.viewportY=b;this.target=c;this.originalEvent=d}u(Do,ld);t(\"H.mapevents.ContextMenuEvent\",Do);function Eo(a){this.vh=z(this.vh,this);this.xh=z(this.xh,this);this.wh=z(this.wh,this);this.s=!1;this.g=-1;this.F=0;Eo.l.constructor.call(this,a,[{Ta:\"contextmenu\",listener:this.vh},{target:a,Ta:\"longpress\",listener:this.xh},{target:a,Ta:\"dbltap\",listener:this.wh}])}u(Eo,ro);n=Eo.prototype;n.xh=function(a){var b=a.currentPointer;\"touch\"===b.type&&1===a.pointers.length&&Fo(this,b.viewportX,b.viewportY,a.originalEvent,a.target)};n.wh=function(a){\"touch\"===a.currentPointer.type&&(this.F=Date.now())};\nn.vh=function(a){var b=this;-1===this.g?this.g=setTimeout(function(){var c=fo(b.i),d=a.pageX-c.x;c=a.pageY-c.y;b.g=-1;Fo(b,d,c,a)},this.Wg):(clearInterval(this.g),this.g=-1);a.preventDefault()};function Fo(a,b,c,d,e){var f=a.map,g=Date.now()-a.F;e?!a.s&&g>a.Jh&&(a.s=!0,e.dispatchEvent(new Do(b,c,e,d)),ue(f.J(),a.Si,a.lj,!1,a)):f.Qd(b,c,a.on.bind(a,b,c,d))}n.on=function(a,b,c,d){d=d&&va(d.dispatchEvent)?d:this.map;Fo(this,a,b,c,d)};n.Si=[\"mousedown\",\"touchstart\",\"pointerdown\",\"wheel\"];\nn.lj=function(){this.s&&(this.s=!1,this.map.dispatchEvent(new ld(\"contextmenuclose\",this.map)))};n.u=function(){var a=this.map.J();clearInterval(this.g);a&&Be(a,this.Si,this.lj,!1,this);ro.prototype.u.call(this)};function Go(a,b,c,d,e){Go.l.constructor.call(this,\"wheel\");this.delta=a;this.viewportX=b;this.viewportY=c;this.target=d;this.originalEvent=e}u(Go,ld);t(\"H.mapevents.WheelEvent\",Go);function Ho(a){var b=\"onwheel\"in document;this.L=b;this.F=(b?\"d\":\"wheelD\")+\"elta\";this.g=z(this.g,this);Ho.l.constructor.call(this,a,[{Ta:(b?\"\":\"mouse\")+\"wheel\",listener:this.g}]);this.s=this.map.Da}u(Ho,ro);\nHo.prototype.g=function(a){if(!a.dl){var b=fo(this.i);var c=a.pageX-b.x;b=a.pageY-b.y;var d=this.F,e=a[d+(d+\"Y\"in a?\"Y\":\"\")],f;Uc&&\"rtl\"===x.getComputedStyle(this.s.element).direction&&(c-=(x.devicePixelRatio-1)*this.s.width);if(e){var g=Math.abs;var h=g(e);e=(!(f=a[d+\"X\"])||3<=h/g(f))&&(!(f=a[d+\"Z\"])||3<=h/g(f))?((0<e)-(0>e))*(this.L?1:-1):0}c=new Go(e,c,b,null,a);c.delta&&(a.stopImmediatePropagation(),a.preventDefault(),this.map.Qd(c.viewportX,c.viewportY,this.I.bind(this,c)))}};\nHo.prototype.I=function(a,b){var c=a.target=b||this.map,d,e;setTimeout(function(){c.dispatchEvent(a);a.f||(d=a.originalEvent,e=new x.WheelEvent(\"wheel\",d),e.dl=1,d.target.dispatchEvent(e))},0)};function Io(a){var b=window;this.g=z(this.g,this);ro.call(this,a,[{Ta:\"mousedown\",listener:this.g},{Ta:\"mousemove\",listener:this.g,target:b},{Ta:\"mouseup\",listener:this.g,target:b},{Ta:\"mouseover\",listener:this.g},{Ta:\"mouseout\",listener:this.g},{Ta:\"dragstart\",listener:this.s}])}u(Io,ro);\nIo.prototype.g=function(a){var b=a.type,c=fo(this.i);c={x:a.pageX-c.x,y:a.pageY-c.y};var d;(d=this.a.a[0])||(d=new io(c.x,c.y,1,\"mouse\"),this.a.push(d));this.b.push(d);jo(d,c.x,c.y);/^mouse(?:move|over|out)$/.test(b)?to(this,\"pointermove\",a):(/^mouse(down|up)$/.test(b)&&(c=a.which-1,\"up\"===go.RegExp.$1?lo(d,c):ko(d,c)),to(this,b.replace(\"mouse\",\"pointer\"),a));this.b.clear()};Io.prototype.s=function(a){a.preventDefault()};function Jo(a){var b=a.Da.element.style;if(-1!==Ko.indexOf(a))throw Error(\"InvalidArgument: map is already in use\");this.a=a;Ko.push(a);b.msTouchAction=b.touchAction=\"none\";Wc||!window.PointerEvent&&!window.MSPointerEvent?(this.c=new zo(this.a),this.b=new Io(this.a)):this.c=new Ao(this.a);this.g=new Ho(this.a);this.f=new Eo(this.a);this.a.Ab(this.C,this);hd.call(this)}u(Jo,hd);t(\"H.mapevents.MapEvents\",Jo);Jo.prototype.c=null;Jo.prototype.b=null;Jo.prototype.g=null;Jo.prototype.f=null;\nvar Ko=[];zc(Ko);Jo.prototype.C=function(){this.a=null;this.c.C();this.g.C();this.f.C();this.b&&this.b.C();Ko.splice(Ko.indexOf(this.a),1);hd.prototype.C.call(this)};Jo.prototype.dispose=Jo.prototype.C;Jo.prototype.Ol=function(){return this.a};Jo.prototype.getAttachedMap=Jo.prototype.Ol;function Lo(a,b){var c;if(-1!==Mo.indexOf(a))throw new D(Lo,0,\"events are already used\");b=b||{};hd.call(this);this.a=c=a.a;this.i=a;Mo.push(a);c.draggable=!0;this.j=b.kinetics||{duration:600,Jd:Tl};this.m=b.modifierKey||\"Alt\";this.enable(b.enabled);this.c=c.Da;this.f=this.c.element;this.g=0;c.addEventListener(\"dragstart\",this.Th,!1,this);c.addEventListener(\"drag\",this.Yj,!1,this);c.addEventListener(\"dragend\",this.Sh,!1,this);c.addEventListener(\"wheel\",this.mk,!1,this);c.addEventListener(\"dbltap\",\nthis.hk,!1,this);c.addEventListener(\"pointermove\",this.Zj,!1,this);te(this.f,\"contextmenu\",this.Xj,!1,this);a.Ab(this.C,this)}u(Lo,hd);t(\"H.mapevents.Behavior\",Lo);var Mo=[];zc(Mo);Lo.prototype.b=0;Lo.DRAGGING=1;Lo.WHEELZOOM=4;Lo.DBLTAPZOOM=8;Lo.FRACTIONALZOOM=16;Lo.Feature={PANNING:1,PINCH_ZOOM:2,WHEEL_ZOOM:4,DBL_TAP_ZOOM:8,FRACTIONAL_ZOOM:16,HEADING:64,TILT:32};function No(a,b){if(a!==+a||a%1||0>a||2147483647<a)throw new D(b,0,\"integer in range [0...0x7FFFFFFF] required\");}\nLo.prototype.disable=function(a){var b=this.b;a!==B?(No(a,this.disable),b^=b&a):b=0;this.c.endInteraction(!0);this.b=b;this.a.draggable=0<(b&1)};Lo.prototype.disable=Lo.prototype.disable;Lo.prototype.enable=function(a){var b=this.b;a!==B?(No(a,this.enable),b|=a&127):b=127;this.b=b;this.a.draggable=0<(b&1)};Lo.prototype.enable=Lo.prototype.enable;Lo.prototype.isEnabled=function(a){No(a,this.isEnabled);return a===(this.b&a)};Lo.prototype.isEnabled=Lo.prototype.isEnabled;\nfunction Oo(a,b,c){var d=\"touch\"===b.currentPointer.type,e=0,f;if(f=!d){f=a.m;var g,h=b.originalEvent;h.getModifierState?g=h.getModifierState(f):g=!!h[f.replace(/^Control$/,\"ctrl\").toLowerCase()+\"Key\"];f=g}f?e|=96:(e|=1,d&&(b=b.pointers,2===b.length&&(e|=66,c?55>ud(b[0].viewportY-b[1].viewportY)&&(e|=32):a.Bh&al.TILT&&(e|=32))));e&=a.b;return(e&32?al.TILT:0)|(e&64?al.HEADING:0)|(e&2?al.ZOOM:0)|(e&1?al.COORD:0)}\nfunction Po(a){var b=a.pointers;a=b[0];b=b[1];a=[a.viewportX,a.viewportY];b&&a.push(b.viewportX,b.viewportY);return a}n=Lo.prototype;n.Bh=0;n.Th=function(a){var b=Oo(this,a,!0);if(this.Bh=b){var c=this.c;a=Po(a);c.startInteraction(b,this.j);c.interaction.apply(c,a);if(this.b&4&&!(this.b&16)&&(b=a[0],c=a[1],this.g)){a=this.a.sb();var d=(0>this.g?td:sd)(a);a!==d&&(this.g=0,Qo(this,a,d,b,c))}}};\nn.Yj=function(a){var b=Oo(this,a,!1);if(b!==this.Bh)\"pointerout\"!==a.originalEvent.type&&\"pointerover\"!==a.originalEvent.type&&(this.Sh(a),this.Th(a));else if(b){b=this.c;var c=Po(a);b.interaction.apply(b,c);a.originalEvent.preventDefault()}};n.Sh=function(a){Oo(this,a,!1)&&this.c.endInteraction(!this.j)};\nfunction Qo(a,b,c,d,e){var f=+c-+b;a=a.a.b;if(isNaN(+b))throw Error(\"start zoom needs to be a number\");if(isNaN(+c))throw Error(\"to zoom needs to be a number\");0!==f&&(a.startControl(null,d,e),a.control(0,0,6,0,0,0),a.endControl(!0,function(g){g.zoom=c}))}n.mk=function(a){if(!a.defaultPrevented&&this.b&4){var b=a.delta;var c=this.a.sb();var d=this.a;var e=d.wc().type;d=this.b&16?c-b:(0>-b?td:sd)(c)-b;if(e===Sm.P2D||e===Sm.WEBGL)Qo(this,c,d,a.viewportX,a.viewportY),this.g=b;a.preventDefault()}};\nn.Zj=function(){};n.hk=function(a){var b=a.currentPointer,c=this.a.sb(),d=a.currentPointer.type,e=this.a.wc().type;(e===Sm.P2D||e===Sm.WEBGL)&&this.b&8&&(a=\"mouse\"===d?0===a.originalEvent.button?-1:1:0<a.pointers.length?1:-1,a=this.b&16?c-a:(0>-a?td:sd)(c)-a,Qo(this,c,a,b.viewportX,b.viewportY))};n.Xj=function(a){return this.b&8?(a.preventDefault(),!1):!0};\nn.C=function(){var a=this.a;a&&(a.draggable=!1,a.removeEventListener(\"dragstart\",this.Th,!1,this),a.removeEventListener(\"drag\",this.Yj,!1,this),a.removeEventListener(\"dragend\",this.Sh,!1,this),a.removeEventListener(\"wheel\",this.mk,!1,this),a.removeEventListener(\"dbltap\",this.hk,!1,this),a.removeEventListener(\"pointermove\",this.Zj,!1,this),this.a=null);this.f&&(this.f.style.msTouchAction=\"\",Be(this.f,\"contextmenu\",this.Xj,!1,this),this.f=null);this.j=this.c=null;Mo.splice(Mo.indexOf(this.i),1);hd.prototype.C.call(this)};\nLo.prototype.dispose=Lo.prototype.C;t(\"H.mapevents.buildInfo\",function(){return wf(\"mapsjs-mapevents\",\"1.11.0\",\"2e64438\")});\n");