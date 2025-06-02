using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Uppari_SCMS.Data;
using Uppari_SCMS.Data.Auth; // Add this namespace for Identity services

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.UseNetTopologySuite()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// Fix: Replace AddDefaultIdentity with AddIdentity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Important
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanAccessReports", policy =>
        policy.RequireClaim("CanViewReports", "true"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login"; // Your Blazor login page
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();  // Ensure authorization middleware is added

app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
