namespace SharedLibrary.TokenUtilities
{
    public interface ITokenUtilities
    {
        string HashPassword(string password);
        string CreateJwtFromDictionary(IDictionary<string, string> data);
        public string CreateBase64RefreshToken(string id);
        Dictionary<string, string> GetDataDictionaryFromJwt(string token);
        bool ValidateJwt(string token);
        public string? ValidateBase64RefreshToken(string token);
    }
}
