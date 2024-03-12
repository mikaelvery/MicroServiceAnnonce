// using ArchiveService.consumers;
using AdService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
// Auto Mappeur afin de gerer les mapping entre les DTO et les entités
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Connect to MSSQL avec le DB Context
builder.Services.AddDbContext<ImmoIaDbContext>(option =>
{
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Add outbox
    x.AddEntityFrameworkOutbox<ImmoIaDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(30);

        o.UseSqlServer();
        o.UseBusOutbox();
    });

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("ad", false));

    // Setup RabbitMQ Endpoint
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

builder.Services.AddSwaggerGen();
