//using DataLayer.Context;

using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Auth;
using DataLayer.Repository.Order;
using DataLayer.Repository.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceLayer.Auth;
using ServiceLayer.File;
using ServiceLayer.Helper;
using ServiceLayer.Interface.IHelper;
using ServiceLayer.Interface.IService;
using ServiceLayer.Mongo;
using ServiceLayer.Order;
using ServiceLayer.Product;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductMasterRepository, ProductMasterRepository>();
builder.Services.AddScoped<IProductMasterService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtMiddleware, JwtMiddleware>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderMasterRepository, OrderMasterRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IWhatsApp, WhatsAppHelper>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
builder.Services.Configure<MongoDbDC>(
    builder.Configuration.GetSection("Order"));
builder.Services.AddScoped<CartService>();
builder.Services.AddControllers();

var key = builder.Configuration.GetSection("Key").GetSection("userKey").Value;
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
    };
});
//builder.Services.AddSingleton<IUser,UserRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Here enter Jwt Token"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
         {
                    Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    },
    new string[]{}

    }
    });
});

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
// Shows UseCors with CorsPolicyBuilder.
// Adding CORS Policy

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

// Shows UseCors with named policy.
app.UseCors("MyPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();

