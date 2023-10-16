using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DatabaseSaverWebAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

// Get the Key Vault URL
var keyVaultUrl = Environment.GetEnvironmentVariable("KVURL");

if (!string.IsNullOrEmpty(keyVaultUrl))
{
    logger.LogInformation("KeyVault URL retrieved!");

    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
}
else
{
    logger.LogInformation("KeyVault URL not found in config");
}

// Get SQL Connection String from Key Vault
string secretName = Environment.GetEnvironmentVariable("KVSecretName") ?? "YourSecretName"; // Replace "YourSecretName" with the name of your secret if it's not in an environment variable

var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
string sqlConnectionString = secretClient.GetSecret(secretName).Value.Value;

if (!string.IsNullOrEmpty(sqlConnectionString))
{
    logger.LogInformation("SQL connection string retrieved from Key Vault.");
}
else
{
    logger.LogInformation("SQL connection string retrieval error from Key Vault.");
}

builder.Services.AddDbContext<FormSubmissionContext>(options => options.UseSqlServer(sqlConnectionString));

builder.Services.AddControllers();

// Swagger Configuration
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

app.UseAuthorization();

app.MapControllers();

app.Run();