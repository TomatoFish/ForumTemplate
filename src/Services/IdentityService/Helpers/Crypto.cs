using System.Security.Cryptography;

namespace IdentityService.Helpers;

public static class Crypto
{
    private const int SaltSize = 16;
    private const int KeySize = 256;
    private const int Iterations = 1000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ':';
    
    public static string Encrypt(string from)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(from, salt, Iterations, HashAlgorithmName, KeySize);
        var saltString = Convert.ToBase64String(salt);
        var hashString = Convert.ToBase64String(hash);
        var resultHash = string.Join(Delimiter, hashString, saltString);

        return resultHash;
    }

    public static bool Compare(string comparable, string reference)
    {
        var referenceParts = reference.Split(Delimiter);
        var referenceHash = Convert.FromBase64String(referenceParts[0]);
        var referenceSalt = Convert.FromBase64String(referenceParts[1]);
        var comparableHash = Rfc2898DeriveBytes.Pbkdf2(comparable, referenceSalt, Iterations, HashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(referenceHash, comparableHash);
    }
}