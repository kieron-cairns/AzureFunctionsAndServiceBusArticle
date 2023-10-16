using Azure.Identity;
using DatabaseSaverWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// var keyVaultUrl = builder.Configuration["KVURL"];

var keyVaultUrl = Environment.GetEnvironmentVariable("KVURL");

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

if(!string.IsNullOrEmpty(keyVaultUrl))
{

    logger.LogInformation("$KeyVaultUrl URL retrieved!");

    var credential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
}
else
{
    logger.LogInformation("KeyVault URL not found in config");
}

var sqlConnectionString = Environment.GetEnvironmentVariable("KVSecretName");
if (!string.IsNullOrEmpty(sqlConnectionString))
{

    logger.LogInformation("SQL connection string retrieved");
}
else 
{
    logger.LogInformation("SQL connection string retrieval error");
}

builder.Services.AddDbContext<FormSubmissionContext>(options => options.)

    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
