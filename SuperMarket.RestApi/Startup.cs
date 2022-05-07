using Autofac;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                    {Title = "BookStore.RestAPI", Version = "v1"});
        });
    }


    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<EFDataContext>()
            .WithParameter("connectionString",
                Configuration["ConnectionString"])
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(
                typeof(EFCategoryRepository).Assembly)
            .AssignableTo<Repository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(CategoryAppService).Assembly)
            .AssignableTo<Service>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<EFUnitOfWork>()
            .As<UnitOfWork>()
            .InstancePerLifetimeScope();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "BookStore.RestAPI v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}