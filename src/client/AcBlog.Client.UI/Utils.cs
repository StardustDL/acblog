using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AcBlog.Client.UI
{
    public static class Utils
    {
        public static async Task UseToken(this IRepository repository, IAccessTokenProvider provider)
        {
            var tokenResult = await provider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                if (repository.Context is null)
                    repository.Context = new RepositoryAccessContext();
                repository.Context.Token = token.Value;
            }
        }

        public static string ToFriendlyString(this DateTimeOffset value)
        {
            TimeSpan tspan = DateTimeOffset.Now - value;
            StringBuilder sb = new StringBuilder();
            if (tspan.TotalDays > 60)
            {
                sb.Append(value.ToString("F"));
            }
            else if (tspan.TotalDays > 30)
            {
                sb.Append("1 month ago");
            }
            else if (tspan.TotalDays > 14)
            {
                sb.Append("2 weeks ago");
            }
            else if (tspan.TotalDays > 7)
            {
                sb.Append("1 week ago");
            }
            else if (tspan.TotalDays > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalDays)} days ago");
            }
            else if (tspan.TotalHours > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalHours)} hours ago");
            }
            else if (tspan.TotalMinutes > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalMinutes)} minutes ago");
            }
            else if (tspan.TotalSeconds > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalSeconds)} seconds ago");
            }
            else
            {
                sb.Append("just");
            }
            return sb.ToString();
        }

        public static string ToFriendlyString(this TimeSpan value)
        {
            StringBuilder sb = new StringBuilder();
            bool haspre = false;
            if (value.Days > 0)
            {
                sb.Append(string.Format("{0} d", value.Days));
                haspre = true;
            }
            if (value.Hours > 0)
            {
                if (haspre) sb.Append(" ");
                sb.Append(string.Format("{0} h", value.Hours));
                haspre = true;
            }
            if (value.Minutes > 0)
            {
                if (haspre) sb.Append(" ");
                sb.Append(string.Format("{0} min", value.Minutes));
                haspre = true;
            }
            if (value.Seconds > 0)
            {
                if (haspre) sb.Append(" ");
                sb.Append(string.Format("{0} s", value.Seconds));
                haspre = true;
            }
            if (value.Milliseconds > 0)
            {
                if (haspre) sb.Append(" ");
                sb.Append(string.Format("{0} ms", value.Milliseconds));
            }
            else
            {
                if (!haspre) sb.Append(string.Format("{0} ms", value.Milliseconds));
            }
            return sb.ToString();
        }

        public static string GetGravatarUrl(string email, uint size = 128, string @default = "mp")
        {
            static string ComputeHash(string input)
            {
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                byte[] inputArray = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashedArray = MD5.ComputeHash(inputArray);
                MD5.Clear();
                return BitConverter.ToString(hashedArray).Replace("-", "");
            }

            email = email.Trim().ToLower();
            string src = $"https://www.gravatar.com/avatar/{ComputeHash(email).ToLower()}?size={size}&d={@default}";
            return src;
        }
    }
}
