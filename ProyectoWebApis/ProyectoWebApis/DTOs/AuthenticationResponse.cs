namespace ProyectoWebApis.DTOs
{
    public class AuthenticationResponse
    {
        public string Token { get; internal set; }
        public DateTime Expiration { get; internal set; }
    }
}
