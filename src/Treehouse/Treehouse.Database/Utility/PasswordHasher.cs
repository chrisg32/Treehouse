using System.Security.Cryptography;
using System.Text;

namespace TreeHouse.Database.Utility
{
    public static class PasswordHasher
    {
        public static string HashPass(string text)
        {
            var clearBytes = Encoding.Default.GetBytes(text);
            var hashedBytes = SHA1.Create().ComputeHash(clearBytes);
            return Encoding.Unicode.GetString(hashedBytes);
        }
    }
}
