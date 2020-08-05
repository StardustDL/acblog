using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    public static class RepositoriyTester
    {
        public static async Task<RepositoryStatus> TestStatus<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            return await repository.GetStatus();
        }

        public static async Task<TId> TestCreate<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, T value) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Create(value);
            Assert.IsNotNull(result);

            var item = await repository.Get(result);
            item.WithDeepEqual(value).IgnoreSourceProperty(x => x.Id);

            return result;
        }

        public static async Task<IEnumerable<TId>> TestAll<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.All();
            Assert.IsNotNull(result);

            foreach (var v in result)
            {
                await repository.TestGet(v);
            }

            return result;
        }

        public static async Task TestGet<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, TId id) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Get(id);
            Assert.IsNotNull(result);
        }

        public static async Task TestDelete<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, TId id) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Delete(id);
            Assert.IsTrue(result);

            var item = await repository.Get(id);
            Assert.IsNull(item);
        }

        public static async Task TestUpdate<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, T value) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Update(value);
            Assert.IsTrue(result);

            var item = await repository.Get(value.Id);
            item.ShouldDeepEqual(value);
        }

        public static async Task TestCRUD<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, T value, T updated) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var status = await repository.TestStatus();
            Assert.IsTrue(status.CanRead);
            Assert.IsTrue(status.CanWrite);

            var id = await repository.TestCreate(value);

            await repository.TestGet(id);

            await repository.TestAll();

            updated.Id = id;
            await repository.TestUpdate(updated);

            await repository.TestDelete(id);
        }
    }
}
