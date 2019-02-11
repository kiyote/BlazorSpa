using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSpa.Service {
	public static class ServiceExtensions {
		public static IServiceCollection RegisterServices(this IServiceCollection services) {
			services.AddSingleton<IImageService, ImageService>();
			services.AddSingleton<IIdentificationService, IdentificationService>();
			services.AddSingleton<IStructureService, StructureService>();

			return services;
		}
	}
}
