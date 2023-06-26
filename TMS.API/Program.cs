using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TMS.API.Mappings;
using TMS.Core;
using TMS.Infrastructure.Context;
using TMS.Infrastructure.Interfaces;
using TMS.Infrastructure.Repository;
using TMS.Services.Interfaces;
using TMS.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Configure identity options here.
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    // Lockout settings  
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration.GetSection("AccountLockOutInfo:LockoutTimeSpan").Value));
    options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(builder.Configuration.GetSection("AccountLockOutInfo:MaxFailedAccessAttempts").Value);
    options.Lockout.AllowedForNewUsers = false;
    // User settings  
    options.User.RequireUniqueEmail = true;
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
 .AddDefaultTokenProviders();
builder.Services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetSection("ConnectionString:TimeSheetDB").Value));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSender"));

#region Service Scope

#region Unit Of Work Service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region User Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

#region Role Services
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
#endregion

#region Invoice Services
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
#endregion

#region Invoice Detail Services
builder.Services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
#endregion

#region Tasks Services
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
#endregion

#region Task Assignment Services 
builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
#endregion

#region TimeSheetApproval Service
builder.Services.AddScoped<ITimesheetApprovalsRepository, TimesheetApprovalsRepository>();
builder.Services.AddScoped<ITimesheetApprovalsService, TimesheetApprovalsService>();
#endregion

#region TimeSheet Services
builder.Services.AddScoped<ITimesheetMasterRepository, TimesheetMasterRepository>();
builder.Services.AddScoped<ITimesheetMasterService, TimesheetMasterService>();
#endregion

#region TimeSheet Services
builder.Services.AddScoped<ITimesheetDetailsRepository, TimesheetDetailsRepository>();
builder.Services.AddScoped<ITimesheetDetailsService, TimesheetDetailsService>();
#endregion

#region User Resolver Service
builder.Services.AddScoped<IUserResolverService, UserResolverService>();
#endregion

#region Mail Service
builder.Services.AddTransient<IMailService, MailService>();
#endregion

#endregion

// Automapper
// Start Registering and Initializing AutoMapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfiles());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Host.UseSerilog((context, configuration) =>
configuration.WriteTo.Console()
.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:key").Value));
    options.TokenValidationParameters = new
    TokenValidationParameters
    {
        IssuerSigningKey = serverSecret,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        RequireExpirationTime = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
#region CORS
// Add service and create Policy with options
//builder.Services.AddCors(options =>
//{
//    //options.AddPolicy("CorsPolicy", p => p.WithOrigins(Configuration["CORSUrl:Url"])
//    //        .AllowAnyMethod()
//    //        .AllowAnyHeader()
//    //        .AllowCredentials());

//    options.AddPolicy("CorsPolicy", p => p.WithOrigins(builder.Configuration.GetSection("CORSUrl").Value)
//                             .AllowAnyMethod()
//                             .AllowAnyHeader()
//                             .AllowCredentials());
//});
builder.Services.AddCors();
#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Timesheet Management API",
        Description = "Timesheet Management API",
        //TermsOfService = new Uri("https://convergesolution.com/"),
        //License = new OpenApiLicense
        //{
        //    Name = "Use under LICX",
        //    Url = new Uri("https://convergesolution.com/")
        //}
    });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    //c.DocumentFilter<RemoveSchemasFilter>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(builder => builder.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod());

app.Run();