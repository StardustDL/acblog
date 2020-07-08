using AcBlog.Data.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AcBlog.Tools.Sdk.Helpers
{

    internal class Equality
    {
        [DoesNotReturn]
        void ThrowNeq()
        {
            throw new Exception();
        }

        public void Add<T>(T a, T b)
        {
            if (a is null && b is null)
            {
                return;
            }
            if (a is null || b is null)
                ThrowNeq();
            if (!a.Equals(b))
                ThrowNeq();
        }

        public void Add<T>(T[] a, T[] b)
        {
            if (a.Length != b.Length)
                ThrowNeq();
            for (int i = 0; i < a.Length; i++)
                Add(a[i], b[i]);
        }
    }
}
