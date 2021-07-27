namespace MessageSending
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class EnvironmentConfiguration : IEnvironmentConfiguration
    {
        public EnvironmentConfiguration(IWebHostEnvironment environment)
        {
            IsProduction = environment.IsProduction();
        }

        public bool IsProduction { private set; get; }
    }
}
