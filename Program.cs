using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
using ApiEstagioBicicletaria.Services.ServicesLogs;
using ApiEstagioBicicletaria.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<ServicoJwt>();
            builder.Services.AddScoped(typeof(GeradorCodigoIndentificador<>));
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IServicoService, ServicoService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IVendaService, VendaService>();
            builder.Services.AddScoped<IVendedorService, VendedorService>();
            builder.Services.AddScoped<IFornecedorService, FornecedorService>();
            builder.Services.AddScoped<IEstoqueService, EstoqueService>();
            builder.Services.AddScoped<IEntradaEstoqueService, EntradaEstoqueService>();
            builder.Services.AddScoped<EntradaEstoqueRepositorio>();
            builder.Services.AddScoped<ItemEntradaEstoqueRepositorio>();
            builder.Services.AddScoped<FornecedorRepositorio>();
            builder.Services.AddScoped<ProdutoRepositorio>();
            builder.Services.AddScoped<EstoqueRepositorio>();
            builder.Services.AddScoped<UsuarioRepositorio>();
            builder.Services.AddScoped<SenhaService>();
            builder.Services.AddScoped<VendedorRepositorio>();
            builder.Services.AddScoped<UsuarioLogadoService>();
            builder.Services.AddScoped(typeof(LogRepositorio<>));
            builder.Services.AddScoped<ClienteLogService>();
            builder.Services.AddScoped<EnderecoLogService>();
            builder.Services.AddScoped<EstoqueLogService>();
            builder.Services.AddScoped<FornecedorLogService>();
            builder.Services.AddScoped<ProdutoLogService>();
            builder.Services.AddScoped<ServicoLogService>();
            builder.Services.AddScoped<VendedorLogService>();
            builder.Services.AddScoped<VendaLogService>();
            builder.Services.AddScoped<ItemVendaLogService>();
            builder.Services.AddScoped<ServicoVendaLogService>();
            builder.Services.AddScoped<ParcelaLogService>();
            builder.Services.AddScoped<TransacaoLogService>();
            builder.Services.AddScoped<UsuarioLogService>();
            builder.Services.AddScoped<EntradaEstoqueLogService>();
            builder.Services.AddScoped<ItemEntradaEstoqueLogService>();
            builder.Services.AddHttpContextAccessor();

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

                                if (ti.Type == typeof(ClienteDtoOutPut))
                                {
                                    ti.PolymorphismOptions = new JsonPolymorphismOptions
                                    {
                                        TypeDiscriminatorPropertyName = "$type",
                                        IgnoreUnrecognizedTypeDiscriminators = true,
                                        DerivedTypes =
                                        {
                                            new JsonDerivedType(typeof(ClienteFisicoDtoOutPut), "fisico"),
                                            new JsonDerivedType(typeof(ClienteJuridicoDtoOutPut), "juridico")
                                        }
                                    };
                                }
                            }
                        }
                    };
                });

            
            var jwtKey = builder.Configuration["JWT_KEY"];
        

            var bytesJwtKey=Encoding.UTF8.GetBytes(jwtKey);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(bytesJwtKey),
                        NameClaimType=ClaimTypes.Name,
                        RoleClaimType=ClaimTypes.Role,
                    };
                });

            builder.Services.AddAuthorization();



            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("PermitirTudo");

            app.UseAuthentication();
            app.UseAuthorization();
  
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            using(var scope = app.Services.CreateScope())
            {
                var contexto = scope.ServiceProvider.GetRequiredService<ContextoDb>();
                var senhaService = scope.ServiceProvider.GetRequiredService<SenhaService>();

                if (!contexto.Usuarios.Any())
                {
                    Usuario usuario = new Usuario("1234","teste","teste@gmail.com",senhaService.GerarHashDaSenha("teste"),PerfilUsuario.Admin);

                    contexto.Usuarios.Add(usuario);
                    contexto.SaveChanges();
                }
            }


            app.Run();
          

        }
    }
}
