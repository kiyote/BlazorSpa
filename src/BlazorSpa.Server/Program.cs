using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

namespace BlazorSpa.Server {
	public sealed class Program {
		public static void Main( string[] args ) {
			var isService = !( Debugger.IsAttached || args.Contains( "--console" ) );
			IWebHostBuilder builder = BuildWebHost( args );

			if( isService ) {
				string pathToExe = Process.GetCurrentProcess().MainModule.FileName;
				string pathToContentRoot = Path.GetDirectoryName( pathToExe );
				builder.UseContentRoot( pathToContentRoot );
			}

			var host = builder.Build();

			if( isService ) {
				host.RunAsService();
			} else {
				host.Run();
			}
		}

		public static IWebHostBuilder BuildWebHost( string[] args ) =>
			WebHost.CreateDefaultBuilder( args )
				.UseConfiguration( new ConfigurationBuilder()
					.AddCommandLine( args )
					.Build() )
				.UseStartup<Startup>();
	}
}
