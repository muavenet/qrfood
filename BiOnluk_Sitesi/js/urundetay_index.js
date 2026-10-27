$(function () {

    OrdersApp.Sepet = function () {

        var self = this;

        this.urunId = ko.observable();      
        this.urunadi = ko.observable();
        this.fiyat = ko.observable();
      
    };

    OrdersApp.vm = function () {

        urunlers = ko.observableArray([]),

        //SepeteAt = function (sepetItem) {

        //    OrdersApp.shoppingDataService.addUrun({ urunadi: sepetItem.urunadi, urunid: sepetItem.urunId });
        //},
        
        loadProductsCallback = function () {
                urunlers.push(new OrdersApp.Sepet()                        
                        .urunId(p.UrunId)
                        .urunadi(p.UrunAdi)
                        .fiyat(p.Fiyat)
                      );
           
        },

            loadProducts = function () {
              
                OrdersApp.shoppingDataService.getUrunDetay(OrdersApp.vm.loadProductsCallback);
            }

        return {

            sepets: sepets,
            loadProductsCallback: loadProductsCallback,
            loadProducts: loadProducts,
        }

    }();

    OrdersApp.vm.loadProducts();

    ko.applyBindings(OrdersApp.vm);
});