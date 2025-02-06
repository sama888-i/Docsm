namespace Docsm.Options
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

    }
}
