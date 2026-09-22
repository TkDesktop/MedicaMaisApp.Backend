using System.Security.Cryptography;

namespace MedicaMaisApp.Backend.Services
{
    // Gera e valida hash de senha usando PBKDF2 (nativo do .NET, sem depender de pacotes externos).
    // Nunca guardamos a senha em texto puro no banco.
    public static class SenhaHasher
    {
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;
        private const int Iteracoes = 100_000;

        public static (string hash, string salt) Gerar(string senha)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(TamanhoSalt);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                saltBytes,
                Iteracoes,
                HashAlgorithmName.SHA256,
                TamanhoHash);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        public static bool Validar(string senha, string hashArmazenado, string saltArmazenado)
        {
            byte[] saltBytes = Convert.FromBase64String(saltArmazenado);

            byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                saltBytes,
                Iteracoes,
                HashAlgorithmName.SHA256,
                TamanhoHash);

            byte[] hashArmazenadoBytes = Convert.FromBase64String(hashArmazenado);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashArmazenadoBytes);
        }
    }
}
