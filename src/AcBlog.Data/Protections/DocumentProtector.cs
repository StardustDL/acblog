using AcBlog.Data.Documents;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Protections
{
    public sealed class DocumentProtector : IProtector<Document>
    {
        /*static readonly SHA256Managed _sha = new SHA256Managed();

        static byte[] FormalKey(byte[] raw)
        {
            var bs = _sha.ComputeHash(raw);
            if (bs.Length >= 32)
            {
                return bs[..32];
            }
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
        }*/

        static readonly string _protectFlag = Convert.ToBase64String(Encoding.UTF8.GetBytes("Protected Post by PostProtector"));

        public Task<bool> IsProtected(Document value, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(value.Tag == _protectFlag);
        }

        public async Task<Document> Protect(Document value, ProtectionKey key, CancellationToken cancellationToken = default)
        {
            try
            {
                /*var res = new Document();
                byte[] bs;
                await using (var ms = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(ms, value, cancellationToken: cancellationToken);
                    bs = ms.ToArray();
                }
                var ky = Encoding.UTF8.GetBytes(key.Password);
                res.Tag = _protectFlag;
                res.Raw = Convert.ToBase64String(AesEncrypt(bs, ky));
                return res;*/
                return value;
            }
            catch (Exception ex)
            {
                throw new ProtectionException("Protect failed.", ex);
            }
        }

        public async Task<Document> Deprotect(Document value, ProtectionKey key, CancellationToken cancellationToken = default)
        {
            try
            {
                /*if (!await IsProtected(value, cancellationToken))
                {
                    return value;
                }
                var bs = Convert.FromBase64String(value.Raw);
                var ky = Encoding.UTF8.GetBytes(key.Password);
                await using var ms = new MemoryStream(AesDecrypt(bs, ky));
                return await JsonSerializer.DeserializeAsync<Document>(ms, cancellationToken: cancellationToken)
                    ?? throw new NullReferenceException("Null");*/
                return value;
            }
            catch (Exception ex)
            {
                throw new ProtectionException("Deprotect failed.", ex);
            }
        }
    }
}
