using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BlazorSpa.Repository.Model;
using BlazorSpa.Shared;
using Moq;
using NUnit.Framework;

namespace BlazorSpa.Repository.S3.Tests {
	[TestFixture]
	public class ImageRepositoryUnitTests {

		private IImageRepository _repository;
		private Mock<IAmazonS3> _client;
		private S3Options _options;

		[SetUp]
		public void TestInitialize() {
			_client = new Mock<IAmazonS3>( MockBehavior.Strict );
			_options = new S3Options {
				Bucket = "test"
			};
			_repository = new ImageRepository( _client.Object, _options );
		}

		[Test]
		public async Task Add_RequestSuccessful_UrlReturned() {
			var imageId = new Id<Image>();
			var contentType = "contentType";
			var content = Convert.ToBase64String( Encoding.UTF8.GetBytes( "content" ) );
			var expected = $"https://{_options.Bucket}.s3.amazonaws.com/image_{imageId.Value}";

			string requestBucket = default;
			string requestKey = default;
			S3CannedACL requestCannedACL = default;
			string requestContentType = default;
			long requestInputStreamLength = -1L;
			_client
				.Setup( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ) )
				.Callback<PutObjectRequest, CancellationToken>( ( request, token ) => {
					requestBucket = request.BucketName;
					requestKey = request.Key;
					requestCannedACL = request.CannedACL;
					requestContentType = request.ContentType;
					requestInputStreamLength = request.InputStream.Length;
				} )
				.Returns( Task.FromResult( new PutObjectResponse {
					HttpStatusCode = HttpStatusCode.OK
				} ) );

			var actual = await _repository.Add( imageId, contentType, content );

			Assert.AreEqual( expected, actual.Url );
			Assert.AreEqual( _options.Bucket, requestBucket );
			Assert.AreEqual( $"image_{imageId.Value}", requestKey );
			Assert.AreEqual( S3CannedACL.PublicRead, requestCannedACL );
			Assert.AreEqual( contentType, requestContentType );
			Assert.AreEqual( "content".Length, requestInputStreamLength );
		}

		[Test]
		public async Task Add_S3Failture_Retried() {
			var imageId = new Id<Image>();
			var contentType = "contentType";
			var content = Convert.ToBase64String( Encoding.UTF8.GetBytes( "content" ) );
			_client
				.Setup( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ) )
				.Returns( Task.FromResult( new PutObjectResponse {
					HttpStatusCode = HttpStatusCode.BadRequest
				} ) );

			var actual = await _repository.Add( imageId, contentType, content );

			_client.Verify( c => c.PutObjectAsync( It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>() ), Times.AtLeast( 2 ) );
			Assert.IsNull( actual );
		}

		[Test]
		public async Task Exists_NotInS3_ReturnsFalse() {
			var imageId = new Id<Image>();
			_client.Setup( c => c.GetObjectMetadataAsync( It.IsAny<GetObjectMetadataRequest>(), default ))
				.Callback<GetObjectMetadataRequest, CancellationToken>( ( req, tok ) => { throw new AmazonS3Exception( "message", ErrorType.Receiver, "NotFound", "requestId", HttpStatusCode.OK ); } )
				.Returns( Task.FromResult<GetObjectMetadataResponse>(default) );

			var actual = await _repository.Exists( imageId );

			Assert.IsFalse( actual );
		}
	}
}