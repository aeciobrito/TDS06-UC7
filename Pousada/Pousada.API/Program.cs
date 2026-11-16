using Microsoft.EntityFrameworkCore;
using Pousada.API.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 1. Registrar o serviço de CORS permitindo qualquer origem (incluindo "null" de arquivo local)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
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

// 2. Aplicar o CORS no pipeline (OBRIGATÓRIO: antes de UseAuthorization e MapControllers)
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();