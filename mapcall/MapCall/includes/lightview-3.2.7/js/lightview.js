// NOTE: jduncan 2021-04-21/
// I believe the following is an old shim from Google added to make canvas work properly in IE, at least
// according to this page: https://github.com/flot/flot/issues/195
// Copyright 2006 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
document.createElement("canvas").getContext||function(){function C(){}function B(a){this.type_=a,this.r1_=this.y1_=this.x1_=this.r0_=this.y0_=this.x0_=0,this.colors_=[]}function A(a,b,c){!z(b)||(a.m_=b,c&&(a.lineScale_=f(e(b[0][0]*b[1][1]-b[0][1]*b[1][0]))))}function z(a){var b=0;for(;b<3;b++){var c=0;for(;c<2;c++)if(!isFinite(a[b][c])||isNaN(a[b][c]))return!1}return!0}function y(a,b,c,d){a.currentPath_.push({type:"bezierCurveTo",cp1x:b.x,cp1y:b.y,cp2x:c.x,cp2y:c.y,x:d.x,y:d.y}),a.currentX_=d.x,a.currentY_=d.y}function w(a){this.m_=r(),this.mStack_=[],this.aStack_=[],this.currentPath_=[],this.fillStyle=this.strokeStyle="#000",this.lineWidth=1,this.lineJoin="miter",this.lineCap="butt",this.miterLimit=g*1,this.globalAlpha=1,this.canvas=a;var b=a.ownerDocument.createElement("div");b.style.width=a.clientWidth+"px",b.style.height=a.clientHeight+"px",b.style.overflow="hidden",b.style.position="absolute",a.appendChild(b),this.element_=b,this.lineScale_=this.arcScaleY_=this.arcScaleX_=1}function v(a){switch(a){case"butt":return"flat";case"round":return"round";case"square":default:return"square"}}function u(a){var b,c=1;a=String(a);if(a.substring(0,3)=="rgb"){var d=a.indexOf("(",3),e=a.indexOf(")",d+1),f=a.substring(d+1,e).split(",");b="#";var g=0;for(;g<3;g++)b+=o[Number(f[g])];f.length==4&&a.substr(3,1)=="a"&&(c=f[3])}else b=a;return{color:b,alpha:c}}function t(a,b){b.fillStyle=a.fillStyle,b.lineCap=a.lineCap,b.lineJoin=a.lineJoin,b.lineWidth=a.lineWidth,b.miterLimit=a.miterLimit,b.shadowBlur=a.shadowBlur,b.shadowColor=a.shadowColor,b.shadowOffsetX=a.shadowOffsetX,b.shadowOffsetY=a.shadowOffsetY,b.strokeStyle=a.strokeStyle,b.globalAlpha=a.globalAlpha,b.arcScaleX_=a.arcScaleX_,b.arcScaleY_=a.arcScaleY_,b.lineScale_=a.lineScale_}function s(a,b){var c=r(),d=0;for(;d<3;d++){var e=0;for(;e<3;e++){var f=0,g=0;for(;g<3;g++)f+=a[d][g]*b[g][e];c[d][e]=f}}return c}function r(){return[[1,0,0],[0,1,0],[0,0,1]]}function n(a){var b=a.srcElement;b.firstChild&&(b.firstChild.style.width=b.clientWidth+"px",b.firstChild.style.height=b.clientHeight+"px")}function m(a){var b=a.srcElement;switch(a.propertyName){case"width":b.style.width=b.attributes.width.nodeValue+"px",b.getContext().clearRect();break;case"height":b.style.height=b.attributes.height.nodeValue+"px",b.getContext().clearRect()}}function k(a,b){var c=j.call(arguments,2);return function(){return a.apply(b,c.concat(j.call(arguments)))}}function i(){return this.context_||(this.context_=new w(this))}var a=Math,b=a.round,c=a.sin,d=a.cos,e=a.abs,f=a.sqrt,g=10,h=g/2,j=Array.prototype.slice,l={init:function(a){if(/MSIE/.test(navigator.userAgent)&&!window.opera){var b=a||document;b.createElement("canvas"),b.attachEvent("onreadystatechange",k(this.init_,this,b))}},init_:function(a){a.namespaces.g_vml_||a.namespaces.add("g_vml_","urn:schemas-microsoft-com:vml","#default#VML"),a.namespaces.g_o_||a.namespaces.add("g_o_","urn:schemas-microsoft-com:office:office","#default#VML");if(!a.styleSheets.ex_canvas_){var b=a.createStyleSheet();b.owningElement.id="ex_canvas_",b.cssText="canvas{display:inline-block;overflow:hidden;text-align:left;width:300px;height:150px}g_vml_\\:*{behavior:url(#default#VML)}g_o_\\:*{behavior:url(#default#VML)}"}var c=a.getElementsByTagName("canvas"),d=0;for(;d<c.length;d++)this.initElement(c[d])},initElement:function(a){if(!a.getContext){a.getContext=i,a.innerHTML="",a.attachEvent("onpropertychange",m),a.attachEvent("onresize",n);var b=a.attributes;b.width&&b.width.specified?a.style.width=b.width.nodeValue+"px":a.width=a.clientWidth,b.height&&b.height.specified?a.style.height=b.height.nodeValue+"px":a.height=a.clientHeight}return a}};l.init();var o=[],p=0;for(;p<16;p++){var q=0;for(;q<16;q++)o[p*16+q]=p.toString(16)+q.toString(16)}var x=w.prototype;x.clearRect=function(){this.element_.innerHTML=""},x.beginPath=function(){this.currentPath_=[]},x.moveTo=function(a,b){var c=this.getCoords_(a,b);this.currentPath_.push({type:"moveTo",x:c.x,y:c.y}),this.currentX_=c.x,this.currentY_=c.y},x.lineTo=function(a,b){var c=this.getCoords_(a,b);this.currentPath_.push({type:"lineTo",x:c.x,y:c.y}),this.currentX_=c.x,this.currentY_=c.y},x.bezierCurveTo=function(a,b,c,d,e,f){var g=this.getCoords_(e,f),h=this.getCoords_(a,b),i=this.getCoords_(c,d);y(this,h,i,g)},x.quadraticCurveTo=function(a,b,c,d){var e=this.getCoords_(a,b),f=this.getCoords_(c,d),g={x:this.currentX_+.6666666666666666*(e.x-this.currentX_),y:this.currentY_+.6666666666666666*(e.y-this.currentY_)};y(this,g,{x:g.x+(f.x-this.currentX_)/3,y:g.y+(f.y-this.currentY_)/3},f)},x.arc=function(a,b,e,f,i,j){e*=g;var k=j?"at":"wa",l=a+d(f)*e-h,m=b+c(f)*e-h,n=a+d(i)*e-h,o=b+c(i)*e-h;l==n&&!j&&(l+=.125);var p=this.getCoords_(a,b),q=this.getCoords_(l,m),r=this.getCoords_(n,o);this.currentPath_.push({type:k,x:p.x,y:p.y,radius:e,xStart:q.x,yStart:q.y,xEnd:r.x,yEnd:r.y})},x.rect=function(a,b,c,d){this.moveTo(a,b),this.lineTo(a+c,b),this.lineTo(a+c,b+d),this.lineTo(a,b+d),this.closePath()},x.strokeRect=function(a,b,c,d){var e=this.currentPath_;this.beginPath(),this.moveTo(a,b),this.lineTo(a+c,b),this.lineTo(a+c,b+d),this.lineTo(a,b+d),this.closePath(),this.stroke(),this.currentPath_=e},x.fillRect=function(a,b,c,d){var e=this.currentPath_;this.beginPath(),this.moveTo(a,b),this.lineTo(a+c,b),this.lineTo(a+c,b+d),this.lineTo(a,b+d),this.closePath(),this.fill(),this.currentPath_=e},x.createLinearGradient=function(a,b,c,d){var e=new B("gradient");e.x0_=a,e.y0_=b,e.x1_=c,e.y1_=d;return e},x.createRadialGradient=function(a,b,c,d,e,f){var g=new B("gradientradial");g.x0_=a,g.y0_=b,g.r0_=c,g.x1_=d,g.y1_=e,g.r1_=f;return g},x.drawImage=function(c){var d,e,f,h,i,j,k,l,m=c.runtimeStyle.width,n=c.runtimeStyle.height;c.runtimeStyle.width="auto",c.runtimeStyle.height="auto";var o=c.width,p=c.height;c.runtimeStyle.width=m,c.runtimeStyle.height=n;if(arguments.length==3)d=arguments[1],e=arguments[2],i=j=0,k=f=o,l=h=p;else if(arguments.length==5)d=arguments[1],e=arguments[2],f=arguments[3],h=arguments[4],i=j=0,k=o,l=p;else if(arguments.length==9)i=arguments[1],j=arguments[2],k=arguments[3],l=arguments[4],d=arguments[5],e=arguments[6],f=arguments[7],h=arguments[8];else throw Error("Invalid number of arguments");var q=this.getCoords_(d,e),r=[];r.push(" <g_vml_:group",' coordsize="',g*10,",",g*10,'"',' coordorigin="0,0"',' style="width:',10,"px;height:",10,"px;position:absolute;");if(this.m_[0][0]!=1||this.m_[0][1]){var s=[];s.push("M11=",this.m_[0][0],",","M12=",this.m_[1][0],",","M21=",this.m_[0][1],",","M22=",this.m_[1][1],",","Dx=",b(q.x/g),",","Dy=",b(q.y/g),"");var t=q,u=this.getCoords_(d+f,e),v=this.getCoords_(d,e+h),w=this.getCoords_(d+f,e+h);t.x=a.max(t.x,u.x,v.x,w.x),t.y=a.max(t.y,u.y,v.y,w.y),r.push("padding:0 ",b(t.x/g),"px ",b(t.y/g),"px 0;filter:progid:DXImageTransform.Microsoft.Matrix(",s.join(""),", sizingmethod='clip');")}else r.push("top:",b(q.y/g),"px;left:",b(q.x/g),"px;");r.push(' ">','<g_vml_:image src="',c.src,'"',' style="width:',g*f,"px;"," height:",g*h,'px;"',' cropleft="',i/o,'"',' croptop="',j/p,'"',' cropright="',(o-i-k)/o,'"',' cropbottom="',(p-j-l)/p,'"'," />","</g_vml_:group>"),this.element_.insertAdjacentHTML("BeforeEnd",r.join(""))},x.stroke=function(c){var d=[],e=u(c?this.fillStyle:this.strokeStyle),f=e.color,h=e.alpha*this.globalAlpha;d.push("<g_vml_:shape",' filled="',!!c,'"',' style="position:absolute;width:',10,"px;height:",10,'px;"',' coordorigin="0 0" coordsize="',g*10," ",g*10,'"',' stroked="',!c,'"',' path="');var i={x:null,y:null},j={x:null,y:null},k=0;for(;k<this.currentPath_.length;k++){var l=this.currentPath_[k];switch(l.type){case"moveTo":d.push(" m ",b(l.x),",",b(l.y));break;case"lineTo":d.push(" l ",b(l.x),",",b(l.y));break;case"close":d.push(" x "),l=null;break;case"bezierCurveTo":d.push(" c ",b(l.cp1x),",",b(l.cp1y),",",b(l.cp2x),",",b(l.cp2y),",",b(l.x),",",b(l.y));break;case"at":case"wa":d.push(" ",l.type," ",b(l.x-this.arcScaleX_*l.radius),",",b(l.y-this.arcScaleY_*l.radius)," ",b(l.x+this.arcScaleX_*l.radius),",",b(l.y+this.arcScaleY_*l.radius)," ",b(l.xStart),",",b(l.yStart)," ",b(l.xEnd),",",b(l.yEnd))}if(l){if(i.x==null||l.x<i.x)i.x=l.x;if(j.x==null||l.x>j.x)j.x=l.x;if(i.y==null||l.y<i.y)i.y=l.y;if(j.y==null||l.y>j.y)j.y=l.y}}d.push(' ">');if(c)if(typeof this.fillStyle=="object"){var m=this.fillStyle,n=0,o={x:0,y:0},p=0,q=1;if(m.type_=="gradient"){var r=m.x1_/this.arcScaleX_,s=m.y1_/this.arcScaleY_,t=this.getCoords_(m.x0_/this.arcScaleX_,m.y0_/this.arcScaleY_),w=this.getCoords_(r,s);n=Math.atan2(w.x-t.x,w.y-t.y)*180/Math.PI,n<0&&(n+=360),n<1e-6&&(n=0)}else{var t=this.getCoords_(m.x0_,m.y0_),x=j.x-i.x,y=j.y-i.y;o={x:(t.x-i.x)/x,y:(t.y-i.y)/y},x/=this.arcScaleX_*g,y/=this.arcScaleY_*g;var z=a.max(x,y);p=2*m.r0_/z,q=2*m.r1_/z-p}var A=m.colors_;A.sort(function(a,b){return a.offset-b.offset});var B=A.length,C=A[0].color,D=A[B-1].color,E=A[0].alpha*this.globalAlpha,F=A[B-1].alpha*this.globalAlpha,G=[],k=0;for(;k<B;k++){var H=A[k];G.push(H.offset*q+p+" "+H.color)}d.push('<g_vml_:fill type="',m.type_,'"',' method="none" focus="100%"',' color="',C,'"',' color2="',D,'"',' colors="',G.join(","),'"',' opacity="',F,'"',' g_o_:opacity2="',E,'"',' angle="',n,'"',' focusposition="',o.x,",",o.y,'" />')}else d.push('<g_vml_:fill color="',f,'" opacity="',h,'" />');else{var I=this.lineScale_*this.lineWidth;I<1&&(h*=I),d.push("<g_vml_:stroke",' opacity="',h,'"',' joinstyle="',this.lineJoin,'"',' miterlimit="',this.miterLimit,'"',' endcap="',v(this.lineCap),'"',' weight="',I,'px"',' color="',f,'" />')}d.push("</g_vml_:shape>"),this.element_.insertAdjacentHTML("beforeEnd",d.join(""))},x.fill=function(){this.stroke(!0)},x.closePath=function(){this.currentPath_.push({type:"close"})},x.getCoords_=function(a,b){var c=this.m_;return{x:g*(a*c[0][0]+b*c[1][0]+c[2][0])-h,y:g*(a*c[0][1]+b*c[1][1]+c[2][1])-h}},x.save=function(){var a={};t(this,a),this.aStack_.push(a),this.mStack_.push(this.m_),this.m_=s(r(),this.m_)},x.restore=function(){t(this.aStack_.pop(),this),this.m_=this.mStack_.pop()},x.translate=function(a,b){A(this,s([[1,0,0],[0,1,0],[a,b,1]],this.m_),!1)},x.rotate=function(a){var b=d(a),e=c(a);A(this,s([[b,e,0],[-e,b,0],[0,0,1]],this.m_),!1)},x.scale=function(a,b){this.arcScaleX_*=a,this.arcScaleY_*=b,A(this,s([[a,0,0],[0,b,0],[0,0,1]],this.m_),!0)},x.transform=function(a,b,c,d,e,f){A(this,s([[a,b,0],[c,d,0],[e,f,1]],this.m_),!0)},x.setTransform=function(a,b,c,d,e,f){A(this,[[a,b,0],[c,d,0],[e,f,1]],!0)},x.clip=function(){},x.arcTo=function(){},x.createPattern=function(){return new C},B.prototype.addColorStop=function(a,b){b=u(b),this.colors_.push({offset:a,color:b.color,alpha:b.alpha})},G_vmlCanvasManager=l,CanvasRenderingContext2D=w,CanvasGradient=B,CanvasPattern=C}();


/*!
 * Spinners 3.0.0
 * (c) 2010-2012 Nick Stakenburg - http://www.nickstakenburg.com
 *
 * Spinners is freely distributable under the terms of an MIT-style license.
 *
 * GitHub: http://github.com/staaky/spinners
 */
