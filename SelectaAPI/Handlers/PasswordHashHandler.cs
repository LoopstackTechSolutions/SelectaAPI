using System.Security.Cryptography;
using System.Text;

namespace SelectaAPI.Handlers
{
    public class PasswordHashHandler
    {
        // Parâmetros de segurança
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 10000; // número de iterações PBKDF2

        /// <summary>
        /// Gera o hash da senha com salt embutido.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    var key = pbkdf2.GetBytes(KeySize);
                    var hash = new byte[SaltSize + KeySize];
                    Buffer.BlockCopy(salt, 0, hash, 0, SaltSize);
                    Buffer.BlockCopy(key, 0, hash, SaltSize, KeySize);

                    // Retorna em Base64 para salvar no banco
                    return Convert.ToBase64String(hash);
                }
            }
        }

        /// <summary>
        /// Verifica se a senha corresponde ao hash armazenado.
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // Extrair o salt
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            // Extrair o hash original
            var originalHash = new byte[KeySize];
            Buffer.BlockCopy(hashBytes, SaltSize, originalHash, 0, KeySize);

            // Recalcular o hash com a senha informada
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var testHash = pbkdf2.GetBytes(KeySize);

                return CryptographicOperations.FixedTimeEquals(testHash, originalHash);
            }
        }
    }
}
