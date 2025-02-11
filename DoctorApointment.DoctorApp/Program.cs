using Microsoft.AspNetCore.Authentication.Cookies;
using FluentValidation.AspNetCore;
using DoctorApointment.Apilntegreation;
using DoctorApointment.ViewModels.System.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
    });

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IClinicApiClient, ClinicApiClient>();
builder.Services.AddTransient<ISpecialityApiClient, SpecialityApiClient>();
builder.Services.AddTransient<ILocationApiClient, LocationApiClient>();
builder.Services.AddTransient<IScheduleApiClient, ScheduleApiClient>();
builder.Services.AddTransient<IMedicineApiClient, MedicineApiClient>();
builder.Services.AddTransient<IServiceApiClient, ServiceApiClient>();
builder.Services.AddTransient<IAppointmentApiClient, AppointmentApiClient>();
builder.Services.AddTransient<IPostApiClient, PostApiClient>();
builder.Services.AddTransient<IMasterDataApiClient, MasterDataApiClient>();
builder.Services.AddTransient<IStatisticApiClient, StatisticApiClient>();
builder.Services.AddTransient<IDoctorApiClient, DoctorApiClient>();
builder.Services.AddTransient<IAnnualServiceFeeApiClient, AnnualServiceFeeApiClient>();

IMvcBuilder builde = builder.Services.AddRazorPages();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
#if DEBUG
if (environment == Environments.Development)
{
    builde.AddRazorRuntimeCompilation();
}
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
