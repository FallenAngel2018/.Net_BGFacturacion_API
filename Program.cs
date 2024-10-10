using BgTiendaFacturacionAPI.Domain.Client;
using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Domain.User;
using BgTiendaFacturacionAPI.Infrastructure.Client;
using BgTiendaFacturacionAPI.Infrastructure.Connection;
using BgTiendaFacturacionAPI.Infrastructure.Product;
using BgTiendaFacturacionAPI.Infrastructure.User;


var builder = WebApplication.CreateBuilder(args);

// Fuente: https://stackoverflow.com/questions/76050706/how-to-enable-cors-in-a-c-sharp-application
// Add services to the container.
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    // builder.WithOrigins("*")
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();

    // U Can Filter Here
}));

// Add services to the container.
//builder.Services.UseCors(x => x
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader());

builder.Services.AddControllers()
    // Fuente: chatgpt - Cómo puedo lograr que los datos que se van a devolver en json comiencen con mayúscula?
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantiene PascalCase
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Set Connection string secrets from current dir
//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");

// Fuente: https://learn.microsoft.com/en-us/answers/questions/1320240/how-do-i-access-secrets-json-from-any-class
// Set Connection string secrets from VS tool
builder.Configuration.AddJsonFile("secrets.json",
        optional: true,
        reloadOnChange: true);
//register the service 
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();



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

// Fuente: https://stackoverflow.com/questions/56328474/origin-http-localhost4200-has-been-blocked-by-cors-policy-in-angular7
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.Run();
