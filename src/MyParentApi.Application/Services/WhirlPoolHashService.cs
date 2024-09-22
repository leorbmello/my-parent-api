using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
using System.Text;

namespace MyParentApi.Application.Services
{
    internal class WhirlPoolHashService
    {
        /// <summary>
        ///     Validates the user's inputted password, which has been decrypted from the client
        ///     request decode method, and is ready to be hashed and compared with the SHA-1
        ///     hash in the database.
        /// </summary>
        /// <param name="input">Inputted password from the client's request</param>
        /// <param name="hash">Hashed password in the database</param>
        /// <param name="salt">Salt for the hashed password in the databse</param>
        /// <returns>Returns true if the password is correct.</returns>
        public static PasswordVerificationResult CheckPassword(string input, string hash, string salt)
        {
            int version = int.Parse(hash[..2]);
            return HashPassword(input, salt, version).Equals(hash)
                       ? version >= 0 && version <= 2 ? PasswordVerificationResult.Success :
                                                        PasswordVerificationResult.SuccessRehashNeeded
                       : PasswordVerificationResult.Failed;
        }

        public static string WhirlpoolHashOnce(string message)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
                return string.Empty;

            WhirlpoolDigest whirlpool = new();

            byte[] data = new UTF8Encoding().GetBytes(message);
            whirlpool.Reset();
            whirlpool.BlockUpdate(data, 0, data.Length);

            var ret = new byte[whirlpool.GetDigestSize()];
            whirlpool.DoFinal(ret, 0);
            return ByteToString(ret);
        }

        public static string HashPassword(string password, string salt, int version = 0)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                return string.Empty;

            string result = "";
            for (var i = 0; i < 15000; i++)
            {
                result = WhirlpoolHashOnce($"{salt}{password}{salt}");
            }

            result = $"{version:00}{result}";
            return result;
        }

        public static string GenerateSalt()
        {
            const string UPPER_S = "QWERTYUIOPASDFGHJKLZXCVBNM";
            const string LOWER_S = "qwertyuioplkjhgfdsazxcvbnm";
            const string NUMBER_S = "1236547890";
            const string POOL_S = UPPER_S + LOWER_S + NUMBER_S;
            const int SIZE_I = 64;

            var output = "";
            for (var i = 0; i < SIZE_I; i++)
                output += POOL_S[RandomNumberGenerator.GetInt32(POOL_S.Length) % POOL_S.Length];

            output = ByteToString(Encoding.UTF8.GetBytes(output));

            return output;
        }

        private static string ByteToString(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }
    }
}
