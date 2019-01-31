namespace BlazorSpa.Repository.DynamoDb {
	public sealed class DynamoDbOptions {

		public string CredentialsProfile { get; set; }

		public string RegionEndpoint { get; set; }

		public string ServiceUrl { get; set; }

		public string CredentialsFile { get; set; }

		public string Role { get; set; }
	}
}
