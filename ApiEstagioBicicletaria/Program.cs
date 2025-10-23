using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ContextoDb>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
