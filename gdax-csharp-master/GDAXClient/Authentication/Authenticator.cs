using GDAXClient.Services.Accounts;

namespace GDAXClient.Authentication
{
    public class Authenticator : IAuthenticator
    {
        public Authenticator(
            string apiKey,
            string unsignedSignature,
            string passphrase)
        {
            ApiKey = apiKey;
            UnsignedSignature = unsignedSignature;
            Passphrase = passphrase;
        }

        public string ApiKey { get; set; }

        public string UnsignedSignature { get; set; }

        public string Passphrase { get; set; }
    }
}
