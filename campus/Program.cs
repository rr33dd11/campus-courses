using System.Text;
using System.Text.Json.Serialization;
using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.SeedingData;
using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.UsersDTO;
using campus.Services;
using campus.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AuthorizationMiddleware = campus.AdditionalServices.TokenHelpers.AuthorizationMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(
    new RedisRepository(builder.Configuration.GetConnectionString("RedisDatabase")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddSingleton<TokenInteraction>();
builder.Services.AddSingleton<DataSeeder>();
builder.Services.AddSingleton<IAuthorizationHandler, BlackTokenHandler>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddleware>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        var secret = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Secret").Value);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "JWTToken",
            ValidAudience = "Human",
            IssuerSigningKey = new SymmetricSecurityKey(secret)
        };
    }
);

builder.Services.AddAuthorization(services =>
{
    services.AddPolicy("BlackToken", policy => policy.Requirements.Add(new BlackListRequirement()));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите токен в формате Bearer {your_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT" 
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetService<AppDBContext>();
dbContext?.Database.Migrate();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true));

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();