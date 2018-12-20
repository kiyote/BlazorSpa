using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BlazorSpa.Repository.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlazorSpa.Repository.S3.Tests {
	[TestClass]
	public class AvatarRepositoryTests {

		private IAvatarRepository _repository;
		private Mock<IAmazonS3> _client;
		private S3Options _options;

		[TestInitialize]
		public void TestInitialize() {
			_client = new Mock<IAmazonS3>( MockBehavior.Strict );
			_options = new S3Options {
				Bucket = "test"
			};
			_repository = new AvatarRepository( _client.Object, _options );
		}

		[TestMethod]
		public async Task SetAvatar_RequestSuccessful_UrlReturned() {
			var userId = new Id<User>();
			var contentType = "contentType";
			var content = Convert.ToBase64String( Encoding.UTF8.GetBytes( "content" ) );
			var expected = $"https://{_options.Bucket}.s3.amazonaws.com/avatar_{userId.Value}";

			string requestBucket = default;
			string requestKey = default;
			S3CannedACL requestCannedACL = default;
			string requestContentType = default;
			long requestInputStreamLength = -1L;
			_client
				.Setup( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ) )
				.Callback<PutObjectRequest, CancellationToken>((request, token) => {
					requestBucket = request.BucketName;
					requestKey = request.Key;
					requestCannedACL = request.CannedACL;
					requestContentType = request.ContentType;
					requestInputStreamLength = request.InputStream.Length;
				} )
				.Returns( Task.FromResult(new PutObjectResponse {
					HttpStatusCode = HttpStatusCode.OK
				} ) );

			var actual = await _repository.SetAvatar( userId, contentType, content );

			Assert.AreEqual( expected, actual );
			Assert.AreEqual( _options.Bucket, requestBucket );
			Assert.AreEqual( $"avatar_{userId.Value}", requestKey );
			Assert.AreEqual( S3CannedACL.PublicRead, requestCannedACL );
			Assert.AreEqual( contentType, requestContentType );
			Assert.AreEqual( "content".Length, requestInputStreamLength );
		}

		[TestMethod]
		public async Task SetAvatar_S3Failture_Retried() {
			var userId = new Id<User>();
			var contentType = "contentType";
			var content = Convert.ToBase64String( Encoding.UTF8.GetBytes( "content" ) );
			_client
				.Setup( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ) )
				.Returns( Task.FromResult( new PutObjectResponse {
					HttpStatusCode = HttpStatusCode.BadRequest
				} ) );

			var actual = await _repository.SetAvatar( userId, contentType, content );

			_client.Verify( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ), Times.AtLeast( 2 ) );
		}
	}
}
