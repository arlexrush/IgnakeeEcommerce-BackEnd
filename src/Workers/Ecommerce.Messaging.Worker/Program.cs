using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Messaging.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));
builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.SectionName));
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
        ?? builder.Configuration.GetConnectionString("ConnectionString")
        ?? $"Host={builder.Configuration["POSTGRES_HOST"] ?? "localhost"};" +
           $"Port={builder.Configuration["POSTGRES_PORT"] ?? "5432"};" +
           $"Database={builder.Configuration["POSTGRES_DB"] ?? "ecommerce"};" +
           $"Username={builder.Configuration["POSTGRES_USER"] ?? "postgres"};" +
           $"Password={builder.Configuration["POSTGRES_PASSWORD"] ?? "postgres"}";

    options.UseNpgsql(connectionString, databaseOptions =>
        databaseOptions.MigrationsAssembly(typeof(EcommerceDbContext).Assembly.FullName));
});
builder.Services.AddScoped<OrderCreatedEventHandler>();
builder.Services.AddHostedService<RabbitMqOrderWorker>();

var host = builder.Build();

await using (var scope = host.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
    await dbContext.Database.MigrateAsync();
}

await host.RunAsync();
