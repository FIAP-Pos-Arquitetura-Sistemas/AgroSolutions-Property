using AgroSolutions.FarmService.Data;
using AgroSolutions_FarmService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Adicione este para o Swagger
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuaChaveSuperSecretaAgroSolutions2026!";
var key = Encoding.ASCII.GetBytes(jwtKey);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FarmDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgroSolutions Farm API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite 'Bearer' [espaçoo] e o seu token.\n Exemplo: Bearer eyJhbG..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IFarmService, FarmService>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(x =>
{
    x.IncludeErrorDetails = true; 
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "AgroSolutionsIdentity",
        ValidateAudience = true,
        ValidAudience = "AgroSolutionsApps",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FarmDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    // Tenta até 3 vezes com um intervalo de 10 segundos entre elas
    int retryCount = 0;
    bool success = false;

    while (!success && retryCount < 3)
    {
        try
        {
            logger.LogInformation("Tentando aplicar migrations (Tentativa {0})...", retryCount + 1);
            context.Database.Migrate();
            logger.LogInformation("Migrations aplicadas ou banco já atualizado!");
            success = true;
        }
        catch (Exception ex)
        {
            retryCount++;
            logger.LogWarning("Banco de dados ainda indisponível. Aguardando 10s para tentar novamente...");
            if (retryCount >= 3) logger.LogError(ex, "Falha definitiva ao conectar no banco.");
            Thread.Sleep(10000); // Aguarda 10 segundos para o banco Serverless subir
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroSolutions Farm API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();