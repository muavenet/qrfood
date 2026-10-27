using System.Web;
using System.Web.Optimization;

namespace HemenTanitim
{  
    public class BundleConfig
    {
      
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
			bundles.IgnoreList.Clear();

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
						"~/Scripts/kendo/kendo.all.min.js",
						// uncomment below if using the Scheduler
						// "~/Scripts/kendo/kendo.timezones.min.js",
						"~/Scripts/kendo/kendo.aspnetmvc.min.js"));

			bundles.IgnoreList.Clear();

			bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
						"~/Content/kendo/kendo.common-bootstrap.min.css",
						"~/Content/kendo/kendo.bootstrap.min.css"));



			bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/FrontEnd/css/bootstrap.min.css",
                    "~/Content/FrontEnd/css/font-awesome.min.css",
                    "~/Content/FrontEnd/css/icofont.css",
                    "~/Content/FrontEnd/css/slidr.css",
                    "~/Content/FrontEnd/css/main.css",
                    "~/Content/FrontEnd/css/presets/preset1.css",
                    "~/Content/FrontEnd/css/responsive.css"
                           
                 ));


     


           bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(

                    "~/Content/FrontEnd/js/jquery.min.js",
                    "~/Content/FrontEnd/js/modernizr.min.js",
                    "~/Content/FrontEnd/js/bootstrap.min.js",             
                    "~/Content/FrontEnd/js/owl.carousel.min.js",
                    "~/Content/FrontEnd/js/smoothscroll.min.js",
                    "~/Content/FrontEnd/js/scrollup.min.js",
                    "~/Content/FrontEnd/js/price-range.js",
                    "~/Content/FrontEnd/js/jquery.countdown.js",
                    "~/Content/FrontEnd/js/custom.js"
                    ));

                   //"~/Content/FrontEnd/js/gmaps.min.js",
                   // "~/Content/FrontEnd/js/goMap.js",
                   // "~/Content/FrontEnd/js/map.js",
          

                bundles.Add(new ScriptBundle("~/bundles/_Sepet").Include(
	                "~/Scripts/jquery-2.1.3.min.js",
                    "~/Scripts/jquery-ui-1.8.20.min.js",
                    "~/Scripts/jquery.signalR-2.0.0.js",
                    "~/signalr/hubs"
                 ));

         
	
		
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //        "~/Scripts/bootstrap.min.js"));


			bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Scripts/plugins/datatables/jquery.dataTables.js",
                    "~/Scripts/plugins/datatables/dataTables.bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
					  "~/Content/Admin/css/datatables/dataTables.bootstrap.css"));

			bundles.Add(new StyleBundle("~/Content/css/datatables_With_Image").Include(
					  "~/Content/Admin/css/datatables/datatables_With_Image.css"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryAdmin").Include(
                    "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                    "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/raphael").Include(
                    "~/Scripts/raphael-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-slider").Include(
                    "~/Scripts/plugins/bootstrap-slider/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-wysihtml5").Include(
                    "~/Scripts/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include(
                    "~/Scripts/plugins/colorpicker/bootstrap-colorpicker.min.js"));

          

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                    "~/Scripts/plugins/datepicker/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                    "~/Scripts/plugins/daterangepicker/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                    "~/Scripts/plugins/flot/jquery.flot.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.resize.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.pie.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.categories.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                    "~/Scripts/plugins/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/input-mask").Include(
                    "~/Scripts/plugins/input-mask/jquery.inputmask.js",
                    "~/Scripts/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Scripts/plugins/input-mask/jquery.inputmask.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/ionslider").Include(
                    "~/Scripts/plugins/ionslider/ion.rangeSlider.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryKnob").Include(
                    "~/Scripts/plugins/jqueryKnob/jquery.knob.js"));

            bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
                    "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                    "~/Scripts/plugins/jvectormap/jquery-jvectormap-data-turkey-tr-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Scripts/plugins/misc/html5shiv.js",
                    "~/Scripts/plugins/misc/jquery.ba-resize.min.js",
                    "~/Scripts/plugins/misc/jquery.placeholder.js",
                    "~/Scripts/plugins/misc/modernizr.min.js",
                    "~/Scripts/plugins/misc/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                    "~/Scripts/plugins/morris/morris.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/slimScroll").Include(
                    "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sparkline").Include(
                    "~/Scripts/plugins/sparkline/jquery.sparkline.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                    "~/Scripts/plugins/timepicker/bootstrap-timepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                    "~/Scripts/AdminLTE/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE_Dashboard").Include(
               "~/Scripts/AdminLTE/dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/Calendar").Include(
               "~/Scripts/plugins/calendar/moment.min.js",
               "~/Scripts/plugins/calendar/fullcalendar.min.js"));


            bundles.Add(new StyleBundle("~/Content/css/AdminLTE").Include(
                      "~/Content/Admin/css/AdminLTE.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/Admin/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-slider").Include(
                    "~/Content/Admin/css/bootstrap-slider/slider.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-wysihtml5").Include(
                      "~/Content/Admin/css/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/colorpicker").Include(
                   "~/Content/Admin/css/colorpicker/bootstrap-colorpicker.min.css"));

            

            bundles.Add(new StyleBundle("~/Content/css/datepicker").Include(
                   "~/Content/Admin/css/datepicker/datepicker3.css"));

            bundles.Add(new StyleBundle("~/Content/css/daterangepicker").Include(
                      "~/Content/Admin/css/daterangepicker/daterangepicker-bs3.css"));

            bundles.Add(new StyleBundle("~/Content/css/iCheck").Include(
                   "~/Content/Admin/css/iCheck/all.css"));

            bundles.Add(new StyleBundle("~/Content/css/ionslider").Include(
                   "~/Content/Admin/css/ionslider/ion.rangeSlider.css",
                   "~/Content/Admin/css/ionslider/ion.rangeSlider.skinNice.css"));

            bundles.Add(new StyleBundle("~/Content/css/jvectormap").Include(
                      "~/Content/Admin/css/jvectormap/jquery-jvectormap-1.2.2.css"));

            bundles.Add(new StyleBundle("~/Content/css/morris").Include(
                   "~/Content/Admin/css/morris/morris.css"));

            bundles.Add(new StyleBundle("~/Content/css/timepicker").Include(
                      "~/Content/Admin/css/timepicker/bootstrap-timepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/calendar").Include(
                     "~/Content/Admin/css/plugins/calendar/fullCalendar.css",
                     "~/Content/Admin/css/plugins/calendar/fullCalendarPrint.css"));

         
            

        }
    }
}
