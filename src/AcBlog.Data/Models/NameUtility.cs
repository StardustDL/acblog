using System.Web;

namespace AcBlog.Data.Models
{
    public static class NameUtility
    {
        public static string Encode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string Decode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
    }
}
