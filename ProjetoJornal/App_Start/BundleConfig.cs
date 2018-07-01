using System.Web;
using System.Web.Optimization;

namespace ProjetoJornal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/logincss").Include(
                        "~/Areas/Admin/Assets/admin/pages/css/login.css"));

            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
                      "~/Areas/Admin/Assets/admin/pages/scripts/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                     "~/Assets/js/modernmag-plugins.min.js",
                      "~/Assets/js/popper.js",
                       "~/Assets/js/bootstrap.min.js",
                     "~/Assets/js/script.js"));

            bundles.Add(new StyleBundle("~/Assets/css").Include(
                "~/Assets/css/modernmag-assets.min.css",
                "~/Assets/css/style.css"));

            //bundles Admin

            bundles.Add(new StyleBundle("~/Areas/Admin/Assets/global/css").Include(
               "~/Areas/Admin/Assets/global/css/plugins.css",
               "~/Areas/Admin/Assets/global/css/components-rounded.css"
               ));

            bundles.Add(new StyleBundle("~/Areas/Admin/Assets/admin/layout3/css").Include(
             "~/Areas/Admin/Assets/admin/layout3/css/layout.css",
             "~/Areas/Admin/Assets/admin/layout3/css/themes/default.css",
             "~/Areas/Admin/Assets/admin/layout3/css/custom.css"
             ));

            bundles.Add(new StyleBundle("~/Areas/Admin/Assets/global/css").Include(
              "~/Areas/Admin/Assets/global/plugins/font-awesome/css/font-awesome.min.css",
              "~/Areas/Admin/Assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
              "~/Areas/Admin/Assets/global/plugins/bootstrap/css/bootstrap.min.css",
              "~/Areas/Admin/Assets/global/plugins/select2/select2.css",
              "~/Areas/Admin/Assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css",
              "~/Areas/Admin/Assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",
              "~/Areas/Admin/Assets/global/plugins/uniform/css/uniform.default.css",
              "~/Areas/Admin/Assets/global/css/plugins.css",
              "~/Areas/Admin/Assets/global/components-rounded.css",
              "~/Areas/Admin/Assets/global/css/components-rounded.css",
               "~/Areas/Admin/Assets/admin/pages/css/news.css",
               "~/Areas/Admin/Assets/global/plugins/bootstrap/css/bootstrap.min.css",
               "~/Areas/Admin/Assets/global/plugins/uniform/css/uniform.default.css",
               "~/Areas/Admin/Assets/global/plugins/bootstrap-summernote/summernote.css",
               "~/Areas/Admin/Assets/global/plugins/jquery-file-upload/blueimp-gallery/blueimp-gallery.min.css",
               "~/Areas/Admin/Assets/global/plugins/dropzone/css/dropzone.css",
               "~/Areas/Admin/Assets/global/global/plugins/bootstrap-toastr/toastr.min.css",
              "~/Areas/Admin/Assets/admin/pages/css/tasks.css",
               "~/Areas/Admin/Assets/global/plugins/icheck/skins/all.css"
              ));

            bundles.Add(new ScriptBundle("~/Areas/Admin/Assets/global/plugins/jquery.min").Include(
            "~/Areas/Admin/Assets/global/plugins/jquery.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Areas/Admin/Assets/global/plugins/jquery-migrate.min").Include(
               "~/Areas/Admin/Assets/global/plugins/jquery-migrate.min.js"
              ));

            bundles.Add(new ScriptBundle("~/Areas/Admin/Assets/global/plugins/jquery-ui").Include(
                "~/Areas/Admin/Assets/global/plugins/jquery-ui/jquery-ui.min.js"
               ));

            bundles.Add(new ScriptBundle("~/Areas/Admin/Assets/global/plugins/bootstrap").Include(
                    "~/Areas/Admin/Assets/global/plugins/bootstrap/js/bootstrap.min.js",
                   "~/Areas/Admin/Assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery.blockui.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery.cokie.min.js",
                   "~/Areas/Admin/Assets/global/plugins/uniform/jquery.uniform.min.js",
                   "~/Areas/Admin/Assets/global/plugins/select2/select2.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery-validation/js/additional-methods.min.js",
                   "~/Areas/Admin/Assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                   "~/Areas/Admin/Assets/global/plugins/datatables/media/js/jquery.dataTables.min.js",
                   "~/Areas/Admin/Assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js",
                   "~/Areas/Admin/Assets/global/plugins/bootstrap-summernote/summernote.min.js",
                   "~/Areas/Admin/Assets/global/plugins/dropzone/dropzone.js",
                   "~/Areas/Admin/Assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"
                   ));


            bundles.Add(new ScriptBundle("~/Areas/Admin/Assets/admin/layout3/scripts").Include(
                "~/Areas/Admin/Assets/global/plugins/bootstrap-toastr/toastr.min.js",
                "~/Areas/Admin/Assets/global/scripts/metronic.js",
                "~/Areas/Admin/Assets/admin/layout3/scripts/layout.js",
                "~/Areas/Admin/Assets/admin/layout3/scripts/demo.js",
                "~/Areas/Admin/Assets/admin/pages/scripts/form-dropzone.js",
                "~/Areas/Admin/Assets/admin/pages/scripts/form-validation.js",
                "~/Areas/Admin/Assets/admin/pages/scripts/components-editors.js",
                "~/Areas/Admin/Assets/global/plugins/icheck/icheck.min.js",
                "~/Areas/Admin/Assets/global/scripts/datatable.js"));

        }
    }
}
