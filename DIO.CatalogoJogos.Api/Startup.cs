using DIO.CatalogoJogos.Api.Extensoes;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DIO.CatalogoJogos.Api
{
  public class Startup
  {
    private IWebHostEnvironment _appHost;

    public Startup(IConfiguration configuration, IWebHostEnvironment appHost)
    {
      Configuration = configuration;
      _appHost = appHost;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DIO.CatalogoJogos.Api", Version = "v1" });
      });
      services.AddAutoMapper(typeof(Mapeamentos.Produtora));
      services.AddServicos();
      services.AddRepositorios();

      var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString($"Data Source={_appHost.ContentRootPath}/{Configuration.GetConnectionString("SqliteConnection")}")
        // .WithGlobalConnectionString($"Data Source=/home/ejandrade/Documentos/projetos-DIO/API-catalogo-de-jogos-dio-dotnet/DIO.CatalogoJogos.Testes/bin/Debug/net5.0/CatalogoJogos.db")
        .ScanIn(typeof(Migracoes._20211010172415_CriarTabelaProdutora).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider(false);

      using (var scope = serviceProvider.CreateScope())
      {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
      }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DIO.CatalogoJogos.Api v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
