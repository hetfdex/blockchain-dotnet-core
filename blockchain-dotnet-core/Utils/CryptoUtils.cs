using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace blockchain_dotnet_core.API.Utils
{
    public static class CryptoUtils
    {
        public static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var curve = ECNamedCurveTable.GetByName("secp256k1");

            var ecDomainParameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            var secureRandom = new SecureRandom();

            var ecKeyGenerationParameters = new ECKeyGenerationParameters(ecDomainParameters, secureRandom);

            var ecKeyPairGenerator = new ECKeyPairGenerator("ECDSA");

            ecKeyPairGenerator.Init(ecKeyGenerationParameters);

            return ecKeyPairGenerator.GenerateKeyPair();
        }

        public static ECPublicKeyParameters LoadPublicKey(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            var bytes = Convert.FromBase64String(publicKey);

            return PublicKeyFactory.CreateKey(bytes) as ECPublicKeyParameters;
        }

        public static string GenerateSignature(ECPrivateKeyParameters privateKey, byte[] bytes)
        {
            if (privateKey == null)
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(true, privateKey);

            signer.BlockUpdate(bytes, 0, bytes.Length);

            var result = signer.GenerateSignature();

            return result.ToBase64();
        }

        public static bool VerifySignature(ECPublicKeyParameters publicKey, byte[] bytes, string signature)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (string.IsNullOrEmpty(signature))
            {
                throw new ArgumentNullException(nameof(signature));
            }

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(false, publicKey);

            signer.BlockUpdate(bytes, 0, bytes.Length);

            var signatureBytes = signature.FromBase64();

            return signer.VerifySignature(signatureBytes);
        }
    }
}