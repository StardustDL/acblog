using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.SDK
{
    public interface IUserService : IUserRepository
    {
        IBlogService Blog { get; }
    }
}
