$(function () {

    OrdersApp.formatCurrency = function (value) {
        return value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1.') + " TL";       
    };


    OrdersApp.Sepet = function () {
      
        var self = this;

        this.id = ko.observable();
		this.IlanId = ko.observable();
		this.quantity = ko.observable();
		this.teslimatSuresi = ko.observable();
		this.IlanAdi = ko.observable();
		this.AySayisi = ko.observable();
		this.IlanAciklama = ko.observable();
        this.fiyat = ko.observable();
		this.Url = ko.observable();
		this.myClass = ko.observable();
		this.IlanFoto = ko.observable();
		this.Adet = ko.observable();
		this.Not = ko.observable();
		

		this.extPrice = ko.computed(function () {
			
				return this.id ? this.fiyat() * this.quantity() : 0;
        }, self);

    };

    OrdersApp.vm = function () {
     
        sepets = ko.observableArray([]),

        sortFunction = function (a, b) {
            return a.shortDesc().toLowerCase() > b.shortDesc().toLowerCase() ? 1 : -1;
        },


			removeFromCart = function (sepetItem) {

				if (sepets().indexOf(sepetItem) > -1) {

					sepets.remove(sepetItem);

					OrdersApp.shoppingDataService.removeSepets({ id: sepetItem.id });
				}
			},


			IlansiparisGir = function (sepetItem) {

				if (sepets().indexOf(sepetItem) > -1) {

					OrdersApp.shoppingDataService.siparisGir({ id: sepetItem.id, adet: sepetItem.quantity });
				}
			},

			siparisGirKapidaOdeme = function (sepetItem) {

				if (sepets().indexOf(sepetItem) > -1) {

					OrdersApp.shoppingDataService.siparisGirKapidaOdeme();
				}
			},
			

			placeOrder = function () {

			$.each(sepets(), function () {
					OrdersApp.shoppingDataService.updateSepet({ id: this.id(), adet: this.quantity() });

				});


				//window.location.href="/SiparisiTamamla";
			},


			placeOrderSepetNot = function () {

				$.each(sepets(), function () {
					OrdersApp.shoppingDataService.updateSepetNot({ id: this.id(), not: this.Not() });

				});


				//window.location.href="/SiparisiTamamla";
			},

          placeOrderCallback = function (json) {
              dialogOptions.title("Place Order").text(json.message).open(true);
          },

		 grandTotal = ko.computed(function () {

				var total = 0;
				$.each(sepets(), function () {
					total += this.extPrice();
				});
				return total;
		 }),

			ToplamSepetAdet = ko.computed(function () {

				var total = 0;
				$.each(sepets(), function () {
					total++;
				});
				return total + " ITEMS";
			}),

			loadProductsCallback = function (json) {

			//Başlangıçta çalışıyor
			$.each(json, function (i, p) {

					sepets.push(new OrdersApp.Sepet()
						.id(p.Id)
						.IlanId(p.ilanId)
						.IlanAdi(p.IlanAdi)
						.IlanAciklama(p.IlanAciklama)
						.teslimatSuresi(p.teslimatSuresi)						
						.fiyat(p.Fiyat)
						.quantity(p.Adet)					
						.Url("/UrunDetay/Index/" + p.IlanId)
						.IlanFoto(p.ilanFotosu)
						.Not(p.Not)
							
				);
				
			});
		
				//sepets.sort(sortFunction);
			},

            loadProducts = function () {
                //Başlangıçta çalışıyor
                OrdersApp.shoppingDataService.getProducts(OrdersApp.vm.loadProductsCallback);
			};

        return {
          
            sepets: sepets,
            loadProductsCallback: loadProductsCallback,
            loadProducts: loadProducts,
            removeFromCart: removeFromCart,
            placeOrderCallback: placeOrderCallback,
			placeOrder: placeOrder,
			IlansiparisGir: IlansiparisGir,
			siparisGirKapidaOdeme: siparisGirKapidaOdeme,
			placeOrderSepetNot: placeOrderSepetNot
          
        }

    }();

    OrdersApp.vm.loadProducts();

    ko.applyBindings(OrdersApp.vm);


});