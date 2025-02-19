// Program.cs
using UmbracoProject.Services;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Events;
using UmbracoProject.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register ContentService as a scoped service
//builder.Services.AddScoped<ContentService>();

builder.Services.AddSingleton<JsonExportService>();
builder.Services.AddHttpClient(); // This allows IHttpClientFactory to work

// builder.Services.Configure<JsonLoaderOptions>(options =>
// {
//     // Combine the base directory with the relative path to the JSON file
//     options.JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "content-bundle.json");
// });

// builder.Services.AddSingleton<JsonLoaderService>();


// Configure Umbraco
builder.Services.AddUmbraco(builder.Environment, builder.Configuration)
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .AddDeliveryApi()
    .Build();

builder.Services.AddScoped<INotificationHandler<ContentSavingNotification>, ModuleEventHandler>();

builder.Services.AddHttpClient();

var app = builder.Build();

await app.BootUmbracoAsync();

// Configure Umbraco middleware
app.UseUmbraco()
    .WithMiddleware(u => {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u => {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

// Use routing and endpoints
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
