using System;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using ProductsApiApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var constr = "Data Source=34.253.172.150;Initial Catalog=Northwind;User ID=sa;Password=Passw0rd;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//SqlConnection conn = new SqlConnection();
//conn.ConnectionString = constr;

var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddCors();
builder.Services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(connectionStr));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Swagger API for Northwind database",
            Description = "This is the swagger documentation for the product, category, suppliers tables based on the northwind database",
            Version = "v1"
        });
    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    options.IncludeXmlComments(filePath);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API for Northwind database");
        //sets the base url as swagger
        c.RoutePrefix = string.Empty;
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
