using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

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
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddCors(options =>
            {
                //mudar quando rodar o sistema
                options.AddPolicy("PermitirTudo", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                    {
                        Modifiers =
                        {
                                ti =>
                                {
                                    if (ti.Type == typeof(Cliente)) 
                                    {
                                        ti.PolymorphismOptions = new JsonPolymorphismOptions
                                        {
                                            TypeDiscriminatorPropertyName = "$type",
                                            IgnoreUnrecognizedTypeDiscriminators = true,
                                            DerivedTypes =
                                            {
                                                new JsonDerivedType(typeof(ClienteFisico), "fisico"),
                                                new JsonDerivedType(typeof(ClienteJuridico), "juridico")
                                            }
                                        };
                                    }
                                }
                        }
                    };

                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseCors("PermitirTudo");
            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
