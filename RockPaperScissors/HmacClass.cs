using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;



namespace RockPaperScissors
{
    class HmacClass
    {
        private static RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
        public static int GetRandomNum(int minValue, int maxValue)
        {
            byte[] randombyte = new byte[1];
            rnd.GetBytes(randombyte);
            double random_multiplyer = (randombyte[0] / 255d);
            int difference = maxValue - minValue + 1;
            int result = (int)(minValue + Math.Floor(random_multiplyer * difference));
            return result;
        }

        public static byte[] GetKey()
        {
            byte[] key = new byte[128];
            rnd.GetBytes(key);
            return key;

        }
        public static string GetHmac(byte[] key, string message)
        {
            HMACSHA256 hmac = new HMACSHA256(key);
            byte[] mes = Encoding.UTF8.GetBytes(message);
            byte[] hashValue = hmac.ComputeHash(mes);

            string hashString = BitConverter.ToString(hashValue);
            hashString = hashString.Replace("-", "").ToLowerInvariant();
            return hashString;
        }

    }
}
