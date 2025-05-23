using System.Threading.RateLimiting;
using Exercices.Exo01.Extensions;
using Exercices.Exo01.Models;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// On relie notre section de secrets.json à une classe C# pour plus de facilité par la suite dans la récupération de la config
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Ajout des services relatifs à Entity Framework Core
builder.Services.AddDataDependencies();

// On extrait de la section dans secrets.json les paramètres du token JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

// On ajoute la configuration de notre API avec utilisation d'un JWT pour l'Authentication
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // On récupère les valeurs depuis notre variable de sorte à éviter les fautes typographiques
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Key))
    };
});


// Pour faire une application ayant une protection par les rôles, on va aussi paramétrer des Policies de rôles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin", "User"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
        options.QueueLimit = 2;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
        // On va partitionner par utilisateur, donc on va utiliser le nom de l'utilisateur
        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),

            // On paramètre notre limitation globale sous la forme d'un Fixed Window Rate Limiter
            factory: partition => new FixedWindowRateLimiterOptions
            {
                // On va faire un auto-replenishment de la fenêtre de temps
                AutoReplenishment = true,

                // On va limiter à 10 requêtes par minute
                PermitLimit = 10,

                // On ne demande pas de mettre en attente les autres requêtes
                QueueLimit = 0,

                // La fenêtre de temps est de 1 minute
                Window = TimeSpan.FromMinutes(1)
            });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
