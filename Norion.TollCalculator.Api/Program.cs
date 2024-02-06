using AutoMapper;
using Norion.TollCalculator.Api.Mappers.AutoMapper;
using Norion.TollCalculator.Application.Service;
using Norion.TollCalculator.Domain.Context;
using Norion.TollCalculator.Domain.Repository;
using Norion.TollCalculator.Domain.Service;
using Norion.TollCalculator.Infrastructure.Context;
using Norion.TollCalculator.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();


builder.Services.AddSingleton<ITollRepository, TollRepository>();
builder.Services.AddSingleton<ITollService, TollService>();
builder.Services.AddSingleton<IVehicleContext, VehicleContext>();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
