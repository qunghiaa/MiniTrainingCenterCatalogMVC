using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Options;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<TrainingCenterSettings>(
    builder.Configuration.GetSection(
        "TrainingCenterSettings"));

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"));
    });

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "database");

builder.Services.AddScoped<
    ICourseRepository,
    CourseRepository>();

builder.Services.AddScoped<
    ICourseService,
    CourseService>();

builder.Services.AddScoped<
    ICourseDataService,
    CourseDataService>();

builder.Services.AddScoped<
    IEnrollmentRepository,
    EnrollmentRepository>();

builder.Services.AddScoped<
    IEnrollmentService,
    EnrollmentService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapHealthChecks("/health/live");

app.MapHealthChecks("/health/ready");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();