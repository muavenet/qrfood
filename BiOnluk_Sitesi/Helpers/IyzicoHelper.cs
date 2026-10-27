using HemenTanitim.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Iyzico.MVC.Helpers
{
    public class IyzicoHelper : IyzicoSample
    {
        protected Options options;

   

        public void Should_Create_Personal_Sub_Merchant()
        {
            options = new Options();
            options.ApiKey = "sandbox-ShPCvKbmR4WFis53kkeYFe9fCzpND9yl";
            options.SecretKey = "sandbox-aK4Qo9GmI0Txv1BafLOtYeGRVsBy65aJ";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreateSubMerchantRequest request = new CreateSubMerchantRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.SubMerchantExternalId = "B49224";
            request.SubMerchantType = SubMerchantType.PERSONAL.ToString();
            request.Address = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            request.ContactName = "John";
            request.ContactSurname = "Doe";
            request.Email = "email@submerchantemail.com";
            request.GsmNumber = "+905350000000";
            request.Name = "John's market";
            request.Iban = "TR180006200119000006672315";
            request.IdentityNumber = "31300864726";
            request.Currency = Currency.TRY.ToString();

            SubMerchant subMerchant = SubMerchant.Create(request, options);

            PrintResponse<SubMerchant>(subMerchant);

            Assert.AreEqual(Status.SUCCESS.ToString(), subMerchant.Status);
            Assert.AreEqual(Locale.TR.ToString(), subMerchant.Locale);
            Assert.AreEqual("123456789", subMerchant.ConversationId);
            Assert.IsNotNull(subMerchant.SystemTime);
            Assert.IsNull(subMerchant.ErrorCode);
            Assert.IsNull(subMerchant.ErrorMessage);
            Assert.IsNull(subMerchant.ErrorGroup);
        }

        public void Should_Create_Marketplace_Payment()
        {

            options = new Options();
            options.ApiKey = "sandbox-ShPCvKbmR4WFis53kkeYFe9fCzpND9yl";
            options.SecretKey = "sandbox-aK4Qo9GmI0Txv1BafLOtYeGRVsBy65aJ";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "John Doe";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2020";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            firstBasketItem.SubMerchantKey = "sub merchant key";
            firstBasketItem.SubMerchantPrice = "0.27";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            secondBasketItem.SubMerchantKey = "sub merchant key";
            secondBasketItem.SubMerchantPrice = "0.42";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            thirdBasketItem.SubMerchantKey = "vSyVJA4phM0IG8iygud4h9nZqRw=";
            thirdBasketItem.SubMerchantPrice = "0.18";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);

            PrintResponse<Payment>(payment);

            Assert.AreEqual(Status.SUCCESS.ToString(), payment.Status);
            Assert.AreEqual(Locale.TR.ToString(), payment.Locale);
            Assert.AreEqual("123456789", payment.ConversationId);
            Assert.IsNotNull(payment.SystemTime);
            Assert.IsNull(payment.ErrorCode);
            Assert.IsNull(payment.ErrorMessage);
            Assert.IsNull(payment.ErrorGroup);
        }


        public string[] Should_Create_Payment(string miktar, string siparisId, string UrunSahibiId)
        {

            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = siparisId;
            request.Price = "1";
            request.PaidPrice = miktar;
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "http://seotanitim.esiluet.com//SiparisiTamamla/OdemeSonucSayfasi";

            List<int> enabledInstallments = new List<int>();
            enabledInstallments.Add(2);
            enabledInstallments.Add(3);
            enabledInstallments.Add(6);
            enabledInstallments.Add(9);
            request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);

            PrintResponse<CheckoutFormInitialize>(checkoutFormInitialize);

            Assert.AreEqual(Status.SUCCESS.ToString(), checkoutFormInitialize.Status);
            Assert.AreEqual(Locale.TR.ToString(), checkoutFormInitialize.Locale);
            Assert.AreEqual(siparisId, checkoutFormInitialize.ConversationId);
            Assert.IsNotNull(checkoutFormInitialize.SystemTime);
            Assert.IsNull(checkoutFormInitialize.ErrorCode);
            Assert.IsNull(checkoutFormInitialize.ErrorMessage);
            Assert.IsNull(checkoutFormInitialize.ErrorGroup);
            Assert.IsNotNull(checkoutFormInitialize.CheckoutFormContent);
            Assert.IsNotNull(checkoutFormInitialize.PaymentPageUrl);

            if (!String.IsNullOrEmpty(checkoutFormInitialize.CheckoutFormContent))
            {
                string[] array = new string[3];
                array[0] = checkoutFormInitialize.CheckoutFormContent;
                array[1] = checkoutFormInitialize.Token;
                array[2] = checkoutFormInitialize.ConversationId;

                return array; 
            }
            else
            {

                string[] array = new string[3];
                array[0] = checkoutFormInitialize.ErrorCode;

                return array;
            }

            
        }

        public bool Should_Retrieve_Checkout_Form_Result(string convId, string tok)
        {
            options = new Options();
            options.ApiKey = "sandbox-mYyV7Ou8TOzmxAlbcaghDcBckLyIMVMM";
            options.SecretKey = "sandbox-M0iK7cURwR4PF6yU8LkBnl7Xgw6i8ohO";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.ConversationId = convId;
            request.Token = tok;

            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);

            PrintResponse<CheckoutForm>(checkoutForm);

            Assert.AreEqual(Status.SUCCESS.ToString(), checkoutForm.PaymentId);
            Assert.AreEqual(Locale.TR.ToString(), checkoutForm.Locale);
            Assert.AreEqual(convId, checkoutForm.ConversationId);
            Assert.IsNotNull(checkoutForm.SystemTime);
            Assert.IsNull(checkoutForm.ErrorCode);
            Assert.IsNull(checkoutForm.ErrorMessage);
            Assert.IsNull(checkoutForm.ErrorGroup);



            if (checkoutForm.Status == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
         
        }


        

        //public string CreateToken(string amount, string name,string transactionId)
        //     parametre olarak miktar ve müşteri adını aldık, eğer isterseniz tüm bilgileri parametre olarak yazabilirsiniz
        //{
        //    string result = "";       
        //    using (WebClient client = new WebClient())
        //    {
        //        byte[] response =
        //        client.UploadValues("https://sandboxapi.iyzipay.com/", new NameValueCollection()
        //        {
        //              { "api_id", "im05037140016a6bae1ce31443402842"},
        //              { "secret", "im05301400031fcea28cae1443402842"},
        //              { "external_id",transactionId},
        //              { "mode", "test"}, //test işlemleri için "test", gerçek ortam için "live" yazmalısınız
        //              { "type", "CC.DB"},
        //              { "return_url", "http://eticaret.dotnetbilisim.com/SiparisiTamamla/OdemeSayfasi"}, //kartla ödeme yapıldığında ödemen alındımı yoksa hatamı olduğu dönülecek olan site adresiniz
        //              { "amount", amount}, //sepet miktarı - bu bilgiyi örneğin 125.66 şeklinde yollamalısınız aksi halde hata alırsınız
        //              { "currency", "TRY"}, //döviz cinsi
        //              { "descriptor", "AÇIKLAMA ALANI"},//buradan itibaren aşağıdaki alanları sadece site sahibi görecek, kullanıcı görmeyeceği için istediğiniz gibi doldurabilirsiniz
        //              { "item_id_1", "1"},
        //              { "item_name_1", "ÜRÜN ADI"},
        //              { "item_unit_quantity_1", "2"},
        //              { "item_unit_amount_1", "3"},
        //              { "customer_first_name",  "xx"}, //müşteri adı - ben bu bilgileri direkt firma adı girerek geçtim
        //              { "customer_last_name",  "xxx"}, //müşteri soyadı
        //              { "customer_company_name",  "xx"}, // müşteri şirket adı
        //              { "customer_shipping_address_line_1", "ADRES 1"},
        //              { "customer_shipping_address_line_2", "ADRES 2"},
        //              { "customer_shipping_address_zip", "34250"},
        //              { "customer_shipping_address_city", "ŞEHİR"},
        //              { "customer_shipping_address_state", "İLÇE"},
        //              { "customer_shipping_address_country", "ÜLKE"},
        //              { "customer_billing_address_line_1", "FATURA ADRES 1"},
        //              { "customer_billing_address_line_2", "FATURA ADRES 2"},
        //              { "customer_billing_address_zip", "123"},
        //              { "customer_billing_address_city", "FATURA ŞEHİR"},
        //              { "customer_billing_address_state", "FATURA İLÇE"},
        //              { "customer_billing_address_country", "FATURA ÜLKE"},
        //              { "customer_contact_phone", "21261533880"},
        //              { "customer_contact_mobile", "5373666046"},
        //              { "customer_contact_email", "mevamet@gmail.com"},
        //              { "customer_contact_ip", System.Web.HttpContext.Current.Request.UserHostAddress}, //müşteri IP adresi
        //              { "customer_language", "tr"}, // müşteri dili
        //              { "installment", "true"} //eğer taksit yapacaksanız true, yapılmayacaksa false değer atayın
        //        });

        //        result = System.Text.Encoding.UTF8.GetString(response); //yukarıdaki bilgileri iyzico'ya post ettik ve geriye bir sonuç aldık, geri dönen sonucu breakpoint ile inceleyin eğer bir hata yaptıysanız burada hatanızı göreceksiniz, hata yoksa içinde token olan bir json sonuç dönecektir.
        //    }


        //    dynamic data = JObject.Parse(result); //geri dönen json datayı parse ettik

        //    return data.transaction_token; //bu json data içinden bize sadece token gerektiği için bu bilgiyi alıp geri döndürdük.

         
        //}

        //public bool IsSuccess(string response, out string errorMessage) //iyzico'dan aldığımız sonucu metoda paslayacağız ve sonucu geri alıp kontrol edeceğiz
        //{
        //    dynamic data = JObject.Parse(response); //iyzico tarafından gelen datayı parse ettik

        //    if (data.response.state == "success") //eğer gelen data içinde success yazıyorsa işlem başarılı
        //    {
        //        errorMessage = ""; //hata mesajı yok
        //        return true; //geriye true döndük.
        //    }
        //    else
        //    {
        //        errorMessage = "Ödeme Alınamadı !" + "<br /><br />" + data.response.error_message; //gelen datada success bilgisi yok yani ödeme alınamadı, data içindeki hata bilgisini geriye döndük
        //        return false;
        //    }
        //}
    }
}