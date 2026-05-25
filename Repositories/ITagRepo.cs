using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface ITagRepo
    {
        List<Tag> GetTags();
        Tag? GetTagById(short id);
    }
}
