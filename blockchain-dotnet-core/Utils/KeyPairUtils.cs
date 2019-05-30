using System.Collections.Generic;
using System.Text;
using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

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

        public static string GenerateSignature(ECPrivateKeyParameters ecPrivateKeyParameters, List<Transaction> transactions)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");

            var ecDomainParameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(true, ecPrivateKeyParameters);

            signer.BlockUpdate(Encoding.ASCII.GetBytes(transactions.ToString()), 0, transactions.ToString().Length);

            var result = signer.GenerateSignature();

            return HexUtils.ToHex(result);
        }

        public static bool VerifySignature(string publicKey, List<Transaction> transactions, string signature)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");

            var ecDomainParameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            var publicKeyBytes = HexUtils.FromHex(publicKey);

            var q = curve.Curve.DecodePoint(publicKeyBytes);

            var ecPublicKeyParameters = new ECPublicKeyParameters(q, ecDomainParameters);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(false, ecPublicKeyParameters);

            signer.BlockUpdate(Encoding.ASCII.GetBytes(transactions.ToString()), 0, transactions.ToString().Length);

            var signatureBytes = HexUtils.FromHex(signature);

            return signer.VerifySignature(signatureBytes);
        }
    }
}
