using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EfCo.UowRepository
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            //iniciando projeto
            services.AddControllers()
            .AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EfCo.UowRepository", Version = "v1" });
            });
            //String de conexão
            var srtConnection = "Server=localhost; Database=UoW; Trusted_Connection=True; TrustServerCertificate=True;";
            services.AddDbContext<ApplicationContext>(p =>p.UseSqlServer(srtConnection));

            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EfCo.UowRepository v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void InializarBaseDeDados(IApplicationBuilder app)
        {
            using var db = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationContext>();

            if(db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(Enumerable.Range(1, 10)
                .Select(p => new Departamento
                {
                    Descricao = $"Departamento - {p}",
                    Empregados = Enumerable.Range(1, 10)
                    .Select(x => new Empregado
                        {
                            Nome = $"Empregado: {x}/{p}"
                        }).ToList()
                }));
                
                db.SaveChanges();
            }
        }
    }
}
