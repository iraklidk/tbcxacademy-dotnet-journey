var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContainer(builder);

var app = builder.Build();

await app.RegisterMiddlewares().ConfigureAwait(false);

await app.InitializeDatabaseAsync().ConfigureAwait(false);

app.Run();
