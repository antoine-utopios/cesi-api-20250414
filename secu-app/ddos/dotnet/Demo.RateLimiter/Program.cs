using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuration du middleware pour la limite des requ�tes
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", opt => {
        opt.Window = TimeSpan.FromSeconds(30); // On d�fini une plage de temps pour la limite des requ�tes (ici 5min)
        opt.PermitLimit = 5; // On va autoriser 5 requ�tes � �tre trait�es durant cette limite de temps
        opt.QueueLimit = 0; // Puis si l'on en a d'autres, on va les mettre dans une queue qui va en accepter 20 maxi
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Les requ�tes mises dans la queue seront trait�e de la plus ancienne � la plus r�cente
    });


    // En cas de nombre trop important de requ�te, on est cens� retourner un code de status informant le client HTTP que trop de requ�tes touchent le endpoint vis� actuellement
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

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

// On active le middleware de limitation des requ�tes
app.UseRateLimiter(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
