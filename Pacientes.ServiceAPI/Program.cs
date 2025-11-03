using Microsoft.EntityFrameworkCore;
using pacientes.ServiceAPI.Services;
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
