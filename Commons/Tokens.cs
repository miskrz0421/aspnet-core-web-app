using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings;
using System.Threading.Tasks;

namespace PiwkoMozna.Commons;
public static class Tokens{

        public static string GenerateToken()
        {
            byte[] tokenData = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenData);
            }
            return Convert.ToBase64String(tokenData);
        }
}