;var Spinners={version:"3.0.0"};(function(a){function b(a){return a*Math.PI/180}function c(a){this.element=a}function d(b,c){b&&(this.element=b,h.remove(b),h.removeDetached(),this._position=0,this._state="stopped",this.setOptions(a.extend({color:"#000",dashes:12,radius:5,height:5,width:1.8,opacity:1,padding:3,rotation:700},c||{})),this.drawPosition(0),h.add(this))}var e={scroll:function(a,b){if(!b)return a;var c=a.slice(0,b);return a.slice(b,a.length).concat(c)},isElement:function(a){return a&&1==a.nodeType},element:{isAttached:function(){return function(a){for(;a&&a.parentNode;)a=a.parentNode;return!!a&&!!a.body}}()}},f={drawRoundedRectangle:function(c,d){var e=a.extend({top:0,left:0,width:0,height:0,radius:0},d||{}),f=e.left,g=e.top,h=e.width,i=e.height,e=e.radius;c.beginPath(),c.moveTo(f+e,g),c.arc(f+h-e,g+e,e,b(-90),b(0),!1),c.arc(f+h-e,g+i-e,e,b(0),b(90),!1),c.arc(f+e,g+i-e,e,b(90),b(180),!1),c.arc(f+e,g+e,e,b(-180),b(-90),!1),c.closePath(),c.fill()}},g=function(){function a(a){var c=[];0==a.indexOf("#")&&(a=a.substring(1)),a=a.toLowerCase();if(""!=a.replace(b,""))return null;3==a.length?(c[0]=a.charAt(0)+a.charAt(0),c[1]=a.charAt(1)+a.charAt(1),c[2]=a.charAt(2)+a.charAt(2)):(c[0]=a.substring(0,2),c[1]=a.substring(2,4),c[2]=a.substring(4));for(a=0;a<c.length;a++)c[a]=parseInt(c[a],16);return c.red=c[0],c.green=c[1],c.blue=c[2],c}var b=RegExp("[0123456789abcdef]","g"),c=function(){function a(a,b,c){return a=a.toString(c||10),Array(b-a.length).join("0")+a}return function(b,c,d){return"#"+a(b,2,16)+a(c,2,16)+a(d,2,16)}}();return{hex2rgb:a,hex2fill:function(b,c){"undefined"==typeof c&&(c=1);var d=c,e=a(b);return e[3]=d,e.opacity=d,"rgba("+e.join()+")"},rgb2hex:c}}();a.extend(Spinners,{enabled:!1,support:{canvas:function(){var b=a("<canvas>")[0];return!!b.getContext&&!!b.getContext("2d")}()},init:function(){if(this.support.canvas||window.G_vmlCanvasManager&&window.attachEvent&&-1===navigator.userAgent.indexOf("Opera"))window.G_vmlCanvasManager&&window.G_vmlCanvasManager.init_(document),this.enabled=!0},create:function(a,b){return c.create(a,b),this.get(a)},get:function(a){return new c(a)},play:function(a){return this.get(a).play(),this},pause:function(a){return this.get(a).pause(),this},toggle:function(a){return this.get(a).toggle(),this},stop:function(a){return this.get(a).stop(),this},remove:function(a){return this.get(a).remove(),this},removeDetached:function(){return h.removeDetached(),this},center:function(a){return this.get(a).center(),this},setOptions:function(a,b){return this.get(a).setOptions(b),this},getDimensions:function(a){return a=2*h.get(a)[0].getLayout().workspace.radius,{width:a,height:a}}});var h={spinners:[],get:function(b){if(b){var c=[];return a.each(this.spinners,function(d,f){f&&(e.isElement(b)?f.element==b:a(f.element).is(b))&&c.push(f)}),c}},add:function(a){this.spinners.push(a)},remove:function(b){a(a.map(this.spinners,function(c){if(e.isElement(b)?c.element==b:a(c.element).is(b))return c.element})).each(a.proxy(function(a,b){this.removeByElement(b)},this))},removeByElement:function(b){var c=this.get(b)[0];c&&(c.remove(),this.spinners=a.grep(this.spinners,function(a){return a.element!=b}))},removeDetached:function(){a.each(this.spinners,a.proxy(function(a,b){b&&b.element&&!e.element.isAttached(b.element)&&this.remove(b.element)},this))}};a.extend(c,{create:function(b,c){if(b){var f=c||{},g=[];return e.isElement(b)?g.push(new d(b,f)):a(b).each(function(a,b){g.push(new d(b,f))}),g}}}),a.extend(c.prototype,{items:function(){return h.get(this.element)},play:function(){return a.each(this.items(),function(a,b){b.play()}),this},stop:function(){return a.each(this.items(),function(a,b){b.stop()}),this},pause:function(){return a.each(this.items(),function(a,b){b.pause()}),this},toggle:function(){return a.each(this.items(),function(a,b){b.toggle()}),this},center:function(){return a.each(this.items(),function(a,b){b.center()}),this},setOptions:function(b){return a.each(this.items(),function(a,c){c.setOptions(b)}),this},remove:function(){return h.remove(this.element),this}}),a.extend(d.prototype,{setOptions:function(b){this.options=a.extend({},this.options,b||{}),this.options.radii&&(b=this.options.radii,this.options.radius=Math.min(b[0],b[1]),this.options.height=Math.max(b[0],b[1])-this.options.radius),this.options.dashWidth&&(this.options.width=this.options.dashWidth),this.options.speed&&(this.options.duration=1e3*this.options.speed);var b=this._state,c=this._position;this._layout=null,this.build(),c&&c>=this.options.dashes-1&&(this._position=this.options.dashes-1);switch(b){case"playing":this.play();break;case"paused":case"stopped":this.drawPosition(this._position)}this._centered&&this.center()},remove:function(){this.canvas&&(this.pause(),a(this.canvas).remove(),this.ctx=this.canvas=null)},build:function(){this.remove();var b=this.getLayout().workspace.radius;return a(document.body).append(this.canvas=a("<canvas>").attr({width:2*b,height:2*b}).css({zoom:1})),window.G_vmlCanvasManager&&G_vmlCanvasManager.initElement(this.canvas[0]),this.ctx=this.canvas[0].getContext("2d"),this.ctx.globalAlpha=this.options.opacity,a(this.element).append(this.canvas),this.ctx.translate(b,b),this},drawPosition:function(a){var c=this.getLayout().workspace,a=e.scroll(c.opacities,-1*a),d=c.radius,c=this.options.dashes,f=b(360/c);this.ctx.clearRect(-1*d,-1*d,2*d,2*d);for(d=0;d<c;d++)this.drawDash(a[d],this.options.color),this.ctx.rotate(f)},drawDash:function(a,b){this.ctx.fillStyle=g.hex2fill(b,a);var c=this.getLayout(),d=c.workspace.radius,e=c.dash.position,c=c.dash.dimensions;f.drawRoundedRectangle(this.ctx,{top:e.top-d,left:e.left-d,width:c.width,height:c.height,radius:Math.min(c.height,c.width)/2})},_nextPosition:function(){var b=this.options.rotation/this.options.dashes;this.nextPosition(),this._playTimer=window.setTimeout(a.proxy(this._nextPosition,this),b)},nextPosition:function(){this._position==this.options.dashes-1&&(this._position=-1),this._position++,this.drawPosition(this._position)},play:function(){if("playing"!=this._state){this._state="playing";var b=this.options.rotation/this.options.dashes;return this._playTimer=window.setTimeout(a.proxy(this._nextPosition,this),b),this}},pause:function(){if("paused"!=this._state)return this._pause(),this._state="paused",this},_pause:function(){this._playTimer&&(window.clearTimeout(this._playTimer),this._playTimer=null)},stop:function(){if("stopped"!=this._state)return this._pause(),this._position=0,this.drawPosition(0),this._state="stopped",this},toggle:function(){return this["playing"==this._state?"pause":"play"](),this},getLayout:function(){if(this._layout)return this._layout;for(var a=this.options,b=a.dashes,c=a.width,d=a.radius,e=a.radius+a.height,f=Math.max(c,e),f=Math.ceil(Math.max(f,Math.sqrt(e*e+c/2*(c/2)))),a=f+=a.padding,g=1/b,h=[],i=0;i<b;i++)h.push((i+1)*g);return this._layout=b={workspace:{radius:a,opacities:h},dash:{position:{top:f-e,left:f-c/2},dimensions:{width:c,height:e-d}}}},center:function(){var b=2*this.getLayout().workspace.radius;a(this.element.parentNode).css({position:"relative"}),a(this.element).css({position:"absolute",height:b+"px",width:b+"px",top:"50%",left:"50%",marginLeft:-0.5*b+"px",marginTop:-0.5*b+"px"}),this._centered=!0}}),Spinners.init(),Spinners.enabled||(c.create=function(){return[]})})(jQuery);


/*!
 * Lightview - The jQuery Lightbox - v3.2.7
 * (c) 2008-2012 Nick Stakenburg
 *
 * http://projects.nickstakenburg.com/lightview
 *
 * License: http://projects.nickstakenburg.com/lightview/license
 */
;var Lightview = {
  version: '3.2.7',

  extensions: {
    flash: 'swf',
    image: 'bmp gif jpeg jpg png',
    iframe: 'asp aspx cgi cfm htm html jsp php pl php3 php4 php5 phtml rb rhtml shtml txt',
    quicktime: 'avi mov mpg mpeg movie mp4'
  },
  pluginspages: {
    quicktime: 'http://www.apple.com/quicktime/download',
    flash: 'http://www.adobe.com/go/getflashplayer'
  }
};

Lightview.Skins = {
  // every possible property is defined on the base skin 
  // all other skins inherit from this skin
  'base': {
    ajax: {
      type: 'get'
    },
    background: {
      color: '#fff',
      opacity: 1
    },
    border: {
      size: 0,
      color: '#ccc',
      opacity: 1
    },
    continuous: false,
    controls: {
      close: 'relative',
      slider: {
        items: 5
      },
      text: {
        previous: "Prev", // when modifying this on skins images/css might have to be changed
        next:     "Next"
      },
      thumbnails: {
        spinner: { color: '#777' },
        mousewheel: true
      },
      type: 'relative'
    },
    effects: {
      caption: { show: 180, hide: 180 },
      content: { show: 280, hide: 280 },
      overlay: { show: 240, hide: 280 },
      sides:   { show: 150, hide: 180 },
      spinner: { show: 50,  hide: 100 },
      slider:  { slide: 180 },
      thumbnails: { show: 120, hide: 0, slide: 180, load: 340 },
      window:  { show: 120, hide: 50, resize: 200, position: 180 }
    },
    errors: {
      'missing_plugin': "The content your are attempting to view requires the <a href='#{pluginspage}' target='_blank'>#{type} plugin<\/a>."
    },
    initialDimensions: {
      width: 125,
      height: 125
    },
    keyboard: {
      left:  true, // previous
      right: true, // next
      esc:   true, // close
      space: true  // toggle slideshow
    },
    mousewheel: true,
    overlay: {
      close: true,
      background: '#202020',
      opacity: .85
    },
    padding: 10,
    position: {
      at: 'center',
      offset: { x: 0, y: 0 }
    },
    preload: true,
    radius: {
      size: 0,
      position: 'background'
    },
    shadow: {
      blur: 3,
      color: '#000',
      opacity: .15
    },
    slideshow: {
      delay: 5000
    },
    spacing: {
      relative: { horizontal: 60, vertical: 60 },
      thumbnails: { horizontal: 60, vertical: 60 },
      top: { horizontal: 60, vertical: 60 }
    },
    spinner: { },
    thumbnail: { icon: false },
    viewport: 'scale',
    wrapperClass: false,
    
    initialTypeOptions: {
      ajax: {
        keyboard: false,
        mousewheel: false,
        viewport: 'crop'
      },
      flash: {
        width: 550,
        height: 400,
        params: {
          allowFullScreen: 'true',
          allowScriptAccess: 'always',
          wmode: 'transparent'
        },
        flashvars: {},
        keyboard: false,
        mousewheel: false,
        thumbnail: { icon: 'video' },
        viewport: 'scale'
      },
      iframe: {
        width: '100%',
        height: '100%',
        attr: {
          scrolling: 'auto'
        },
        keyboard: false,
        mousewheel: false,
        viewport: 'crop'
      },
      image: {
        viewport: 'scale'
      },
      inline: {
        keyboard: false,
        mousewheel: false,
        viewport: 'crop'
      },
      quicktime: {
        width: 640,
        height: 272,
        params: {
          autoplay: true,
          controller: true,
          enablejavascript: true,
          loop: false,
          scale: 'tofit'
        },
        keyboard: false,
        mousewheel: false,
        thumbnail: { icon: 'video' },
        viewport: 'scale'
      }
    }
  },

  // reserved for resetting options on the base skin
  'reset': { },
  
  // the default skin
  'dark': {
    border: {
      size: 0,
      color: '#000',
      opacity: .25
    },
    radius: { size: 5 },
    background: '#141414',
    shadow: {
      blur: 5,
      opacity: .08
    },
    overlay: {
      background: '#2b2b2b',
      opacity: .85
    },
    spinner: {
      color: '#777'
    }
  },

  'light': {
    border: { opacity: .25 },
    radius: { size: 5 },
    spinner: {
      color: '#333'
    }
  },
  
  'mac': {
    background: '#fff',
    border: {
      size: 0,
      color: '#dfdfdf',
      opacity: .3
    },
    shadow: {
      blur: 3,
      opacity: .08
    },
    overlay: {
      background: '#2b2b2b',
      opacity: .85
    }
  }
};

