using TextCopy;
using System.Security.Cryptography;
using Cocona;
namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CoconaLiteApp.Run((
                [Argument(Description = "Length of the key in bytes")] int length,
                [Option('f', Description = "type of output (base64, hex)")] string? format,
                [Option('c', Description = "Copy result to clipboard")] bool toClipboard) =>
            {
                if (string.IsNullOrEmpty(format)) format = "base64";
                string result = GenerateKey(length, format);
                System.Console.WriteLine(result);
                if (toClipboard)
                {
                    ClipboardService.SetText(result);
                }
            });
        }

        static string GenerateKey(int length, string type)
        {
            // Generate a random key
            byte[] key = new byte[length];
            try
            {
                RandomNumberGenerator.Create().GetBytes(key);

                // Convert the key to the desired format
                string result = type switch
                {
                    "base64" => System.Convert.ToBase64String(key),
                    "hex" => System.BitConverter.ToString(key).Replace("-", " "),
                    _ => throw new System.ArgumentException($"Invalid type: {type}", nameof(type))
                };
                return result;
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine($"Invalid type: {type}. Accepted types are base64 and hex.");
            }
            catch (System.Exception)
            {
                throw new System.Exception("Failed to generate a random key");
            }
            return "";
        }
    }
}