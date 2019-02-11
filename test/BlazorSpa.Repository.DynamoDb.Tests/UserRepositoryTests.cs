using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorSpa.Repository;
using BlazorSpa.Repository.DynamoDb;
using BlazorSpa.Repository.DynamoDb.Model;
using BlazorSpa.Repository.Model;
using Moq;
using NUnit.Framework;

namespace Tests {
	public class UserRepositoryTests {

		private IUserRepository _userRepository;
		private Mock<IDynamoDBContext> _context;

		[SetUp]
		public void Setup() {
			_context = new Mock<IDynamoDBContext>( MockBehavior.Strict );
			_userRepository = new UserRepository( _context.Object );
		}

		[Test]
		public async Task GetUser_InvalidUserId_NoUserReturned() {
			/*
			var userId = new Id<User>();
			var userKey = UserRecord.GetKey( userId.Value );
			var result = new AsyncSearch<UserRecord>();

			_context
				.Setup( c => c.QueryAsync<UserRecord>( userKey, QueryOperator.Equal, It.IsAny<List<object>>(), null ))
				.Returns( result );

			var actual = await _userRepository.GetUser( userId );

			_context.VerifyAll();
			*/
		}
	}
}