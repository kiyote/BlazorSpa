using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BlazorSpa.Repository.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazorSpa.Repository.S3 {
	public class AvatarRepository : IAvatarRepository {

		private const int RetryCount = 3;
		private readonly IAmazonS3 _client;
		private readonly string _bucket;

		public AvatarRepository(
			IAmazonS3 client,
			S3Options options
		) {
			_client = client;
			_bucket = options.Bucket;
		}

		async Task<string> IAvatarRepository.SetAvatar( Id<User> userId, string contentType, string content) {
			using( var image = Image.Load( Convert.FromBase64String( content) ) ) {
				if( ( image.Width != 64 ) || ( image.Height != 64 ) ) {
					content = image.Clone( x => x.Resize( 64, 64 ) ).ToBase64String( ImageFormats.Png ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			var key = GetKey( userId );
			using( var ms = new MemoryStream( Convert.FromBase64String( content ) ) ) {
				var request = new PutObjectRequest() {
					BucketName = _bucket,
					Key = key,
					ContentType = contentType,
					InputStream = ms,
					CannedACL = S3CannedACL.PublicRead
				};
				bool isOk = true;
				int attemptNumber = 1;
				do {
					var response = await _client.PutObjectAsync( request );
					if (response.HttpStatusCode != HttpStatusCode.OK) {
						Task.Delay( 100 * attemptNumber ).Wait();
						attemptNumber += 1;
						isOk = false;
					}
				} while( !isOk && (attemptNumber <= RetryCount) );

				if (isOk) {
					return GenerateUrl( _bucket, key );
				}
			}

			return string.Empty;
		}

		Task<string> IAvatarRepository.GetAvatarUrl( Id<User> userId) {
			return Task.FromResult( GenerateUrl( _bucket, GetKey(userId) ) );
		}

		async Task IAvatarRepository.DeleteAvatar( Id<User> userId ) {
			var request = new DeleteObjectRequest {
				BucketName = _bucket,
				Key = GetKey(userId)
			};
			await _client.DeleteObjectAsync( request );
		}

		private static string GetKey(Id<User> userId) {
			return $"avatar_{userId.Value}";
		}

		private static string GenerateUrl(string bucket, string key) {
			return $"https://{bucket}.s3.amazonaws.com/{key}";
		}
	}
}
