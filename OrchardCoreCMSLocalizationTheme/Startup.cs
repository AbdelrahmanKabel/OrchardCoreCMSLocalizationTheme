using OrchardCore.Apis;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.GraphQL;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace OrchardCoreCMSLocalizationTheme
{
    [RequireFeatures("OrchardCore.Apis.GraphQL")]
    public class Startup : OrchardCore.Modules.StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // I have omitted the registering of the AutoroutePart, as we expect that to already be registered
            //services.AddObjectGraphType<Name, NameQueryObjectType>();
            //services.AddInputObjectGraphType<Name, NameInputObjectType>();

            //services.AddScoped<IDataMigration, Migrations>();
            services.AddContentPart<ContactUS>();
            
            services.AddGraphQLFilterType<ContentItem, ContactUSGraphQLFilter>();
            services.AddInputObjectGraphType<ContactUS, NameInputObjectType>();

            services.AddObjectGraphType<ContactUS, ContactUSObjectGraphType>();
            services.AddObjectGraphType<ContentItem, CustomContentItemType>();
            services.AddSingleton<IIndexProvider, ContactUSIndexProvider>();

        }
    }
}
