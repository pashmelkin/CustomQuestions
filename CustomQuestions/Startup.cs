using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using MediatR;
using CustomQuestions.Framework;
using Autofac;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;

namespace CustomQuestions
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Custom Questions API", Version = "v1" });
            });
            services.AddMediatR();
            services.AddOptions();           
            services.Configure<PostgreSQLSettings>(options => { Configuration.GetSection("PostgreSQL").Bind(options); });

            // services.AddSingleton(serviceProvider => new HttpClient());
            services.AddScoped<IHttpClientFactory, HttpClientFactory>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
               .AsSelf()
               .AsImplementedInterfaces();

            return new AutofacServiceProvider(builder.Build());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Custom Questions API V1");
            });

        }
    }
}
