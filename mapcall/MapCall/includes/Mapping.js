Type.registerNamespace("MMSINC");
Type.registerNamespace("MMSINC.Mapping");

MMSINC.Mapping.LatLonPicker = function (txtLat, txtLon) {
    this._txtLat = txtLat[0]; // txtLat and txtLon are both jQuery objects. Or should be anyway.
    this._txtLon = txtLon[0];
    this.href = '/public/EsriPicker.aspx';

    return true;
};

MMSINC.Mapping.LatLonPicker.prototype = {
    show: function () {
        Lightview.show({
            url: this.href,
            title: 'To select a new location, select the desired point on the map and click save.',
            options: {
                autosize: false,
                fullscreen: true,
                resizeDuration: 0,
                onShow: function () {
                    document.lightview = Lightview;
                },
                onHide: function () {
                    this.lightview = null;
                    this.txtLat = null;
                    this.txtLon = null;
                }
            }
        });
        document.txtLat = this._txtLat;
        document.txtLon = this._txtLon;
    }
};

MMSINC.Mapping.LatLonPicker.registerClass('MMSINC.Mapping.LatLonPicker', null, Sys.IDisposable);

MMSINC.Mapping.IconPicker = function (img, hid) {
    this._img = img;
    this._hid = hid;
    this.href = '/Modules/Maps/IconPicker.aspx';

    return true;
};

MMSINC.Mapping.IconPicker.prototype = {
    show: function () {
        Lightview.show({
            url: this.href,
            autosize: true,
            options: {
                resizeDuration: 0,
                onShow: function() {
                    document.lightview = Lightview;
                },
                onHide: function() {
                    document.lightview = null;
                    this._img = null;
                    this._hid = null;
                }
            }
        });
        document.img = this._img[0];
        document.hid = this._hid[0];
    }
};

MMSINC.Mapping.IconPicker.registerClass('MMSINC.Mapping.IconPicker', null, Sys.IDisposable);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
