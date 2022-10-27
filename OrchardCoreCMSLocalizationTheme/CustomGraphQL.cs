using GraphQL.Types;
using Microsoft.Extensions.Options;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Apis.GraphQL.Queries;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.GraphQL.Options;
using OrchardCore.ContentManagement.GraphQL.Queries;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Records;
using YesSql;
using YesSql.Indexes;

namespace OrchardCoreCMSLocalizationTheme
{
    public class ContactUS : ContentPart
    {
        public TextField Name { get; set; }
        public TextField Email { get; set; }
        public TextField Message { get; set; }
    }
    public class ContactUSModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class ContactUSFilterModel : ContentPart
    {
        public string Name { get; set; }
        //public string Email { get; set; }
        //public string Message { get; set; }
    }
    public class ContactUSObjectGraphType : ObjectGraphType<ContactUS>
    {
        public ContactUSObjectGraphType(IServiceProvider services)
        {
            Field<ListGraphType<CustomContentItemType>>(
                 "ContactUSByName",
                 arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "name" }),
                 resolve: context =>
                 {
                     var graphContext = (GraphQLContext)context.UserContext;
                     var contentManager = graphContext.ServiceProvider.GetRequiredService<IContentManager>();
                     var session = graphContext.ServiceProvider.GetService<YesSql.ISession>();

                     var contactusName = context.GetArgument<string>("name");
                     if (string.IsNullOrEmpty(contactusName)) return null;

                     var contactus = session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == "ContactUS")
                                              .With<ContactUSIndex>(x => x.Name.Contains(contactusName))
                                              .ListAsync().Result;

