using Microsoft.EntityFrameworkCore;
using MiniTrainingCenterCatalog.Mvc.Data;
using MiniTrainingCenterCatalog.Mvc.Options;
using MiniTrainingCenterCatalog.Mvc.Repositories;
using MiniTrainingCenterCatalog.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MiniTrainingCenterCatalog.Mvc.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddProblemDetails();

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
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
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
            "CanAdjustCourseSeats",
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
    .AddCheck(
        "self",
        () => HealthCheckResult.Healthy(),
        tags: new[] { "live" })
    .AddDbContextCheck<AppDbContext>(
        name: "database",
        tags: new[] { "ready" });

builder.Services.AddScoped<
    ICourseRepository,
    CourseRepository>();

builder.Services.AddScoped<
    ICourseService,
    CourseService>();
builder.Services.AddScoped<
    IFileUploadService,
    FileUploadService>();
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
app.UseStatusCodePagesWithReExecute("/Home/ErrorStatusCode", "?code={0}");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate = check =>
            check.Tags.Contains("live")
    });

app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {
        Predicate = check =>
            check.Tags.Contains("ready"),
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description
                }),
                totalDuration = report.TotalDuration.TotalMilliseconds
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });

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
