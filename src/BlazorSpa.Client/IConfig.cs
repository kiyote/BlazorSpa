using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSpa.Client {
	public interface IConfig {
		string Host { get; }

		string CongnitoUrl { get; }

		string TokenUrl { get; }

		string AuthUrl { get; }

		string CognitoClientId { get; }
	}
}
