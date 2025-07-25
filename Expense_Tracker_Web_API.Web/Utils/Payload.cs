using System.Security.Cryptography;
using System.Text;

namespace Expense_Tracker_Web_API.Web.Utils;

public class Payload
{
    public string DecryptPayload(string encryptedPayload, string secretKey)
    {
        byte[] fullCipher = Convert.FromBase64String(encryptedPayload);

        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - 16];

        Array.Copy(fullCipher, 0, iv, 0, 16);
        Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey));

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(decrypted);
    }

}
