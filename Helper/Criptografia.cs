using System.Security.Cryptography;
using System.Text;

namespace MercadoSocial.Helper
{
    public static class Criptografia
    {
        public static string GenerateHash(this string value)
        {
            var hash = SHA1.Create();
            var array = new ASCIIEncoding().GetBytes(value);

            array = hash.ComputeHash(array);
            var strHexa = new StringBuilder();

            foreach(var item in array)
            {
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();
        }


    }
}
