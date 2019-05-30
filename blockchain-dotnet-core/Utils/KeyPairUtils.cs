using System.Collections.Generic;
using System.Text;
using blockchain_dotnet_core.API.Models;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
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

        public static string GenerateSignature(ECPrivateKeyParameters ecPrivateKeyParameters, TransactionOutput transactionOutput)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(true, ecPrivateKeyParameters);

            signer.BlockUpdate(Encoding.ASCII.GetBytes(transactionOutput.ToString()), 0, transactionOutput.ToString().Length);

            var result = signer.GenerateSignature();

            return HexUtils.ToHex(result);
        }

        public static bool VerifySignature(ECPublicKeyParameters publicKey, Transaction transaction, string signature)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(false, publicKey);

            signer.BlockUpdate(Encoding.ASCII.GetBytes(transaction.ToString()), 0, transaction.ToString().Length);

            var signatureBytes = HexUtils.FromHex(signature);

            return signer.VerifySignature(signatureBytes);
        }
    }
}
