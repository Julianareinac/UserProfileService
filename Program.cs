using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProfileService.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión a la base de datos (ajusta la cadena según tu base de datos)
builder.Services.AddDbContext<ProfileContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProfileDatabase"))); 

// Configurar la autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Aquí solo configuramos el esquema JWT, sin necesidad de validar el token
        options.RequireHttpsMetadata = false; // Si no estás usando HTTPS
        options.SaveToken = true;  // Guarda el token en el contexto de la solicitud
    });

// Añadir los servicios de la aplicación (controladores)
builder.Services.AddControllers();

var app = builder.Build();

// Verifica si las tablas existen y, si no, crea las tablas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProfileContext>();
    context.Database.EnsureCreated(); // Crea la base de datos y tablas si no existen
}

// Configurar el middleware para autenticación y autorización
app.UseAuthentication();  // Habilita la autenticación
app.UseAuthorization();   // Habilita la autorización

// Configurar la ruta de los controladores (automáticamente mapea los controladores)
app.MapControllers();

// Ejecutar la aplicación
app.Run();
