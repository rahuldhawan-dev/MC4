var href = window.location.href;
var isHttps = (href.indexOf("https") > -1);
if (href.indexOf('contractors.mapcall.') > 0 && !isHttps) {
  window.location = href.replace('http://', 'https://');
}
