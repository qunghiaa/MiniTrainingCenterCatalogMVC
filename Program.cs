using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Options;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using MiniTrainingCenterCatalog.Mvc.Models;

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
    .AddIdentity<
        ApplicationUser,
        IdentityRole>()
    .AddEntityFrameworkStores<
        AppDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.LoginPath =
            "/Account/Login";

        options.AccessDeniedPath =
            "/Account/AccessDenied";
    });
    builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy(
            "CanManageCourse",
            p =>
                p.RequireRole("Admin"));

        options.AddPolicy(
            "CanViewCourse",
            p =>
                p.RequireRole(
                    "Admin",
                    "Staff"));

        options.AddPolicy(
            "CanViewAuditLog",
            p =>
                p.RequireRole(
                    "Admin"));

        options.AddPolicy(
            "CanUploadCourseImage",
            p =>
                p.RequireRole(
                    "Admin"));
    });
    builder.Services.AddScoped<
    IAuditLogService,
    AuditLogService>();
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
    IAuditService,
    AuditService>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live");

app.MapHealthChecks("/health/ready");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
using (var scope =
    app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(
        scope.ServiceProvider);
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedRoles.InitializeAsync(services);

    await SeedAdmin.InitializeAsync(services);
}
app.Run();