using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

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
                    cmd.CommandText = @"Select c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime
                                            FROM Comment c
                                            JOIN Post p ON p.id = c.PostId
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
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))

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
