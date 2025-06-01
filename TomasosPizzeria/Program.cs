using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Interfaces;
using TomasosPizzeria.Data.Repos;

// ?? JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TomasosPizzeria.Service;
using TomasosPizzeria.Extentions;

var builder = WebApplication.CreateBuilder(args);

// ? Configura DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? Agrega los repositorios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// ?? Agrega TokenService para JWT
builder.Services.AddScoped<ITokenService,TokenService>();

// ?? Configura JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ? Controladores + JSON config
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ? Swagger
;
builder.Services.AddSwaggerExtended();

var app = builder.Build();

// ? Middleware
app.UseSwaggerExtended();

app.UseHttpsRedirection();
app.UseRouting();

// ?? Habilita autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
