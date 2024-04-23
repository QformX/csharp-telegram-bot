using System.Security.Cryptography;

public class TokenGenerator
{
    internal string GetToken(string path)
    {
        string decrypted;
        using (Aes aes = Aes.Create())
        {
            byte[] salt = new byte[16];
            Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes("password", salt);
            aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
            aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

            byte[] encryptedFromFile;
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                salt = reader.ReadBytes(16);
                encryptedFromFile = reader.ReadBytes((int)reader.BaseStream.Length - 16);
            }

            Rfc2898DeriveBytes decryptionKey = new Rfc2898DeriveBytes("password", salt);
            aes.Key = decryptionKey.GetBytes(aes.KeySize / 8);
            aes.IV = decryptionKey.GetBytes(aes.BlockSize / 8);
            using (MemoryStream ms = new MemoryStream(encryptedFromFile))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        decrypted = reader.ReadToEnd();
                    }
                }
            }
        }
        return decrypted;
    }
}