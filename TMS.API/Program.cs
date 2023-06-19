using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
 .AddDefaultTokenProviders()
   .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, int>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, int>>();
builder.Services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetSection("ConnectionString:TimeSheetDB").Value));

#region Service Scope
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IProjectDetailsRepository ,ProjectDocumentRepository>();
builder.Services.AddScoped<IProjectDocumentService, ProjectDocumentService>();

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

//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
//});
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
        Title = "Assessment API",
        Description = "Assessment API",
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
