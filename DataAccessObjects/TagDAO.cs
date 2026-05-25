using BusinessObjects;
using DataAccessObjects.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class TagDAO
    {
        private static TagDAO? instance = null;
        private static readonly object instanceLock = new object();

        private TagDAO() { }

        public static TagDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TagDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Tag> GetTags()
        {
            using var context = new FunewsManagementContext();
            return context.Tags.ToList();
        }

        public Tag? GetTagById(short id)
        {
            using var context = new FunewsManagementContext();
            return context.Tags.SingleOrDefault(t => t.TagId == id);
        }
    }
}