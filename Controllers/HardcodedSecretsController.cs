using InsecurelySharp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace InsecurelySharp.Controllers
{
    public class HardcodedSecretsController : Controller
    {
        private readonly ILogger<HardcodedSecretsController> _logger;
        private readonly IConfiguration _configuration;
        public HardcodedSecretsController(ILogger<HardcodedSecretsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exercise()
        {
            return View();
        }

        public IResult Encrypt([FromQuery] string plaintext) {
            var key = "S3CretSupER__111199693921passw@rdPASSWORD";

            // Create a new instance of the Aes class.
            using (Aes aes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                byte[] passwordBytes = System.Text.Encoding.Unicode.GetBytes(key);
                byte[] aesKey = SHA256.Create().ComputeHash(passwordBytes);
                byte[] aesIV = MD5.Create().ComputeHash(passwordBytes);

                aes.Key = aesKey;
                aes.IV = aesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt,
                                                                        encryptor,
                                                                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plaintext);
                        }

                        var encrypted = msEncrypt.ToArray();
                        return Results.Ok(new { ciphertext = encrypted });
                    }
                }
            }
        }

    public IResult EncryptSafe([FromQuery] string plaintext)
    {
        var key = _configuration.GetSection("CustomSettings").GetValue<string>("hardcodedsecrets_solution_key");

        // Create a new instance of the Aes class.
        using (Aes aes = Aes.Create())
        {

            // Encrypt the string to an array of bytes.
            byte[] passwordBytes = System.Text.Encoding.Unicode.GetBytes(key);
            byte[] aesKey = SHA256.Create().ComputeHash(passwordBytes);
            byte[] aesIV = MD5.Create().ComputeHash(passwordBytes);

            aes.Key = aesKey;
            aes.IV = aesIV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt,
                                                                    encryptor,
                                                                    CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plaintext);
                    }

                    var encrypted = msEncrypt.ToArray();
                    return Results.Ok(new { ciphertext = encrypted });
                }
            }
        }
    }

        public IActionResult Solution()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /*
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold the decrypted text.
            string plaintext = null;

            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key,
                                                                    aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                                                                     decryptor,
                                                                     CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }*/
    }
}
