using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Themes.LocalizationTheme
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest _manifest;

        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineScript("LocalizationTheme-bootstrap-bundle")
                .SetCdn("https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js", "https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.js")
                .SetCdnIntegrity("sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p", "sha384-8fq7CZc5BnER+jVlJI2Jafpbn4A9320TKhNJfYP33nneHep7sUg/OD30x7fK09pS")
                .SetVersion("5.1.3");

            _manifest
                .DefineStyle("LocalizationTheme-bootstrap-oc")
                .SetUrl("~/LocalizationTheme/css/bootstrap-oc.min.css", "~/LocalizationTheme/css/bootstrap-oc.css")
                .SetVersion("1.0.0");

            _manifest
                .DefineScript("LocalizationTheme")
                .SetDependencies("LocalizationTheme-bootstrap-bundle")
                .SetUrl("~/LocalizationTheme/js/scripts.min.js", "~/LocalizationTheme/js/scripts.js")
                .SetVersion("6.0.7");

            _manifest
                .DefineStyle("LocalizationTheme")
                .SetUrl("~/LocalizationTheme/css/styles.min.css", "~/LocalizationTheme/css/styles.css")
                .SetVersion("6.0.7");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
