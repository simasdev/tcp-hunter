using Microsoft.Extensions.DependencyInjection;
using TCPHunter.TCPHelper.Common.Interfaces;
using TCPHunter.TCPHelper.Implementation;

namespace TCPHunter.TCPHelper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTCPHunter(this IServiceCollection services)
        {
            services.AddTransient<ITCPReader, TCPReader>();
            services.AddTransient<ITCPManager, TCPManager>();

            return services;
        }
    }
}
