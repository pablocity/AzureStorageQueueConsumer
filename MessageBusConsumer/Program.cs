using MessageBusConsumer;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddTransient<IQueueService, AzureStorageQueueService>();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(Program)));

builder.Services.Configure<MediatorOptions>(builder.Configuration.GetSection(nameof(MediatorOptions)));
builder.Services.Configure<AzureStorageQueueOptions>(builder.Configuration.GetSection(nameof(AzureStorageQueueOptions)));

var app = builder.Build();

app.Run();
