using Consultas.ServiceAPI.Data;
using Consultas.ServiceAPI.Interfaces;
using Consultas.ServiceAPI.Repositories;
using Consultas.ServiceAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ConsultasDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IMedicoRepository, MedicoRepositoryHttp>();
builder.Services.AddTransient<IMedicoService, MedicoService>();

builder.Services.AddTransient<IConsultaRepository, ConsultaRepository>();
builder.Services.AddTransient<IConsultaService, ConsultaService>();

builder.Services.AddTransient<IPacienteRepository, PacienteRepositoryHttp>();
builder.Services.AddTransient<IPacienteService, PacienteService>();

builder.Services.AddHttpClient<IMedicoRepository, MedicoRepositoryHttp>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            // Ignora certificados não confiáveis (só em DEV)
            ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true
        });

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

//MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddAuthorization();
builder.Services.AddControllers();

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
app.MapControllers();
app.Run();
