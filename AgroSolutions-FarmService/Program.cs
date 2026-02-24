using AgroSolutions.FarmService.Data;
using AgroSolutions_FarmService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Adicione este para o Swagger
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuaChaveSuperSecretaAgroSolutions2026!";
var key = Encoding.ASCII.GetBytes(jwtKey);

// 2. Banco de Dados
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FarmDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. REGISTRO DO SWAGGER (O que estava faltando e causou o erro)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgroSolutions Farm API", Version = "v1" });

    // Configuração para permitir colar o Token JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite 'Bearer' [espaço] e o seu token.\n Exemplo: Bearer eyJhbG..."
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

// 4. Registro do seu Service de Negócio
builder.Services.AddScoped<IFarmService, FarmService>();

// 5. Autenticação e Autorização
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(x =>
{
    x.IncludeErrorDetails = true; // Isso ajudará a ver o erro real no log do console
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
        ClockSkew = TimeSpan.Zero // Remove a tolerância de 5 min do .NET para testes precisos
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline de Execução
app.UseHttpsRedirection();
app.UseStaticFiles();

// O Swagger agora encontrará os serviços registrados acima
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