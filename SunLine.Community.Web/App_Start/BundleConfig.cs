using System.Web.Optimization;
using SunLine.Community.Web.Extensions;

namespace SunLine.Community.Web
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/SunLine.Community.Validation.js",
                        "~/Scripts/jquery.validate*").ForceOrdered());

            bundles.Add(new ScriptBundle("~/bundles/webapp").Include(
                        "~/Scripts/SunLine.Community.WebApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/usermessage").Include(
                        "~/Scripts/SunLine.Community.UserMessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/userprofile").Include(
                        "~/Scripts/SunLine.Community.UserProfile.js"));

            bundles.Add(new ScriptBundle("~/bundles/userconnection").Include(
                        "~/Scripts/SunLine.Community.UserConnection.js"));

            bundles.Add(new ScriptBundle("~/bundles/editprofile").Include(
                        "~/Scripts/SunLine.Community.EditProfile.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                        "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/blueimp").Include(
                        "~/Scripts/blueimp-gallery/blueimp-gallery.js",
                        "~/Scripts/blueimp-gallery/jquery.blueimp-gallery.js"));

            bundles.Add(new ScriptBundle("~/bundles/uikit").Include(
                "~/Scripts/uikit/vendor/codemirror/codemirror.js",
                "~/Scripts/uikit/vendor/marked.js",
                "~/Scripts/uikit/js/uikit.js",
                "~/Scripts/uikit/js/addons/htmleditor.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/TweenMax.js",
                      "~/Scripts/resizeable.js",
                      "~/Scripts/joinable.js",
                      "~/Scripts/xenon-api.js",
                      "~/Scripts/xenon-toggles.js",
                      "~/Scripts/toastr/toastr.js",
                      "~/Scripts/xenon-custom.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/linecons.css",
                      "~/Content/font-awesome.css",
                      "~/Content/elusive.css",
                      "~/Content/bootstrap.css",
                      "~/Content/xenon-core.css",
                      "~/Content/xenon-forms.css",
                      "~/Content/xenon-components.css",
                      "~/Content/xenon-skins.css",
                      "~/Content/custom.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                      "~/Content/linecons.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/xenon").Include(
                      "~/Content/xenon-core.css",
                      "~/Content/xenon-forms.css",
                      "~/Content/xenon-components.css",
                      "~/Content/xenon-skins.css"));

            bundles.Add(new StyleBundle("~/Scripts/select2/select2").Include(
                      "~/Scripts/select2/select2.css",
                      "~/Scripts/select2/select2-bootstrap.css").ForceOrdered());

            bundles.Add(new StyleBundle("~/Scripts/dropzone/css/dropzone").Include(
                    "~/Scripts/dropzone/css/dropzone.css"));

            bundles.Add(new StyleBundle("~/Content/blueimp/blueimp").Include(
                    "~/Content/blueimp-gallery/blueimp-gallery.css"));

            bundles.Add(new StyleBundle("~/Content/uikit").Include(
                "~/Scripts/uikit/vendor/codemirror/codemirror.css",
                "~/Scripts/uikit/uikit.css",
                "~/Scripts/uikit/css/addons/uikit.almost-flat.addons.css"
            ));
        }
    }
}
