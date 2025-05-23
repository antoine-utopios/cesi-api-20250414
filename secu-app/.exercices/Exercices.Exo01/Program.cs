using System.Threading.RateLimiting;
using Exercices.Exo01.Extensions;
using Exercices.Exo01.Models;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// On relie notre section de secrets.json � une classe C# pour plus de facilit� par la suite dans la r�cup�ration de la config
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Ajout des services relatifs � Entity Framework Core
builder.Services.AddDataDependencies();

// On extrait de la section dans secrets.json les param�tres du token JWT
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

        // On r�cup�re les valeurs depuis notre variable de sorte � �viter les fautes typographiques
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Key))
    };
});


// Pour faire une application ayant une protection par les r�les, on va aussi param�trer des Policies de r�les
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

            // On param�tre notre limitation globale sous la forme d'un Fixed Window Rate Limiter
            factory: partition => new FixedWindowRateLimiterOptions
            {
                // On va faire un auto-replenishment de la fen�tre de temps
                AutoReplenishment = true,

                // On va limiter � 10 requ�tes par minute
                PermitLimit = 10,

                // On ne demande pas de mettre en attente les autres requ�tes
                QueueLimit = 0,

                // La fen�tre de temps est de 1 minute
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
