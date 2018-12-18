using System;

namespace BlazorSpa.Repository.S3 {
	public class S3Options {
		public string CredentialsFile { get; set; }

		public string CredentialsProfile { get; set; }

		public string RegionEndpoint { get; set; }

		public string ServiceUrl { get; set; }

		public string Role { get; set; }

		public string Bucket { get; set; }
	}
}
