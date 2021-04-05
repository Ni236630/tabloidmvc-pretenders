using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetAllCommentsByPostId(int id);

        void AddComment(CommentViewModel comment);

        Comment GetCommentById(int id);

        void EditComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
