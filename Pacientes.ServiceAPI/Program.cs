using MassTransit;
using Microsoft.EntityFrameworkCore;
using pacientes.ServiceAPI.Services;
using Pacientes.ServiceAPI.Consumers;
using Pacientes.ServiceAPI.Data;
using Pacientes.ServiceAPI.Interfaces;
using Pacientes.ServiceAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PacientesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPacienteRepository, PacienteRepository>();
builder.Services.AddTransient<IPacienteService, PacienteService>();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});


builder.Services.AddMassTransit(x =>
{
    // Registramos o consumer no MassTransit
    x.AddConsumer<ReceitaGeradaConsumer>();

    // Configuração do RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Cria automaticamente a fila e binding para o evento
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();

//Autentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "https://localhost:5001/resources";
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "Pacientes.ServiceAPI.Scope");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers().RequireAuthorization("ApiScope");
app.Run();
