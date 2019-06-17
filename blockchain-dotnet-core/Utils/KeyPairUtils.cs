using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace blockchain_dotnet_core.API.Utils
{
    public static class KeyPairUtils
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
            var bytes = Convert.FromBase64String(publicKey);

            return PublicKeyFactory.CreateKey(bytes) as ECPublicKeyParameters;
        }

        public static string GenerateSignature(ECPrivateKeyParameters ecPrivateKeyParameters, byte[] transactionOutputs)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(true, ecPrivateKeyParameters);

            signer.BlockUpdate(transactionOutputs, 0, transactionOutputs.Length);

            var result = signer.GenerateSignature();

            return HexUtils.BytesToString(result);
        }

        public static bool VerifySignature(ECPublicKeyParameters publicKey, byte[] transactionOutputs, string signature)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(false, publicKey);

            signer.BlockUpdate(transactionOutputs, 0, transactionOutputs.Length);

            var signatureBytes = HexUtils.StringToBytes(signature);

            return signer.VerifySignature(signatureBytes);
        }
    }
}