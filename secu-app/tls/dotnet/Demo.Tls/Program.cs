var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpsRedirection(options =>
{
    // Si un client cherche à atteindre la version HTTP de l'application, il va recevoir un status 308 suivi du lien de redirection
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;

    // Vers la version HTTPS avec le bon port
    options.HttpsPort = 7291;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
