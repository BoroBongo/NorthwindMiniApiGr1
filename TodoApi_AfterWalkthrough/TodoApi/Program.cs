using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TodoApi.Models;

namespace TodoApi;
public class Program
    {
    public IConfiguration Configuration { get; }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(this.Configuration.GetConnectionString("DBConnection")));
        builder.Services.AddControllers();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers(); ;

        app.Run();

    }


    // Retrieves a connection string by name.
    // Returns null if the name is not found.
    static string GetConnectionStringByName(string name)
    {
        // Assume failure.
        string returnValue = "Data Source = (localdb)/MSSQLLocalDB; Initial Catalog = Northwind; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

        return returnValue;
    }
}
