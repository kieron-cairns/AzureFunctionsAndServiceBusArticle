using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DatabaseSaverWebAPI.Data;
using DatabaseSaverWebAPI.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

builder.Logging.AddConsole();  
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

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
string secretName = Environment.GetEnvironmentVariable("KVSecretName") ?? "SQLServerConnectionString"; // Replace "YourSecretName" with the name of your secret if it's not in an environment variable

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<FormSubmissionContext>(options => options.UseSqlServer(sqlConnectionString));
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IFormSubmissionContext, FormSubmissionContext>();


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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(exceptionHandlerPathFeature.Error, "An error occurred processing your request.");

                await context.Response.WriteAsync("<h1>An error occurred while processing your request.</h1>");
                await context.Response.WriteAsync("<p>Please try again later.</p>");
            }
        });
    });
}


app.Run();