using Application.AppData;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Service;
using FluentValidation;
using Infrastructure.CrossCutting.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Services;
using Services.Validators;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            var chaveToken = this.Configuration.GetValue<string>("Seguranca:ChaveToken");

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(chaveToken))
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Name = "PDA Soluções",
                        Email = "contato@pdasolucoes.com.br",
                        Url = new Uri("http://pdasolucoes.com.br/")
                    },
                    Description = "Dashboard",
                    Title = "PDA Soluções",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Por favor insira o token JWT no campo abaixo"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            this.ConfigurarInfra(chaveToken, services);
            this.ConfigurarMapper(services);
            this.ConfigurarValidador(services);
            this.ConfigurarServico(services);
            this.ConfigurarRedis(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dashoboard v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors(option => option.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigurarServico(IServiceCollection services)
        {
            var connectionString = this.Configuration.GetValue<string>("ConnectionStrings:dbConnectionString");

            services.AddScoped<IDbConnection>(x => { return new SqlConnection(connectionString); });
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICatalogoService, CatalogoService>();
        }

        private void ConfigurarInfra(string chaveToken, IServiceCollection services)
        {
            var tempoVidaToken = this.Configuration.GetValue<string>("Seguranca:TempoVidaToken");

            services.AddSingleton(this.Configuration);
            services.AddSingleton<IJwt, Jwt>(_ => new Jwt(chaveToken, TimeSpan.Parse(tempoVidaToken)));
        }

        private void ConfigurarValidador(IServiceCollection services)
        {
            services.AddScoped<IValidator<Catalogo>, CatalogoValidator>();
            services.AddScoped<IValidator<Produto>, ProdutoValidator>();
            services.AddScoped<IValidator<Categoria>, CategoriaValidator>();
        }
        private void ConfigurarMapper(IServiceCollection services)
        {
            var maperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            try
            {
                services.AddSingleton(maperConfig.CreateMapper());
            }
            finally
            {
                maperConfig = null;
            }
        }

        private void ConfigurarRedis(IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
        }
    }
}
