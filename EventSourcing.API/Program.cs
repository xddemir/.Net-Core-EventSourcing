using System.Reflection;
using EventSourcing.API.BackgroundServices;
using EventSourcing.API.EventStores;
using EventSourcing.API.Extensions;
using EventSourcing.API.Models;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
builder.Services.AddEventStore(builder.Configuration);
builder.Services.AddSingleton<ProductStream>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddWorkerServices();


var app = builder.Build();  

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();