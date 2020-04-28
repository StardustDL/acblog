using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Providers
{
    public interface IUserProvider : IRecordProvider<User, string>
    {
    }
}
