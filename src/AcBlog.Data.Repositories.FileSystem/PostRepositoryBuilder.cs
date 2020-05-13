using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using System.Security.Cryptography;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class PostRepositoryBuilder : BaseRepositoryBuilder<PostBuildData>
    {
        public PostRepositoryBuilder(IList<PostBuildData> data, DirectoryInfo dist) : base(data, dist)
        {
        }

        public IProtector<Post>? Protector { get; set; }

        public IList<Keyword> Keywords { get; private set; } = Array.Empty<Keyword>();

        public IList<Category> Categories { get; private set; } = Array.Empty<Category>();

        async Task BuildIndexType()
        {
            await BuildPaging(Data.Where(x => x.Raw.Type == PostType.Article).ToList(),
                Dist.CreateSubdirectory("articles"));

            await BuildPaging(Data.Where(x => x.Raw.Type == PostType.Slides).ToList(),
                Dist.CreateSubdirectory("slides"));

            await BuildPaging(Data.Where(x => x.Raw.Type == PostType.Note).ToList(),
                Dist.CreateSubdirectory("notes"));
        }

        async Task BuildIndexKeyword()
        {
            var kwroot = Dist.CreateSubdirectory("keywords");
            Dictionary<string, List<PostBuildData>> dict = new Dictionary<string, List<PostBuildData>>();
            Dictionary<string, string> mapper = new Dictionary<string, string>();
            foreach (var v in Data)
            {
                List<string> keyids = new List<string>();
                foreach (var k in v.Keywords)
                {
                    if (!mapper.ContainsKey(k))
                    {
                        string id = Guid.NewGuid().ToString();
                        mapper.Add(k, id);
                        var list = new List<PostBuildData>
                        {
                            v
                        };
                        dict.Add(id, list);
                        keyids.Add(id);
                    }
                    else
                    {
                        string id = mapper[k];
                        dict[id].Add(v);
                        keyids.Add(id);
                    }
                }
                v.Raw.KeywordIds = keyids.ToArray();
            }
            foreach (var v in dict)
            {
                await BuildPaging(v.Value, kwroot.CreateSubdirectory(v.Key));
            }
            Keywords = mapper.Select(x => new Keyword
            {
                Id = x.Value,
                Name = x.Key
            }).ToList();
        }

        private class CategoryNode
        {
            public CategoryNode(Category category)
            {
                Category = category;
            }

            public Category Category { get; set; }

            public IList<PostBuildData> Data { get; } = new List<PostBuildData>();

            public Dictionary<string, CategoryNode> Children { get; } = new Dictionary<string, CategoryNode>();
        }

        async Task BuildIndexCategory()
        {
            var kwroot = Dist.CreateSubdirectory("categories");
            Dictionary<string, CategoryNode> mapper = new Dictionary<string, CategoryNode>();
            CategoryNode all = new CategoryNode(new Category());
            foreach (var v in Data)
            {
                CategoryNode node = all;
                foreach (var k in v.Category)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        Category c = new Category
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = k,
                        };
                        if (node != all)
                        {
                            c.ParentId = node.Category.Id;
                        }
                        var tn = new CategoryNode(c);
                        mapper.Add(c.Id, tn);
                        node.Children.Add(k, tn);
                        node = tn;
                    }
                    else
                    {
                        node = node.Children[k];
                    }
                    node.Data.Add(v);
                }
                v.Raw.CategoryId = node.Category.Id;
            }
            foreach (var v in mapper)
            {
                await BuildPaging(v.Value.Data, kwroot.CreateSubdirectory(v.Key));
            }
            Categories = mapper.Select(x => x.Value.Category).ToList();
        }

        public override async Task Build()
        {
            Data = (from x in Data orderby x.Raw.CreationTime descending select x).ToArray();

            await BuildIndexType();

            await BuildIndexKeyword();

            await BuildIndexCategory();

            await base.Build();
        }

        protected override async Task SaveItem(PostBuildData item)
        {
            Post post = item.Raw;
            if (item.Key != null && Protector != null)
                post = await Protector.Protect(item.Raw, item.Key);
            await BaseRepositoryBuilder.SaveToFile(Path.Join(Dist.FullName, $"{item.Raw.Id}.json"), post);
        }

        protected override string GetId(PostBuildData item) => item.Raw.Id;
    }
}
