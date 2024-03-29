using Microsoft.OpenApi.Models;
using quissile.wwwapi8.Data;
using quissile.wwwapi8.Endpoints;
using quissile.wwwapi8.Models;
using quissile.wwwapi8.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quissile API",
        Description = "Developed for Gudbrand",
        Version = "v1"
    });
});

builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IRepository<Question>, Repository<Question>>();
builder.Services.AddScoped<IRepository<Alternative>, Repository<Alternative>>();
builder.Services.AddScoped<IRepository<Quiz>, Repository<Quiz>>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactClient", builder =>
    {
        builder.WithOrigins("http://localhost:5173", "https://quizzilecli.netlify.app/")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactClient");
app.UseHttpsRedirection();
app.ConfigureQuestionEndpoint();
app.ConfigureAlternativeEndpoint();
app.ConfigureQuizEndpoint();
app.Run();

