namespace MessageSending
{
    using Amazon;
    using Amazon.SecretsManager;
    using Amazon.SimpleEmailV2;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IAmazonSimpleEmailServiceV2, AmazonSimpleEmailServiceV2Client>(
                _ => new AmazonSimpleEmailServiceV2Client(RegionEndpoint.EUWest3));
            services.AddSingleton<IMessageSendingConfiguration, MessageSendingConfiguration>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHttpClient(Constants.RecaptchaApiClient, configureClient =>
                configureClient.BaseAddress = new System.Uri("https://www.google.com/recaptcha/api/"));
            services.AddSingleton<IMessageSendingRequestVerifiyer, MessageSendingRequestVerifiyer>();
            services.AddSingleton<IVerifyServiceConfiguration, VerifyServiceConfiguration>();
            services.AddSingleton<IAmazonSecretsManager, AmazonSecretsManagerClient>(
                _ => new AmazonSecretsManagerClient(RegionEndpoint.EUWest3));
            services.AddSingleton<IEnvironmentConfiguration, EnvironmentConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireCors(configure =>
                    {
                        configure.AllowAnyHeader();
                        configure.AllowAnyMethod();
                        configure.AllowAnyOrigin();
                    });
            });
        }
    }
}
