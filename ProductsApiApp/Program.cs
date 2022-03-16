using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductsApiApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var constr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

SqlConnection conn = new SqlConnection();
conn.ConnectionString = constr;
conn.Open();
Console.WriteLine(conn.State);

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(conn));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
