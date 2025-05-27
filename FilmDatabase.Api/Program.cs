using FilmDatabase.Api.Middleware;
using FilmDatabase.Core.Interfaces;
using FilmDatabase.Core.Services;
using FilmDatabase.Database.Context;
using FilmDatabase.Database.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adaugare DbContext
builder.Services.AddDbContext<FilmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository si Service
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IFilmService, FilmService>();

// Controllers Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Film Database API",
        Version = "v1",
        Description = "API pentru gestionarea bazei de date de filme - Tema 2 DAWM\n\n" +
                     "Funcționalități implementate:\n" +
                     "• Filtrare după genre, director, title, year, minYear, maxYear\n" +
                     "• Sortare după Title, Year, Director, Genre (asc/desc)\n" +
                     "• Paginare cu pageNumber și pageSize\n" +
                     "• Endpoint PUT pentru actualizarea filmelor\n" +
                     "• Tratarea erorilor prin middleware personalizat"
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// IMPORTANT: Middleware pentru tratarea erorilor (Tema 2 - 2 puncte)
// Trebuie să fie primul middleware pentru a prinde toate excepțiile
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Film Database API V1");
        c.RoutePrefix = string.Empty; // Setează Swagger UI ca pagină principală
        c.DocumentTitle = "Film Database API - Tema 2";
        c.DefaultModelsExpandDepth(-1); // Collapsed models by default
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed data la pornirea aplicației
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FilmDbContext>();
        FilmDbContext.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();