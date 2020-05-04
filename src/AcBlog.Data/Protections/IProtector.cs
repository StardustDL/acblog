using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Protections
{
    public interface IProtector<T>
    {
        public Task<bool> IsProtected(T value);

        public Task<T> Protect(T value, ProtectionKey key);

        public Task<T> Deprotect(T value, ProtectionKey key);
    }
}
