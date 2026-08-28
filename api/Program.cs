using api.Configuration;
using api.Data;
using api.Interfaces;
using api.Middleware;
using api.Models;
using api.Repositories;
using api.Services;
using apiRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(Options =>
{
    Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApiUser, IdentityRole>(Options =>
{
    Options.Password.RequireDigit = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequireUppercase = true;
    Options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme =
    Options.DefaultChallengeScheme =
    Options.DefaultForbidScheme =
    Options.DefaultScheme =
    Options.DefaultSignInScheme =
    Options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
        )
    };
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelManagementSystem", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod());
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("User", "Customer", "Administrator"));

    options.AddPolicy("CustomerPolicy", policy =>
        policy.RequireRole("Customer", "Administrator"));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Administrator"));
});



builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddHostedService<FactorGenerationService>();
builder.Services.AddHostedService<CustomerRoleDowngradeService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFactorRepository, FactorRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

app.UseCors("AllowAll");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
