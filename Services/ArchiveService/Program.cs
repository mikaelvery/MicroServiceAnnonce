using ArchiveService.Data;
using ArchiveService.Services;
using ArchiveService.Consumers;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<AdServiceHttpClient>().AddPolicyHandler(GetPolicy());
// Rabbit MQ
builder.Services.AddMassTransit(x => 
{
    x.AddConsumersFromNamespaceContaining<AdCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("archiveAd", false));

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

// Configure the HTTP request pipeline.

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(3 * retryAttempt));
