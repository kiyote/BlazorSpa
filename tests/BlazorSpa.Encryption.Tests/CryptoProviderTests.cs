using BlazorSpa.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlazorSpa.Encryption.Tests {
	[TestClass]
	public class CryptoProviderTests {

		[TestMethod]
		public void Create_ValidId_KeysCreated() {
			ICryptoProvider provider = new CryptoProvider();
			var keyId = new Id<KeyPair>();

			var keyPair = provider.Create( keyId );

			Assert.IsNotNull( keyPair );
			Assert.AreEqual( keyPair.Id, keyId );
			Assert.IsNotNull( keyPair.Public );
			Assert.IsNotNull( keyPair.Private );
		}

		[TestMethod]
		public void Encrypt_ValidPayload_PayloadEncrypted() {
			ICryptoProvider provider = new CryptoProvider();
			var keyId = new Id<KeyPair>();
			var keyPair = provider.Create( keyId );
			var testValue = "test_value";

			var encryptedValue = provider.Encrypt( keyPair.Public, testValue );

			Assert.IsNotNull( encryptedValue );
		}

		[TestMethod]
		public void Decrypt_ValidPayload_PayloadDecrypted() {
			ICryptoProvider provider = new CryptoProvider();
			var keyId = new Id<KeyPair>();
			var keyPair = provider.Create( keyId );
			var testValue = "test_value";
			var encryptedValue = provider.Encrypt( keyPair.Public, testValue );

			var decryptedValue = provider.Decrypt( keyPair.Private, encryptedValue );

			Assert.AreEqual( testValue, decryptedValue );
		}
	}
}
