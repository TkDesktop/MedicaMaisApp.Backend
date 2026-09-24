using System.Text;
using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resend;

var builder = WebApplication.CreateBuilder(args);

// ---------- Banco de dados (MySQL via Pomelo EF Core) ----------
var connectionString = builder.Configuration.GetConnectionString("Padrao");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// ---------- Serviços de domínio ----------
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<IPressaoService, PressaoService>();
builder.Services.AddScoped<IHumorService, HumorService>();
builder.Services.AddScoped<IVisitaService, VisitaService>();
builder.Services.AddScoped<IAssinaturaService, AssinaturaService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ---------- Autenticação JWT ----------
var chaveJwt = builder.Configuration["Jwt:Chave"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Emissor"],
        ValidAudience = builder.Configuration["Jwt:Audiencia"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt))
    };
});

builder.Services.AddAuthorization();

// ---------- CORS (para o front-end estático consumir a API) ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
        // Em produção, troque AllowAnyOrigin() por WithOrigins("https://seu-dominio.com")
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Medica+ API", Version = "v1" });

    // Permite testar endpoints autenticados direto pelo Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

// Aplica migrations pendentes automaticamente ao iniciar (cria as tabelas no MySQL na primeira execução)
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    contexto.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("PermitirTudo");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();