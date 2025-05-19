using IvanSusaninProject.Adapters;
using IvanSusaninProject.Infrastructure;
using IvanSusaninProject_BusinessLogic.Implementations;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Implementations;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

builder.Services.AddRazorPages();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfigurationDatabase, ConfigurationDatabase>();
builder.Services.AddTransient<IvanSusaninProject_DbContext>();
builder.Services.AddSingleton<IExcursionAdapter, ExcursionAdapter>();
builder.Services.AddSingleton<IExecutorAdapter, ExecutorAdapter>();
builder.Services.AddSingleton<IGroupAdapter, GroupAdapter>();
builder.Services.AddSingleton<IGuarantorAdapter, GuarantorAdapter>();
builder.Services.AddSingleton<IGuideAdapter, GuideAdapter>();
builder.Services.AddSingleton<IPlaceAdapter, PlaceAdapter>();
builder.Services.AddSingleton<ITourAdapter, TourAdapter>();
builder.Services.AddSingleton<ITripAdapter, TripAdapter>();
builder.Services.AddSingleton<IExcursionBusinessLogicsContract, ExcursionBusinessLogicsContracts>();
builder.Services.AddSingleton<IExecutorBusinessLogicsContract, ExecutorBusinessLogicsContract>();
builder.Services.AddSingleton<IGroupBusinessLogicsContract, GroupBusinessLogicsContract>();
builder.Services.AddSingleton<IGuarantorBusinessLogicsContract, GuarantorBusinessLogicsContract>();
builder.Services.AddSingleton<IGuideBusinessLogicsContract, GuideBusinessLogicsContract>();
builder.Services.AddSingleton<IPlaceBusinessLogicContract, PlaceBusinessLogicContract>();
builder.Services.AddSingleton<ITourBusinessLogicsContract, TourBusinessLogicsContract>();
builder.Services.AddSingleton<ITripBusinessLogicContract, TripBusinessLogicContract>();
builder.Services.AddSingleton<IExcursionStorageContract, ExcursionStorageContract>();
builder.Services.AddSingleton<IExecutorStorageContract, ExecutorStorageContract>();
builder.Services.AddSingleton<IGroupStorageContract, GroupStorageContract>();
builder.Services.AddSingleton<IGuarantorStorageContract, GuarantorStorageContract>();
builder.Services.AddSingleton<IGuideStrorageContract, GuideStrorageContract>();
builder.Services.AddSingleton<IPlaceStorageContract, PlaceStorageContract>();
builder.Services.AddSingleton<ITourStorageContract, TourStorageContract>();
builder.Services.AddSingleton<ITripStorageContract, TripStorageContract>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsProduction())
{
    var dbContext = app.Services.GetRequiredService<IvanSusaninProject_DbContext>();
    if (dbContext.Database.CanConnect())
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
