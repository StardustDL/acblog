using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    public static class RepositoriyTester
    {
        public static async Task<RepositoryStatus> Status<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            return await repository.GetStatus();
        }

        public static async Task<TId> Create<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository, T value) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Create(value);
            Assert.IsNotNull(result);

            var item = await repository.Get(result);
            item.WithDeepEqual(value).IgnoreSourceProperty(x => x.Id);

            return result;
        }

        public static async Task All<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.All();
            Assert.IsNotNull(result);

            foreach (var v in result)
            {
                await Get(repository, v);
            }
        }

        public static async Task Get<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository, TId id) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Get(id);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result);
        }

        public static async Task Delete<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository, TId id) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Delete(id);
            Assert.IsTrue(result);

            var item = await repository.Get(id);
            Assert.IsNull(item);
        }

        public static async Task Update<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository, T value) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var result = await repository.Update(value);
            Assert.IsTrue(result);

            var item = await repository.Get(value.Id);
            item.ShouldDeepEqual(value);
        }

        public static async Task CRUD<T, TId, TQuery>(IRecordRepository<T, TId, TQuery> repository, T value, T updated) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            var status = await Status(repository);
            Assert.IsTrue(status.CanRead);
            Assert.IsTrue(status.CanWrite);

            var id = await Create(repository, value);

            await Get(repository, id);

            await All(repository);

            updated.Id = id;
            await Update(repository, updated);

            await Delete(repository, id);
        }
    }
}
