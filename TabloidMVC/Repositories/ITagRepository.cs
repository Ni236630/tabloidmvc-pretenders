using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
        List<Tag> GetPostTags(int postId);

        Tag GetTagById(int id);

        void AddTag(Tag tag);

        void EditTag(Tag tag);

        void DeleteTag(Tag tag);
        void DeleteAllPostTags(int postId);
        void AddPostTags(int tagId, int postId);
    }
}
