using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using whatsapp.dotnet;

namespace whatsapp.dotnet
{
    public static class Help
    { 
        public static string Descript(string strInput)
        {
            try
            {
                var keyString = "5A6365D620864B9CB2FCC04979A4B815"; //replace with your key
                var ivString = "A3ECD63F16654D21"; //replace with your iv

                var key = Encoding.ASCII.GetBytes(keyString);
                var iv = Encoding.ASCII.GetBytes(ivString);

                using (var rijndaelManaged =
                    new RijndaelManaged {Key = key, IV = iv, Mode = CipherMode.CBC})
                {
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.KeySize = 256;
                    using (var memoryStream =
                        new MemoryStream(System.Convert.FromBase64String(strInput)))
                    using (var cryptoStream =
                        new CryptoStream(memoryStream,
                            rijndaelManaged.CreateDecryptor(key, iv),
                            CryptoStreamMode.Read))
                    {
                        return new StreamReader(cryptoStream).ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            { 
                return string.Empty;
            }
        }

        public static MessageResume Convert(Message message)
        {
            return new MessageResume
            {
                Id = message.objectId,
                AsFrom = message.from,
                AsTo = message.to,
                FromMe = message.fromMe,
                Type = message.type,
                HasMedia = message.hasMedia,
                CreatedIn = UnixTimeStampToDateTime(message.timestamp.ToDouble(0)),
                Body = removeInvalid(message.body),
                ChatFk = message.from
            };
        }

        public static double ToDouble(this object self, double defaultvalue)
        {
            var s = System.Convert.ToString(self);
            if (string.IsNullOrWhiteSpace(s))
                return defaultvalue;
            var result = 0.0;
            return !double.TryParse(s, out result) ? defaultvalue : result;
        }
        public static string removeInvalid(string caracter)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            caracter = r.Replace(caracter, "");
            return caracter;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}