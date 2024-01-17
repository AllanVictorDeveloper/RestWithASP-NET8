using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using RestWithASPNET.Api.Business;
using RestWithASPNET.Api.Business.Implementations;
using RestWithASPNET.Api.Configurations;
using RestWithASPNET.Api.Model.Context;
using RestWithASPNET.Api.Repository;
using RestWithASPNET.Api.Repository.Generic;
using RestWithASPNET.Api.Repository.Implementations;
using RestWithASPNET.Api.Services;
using RestWithASPNET.Api.Services.Implementations;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var tokenConfigurations = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(

    builder.Configuration.GetSection("TokenConfigurations")
    )
    .Configure(tokenConfigurations);

builder.Services.AddSingleton(tokenConfigurations);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = tokenConfigurations.Issuer,
            ValidAudience = tokenConfigurations.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var connection = builder.Configuration["ConnectionStrings:SqlServerConnection"];

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(connection);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "REST API's from 0 to Azure with ASP.NET Core 8 and Docker - v1.0.0",
        Version = "v1",

        Description = "API RESTful developed in course REST API's from 0 to Azure with ASP.NET Core 8 and Docker",

        Contact = new OpenApiContact
        {
            Name = "Allan Victor",
            Url = new Uri("https://meusite.com.br")
        }
    });
});

//Versioning API
builder.Services.AddApiVersioning();

builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
}).AddXmlSerializerFormatters();

//Dependency Injection
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILoginBusiness, LoginBusiness>();
builder.Services.AddScoped<IFileBusiness, FileBusiness>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRespository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API's from 0 to Azure with ASP.NET Core 8 and Docker - v1");
});

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