(function ($, A) {
    function px(a) {
        var b = {};
        for (var c in a) {
            b[c] = a[c] + "px"
        }
        return b
    }
    function pyth(a, b) {
        return Math.sqrt(a * a + b * b)
    }
    function degrees(a) {
        return 180 * a / Math.PI
    }
    function radian(a) {
        return a * Math.PI / 180
    }
    function sfcc(a) {
        return String.fromCharCode.apply(String, a.split(","))
    }
    function warn(a) {
        A.console && console[console.warn ? "warn" : "log"](a)
    }
    function createHTML(a) {
        var b = "<" + a.tag;
        for (var c in a) {
            0 > $.inArray(c, "children html tag".split(" ")) && (b += " " + c + '="' + a[c] + '"')
        }
        return RegExp("^(?:area|base|basefont|br|col|frame|hr|img|input|link|isindex|meta|param|range|spacer|wbr)$", "i").test(a.tag) ? b += "/>" : (b += ">", a.children && $.each(a.children, function (a, c) {
            b += createHTML(c)
        }), a.html && (b += a.html), b += "</" + a.tag + ">"), b
    }
    function deepExtend(a, b) {
        for (var c in b) {
            b[c] && b[c].constructor && b[c].constructor === Object ? (a[c] = _.clone(a[c]) || {}, deepExtend(a[c], b[c])) : a[c] = b[c]
        }
        return a
    }
    function deepExtendClone(a, b) {
        return deepExtend(_.clone(a), b)
    }
    function View() {
        this.initialize.apply(this, arguments)
    }
    function detectType(a, b) {
        var c, d = (b || detectExtension(a) || "").toLowerCase();
        return $("flash image iframe quicktime".split(" ")).each(function (a, b) {
            $.inArray(d, Lightview.extensions[b].split(" ")) > -1 && (c = b)
        }), c ? c : "#" == a.substr(0, 1) ? "inline" : document.domain && document.domain != a.replace(/(^.*\/\/)|(:.*)|(\/.*)/g, "") ? "iframe" : "image"
    }
    function detectExtension(a) {
        var b = (a || "").replace(/\?.*/g, "").match(/\.([^.]{3,4})$/);
        return b ? b[1] : null
    }
    function deferUntil(a, b) {
        var c = $.extend({
            lifetime: 300000,
            iteration: 10,
            fail: null
        }, arguments[2] || {}),
            d = 0;
        return a._interval = A.setInterval($.proxy(function () {
            return d += c.iteration, b() ? (A.clearInterval(a._interval), a(), void 0) : (d >= c.lifetime && (A.clearInterval(a._interval), c.fail && c.fail()), void 0)
        }, a), c.iteration), a._interval
    }(function () {
        function a(a) {
            var b;
            if (a.originalEvent.wheelDelta ? b = a.originalEvent.wheelDelta / 120 : a.originalEvent.detail && (b = -a.originalEvent.detail / 3), b) {
                var c = $.Event("lightview:mousewheel");
                $(a.target).trigger(c, b), c.isPropagationStopped() && a.stopPropagation(), c.isDefaultPrevented() && a.preventDefault()
            }
        }
        $(document.documentElement).bind("mousewheel DOMMouseScroll", a)
    })();
    var B = {};
    (function () {
        var a = {};
        $.extend(a, {
            Quart: function (a) {
                return Math.pow(a, 4)
            }
        }), $.each(a, function (a, b) {
            B["easeIn" + a] = b, B["easeOut" + a] = function (a) {
                return 1 - b(1 - a)
            }, B["easeInOut" + a] = function (a) {
                return 0.5 > a ? b(2 * a) / 2 : 1 - b(-2 * a + 2) / 2
            }
        }), $.each(B, function (a, b) {
            $.easing[a] || ($.easing[a] = b)
        })
    })();
    var C = Array.prototype.slice,
        _ = {
            clone: function (a) {
                return $.extend({}, a)
            },
            isElement: function (a) {
                return a && 1 == a.nodeType
            },
            element: {
                isAttached: function () {
                    function a(a) {
                        for (var b = a; b && b.parentNode;) {
                            b = b.parentNode
                        }
                        return b
                    }
                    return function (b) {
                        var c = a(b);
                        return !(!c || !c.body)
                    }
                }()
            }
        }, Browser = function (a) {
            function b(b) {
                var c = RegExp(b + "([\\d.]+)").exec(a);
                return c ? parseFloat(c[1]) : !0
            }
            return {
                IE: !(!A.attachEvent || -1 !== a.indexOf("Opera")) && b("MSIE "),
                Opera: a.indexOf("Opera") > -1 && ( !! A.opera && opera.version && parseFloat(opera.version()) || 7.55),
                WebKit: a.indexOf("AppleWebKit/") > -1 && b("AppleWebKit/"),
                Gecko: a.indexOf("Gecko") > -1 && -1 === a.indexOf("KHTML") && b("rv:"),
                MobileSafari: !! a.match(/Apple.*Mobile.*Safari/),
                Chrome: a.indexOf("Chrome") > -1 && b("Chrome/")
            }
        }(navigator.userAgent),
        getUniqueID = function () {
            var a = 0,
                b = "lv_identity_";
            return function (c) {
                for (c = c || b, a++; document.getElementById(c + a);) {
                    a++
                }
                return c + a
            }
        }(),
        Requirements = {
            scripts: {
                jQuery: {
                    required: "1.4.4",
                    available: A.jQuery && jQuery.fn.jquery
                },
                SWFObject: {
                    required: "2.2",
                    available: A.swfobject && swfobject.ua && "2.2"
                },
                Spinners: {
                    required: "3.0.0",
                    available: A.Spinners && (Spinners.version || Spinners.Version)
                }
            },
            check: function () {
                function b(b) {
                    for (var c = b.match(a), d = c && c[1] && c[1].split(".") || [], e = 0, f = 0, g = d.length; g > f; f++) {
                        e += parseInt(d[f] * Math.pow(10, 6 - 2 * f))
                    }
                    return c && c[3] ? e - 1 : e
                }
                var a = /^(\d+(\.?\d+){0,3})([A-Za-z_-]+[A-Za-z0-9]+)?/;
                return function (a) {
                    (!this.scripts[a].available || b(this.scripts[a].available) < b(this.scripts[a].required) && !this.scripts[a].notified) && (this.scripts[a].notified = !0, warn("Lightview requires " + a + " >= " + this.scripts[a].required))
                }
            }()
        };
    (function () {
        $(document).ready(function () {
            function b(b) {
                var c = !1;
                if (a) {
                    c = $.map(C.call(navigator.plugins), function (a) {
                        return a.name
                    }).join(",").indexOf(b) >= 0
                } else {
                    try {
                        c = new ActiveXObject(b)
                    } catch (d) {}
                }
                return !!c
            }
            var a = navigator.plugins && navigator.plugins.length;
            Lightview.plugins = a ? {
                flash: b("Shockwave Flash"),
                quicktime: b("QuickTime")
            } : {
                flash: b("ShockwaveFlash.ShockwaveFlash"),
                quicktime: b("QuickTime.QuickTime")
            }
        })
    })(), $.extend(!0, Lightview, function () {
        function c(a) {
            return e(a, "prefix")
        }
        function d(b, c) {
            for (var d in b) {
                if (void 0 !== a.style[b[d]]) {
                    return "prefix" == c ? b[d] : !0
                }
            }
            return !1
        }
        function e(a, c) {
            var e = a.charAt(0).toUpperCase() + a.substr(1),
                f = (a + " " + b.join(e + " ") + e).split(" ");
            return d(f, c)
        }
        function g() {
            Requirements.check("jQuery"), (this.support.canvas || Browser.IE) && (A.G_vmlCanvasManager && A.G_vmlCanvasManager.init_(document), Overlay.init(), Window.init(), Window.center(), F.init())
        }
        var a = document.createElement("div"),
            b = "Webkit Moz O ms Khtml".split(" "),
            f = {
                canvas: function () {
                    var a = document.createElement("canvas");
                    return !(!a.getContext || !a.getContext("2d"))
                }(),
                touch: function () {
                    try {
                        return !!document.createEvent("TouchEvent")
                    } catch (a) {
                        return !1
                    }
                }(),
                css: {
                    boxShadow: e("boxShadow"),
                    borderRadius: e("borderRadius"),
                    transitions: function () {
                        var a = ["WebKitTransitionEvent", "TransitionEvent", "OTransitionEvent"],
                            b = !1;
                        return $.each(a, function (a, c) {
                            try {
                                document.createEvent(c), b = !0
                            } catch (d) {}
                        }), b
                    }(),
                    expressions: Browser.IE && 7 > Browser.IE,
                    prefixed: c
                }
            };
        return {
            init: g,
            support: f
        }
    }());
    var D = function () {
        function c(c, d) {
            c = c || {}, c.skin = c.skin || (Lightview.Skins[Window.defaultSkin] ? Window.defaultSkin : "lightview");
            var e = c.skin ? _.clone(Lightview.Skins[c.skin] || Lightview.Skins[Window.defaultSkin]) : {}, f = deepExtendClone(b, e);
            d && (f = deepExtend(f, f.initialTypeOptions[d]));
            var g = deepExtendClone(f, c);
            if (g.ajax) {
                if ("boolean" == $.type(g.ajax)) {
                    var h = b.ajax || {}, i = a.ajax;
                    g.ajax = {
                        cache: h.cache || i.cache,
                        type: h.type || i.type
                    }
                }
                g.ajax = deepExtendClone(i, g.ajax)
            }
            if (g.controls && (g.controls = "string" == $.type(g.controls) ? deepExtendClone(f.controls || b.controls || a.controls, {
                type: g.controls
            }) : deepExtendClone(a.controls, g.controls)), "string" == $.type(g.background) ? g.background = {
                color: g.background,
                opacity: 1
            } : g.background && (g.background = {
                opacity: g.background.opacity || 1,
                color: g.background.color || "#000"
            }), g.effects || (g.effects = {}, $.each(a.effects, function (a, b) {
                $.each(g.effects[a] = $.extend({}, b), function (b) {
                    g.effects[a][b] = 0
                })
            })), Browser.MobileSafari) {
                var j = g.effects.overlay;
                j.show = 0, j.hide = 0
            }
            if (g.effects && !Lightview.support.canvas && Browser.IE && 9 > Browser.IE) {
                var k = g.effects;
                7 > Browser.IE && $.extend(!0, k, {
                    caption: {
                        show: 0,
                        hide: 0
                    },
                    window: {
                        show: 0,
                        hide: 0,
                        resize: 0
                    },
                    content: {
                        show: 0,
                        hide: 0
                    },
                    spinner: {
                        show: 0,
                        hide: 0
                    },
                    slider: {
                        slide: 0
                    }
                }), $.extend(!0, k, {
                    sides: {
                        show: 0,
                        hide: 0
                    }
                })
            }
            if (g.border) {
                var l, m = b.border || {}, n = a.border;
                l = "number" == $.type(g.border) ? {
                    size: g.border,
                    color: m.color || n.color,
                    opacity: m.opacity || n.opacity
                } : "string" == $.type(g.border) ? {
                    size: m.size || n.size,
                    color: g.border,
                    opacity: m.opacity || n.opacity
                } : deepExtendClone(n, g.border), g.border = 0 === l.size ? !1 : l
            }
            var o = a.position;
            if (g.position || "number" == $.type(g.position)) {
                var p, q = b.position || {};
                p = "string" == $.type(g.position) ? {
                    at: g.position,
                    offset: q.offset || o.offset
                } : "number" == $.type(g.position) ? {
                    at: "top",
                    offset: {
                        x: 0,
                        y: g.position
                    }
                } : deepExtendClone(o, g.position), g.position = p
            } else {
                g.position = _.clone(o)
            }
            if (g.radius || "number" == $.type(g.radius)) {
                var r, s = b.radius || {}, t = a.radius;
                r = "number" == $.type(g.radius) ? {
                    size: g.radius,
                    position: s.position || t.position
                } : "string" == $.type(g.radius) ? {
                    size: s.size || t.size,
                    position: g.position
                } : deepExtendClone(t, g.radius), g.radius = r
            }
            if (g.shadow) {
                var u, v = b.shadow,
                    w = a.shadow;
                u = "boolean" == $.type(g.shadow) ? v && "shadow" == $.type(v) ? w : v ? v : w : deepExtendClone(w, g.shadow || {}), 1 > u.blur && (u = !1), g.shadow = u
            }
            if (g.thumbnail) {
                var x, y = b.thumbnail || {}, z = a.thumbnail;
                x = "string" == $.type(g.thumbnail) ? {
                    image: g.thumbnail,
                    icon: f.thumbnail && f.thumbnail.icon || y.icon || z.icon
                } : deepExtendClone(z, g.thumbnail), g.thumbnail = x
            }
            return g.slideshow && "number" == $.type(g.slideshow) && (g.slideshow = {
                delay: g.slideshow
            }), g
        }
        var a = Lightview.Skins.base,
            b = deepExtendClone(a, Lightview.Skins.reset);
        return {
            create: c
        }
    }(),
        Color = function () {
            function c(a) {
                var b = a;
                return b.red = a[0], b.green = a[1], b.blue = a[2], b
            }
            function d(a) {
                return parseInt(a, 16)
            }
            function e(a) {
                var e = Array(3);
                if (0 == a.indexOf("#") && (a = a.substring(1)), a = a.toLowerCase(), "" != a.replace(b, "")) {
                    return null
                }
                3 == a.length ? (e[0] = a.charAt(0) + a.charAt(0), e[1] = a.charAt(1) + a.charAt(1), e[2] = a.charAt(2) + a.charAt(2)) : (e[0] = a.substring(0, 2), e[1] = a.substring(2, 4), e[2] = a.substring(4));
                for (var f = 0; e.length > f; f++) {
                    e[f] = d(e[f])
                }
                return c(e)
            }
            function f(a, b) {
                var c = e(a);
                return c[3] = b, c.opacity = b, c
            }
            function g(a, b) {
                return "undefined" == $.type(b) && (b = 1), "rgba(" + f(a, b).join() + ")"
            }
            function h(a) {
                return "#" + (i(a)[2] > 50 ? "000" : "fff")
            }
            function i(a) {
                return j(e(a))
            }
            function j(a) {
                var f, g, h, a = c(a),
                    b = a.red,
                    d = a.green,
                    e = a.blue,
                    i = b > d ? b : d;
                e > i && (i = e);
                var j = d > b ? b : d;
                if (j > e && (j = e), h = i / 255, g = 0 != i ? (i - j) / i : 0, 0 == g) {
                    f = 0
                } else {
                    var k = (i - b) / (i - j),
                        l = (i - d) / (i - j),
                        m = (i - e) / (i - j);
                    f = b == i ? m - l : d == i ? 2 + k - m : 4 + l - k, f /= 6, 0 > f && (f += 1)
                }
                f = Math.round(360 * f), g = Math.round(100 * g), h = Math.round(100 * h);
                var n = [];
                return n[0] = f, n[1] = g, n[2] = h, n.hue = f, n.saturation = g, n.brightness = h, n
            }
            var a = "0123456789abcdef",
                b = RegExp("[" + a + "]", "g");
            return {
                hex2rgb: e,
                hex2fill: g,
                getSaturatedBW: h
            }
        }(),
        Canvas = {
            init: function () {
                return A.G_vmlCanvasManager && !Lightview.support.canvas && Browser.IE ? function (a) {
                    G_vmlCanvasManager.initElement(a)
                } : function () {}
            }(),
            resize: function (a, b) {
                $(a).attr({
                    width: b.width * this.devicePixelRatio,
                    height: b.height * this.devicePixelRatio
                }).css(px(b))
            },
            drawRoundedRectangle: function (a) {
                var b = $.extend(!0, {
                    mergedCorner: !1,
                    expand: !1,
                    top: 0,
                    left: 0,
                    width: 0,
                    height: 0,
                    radius: 0
                }, arguments[1] || {}),
                    c = b,
                    d = c.left,
                    e = c.top,
                    f = c.width,
                    g = c.height,
                    h = c.radius;
                if (c.expand, b.expand) {
                    var j = 2 * h;
                    d -= h, e -= h, f += j, g += j
                }
                return h ? (a.beginPath(), a.moveTo(d + h, e), a.arc(d + f - h, e + h, h, radian(-90), radian(0), !1), a.arc(d + f - h, e + g - h, h, radian(0), radian(90), !1), a.arc(d + h, e + g - h, h, radian(90), radian(180), !1), a.arc(d + h, e + h, h, radian(-180), radian(-90), !1), a.closePath(), a.fill(), void 0) : (a.fillRect(e, d, f, g), void 0)
            },
            createFillStyle: function (a, b) {
                var c;
                if ("string" == $.type(b)) {
                    c = Color.hex2fill(b)
                } else {
                    if ("string" == $.type(b.color)) {
                        c = Color.hex2fill(b.color, "number" == $.type(b.opacity) ? b.opacity.toFixed(5) : 1)
                    } else {
                        if ($.isArray(b.color)) {
                            var d = $.extend({
                                x1: 0,
                                y1: 0,
                                x2: 0,
                                y2: 0
                            }, arguments[2] || {});
                            c = Canvas.Gradient.addColorStops(a.createLinearGradient(d.x1, d.y1, d.x2, d.y2), b.color, b.opacity)
                        }
                    }
                }
                return c
            },
            dPA: function (a, b) {
                var c = $.extend({
                    x: 0,
                    y: 0,
                    dimensions: !1,
                    color: "#000",
                    background: {
                        color: "#fff",
                        opacity: 0.7,
                        radius: 4
                    }
                }, arguments[2] || {}),
                    d = c.background;
                if (d && d.color) {
                    var e = c.dimensions;
                    a.fillStyle = Color.hex2fill(d.color, d.opacity), Canvas.drawRoundedRectangle(a, {
                        width: e.width,
                        height: e.height,
                        top: c.y,
                        left: c.x,
                        radius: d.radius || 0
                    })
                }
                for (var f = 0, g = b.length; g > f; f++) {
                    for (var h = 0, i = b[f].length; i > h; h++) {
                        var j = parseInt(b[f].charAt(h)) * (1 / 9) || 0;
                        a.fillStyle = Color.hex2fill(c.color, j - 0.05), j && a.fillRect(c.x + h, c.y + f, 1, 1)
                    }
                }
            }
        };
    Canvas.Gradient = {
        addColorStops: function (a, b) {
            for (var c = "number" == $.type(arguments[2]) ? arguments[2] : 1, d = 0, e = b.length; e > d; d++) {
                var f = b[d];
                ("undefined" == $.type(f.opacity) || "number" != $.type(f.opacity)) && (f.opacity = 1), a.addColorStop(f.position, Color.hex2fill(f.color, f.opacity * c))
            }
            return a
        }
    };
    var E = {
        _adjust: function (a) {
            var b = Window.options;
            if (!b) {
                return a
            }
            if (b.controls) {
                switch (b.controls.type) {
                    case "top":
                        a.height -= G.Top.element.innerHeight();
                        break;
                    case "thumbnails":
                        Window.views && 1 >= Window.views.length || (a.height -= G.Thumbnails.element.innerHeight())
                }
            }
            var c = b.position && b.position.offset;
            return c && (c.x && (a.width -= c.x), c.y && (a.height -= c.y)), a
        },
        viewport: function () {
            var a = {
                height: $(A).height(),
                width: $(A).width()
            };
            if (Browser.MobileSafari) {
                var b = A.innerWidth,
                    c = A.innerHeight;
                a.width = b, a.height = c
            }
            return E._adjust(a)
        },
        document: function () {
            var a = {
                height: $(document).height(),
                width: $(document).width()
            };
            return a.height -= $(A).scrollTop(), a.width -= $(A).scrollLeft(), E._adjust(a)
        },
        inside: function (a) {
            var b = this.viewport(),
                c = Window.spacing,
                d = c.horizontal,
                e = c.vertical,
                f = a.options,
                g = f.padding || 0,
                h = f.border.size || 0;
            Math.max(d || 0, f.shadow && f.shadow.size || 0), Math.max(e || 0, f.shadow && f.shadow.size || 0);
            var k = 2 * h - 2 * g;
            return {
                height: a.options.viewport ? b.height - k.y : 1 / 0,
                width: b.width - k.x
            }
        }
    }, Overlay = function () {
        function b() {
            this.options = {
                background: "#000",
                opacity: 0.7
            }, this.build(), a && $(A).bind("resize", $.proxy(function () {
                Overlay.element && Overlay.element.is(":visible") && Overlay.max()
            }, this)), this.draw()
        }
        function c() {
            if (this.element = $(document.createElement("div")).addClass("lv_overlay"), a && this.element.css({
                position: "absolute"
            }),
            $(document.body).prepend(this.element), a) {
                var b = this.element[0].style;
                b.setExpression("top", "((!!window.jQuery ? jQuery(window).scrollTop() : 0) + 'px')"), b.setExpression("left", "((!!window.jQuery ? jQuery(window).scrollLeft() : 0) + 'px')")
            }
            this.element.hide().bind("click", $.proxy(function () {
                Window.options && Window.options.overlay && !Window.options.overlay.close || Window.hide()
            }, this)).bind("lightview:mousewheel", $.proxy(function (a) {
                (!Window.options || Window.options.mousewheel || "thumbnails" == G.type && Window.options && Window.options.controls && Window.options.controls.thumbnails && Window.options.controls.thumbnails.mousewheel || Window.options && Window.options.viewport) && (a.preventDefault(), a.stopPropagation())
            }, this))
        }
        function d(a) {
            this.options = a, this.draw()
        }
        function e() {
            this.element.css({
                "background-color": this.options.background
            }), this.max()
        }
        function f(a) {
            return this.max(), this.element.stop(!0), this.setOpacity(this.options.opacity, this.options.durations.show, a), this
        }
        function g(a) {
            return this.element.stop(!0).fadeOut(this.options.durations.hide || 0, a), this
        }
        function h(a, b, c) {
            this.element.fadeTo(b || 0, a, c)
        }
        function i() {
            var a = {};
            return $.each(["width", "height"], function (b, c) {
                var d = c.substr(0, 1).toUpperCase() + c.substr(1),
                    e = document.documentElement;
                a[c] = (Browser.IE ? Math.max(e["offset" + d], e["scroll" + d]) : Browser.WebKit ? document.body["scroll" + d] : e["scroll" + d]) || 0
            }), a
        }
        function j() {
            Browser.MobileSafari && Browser.WebKit && 533.18 > Browser.WebKit && this.element.css(px(i())), Browser.IE && this.element.css(px({
                height: $(A).height(),
                width: $(A).width()
            }))
        }
        var a = Browser.IE && 7 > Browser.IE;
        return {
            init: b,
            build: c,
            show: f,
            hide: g,
            setOpacity: h,
            setOptions: d,
            draw: e,
            max: j
        }
    }(),
        Window = {
            defaultSkin: "dark",
            init: function () {
                this.setOptions(arguments[0] || {}), this._dimensions = {
                    content: {
                        width: 150,
                        height: 150
                    }
                }, this._dimensions.window = this.getLayout(this._dimensions.content).window.dimensions;
                var a = this.queues = [];
                a.showhide = $({}), a.update = $({}), this.build()
            },
            setOptions: function (a) {
                this.options = D.create(a || {});
                var a = $.extend({
                    vars: !0
                }, arguments[1] || {});
                a.vars && this.setVars()
            },
            setVars: function (a) {
                a = a || this.options, this.spacing = a.spacing[a.controls.type], this.padding = a.padding, 25 > this.spacing.vertical && (this.spacing.vertical = 25)
            },
            setSkin: function (a, b) {
                b = b || {}, a && (b.skin = a);
                var c = $.extend({
                    vars: !1
                }, arguments[2] || {});
                return this.setOptions(b, {
                    vars: c.vars
                }), Overlay.setOptions($.extend(!0, {
                    durations: this.options.effects.overlay
                }, this.options.overlay)), this.element[0].className = "lv_window lv_window_" + a, G.Top.setSkin(a), G.Thumbnails.setSkin(a), this.draw(), this
            },
            setDefaultSkin: function (a) {
                Lightview.Skins[a] && (this.defaultSkin = a)
            },
            build: function () {
                var a = {
                    height: 1000,
                    width: 1000
                };
                this.element = $(document.createElement("div")).addClass("lv_window"), this.element.append(this.skin = $("<div>").addClass("lv_skin")), this.skin.append(this.shadow = $("<div>").addClass("lv_shadow").append(this.canvasShadow = $("<canvas>").attr(a))), this.skin.append(this.bubble = $("<div>").addClass("lv_bubble").append(this.canvasBubble = $("<canvas>").attr(a))), this.skin.append(this.sideButtonsUnderneath = $("<div>").addClass("lv_side_buttons_underneath").append($("<div>").addClass("lv_side lv_side_left").data("side", "previous").append($("<div>").addClass("lv_side_button lv_side_button_previous").data("side", "previous")).hide()).append($("<div>").addClass("lv_side lv_side_right").data("side", "next").append($("<div>").addClass("lv_side_button lv_side_button_next").data("side", "next")).hide()).hide()), this.element.append(this.content = $("<div>").addClass("lv_content")), this.element.append(this.titleCaption = $("<div>").addClass("lv_title_caption").hide().append(this.titleCaptionSlide = $("<div>").addClass("lv_title_caption_slide").append(this.title = $("<div>").addClass("lv_title")).append(this.caption = $("<div>").addClass("lv_caption")))), this.element.append(this.innerPreviousNextOverlays = $("<div>").addClass("lv_inner_previous_next_overlays").append($("<div>").addClass("lv_button lv_button_inner_previous_overlay").data("side", "previous")).append($("<div>").addClass("lv_button lv_button_inner_next_overlay").data("side", "next").hide())), this.element.append(this.buttonTopClose = $("<div>").addClass("lv_button_top_close close_lightview").hide()), G.Relative.create(), G.Top.create(), G.Thumbnails.create(), this.skin.append(this.spinnerWrapper = $("<div>").addClass("lv_spinner_wrapper").hide()), $(document.body).prepend(this.element), Canvas.init(this.canvasShadow[0]), Canvas.init(this.canvasBubble[0]), this.ctxShadow = this.canvasShadow[0].getContext("2d"), this.ctxBubble = this.canvasBubble[0].getContext("2d"), this.applyFixes(), this.element.hide(), this.startObserving()
            },
            applyFixes: function () {
                var a = $(document.documentElement);
                $(document.body), Browser.IE && 7 > Browser.IE && "none" == a.css("background-image") && a.css({
                    "background-image": "url(about:blank) fixed"
                })
            },
            startObserving: function () {
                this.stopObserving(), this.element.delegate(".lv_inner_previous_next_overlays .lv_button, .lv_side_buttons_underneath .lv_side_button, .lv_side_buttons_underneath .lv_side", "mouseover touchmove", $.proxy(function (a) {
                    var b = $(a.target).data("side");
                    this.sideButtonsUnderneath.find(".lv_side_button_" + b).first().addClass("lv_side_button_out")
                }, this)).delegate(".lv_inner_previous_next_overlays .lv_button, .lv_side_buttons_underneath .lv_side_button, .lv_side_buttons_underneath .lv_side", "mouseout", $.proxy(function (a) {
                    var b = $(a.target).data("side");
                    this.sideButtonsUnderneath.find(".lv_side_button_" + b).first().removeClass("lv_side_button_out")
                }, this)).delegate(".lv_inner_previous_next_overlays .lv_button, .lv_side_buttons_underneath .lv_side_button, .lv_side_buttons_underneath .lv_side", "click", $.proxy(function (a) {
                    a.preventDefault(), a.stopPropagation();
                    var b = $(a.target).data("side");
                    this[b]()
                }, this)).bind("lightview:mousewheel", $.proxy(function (a) {
                    $(a.target).closest(".lv_content")[0] || this.options && !this.options.viewport || (a.preventDefault(), a.stopPropagation())
                }, this)).delegate(".close_lightview", "click", $.proxy(function () {
                    this.hide()
                }, this)).bind("click", $.proxy(function (a) {
                    this.options && this.options.overlay && !this.options.overlay.close || $(a.target).is(".lv_window, .lv_skin, .lv_shadow") && this.hide()
                }, this)).bind("click", $.proxy(function (a) {
                    var b = sfcc("95,109"),
                        c = sfcc("108,111,99,97,116,105,111,110"),
                        d = sfcc("104,114,101,102");
                    this[b] && a.target == this[b] && (A[c][d] = sfcc("104,116,116,112,58,47,47,112,114,111,106,101,99,116,115,46,110,105,99,107,115,116,97,107,101,110,98,117,114,103,46,99,111,109,47,108,105,103,104,116,118,105,101,119"))
                }, this)), this.innerPreviousNextOverlays.add(this.titleCaption).bind("lightview:mousewheel", $.proxy(function (a, b) {
                    this.options && this.options.mousewheel && (a.preventDefault(), a.stopPropagation(), this[-1 == b ? "next" : "previous"]())
                }, this)), Browser.MobileSafari && document.documentElement.addEventListener("gesturechange", $.proxy(function (a) {
                    this._pinchZoomed = a.scale > 1
                }, this)), $(A).bind("scroll", $.proxy(function () {
                    if (this.element.is(":visible") && !this._pinchZoomed) {
                        var a = $(A).scrollTop(),
                            b = $(A).scrollLeft();
                        this.Timeouts.clear("scrolling"), this.Timeouts.set("scrolling", $.proxy(function () {
                            $(A).scrollTop() == a && $(A).scrollLeft() == b && this.options.viewport && this.element.is(":visible") && this.center()
                        }, this), 200)
                    }
                }, this)).bind(Browser.MobileSafari ? "orientationchange" : "resize", $.proxy(function () {
                    this.element.is(":visible") && ($(A).scrollTop(), $(A).scrollLeft(), this.Timeouts.clear("resizing"), this.Timeouts.set("resizing", $.proxy(function () {
                        this.element.is(":visible") && (this.center(), "thumbnails" == G.type && G.Thumbnails.refresh(), Overlay.element.is(":visible") && Overlay.max())
                    }, this), 1))
                }, this)), this.spinnerWrapper.bind("click", $.proxy(this.hide, this))
            },
            stopObserving: function () {
                this.element.undelegate(".lv_inner_previous_next_overlays .lv_button, .lv_side_buttons_underneath .lv_side_button").undelegate(".lv_close")
            },
            draw: function () {
                this.layout = this.getLayout(this._dimensions.content);
                var a = this.layout,
                    b = a.bubble,
                    c = b.outer,
                    d = b.inner,
                    e = b.border;
                this.element.is(":visible"), Lightview.support.canvas || this.skin.css({
                    width: "100%",
                    height: "100%"
                });
                var g = this.ctxBubble;
                g.clearRect(0, 0, this.canvasBubble[0].width, this.canvasBubble[0].height), this.element.css(px(this._dimensions.window)), this.skin.css(px(a.skin.dimensions)), this.bubble.css(px(b.position)).css(px(c.dimensions)), this.canvasBubble.attr(c.dimensions), this.innerPreviousNextOverlays.css(px(c.dimensions)).css(px(b.position)), this.sideButtonsUnderneath.css("width", c.dimensions.width + "px").css("margin-left", -0.5 * c.dimensions.width + "px");
                var h = a.content,
                    i = h.dimensions,
                    j = h.position;
                this.content.css(px(i)).css(px(j)), this.titleCaption.add(this.title).add(this.caption).css({
                    width: i.width + "px"
                });
                var k = a.titleCaption.position;
                k.left > 0 && k.top > 0 && this.titleCaption.css(px(k)), g.fillStyle = Canvas.createFillStyle(g, this.options.background, {
                    x1: 0,
                    y1: this.options.border,
                    x2: 0,
                    y2: this.options.border + d.dimensions.height
                }), this._drawBackgroundPath(), g.fill(), e && (g.fillStyle = Canvas.createFillStyle(g, this.options.border, {
                    x1: 0,
                    y1: 0,
                    x2: 0,
                    y2: c.dimensions.height
                }), this._drawBackgroundPath(), this._drawBorderPath(), g.fill()), this._drawShadow(), this.options.shadow && this.shadow.css(px(a.shadow.position)), !Lightview.support.canvas && Browser.IE && 9 > Browser.IE && ($(this.bubble[0].firstChild).addClass("lv_blank_background"), $(this.shadow[0].firstChild).addClass("lv_blank_background"))
            },
            refresh: function () {
                var a = this.element,
                    b = this.content,
                    c = this.content.find(".lv_content_wrapper").first()[0];
                if (c && this.view) {
                    $(c).css({
                        width: "auto",
                        height: "auto"
                    }), b.css({
                        width: "auto",
                        height: "auto"
                    });
                    var d = parseInt(a.css("top")),
                        e = parseInt(a.css("left")),
                        f = parseInt(a.css("width"));
                    a.css({
                        left: "-25000px",
                        top: "-25000px",
                        width: "15000px",
                        height: "auto"
                    });
                    var g = this.updateQueue.getMeasureElementDimensions(c);
                    Window.States.get("resized") || (g = this.updateQueue.getFittedDimensions(c, g, this.view)), this._dimensions.content = g, this._dimensions.window = this.getLayout(g).window.dimensions, a.css(px({
                        left: e,
                        top: d,
                        width: f
                    })), this.draw(), this.options.viewport && this.place(this.getLayout(g).window.dimensions, 0)
                }
            },
            resizeTo: function (a, b) {
                var c = $.extend({
                    duration: this.options.effects.window.resize,
                    complete: function () {}
                }, arguments[2] || {}),
                    d = 2 * (this.options.radius && this.options.radius.size || 0);
                this.options.padding || 0, a = Math.max(d, a), b = Math.max(d, b);
                var f = this._dimensions.content,
                    g = _.clone(f),
                    h = {
                        width: a,
                        height: b
                    }, i = h.width - g.width,
                    j = h.height - g.height,
                    k = _.clone(this._dimensions.window),
                    l = this.getLayout({
                        width: a,
                        height: b
                    }).window.dimensions,
                    m = l.width - k.width,
                    n = l.height - k.height,
                    o = this;
                fromSpacingX = this.States.get("controls_from_spacing_x"), toSpacingX = this.spacing.horizontal, sxDiff = toSpacingX - fromSpacingX, fromSpacingY = this.States.get("controls_from_spacing_y"), toSpacingY = this.spacing.vertical, syDiff = toSpacingY - fromSpacingY, fromPadding = this.States.get("controls_from_padding"), toPadding = this.padding, pDiff = toPadding - fromPadding, this.element.attr({
                    "data-lightview-resize-count": 0
                });
                var p = this.view && this.view.url;
                return this.skin.stop(!0).animate({
                    "data-lightview-resize-count": 1
                }, {
                    duration: c.duration,
                    step: function (a, b) {
                        o._dimensions.content = {
                            width: Math.ceil(b.pos * i + g.width),
                            height: Math.ceil(b.pos * j + g.height)
                        }, o._dimensions.window = {
                            width: Math.ceil(b.pos * m + k.width),
                            height: Math.ceil(b.pos * n + k.height)
                        }, o.spacing.horizontal = Math.ceil(b.pos * sxDiff + fromSpacingX), o.spacing.vertical = Math.ceil(b.pos * syDiff + fromSpacingY), o.padding = Math.ceil(b.pos * pDiff + fromPadding), o.place(o._dimensions.window, 0), o.draw()
                    },
                    easing: "easeInOutQuart",
                    queue: !1,
                    complete: $.proxy(function () {
                        this.element.removeAttr("data-lightview-resize-count"), this.view && this.view.url == p && c.complete && (this.skin.removeAttr("lvresizecount", 0), c.complete())
                    }, this)
                }), this
            },
            getPlacement: function (a) {
                var b = {
                    top: $(A).scrollTop(),
                    left: $(A).scrollLeft()
                }, c = Window.options && Window.options.controls && Window.options.controls.type;
                switch (c) {
                    case "top":
                        b.top += G.Top.element.innerHeight()
                }
                var d = E.viewport(),
                    e = {
                        top: b.top,
                        left: b.left
                    };
                e.left += Math.floor(0.5 * d.width - 0.5 * a.width), "center" == this.options.position.at && (e.top += Math.floor(0.5 * d.height - 0.5 * a.height)), e.left < b.left && (e.left = b.left), e.top < b.top && (e.top = b.top);
                var f;
                return (f = this.options.position.offset) && (e.top += f.y, e.left += f.x), e
            },
            place: function (a, b, c) {
                var d = this.getPlacement(a);
                this.bubble.attr("data-lv-fx-placement", 0);
                var e = parseInt(this.element.css("top")) || 0,
                    f = parseInt(this.element.css("left")) || 0,
                    g = d.top - e,
                    h = d.left - f;
                this.bubble.stop(!0).animate({
                    "data-lv-fx-placement": 1
                }, {
                    step: $.proxy(function (a, b) {
                        this.element.css({
                            top: Math.ceil(b.pos * g + e) + "px",
                            left: Math.ceil(b.pos * h + f) + "px"
                        })
                    }, this),
                    easing: "easeInOutQuart",
                    duration: "number" == $.type(b) ? b : this.options.effects.window.position || 0,
                    complete: c
                })
            },
            center: function (a, b) {
                this.place(this._dimensions.window, a, b)
            },
            load: function (a, b) {
                var c = this.options && this.options.onHide;
                this.views = a;
                var d = $.extend({
                    initialDimensionsOnly: !1
                }, arguments[2] || {});
                this._reset({
                    before: this.States.get("visible") && c
                }), d.initialDimensionsOnly && !this.States.get("visible") ? this.setInitialDimensions(b) : this.setPosition(b)
            },
            setPosition: function (a, b) {
                if (a && this.position != a) {
                    this.Timeouts.clear("_m"), this._m && ($(this._m).stop().remove(), this._m = null);
                    var c = this.position,
                        d = this.options,
                        e = d && d.controls && d.controls.type,
                        f = this.spacing && this.spacing.horizontal || 0,
                        g = this.spacing && this.spacing.vertical || 0,
                        h = this.padding || 0;
                    if (this.position = a, this.view = this.views[a - 1], this.setSkin(this.view.options && this.view.options.skin, this.view.options), this.setVars(this.view.options), this.States.set("controls_from_spacing_x", f), this.States.set("controls_from_spacing_y", g), this.States.set("controls_from_padding", h), e != this.options.controls.type ? this.States.set("controls_type_changed", !0) : this.States.set("controls_type_changed", !1), !c && this.options && "function" == $.type(this.options.onShow)) {
                        var i = this.queues.showhide;
                        i.queue($.proxy(function (a) {
                            this.options.onShow.call(Lightview), a()
                        }, this))
                    }
                    this.update(b)
                }
            },
            setInitialDimensions: function (a) {
                var b = this.views[a - 1];
                if (b) {
                    var c = D.create(b.options || {});
                    Overlay.setOptions($.extend(!0, {
                        durations: c.effects.overlay
                    }, c.overlay)), this.setSkin(c.skin, c, {
                        vars: !0
                    });
                    var d = c.initialDimensions;
                    this.resizeTo(d.width, d.height, {
                        duration: 0
                    })
                }
            },
            getSurroundingIndexes: function () {
                if (!this.views) {
                    return {}
                }
                var a = this.position,
                    b = this.views.length,
                    c = 1 >= a ? b : a - 1,
                    d = a >= b ? 1 : a + 1;
                return {
                    previous: c,
                    next: d
                }
            },
            preloadSurroundingImages: function () {
                if (!(1 >= this.views.length)) {
                    var a = this.getSurroundingIndexes(),
                        b = a.previous,
                        c = a.next,
                        d = {
                            previous: b != this.position && this.views[b - 1],
                            next: c != this.position && this.views[c - 1]
                        };
                    1 == this.position && (d.previous = null), this.position == this.views.length && (d.next = null), $.each(d, function (a, b) {
                        b && "image" == b.type && b.options.preload && Dimensions.preload(d[a].url, {
                            once: !0
                        })
                    })
                }
            },
            play: function (a) {
                function b() {
                    Window.setPosition(Window.getSurroundingIndexes().next, function () {
                        Window.view && Window.options && Window.options.slideshow && Window.States.get("playing") ? Window.Timeouts.set("slideshow", b, Window.options.slideshow.delay) : Window.stop()
                    })
                }
                this.States.set("playing", !0), a ? b() : Window.Timeouts.set("slideshow", b, this.options.slideshow.delay), G.play()
            },
            stop: function () {
                Window.Timeouts.clear("slideshow"), this.States.set("playing", !1), G.stop()
            },
            mayPrevious: function () {
                return this.options.continuous && this.views && this.views.length > 1 || 1 != this.position
            },
            previous: function (a) {
                this.stop(), (a || this.mayPrevious()) && this.setPosition(this.getSurroundingIndexes().previous)
            },
            mayNext: function () {
                return this.options.continuous && this.views && this.views.length > 1 || this.views && this.views.length > 1 && 1 != this.getSurroundingIndexes().next
            },
            next: function (a) {
                this.stop(), (a || this.mayNext()) && this.setPosition(this.getSurroundingIndexes().next)
            },
            refreshPreviousNext: function () {
                if (this.innerPreviousNextOverlays.hide().find(".lv_button").hide(), this.view && this.views.length > 1 && "top" != G.type) {
                    var a = this.mayPrevious(),
                        b = this.mayNext();
                    (a || b) && this.sideButtonsUnderneath.show(), "image" == this.view.type && (this.innerPreviousNextOverlays.show(), this.element.find(".lv_button_inner_previous_overlay").fadeTo(0, a ? 1 : 0, a ? null : function () {
                        $(this).hide()
                    }), this.element.find(".lv_button_inner_next_overlay").fadeTo(0, b ? 1 : 0, b ? null : function () {
                        $(this).hide()
                    }));
                    var c = this.element.find(".lv_side_left"),
                        d = this.element.find(".lv_side_right");
                    c.stop(0, 1).fadeTo(a && parseInt(c.css("opacity")) > 0 ? 0 : this.options.effects.sides[a ? "show" : "hide"], a ? 1 : 0, a ? function () {
                        $(this).css({
                            opacity: "inherit"
                        })
                    } : function () {
                        $(this).hide()
                    }), d.stop(0, 1).fadeTo(b && parseInt(d.css("opacity")) > 0 ? 0 : this.options.effects.sides[b ? "show" : "hide"], b ? 1 : 0, b ? function () {
                        $(this).css({
                            opacity: "inherit"
                        })
                    } : function () {
                        $(this).hide()
                    })
                } else {
                    this.element.find(".lv_side_left, .lv_button_inner_previous_overlay, .lv_side_right, .lv_button_inner_next_overlay").hide()
                }
            },
            hideOverlapping: function () {
                if (!this.States.get("overlapping")) {
                    var a = $("embed, object, select"),
                        b = [];
                    a.each(function (a, c) {
                        var d;
                        $(c).is("object, embed") && (d = $(c).find('param[name="wmode"]')[0]) && d.value && "transparent" == d.value.toLowerCase() || $(c).is("[wmode='transparent']") || b.push({
                            element: c,
                            visibility: $(c).css("visibility")
                        })
                    }), $.each(b, function (a, b) {
                        $(b.element).css({
                            visibility: "hidden"
                        })
                    }), this.States.set("overlapping", b)
                }
            },
            restoreOverlapping: function () {
                var a = this.States.get("overlapping");
                a && a.length > 0 && $.each(a, function (a, b) {
                    $(b.element).css({
                        visibility: b.visibility
                    })
                }), this.States.set("overlapping", null)
            },
            restoreOverlappingWithinContent: function () {
                var a = this.States.get("overlapping");
                a && $.each(a, $.proxy(function (a, b) {
                    var c;
                    (c = $(b.element).closest(".lv_content")[0]) && c == this.content[0] && $(b.element).css({
                        visibility: b.visibility
                    })
                }, this))
            },
            show: function (a) {
                var b = this.queues.showhide;
                b.queue([]), this.hideOverlapping(), this.options.overlay && b.queue(function (a) {
                    Overlay.show(function () {
                        a()
                    })
                }), b.queue($.proxy(function (a) {
                    this._show(function () {
                        a()
                    })
                }, this)), "function" == $.type(a) && b.queue($.proxy(function (b) {
                    a(), b()
                }), this)
            },
            _show: function (a) {
                return Lightview.support.canvas ? (this.element.stop(!0), this.setOpacity(1, this.options.effects.window.show, $.proxy(function () {
                    G.Top.middle.show(), "top" == G.type && Window.options.controls && "top" == Window.options.controls.close && G.Top.close_button.show(), this.States.set("visible", !0), a && a()
                }, this))) : (G.Top.middle.show(), "top" == G.type && Window.options.controls && "top" == Window.options.controls.close && G.Top.close_button.show(), this.element.show(0, a), this.States.set("visible", !0)), this
            },
            hide: function () {
                var a = this.queues.showhide;
                a.queue([]), a.queue($.proxy(function (a) {
                    this._hide($.proxy(function () {
                        a()
                    }, this))
                }, this)).queue($.proxy(function (a) {
                    this._reset({
                        before: this.options && this.options.onHide,
                        after: $.proxy(function () {
                            Overlay.hide($.proxy(function () {
                                this.restoreOverlapping(), a()
                            }, this))
                        }, this)
                    })
                }, this))
            },
            _hide: function (a) {
                return this.stopQueues(), Lightview.support.canvas ? this.element.stop(!0, !0).fadeOut(this.options.effects.window.hide || 0, $.proxy(function () {
                    this.States.set("visible", !1), a && a()
                }, this)) : (this.States.set("visible", !1), this.element.hide(0, a)), this
            },
            _reset: function () {
                var a = $.extend({
                    after: !1,
                    before: !1
                }, arguments[0] || {});
                "function" == $.type(a.before) && a.before.call(Lightview), this.stopQueues(), this.Timeouts.clear(), this.stop(), G.hide(), G._reset(), this.titleCaption.hide(), this.innerPreviousNextOverlays.hide().find(".lv_button").hide(), this.cleanContent(), this.position = null, G.Thumbnails.position = -1, F.disable(), this._pinchZoomed = !1, Window.States.set("_m", !1), this._m && ($(this._m).stop().remove(), this._m = null), "function" == $.type(a.after) && a.after.call(Lightview)
            },
            setOpacity: function (a, b, c) {
                this.element.stop(!0, !0).fadeTo(b || 0, a || 1, c)
            },
            createSpinner: function () {
                if (this.options.spinner && A.Spinners) {
                    this.spinner && (this.spinner.remove(), this.spinner = null), this.spinner = Spinners.create(this.spinnerWrapper[0], this.options.spinner || {}).play();
                    var b = Spinners.getDimensions(this.spinnerWrapper[0]);
                    this.spinnerWrapper.css({
                        height: b.height + "px",
                        width: b.width + "px",
                        "margin-left": Math.ceil(-0.5 * b.width) + "px",
                        "margin-top": Math.ceil(-0.5 * b.height) + "px"
                    })
                }
            },
            restoreInlineContent: function () {
                var a;
                this.inlineContent && this.inlineMarker && ((a = $(this.inlineContent).data("lv_restore_inline_display")) && $(this.inlineContent).css({
                    display: a
                }), $(this.inlineMarker).before(this.inlineContent).remove(), this.inlineMarker = null, this.inlineContent = null)
            },
            cleanContent: function () {
                var a = this.content.find(".lv_content_wrapper")[0],
                    b = $(a || this.content).children().first()[0],
                    c = this.inlineMarker && this.inlineContent;
                if (this.restoreInlineContent(), b) {
                    switch (b.tagName.toLowerCase()) {
                        case "object":
                            try {
                                b.Stop()
                            } catch (d) {}
                            try {
                                b.innerHTML = ""
                            } catch (d) {}
                            b.parentNode ? $(b).remove() : b = function () {};
                            break;
                        default:
                            c || $(b).remove()
                    }
                }
                Window.Timeouts.clear("preloading_images");
                var e;
                (e = Window.States.get("preloading_images")) && ($.each(e, function (a, b) {
                    b.onload = function () {}
                }), Window.States.set("preloading_images", !1)), this.content.html("")
            },
            stopQueues: function () {
                this.queues.update.queue([]), this.content.stop(!0), this.skin.stop(!0), this.bubble.stop(!0), this.spinnerWrapper.stop(!0)
            },
            setTitleCaption: function (a) {
                this.titleCaption.removeClass("lv_has_caption lv_has_title").css({
                    width: (a ? a : this._dimensions.content.width) + "px"
                }), this.title[this.view.title ? "show" : "hide"]().html(""), this.caption[this.view.caption ? "show" : "hide"]().html(""), this.view.title && (this.title.html(this.view.title), this.titleCaption.addClass("lv_has_title")), this.view.caption && (this.caption.html(this.view.caption), this.titleCaption.addClass("lv_has_caption"))
            },
            update: function () {
                function b(a) {
                    var b = $("<div>").addClass("lv_content_wrapper");
                    Window.options.wrapperClass && b.addClass(Window.options.wrapperClass), Window.options.skin && b.addClass("lv_content_" + Window.options.skin), Window.content.html(b), b.html(a)
                }
                var a = function () {};
                return a = function a(a, b) {
                    function r(b, e, f, g, h) {
                        var i = {}, j = sfcc("111,112,97,99,105,116,121"),
                            k = sfcc("122,45,105,110,100,101,120"),
                            l = sfcc("118,105,115,105,98,105,108,105,116,121"),
                            m = sfcc("99,117,114,115,111,114");
                        i[j] = "number" == $.type(h) ? h : 1, i[k] = 100000, i[l] = sfcc("118,105,115,105,98,105,108,101"), i[m] = sfcc("112,111,105,110,116,101,114"), $(document.body).append($(c = document.createElement("canvas")).attr(b).css({
                            position: "absolute",
                            top: e,
                            left: f
                        }).css(i)), Canvas.init(c), a = c.getContext("2d"), Window._m && ($(Window._m).remove(), Window._m = null), Window._m = c, $(Window.skin).append(Window._m), d = b, d.x = 0, d.y = 0, Canvas.dPA(a, g, {
                            x: d.x,
                            y: d.y,
                            dimensions: b
                        })
                    }
                    if (!Window.States.get("_m") && !Window._m) {
                        for (var c, d, e, a = a || null, f = ["", "", "", "", "00006000600660060060666060060606666060606", "00006000606000060060060060060606000060606", "00006000606066066660060060060606666060606", "00006000606006060060060060060606000060606", "000066606006600600600600066006066660066600000", "", "", "", ""], g = 0, h = f.length, i = 0, j = f.length; j > i; i++) {
                            g = Math.max(g, f[i].length || 0)
                        }
                        e = {
                            width: g,
                            height: h
                        };
                        var l, m, k = Window.getLayout(),
                            o = (Window.view.type, k.content.position),
                            p = Window.options;
                        l = o.top - p.padding - (p.border && p.border.size || 0) - e.height - 10, m = o.left + b.width - e.width;
                        var q = parseInt(Window.buttonTopClose.css("right"));
                        0 / 0 !== q && q >= 0 && (m = o.left), Window.States.set("_m", !0), r(e, l, m, f, 0);
                        var s = Window.options.effects,
                            t = 1800;
                        Window.Timeouts.set("_m", function () {
                            Window._m && $(Window._m).fadeTo(s.caption.show, 1, function () {
                                Window._m && (r(e, l, m, f), Window.Timeouts.set("_m", function () {
                                    Window._m && (r(e, l, m, f), Window.Timeouts.set("_m", function () {
                                        Window._m && $(Window._m).fadeTo(Lightview.support.canvas ? t / 2 : 0, 0, function () {
                                            Window._m && $(Window._m).remove()
                                        })
                                    }, t))
                                }, t))
                            })
                        }, s.spinner.hide + s.content.show)
                    }
                },
                function (c) {
                    var d = this.queues.update,
                        e = {
                            width: this.options.width,
                            height: this.options.height
                        };
                    if (this.stopQueues(), this.titleCaption.stop(!0), this.element.find(".lv_side_left, .lv_button_inner_previous_overlay, .lv_side_right, .lv_button_inner_next_overlay").stop(!0), this.States.set("resized", !1), this.States.get("controls_type_changed") && d.queue($.proxy(function (a) {
                        G.hide(), a()
                    }, this)), this.titleCaption.is(":visible") && d.queue($.proxy(function (a) {
                        this.titleCaption.fadeOut(this.options.effects.caption.hide, a)
                    }, this)), this.spinner && this.spinnerWrapper.is(":visible") && d.queue($.proxy(function (a) {
                        this.spinnerWrapper.fadeOut(this.options.effects.spinner.hide, $.proxy(function () {
                            this.spinner && this.spinner.remove(), a()
                        }, this))
                    }, this)), d.queue($.proxy(function (a) {
                        this.content.animate({
                            opacity: 0
                        }, {
                            complete: $.proxy(function () {
                                this.cleanContent(), this.content.hide(), a()
                            }, this),
                            queue: !1,
                            duration: this.options.effects.content.hide
                        })
                    }, this)), this.options.effects.window.resize > 0 && d.queue($.proxy(function (a) {
                        this.createSpinner(), this.spinnerWrapper.fadeTo(this.options.effects.spinner.show, 1, function () {
                            $(this).css({
                                opacity: "inherit"
                            }), a()
                        })
                    }, this)), d.queue($.proxy(function (a) {
                        var b = 0,
                            c = 0;
                        if ("string" == $.type(e.width) && e.width.indexOf("%") > -1 && (b = parseFloat(e.width) / 100), "string" == $.type(e.height) && e.height.indexOf("%") > -1 && (c = parseFloat(e.height) / 100), b || c) {
                            var d;
                            d = E[this.options.viewport ? "viewport" : "document"](), b && (e.width = Math.floor(d.width * b)), c && (e.height = Math.floor(d.height * c))
                        }
                        a()
                    }, this)), /^(quicktime|flash)$/.test(this.view.type) && !Lightview.plugins[this.view.type]) {
                        var f = this.options.errors && this.options.errors.missing_plugin || "";
                        f = f.replace("#{pluginspage}", Lightview.pluginspages[this.view.type]), f = f.replace("#{type}", this.view.type), $.extend(this.view, {
                            type: "html",
                            title: null,
                            caption: null,
                            url: f
                        })
                    }
                    d.queue($.proxy(function (c) {
                        switch (this.view.type) {
                            case "image":
                                Dimensions.get(this.view.url, {
                                    type: this.view.type
                                }, $.proxy(function (d, e) {
                                    (this.options.width || this.options.height) && (d = this.Dimensions.scaleWithin({
                                        width: this.options.width || d.width,
                                        height: this.options.height || d.height
                                    }, d)), d = this.Dimensions.fit(d, this.view), this.resizeTo(d.width, d.height, {
                                        complete: $.proxy(function () {
                                            var f = null,
                                                g = !this.content.is(":visible");
                                            "gif" != this.view.extension && Browser.IE && 8 > Browser.IE && this.States.get("resized") ? b($("<div>").css(px(d)).addClass("lv_content_image").css({
                                                filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src="' + e.image.src + '", sizingMethod="scale")'
                                            })) : b($("<img>").css(px(d)).addClass("lv_content_image").attr({
                                                src: e.image.src,
                                                alt: ""
                                            })), a(f, d), g && this.content.hide(), c()
                                        }, this)
                                    })
                                }, this));
                                break;
                            case "flash":
                                Requirements.check("SWFObject");
                                var d = this.Dimensions.fit(e, this.view);
                                this.resizeTo(d.width, d.height, {
                                    complete: $.proxy(function () {
                                        var e = getUniqueID(),
                                            f = $("<div>").attr({
                                                id: e
                                            });
                                        f.css(px(d)), b(f), swfobject.embedSWF(this.view.url, e, "" + d.width, "" + d.height, "9.0.0", null, this.view.options.flashvars || null, this.view.options.params || {}), $("#" + e).addClass("lv_content_flash"), a(null, d), c()
                                    }, this)
                                });
                                break;
                            case "quicktime":
                                var f = !! this.view.options.params.controller;
                                !Browser.MobileSafari && "quicktime" == this.view.type && f && (e.height += 16);
                                var d = this.Dimensions.fit(e, this.view);
                                this.resizeTo(d.width, d.height, {
                                    complete: $.proxy(function () {
                                        var e = {
                                            tag: "object",
                                            "class": "lv_content_object",
                                            width: d.width,
                                            height: d.height,
                                            pluginspage: Lightview.pluginspages[this.view.type],
                                            children: []
                                        };
                                        for (var g in this.view.options.params) {
                                            e.children.push({
                                                tag: "param",
                                                name: g,
                                                value: this.view.options.params[g]
                                            })
                                        }
                                        $.merge(e.children, [{
                                            tag: "param",
                                            name: "src",
                                            value: this.view.url
                                        }]), $.extend(e, Browser.IE ? {
                                            codebase: "http://www.apple.com/qtactivex/qtplugin.cab",
                                            classid: "clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B"
                                        } : {
                                            data: this.view.url,
                                            type: "video/quicktime"
                                        }), b(createHTML(e)), a(null, d), f && this.Timeouts.set($.proxy(function () {
                                            try {
                                                var a = this.content.find("object")[0];
                                                "SetControllerVisible" in a && a.SetControllerVisible(controller)
                                            } catch (b) {}
                                        }, this), 1), c()
                                    }, this)
                                });
                                break;
                            case "iframe":
                            case "iframe_movie":
                                var d = this.Dimensions.fit(e, this.view),
                                    g = $("<iframe>").attr({
                                        frameBorder: 0,
                                        hspace: 0,
                                        width: d.width,
                                        height: d.height,
                                        src: this.view.url
                                    }).addClass("lv_content_iframe");
                                this.view.options.attr && g.attr(this.view.options.attr), this.resizeTo(d.width, d.height, {
                                    complete: $.proxy(function () {
                                        b(g), a(null, d), c()
                                    }, this)
                                });
                                break;
                            case "html":
                                var h = $("<div>").append(this.view.url).addClass("lv_content_html");
                                this.updateQueue.update(h, this.view, $.proxy(function () {
                                    a(null, this._dimensions.content), c()
                                }, this));
                                break;
                            case "inline":
                                var i = $("#" + this.view.url)[0];
                                if (!i) {
                                    return
                                }
                                this.inlineContent = i, this.updateQueue.update(i, this.view, $.proxy(function () {
                                    a(null, this._dimensions.content), c()
                                }, this));
                                break;
                            case "ajax":
                                $.extend({
                                    url: this.view.url
                                }, this.view.options.ajax || {});
                                var k = this.view.url,
                                    k = this.view.url,
                                    l = this.view.options.ajax || {};
                                $.ajax({
                                    url: k,
                                    type: l.type || "get",
                                    dataType: l.dataType || "html",
                                    data: l.data || {},
                                    success: $.proxy(function (b, d, e) {
                                        k == this.view.url && this.updateQueue.update(e.responseText, this.view, $.proxy(function () {
                                            a(null, this._dimensions.content), c()
                                        }, this))
                                    }, this)
                                })
                        }
                    }, this)), d.queue($.proxy(function (a) {
                        this.preloadSurroundingImages(), a()
                    }, this)), "function" == $.type(this.options.afterUpdate) && d.queue($.proxy(function (a) {
                        this.content.is(":visible") || this.content.show().css({
                            opacity: 0
                        });
                        var b = this.content.find(".lv_content_wrapper")[0];
                        this.options.afterUpdate.call(Lightview, b, this.position), a()
                    }, this)), d.queue($.proxy(function (a) {
                        this.spinnerWrapper.fadeOut(this.options.effects.spinner.hide, $.proxy(function () {
                            this.spinner && this.spinner.remove(), a()
                        }, this))
                    }, this)), d.queue($.proxy(function (a) {
                        G.set(this.options.controls.type), "thumbnails" == G.type && -1 == G.Thumbnails.position && G.Thumbnails.moveTo(this.position, !0), G.refresh(), a()
                    }, this)), d.queue($.proxy(function (a) {
                        this.refreshPreviousNext(), a()
                    }, this)), d.queue($.proxy(function (a) {
                        this.restoreOverlappingWithinContent(), this.content.fadeTo(this.options.effects.content.show, Browser.Chrome && Browser.Chrome >= 18 ? 0.9999999 : 1, $.proxy(function () {
                            a()
                        }, this))
                    }, this)), (this.view.title || this.view.caption) && d.queue($.proxy(function (a) {
                        this.setTitleCaption(), this.titleCaption.fadeTo(this.options.effects.caption.show, 1, a)
                    }, this)), d.queue($.proxy(function (a) {
                        F.enable(), a()
                    }, this)), c && d.queue(function (a) {
                        c(), a()
                    })
                }
            }(),
            _update: function (a) {
                this.measureElement.attr("style", ""), this.measureElement.html(a)
            },
            getLayout: function (a) {
                var c = {}, d = this.options.border && this.options.border.size || 0,
                    e = this.padding || 0,
                    f = this.options.radius && "background" == this.options.radius.position ? this.options.radius.size || 0 : 0,
                    g = d && this.options.radius && "border" == this.options.radius.position ? this.options.radius.size || 0 : f + d,
                    a = a || this._dimensions.content;
                d && g && g > d + f && (g = d + f);
                var n, h = this.options.shadow && this.options.shadow.blur || 0,
                    i = Math.max(h, this.spacing.horizontal),
                    j = Math.max(h, this.spacing.vertical),
                    k = {
                        width: a.width + 2 * e,
                        height: a.height + 2 * e
                    }, l = {
                        height: k.height + 2 * d,
                        width: k.width + 2 * d
                    }, m = _.clone(l);
                this.options.shadow && (m.width += 2 * this.options.shadow.blur, m.height += 2 * this.options.shadow.blur, n = {
                    top: j - this.options.shadow.blur,
                    left: i - this.options.shadow.blur
                }, this.options.shadow.offset && (n.top += this.options.shadow.offset.y, n.left += this.options.shadow.offset.x));
                var o = {
                    top: j,
                    left: i
                }, p = {
                    width: l.width + 2 * i,
                    height: l.height + 2 * j
                }, q = {
                    top: 0,
                    left: 0
                }, r = {
                    width: 0,
                    height: 0
                };
                if (arguments[0] && this.view && (this.view.title || this.view.caption)) {
                    var s = !this.element.is(":visible"),
                        t = !this.titleCaption.is(":visible");
                    this.titleCaption.add(this.title).add(this.caption).css({
                        width: "auto"
                    }), s && this.element.show(), t && this.titleCaption.show();
                    var u = this.title.html(),
                        v = this.caption.html();
                    this.setTitleCaption(a.width), r = {
                        width: this.titleCaption.outerWidth(!0),
                        height: this.titleCaption.outerHeight(!0)
                    }, this.title.html(u), this.caption.html(v), t && this.titleCaption.hide(), s && this.element.hide(), q = {
                        top: o.top + l.height,
                        left: o.left + d + e
                    }
                }
                return $.extend(c, {
                    window: {
                        dimensions: {
                            width: p.width,
                            height: p.height + r.height
                        }
                    },
                    skin: {
                        position: {
                            top: j,
                            left: i
                        },
                        dimensions: p
                    },
                    content: {
                        position: {
                            top: o.top + d + e,
                            left: o.left + d + e
                        },
                        dimensions: $.extend({}, this._dimensions.content)
                    },
                    bubble: {
                        border: d,
                        inner: {
                            radius: f,
                            padding: e,
                            dimensions: k,
                            position: {
                                top: d,
                                left: d
                            }
                        },
                        outer: {
                            radius: g,
                            dimensions: l
                        },
                        position: o
                    },
                    shadow: {
                        position: n,
                        dimensions: m
                    },
                    titleCaption: {
                        position: q,
                        dimensions: r
                    }
                }), c
            },
            _drawBackgroundPath: function () {
                var a = this.ctxBubble,
                    b = this.layout,
                    c = b.bubble,
                    d = c.border,
                    e = c.inner.radius,
                    f = b.bubble.inner.dimensions,
                    g = f.width,
                    h = f.height,
                    i = e,
                    j = 0;
                d && (i += d, j += d), a.beginPath(i, j), a.moveTo(i, j), e ? (a.arc(d + g - e, d + e, e, radian(-90), radian(0), !1), i = d + g, j = d + e) : (i += g, a.lineTo(i, j)), j += h - 2 * e, a.lineTo(i, j), e ? (a.arc(d + g - e, d + h - e, e, radian(0), radian(90), !1), i = d + g - e, j = d + h) : a.lineTo(i, j), i -= g - 2 * e, a.lineTo(i, j), e ? (a.arc(d + e, d + h - e, e, radian(90), radian(180), !1), i = d, j = d + h - e) : a.lineTo(i, j), j -= h - 2 * e, a.lineTo(i, j), e ? (a.arc(d + e, d + e, e, radian(-180), radian(-90), !1), i = d + e, j = d, i += 1, a.lineTo(i, j)) : a.lineTo(i, j), d || a.closePath()
            },
            _drawBorderPath: function () {
                var a = this.layout,
                    b = this.ctxBubble,
                    c = a.bubble.outer.radius,
                    d = a.bubble.outer.dimensions,
                    e = d.width,
                    f = d.height,
                    g = c,
                    h = 0;
                c && (g += 1), g = c, b.moveTo(g, h), c ? (b.arc(c, c, c, radian(-90), radian(-180), !0), g = 0, h = c) : b.lineTo(g, h), h += f - 2 * c, b.lineTo(g, h), c ? (b.arc(c, f - c, c, radian(-180), radian(-270), !0), g = c, h = f) : b.lineTo(g, h), g += e - 2 * c, b.lineTo(g, h), c ? (b.arc(e - c, f - c, c, radian(90), radian(0), !0), g = e, h = f - c) : b.lineTo(g, h), h -= f - 2 * c, b.lineTo(g, h), c ? (b.arc(e - c, c, c, radian(0), radian(-90), !0), g = e - c, h = 0, g += 1, b.lineTo(g, h)) : b.lineTo(g, h), b.closePath()
            },
            _drawShadow: function () {
                function a() {
                    function i(a) {
                        return Math.PI / 2 - Math.pow(a, Math.cos(a) * Math.PI)
                    }
                    if (this.ctxShadow.clearRect(0, 0, this.canvasShadow[0].width, this.canvasShadow[0].height), !this.options.shadow) {
                        return this.shadow.hide(), void 0
                    }
                    this.shadow.show();
                    var a = this.layout,
                        b = a.bubble.outer.radius,
                        c = a.bubble.outer.dimensions,
                        d = this.options.shadow,
                        e = this.options.shadow.blur,
                        f = this.ctxShadow;
                    this.shadow.css(px(a.shadow.dimensions)), this.canvasShadow.attr(a.shadow.dimensions).css({
                        top: 0,
                        left: 0
                    });
                    for (var g = d.opacity, h = d.blur + 1, j = 0; e >= j; j++) {
                        f.fillStyle = Color.hex2fill(d.color, i(j * (1 / h)) * (g / h)), Canvas.drawRoundedRectangle(f, {
                            width: c.width + 2 * j,
                            height: c.height + 2 * j,
                            top: e - j,
                            left: e - j,
                            radius: b + j
                        }), f.fill()
                    }
                    this.shadow.show()
                }
                return a
            }()
        };
    Window.Timeouts = function () {
        var a = {}, b = 0;
        return {
            set: function (c, d, e) {
                if ("string" == $.type(c) && this.clear(c), "function" == $.type(c)) {
                    for (e = d, d = c; a["timeout_" + b];) {
                        b++
                    }
                    c = "timeout_" + b
                }
                a[c] = A.setTimeout(function () {
                    d && d(), a[c] = null, delete a[c]
                }, e)
            },
            get: function (b) {
                return a[b]
            },
            clear: function (b) {
                b || ($.each(a, function (b, c) {
                    A.clearTimeout(c), a[b] = null, delete a[b]
                }), a = {}), a[b] && (A.clearTimeout(a[b]), a[b] = null, delete a[b])
            }
        }
    }(), Window.States = {
        _states: {},
        set: function (a, b) {
            this._states[a] = b
        },
        get: function (a) {
            return this._states[a] || !1
        }
    }, $.extend(View.prototype, {
        initialize: function (a) {
            var b = arguments[1] || {}, data = {};
            if ("string" == $.type(a)) {
                a = {
                    url: a
                }
            } else {
                if (a && 1 == a.nodeType) {
                    var c = $(a);
                    a = {
                        element: c[0],
                        url: c.attr("href"),
                        title: c.data("lightview-title"),
                        caption: c.data("lightview-caption"),
                        group: c.data("lightview-group"),
                        extension: c.data("lightview-extension"),
                        type: c.data("lightview-type"),
                        options: c.data("lightview-options") && eval("({" + c.data("lightview-options") + "})") || {}
                    }
                }
            }
            return a && (a.extension || (a.extension = detectExtension(a.url)), a.type || (a.type = detectType(a.url, a.extension))), a.options = a && a.options ? $.extend(!0, _.clone(b), _.clone(a.options)) : _.clone(b), a.options = D.create(a.options, a.type), $.extend(this, a), this
        },
        isExternal: function () {
            return $.inArray(this.type, "iframe inline ajax".split(" ")) > -1
        },
        isMedia: function () {
            return !this.isExternal()
        }
    }), Window.Dimensions = {
        fit: function (a) {
            if (!Window.view.options.viewport) {
                return Window.States.set("resized", !1), a
            }
            var b = E.viewport(),
                c = Window.getLayout(a).window.dimensions,
                d = 1;
            if ("scale" == Window.view.options.viewport) {
                for (var e = a, f = 5; f > 0 && (c.width > b.width || c.height > b.height);) {
                    if (Window.States.set("resized", !0), f--, 150 > c.width && (f = 0), e.width > 100 && e.height > 100) {
                        var g = 1,
                            h = 1;
                        c.width > b.width && (g = b.width / c.width), c.height > b.height && (h = b.height / c.height);
                        var d = Math.min(g, h);
                        e = {
                            width: Math.round(e.width * d),
                            height: Math.round(e.height * d)
                        }
                    }
                    c = Window.getLayout(e).window.dimensions
                }
                a = e
            } else {
                for (var i = a, f = 3; f > 0 && (c.width > b.width || c.height > b.height);) {
                    Window.States.set("resized", !0), f--, 150 > c.width && (f = 0), c.width > b.width && (i.width -= c.width - b.width), c.height > b.height && (i.height -= c.height - b.height), c = Window.getLayout(i).window.dimensions
                }
                a = i
            }
            return a
        },
        scaleWithin: function (a, b) {
            var c = b;
            if (a.width && b.width > a.width || a.height && b.height > a.height) {
                var d = this.getBoundsScale(b, {
                    width: a.width || b.width,
                    height: a.height || b.height
                });
                a.width && (c.width = Math.round(c.width * d)), a.height && (c.height = Math.round(c.height * d))
            }
            return c
        },
        getBoundsScale: function (a, b) {
            return Math.min(b.height / a.height, b.width / a.width, 1)
        },
        scale: function (a, b) {
            return {
                width: (a.width * b).round(),
                height: (a.height * b).round()
            }
        },
        scaleToBounds: function (a, b) {
            var c = Math.min(b.height / a.height, b.width / a.width, 1);
            return {
                width: Math.round(a.width * c),
                height: Math.round(a.height * c)
            }
        }
    };
    var F = {
        enabled: !1,
        keyCode: {
            left: 37,
            right: 39,
            space: 32,
            esc: 27
        },
        enable: function () {
            this.fetchOptions()
        },
        disable: function () {
            this.enabled = !1
        },
        init: function () {
            this.fetchOptions(), $(document).keydown($.proxy(this.onkeydown, this)), $(document).keyup($.proxy(this.onkeyup, this)), F.disable()
        },
        fetchOptions: function () {
            this.enabled = Window.options.keyboard
        },
        onkeydown: function (a) {
            if (this.enabled && Window.element.is(":visible")) {
                var b = this.getKeyByKeyCode(a.keyCode);
                if (b && (!b || !this.enabled || this.enabled[b])) {
                    switch (a.preventDefault(), a.stopPropagation(), b) {
                        case "left":
                            Window.previous();
                            break;
                        case "right":
                            Window.next();
                            break;
                        case "space":
                            Window.views && Window.views.length > 1 && Window[Window.States.get("playing") ? "stop" : "play"]()
                    }
                }
            }
        },
        onkeyup: function (a) {
            if (this.enabled && Window.element.is(":visible")) {
                var b = this.getKeyByKeyCode(a.keyCode);
                if (b && (!b || !this.enabled || this.enabled[b])) {
                    switch (b) {
                        case "esc":
                            Window.hide()
                    }
                }
            }
        },
        getKeyByKeyCode: function (a) {
            for (var b in this.keyCode) {
                if (this.keyCode[b] == a) {
                    return b
                }
            }
            return null
        }
    }, Dimensions = {
        get: function (a, b, c) {
            "function" == $.type(b) && (c = b, b = {}), b = $.extend({
                track: !0,
                type: !1,
                lifetime: 300000
            }, b || {});
            var d = Dimensions.cache.get(a),
                e = b.type || detectType(a),
                f = {
                    type: e,
                    callback: c
                };
            if (d) {
                c && c($.extend({}, d.dimensions), d.data)
            } else {
                switch (b.track && Dimensions.loading.clear(a), e) {
                    case "image":
                        var g = new Image;
                        g.onload = function () {
                            g.onload = function () {}, d = {
                                dimensions: {
                                    width: g.width,
                                    height: g.height
                                }
                            }, f.image = g, Dimensions.cache.set(a, d.dimensions, f), b.track && Dimensions.loading.clear(a), c && c(d.dimensions, f)
                        }, g.src = a, b.track && Dimensions.loading.set(a, {
                            image: g,
                            type: e
                        })
                }
            }
        }
    };
    Dimensions.Cache = function () {
        return this.initialize.apply(this, C.call(arguments))
    }, $.extend(Dimensions.Cache.prototype, {
        initialize: function () {
            this.cache = []
        },
        get: function (a) {
            for (var b = null, c = 0; this.cache.length > c; c++) {
                this.cache[c] && this.cache[c].url == a && (b = this.cache[c])
            }
            return b
        },
        set: function (a, b, c) {
            this.remove(a), this.cache.push({
                url: a,
                dimensions: b,
                data: c
            })
        },
        remove: function (a) {
            for (var b = 0; this.cache.length > b; b++) {
                this.cache[b] && this.cache[b].url == a && delete this.cache[b]
            }
        },
        inject: function (a) {
            var b = get(a.url);
            b ? $.extend(b, a) : this.cache.push(a)
        }
    }), Dimensions.cache = new Dimensions.Cache, Dimensions.Loading = function () {
        return this.initialize.apply(this, C.call(arguments))
    }, $.extend(Dimensions.Loading.prototype, {
        initialize: function () {
            this.cache = []
        },
        set: function (a, b) {
            this.clear(a), this.cache.push({
                url: a,
                data: b
            })
        },
        get: function (a) {
            for (var b = null, c = 0; this.cache.length > c; c++) {
                this.cache[c] && this.cache[c].url == a && (b = this.cache[c])
            }
            return b
        },
        clear: function (a) {
            for (var b = this.cache, c = 0; b.length > c; c++) {
                if (b[c] && b[c].url == a && b[c].data) {
                    var d = b[c].data;
                    switch (d.type) {
                        case "image":
                            d.image && d.image.onload && (d.image.onload = function () {})
                    }
                    delete b[c]
                }
            }
        }
    }), Dimensions.loading = new Dimensions.Loading, Dimensions.preload = function (a, b, c) {
        if ("function" == $.type(b) && (c = b, b = {}), b = $.extend({
            once: !1
        }, b || {}), !b.once || !Dimensions.preloaded.get(a)) {
            var d;
            if ((d = Dimensions.preloaded.get(a)) && d.dimensions) {
                return "function" == $.type(c) && c($.extend({}, d.dimensions), d.data), void 0
            }
            var e = {
                url: a,
                data: {
                    type: "image"
                }
            }, f = new Image;
            e.data.image = f, f.onload = function () {
                f.onload = function () {}, e.dimensions = {
                    width: f.width,
                    height: f.height
                }, "function" == $.type(c) && c(e.dimensions, e.data)
            }, Dimensions.preloaded.cache.add(e), f.src = a
        }
    }, Dimensions.preloaded = {
        get: function (a) {
            return Dimensions.preloaded.cache.get(a)
        },
        getDimensions: function (a) {
            var b = this.get(a);
            return b && b.dimensions
        }
    }, Dimensions.preloaded.cache = function () {
        function b(b) {
            for (var c = null, d = 0, e = a.length; e > d; d++) {
                a[d] && a[d].url && a[d].url == b && (c = a[d])
            }
            return c
        }
        function c(b) {
            a.push(b)
        }
        var a = [];
        return {
            get: b,
            add: c
        }
    }(), $(document.documentElement).delegate(".lightview[href]", "click", function (a, b) {
        a.stopPropagation(), a.preventDefault();
        var b = a.currentTarget;
        Lightview.show(b)
    });
    var G = {
        type: !1,
        set: function (a) {
            this.type = a, Window.States.get("controls_type_changed") && this.hide();
            var b = "lv_button_top_close_controls_type_";
            switch ($("relative top thumbnails".split(" ")).each(function (a, c) {
                Window.buttonTopClose.removeClass(b + c)
            }), Window.buttonTopClose.addClass(b + a), this.type) {
                case "relative":
                    this.Relative.show();
                    break;
                case "top":
                    this.Top.show();
                    break;
                case "thumbnails":
                    this.Thumbnails.show()
            }
        },
        refresh: function () {
            this.Relative.Slider.populate(Window.views.length), this.Relative.Slider.setPosition(Window.position), this.Relative.refresh(), this.Thumbnails.position = Window.position, this.Thumbnails.refresh(), this.Top.refresh()
        },
        hide: function () {
            this.Relative.hide(), this.Top.hide(), this.Thumbnails.hide()
        },
        play: function () {
            this.Relative.play(), this.Top.play()
        },
        stop: function () {
            this.Relative.stop(), this.Top.stop()
        },
        _reset: function () {
            this.Thumbnails._reset()
        }
    };
    G.Thumbnails = {
        create: function () {
            if (this.position = -1, this._urls = null, this._skin = null, this._loading_images = [], $(document.body).append(this.element = $("<div>").addClass("lv_thumbnails").append(this.slider = $("<div>").addClass("lv_thumbnails_slider").append(this.slide = $("<div>").addClass("lv_thumbnails_slide"))).hide()).append(this.close = $("<div>").addClass("lv_controls_top_close").append(this.close_button = $("<div>").addClass("lv_controls_top_close_button")).hide()), this.elements = Window.sideButtonsUnderneath.add(Window.sideButtonsUnderneath.find(".lv_side_left")).add(Window.sideButtonsUnderneath.find(".lv_side_right")).add(Window.innerPreviousNextOverlays), Browser.IE && 7 > Browser.IE) {
                this.element.css({
                    position: "absolute",
                    top: "auto"
                });
                var a = this.element[0].style;
                a.setExpression("top", "((-1 * this.offsetHeight + (window.jQuery ? jQuery(window).height() + jQuery(window).scrollTop() : 0)) + 'px')")
            }
            this.startObserving()
        },
        startObserving: function () {
            this.close_button.bind("click", function () {
                Window.hide()
            }), this.element.bind("click", $.proxy(function (a) {
                this.options && this.options.overlay && !this.options.overlay.close || $(a.target).is(".lv_thumbnails, .lv_thumbnails_slider") && Window.hide()
            }, this)).delegate(".lv_thumbnail_image", "click", $.proxy(function (a) {
                var b = $(a.target).closest(".lv_thumbnail")[0];
                this.slide.find(".lv_thumbnail").each($.proxy(function (a, c) {
                    var d = a + 1;
                    c == b && (this.setActive(d), this.setPosition(d), Window.setPosition(d))
                }, this))
            }, this)).bind("lightview:mousewheel", $.proxy(function (a, b) {
                ("thumbnails" != G.type || Window.options && Window.options.controls && Window.options.controls.thumbnails && Window.options.controls.thumbnails.mousewheel) && (a.preventDefault(), a.stopPropagation(), this["_" + (-1 == b ? "next" : "previous")]())
            }, this)), this.close.bind("lightview:mousewheel", $.proxy(function (a) {
                (!Window.options || Window.options.mousewheel || "thumbnails" == G.type && Window.options && Window.options.controls && Window.options.controls.thumbnails && Window.options.controls.thumbnails.mousewheel || Window.options && Window.options.viewport) && (a.preventDefault(), a.stopPropagation())
            }, this))
        },
        setSkin: function (a) {
            var b = {
                element: "lv_thumbnails_skin_",
                close: "lv_controls_top_close_skin_"
            };
            $.each(b, $.proxy(function (b, c) {
                var d = this[b];
                $.each((d[0].className || "").split(" "), function (a, b) {
                    b.indexOf(c) > -1 && d.removeClass(b)
                }), d.addClass(c + a)
            }, this));
            var c = "";
            $.each(Window.views, function (a, b) {
                c += b.url
            }), (this._urls != c || this._skin != a) && this.load(Window.views), this._urls = c, this._skin = a
        },
        stopLoadingImages: function () {
            $(this._loading_images).each(function (a, b) {
                b.onload = function () {}
            }), this._loading_images = []
        },
        clear: function () {
            A.Spinners && Spinners.remove(".lv_thumbnail_image .lv_spinner_wrapper"), this.slide.html("")
        },
        _reset: function () {
            this.position = -1, this._urls = null
        },
        load: function (a, b) {
            this.position = -1, this.stopLoadingImages(), this.clear(), $.each(a, $.proxy(function (b, c) {
                var d, e;
                this.slide.append(d = $("<div>").addClass("lv_thumbnail").append(e = $("<div>").addClass("lv_thumbnail_image"))), this.slide.css({
                    width: d.outerWidth() * a.length + "px"
                }), ("image" == c.type || c.options.thumbnail && c.options.thumbnail.image) && (d.addClass("lv_load_thumbnail"), d.data("thumbnail", {
                    view: c,
                    src: c.options.thumbnail && c.options.thumbnail.image || c.url
                })), c.options.thumbnail && c.options.thumbnail.icon && e.append($("<div>").addClass("lv_thumbnail_icon lv_thumbnail_icon_" + c.options.thumbnail.icon))
            }, this)), b && this.moveTo(b, !0)
        },
        _getThumbnailsWithinViewport: function () {
            var a = this.position,
                b = [],
                c = this.slide.find(".lv_thumbnail:first").outerWidth();
            if (!a || !c) {
                return b
            }
            var d = E.viewport().width,
                e = Math.ceil(d / c),
                f = Math.floor(Math.max(a - 0.5 * e, 0)),
                g = Math.ceil(Math.min(a + 0.5 * e));
            return Window.views && g > Window.views.length && (g = Window.views.length), this.slider.find(".lv_thumbnail").each(function (a, c) {
                a + 1 >= f && g >= a + 1 && b.push(c)
            }), b
        },
        loadThumbnailsWithinViewport: function () {
            var a = this._getThumbnailsWithinViewport();
            $(a).filter(".lv_load_thumbnail").each($.proxy(function (a, b) {
                var c = $(b).find(".lv_thumbnail_image"),
                    d = $(b).data("thumbnail"),
                    e = d.view;
                $(b).removeClass("lv_load_thumbnail");
                var f, g, h, i, j = e.options.controls;
                if (A.Spinners && (i = j && j.thumbnails && j.thumbnails.spinner)) {
                    c.append(g = $("<div>").addClass("lv_thumbnail_image_spinner_overlay").append(h = $("<div>").addClass("lv_spinner_wrapper"))), f = Spinners.create(h[0], i || {}).play();
                    var k = Spinners.getDimensions(h[0]);
                    h.css(px({
                        height: k.height,
                        width: k.width,
                        "margin-left": Math.ceil(-0.5 * k.width),
                        "margin-top": Math.ceil(-0.5 * k.height)
                    }))
                }
                var l = {
                    width: c.innerWidth(),
                    height: c.innerHeight()
                }, m = Math.max(l.width, l.height);
                Dimensions.preload(d.src, {
                    type: e.type
                }, $.proxy(function (a, b) {
                    var h, d = b.image;
                    if (d.width > l.width && d.height > l.height) {
                        h = Window.Dimensions.scaleWithin({
                            width: m,
                            height: m
                        }, a);
                        var i = 1,
                            j = 1;
                        h.width < l.width && (i = l.width / h.width), h.height < l.height && (j = l.height / h.height);
                        var k = Math.max(i, j);
                        k > 1 && (h.width *= k, h.height *= k), $.each("width height".split(" "), function (a, b) {
                            h[b] = Math.round(h[b])
                        })
                    } else {
                        h = Window.Dimensions.scaleWithin(d.width < l.width || d.height < l.height ? {
                            width: m,
                            height: m
                        } : l, a)
                    }
                    var n = Math.round(0.5 * l.width - 0.5 * h.width),
                        o = Math.round(0.5 * l.height - 0.5 * h.height),
                        p = $("<img>").attr({
                            src: b.image.src
                        }).css(px(h)).css(px({
                            top: o,
                            left: n
                        }));
                    c.prepend(p), g ? g.fadeOut(e.options.effects.thumbnails.load, function () {
                        f && (f.remove(), f = null, g.remove())
                    }) : p.css({
                        opacity: 0
                    }).fadeTo(e.options.effects.thumbnails.load, 1)
                }, this))
            }, this))
        },
        show: function () {
            this.elements.add(Window.buttonTopClose).add(this.close).hide();
            var a = this.elements,
                b = Window.options.controls,
                c = b && b.close;
            switch (c) {
                case "top":
                    a = a.add(this.close);
                    break;
                case "relative":
                    a = a.add(Window.buttonTopClose)
            }
            Window.refreshPreviousNext(), a.show(), Window.views && 1 >= Window.views.length || this.element.stop(1, 0).fadeTo(Window.options.effects.thumbnails.show, 1)
        },
        hide: function () {
            this.elements.add(Window.buttonTopClose).add(this.close).hide(), this.element.stop(1, 0).fadeOut(Window.options.effects.thumbnails.hide)
        },
        _previous: function () {
            if (!(1 > this.position)) {
                var a = this.position - 1;
                this.setActive(a), this.setPosition(a), Window.setPosition(a)
            }
        },
        _next: function () {
            if (!(this.position + 1 > Window.views.length)) {
                var a = this.position + 1;
                this.setActive(a), this.setPosition(a), Window.setPosition(a)
            }
        },
        adjustToViewport: function () {
            var a = E.viewport();
            this.slider.css({
                width: a.width + "px"
            })
        },
        setPosition: function (a) {
            var b = 0 > this.position;
            1 > a && (a = 1);
            var c = this.itemCount();
            a > c && (a = c), this.position = a, this.setActive(a), Window.refreshPreviousNext(), this.moveTo(a, b)
        },
        moveTo: function (a, b) {
            this.adjustToViewport();
            var c = E.viewport(),
                d = c.width,
                e = this.slide.find(".lv_thumbnail").outerWidth(),
                g = 0.5 * d + -1 * (e * (a - 1) + 0.5 * e);
            this.slide.stop(1, 0).animate({
                left: g + "px"
            }, b ? 0 : Window.options.effects.thumbnails.slide, $.proxy(function () {
                this.loadThumbnailsWithinViewport()
            }, this))
        },
        setActive: function (a) {
            var b = this.slide.find(".lv_thumbnail").removeClass("lv_thumbnail_active");
            a && $(b[a - 1]).addClass("lv_thumbnail_active")
        },
        refresh: function () {
            this.position && this.setPosition(this.position)
        },
        itemCount: function () {
            return this.slide.find(".lv_thumbnail").length || 0
        }
    }, G.Relative = {
        create: function () {
            this.Slider.create(), this.elements = $(this.Slider.element).add(Window.sideButtonsUnderneath).add(Window.sideButtonsUnderneath.find(".lv_side_left")).add(Window.sideButtonsUnderneath.find(".lv_side_right")).add(Window.innerPreviousNextOverlays).add(Window.innerPreviousNextOverlays.find(".lv_button"))
        },
        show: function () {
            this.hide();
            var a = this.elements,
                b = Window.options.controls,
                c = b && b.close;
            switch (c) {
                case "top":
                    a = a.add(G.Top.close);
                    break;
                case "relative":
                    a = a.add(Window.buttonTopClose)
            }
            a.show(), Window.refreshPreviousNext(), (Window.view && Window.views.length > 1 && Window.mayPrevious() || Window.mayNext()) && this.Slider.show()
        },
        hide: function () {
            this.elements.add(G.Top.close).add(Window.buttonTopClose).hide()
        },
        refresh: function () {
            this.Slider.refresh()
        },
        play: function () {
            this.Slider.play()
        },
        stop: function () {
            this.Slider.stop()
        }
    }, G.Relative.Slider = {
        setOptions: function () {
            var a = Window.options,
                b = a.controls && a.controls.slider || {};
            this.options = {
                items: b.items || 5,
                duration: a.effects && a.effects.slider && a.effects.slider.slide || 100,
                slideshow: a.slideshow
            }
        },
        create: function () {
            $(Window.element).append(this.element = $("<div>").addClass("lv_controls_relative").append(this.slider = $("<div>").addClass("lv_slider").append(this.slider_previous = $("<div>").addClass("lv_slider_icon lv_slider_previous").append($("<div>").addClass("lv_icon").data("side", "previous"))).append(this.slider_numbers = $("<div>").addClass("lv_slider_numbers").append(this.slider_slide = $("<div>").addClass("lv_slider_slide"))).append(this.slider_next = $("<div>").addClass("lv_slider_icon lv_slider_next").append($("<div>").addClass("lv_icon").data("side", "next"))).append(this.slider_slideshow = $("<div>").addClass("lv_slider_icon lv_slider_slideshow").append($("<div>").addClass("lv_icon lv_slider_next"))))), this.element.hide(), this.count = 0, this.position = 1, this.page = 1, this.setOptions(), this.startObserving()
        },
        startObserving: function () {
            this.slider_slide.delegate(".lv_slider_number", "click", $.proxy(function (a) {
                a.preventDefault(), a.stopPropagation();
                var b = parseInt($(a.target).html());
                this.setActive(b), Window.stop(), Window.setPosition(b)
            }, this)), $.each("previous next".split(" "), $.proxy(function (a, b) {
                this["slider_" + b].bind("click", $.proxy(this[b + "Page"], this))
            }, this)), this.slider.bind("lightview:mousewheel", $.proxy(function (a, b) {
                Window.options && Window.options.mousewheel && (this.count <= this.options.items || (a.preventDefault(), a.stopPropagation(), this[(b > 0 ? "previous" : "next") + "Page"]()))
            }, this)), this.slider_slideshow.bind("click", $.proxy(function () {
                $(this).hasClass("lv_slider_slideshow_disabled") || Window[Window.States.get("playing") ? "stop" : "play"](!0)
            }, this))
        },
        refresh: function () {
            this.setOptions();
            var a = this.itemCount(),
                b = this.options.items >= a ? a : this.options.items,
                c = $(Window.element).is(":visible");
            if (this.element.css({
                width: "auto"
            }), this.slider[a > 1 ? "show" : "hide"](), !(2 > a)) {
                c || $(Window.element).show();
                var d = $(document.createElement("div")).addClass("lv_slider_number");
                this.slider_slide.append(d);
                var e = d.outerWidth(!0);
                this.nr_width = e, d.addClass("lv_slider_number_last"), this.nr_margin_last = e - d.outerWidth(!0) || 0, d.remove();
                var a = this.itemCount(),
                    b = this.options.items >= a ? a : this.options.items,
                    f = this.count % this.options.items,
                    g = f ? this.options.items - f : 0;
                this.slider_numbers.css({
                    width: this.nr_width * b - this.nr_margin_last + "px"
                }), this.slider_slide.css({
                    width: this.nr_width * (this.count + g) + "px"
                });
                var h = Window.views && $.grep(Window.views, function (a) {
                    return a.options.slideshow
                }).length > 0;
                this.slider_slideshow.hide().removeClass("lv_slider_slideshow_disabled"), h && this.slider_slideshow.show(), this.options.slideshow || this.slider_slideshow.addClass("lv_slider_slideshow_disabled"), this.itemCount() <= this.options.items ? (this.slider_next.hide(), this.slider_previous.hide()) : (this.slider_next.show(), this.slider_previous.show()), this.element.css({
                    width: "auto"
                }), this.slider.css({
                    width: "auto"
                });
                var i = 0,
                    j = jQuery.map($.makeArray(this.slider.children("div:visible")), function (a) {
                        var c = $(a).outerWidth(!0);
                        return Browser.IE && 7 > Browser.IE && (c += (parseInt($(a).css("margin-left")) || 0) + (parseInt($(a).css("margin-right")) || 0)), c
                    });
                $.each(j, function (a, b) {
                    i += b
                }), Browser.IE && 7 > Browser.IE && i++, this.element.css({
                    position: "absolute"
                }), i && this.element.css({
                    width: i + "px"
                }), i && this.slider.css({
                    width: i + "px"
                }), this.element.css({
                    "margin-left": Math.ceil(-0.5 * i) + "px"
                });
                var k = parseInt(this.slider_slide.css("left") || 0),
                    l = this.pageCount(); - 1 * (l - 1) * this.options.items * this.nr_width > k && this.scrollToPage(l, !0), this.refreshButtonStates(), c || $(Window.element).hide(), Window.options && Window.options.controls && !Window.options.controls.slider && this.slider.hide()
            }
        },
        itemCount: function () {
            return this.slider_slide.find(".lv_slider_number").length || 0
        },
        pageCount: function () {
            return Math.ceil(this.itemCount() / this.options.items)
        },
        setActive: function (a) {
            $(this.slider_numbers.find(".lv_slider_number").removeClass("lv_slider_number_active")[a - 1]).addClass("lv_slider_number_active")
        },
        setPosition: function (a) {
            1 > a && (a = 1);
            var b = this.itemCount();
            a > b && (a = b), this.position = a, this.setActive(a), this.scrollToPage(Math.ceil(a / this.options.items))
        },
        refreshButtonStates: function () {
            this.slider_next.removeClass("lv_slider_next_disabled"), this.slider_previous.removeClass("lv_slider_previous_disabled"), 1 > this.page - 1 && this.slider_previous.addClass("lv_slider_previous_disabled"), this.page >= this.pageCount() && this.slider_next.addClass("lv_slider_next_disabled"), this[Window.States.get("playing") ? "play" : "stop"]()
        },
        scrollToPage: function (a, b) {
            this.page == a || 1 > a || a > this.pageCount() || (Browser.MobileSafari && this.slider_numbers.css({
                opacity: 0.999
            }), this.slider_slide.stop(!0).animate({
                left: -1 * this.options.items * this.nr_width * (a - 1) + "px"
            }, b ? 0 : this.options.duration || 0, "linear", $.proxy(function () {
                Browser.MobileSafari && this.slider_numbers.css({
                    opacity: 1
                })
            }, this)), this.page = a, this.refreshButtonStates())
        },
        previousPage: function () {
            this.scrollToPage(this.page - 1)
        },
        nextPage: function () {
            this.scrollToPage(this.page + 1)
        },
        populate: function (a) {
            this.slider_slide.find(".lv_slider_number, .lv_slider_number_empty").remove();
            for (var b = 0; a > b; b++) {
                this.slider_slide.append($("<div>").addClass("lv_slider_number").html(b + 1))
            }
            for (var c = this.options.items, d = a % c ? c - a % c : 0, b = 0; d > b; b++) {
                this.slider_slide.append($("<div>").addClass("lv_slider_number_empty"))
            }
            this.slider_numbers.find(".lv_slider_number, lv_slider_number_empty").removeClass("lv_slider_number_last").last().addClass("lv_slider_number_last"), this.count = a, this.refresh()
        },
        show: function () {
            this.element.show()
        },
        hide: function () {
            this.element.hide()
        },
        play: function () {
            this.slider_slideshow.addClass("lv_slider_slideshow_playing")
        },
        stop: function () {
            this.slider_slideshow.removeClass("lv_slider_slideshow_playing")
        }
    }, G.Top = {
        create: function () {
            if ($(document.body).append(this.element = $("<div>").addClass("lv_controls_top").append(this.middle = $("<div>").addClass("lv_top_middle").hide().append(this.middle_previous = $("<div>").addClass("lv_top_button lv_top_previous").data("side", "previous").append($("<div>").addClass("lv_icon").append(this.text_previous = $("<span>")))).append(this.middle_slideshow = $("<div>").addClass("lv_top_button lv_top_slideshow").append($("<div>").addClass("lv_icon"))).append(this.middle_next = $("<div>").addClass("lv_top_button lv_top_next").data("side", "next").append($("<div>").addClass("lv_icon").append(this.text_next = $("<span>"))))).hide()).append(this.close = $("<div>").addClass("lv_controls_top_close").append(this.close_button = $("<div>").addClass("lv_controls_top_close_button")).hide()), Browser.IE && 7 > Browser.IE) {
                var a = this.element[0].style;
                a.position = "absolute", a.setExpression("top", '((!!window.jQuery && jQuery(window).scrollTop()) || 0) + "px"');
                var b = this.close[0].style;
                b.position = "absolute", b.setExpression("top", '((!!window.jQuery && jQuery(window).scrollTop()) || 0) + "px"')
            }
            this.setOptions(), this.startObserving()
        },
        setOptions: function () {
            this.options = $.extend({
                slideshow: !0,
                text: {
                    previous: "Prev",
                    next: "Next"
                },
                close: !0
            }, Window.options && Window.options.controls || {}), this.setText()
        },
        setSkin: function (a) {
            var b = {
                element: "lv_controls_top_skin_",
                close: "lv_controls_top_close_skin_"
            };
            $.each(b, $.proxy(function (b, c) {
                var d = this[b];
                $.each((d[0].className || "").split(" "), function (a, b) {
                    b.indexOf(c) > -1 && d.removeClass(b)
                }), d.addClass(c + a)
            }, this))
        },
        setText: function () {
            this.text_previous.hide(), this.text_next.hide(), this.options.text && (this.text_previous.html(this.options.text.previous).show(), this.text_next.html(this.options.text.next).show())
        },
        startObserving: function () {
            this.middle_previous.bind("click", function () {
                Window.stop(), Window.previous(), $(this).blur()
            }), this.middle_slideshow.bind("click", function () {
                $(this).find(".lv_icon_disabled").length > 0 || Window[Window.States.get("playing") ? "stop" : "play"](!0)
            }), this.middle_next.bind("click", function () {
                Window.stop(), Window.next(), $(this).blur()
            }), this.close_button.bind("click", function () {
                Window.hide()
            }), this.element.add(this.close).bind("lightview:mousewheel", $.proxy(function (a) {
                (!Window.options || !Window.options.mousewheel || Window.options && Window.options.viewport) && (a.preventDefault(), a.stopPropagation())
            }, this))
        },
        show: function () {
            var a = this.element,
                b = Window.options.controls,
                c = b && b.close;
            switch (c) {
                case "top":
                    a = a.add(this.close);
                    break;
                case "relative":
                    a = a.add(Window.buttonTopClose)
            }
            a.show()
        },
        hide: function () {
            this.element.hide(), this.close.hide()
        },
        refresh: function () {
            this.setOptions(), this.element.find(".lv_icon_disabled").removeClass("lv_icon_disabled"), Window.mayPrevious() || this.middle_previous.find(".lv_icon").addClass("lv_icon_disabled"), Window.options.slideshow || this.middle_slideshow.find(".lv_icon").addClass("lv_icon_disabled"), Window.mayNext() || this.middle_next.find(".lv_icon").addClass("lv_icon_disabled"), this.element.removeClass("lv_controls_top_with_slideshow");
            var a = Window.views && $.grep(Window.views, function (a) {
                return a.options.slideshow
            }).length > 0;
            a && this.element.addClass("lv_controls_top_with_slideshow"), this.element["top" == G.type && Window.views.length > 1 ? "show" : "hide"](), this[Window.States.get("playing") ? "play" : "stop"]()
        },
        play: function () {
            this.middle_slideshow.addClass("lv_top_slideshow_playing")
        },
        stop: function () {
            this.middle_slideshow.removeClass("lv_top_slideshow_playing")
        }
    }, Window.updateQueue = function () {
        function a() {
            $(document.body).append($(document.createElement("div")).addClass("lv_update_queue").append($("<div>").addClass("lv_window").append(this.container = $("<div>").addClass("lv_content"))))
        }
        function b(a) {
            return {
                width: $(a).innerWidth(),
                height: $(a).innerHeight()
            }
        }
        function c(a) {
            var c = b(a),
                d = a.parentNode;
            return d && $(d).css({
                width: c.width + "px"
            }) && b(a).height > c.height && c.width++, $(d).css({
                width: "100%"
            }), c
        }
        function d(a, b, c) {
            this.container || this.build(), $.extend({
                spinner: !1
            }, arguments[3] || {}), (b.options.inline || _.isElement(a)) && (b.options.inline && "string" == $.type(a) && (a = $("#" + a)[0]), !Window.inlineMarker && a && _.element.isAttached(a) && ($(a).data("lv_restore_inline_display", $(a).css("display")), Window.inlineMarker = document.createElement("div"), $(a).before($(Window.inlineMarker).hide())));
            var e = document.createElement("div");
            this.container.append($(e).addClass("lv_content_wrapper").append(a)), _.isElement(a) && $(a).show(), b.options.wrapperClass && $(e).addClass(b.options.wrapperClass), b.options.skin && $(e).addClass("lv_content_" + b.options.skin);
            var f = $(e).find("img[src]").filter(function () {
                return !($(this).attr("height") && $(this).attr("width"))
            });
            if (f.length > 0) {
                Window.States.set("preloading_images", !0);
                var g = 0,
                    h = b.url,
                    i = Math.max(8000, 750 * (f.length || 0));
                Window.Timeouts.clear("preloading_images"), Window.Timeouts.set("preloading_images", $.proxy(function () {
                    f.each(function () {
                        this.onload = function () {}
                    }), g >= f.length || Window.view && Window.view.url != h || this._update(e, b, c)
                }, this), i), Window.States.set("preloading_images", f), $.each(f, $.proxy(function (a, d) {
                    var i = new Image;
                    i.onload = $.proxy(function () {
                        i.onload = function () {};
                        var a = i.width,
                            j = i.height,
                            k = $(d).attr("width"),
                            l = $(d).attr("height");
                        if (k && l || (!k && l ? (a = Math.round(l * a / j), j = l) : !l && k && (j = Math.round(k * j / a), a = k), $(d).attr({
                            width: a,
                            height: j
                        })), g++, g == f.length) {
                            if (Window.Timeouts.clear("preloading_images"), Window.States.set("preloading_images", !1), Window.view && Window.view.url != h) {
                                return
                            }
                            this._update(e, b, c)
                        }
                    }, this), i.src = d.src
                }, this))
            } else {
                this._update(e, b, c)
            }
        }
        function e(a, b, d) {
            var e = c(a);
            e = f(a, e, b), Window.resizeTo(e.width, e.height, {
                complete: function () {
                    Window.content.html(a), d && d()
                }
            })
        }
        function f(a, b, d) {
            var e = {
                width: b.width - (parseInt($(a).css("padding-left")) || 0) - (parseInt($(a).css("padding-right")) || 0),
                height: b.height - (parseInt($(a).css("padding-top")) || 0) - (parseInt($(a).css("padding-bottom")) || 0)
            }, f = Window.options.width;
            if (f && "number" == $.type(f) && e.width > f && ($(a).css({
                width: f + "px"
            }), b = c(a)), b = Window.Dimensions.fit(b, d), /(inline|ajax|html)/.test(d.type) && Window.States.get("resized")) {
                var g = $("<div>");
                g.css({
                    position: "absolute",
                    top: 0,
                    left: 0,
                    width: "100%",
                    height: "100%"
                }), $(a).append(g);
                var h = g.innerWidth();
                $(a).css(px(b)).css({
                    overflow: "auto"
                });
                var i = g.innerWidth(),
                    j = h - i;
                j && (b.width += j, $(a).css(px(b)), b = Window.Dimensions.fit(b, d)), g.remove()
            }
            return b
        }
        return {
            build: a,
            update: d,
            _update: e,
            getFittedDimensions: f,
            getMeasureElementDimensions: c
        }
    }(), $.extend(!0, Lightview, function () {
        function show(d) {
            var e = arguments[1] || {}, position = arguments[2];
            arguments[1] && "number" == $.type(arguments[1]) && (position = arguments[1], e = D.create({}));
            var f = [],
                object_type;
            switch (object_type = $.type(d)) {
                case "string":
                case "object":
                    var g = new View(d, e);
                    if (g.group) {
                        if (d && 1 == d.nodeType) {
                            var h = $('.lightview[data-lightview-group="' + $(d).data("lightview-group") + '"]'),
                                j = {};
                            h.filter("[data-lightview-group-options]").each(function (i, a) {
                                $.extend(j, eval("({" + ($(a).attr("data-lightview-group-options") || "") + "})"))
                            }), h.each(function (a, b) {
                                position || b != d || (position = a + 1), f.push(new View(b, $.extend({}, j, e)))
                            })
                        }
                    } else {
                        var j = {};
                        d && 1 == d.nodeType && $(d).is("[data-lightview-group-options]") && ($.extend(j, eval("({" + ($(d).attr("data-lightview-group-options") || "") + "})")), g = new View(d, $.extend({}, j, e))), f.push(g)
                    }
                    break;
                case "array":
                    $.each(d, function (a, b) {
                        var c = new View(b, e);
                        f.push(c)
                    })
            }(!position || 1 > position) && (position = 1), position > f.length && (position = f.length), Window.load(f, position, {
                initialDimensionsOnly: !0
            }), Window.show(function () {
                Window.setPosition(position)
            })
        }
        function refresh() {
            return Window.refresh(), this
        }
        function setDefaultSkin(a) {
            return Window.setDefaultSkin(a), this
        }
        function hide() {
            return Window.hide(), this
        }
        function play(a) {
            return Window.play(a), this
        }
        function stop() {
            return Window.stop(), this
        }
        return {
            show: show,
            hide: hide,
            play: play,
            stop: stop,
            refresh: refresh,
            setDefaultSkin: setDefaultSkin
        }
    }()), A.Lightview = Lightview, $(document).ready(function () {
        Lightview.init()
    })
})(jQuery, window);;