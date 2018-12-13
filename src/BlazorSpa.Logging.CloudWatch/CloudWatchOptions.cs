using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BlazorSpa.Logging.CloudWatch {
	public sealed class CloudWatchOptions {
		public string Region { get; set; }

		public string CredentialsProfile { get; set; }

		public string LogGroup { get; set; }

		public string StreamName { get; set; }

		public string CredentialsFile { get; set; }

		public string Role { get; set; }

		public LogLevel LogLevel { get; set; }

	}
}
