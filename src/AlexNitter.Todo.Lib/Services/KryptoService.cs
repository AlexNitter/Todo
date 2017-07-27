using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AlexNitter.Todo.Lib.Services
{
    public class KryptoService
    {
        /// <summary>
        /// Generiert einen kryptografisch sicheren Zufallswert
        /// </summary>
        /// <param name="bitCount">Stärke des Zufallswerts in Bit</param>
        public String GenerateSalt(Int32 bitCount)
        {
            var salt = "";

            using (var generator = RandomNumberGenerator.Create())
            {
                var bytes = new Byte[bitCount / 8];
                generator.GetNonZeroBytes(bytes);

                var formatter = new ToBase64Transform();
                salt = Convert.ToBase64String(bytes);
            }

            return salt;
        }

        /// <summary>
        /// Erzeugt einen kryptografisch sicheren Hash anhand des SHA256-Alogirhtmus bestehend aus salt + valueToHash
        /// </summary>
        /// <param name="valueToHash">Zu hashender Wert</param>
        /// <param name="salt">Salt</param>
        public String Hash(String valueToHash, String salt)
        {
            var hash = "";

            using (var hasher = new SHA256Managed())
            {
                var hashBytes = hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(salt + valueToHash));
                hash = Convert.ToBase64String(hashBytes);
            }

            return hash;
        }
    }
}
