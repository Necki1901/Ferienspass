using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ferienspaß {


    

    class Crypt {

        public static Password CreateNewPassword() {
            string pwdKlartext = RandomString(6);
            string salt = RandomString(16);
            string encodedPwd = GenerateSHA512String(pwdKlartext + salt);
            return new Password(pwdKlartext, encodedPwd, salt);
        }

        public static string CreateSalt() {
            return RandomString(16);
        }

        public static string GenerateSHA512String(string inputString) {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        private static RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        private static int RandomInteger(int min, int max) {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue) {
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }

        private static Random random = new Random();
        public static string RandomString(int length) {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[RandomInteger(0, s.Length)]).ToArray());
        }
    }

    class Password {
        public string Klartext { get; set; }
        public string EncodedPassword { get; set; }
        public string Salt { get; set; }

        public Password(string klartext, string encoded, string salt) {
            Klartext = klartext;
            EncodedPassword = encoded;
            Salt = salt;
        }
        public Password(string encoded) {
            EncodedPassword = encoded;
        }
    }
}
