using System;
using System.Buffers.Text;
using System.Text;
using System.Web;

namespace AcBlog.Data.Models
{
    public static class NameUtility
    {
        public static string Encode(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str)).Replace('+', '-').Replace('/', '_');
        }

        public static string Decode(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str.Replace('-', '+').Replace('_', '/')));
        }
    }
}
