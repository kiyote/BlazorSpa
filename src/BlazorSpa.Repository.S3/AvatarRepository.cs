using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace BlazorSpa.Repository.S3 {
	public class AvatarRepository : IAvatarRepository {

		private readonly IAmazonS3 _client;
		private readonly string _bucket;

		public AvatarRepository(
			IAmazonS3 client,
			S3Options options
		) {
			_client = client;
			_bucket = options.Bucket;
		}

		async Task<string> IAvatarRepository.SetAvatar(string userId, string contentType, string content) {
			var key = $"avatar_{userId}";
			var request = new PutObjectRequest() {
				BucketName = _bucket,
				Key = key,
				ContentType = contentType,
				InputStream = new MemoryStream( Convert.FromBase64String( content ) ),
				CannedACL = S3CannedACL.PublicRead
			};
			var response = await _client.PutObjectAsync( request );

			return GenerateUrl( _bucket, key );
		}

		Task<string> IAvatarRepository.GetAvatarUrl(string userId) {
			var key = $"avatar_{userId}";
			return Task.FromResult( GenerateUrl( _bucket, key ) );
		}

		private static string GenerateUrl(string bucket, string key) {
			return $"https://{bucket}.s3.amazonaws.com/{key}";
		}
	}
}
