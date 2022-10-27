using GraphQL.Types;
using Microsoft.Extensions.Options;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Apis.GraphQL.Queries;
using OrchardCore.ContentManagement.GraphQL.Options;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;

namespace OrchardCoreCMSLocalizationTheme
{
    
    [GraphQLFieldName("Or", "OR")]
    [GraphQLFieldName("And", "AND")]
    [GraphQLFieldName("Not", "NOT")]
    public class ContactUScontentItemWhereInput : ContentItemWhereInput
    {
        private readonly IOptions<GraphQLContentOptions> _optionsAccessor;

        public ContactUScontentItemWhereInput(string contentItemName, IOptions<GraphQLContentOptions> optionsAccessor) : base(contentItemName, optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
            base.Name = contentItemName + "WhereInput";
            base.Description = "the " + contentItemName + " content item filters";
            AddScalarFilterFields<StringGraphType>("name", "the name of the content item");
            ListGraphType resolvedType = new ListGraphType(this);
            Field<ListGraphType<ContentItemWhereInput>>("Or", "OR logical operation").ResolvedType = resolvedType;
            Field<ListGraphType<ContentItemWhereInput>>("And", "AND logical operation").ResolvedType = resolvedType;
            Field<ListGraphType<ContentItemWhereInput>>("Not", "NOT logical operation").ResolvedType = resolvedType;
            
        }
    }
}
