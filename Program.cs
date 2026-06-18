using MiniTrainingCenterCatalog.Mvc.Services;
using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.Options;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<TrainingCenterSettings>(
    builder.Configuration.GetSection(
        "TrainingCenterSettings"));
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString(
                    "DefaultConnection"));
    });
// Add services to the container.
builder.Services.AddScoped<ICourseRepository,
    CourseRepository>();

builder.Services.AddScoped<ICourseDataService,
    CourseDataService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICourseService, CourseService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
