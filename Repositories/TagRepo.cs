using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class TagRepo : ITagRepo
    {
        public List<Tag> GetTags() => TagDAO.Instance.GetTags();
        public Tag? GetTagById(short id) => TagDAO.Instance.GetTagById(id);
    }
}
