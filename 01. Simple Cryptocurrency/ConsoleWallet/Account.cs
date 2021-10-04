namespace ConsoleWallet
{
    using System;
    using System.Numerics;
    using System.Security.Cryptography;
    using Coin.Utils;
    using EllipticCurve;

    public class Account
    {
        private BigInteger secretNumber;

        private PrivateKey privateKey;

        private PublicKey publicKey;

        public Account()
            : this(string.Empty)
        {
        }

        public Account(string input)
        {
            this.Initialize(input);
        }

        public BigInteger SecretNumber => this.secretNumber;

        public string PublicKey => Convert.ToHexString(this.publicKey.toString());

        public string Address => SHA256.Create().ComputeHash(this.publicKey.toString()).ConvertToHexString();

        public string CreateSignature(string message) => Ecdsa.sign(message, this.privateKey).toBase64();

        private void Initialize(string input)
        {
            var success = BigInteger.TryParse(input, out var secret);

            if (success)
            {
                this.privateKey = new PrivateKey("secp256k1", secret);
            }
            else
            {
                this.privateKey = new PrivateKey();
            }

            this.secretNumber = this.privateKey.secret;
            this.publicKey = this.privateKey.publicKey();
        }
    }
}
