using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Twilio.Clients;
using DoctorApointment.Application.Catalog.Appointment;
using DoctorApointment.Application.Catalog.Clinic;
using DoctorApointment.Application.Catalog.Comment;
using DoctorApointment.Application.Catalog.Contact;
using DoctorApointment.Application.Catalog.Location.Distric;
using DoctorApointment.Application.Catalog.Location;
using DoctorApointment.Application.Catalog.MasterData;
using DoctorApointment.Application.Catalog.MedicalRecords;
using DoctorApointment.Application.Catalog.Medicine;
using DoctorApointment.Application.Catalog.Post;
using DoctorApointment.Application.Catalog.Rate;
using DoctorApointment.Application.Catalog.Schedule;
using DoctorApointment.Application.Catalog.Service;
using DoctorApointment.Application.Catalog.SlotSchedule;
using DoctorApointment.Application.Catalog.Speciality;
using DoctorApointment.Application.Catalog.Topic;
using DoctorApointment.Application.Common;
using DoctorApointment.Application.System.AnnualServiceFee;
using DoctorApointment.Application.System.Doctor;
using DoctorApointment.Application.System.StatisticService;
using DoctorApointment.Application.System.Users;
using DoctorApointment.Data.EF;
using DoctorApointment.Data.Entities;
using DoctorApointment.Utilities.Constants;
using DoctorApointment.ViewModels.System.Models;
using DoctorApointment.ViewModels.System.Users;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<DoctorManageDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

builder.Services.AddIdentity<AppUsers, AppRoles>()
    .AddEntityFrameworkStores<DoctorManageDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<UserManager<AppUsers>, UserManager<AppUsers>>();
builder.Services.AddTransient<SignInManager<AppUsers>, SignInManager<AppUsers>>();
builder.Services.AddTransient<RoleManager<AppRoles>, RoleManager<AppRoles>>();

builder.Services.AddTransient<IStorageService, FileStorageService>();

builder.Services.AddTransient<IClinicService, ClinicService>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IDistricService, DistricService>();
builder.Services.AddTransient<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IRateService, RateService>();
builder.Services.AddTransient<IScheduleService, ScheduleService>();
builder.Services.AddTransient<ISlotScheduleService, SlotScheduleService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ISpecialityService, SpecialityService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IMasterDataService, MasterDataService>();
builder.Services.AddTransient<IAnnualServiceFeeService, AnnualServiceFeeService>();
builder.Services.AddTransient<IStatisticService, StatisticService>();
builder.Services.AddTransient<IMedicineService, MedicineService>();
builder.Services.AddTransient<IServicesService, ServicesService>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHttpClient<ITwilioRestClient, TwilioClient>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Doctor Manage Solution", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
    // Set the comments path for the Swagger JSON and UI.
  /*  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));*/
});
builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
});
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

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Doctor Manage Solution V1");
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=swagger}");


app.Run();