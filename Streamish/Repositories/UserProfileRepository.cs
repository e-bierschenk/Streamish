using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.Name, up.Email, up.ImageUrl, up.DateCreated
                        FROM UserProfile up";

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            UserProfile user = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            };
                            users.Add(user);
                        }
                        return users;
                    }
                }
            }
        }

        public UserProfile GetByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.Name, up.Email, up.ImageUrl, up.DateCreated,
                                v.Id AS VideoId, v.Title, v.Description, v.Url, v.DateCreated AS VideoCreatedDate, v.UserProfileId,
                                c.Id AS CommentId, c.Message, c.VideoId AS CommentVideo, c.UserProfileId AS CommentUser
                        FROM UserProfile up
                        LEFT JOIN Video v ON up.Id = v.UserProfileId
                        LEFT JOIN Comment c ON v.Id = c.VideoId
                        WHERE up.Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile user = null;
                        while (reader.Read())
                        {
                            if (user == null)
                            {
                                user = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }

                            
                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                int currentVideoId = DbUtils.GetInt(reader, "VideoId");
                                Video video = new Video()
                                {
                                    Id = DbUtils.GetInt(reader, "VideoId"),
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    Url = DbUtils.GetString(reader, "Url"),
                                    DateCreated = DbUtils.GetDateTime(reader, "VideoCreatedDate"),
                                    UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                    Comments = new List<Comment>()
                                };
                                if(user.Videos.FirstOrDefault(p => p.Id == currentVideoId) == null)
                                {
                                    user.Videos.Add(video);
                                }
                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    Comment comment = new Comment()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        VideoId = DbUtils.GetInt(reader, "CommentVideo"),
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUser")
                                    };
                                    
                                    user.Videos.FirstOrDefault(p => p.Id == currentVideoId).Comments.Add(comment);
                                }
                            }
                        }
                        return user;
                    }
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (Name, Email, DateCreated, ImageUrl)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @email, @date, @img)";
                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@date", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@img", userProfile.ImageUrl);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET Name = @name,
                            Email = @email,
                            DateCreated = @date,
                            ImageUrl = @img
                        WHERE Id = @id
                        ";
                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@date", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@img", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM UserProfile WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
