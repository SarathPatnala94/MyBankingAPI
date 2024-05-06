using MyBankingAPI.Interfaces;
using MyBankingAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var baseDirectory = Directory.GetCurrentDirectory();

// Construct the relative paths to the JSON files
var accountsFilePath = Path.Combine(baseDirectory, "files", "accounts.json");
var customersFilePath = Path.Combine(baseDirectory, "files", "customers.json");

// Register repositories with the relative file paths
builder.Services.AddSingleton<IAccountRepository>(new JsonAccountRepository(accountsFilePath));
builder.Services.AddSingleton<ICustomerRepository>(new JsonCustomerRepository(customersFilePath));

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
