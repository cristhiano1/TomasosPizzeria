using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

// ?? JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TomasosPizzeria.Context;
using TomasosPizzeria.Data.Interfaces;
using TomasosPizzeria.Data.Repos;
using TomasosPizzeria.Extentions;
using TomasosPizzeria.Service;

var builder = WebApplication.CreateBuilder(args);

//Vi läser upp värden från appsettings.json och lägger i variabler. Med en 
//managed identity blir det bara url:en som behövs här, resten sätts upp i Azure
//Web api:et måste deployas till Azure för att det skall fungera
var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");


//Detta lägger till värdet från vår keyvault till IConfiguration vilket gör att det
//går att komma åt i hela applikationen genom DI. Detta behöver man inte göra om värden 
//bara skall användas här i program.cs
builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(),
    new DefaultKeyVaultSecretManager());



//Skapa en klient och verifiera sig mot Azure keyvault tjänsten
var credential = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
    new AzureServiceTokenProvider().KeyVaultTokenCallback));
var client = new SecretClient(new Uri("https://pizzakey.vault.azure.net/"),
    new DefaultAzureCredential());

//Hämta ett värde. En connstring skulle här tex kunna användas när vi i
//nästa steg sätter upp EF
var connString = client.GetSecret("constring").Value.Value.ToString();


// ? Configura DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString));

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["tokenKey"]!)),
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