                     return contactus;
                 }
             );
            //Name = "ContactUSPart";

            // Map the fields you want to expose
            //Field(x => x.Name);
            //Field(x => x.Email);
            //Field(x => x.Message);
        }
    }
    public class CustomContentItemType : ObjectGraphType<ContentItem>
    {
        public CustomContentItemType(IOptions<GraphQLContentOptions> optionsAccessor)
        {
            Name = "CustomContentItemType";

            Field(ci => ci.ContentItemId).Description("Content item id");
            Field(ci => ci.ContentItemVersionId).Description("The content item version id");
            Field(ci => ci.ContentType).Description("Type of content");
            Field(ci => ci.DisplayText, nullable: true).Description("The display text of the content item");
            Field(ci => ci.Published).Description("Is the published version");
            Field(ci => ci.Latest).Description("Is the latest version");
            Field<DateTimeGraphType>("modifiedUtc", resolve: ci => ci.Source.ModifiedUtc, description: "The date and time of modification");
            Field<DateTimeGraphType>("publishedUtc", resolve: ci => ci.Source.PublishedUtc, description: "The date and time of publication");
            Field<DateTimeGraphType>("createdUtc", resolve: ci => ci.Source.CreatedUtc, description: "The date and time of creation");
            Field(ci => ci.Owner).Description("The owner of the content item");
            Field(ci => ci.Author).Description("The author of the content item");

            //Interface<ContentItemInterface>();
            //IsTypeOf = IsContentType;
        }

        //private bool IsContentType(object obj)
        //{
        //    return obj is ContentItem && ((ContentItem)obj).ContentType == Name;
        //}
    }
    public class NameInputObjectType : InputObjectGraphType<ContactUS>
    {
        public NameInputObjectType()
        {
            Name = "ContactUSNameInput";

            Field(x => x.Name.Text, nullable: true).Description("the name of the content item to filter");
            //Field(x => x.Email.Text, nullable: true).Description("the email of the content item to filter");
            //Field(x => x.Message.Text, nullable: true).Description("the message of the content item to filter");

        }
    }

    public class ContactUSGraphQLFilter : GraphQLFilter<ContentItem>
    {
        public override Task<IQuery<ContentItem>> PreQueryAsync(IQuery<ContentItem> query, ResolveFieldContext context)
        {
            if (!context.HasArgument("where"))
            {
                return Task.FromResult(query);
            }

            string value = "";

            if( ((Dictionary<string, object>)context.GetArgument<object>("where")).Keys.Contains("text") )
            {
                //((Dictionary<string, object>)context.GetArgument<object>("where")).Keys.FirstOrDefault(i => i == "text");
                value = ((Dictionary<string, object>)context.GetArgument<object>("where"))["text"].ToString();
            }
            if (string.IsNullOrEmpty(value))
            {
                return Task.FromResult(query);
            }
            var autorouteQuery = query.With<ContactUSIndex>();
            return Task.FromResult(autorouteQuery.Where(index => index.Name == value).All());

            //var part = context.GetArgument<string>("text");

            //if (part == null)
            //{
            //    return Task.FromResult(query);
            //}

            //var autorouteQuery = query.With<ContactUSIndex>();

            //if (!string.IsNullOrWhiteSpace(part))
            //{
            //    return Task.FromResult(autorouteQuery.Where(index => index.Name == part).All());
            //}

            return Task.FromResult(query);
        }
    }
    public class ContactUSIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class ContactUSIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ContactUSIndex>().Map(contentItem =>
            {

                var contactUSPartContent = contentItem.As<ContactUS>();

                return contactUSPartContent == null ? null : new ContactUSIndex
                {
                    ContentItemId = contentItem.ContentItemId,
                    Name = contactUSPartContent.Name.Text,
                    Email = contactUSPartContent.Email.Text,
                    Message = contactUSPartContent.Message.Text,
                };
            });
        }
    }
    //Old Code 

    //public class ContactUSQueryObjectType : ObjectGraphType<ContactUSIndex>
    //{
    //    public ContactUSQueryObjectType()
    //    {
    //        Name = "ContactUS";

    //        // Map the fields you want to expose
    //        Field(x => x.Name);
    //        Field(x => x.Email);
    //    }
    //}

    //public class ContactUSInputObjectType : ContentItemWhereInput
    //{
    //    public ContactUSInputObjectType()
    //    {
    //        //Name = "ContactUSInput";

    //        //Field(x => x.Name, nullable: true).Description("the Name of the content item to filter");
    //        //Field(x => x.Email, nullable: true).Description("the Name of the content item to filter");
    //        //AddScalarFilterFields<StringGraphType>("Name", "The Name of Contact US");
    //    }
    //}
    //public class  AutoroutePartGraphType: InputObjectGraphType<ContactUS>
    //{
    //    public AutoroutePartGraphType()
    //    {
    //        Name = "AutoroutePartInput";

    //        Field(x => x.Name, nullable: true).Description("the path of the content item to filter");
    //        Field(x => x.Email, nullable: true).Description("the path of the content item to filter");
    //    }
    //}
    //public class AutorouteInputObjectType : ObjectGraphType<ContactUS>
    //{
    //    public AutorouteInputObjectType()
    //    {
    //        Name = "Contact Us Query";
    //        Field(x => x.Name, nullable: true).Description("the path of the content item to filter");
    //        Field(x => x.Email, nullable: true).Description("the path of the content item to filter");
    //    }
    //}
    //public class AutoroutePartGraphQLFilter : GraphQLFilter<ContentItem>
    //{
    //    public override Task<IQuery<ContentItem>> PreQueryAsync(IQuery<ContentItem> query, ResolveFieldContext context)
    //    {
    //        if (!context.HasArgument("name"))
    //        {
    //            return Task.FromResult(query);
    //        }

    //        var name = context.GetArgument<string>("name");

    //        if (name == null)
    //        {
    //            return Task.FromResult(query);
    //        }

    //        var autorouteQuery = query.With<ContactUSIndex>();

    //        if (!string.IsNullOrWhiteSpace(name))
    //        {
    //            return Task.FromResult(autorouteQuery.Where(index => index.Name == name).All());
    //        }

    //        return Task.FromResult(query);
    //    }
    //}
}
