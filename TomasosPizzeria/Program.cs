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

//Vi l�ser upp v�rden fr�n appsettings.json och l�gger i variabler. Med en 
//managed identity blir det bara url:en som beh�vs h�r, resten s�tts upp i Azure
//Web api:et m�ste deployas till Azure f�r att det skall fungera
var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");


//Detta l�gger till v�rdet fr�n v�r keyvault till IConfiguration vilket g�r att det
//g�r att komma �t i hela applikationen genom DI. Detta beh�ver man inte g�ra om v�rden 
//bara skall anv�ndas h�r i program.cs
builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(),
    new DefaultKeyVaultSecretManager());



//Skapa en klient och verifiera sig mot Azure keyvault tj�nsten
var credential = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
    new AzureServiceTokenProvider().KeyVaultTokenCallback));
var client = new SecretClient(new Uri("https://pizzakey.vault.azure.net/"),
    new DefaultAzureCredential());

//H�mta ett v�rde. En connstring skulle h�r tex kunna anv�ndas n�r vi i
//n�sta steg s�tter upp EF
var connString = client.GetSecret("constring").Value.Value.ToString();


// ? Configura DbContext con la cadena de conexi�n
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

// ?? Habilita autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
