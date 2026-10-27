using HemenTanitim.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

namespace Iyzico.MVC.Helpers
{
    public class IyzicoHelper : IyzicoSample
    {
        protected Options options;

        HemenTanitimContext db = new HemenTanitimContext();

        public void Should_Create_Marketplace_Payment_Onay(string conId)
        {
            options = new Options();

            options.ApiKey = "sandbox-dD07FHwAEnczmUPLtPIXQ2pCgoTKk00r";
            options.SecretKey = "sandbox-56AKyFinLJ0H0wfcT0XYTBUMc02hFv8j";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreateApprovalRequest request = new CreateApprovalRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = conId;
            request.PaymentTransactionId = "1";

            Approval approval = Approval.Create(request, options);

        }

        public string[] Should_Create_Payment(string tutar, ApplicationUser user, List<Sepet> sepet, string siparisId)
        {

            options = new Options();
            
            options.ApiKey = "sandbox-dD07FHwAEnczmUPLtPIXQ2pCgoTKk00r";
            options.SecretKey = "sandbox-56AKyFinLJ0H0wfcT0XYTBUMc02hFv8j";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";


            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = siparisId;
            request.Price = tutar;
            request.PaidPrice = tutar;
            request.Currency = Currency.TRY.ToString();
            request.BasketId = siparisId;
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "http://demo.infratech.com.tr/SiparisiTamamla/sprRedirect?siparisId_=" + siparisId;

            List<int> enabledInstallments = new List<int>();
            enabledInstallments.Add(2);
            enabledInstallments.Add(3);
            enabledInstallments.Add(6);
            enabledInstallments.Add(9);
            request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = user.Id.ToString();
            buyer.Name = user.UserName;
            buyer.Surname = "Soyad Gelecek";
            buyer.GsmNumber = user.Telefon;
            buyer.Email = "Mailyok@gmail.com";
            buyer.IdentityNumber = "012346";
            buyer.LastLoginDate = user.SonGorulmeTarihi.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationDate = user.UyelikTarihi.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationAddress = "Adres Yok";

            buyer.Ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            buyer.City = user.Konum.ToString();
            buyer.Country = "Turkey";
            //buyer.ZipCode = user;
            request.Buyer = buyer;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;


            List<BasketItem> basketItems = new List<BasketItem>();

            BasketItem firstBasketItem = new BasketItem();

            //BU SEPET DOĞRU ÇALIŞMIYOR OLABİLİR
            foreach (var item in sepet)
            {
                firstBasketItem.Id = siparisId.ToString();
                firstBasketItem.Category1 = item.IlanAdi;
                firstBasketItem.Name = item.IlanAdi;
                firstBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
                firstBasketItem.Price = tutar;
            }
            basketItems.Add(firstBasketItem);
            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);

            PrintResponse<CheckoutFormInitialize>(checkoutFormInitialize);

            //Assert.AreEqual(Status.SUCCESS.ToString(), checkoutFormInitialize.Status);
            //Assert.AreEqual(Locale.TR.ToString(), checkoutFormInitialize.Locale);
            //Assert.AreEqual(siparisId, checkoutFormInitialize.ConversationId);
            //Assert.IsNotNull(checkoutFormInitialize.SystemTime);
            //Assert.IsNull(checkoutFormInitialize.ErrorCode);
            //Assert.IsNull(checkoutFormInitialize.ErrorMessage);
            //Assert.IsNull(checkoutFormInitialize.ErrorGroup);
            //Assert.IsNotNull(checkoutFormInitialize.CheckoutFormContent);
            //Assert.IsNotNull(checkoutFormInitialize.PaymentPageUrl);

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

            options.ApiKey = "sandbox-dD07FHwAEnczmUPLtPIXQ2pCgoTKk00r";
            options.SecretKey = "sandbox-56AKyFinLJ0H0wfcT0XYTBUMc02hFv8j";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";


            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.ConversationId = convId;
            request.Token = tok;

            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);


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