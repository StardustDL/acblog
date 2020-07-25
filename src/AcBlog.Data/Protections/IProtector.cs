using AcBlog.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Protections
{
    public interface IProtector<T>
    {
        public Task<bool> IsProtected(T value, CancellationToken cancellationToken = default);

        public Task<T> Protect(T value, ProtectionKey key, CancellationToken cancellationToken = default);

        public Task<T> Deprotect(T value, ProtectionKey key, CancellationToken cancellationToken = default);
    }
}
