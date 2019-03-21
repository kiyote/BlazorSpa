using Microsoft.AspNetCore.Blazor.Hosting;

namespace BlazorSpa.Client {
	public sealed class Program {
		public static void Main( string[] args ) {
			CreateHostBuilder( args ).Build().Run();
		}

		public static IWebAssemblyHostBuilder CreateHostBuilder( string[] args ) =>
			BlazorWebAssemblyHost.CreateDefaultBuilder()
				.UseBlazorStartup<Startup>();
	}
}
