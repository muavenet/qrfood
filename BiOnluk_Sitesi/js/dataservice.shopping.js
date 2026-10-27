(function (OrdersApp) {
    "use strict";
    OrdersApp.shoppingDataService = {
        getProducts : function (callback) {
            OrdersApp.ajaxService.ajaxGetJson("GetProducts", null, callback);
        },        
        removeSepets : function (id) {
            OrdersApp.ajaxService.ajaxPostJson("removeSepets", id , null);
        },
        updateSepet: function (id,adet) {

            var data = { id: id.id, adet : id.adet};         

            OrdersApp.ajaxService.ajaxPostJson("updateSepet", data, null);
        },
        updateSepetNot: function (id, not) {

            var data = { id: id.id, not: id.not };

            OrdersApp.ajaxService.ajaxPostJson("updateSepetNot", data, null);
        },
        getUrunDetay: function (id) {
            OrdersApp.ajaxService.ajaxPostJson("getUrunDetay", id , null);
		},
		siparisGir: function (id) {
			OrdersApp.ajaxService.ajaxPostJsonHref("siparisGir", id);
        },
        siparisGirKapidaOdeme: function () {
            var data = { siparisId: 0, Havale: true };
            OrdersApp.ajaxService.ajaxPostJsonHrefURL("SiparisiTamamla/OdemeSayfasi", data);
        }
    };    
}(OrdersApp));