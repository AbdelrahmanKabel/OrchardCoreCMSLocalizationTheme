using OrchardCore.Apis;
using OrchardCoreCMSLocalizationTheme;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOrchardCms();
//builder.Services.AddObjectGraphType<ContactUSIndex, ContactUSQueryObjectType>();
//builder.Services.AddInputObjectGraphType<string, ContactUSInputObjectType>();

var startup = new Startup();
startup.ConfigureServices(builder.Services);

//builder.Services.AddObjectGraphType<Name, NameQueryObjectType>();
//builder.Services.AddInputObjectGraphType<Name, NameInputObjectType>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOrchardCore();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200/");
    await next.Invoke();
});


app.Run();
