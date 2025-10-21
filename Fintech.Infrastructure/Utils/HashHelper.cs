using System.Security.Cryptography;
using System.Text;

namespace Fintech.Infrastructure.Utils;

public static class HashHelper
{
    public static string ComputeSha256(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static async Task<string> ComputeSha256Async(Stream stream)
    {
        if (stream == null || stream.Length == 0)
            return string.Empty;

        using var sha256 = SHA256.Create();
        byte[] hash = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
