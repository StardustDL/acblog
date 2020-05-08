using AcBlog.Data.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Protections
{
    public sealed class PostProtector : IProtector<Post>
    {
        readonly static SHA256Managed sha = new SHA256Managed();

        static byte[] FormalKey(byte[] raw)
        {
            var bs = sha.ComputeHash(raw);
            if (bs.Length >= 32)
                return bs[..32];
            else
            {
                byte[] res = new byte[32];
                bs.CopyTo(res, 0);
                return res;
            }
        }

        static byte[] AesEncrypt(byte[] bs, byte[] key)
        {
            byte[] toEncryptArray = bs;

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = FormalKey(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }

        static byte[] AesDecrypt(byte[] bs, byte[] key)
        {
            byte[] toEncryptArray = bs;

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = FormalKey(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return resultArray;
        }

        readonly static string ProtectFlag = Convert.ToBase64String(Encoding.UTF8.GetBytes("Protected Post by PostProtector"));

        public Task<bool> IsProtected(Post value)
        {
            return Task.FromResult(value.Title == ProtectFlag);
        }

        public async Task<Post> Protect(Post value, ProtectionKey key)
        {
            var res = new Post { Id = value.Id };
            byte[] bs;
            using (var ms = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(ms, value);
                bs = ms.ToArray();
            }
            var ky = Encoding.UTF8.GetBytes(key.Password);
            res.Title = ProtectFlag;
            res.Content = new Document
            {
                Raw = Convert.ToBase64String(AesEncrypt(bs, ky))
            };
            return res;
        }

        public async Task<Post> Deprotect(Post value, ProtectionKey key)
        {
            if (!await IsProtected(value))
            {
                return value;
            }
            var bs = Convert.FromBase64String(value.Content.Raw);
            var ky = Encoding.UTF8.GetBytes(key.Password);
            using (var ms = new MemoryStream(AesDecrypt(bs, ky)))
            {
                return await JsonSerializer.DeserializeAsync<Post>(ms);
            }
        }
    }
}
