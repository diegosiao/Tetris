using Microsoft.Extensions.DependencyInjection;

namespace Tetris.Core
{
    public interface ITetrisStartup
    {
        void ConfigureServices(IServiceCollection services);

        void Configure(params object[] dependencies);
    }
}
