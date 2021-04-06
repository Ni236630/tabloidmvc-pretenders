using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public List<Comment> GetAllCommentsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime, up.DisplayName AS CommentAuthor
                                            FROM Comment c
                                            JOIN Post p ON p.id = c.PostId
                                            JOIN UserProfile up ON c.UserProfileId = up.Id
                                            WHERE c.PostId = @id
                                        ORDER BY CreateDateTime ASC";
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        comments.Add(new Comment()
                        {

                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            CommentAuthor = reader.GetString(reader.GetOrdinal("CommentAuthor"))
                        });
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public void AddComment(CommentViewModel comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                                        OUTPUT INSERTED.Id
                                        VALUES (@postId, @userProfileId, @subject, @content, @createDateTime )";
                    cmd.Parameters.AddWithValue("@postId", comment.post.Id);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.user.Id);
                    cmd.Parameters.AddWithValue("@subject", comment.comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.comment.CreateDateTime);


                    int id = (int)cmd.ExecuteScalar();

                    comment.comment.Id = id;
                }
            }
        }

        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, PostId, UserProfileId, Subject, Content, CreateDateTime 
                                        FROM Comment WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    if (reader.Read())
                    {
                        comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))

                        };
                    }

                    reader.Close();

                    return comment;
                }
            }
        }

        public void EditComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                UPDATE Comment
                                    SET
                                        PostId = @postId,
                                        UserProfileId = @userProfileId,
                                        Subject = @subject,
                                        Content = @content,
                                        CreateDateTime = @createDateTime
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void DeleteComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Comment WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", comment.Id);
                    cmd.ExecuteNonQuery();

                }
            }
        }





    }
}
